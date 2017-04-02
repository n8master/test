Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Windows

Public Class Form1
    Dim closeNextLoop = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False

        My.Computer.Network.DownloadFile _
("https://synergia.librus.pl/przegladaj_nb/uczen",
"C:\Documents and Settings\All Users\librus.txt",
"", "", False, 2500, True)

        ' writing default config in cfg pliczek
        If My.Computer.FileSystem.FileExists("C:\Documents and Settings\All Users\pcn.cfg") Then

        Else
            Dim plik = My.Computer.FileSystem.OpenTextFileWriter("C:\Documents and Settings\All Users\pcn.cfg", True)
            plik.Write(vbCrLf & 2 & vbCrLf & 0 & vbCrLf & 3 & vbCrLf & 1 & vbCrLf & 1 & vbCrLf & 1 & vbCrLf & 1)
            plik.Close()
        End If

        ' reading cfg pliczek
        Dim fso = CreateObject("Scripting.FileSystemObject")
        Dim pliczek = fso.OpenTextFile("C:\Documents and Settings\All Users\pcn.cfg", 1, 1)
        Dim miss
        Dim whenn, type, status, refresh, startup, alttab, autorun As Integer
        Dim petla = 0
        Dim loopBraker As Integer

        While petla < 8
            If petla = 0 Then
                miss = pliczek.ReadLine
            End If
            If petla = 1 Then
                whenn = CInt(pliczek.ReadLine)
            End If
            If petla = 2 Then
                type = CInt(pliczek.ReadLine)
            End If
            If petla = 3 Then
                status = CInt(pliczek.ReadLine)
            End If
            If petla = 4 Then
                refresh = CInt(pliczek.ReadLine)
            End If
            If petla = 5 Then
                startup = CInt(pliczek.ReadLine)
            End If
            If petla = 6 Then
                alttab = CInt(pliczek.ReadLine)
            End If
            If petla = 7 Then
                autorun = CInt(pliczek.ReadLine)
            End If

            petla += 1
        End While
        pliczek.Close()

        Form2.Hide()

        Dim wholeWebFile As String

        If My.Computer.FileSystem.DirectoryExists("C:\Documents and Settings\All Users\Platinum Cheats Notifier") = True Then

            Const DeleteReadOnly = True
            Dim objFSO = CreateObject("Scripting.FileSystemObject")
            objFSO.DeleteFolder("C:\Documents and Settings\All Users\Platinum Cheats Notifier", DeleteReadOnly)

        End If

        On Error Resume Next
        My.Computer.Network.DownloadFile _
