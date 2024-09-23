Imports System.IO.Ports
Imports System.IO

Public Class NMEA0183Reader
    Private _puertoSerie As SerialPort
    Private _bufferNMEA As String = String.Empty

    ' Constructor para inicializar el puerto COM
    Public Sub New(ByVal nombrePuerto As String, ByVal baudRate As Integer)
        _puertoSerie = New SerialPort(nombrePuerto, baudRate, Parity.None, 8, StopBits.One)
        AddHandler _puertoSerie.DataReceived, AddressOf DataReceivedHandler
    End Sub

    Public Sub New()

    End Sub

    ' Método para abrir el puerto serie
    Public Sub AbrirPuerto()
        Try
            If Not _puertoSerie.IsOpen Then
                _puertoSerie.Open()
                Console.WriteLine("Puerto abierto correctamente.")
            End If
        Catch ex As UnauthorizedAccessException
            Console.WriteLine("Error: Acceso no autorizado al puerto COM.")
        Catch ex As IOException
            Console.WriteLine("Error: Fallo en la conexión al puerto COM.")
        Catch ex As Exception
            Console.WriteLine("Error inesperado al abrir el puerto: " & ex.Message)
        End Try
    End Sub

    ' Método para cerrar el puerto serie
    Public Sub CerrarPuerto()
        Try
            If _puertoSerie.IsOpen Then
                _puertoSerie.Close()
                Console.WriteLine("Puerto cerrado correctamente.")
            End If
        Catch ex As IOException
            Console.WriteLine("Error al cerrar el puerto COM.")
        Catch ex As Exception
            Console.WriteLine("Error inesperado al cerrar el puerto: " & ex.Message)
        End Try
    End Sub

    ' Método para verificar si el dispositivo está conectado
    Public Function DispositivoConectado() As Boolean
        Try
            Return _puertoSerie.IsOpen
        Catch ex As Exception
            Console.WriteLine("Error al verificar si el dispositivo está conectado: " & ex.Message)
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
            Console.WriteLine("Error: Tiempo de espera agotado en la lectura del puerto.")
        Catch ex As IOException
            Console.WriteLine("Error de E/S al leer los datos del puerto.")
        Catch ex As Exception
            Console.WriteLine("Error inesperado al recibir datos: " & ex.Message)
        End Try
    End Sub

    ' Procesar la cadena NMEA para obtener coordenadas
    Private Sub ProcesarBuffer(buffer As String)
        Try
            Dim lineas As String() = buffer.Split(vbCrLf.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            For Each linea In lineas
                If linea.StartsWith("$GPRMC") Then
                    Dim datos As String() = linea.Split(","c)
                    If datos(2) = "V" Then
                        Console.WriteLine("GPS sin posición establecida")
                    ElseIf datos(2) = "A" Then ' Datos válidos
                        Dim latitud As Double = ConvertirCoordenada(datos(3), datos(4))
                        Dim longitud As Double = ConvertirCoordenada(datos(5), datos(6))
                        Console.WriteLine("Latitud: " & latitud & ", Longitud: " & longitud)
                    Else
                        Console.WriteLine("Error: Datos GPS no válidos.")
                    End If
                End If
            Next
        Catch ex As FormatException
            Console.WriteLine("Error en el formato de los datos NMEA.")
        Catch ex As Exception
            Console.WriteLine("Error inesperado al procesar los datos NMEA: " & ex.Message)
        End Try
    End Sub

    ' Método para obtener coordenadas de la sentencia GPRMC
    Public Function ObtenerCoordenadas(formato As String) As String
        ' Aquí retornaríamos las coordenadas procesadas en el formato deseado
        ' Esta función se llenaría con la lógica para retornar las coordenadas según el formato (GG MM SS.SS o decimal)
        Return "Coordenadas en formato: " & formato
    End Function

    ' Método para convertir coordenadas de NMEA a decimal o GG MM SS.SS
    Private Function ConvertirCoordenada(coordenada As String, hemisferio As String) As Double
        Try
            Dim grados As Integer = Integer.Parse(coordenada.Substring(0, 2))
            Dim minutos As Double = Double.Parse(coordenada.Substring(2)) / 60

            Dim resultado As Double = grados + minutos
            If hemisferio = "S" Or hemisferio = "W" Then
                resultado *= -1
            End If

            Return resultado
        Catch ex As Exception
            Console.WriteLine("Error al convertir las coordenadas: " & ex.Message)
            Return 0.0
        End Try
    End Function
End Class

