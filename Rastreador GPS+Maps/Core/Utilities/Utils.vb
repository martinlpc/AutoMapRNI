Module Utils

    Public Sub Delay(milliseconds As Integer)
        Dim targetTime As DateTime = DateTime.Now.AddMilliseconds(milliseconds)
        While DateTime.Now < targetTime
            Application.DoEvents()
        End While
    End Sub


End Module
