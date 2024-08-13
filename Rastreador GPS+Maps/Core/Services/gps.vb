Imports System.IO.Ports
Imports GPS_Martin

Public Class GPSReader

    Private serialPort As SerialPort
    Const debugLat As Double = -34.7505556
    Const debugLon As Double = -58.4988889

    Public Enum GPS_STATUS
        Disconnected = 0
        Searching = 1
        Positioned = 2
        Simulated = 3
    End Enum

    Public Sub New(portName As String, baudRate As Integer, dataBits As Integer, parity As Integer, stopBits As Integer, handShake As Integer)

        ' ESTO SOLO SIRVE PARA EL GLOBALSAT
        ' GARMIN UTILIZA LA LIBRERIA GPS_MARTIN PARA CONEXION

        serialPort = New SerialPort(portName, baudRate, parity, dataBits, stopBits)
        serialPort.Handshake = handShake

        Try
            serialPort.Open()
        Catch ex As UnauthorizedAccessException
            MsgBox("Error accediendo al puerto " & portName & vbNewLine & ex.Message & vbNewLine & ex.StackTrace)
        End Try
    End Sub

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
        Dim Lat As New CoordenadasGMS
        Dim Lon As New CoordenadasGMS
        Dim gpsData As New Dictionary(Of String, String)()

        Try
            Dim currentStatus = EquipoGpsNmea.InfoGPSConectado
            Dim latStr = EquipoGpsNmea.LatitudStr
            Dim lonStr = EquipoGpsNmea.LongitudStr

            If currentStatus = Nothing Or currentStatus = "Demo" Then
                ' GPS desconectado
                gpsData("Status") = GPS_STATUS.Disconnected.ToString
                gpsData("Lat") = ""
                gpsData("Lon") = ""
            ElseIf latStr = Nothing Or latStr = "?" Then
                ' No hay posición determinada, buscando
                gpsData("Status") = GPS_STATUS.Searching.ToString
                gpsData("Lat") = "--"
                gpsData("Lon") = "--"
            Else
                ' Posicionado
                gpsData("Status") = GPS_STATUS.Positioned.ToString
                gpsData("Lat") = formatGPSStreamToGDec(latStr)
                gpsData("Lon") = formatGPSStreamToGDec(lonStr)
            End If

            Return gpsData
        Catch ex As Exception
            Return "Error"
        End Try
    End Function

    Public Function getPositionFromNMEA()

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
        resultado.Hemisferio = vector(1).Trim()

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
End Class
