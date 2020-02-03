Imports System.Windows.Forms

Public Class frmSondaNBM

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKangoo.Click
        frmMain.boolSprinter = False
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSprinter.Click
        frmMain.boolSprinter = True
        Me.Close()
    End Sub

End Class
