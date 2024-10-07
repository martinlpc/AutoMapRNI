
Public Class frmDebug

    Dim NMEAReader As NMEA0183Reader
    Dim NBMReader As NBM550Reader

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged
        TextBox1.Select(TextBox1.Text.Length, 0)
        TextBox1.ScrollToCaret()
    End Sub

    Private Sub frmDebug_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If NMEAReader.DispositivoConectado() Then NMEAReader.CerrarPuerto()
    End Sub

    Private Sub linkConnectNMEA_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        NMEAReader = New NMEA0183Reader(txtNMEAPort.Text, 4800)

        If NMEAReader.AbrirPuerto() Then
            TextBox1.AppendText("GPS NMEA conectado en puerto: " & txtNMEAPort.Text & vbNewLine)
        Else
            TextBox1.AppendText("Error intentando conectar GPS NMEA" & vbNewLine)
        End If
    End Sub

    Private Sub linkGetNMEAData_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles linkGetNMEAData.LinkClicked
        With NMEAReader
            If .DispositivoConectado() Then
                If .ObtenerGPSStatus Then
                    Dim lat = .ObtenerLatitud
                    Dim hLat = .ObtenerHemisferioLatitud
                    Dim lon = .ObtenerLongitud
                    Dim hLon = .ObtenerHemisferioLongitud
                    TextBox1.AppendText(String.Format("lat: {0} {1} | lon: {2} {3}{4}", lat, hLat, lon, hLon, vbNewLine))
                    Dim mapPoint As GMap.NET.PointLatLng
                    mapPoint.Lat = lat
                    mapPoint.Lng = lon
                    frmMain.Mapa.Position = mapPoint
                Else
                    TextBox1.AppendText("GPS sin posición válida" & vbNewLine)
                End If
            Else
                TextBox1.AppendText("No hay GPS conectado o no se detecta" & vbNewLine)
            End If
        End With
    End Sub

    Private Sub btnConectarNBM_Click(sender As System.Object, e As System.EventArgs) Handles btnConectarNBM.Click

    End Sub
End Class