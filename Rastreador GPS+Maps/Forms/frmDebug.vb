Imports AutoMapRNI.GPSReader
Imports AutoMapRNI.NBM550Reader
Imports AutoMapRNI.NBM550Probe
Imports System.IO.Ports

Public Class frmDebug

    Dim gpsReader As GPSReader
    Dim nbmReader As NBM550Reader

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged
        TextBox1.Select(TextBox1.Text.Length, 0)
        TextBox1.ScrollToCaret()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub frmDebug_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        cboCOMGps.Items.AddRange(SerialPort.GetPortNames())
        cboCOMNBM.Items.AddRange(SerialPort.GetPortNames())

        gpsReader = New GPSReader()
        nbmReader = New NBM550Reader()

    End Sub

    Private Sub cboCOMGps_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboCOMGps.SelectedIndexChanged
        Dim port = cboCOMGps.Text
        Dim resp = gpsReader.openNmeaGpsPort(port)
        If resp Then
            TextBox1.AppendText("GPS nmea en el puerto " & port & vbNewLine)
        Else
            TextBox1.AppendText("No se pudo abrir el puerto " & port & vbNewLine)
        End If
    End Sub
    
    Private Sub btnTestGarmin_Click(sender As System.Object, e As System.EventArgs) Handles btnTestGarmin.Click

        Dim res = gpsReader.getPositionFromGarmin()

        If res IsNot "Error" Then
            For Each elem As KeyValuePair(Of String, String) In res
                TextBox1.AppendText(elem.Key & ": " & elem.Value & vbNewLine)
            Next
        Else
            TextBox1.AppendText(res & vbNewLine)
        End If

    End Sub

    Private Sub btnTestNMEA_Click(sender As System.Object, e As System.EventArgs) Handles btnTestNMEA.Click

        Dim res As Object = gpsReader.getPositionFromNMEA()

        If TypeOf (res) Is Boolean Then
            TextBox1.AppendText(res.ToString & vbNewLine)
        ElseIf TypeOf (res) Is String Then
            TextBox1.AppendText(res & vbNewLine)
        Else

            For Each elem As KeyValuePair(Of String, String) In res
                TextBox1.AppendText(elem.Key & ": " & elem.Value & vbNewLine)
            Next
        End If

    End Sub

    Private Sub btnConectarNBM_Click(sender As System.Object, e As System.EventArgs) Handles btnConectarNBM.Click
        If nbmReader.isConnected Then
            nbmReader.disconnectDevice()
        End If

        Dim res As String = nbmReader.connectToDevice(cboCOMNBM.Text)

        If res = "-999" Then
            TextBox1.AppendText("No se pudo detectar un NBM550" & vbNewLine)
            nbmReader.close()
        Else
            TextBox1.AppendText("Equipo conectado: " & res & vbNewLine)
        End If

    End Sub

    Private Sub btnLeerDatos_Click(sender As System.Object, e As System.EventArgs) Handles btnLeerDatos.Click
        Dim res As String = nbmReader.getProbe()
        TextBox1.AppendText("Info sonda: " & res & vbNewLine)

        res = nbmReader.readBattery()
        TextBox1.AppendText("Batería: " & res & vbNewLine)

        res = nbmReader.readCurrentValue(False)
        TextBox1.AppendText("Nivel actual: " & res & vbNewLine)

        res = nbmReader.readCurrentValue(True)
        TextBox1.AppendText("Nivel max: " & res & vbNewLine)

    End Sub
End Class