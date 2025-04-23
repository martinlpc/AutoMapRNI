Public Class GPSSimulator
    Private _currentLat As Double = -34.9214  ' La Plata, Argentina
    Private _currentLon As Double = -57.9544
    Private _speed As Double = 0.0001         ' Velocidad de cambio por actualización
    Private _direction As Integer = 1         ' 1: incrementando, -1: decrementando
    Private _simulationMode As Integer = 0    ' 0: Paralelo, 1: Meridiano

    ' Configurar simulación
    Public Sub ConfigureSimulation(mode As Integer, startLat As Double, startLon As Double)
        _simulationMode = mode
        _currentLat = startLat
        _currentLon = startLon
    End Sub

    ' Simular movimiento y obtener sentencia NMEA actualizada
    Public Function GetNextNMEAData() As String
        UpdatePosition()
        Return GenerateNMEASentence()
    End Function

    ' Actualizar posición
    Private Sub UpdatePosition()
        If _simulationMode = 0 Then
            ' Modo paralelo (cambiar latitud)
            _currentLat += _speed * _direction

            ' Verificar si estamos cruzando un paralelo
            If Math.Floor(_currentLat) <> Math.Floor(_currentLat + (_speed * _direction)) Then
                frmMain.nuevoMensajeEventos("Cruzando paralelo!")

            End If
        Else
            ' Modo meridiano (cambiar longitud)
            _currentLon += _speed * _direction

            ' Verificar si estamos cruzando un meridiano
            If Math.Floor(_currentLon) <> Math.Floor(_currentLon + (_speed * _direction)) Then
                frmMain.nuevoMensajeEventos("Cruzando meridiano!")
            End If
        End If

        ' Cambiar dirección si nos alejamos demasiado
        If Math.Abs(_currentLat - (-34.9)) > 0.2 OrElse Math.Abs(_currentLon - (-57.9)) > 0.2 Then
            _direction = -_direction
        End If
    End Sub

    ' Generar sentencia NMEA
    Private Function GenerateNMEASentence() As String
        ' Convertir lat/lon a formato NMEA
        Dim latStr As String = ConvertLatToNMEA(_currentLat)
        Dim lonStr As String = ConvertLonToNMEA(_currentLon)

        ' Construir sentencia GGA
        Dim utcTime As String = DateTime.UtcNow.ToString("HHmmss.ff")
        Dim gga As String = String.Format("$GPGGA,{0},{1},N,{2},W,1,08,1.0,12.0,M,0.0,M,,*",
                                         utcTime, latStr, lonStr)

        ' Construir sentencia RMC
        Dim utcDate As String = DateTime.UtcNow.ToString("ddMMyy")
        Dim speed As String = "005.0" ' 5 nudos de velocidad
        Dim course As String = "180.0" ' rumbo sur
        Dim rmc As String = String.Format("$GPRMC,{0},A,{1},N,{2},W,{3},{4},{5},,,A*",
                                         utcTime, latStr, lonStr, speed, course, utcDate)

        ' Añadir checksums
        gga = AddChecksum(gga)
        rmc = AddChecksum(rmc)

        Return gga & vbCrLf & rmc
    End Function

    ' Convertir latitud a formato NMEA (ddmm.mmm)
    Private Function ConvertLatToNMEA(lat As Double) As String
        Dim latAbs As Double = Math.Abs(lat)
        Dim degrees As Integer = Math.Floor(latAbs)
        Dim minutes As Double = (latAbs - degrees) * 60
        Return String.Format("{0:00}{1:00.0000}", degrees, minutes)
    End Function

    ' Convertir longitud a formato NMEA (dddmm.mmm)
    Private Function ConvertLonToNMEA(lon As Double) As String
        Dim lonAbs As Double = Math.Abs(lon)
        Dim degrees As Integer = Math.Floor(lonAbs)
        Dim minutes As Double = (lonAbs - degrees) * 60
        Return String.Format("{0:000}{1:00.0000}", degrees, minutes)
    End Function

    ' Calcular y añadir checksum a sentencia NMEA
    Private Function AddChecksum(sentence As String) As String
        Dim checksum As Integer = 0

        ' Calcular XOR de todos los bytes entre $ y *
        For i As Integer = 1 To sentence.Length - 1
            If sentence(i) = "*" Then Exit For
            checksum = checksum Xor Convert.ToByte(sentence(i))
        Next

        ' Añadir checksum en formato hexadecimal
        Return sentence & checksum.ToString("X2")
    End Function
End Class
