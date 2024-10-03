Imports System.IO.Ports
Imports GPS_Martin


Public Class GPSReader

    Private serialPort As SerialPort

    Public gpsStatus As Integer

    Const debugLat As Double = -34.7505556
    Const debugLon As Double = -58.4988889

    Public Enum GPS_STATUS
        Disconnected = 0
        Searching = 1
        Positioned = 2
        Simulated = 3
    End Enum

    Public Sub New()
    End Sub

    Public Function openNmeaGpsPort(portName As String) As Boolean
        ' ESTO SOLO SIRVE PARA EL GLOBALSAT
        ' GARMIN UTILIZA LA LIBRERIA GPS_MARTIN PARA CONEXION

        ' Fixed known settings
        serialPort = New SerialPort(portName, 4800, Parity.None, 8, StopBits.One)
        serialPort.Handshake = Handshake.None

        Try
            serialPort.Open()
            Return True
        Catch ex As UnauthorizedAccessException
            serialPort.Close()
            Return False
        End Try
    End Function

    Public Function getSimulatedPosition() As Dictionary(Of String, String)
        Dim gpsData As New Dictionary(Of String, String)()

        gpsData("Status") = GPS_STATUS.Simulated
        gpsData("Lat") = debugLat.ToString()
        gpsData("Lon") = debugLon.ToString()

        Dim asd = getPositionFromGarmin()

        Return gpsData
    End Function

    ''' <summary>
    ''' Get Garmin 18x USB status and position
    ''' </summary>
    ''' <returns>{"Status","Lat","Lon"} or "Error"</returns>
    ''' <remarks></remarks>
    Public Function getPositionFromGarmin()
        Dim gpsData As New Dictionary(Of String, String)()

        Try
            Dim currentStatus = EquipoGpsNmea.InfoGPSConectado
            Dim latStr = EquipoGpsNmea.LatitudStr
            Dim lonStr = EquipoGpsNmea.LongitudStr

            If currentStatus = Nothing Or currentStatus = "Demo" Then
                ' GPS desconectado
                gpsData("Status") = GPS_STATUS.Disconnected
                gpsData("Lat") = ""
                gpsData("Lon") = ""
            ElseIf latStr = Nothing Or latStr = "?" Then
                ' No hay posición determinada, buscando
                gpsData("Status") = GPS_STATUS.Searching
                gpsData("Lat") = "--"
                gpsData("Lon") = "--"
            Else
                ' Posicionado
                gpsData("Status") = GPS_STATUS.Positioned
                gpsData("Lat") = formatGPSStreamToGDec(latStr)
                gpsData("Lon") = formatGPSStreamToGDec(lonStr)
            End If

            gpsStatus = gpsData("Status")

            Return gpsData
        Catch ex As Exception
            Return "Error"
        End Try
    End Function

    Public Function getPositionFromNMEA()

        Try
            If Not serialPort.IsOpen Then
                Throw New ApplicationException("No se inicializó el puerto del GPS")
            End If
        Catch ex As Exception
            Return ex.Message
        End Try

        Dim response As New Dictionary(Of String, String)()
        Dim gpsStream As String = String.Empty

        Const MaxRetries As Integer = 10
        Dim retries As Integer = 0

        Try
            While retries <= MaxRetries
                Delay(200)
                gpsStream &= serialPort.ReadExisting()

                If String.IsNullOrWhiteSpace(gpsStream) Then
                    response("Status") = GPS_STATUS.Disconnected
                    response("Lat") = ""
                    response("Lon") = ""

                    Return response
                End If

                If Not checkGpsStream(gpsStream) Then
                    retries += 1
                    Continue While
                Else
                    Exit While
                End If
            End While

            If retries > MaxRetries Then
                Throw New ApplicationException("Demasiados intentos de conexión del GPS fallidos")
            End If

            Dim gpsData = getGPRMCParsedData(gpsStream)

            If gpsData("Status") = "V" Then
                response("Status") = GPS_STATUS.Searching
                response("Lat") = "--"
                response("Lon") = "--"

                Return response
            Else
                Dim lat, lon, sign, deg, decimals As String

                If gpsData("Lat hemisphere") = "S" Then sign = "-" Else sign = ""
                deg = gpsData("Lat").Substring(0, 2)
                decimals = gpsData("Lat").Substring(2).Replace(".", ",")
                decimals = CDbl(decimals) / 60
                lat = sign & (CDbl(deg) + CDbl(decimals)).ToString

                If gpsData("Lon hemisphere") = "W" Then sign = "-" Else sign = ""
                deg = gpsData("Lon").Substring(0, 3)
                decimals = gpsData("Lon").Substring(3).Replace(".", ",")
                decimals = CDbl(decimals) / 60
                lon = sign & (CDbl(deg) + CDbl(decimals)).ToString

                response("Status") = GPS_STATUS.Positioned
                response("Lat") = lat
                response("Lon") = lon

                Return response
            End If

        Catch ex As Exception
            Return ex.Message & vbNewLine & ex.StackTrace
        End Try

    End Function

    Private Function checkGpsStream(ByRef gpsStream As String) As Boolean
        Dim GPRMCLastIndex, GPRMCEnd As Integer

        GPRMCLastIndex = gpsStream.LastIndexOf("$GPRMC")
        If GPRMCLastIndex = -1 Then Return False

        gpsStream = gpsStream.Substring(GPRMCLastIndex)
        GPRMCEnd = gpsStream.IndexOf(vbCrLf)
        If GPRMCEnd = -1 Then Return False

        gpsStream.Substring(0, GPRMCEnd)
        Dim isChecksumValid As Boolean = checkMD5(gpsStream)
        If Not isChecksumValid Then Return False

        Return True
    End Function

    ''' <summary>
    ''' Parsea la sentencia GPRMC de un GPS para extraer sus datos en un dict(str, str)
    ''' </summary>
    ''' <param name="sentence"></param>
    ''' <returns>Dict(str, str)</returns>
    ''' <remarks></remarks>
    Private Function getGPRMCParsedData(sentence As String) As Dictionary(Of String, String)

        Dim extractedData As New Dictionary(Of String, String)()

        Dim data As String() = sentence.Split(",")

        extractedData("UTC Time") = data(1)
        extractedData("Status") = data(2)
        extractedData("Lat") = data(3)
        extractedData("Lat hemisphere") = data(4)
        extractedData("Lon") = data(5)
        extractedData("Lon hemisphere") = data(6)
        extractedData("Ground speed") = data(7)
        extractedData("Heading") = data(8)
        extractedData("Date") = data(9)
        extractedData("Mag variation") = data(10)
        extractedData("Mag var heading") = data(11).Split("*"c)(0)

        Return extractedData
    End Function

    ''' <summary>
    ''' Función que hizo MD para completar las coordenadas del gps GARMIN cuando le faltan dígitos y evitar errores
    ''' </summary>
    ''' <param name="latLon"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function formatGPSStreamToGMS(latLon As String) As CoordenadasGMS
        Dim resultado As New CoordenadasGMS

        If (String.IsNullOrWhiteSpace(latLon)) Or latLon = Nothing Then
            Return resultado
        End If

        REM Ejemplo 1º 22' 33" N
        Dim vector As String() = latLon.Split(New Char() {Chr(&HB0)}) REM Chr(&HB0) = grados "\u00B0"
        resultado.Grados = Math.Abs(CInt(vector(0)))
        vector = vector(1).Split(New Char() {"'"})
        resultado.Minutos = Math.Abs(CInt(vector(0)))
        vector = vector(1).Split(New Char() {""""})
        resultado.Segundos = Math.Round(Math.Abs(CDbl(vector(0))), 3)
        resultado.Hemisf = vector(1).Trim()

        Return resultado
    End Function

    Private Function formatGPSStreamToGDec(latLon As String) As String
        Dim res As Double

        If (String.IsNullOrWhiteSpace(latLon)) Or latLon = Nothing Then
            Return ""
        End If

        ' Stream Garmin ej: "34° 45' -2,14" S"
        Dim coorArray As String() = latLon.Split(New Char() {Chr(&HB0)}) ' simbolo "°"
        Dim grados = Math.Abs(CInt(coorArray(0)))

        coorArray = coorArray(1).Split(New Char() {"'", """"})
        Dim decimales = CDbl(coorArray(0)) / 60 + CDbl(coorArray(1)) / 3600

        res = grados + decimales

        If coorArray.Contains("S") Or coorArray.Contains("W") Then
            res = 0 - res
        End If

        Return res.ToString

    End Function

    ''' <summary>
    ''' Chequea la redundancia de datos del streaming del GPS
    ''' </summary>
    ''' <param name="nmeaSentence">Cadena de datos que envía el GPS a analizar</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function checkMD5(nmeaSentence As String) As Boolean
        Dim checksumIndex As Integer = nmeaSentence.IndexOf("*")
        If checksumIndex = -1 Then Return False

        Dim dataPart As String = nmeaSentence.Substring(1, checksumIndex - 1)
        Dim receivedChecksum As String = nmeaSentence.Substring(checksumIndex + 1, 2)

        Dim calculatedChecksum As String = calculateMD5Checksum(dataPart)

        Return String.Equals(calculatedChecksum, receivedChecksum, StringComparison.OrdinalIgnoreCase)
    End Function

    Private Shared Function calculateMD5Checksum(data As String) As String
        Dim checksum As Integer = 0
        For Each character As Char In data
            checksum = checksum Xor Convert.ToByte(character)
        Next
        Return checksum.ToString("X2")
    End Function

End Class
