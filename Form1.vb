Imports Microsoft.WindowsAPICodePack.Shell
Imports System.Runtime.InteropServices
Imports ManagedLDiff
Imports System.IO
Imports Lasse.ManagedShellIcons
Imports System.Windows.Forms
Imports System.ComponentModel

Public Class Form1
    Private _layers As List(Of String)
    Private _diffTool As LDiffTool
    Private _images As ImageList
    Private _baseSize As Integer
    Private Sub ListView1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub

    Public Sub New()
        InitializeComponent()
        _layers = New List(Of String)
        _images = New ImageList()
        ListView1.SmallImageList = _images
        ListView1.StateImageList = _images
        ListView1.LargeImageList = _images
        SetWindowTheme(ListView1.Handle, "Explorer", Nothing)
        _baseSize = -1
    End Sub

    <DllImport("uxtheme.dll", ExactSpelling:=True, CharSet:=CharSet.Unicode)>
    Private Shared Function SetWindowTheme(ByVal hwnd As IntPtr, ByVal pszSubAppName As String, ByVal pszSubIdList As String) As Integer
    End Function

    Private Sub AddLayer(sender As System.Object, e As System.EventArgs) Handles LayerButton.Click
        Dim dialog = New OpenFileDialog()
        dialog.ShowDialog()

        If dialog.FileName IsNot Nothing Then : AddFileToList(dialog.FileName)
        End If
    End Sub

    Private Sub OpenBase_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim dialog = New OpenFileDialog()
        dialog.ShowDialog()
        Dim fileName = dialog.FileName
        If fileName IsNot Nothing Then
            Try
                _diffTool = New LDiffTool(fileName)
                AddFileToList(fileName, True)
                LayerButton.Enabled = True
            Catch ex As Exception
                ShowException(ex)
            End Try
        End If
    End Sub

    Private Sub ShowException(ex As Exception)
        MessageBox.Show(ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub AddFileToList(fileName As String, Optional baseFile As Boolean = False)
        Try
            If baseFile Then : AddItemToList(fileName, 0)
            Else : AddItemToList(fileName)
            End If
        Catch ex As Exception : MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub AddItemToList(fileName As String, Optional index As Integer = -1)
        Try
            If index >= 0 And index < _layers.Count Then : _layers(index) = fileName
            Else : _layers.Add(fileName)
            End If
            RefreshList()
        Catch : Throw
        End Try
    End Sub

    Private Sub RefreshList()
        Try
            If _layers.Count = 0 Then
                LayerButton.Enabled = False
                _diffTool.Close()
                FileSizeText.Text = ""
                _baseSize = -1
            End If
            StartCompress.Enabled = _layers.Count > 1
            ListView1.Items.Clear()
            Dim index = 0
            _layers.ForEach(Sub(path As String)
                                Dim fileInfo = New FileInfo(path)
                                Dim listViewItem = New ListViewItem({fileInfo.Name, fileInfo.CreationTime, fileInfo.LastWriteTime})
                                listViewItem.IndentCount = 0
                                If index = 0 Then
                                    listViewItem.BackColor = Color.AliceBlue
                                    _baseSize = fileInfo.Length
                                    FileSizeText.Text = FormatByteSize(fileInfo.Length)
                                Else
                                    If Not _baseSize = fileInfo.Length Then : Throw New InvalidDataException("The stacked file is not the same size as the base file. This is not yet supported!")
                                    End If
                                    Dim icon As Icon = ShellIconManager.GetFileIcon(fileInfo.FullName, FileIconSize.Small)
                                    _images.Images.Add(icon)
                                    listViewItem.ImageIndex = _images.Images.Count - 1
                                    ListView1.Items.Add(listViewItem)
                                    index += 1
                            End Sub)
        Catch : Throw
        End Try
    End Sub

    Private Function FormatByteSize(size As Long, Optional biBytes As Boolean = False)
        Dim threshold = IIf(Not biBytes, 1000, 1024)
        Dim sizeNames As String() = IIf(Not biBytes, {"bytes", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"}, {"bytes", "kiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"})
        Dim i = IIf(size = 0, 0, Math.Floor(Math.Log(size) / Math.Log(threshold)))
        Return Math.Round(size / Math.Pow(threshold, i), 2).ToString() + " " + sizeNames(i)
    End Function

    Private Sub MenuItem5_Click(sender As System.Object, e As System.EventArgs)
        AboutBox.Show()
    End Sub

    Private Sub StartCompress_Click(sender As System.Object, e As System.EventArgs) Handles StartCompress.Click
        StartCompression()
    End Sub

    Private Sub StartCompression()
        ProgressForm.ShowDialog(Me)
    End Sub

    Private Sub Right_click(o As Object, e As CancelEventArgs) Handles LayerListContextMenu.Opening
        e.Cancel = ListView1.SelectedItems.Count <= 0
        'Dim a = e.Button = MouseButtons.Right
        'If a Then
        '    LayerListContextMenu.Show()
        'End If
    End Sub

    Private Sub RemoveItemFromList(a As Object, e As ToolStripItemClickedEventArgs) Handles LayerListContextMenu.ItemClicked
        For index = 0 To LayerListContextMenu.Items.Count - 1
            If e.ClickedItem Is LayerListContextMenu.Items(index) Then
                If index = 0 Then
                    RemoveSelectedListItems()
                End If
            End If
        Next
    End Sub

    Private Sub RemoveSelectedListItems()
        Dim list = New List(Of String)
        For Each item In ListView1.SelectedItems
            list.Add(_layers.ElementAt(ListView1.Items.IndexOf(item)))
        Next
        For Each item In list
            _layers.Remove(item)
        Next
        RefreshList()
    End Sub

    Private Sub ListDeleteEventHandler(o As Object, e As KeyEventArgs) Handles ListView1.KeyUp
        If e.KeyCode = Keys.Delete Then
            RemoveSelectedListItems()
        End If
    End Sub

    'Private Sub CloseForm() Handles MenuItem2.Click
    '    Close()
    'End Sub
End Class
