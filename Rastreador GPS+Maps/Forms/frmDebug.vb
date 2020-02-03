Public Class frmDebug

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged
        TextBox1.Select(TextBox1.Text.Length, 0)
        TextBox1.ScrollToCaret()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As System.Object, e As System.EventArgs)

    End Sub
End Class