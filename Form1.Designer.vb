Imports Microsoft.WindowsAPICodePack.Shell
Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.LayerButton = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.Fuke = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Size = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Modified = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LayerListContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusBar2 = New System.Windows.Forms.StatusBar()
        Me.OpenBaseFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.StartCompress = New System.Windows.Forms.Button()
        Me.FileSizeText = New System.Windows.Forms.Label()
        Me.LayerListContextMenu.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button1.Location = New System.Drawing.Point(12, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Open base"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'LayerButton
        '
        Me.LayerButton.Enabled = False
        Me.LayerButton.Location = New System.Drawing.Point(93, 12)
        Me.LayerButton.Name = "LayerButton"
        Me.LayerButton.Size = New System.Drawing.Size(75, 23)
        Me.LayerButton.TabIndex = 1
        Me.LayerButton.Text = "Add Layer"
        Me.LayerButton.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Fuke, Me.Size, Me.Modified})
        Me.ListView1.ContextMenuStrip = Me.LayerListContextMenu
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListView1.LabelEdit = True
        Me.ListView1.Location = New System.Drawing.Point(9, 38)
        Me.ListView1.Margin = New System.Windows.Forms.Padding(0)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ListView1.Size = New System.Drawing.Size(321, 163)
        Me.ListView1.TabIndex = 2
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'Fuke
        '
        Me.Fuke.Text = "Name"
        Me.Fuke.Width = 128
        '
        'Size
        '
        Me.Size.Text = "Creation Date"
        Me.Size.Width = 89
        '
        'Modified
        '
        Me.Modified.Text = "Modification Date"
        Me.Modified.Width = 96
        '
        'LayerListContextMenu
        '
        Me.LayerListContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripMenuItem2, Me.ToolStripMenuItem3})
        Me.LayerListContextMenu.Name = "LayerListContextMenu"
        Me.LayerListContextMenu.Size = New System.Drawing.Size(138, 70)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(137, 22)
        Me.ToolStripMenuItem1.Text = "Remove"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(137, 22)
        Me.ToolStripMenuItem2.Text = "Move up"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(137, 22)
        Me.ToolStripMenuItem3.Text = "Move down"
        '
        'StatusBar2
        '
        Me.StatusBar2.Location = New System.Drawing.Point(0, 0)
        Me.StatusBar2.Name = "StatusBar2"
        Me.StatusBar2.Size = New System.Drawing.Size(100, 22)
        Me.StatusBar2.TabIndex = 0
        '
        'OpenBaseFileDialog
        '
        Me.OpenBaseFileDialog.FileName = "OpenFileDialog1"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(691, 237)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(315, 199)
        Me.TabControl1.TabIndex = 6
        Me.TabControl1.Tag = ""
        '
        'TabPage1
        '
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(307, 173)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Pack"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(307, 173)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Unpack"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'StartCompress
        '
        Me.StartCompress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StartCompress.Enabled = False
        Me.StartCompress.Location = New System.Drawing.Point(252, 204)
        Me.StartCompress.Name = "StartCompress"
        Me.StartCompress.Size = New System.Drawing.Size(75, 23)
        Me.StartCompress.TabIndex = 7
        Me.StartCompress.Text = "Compress"
        Me.StartCompress.UseVisualStyleBackColor = True
        '
        'FileSizeText
        '
        Me.FileSizeText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FileSizeText.Location = New System.Drawing.Point(174, 12)
        Me.FileSizeText.Name = "FileSizeText"
        Me.FileSizeText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FileSizeText.Size = New System.Drawing.Size(156, 23)
        Me.FileSizeText.TabIndex = 9
        Me.FileSizeText.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(339, 239)
        Me.Controls.Add(Me.FileSizeText)
        Me.Controls.Add(Me.StartCompress)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.LayerButton)
        Me.Name = "Form1"
        Me.Text = "Differential Compression"
        Me.LayerListContextMenu.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents LayerButton As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents Fuke As System.Windows.Forms.ColumnHeader
    Friend Shadows WithEvents Size As System.Windows.Forms.ColumnHeader
    Friend WithEvents OpenBaseFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents StatusBar2 As System.Windows.Forms.StatusBar
    Friend WithEvents StartCompress As System.Windows.Forms.Button
    Friend WithEvents Modified As System.Windows.Forms.ColumnHeader
    Friend WithEvents LayerListContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileSizeText As System.Windows.Forms.Label

End Class