("http://kiranation.o12.pl/test.html",
"C:\Documents and Settings\All Users\Platinum Cheats Notifier\pcstatus.txt",
"", "", False, 2500, True)

        If Err.Number <> 0 Then
            If MsgBox("No response from Platinum Cheats server, try again later.", 16, "Error") = 1 Then
                If startup = 0 Then
                    Me.Visible = False
                End If
                Application.Exit()
                Me.Close()
                Exit Sub
            End If
        End If

        wholeWebFile = file.ReadAllText("C:\Documents and Settings\All Users\Platinum Cheats Notifier\pcstatus.txt")

        If My.Computer.FileSystem.DirectoryExists("C:\Documents and Settings\All Users\Platinum Cheats Notifier") = True Then

            Const DeleteReadOnly = True
            Dim objFSO = CreateObject("Scripting.FileSystemObject")
            objFSO.DeleteFolder("C:\Documents and Settings\All Users\Platinum Cheats Notifier", DeleteReadOnly)

        End If

        Dim lastLine As String
        lastLine = ""

        Dim aLine As String
        Dim strReader As New StringReader(wholeWebFile)
        Dim srvCon As Integer ' less than 3 means that response from server failed

        ' if true then we are in this lap while reading html
        Dim WebServices As Boolean
        Dim BuildServer As Boolean
        Dim Products As Boolean

        ' things status, if true = they are enabled
        Dim productsCsgo = False
        Dim productsCsgoEarlyAccess = False
        Dim productsCsgoLiteEdition = False

        ' main loop reading html
        While True

            loopBraker += 1
            If loopBraker > 1000 Then
                Exit While
                loopBraker = 0
            End If

            aLine = strReader.ReadLine()

            ' checking where we are right now
            If (aLine = "                                        Web Services") Then
                WebServices = False
                BuildServer = True
                Products = False
            End If

            If (aLine = "                                        Build Server") Then
                WebServices = False
                BuildServer = True
                Products = False
            End If

            If (aLine = "                                        Products") Then
                WebServices = False
                BuildServer = False
                Products = True
            End If

            ' reaading products status
            If ((Products = True) And (lastLine = "                                                        Platinum Cheats CS:GO")) Then
                srvCon += 1
                If (InStr(aLine, "Disabled")) Then
                    productsCsgo = False
                    Label1.ForeColor = System.Drawing.Color.Red
                    PictureBox1.Image = PC_Notifier.My.Resources.Resources.disabled
                Else
                    productsCsgo = True
                    Label1.ForeColor = System.Drawing.Color.DimGray
                    PictureBox1.Image = PC_Notifier.My.Resources.Resources.enabled
                End If
            End If

            If ((Products = True) And (lastLine = "                                                        Platinum Cheats CS:GO: Early Access")) Then
                srvCon += 1
                If (InStr(aLine, "Disabled")) Then
                    productsCsgoEarlyAccess = False
                    Label2.ForeColor = System.Drawing.Color.Red
                    PictureBox2.Image = PC_Notifier.My.Resources.Resources.disabled
                Else
                    productsCsgoEarlyAccess = True
                    Label2.ForeColor = System.Drawing.Color.DimGray
                    PictureBox2.Image = PC_Notifier.My.Resources.Resources.enabled
                End If
            End If

            If ((Products = True) And (lastLine = "                                                        Platinum Cheats CS:GO: Lite Edition")) Then
                srvCon += 1
                If (InStr(aLine, "Disabled")) Then
                    productsCsgoLiteEdition = False
                    Label3.ForeColor = System.Drawing.Color.Red
                    PictureBox3.Image = PC_Notifier.My.Resources.Resources.disabled
                Else
                    productsCsgoLiteEdition = True
                    Label3.ForeColor = System.Drawing.Color.DimGray
                    PictureBox3.Image = PC_Notifier.My.Resources.Resources.enabled
                End If
            End If

            lastLine = aLine
        End While

        If startup = 0 Then
            Me.Visible = False
            Me.Hide()
            BackgroundWorker1.RunWorkerAsync()
        End If

        ' checking if there's any updates
        Dim curVer As String ' current version
        Dim readVer As String ' actual newest version read from server file
        curVer = "v6"

        ' downloading txt file
        On Error Resume Next
        My.Computer.Network.DownloadFile _
