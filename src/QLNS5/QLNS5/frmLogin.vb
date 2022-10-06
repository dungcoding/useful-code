Public Class frmLogin
    Dim check As Integer = 3
    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call CenterToScreen()
    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Me.Close()
    End Sub

    Private Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If txtLogin.Text = "" Then
            MsgBox("Vui lòng nhập mật khẩu.", MsgBoxStyle.Exclamation, Title:="Thông báo")
        ElseIf txtPass.Text = "" Then
            MsgBox("Vui lòng nhập tài khoản.", MsgBoxStyle.Exclamation, Title:="Thông báo")
        Else
            If txtLogin.Text = "admin" And txtPass.Text = "admin" Then
                Me.Hide()
                Form1.Show()
            Else
                MsgBox("Tài Khoản hoặc Mật Khẩu không đúng. Vui lòng thử lại !" & vbNewLine & "Bạn còn " & check - 1 & " lần.", MsgBoxStyle.Exclamation, Title:="Thông báo")
                check -= 1
            End If
            If check = 0 Then
                Me.Close()
            End If
        End If

    End Sub
End Class
