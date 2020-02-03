using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using MD.Equipos.GPS.Herramientas;


namespace MD.Equipos.GPS.Garming
{
    public delegate void delegateOutputStatus(Boolean connected);
    public delegate void delegateOutputPVTData(pvt_data_type pvt_data);
    public delegate void delegateOutputSatData(sat_data_type[] sat_data_array);

    public class GarminUSBDevice
    {
        delegateOutputStatus OutputStatus;
        delegateOutputPVTData OutputPVTData;
        delegateOutputSatData OutputSatData;

        public const int MAX_BUFFER_SIZE = 4096;

        static Thread GPSthrd;
        static Boolean GPSExitThread = false;

        static Guid garminUSB = new Guid("2c9c45c28e7d4c08a12d816bbae722c0");
        static public string productDescription = "None";

        static IntPtr gHandle = IntPtr.Zero;
        static IntPtr theDevInfo = IntPtr.Zero;

        static int gUSBPacketSize = 0;

        static GCHandle gcUSBPS = GCHandle.Alloc(gUSBPacketSize, GCHandleType.Pinned);
        static IntPtr gcIntPtr = gcUSBPS.AddrOfPinnedObject();

        static SP_DEVICE_INTERFACE_DATA theInterfaceData = new SP_DEVICE_INTERFACE_DATA();
        static SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();

        static IntPtr mDataPtr = Marshal.AllocHGlobal(MAX_BUFFER_SIZE);
        static byte[] mDataBuffer = new byte[MAX_BUFFER_SIZE];

        public const int GARMIN_LAYERID_TRANSPORT = 0;
        public const int GARMIN_LAYERID_APPL = 20;

        // Linux Garmin Usb driver layer-id to use for some control mechanisms
        public const int GARMIN_LAYERID_PRIVATE = 0x01106E4B;

        // packet ids used in private layer
        public const int PRIV_PKTID_SET_DEBUG = 1;
        public const int PRIV_PKTID_SET_MODE = 2;
        public const int PRIV_PKTID_INFO_REQ = 3;
        public const int PRIV_PKTID_INFO_RESP = 4;
        public const int PRIV_PKTID_RESET_REQ = 5;
        public const int PRIV_PKTID_SET_DEF_MODE = 6;

        public const int MODE_NATIVE = 0;
        public const int MODE_GARMIN_SERIAL = 1;

        public const int GARMIN_PKTID_TRANSPORT_START_SESSION_REQ = 5;
        public const int GARMIN_PKTID_TRANSPORT_START_SESSION_RESP = 6;

        public const int GARMIN_PKTID_PROTOCOL_ARRAY = 253;
        public const int GARMIN_PKTID_PRODUCT_RQST = 254;
        public const int GARMIN_PKTID_PRODUCT_DATA = 255;
        public const int GARMIN_PKTID_EXT_PRODUCT_DATA = 248;
        public const int GARMIN_PKTID_PVT_DATA = 51;
        public const int GARMIN_PKTID_SAT_DATA = 114;

        public const int GARMIN_PKTID_L001_XFER_CMPLT = 12;
        public const int GARMIN_PKTID_L001_COMMAND_DATA = 10;
        public const int GARMIN_PKTID_L001_DATE_TIME_DATA = 14;
        public const int GARMIN_PKTID_L001_RECORDS = 27;
        public const int GARMIN_PKTID_L001_WPT_DATA = 35;

        public const int GARMIN_PKT_HDR_SIZE = 12;
        public const int GARMIN_MAXCHANNELS = 12;
        public const int GARMIN_MAXSATS = 12;

        public const uint FILE_DEVICE_UNKNOWN = 0x00000022;
        public const int ASYNC_DATA_SIZE = 64;
        public const uint METHOD_BUFFERED = 0;
        public const uint FILE_ANY_ACCESS = 0;

        // Flags controlling what is included in the device information set built
        // by SetupDiGetClassDevs
        public const uint DIGCF_DEFAULT = 0x00000001; // only valid with DIGCF_DEVICEINTERFACE
        public const uint DIGCF_PRESENT = 0x00000002;
        public const uint DIGCF_ALLCLASSES = 0x00000004;
        public const uint DIGCF_PROFILE = 0x00000008;
        public const uint DIGCF_DEVICEINTERFACE = 0x00000010;