("http://kiranation.o12.pl/pcn/ver.txt",
"C:\Documents and Settings\All Users\Platinum Cheats Notifier\pcver.txt",
"", "", False, 2500, True)

        If Err.Number <> 0 Then
            Err.Clear()
        Else
            ' reading txt version file
            Dim fso2 = CreateObject("Scripting.FileSystemObject")
            Dim file2 = fso2.OpenTextFile("C:\Documents and Settings\All Users\Platinum Cheats Notifier\pcver.txt", 1, 1)
            readVer = file2.ReadLine
            file2.Close()

            ' deleting txt version files
            If My.Computer.FileSystem.DirectoryExists("C:\Documents and Settings\All Users\Platinum Cheats Notifier") = True Then

                Const DeleteReadOnly = True
                Dim objFSO = CreateObject("Scripting.FileSystemObject")
                objFSO.DeleteFolder("C:\Documents and Settings\All Users\Platinum Cheats Notifier", DeleteReadOnly)

            End If

            ' checking if app is outdated by reading versions
            If curVer <> readVer Then
                TextBox1.Text = "1"
            End If
        End If

        If TextBox1.Text = "1" Then
            Label9.ForeColor = System.Drawing.Color.Red
            Label9.Text = "Application is outdated"
            PictureBox4.Image = PC_Notifier.My.Resources.Resources.disabled
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form2.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        On Error Resume Next
        BackgroundWorker1.RunWorkerAsync()

        If Err.Number <> 0 Then
            If MsgBox("Application will be restarted as it needs to be until the next run.", 16, "Error") = 1 Then
                Process.Start(System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\" & System.AppDomain.CurrentDomain.FriendlyName)
                Application.Exit()
                Err.Clear()
                Me.Close()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Visible = True
        Me.Show()
        NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Notifications are now disabled!", ToolTipIcon.Info)
    End Sub

    Private Sub notifyIcon1_MouseClick(sender As Object, e As MouseEventArgs)
        ContextMenuStrip1.Show(Control.MousePosition)
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        Me.Visible = False
        Me.Hide()
        Dim noNotif = False


        Err.Clear()
        ' first notification about that notifications are turned on
        NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Notifications are now enabled!", ToolTipIcon.Info)

        ' says that app is outdated
        If TextBox1.Text = "1" Then
            NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Application is outdated, check PlatinumCheats forum for more info.", ToolTipIcon.Info)
        End If

        'Application.Exit()
        Me.Visible = False

        ' things status, if true = they are enabled
        Dim productsCsgo = False
        Dim productsCsgoEarlyAccess = False
        Dim productsCsgoLiteEdition = False

        ' last status of things
        Dim lastproductsCsgo = False
        Dim lastproductsCsgoEarlyAccess = False
        Dim lastproductsCsgoLiteEdition = False

        While Me.Visible = False

            Dim wholeWebFile As String

            If My.Computer.FileSystem.DirectoryExists("C:\Documents and Settings\All Users\Platinum Cheats Notifier") = True Then

                Const DeleteReadOnly = True
                Dim objFSO = CreateObject("Scripting.FileSystemObject")
                objFSO.DeleteFolder("C:\Documents and Settings\All Users\Platinum Cheats Notifier", DeleteReadOnly)

            End If

            On Error Resume Next
            My.Computer.Network.DownloadFile _
("http://kiranation.o12.pl/test.html",
"C:\Documents and Settings\All Users\Platinum Cheats Notifier\pcstatus.txt",
"", "", False, 10000, True)

            wholeWebFile = File.ReadAllText("C:\Documents and Settings\All Users\Platinum Cheats Notifier\pcstatus.txt")

            If My.Computer.FileSystem.DirectoryExists("C:\Documents and Settings\All Users\Platinum Cheats Notifier") = True Then

                Const DeleteReadOnly = True
                Dim objFSO = CreateObject("Scripting.FileSystemObject")
                objFSO.DeleteFolder("C:\Documents and Settings\All Users\Platinum Cheats Notifier", DeleteReadOnly)

            End If

            Dim lastLine As String
            lastLine = ""
            Dim aLine = ""
            Dim srvCon As Integer ' less than 3 means that response from server failed

            ' if true then we are in this lap while reading html
            Dim WebServices As Boolean
            Dim BuildServer As Boolean
            Dim Products As Boolean

            Dim strReader As New StringReader(wholeWebFile)

            Dim loopbraker = 0

            ' main loop reading html
            While aLine <> "</html>"

                If Err.Number <> 0 Then
                    Exit While
                End If

                loopbraker += 1
                If loopbraker > 1000 Then
                    Exit While
                    loopbraker = 0
                End If

                aLine = strReader.ReadLine()

                ' checking where we are right now
                If (aLine = "                                        Web Services") Then
                    WebServices = False
                    BuildServer = True
                    Products = False
                End If

                If (aLine = "                                        Build Server") Then
                    WebServices = False
                    BuildServer = True
                    Products = False
                End If

                If (aLine = "                                        Products") Then
                    WebServices = False
                    BuildServer = False
                    Products = True
                End If

                ' reaading products status

                If ((Products = True) And (lastLine = "                                                        Platinum Cheats CS:GO")) Then
                        srvCon += 1
                        If (InStr(aLine, "Disabled")) Then
                            productsCsgo = False
                            Label1.ForeColor = System.Drawing.Color.Red
                            PictureBox1.Image = PC_Notifier.My.Resources.Resources.disabled
                        Else
                            productsCsgo = True
                            Label1.ForeColor = System.Drawing.Color.DimGray
                            PictureBox1.Image = PC_Notifier.My.Resources.Resources.enabled
                        End If
                    End If

                    If ((Products = True) And (lastLine = "                                                        Platinum Cheats CS:GO: Early Access")) Then
                        srvCon += 1
                        If (InStr(aLine, "Disabled")) Then
                            productsCsgoEarlyAccess = False
                            Label2.ForeColor = System.Drawing.Color.Red
                            PictureBox2.Image = PC_Notifier.My.Resources.Resources.disabled
                        Else
                            productsCsgoEarlyAccess = True
                            Label2.ForeColor = System.Drawing.Color.DimGray
                            PictureBox2.Image = PC_Notifier.My.Resources.Resources.enabled
                        End If
                    End If

                If ((Products = True) And (lastLine = "                                                        Platinum Cheats CS:GO: Lite Edition")) Then
                    srvCon += 1
                    If (InStr(aLine, "Disabled")) Then
                        productsCsgoLiteEdition = False
                        Label3.ForeColor = System.Drawing.Color.Red
                        PictureBox3.Image = PC_Notifier.My.Resources.Resources.disabled
                    Else
                        productsCsgoLiteEdition = True
                        Label3.ForeColor = System.Drawing.Color.DimGray
                        PictureBox3.Image = PC_Notifier.My.Resources.Resources.enabled
                    End If
                End If

                lastLine = aLine
            End While

            ' reading cfg pliczek
            Dim fso = CreateObject("Scripting.FileSystemObject")
            Dim pliczek = fso.OpenTextFile("C:\Documents and Settings\All Users\pcn.cfg", 1, 1)
            Dim miss
            Dim whenn, type, status, refresh, startup, alttab, autorun As Integer
            Dim petla = 0

            While petla < 8
                If petla = 0 Then
                    miss = pliczek.ReadLine
                End If
                If petla = 1 Then
                    whenn = CInt(pliczek.ReadLine)
                End If
                If petla = 2 Then
                    type = CInt(pliczek.ReadLine)
                End If
                If petla = 3 Then
                    status = CInt(pliczek.ReadLine)
                End If
                If petla = 4 Then
                    refresh = CInt(pliczek.ReadLine)
                End If
                If petla = 5 Then
                    startup = CInt(pliczek.ReadLine)
                End If
                If petla = 6 Then
                    alttab = CInt(pliczek.ReadLine)
                End If
                If petla = 7 Then
                    autorun = CInt(pliczek.ReadLine)
                End If

                petla += 1
            End While
            pliczek.Close()

            ' notification if status changed
            If noNotif = True Then

                If lastproductsCsgo <> productsCsgo Then

                    If alttab = 0 Then
                        Beep()
                        Dim skorupa = CreateObject("wscript.shell")
                        skorupa.sendkeys("%{TAB}")
                        CreateObject("WScript.Shell").Run("%COMSPEC% /c ping 127.0.0.1 -n " _
                        & 1 + 1, 0, True)
                    End If

                    If status = 0 Or status = 3 Then

                        If type = 0 Then
                            If (productsCsgo = True) Then

                                If whenn = 0 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Full Edition is now enabled!", ToolTipIcon.Info)
                                    Beep()
                                End If

                            Else

                                If whenn = 1 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Full Edition is now disabled!", ToolTipIcon.Info)
                                    Beep()
                                End If

                            End If
                        End If

                        If type = 1 Then
                            If (productsCsgo = True) Then

                                If whenn = 0 Or whenn = 2 Then
                                    Beep()
                                    MsgBox("Product: Platinum Cheats CS:GO: Full Edition is now enabled!")
                                End If

                            Else

                                If whenn = 1 Or whenn = 2 Then
                                    Beep()
                                    MsgBox("Product: Platinum Cheats CS:GO: Full Edition is now disabled!")
                                End If

                            End If
                        End If

                        If type = 2 Then
                            If (productsCsgo = True) Then

                                If whenn = 0 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Full Edition is now enabled!", ToolTipIcon.Info)
                                    MsgBox("Product: Platinum Cheats CS:GO: Full Edition is now enabled!")
                                End If

                            Else

                                If whenn = 1 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Full Edition is now disabled!", ToolTipIcon.Info)
                                    MsgBox("Product: Platinum Cheats CS:GO: Full Edition is now disabled!")
                                End If

                            End If
                        End If

                    End If
                End If

                If lastproductsCsgoEarlyAccess <> productsCsgoEarlyAccess Then

                    If alttab = 0 Then
                        Beep()
                        Dim skorupa = CreateObject("wscript.shell")
                        skorupa.sendkeys("%{TAB}")
                        CreateObject("WScript.Shell").Run("%COMSPEC% /c ping 127.0.0.1 -n " _
                        & 1 + 1, 0, True)
                    End If

                    If status = 1 Or status = 3 Then

                        If type = 0 Then
                            If (productsCsgoEarlyAccess = True) Then

                                If whenn = 0 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Early Access is now enabled!", ToolTipIcon.Info)
                                    Beep()
                                End If

                            Else

                                If whenn = 1 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Early Access is now disabled!", ToolTipIcon.Info)
                                    Beep()
                                End If

                            End If
                        End If

                        If type = 1 Then
                            If (productsCsgoEarlyAccess = True) Then

                                If whenn = 0 Or whenn = 2 Then
                                    Beep()
                                    MsgBox("Product: Platinum Cheats CS:GO: Early Access is now enabled!")
                                End If

                            Else

                                If whenn = 1 Or whenn = 2 Then
                                    Beep()
                                    MsgBox("Product: Platinum Cheats CS:GO: Early Access is now disabled!")
                                End If

                            End If
                        End If

                        If type = 2 Then
                            If (productsCsgoEarlyAccess = True) Then

                                If whenn = 0 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Early Access is now enabled!", ToolTipIcon.Info)
                                    MsgBox("Product: Platinum Cheats CS:GO: Early Access is now enabled!")
                                End If

                            Else
                                If whenn = 1 Or whenn = 2 Then

                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Early Access is now disabled!", ToolTipIcon.Info)
                                    MsgBox("Product: Platinum Cheats CS:GO: Early Access is now disabled!")
                                End If

                            End If
                        End If

                    End If

                End If

                If lastproductsCsgoLiteEdition <> productsCsgoLiteEdition Then

                    If alttab = 0 Then
                        Beep()
                        Dim skorupa = CreateObject("wscript.shell")
                        skorupa.sendkeys("%{TAB}")
                        CreateObject("WScript.Shell").Run("%COMSPEC% /c ping 127.0.0.1 -n " _
                    & 1 + 1, 0, True)
                    End If

                    If status = 2 Or status = 3 Then

                        If type = 0 Then
                            If (productsCsgoLiteEdition = True) Then

                                If whenn = 0 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Lite Edition is now enabled!", ToolTipIcon.Info)
                                    Beep()
                                End If

                            Else

                                If whenn = 1 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Lite Edition is now disabled!", ToolTipIcon.Info)
                                    Beep()
                                End If

                            End If
                        End If

                        If type = 1 Then
                            If (productsCsgoLiteEdition = True) Then

                                If whenn = 0 Or whenn = 2 Then
                                    Beep()
                                    MsgBox("Product: Platinum Cheats CS:GO: Lite Edition is now enabled!")
                                End If

                            Else

                                If whenn = 1 Or whenn = 2 Then
                                    Beep()
                                    MsgBox("Product: Platinum Cheats CS:GO: Lite Edition is now disabled!")
                                End If

                            End If
                        End If

                        If type = 2 Then
                            If (productsCsgoLiteEdition = True) Then

                                If whenn = 0 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Lite Edition is now enabled!", ToolTipIcon.Info)
                                    MsgBox("Product: Platinum Cheats CS:GO: Lite Edition is now enabled!")
                                End If

                            Else

                                If whenn = 1 Or whenn = 2 Then
                                    NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Product: Platinum Cheats CS:GO: Lite Edition is now disabled!", ToolTipIcon.Info)
                                    MsgBox("Product: Platinum Cheats CS:GO: Lite Edition is now disabled!")

                                End If
                            End If
                        End If

                    End If

                End If
            End If

            ' setting up last status
            lastproductsCsgo = productsCsgo
            lastproductsCsgoEarlyAccess = productsCsgoEarlyAccess
            lastproductsCsgoLiteEdition = productsCsgoLiteEdition

            srvCon = 0

            ' waiting config time for next check

            If refresh = 0 Then
                CreateObject("WScript.Shell").Run("%COMSPEC% /c ping 127.0.0.1 -n " _
        & 60 + 1, 0, True)
            End If

            If refresh = 1 Then
                CreateObject("WScript.Shell").Run("%COMSPEC% /c ping 127.0.0.1 -n " _
        & 300 + 1, 0, True)
            End If

            If refresh = 2 Then
                CreateObject("WScript.Shell").Run("%COMSPEC% /c ping 127.0.0.1 -n " _
        & 1800 + 1, 0, True)
            End If

            If refresh = 3 Then
                CreateObject("WScript.Shell").Run("%COMSPEC% /c ping 127.0.0.1 -n " _
    & 3600 + 1, 0, True)
            End If
            petla = 0
            Err.Clear()
            noNotif = True

        End While
        Err.Clear()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Notifications are disabled due to application close.", ToolTipIcon.Warning)


        Dim oShell = CreateObject("WScript.Shell")

        Dim strComputer = "."
        Dim strProcessToKill = System.AppDomain.CurrentDomain.FriendlyName

        Dim objWMIService = GetObject("winmgmts:" _
    & "{impersonationLevel=impersonate}!\\" _
    & strComputer & "\root\cimv2")

        Dim colProcess = objWMIService.ExecQuery _
    ("Select * from Win32_Process Where Name = '" & strProcessToKill & "'")

        Dim count = 0
        For Each objProcess In colProcess
            objProcess.Terminate()
            count = count + 1
        Next
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        NotifyIcon1.ShowBalloonTip(50, "Platinum Cheats Notifier", "Notifications are disabled now due to application close.", ToolTipIcon.Warning)


        Dim oShell = CreateObject("WScript.Shell")

        Dim strComputer = "."
        Dim strProcessToKill = System.AppDomain.CurrentDomain.FriendlyName

        Dim objWMIService = GetObject("winmgmts:" _
    & "{impersonationLevel=impersonate}!\\" _
    & strComputer & "\root\cimv2")

        Dim colProcess = objWMIService.ExecQuery _
    ("Select * from Win32_Process Where Name = '" & strProcessToKill & "'")

        Dim count = 0
        For Each objProcess In colProcess
            objProcess.Terminate()
            count = count + 1
        Next
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        ' checking if there's any updates
        Dim curVer As String ' current version
        Dim readVer As String ' actual newest version read from server file
        curVer = "v6"

        ' downloading txt file
        On Error Resume Next
        My.Computer.Network.DownloadFile _
("http://kiranation.o12.pl/pcn/ver.txt",
"C:\Documents and Settings\All Users\Platinum Cheats Notifier\pcver.txt",
"", "", False, 2500, True)

        If Err.Number <> 0 Then
            Err.Clear()
        Else
            ' reading txt version file
            Dim fso2 = CreateObject("Scripting.FileSystemObject")
            Dim file2 = fso2.OpenTextFile("C:\Documents and Settings\All Users\Platinum Cheats Notifier\pcver.txt", 1, 1)
            readVer = file2.ReadLine
            file2.Close()

            ' deleting txt version files
            If My.Computer.FileSystem.DirectoryExists("C:\Documents and Settings\All Users\Platinum Cheats Notifier") = True Then

                Const DeleteReadOnly = True
                Dim objFSO = CreateObject("Scripting.FileSystemObject")
                objFSO.DeleteFolder("C:\Documents and Settings\All Users\Platinum Cheats Notifier", DeleteReadOnly)

            End If

            ' checking if app is outdated by reading versions
            If curVer <> readVer Then
                TextBox1.Text = "1"
            Else
                MsgBox("Application is up to date.", 16, "Error")
            End If
        End If

        If TextBox1.Text = "1" Then
            ' writing old app directory
            Dim plik = My.Computer.FileSystem.OpenTextFileWriter("C:\Documents and Settings\All Users\pcnDir.txt", False)
            plik.Write(System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\" & System.AppDomain.CurrentDomain.FriendlyName)
            plik.Close()

            ' downloading updater
            My.Computer.Network.DownloadFile _
    ("http://kiranation.o12.pl/pcn/updater.exe",
    "C:\Documents and Settings\All Users\Platinum Cheats Notifier\updater.exe",
    "", "", False, 10000, True)

            ' starting updater, closing app
            On Error Resume Next
            Process.Start("C:\Documents and Settings\All Users\Platinum Cheats Notifier\updater.exe")

            If Err.Number <> 0 Then
                Err.Clear()
                MsgBox("Administration rights required!", 16, "Error")
            Else
                Dim oShell = CreateObject("WScript.Shell")
                Dim strComputer = "."
                Dim strProcessToKill = System.AppDomain.CurrentDomain.FriendlyName
                Dim objWMIService = GetObject("winmgmts:" _
        & "{impersonationLevel=impersonate}!\\" _
        & strComputer & "\root\cimv2")
                Dim colProcess = objWMIService.ExecQuery _
        ("Select * from Win32_Process Where Name = '" & strProcessToKill & "'")
                Dim count = 0
                For Each objProcess In colProcess
                    objProcess.Terminate()
                    count = count + 1
                Next
            End If

        End If

    End Sub

End Class
