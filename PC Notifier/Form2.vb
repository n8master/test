Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Windows

Public Class Form2

    Dim counter

    Public Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If My.Computer.FileSystem.FileExists("C:\Users\Kai\Documents\Platinum Cheats Notifier\autorun.cfg") Then
            Dim autorunCfg = ""
            autorunCfg = File.ReadAllText("C:\Users\Kai\Documents\Platinum Cheats Notifier\autorun.cfg")

            If InStr(autorunCfg, "1") Then
                ComboBox7.SelectedIndex = 0
            Else
                ComboBox7.SelectedIndex = 1
            End If

        Else
            ComboBox7.SelectedIndex = 1
        End If

        ' setting config from cfg file1
        If My.Computer.FileSystem.FileExists("C:\Documents and Settings\All Users\pcn.cfg") Then
            Dim fso1 = CreateObject("Scripting.FileSystemObject")
            Dim file1 = fso1.OpenTextFile("C:\Documents and Settings\All Users\pcn.cfg", 1, 1)
            Dim miss
            Dim petla = 0

            While petla < 8
                If petla = 0 Then
                    miss = file1.ReadLine
                End If
                If petla = 1 Then
                    ComboBox1.SelectedIndex = CInt(file1.ReadLine)
                End If
                If petla = 2 Then
                    ComboBox2.SelectedIndex = CInt(file1.ReadLine)
                End If
                If petla = 3 Then
                    ComboBox3.SelectedIndex = CInt(file1.ReadLine)
                End If
                If petla = 4 Then
                    ComboBox4.SelectedIndex = CInt(file1.ReadLine)
                End If
                If petla = 5 Then
                    ComboBox5.SelectedIndex = CInt(file1.ReadLine)
                End If
                If petla = 6 Then
                    ComboBox6.SelectedIndex = CInt(file1.ReadLine)
                End If
                If petla = 7 Then
                    'ComboBox7.SelectedIndex = CInt(file1.ReadLine)
                End If

                petla += 1
            End While

            file1.Close()
        Else
            ComboBox1.SelectedIndex = 2
            ComboBox2.SelectedIndex = 2
            ComboBox3.SelectedIndex = 3
            ComboBox4.SelectedIndex = 1
            ComboBox5.SelectedIndex = 1
            ComboBox6.SelectedIndex = 1
            ComboBox7.SelectedIndex = 1
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        ' delete old config file
        If My.Computer.FileSystem.FileExists("C:\Documents and Settings\All Users\pcn.cfg") Then
            My.Computer.FileSystem.DeleteFile("C:\Documents and Settings\All Users\pcn.cfg")
        End If

        ' writing config info to cfg file
        Dim plik = My.Computer.FileSystem.OpenTextFileWriter("C:\Documents and Settings\All Users\pcn.cfg", True)
        plik.Write(vbCrLf & ComboBox1.SelectedIndex & vbCrLf & ComboBox2.SelectedIndex & vbCrLf & ComboBox3.SelectedIndex & vbCrLf & ComboBox4.SelectedIndex & vbCrLf & ComboBox5.SelectedIndex & vbCrLf & ComboBox6.SelectedIndex & vbCrLf & ComboBox7.SelectedIndex)
        plik.Close()

        ' closing form
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        NotifyIcon1.Visible = True

        If ComboBox2.SelectedIndex = 0 Then
            NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "This is notification test.", ToolTipIcon.Info)
            Beep()
        End If

        If ComboBox2.SelectedIndex = 1 Then
            Beep()
            MsgBox("This is notification test.")
        End If

        If ComboBox2.SelectedIndex = 2 Then
            NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "This is notification test.", ToolTipIcon.Info)
            MsgBox("This is notification test.")
        End If

        NotifyIcon1.Visible = False

    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox7.SelectedIndexChanged

        If TextBox1.Text = "xd" Then

            If ComboBox7.SelectedIndex = 0 Then

                If My.Computer.FileSystem.FileExists(System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\" & "add_autorun.exe") Then
                    On Error Resume Next
                    Process.Start(System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\" & "add_autorun.exe")

                    If Err.Number <> 0 Then
                        Err.Clear()
                        MsgBox("Administration rights required!", 16, "Error")
                    Else
                        Application.Exit()
                        Me.Close()
                        MsgBox("test")
                    End If

                Else
                    If MsgBox("add_autorun.exe was not found.", 16, "Error") = 1 Then
                        Application.Exit()
                        Me.Close()
                    End If
                End If

            End If

            If ComboBox7.SelectedIndex = 1 Then

                If My.Computer.FileSystem.FileExists(System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\" & "remove_autorun.exe") Then
                    On Error Resume Next
                    Process.Start(System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\" & "remove_autorun.exe")

                    If Err.Number <> 0 Then
                        Err.Clear()
                        MsgBox("Administration rights required!", 16, "Error")
                    Else
                        Application.Exit()
                        Me.Close()
                    End If

                Else
                    If MsgBox("remove_autorun.exe was not found.", 16, "Error") = 1 Then
                        Application.Exit()
                        Me.Close()
                    End If
                End If

            End If
        End If

        TextBox1.Text = "xd"
    End Sub

End Class