        public static byte[] GARMIN_START_SESSION_REQ = { GARMIN_LAYERID_TRANSPORT, 0, 0, 0, GARMIN_PKTID_TRANSPORT_START_SESSION_REQ, 0, 0, 0, 0, 0, 0, 0 };
        public static byte[] GARMIN_PRODUCT_DATA_REQ = { GARMIN_LAYERID_APPL, 0, 0, 0, GARMIN_PKTID_PRODUCT_RQST, 0, 0, 0, 0, 0, 0, 0 };
        public static byte[] GARMIN_START_PVT_REQ = { GARMIN_LAYERID_APPL, 0, 0, 0, GARMIN_PKTID_L001_COMMAND_DATA, 0, 0, 0, 2, 0, 0, 0, 49, 0 };
        public static byte[] GARMIN_STOP_PVT_REQ = { GARMIN_LAYERID_APPL, 0, 0, 0, GARMIN_PKTID_L001_COMMAND_DATA, 0, 0, 0, 2, 0, 0, 0, 50, 0 };

        public GarminUSBDevice()
        {
        }

        public void initialize(delegateOutputStatus outputStatus, delegateOutputPVTData outputPVTData, delegateOutputSatData outputSatData)
        {
            OutputStatus = outputStatus;
            OutputPVTData = outputPVTData;
            OutputSatData = outputSatData;

            GPSthrd = new Thread(new ThreadStart(GPS));
            GPSthrd.Name = "GPS";
            GPSthrd.Start();
        }

        public void End()
        {
            GPSExitThread = true;
        }

        private void SendPacket(IntPtr Packet, int sizeOfPacket)
        {
            int theBytesToWrite = sizeOfPacket;
            int theBytesReturned = 0;

            GCHandle myBytesOutHD = GCHandle.Alloc(theBytesReturned, GCHandleType.Pinned);
            IntPtr BytesOutPtr = myBytesOutHD.AddrOfPinnedObject();

            int code = Kernel32.WriteFile(gHandle, Packet, theBytesToWrite, BytesOutPtr, IntPtr.Zero);

            int bytesOut = (int)Marshal.PtrToStructure(BytesOutPtr, typeof(Int32));

            // If the packet size was an exact multiple of the USB packet 
            // size, we must make a final write call with no data 
            if (theBytesToWrite % gUSBPacketSize == 0)
                Kernel32.WriteFile(gHandle, IntPtr.Zero, 0, BytesOutPtr, IntPtr.Zero);

            myBytesOutHD.Free();
        }

        //----------------------------------------------------------------------------- 
        // Gets a single packet. Since packets may come simultaneously through 
        // asynchrous reads and normal (ReadFile) reads, a full implementation 
        // may require a packet queue and multiple threads. 
        private bool GetPacket(out Packet_t getPacket)
        {
            uint theBufferSize = 0;
            uint theBytesReturned = 0;

            GCHandle brHandle = GCHandle.Alloc(theBytesReturned, GCHandleType.Pinned);
            IntPtr brIntPtr = brHandle.AddrOfPinnedObject();

            byte[] theTempBuffer = new byte[ASYNC_DATA_SIZE];
            GCHandle tbHandle = GCHandle.Alloc(theTempBuffer, GCHandleType.Pinned);
            IntPtr tpIntPtr = tbHandle.AddrOfPinnedObject();

            GCHandle mDataBufferHandle = GCHandle.Alloc(mDataBuffer, GCHandleType.Pinned);
            IntPtr mDataBufferIntPtr = mDataBufferHandle.AddrOfPinnedObject();

            bool status = false;

            getPacket.mData = mDataPtr;
            getPacket.mDataSize = 0;
            getPacket.mPacketId = 0;
            getPacket.mPacketType = 0;
            getPacket.mReserved1 = 0;
            getPacket.mReserved2 = 0;
            getPacket.mReserved3 = 0;

            if (gHandle == IntPtr.Zero)
                return false;

            uint IOCTL_ASYNC_IN = ((FILE_DEVICE_UNKNOWN) << 16 | FILE_ANY_ACCESS << 14 | (0x850) << 2 | METHOD_BUFFERED);

            status = Kernel32.DeviceIoControl(gHandle, IOCTL_ASYNC_IN, IntPtr.Zero, 0, tpIntPtr, ASYNC_DATA_SIZE, brIntPtr, IntPtr.Zero);

            if (status)
            {

                getPacket = (Packet_t)Marshal.PtrToStructure(tpIntPtr, typeof(Packet_t));

                theBytesReturned = (uint)Marshal.ReadInt32(brIntPtr);

                Marshal.Copy(theTempBuffer, GARMIN_PKT_HDR_SIZE, mDataBufferIntPtr, (int)theBytesReturned - GARMIN_PKT_HDR_SIZE);

                theBufferSize = theBufferSize + (theBytesReturned - GARMIN_PKT_HDR_SIZE);

                // Read async data until the driver returns less than the 
                // max async data size, which signifies the end of a packet 
                while ((theBytesReturned == ASYNC_DATA_SIZE) && (theBufferSize < MAX_BUFFER_SIZE) && (status))
                {
                    status = Kernel32.DeviceIoControl(gHandle, IOCTL_ASYNC_IN, IntPtr.Zero, 0, tpIntPtr, ASYNC_DATA_SIZE, brIntPtr, IntPtr.Zero);

                    if (status)
                    {
                        theBytesReturned = (uint)Marshal.ReadInt32(brIntPtr);

                        Array.Copy(theTempBuffer, 0, mDataBuffer, theBufferSize, theBytesReturned);

                        theBufferSize = theBufferSize + theBytesReturned;
                    }
                }

                if (status)
                {
                    getPacket.mData = mDataPtr;
                    Marshal.Copy(mDataBuffer, 0, getPacket.mData, (int)theBufferSize);
                    getPacket.mDataSize = theBufferSize;
                }
            }

            brHandle.Free();
            tbHandle.Free();
            return status;
        }

