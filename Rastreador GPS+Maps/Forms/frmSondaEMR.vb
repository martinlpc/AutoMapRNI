Public Class frmSondaEMR
    Dim PrimerCarga As Boolean = True

    Private Sub frmSondaEMR_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim nReg As ListViewItem
        For i = 0 To 17
            nReg = New ListViewItem(Sondas300(i).Tipo)
            With nReg
                'Cada subitem es la columna 2 en adelante (los atributos del item)
                .SubItems.Add(Sondas300(i).NumSerie)
                .SubItems.Add(Sondas300(i).LimInf)
                .SubItems.Add(Sondas300(i).LimSup)
                .SubItems.Add(Sondas300(i).Incert)
                .SubItems.Add(Sondas300(i).Factor)
            End With
            ListaSondasEMR300.Items.Add(nReg)
            'Destilda el checkbox del item 
            'ListaSondasEMR300.Items(i).Checked = False
        Next
        PrimerCarga = False
    End Sub

    Private Sub btnAceptar_Click(sender As System.Object, e As System.EventArgs) Handles btnAceptar.Click
        Dim miItemSonda As New ListViewItem
        For Each miItemSonda In ListaSondasEMR300.Items
            If miItemSonda.Checked Then
                With SondaSel
                    .Marca = "NARDA"
                    .Modelo = miItemSonda.Text
                    .NumSerie = miItemSonda.SubItems.Item(1).Text
                    .IncertDB = CSng(miItemSonda.SubItems.Item(4).Text)
                    .Factor = CSng(miItemSonda.SubItems.Item(5).Text)
                End With
            End If
        Next
        With frmMain
            .lblSonda.Text = "Type " & SondaSel.Modelo & " - S/N: " & SondaSel.NumSerie
            .chkActual.Checked = False
            .chkMaxAvg.Enabled = False
            .chkMaxAvg.Checked = False
            .chkMaxHold.Checked = True
        End With
        MsgBox("Con el EMR300, se permite unicamente utilizar el modo MAXHOLD.")
        Me.Hide()
        frmMain.Focus()
    End Sub

    Private Sub ListaSondasEMR300_ItemCheck(sender As Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles ListaSondasEMR300.ItemCheck
        For i As Integer = 0 To ListaSondasEMR300.Items.Count - 1
            If i <> e.Index Then ListaSondasEMR300.Items.Item(i).Checked = False
        Next
    End Sub

    Private Sub btnCancelar_Click(sender As System.Object, e As System.EventArgs) Handles btnCancelar.Click
        With SondaSel
            .Marca = ""
            .Modelo = ""
            .NumSerie = ""
            .Factor = 0
        End With
        frmMain.chkNBM550.Checked = True
        frmMain.chkEMR300.Checked = False
        Me.Hide()
    End Sub
End Class