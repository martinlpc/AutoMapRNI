Imports System.IO.Ports
Imports System.IO
Imports System.Globalization

Public Class NMEA0183Reader
    Private _puertoSerie As SerialPort
    Private _bufferNMEA As String = String.Empty

    Private _lat As Double
    Private _lon As Double
    Private _hemisLat As String
    Private _hemisLon As String
    Private _isGPSSync As Boolean = False

   Public Sub New(ByVal nombrePuerto As String, ByVal baudRate As Integer)
        _puertoSerie = New SerialPort(nombrePuerto, baudRate, Parity.None, 8, StopBits.One)
        AddHandler _puertoSerie.DataReceived, AddressOf DataReceivedHandler
    End Sub

    Public Sub New()
    End Sub

    Public Function AbrirPuerto() As Boolean
        Try
            If Not _puertoSerie.IsOpen Then
                _puertoSerie.Open()
                Return True
            Else
                Return False
            End If
        
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function CerrarPuerto() As Boolean
        Try
            If _puertoSerie.IsOpen Then
                _puertoSerie.Close()
                Return True
            End If
        Catch ex As IOException
            'frmMain.nuevoMensajeEventos("Error al cerrar el puerto COM.")
        Catch ex As Exception
            'frmMain.nuevoMensajeEventos("Error inesperado al cerrar el puerto: " & ex.Message)
        End Try
        Return False
    End Function

    Public Function DispositivoConectado() As Boolean
        Try
            Return _puertoSerie.IsOpen
        Catch ex As Exception
            'frmMain.nuevoMensajeEventos("Error al verificar si el dispositivo está conectado: " & ex.Message)
            Return False
        End Try
    End Function

    ' Evento para manejar los datos recibidos
    Private Sub DataReceivedHandler(sender As Object, e As SerialDataReceivedEventArgs)
        Try
            _bufferNMEA += _puertoSerie.ReadExisting()
            If _bufferNMEA.Contains(vbCrLf) Then
                ProcesarBuffer(_bufferNMEA)
                _bufferNMEA = String.Empty ' Limpiar buffer después de procesar
            End If
        Catch ex As TimeoutException
            'frmMain.nuevoMensajeEventos("Error: Tiempo de espera agotado en la lectura del puerto.")
        Catch ex As IOException
            'frmMain.nuevoMensajeEventos("Error de E/S al leer los datos del puerto.")
        Catch ex As Exception
            'frmMain.nuevoMensajeEventos("Error inesperado al recibir datos: " & ex.Message)
        End Try
    End Sub

    ' Procesar la cadena NMEA para obtener coordenadas
    Private Sub ProcesarBuffer(buffer As String)
        Try
            Dim lineas As String() = buffer.Split(vbCrLf.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            For Each linea In lineas
                If linea.StartsWith("$GPRMC") Then
                    ' Primero se revisa que el checksum XOR de la sentencia sea correcto
                    If Not VerificarChecksum(linea) Then Continue For

                    Dim datos As String() = linea.Split(","c)
                    If datos(2) = "V" Then
                        _isGPSSync = False
                        ResetCoordenadas()
                    ElseIf datos(2) = "A" Then ' Datos válidos
                        _lat = ConvertirCoordenada(datos(3), datos(4))
                        _hemisLat = datos(4)
                        _lon = ConvertirCoordenada(datos(5), datos(6))
                        _hemisLon = datos(6)
                        _isGPSSync = True
                    End If
                End If
            Next
        Catch ex As FormatException
            'frmMain.nuevoMensajeEventos("Error en el formato de los datos NMEA.")
        Catch ex As Exception
            'frmMain.nuevoMensajeEventos("Error inesperado al procesar los datos NMEA: " & ex.Message)
        End Try
    End Sub

    Private Function VerificarChecksum(ByVal linea As String) As Boolean
        Try
            Dim checksumCalculado As Integer = 0
            Dim segmentos() As String = linea.Split("*"c)
            Dim sentencia As String = segmentos(0).Substring(1)
            Dim checksumRecibido As String = segmentos(1).Trim()

            For Each character As Char In sentencia
                checksumCalculado = checksumCalculado Xor Convert.ToByte(character)
            Next

            Return checksumCalculado.ToString("X2") = checksumRecibido.ToUpper()
        Catch ex As Exception
            Return False
        End Try
    End Function

    ' Método para convertir coordenadas de NMEA a decimal o GG MM SS.SS
    Private Function ConvertirCoordenada(coordenada As String, hemisferio As String) As Double
        Try
            Dim grados As Integer
            Dim minutos As Double

            If coordenada.Length = 9 Then
                grados = Integer.Parse(coordenada.Substring(0, 2))
                minutos = Double.Parse(coordenada.Substring(2), CultureInfo.InvariantCulture) / 60
            ElseIf coordenada.Length = 10 Then
                grados = Integer.Parse(coordenada.Substring(0, 3))
                minutos = Double.Parse(coordenada.Substring(3), CultureInfo.InvariantCulture) / 60
            Else
                Throw New ArgumentException("La coordenada NMEA tiene un formato inválido.")
            End If

            Dim resultado As Double = grados + minutos
            If hemisferio = "S" Or hemisferio = "W" Then
                resultado *= -1
            End If

            Return resultado
        Catch ex As Exception
            'Console.WriteLine("Error al convertir las coordenadas: " & ex.Message)
            Return 0.0
        End Try
    End Function

    ' Método para restablecer las coordenadas a valores predeterminados
    Private Sub ResetCoordenadas()
        _lat = 0.0
        _lon = 0.0
        _hemisLat = ""
        _hemisLon = ""
    End Sub

    ' Métodos para obtener las coordenadas actuales
    Public Function ObtenerLatitud() As Double
        Return If(_isGPSSync, _lat, 0.0)
    End Function

    Public Function ObtenerLongitud() As Double
        Return If(_isGPSSync, _lon, 0.0)
    End Function

    Public Function ObtenerHemisferioLatitud() As String
        Return If(_isGPSSync, _hemisLat, "")
    End Function

    Public Function ObtenerHemisferioLongitud() As String
        Return If(_isGPSSync, _hemisLon, "")
    End Function

    Public Function ObtenerGPSStatus() As Boolean
        Return _isGPSSync
    End Function
End Class