        private bool InitializeDevice()
        {
            UInt32 theBytesReturned = 0;
            productDescription = "None";
            bool status = false;

            theDevInfo = SetupAPI.SetupDiGetClassDevs(ref garminUSB, 0, IntPtr.Zero, (int)(DIGCF_PRESENT | DIGCF_DEVICEINTERFACE));
            if (theDevInfo == IntPtr.Zero)
                return false;

            theInterfaceData.size = (uint)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));
            if (!SetupAPI.SetupDiEnumDeviceInterfaces(theDevInfo, IntPtr.Zero, ref garminUSB, 0, ref theInterfaceData) && Marshal.GetLastWin32Error() == 259)
            {
                SetupAPI.SetupDiDestroyDeviceInfoList(theDevInfo);
                return false;
            }

            status = SetupAPI.SetupDiGetDeviceInterfaceDetail(theDevInfo, ref theInterfaceData, IntPtr.Zero, 0, out theBytesReturned, IntPtr.Zero);

            //on Win x86, cbSize must be 5 for some reason. On x64, apparently 8 is what it wants.
            if (IntPtr.Size == 8)
                diDetail.size = 4 + 4;
            else
                diDetail.size = 4 + 1; // 4+Marshal.SystemDefaultCharSize = 6 on Vista and does not work.

            status = SetupAPI.SetupDiGetDeviceInterfaceDetail(theDevInfo, ref theInterfaceData, ref diDetail, theBytesReturned, out theBytesReturned, IntPtr.Zero);
            if (status == false)
                return false;

            gHandle = Kernel32.CreateFile(diDetail.devicePath, 3, 3, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (gHandle.ToInt32() == -1)
                return false;

            uint IOCTL_USB_PACKET_SIZE = ((FILE_DEVICE_UNKNOWN) << 16 | FILE_ANY_ACCESS << 14 | (0x851) << 2 | METHOD_BUFFERED);

            Kernel32.DeviceIoControl(gHandle, IOCTL_USB_PACKET_SIZE, IntPtr.Zero, 0, gcIntPtr, 4, ref theBytesReturned, IntPtr.Zero);
            gUSBPacketSize = Marshal.ReadInt32(gcIntPtr);

            if (gUSBPacketSize == 0)
                return false;

            return true;
        }

