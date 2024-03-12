Imports System.Threading.Tasks
Imports System.Threading
Imports System.Windows.Forms

Public Class ProgressForm

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub OnClose(sender As System.Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim result As DialogResult = MessageBox.Show("Are you sure??", "Do you want to sopt?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        'e.CloseReason()
        e.Cancel = result = DialogResult.No
        'If result = DialogResult.No Then
        '    e.Cancel = True
        'End If

    End Sub
End Class