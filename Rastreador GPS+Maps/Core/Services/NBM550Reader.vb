Imports System.IO.Ports
Imports AutoMapRNI.NBM550Probe

Public Class NBM550Reader
    Private serialPort As SerialPort
    Public attachedProbe As New NBM550Probe()
    Public isConnected As Boolean = False
    Public serialNumber As String
    Const baudRate As Integer = 460800
    Const dataBits As Integer = 8
    Const parity As Integer = 0
    Const stopBits As Integer = 1
    Const handshake As Integer = 0
    Private splitters() As String = {",", ";"}
    Private trimmers() As Char = {vbCr, Chr(34), "'", "°", " """}

    Public Sub New()
    End Sub

    ''' <summary>
    ''' Establish a connection with the NBM550 on the specified COM port
    ''' </summary>
    ''' <param name="portName"></param>
    ''' <returns>"0" if error, string containing its serial number if successful</returns>
    ''' <remarks></remarks>
    Public Function connectToDevice(portName As String) As String
        ' Fixed known settings
        serialPort = New SerialPort(portName, baudRate, parity, dataBits, stopBits)
        serialPort.Handshake = handshake

        Try
            If Not handleDeviceConnection() Then
                Return "-999"
            End If

            If Not readAttachedProbe() Then
                Return "-999"
            End If

            isConnected = True
            Return serialNumber
        Catch ex As Exception
            serialPort.Close()
            Return "-999"
        End Try

    End Function

    Private Function handleDeviceConnection() As Boolean
        Try
            serialPort.Open()
            serialPort.DiscardInBuffer()
            
            Dim resp As String = sendCommand("REMOTE ON;", 100)

            If resp <> "0;" & vbCr & "" And Not resp.Contains("401;") Then
                Return False
            End If

            resp = sendCommand("DEVICE_INFO?;", 300)

            Dim respArray() As String = resp.Split(splitters, StringSplitOptions.None)
            serialNumber = respArray(2).Trim(trimmers)

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Function readAttachedProbe() As Boolean
        Try
            Dim resp As String = sendCommand("PROBE_INFO?;", 300)
            Dim arrayResp As String() = resp.Split(splitters, StringSplitOptions.None)
            Dim dateOfCal As String = arrayResp(3).Trim.Replace(".", "/")

            dateOfCal = dateOfCal.Substring(3, 3) & dateOfCal.Substring(0, 3) & dateOfCal.Substring(6, 2)

            attachedProbe.model = arrayResp(0).Trim(trimmers)
            attachedProbe.serialNumber = arrayResp(2).Trim(trimmers)
            attachedProbe.dateOfCalibration = dateOfCal

            If attachedProbe.model.Contains("EF") Then
                sendCommand("RESULT_UNIT V/m;", 100)
            Else
                sendCommand("RESULT_UNIT A/m;", 100)
            End If

            handleMaxSettings()


            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function getProbe() As String
        Dim probe As String = _
            attachedProbe.model & "," & attachedProbe.serialNumber & "," & attachedProbe.dateOfCalibration

        Return probe
    End Function

    Public Function readCurrentValue(readMax As Boolean) As String
        Try
            Dim res As String = sendCommand("MEAS?;", 300)
            Dim array As String() = res.Split(",")
            Dim level As Single
            Dim getActualLevel As Integer

            If readMax Then
                getActualLevel = 0
            Else
                getActualLevel = 1
            End If

            level = Format(CSng(array(getActualLevel) / 1000), "##0.000")
        
            Return level
        Catch ex As Exception
            Return "-999"
        End Try
    End Function

    Public Function readBattery() As String
        Try
            Dim res As String = sendCommand("BATTERY?;", 100)
            If res = "-999" Then
                Throw New Exception("Error reading device battery")
            End If

            Dim batteryLevel As Integer = CInt(res.Substring(0, Len(res) - 2))

            Return batteryLevel
        Catch ex As Exception
                Return "-999"
        End Try
        
    End Function

    Public Function resetMaxHold() As String
        Try
            Dim res As String = sendCommand("RESET_MAX?;", 300)

            If res = "-999" Then
                Throw New Exception("Error resetting max hold")
            End If
        Catch ex As Exception
                Return "-999"
        End Try
    End Function

    Private Function sendCommand(sentence As String, timeInterval As Integer) As String
        Try
            serialPort.WriteLine(sentence)
            Delay(timeInterval)
            Return serialPort.ReadExisting()
        Catch ex As Exception
            Return "-999"
        End Try
    End Function

    Private Sub handleMaxSettings()
        sendCommand("RESULT_TYPE MAX;", 100)
        serialPort.DiscardInBuffer()

        sendCommand("RESET_MAX;", 100)
        serialPort.DiscardInBuffer()

        sendCommand("MEAS_VIEW NORMAL;", 100)
        serialPort.DiscardInBuffer()

        sendCommand("AUTO_ZERO OFF;", 100)
        serialPort.DiscardInBuffer()
    End Sub

    ''' <summary>
    ''' Sends a remote off sentence and closes the COM port
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function disconnectDevice() As Boolean
        Try
            sendCommand("AUTO_ZERO ON;", 100)
            sendCommand("REMOTE OFF;", 100)

            close()

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Sub close()
        If serialPort IsNot Nothing Then
            If serialPort.IsOpen() Then
                serialPort.Close()
            End If
            serialPort.Dispose()
            serialPort = Nothing
            isConnected = False
        End If
    End Sub
End Class