        private void StartSending_PVTData()
        {
            GCHandle pinnedArray = GCHandle.Alloc(GARMIN_START_PVT_REQ, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            SendPacket(pointer, GARMIN_START_PVT_REQ.Length);
            pinnedArray.Free();
        }

        private void StopSending_PVTData()
        {
            GCHandle pinnedArray = GCHandle.Alloc(GARMIN_STOP_PVT_REQ, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            SendPacket(pointer, GARMIN_STOP_PVT_REQ.Length);
            pinnedArray.Free();
        }

        public void CloseDevice()
        {
            Kernel32.CloseHandle(gHandle);
        }

        private bool StartSession()
        {
            Packet_t StartSessionRspPacket = new Packet_t();
            bool status = false;

            GCHandle pinnedArray = GCHandle.Alloc(GARMIN_START_SESSION_REQ, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            SendPacket(pointer, GARMIN_START_SESSION_REQ.Length);
            pinnedArray.Free();

            status = GetPacket(out StartSessionRspPacket);
            while (((StartSessionRspPacket.mPacketType != GARMIN_LAYERID_TRANSPORT) && (StartSessionRspPacket.mPacketId != GARMIN_PKTID_TRANSPORT_START_SESSION_RESP)) && (status))
            {
                status = GetPacket(out StartSessionRspPacket);
            }

            return status;
        }

        private bool ObtainProductDataFromDevice()
        {
            bool status;
            Packet_t ProductDataRspPacket = new Packet_t();

            GCHandle pinnedArray = GCHandle.Alloc(GARMIN_PRODUCT_DATA_REQ, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            pointer = pinnedArray.AddrOfPinnedObject();
            SendPacket(pointer, GARMIN_PRODUCT_DATA_REQ.Length);
            pinnedArray.Free();

            status = GetPacket(out ProductDataRspPacket);
            while (((ProductDataRspPacket.mPacketType != GARMIN_LAYERID_APPL) && (ProductDataRspPacket.mPacketId != GARMIN_PKTID_PRODUCT_DATA)) && (status))
            {
                status = GetPacket(out ProductDataRspPacket);
            }

            byte[] tbuff = new byte[ProductDataRspPacket.mDataSize];
            Marshal.Copy(ProductDataRspPacket.mData, tbuff, 0, (int)ProductDataRspPacket.mDataSize);
            productDescription = System.Text.Encoding.ASCII.GetString(tbuff, 4, (int)ProductDataRspPacket.mDataSize - 5);

            return status;
        }


        public string GetProductDescription()
        {
            return productDescription;
        }


        void GPS()
        {
            bool status = false;

            pvt_data_type pvt_data = new pvt_data_type();

            sat_data_type sat_data = new sat_data_type();
            byte[] satDataBuf = new byte[Marshal.SizeOf(sat_data)];
            GCHandle satDataBufHandle = GCHandle.Alloc(satDataBuf, GCHandleType.Pinned);
            IntPtr satDataIntPtr = satDataBufHandle.AddrOfPinnedObject();
            byte[] satmDataBuffer = new byte[GARMIN_MAXSATS * Marshal.SizeOf(sat_data)];
            sat_data_type[] sat_data_array = new sat_data_type[GARMIN_MAXSATS];

            Packet_t thePacket = new Packet_t();


            while (GPSExitThread == false)
            {
                Thread.Sleep(1000);

                status = InitializeDevice();

                if (status)
                {
                    status = StartSession();
                }

                if (status)
                {
                    status = ObtainProductDataFromDevice();
                }

                if (status)
                {
                    StartSending_PVTData();
                }

                OutputStatus(status);

                while (status && GPSExitThread == false)
                {

                    Thread.Sleep(10);

                    status = GetPacket(out thePacket);
                    if (status)
                    {
                        switch (thePacket.mPacketId)
                        {
                            case GARMIN_PKTID_PVT_DATA:
                                pvt_data = (pvt_data_type)Marshal.PtrToStructure(thePacket.mData, typeof(pvt_data_type));
                                OutputPVTData(pvt_data);
                                break;
                            case GARMIN_PKTID_SAT_DATA:
                                Marshal.Copy(thePacket.mData, satmDataBuffer, 0, (int)thePacket.mDataSize);

                                for (int i = 0; i < GARMIN_MAXSATS; i++)
                                {
                                    Marshal.Copy(satmDataBuffer, i * Marshal.SizeOf(sat_data), satDataIntPtr, Marshal.SizeOf(sat_data));
                                    sat_data_array[i] = (sat_data_type)Marshal.PtrToStructure(satDataIntPtr, typeof(sat_data_type));
                                }

                                OutputSatData(sat_data_array);

                                break;
                        }
                    }
                }
            }//end While

            if (status)
            {
                StopSending_PVTData();
                CloseDevice();
            }

            Console.WriteLine("GPS Thread Exited  Normally");
        }
    }
}
