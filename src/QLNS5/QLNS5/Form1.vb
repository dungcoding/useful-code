Option Explicit On
Option Strict On
Imports System.Data.OleDb
Imports System.IO
Module modConnection
    Public provider As String
    Public dataFile As String
    Public connString As String
    Public myConnection As OleDbConnection = New OleDbConnection
    Public Sub connection()
        provider = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
        dataFile = "|DataDirectory|Database\QLNS.accdb"
        connString = provider & dataFile
        myConnection.ConnectionString = connString
        myConnection.Open()
    End Sub
End Module
Public Class Form1
    Dim cmd As OleDbCommand
    Private mRow As Integer = 0
    Private newpage As Boolean = True
    Private Sub NhanVienBindingNavigatorSaveItem_Click(sender As Object, e As EventArgs) Handles NhanVienBindingNavigatorSaveItem.Click
        Me.Validate()
        Me.NhanVienBindingSource.EndEdit()
        Me.TableAdapterManager.UpdateAll(Me.QLNSDataSet)
    End Sub

    Private Sub RefreshData()
        Try
            Me.NhanVienBindingSource.Filter = Nothing
            Me.TienLuongBindingSource.Filter = Nothing
            Me.NhanVienTableAdapter.Fill(Me.QLNSDataSet.nhanVien)
            Me.TienLuongTableAdapter.Fill(Me.QLNSDataSet.tienLuong)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call CenterToScreen()
        tabThemNhanVien.Show()
        tabSuaXoa.Hide()
        tabTimKiem.Hide()
        tabLuong.Hide()
        tabPhongBan.Hide()
        tabChucVu.Hide()
        tabLoaiHopDong.Hide()
        tabTienLuong.Hide()
        tabPreview.Hide()
        'TODO: This line of code loads data into the 'QLNSDataSet.loaiNhanVien' table. You can move, or remove it, as needed.
        Me.LoaiNhanVienTableAdapter.Fill(Me.QLNSDataSet.loaiNhanVien)
        'TODO: This line of code loads data into the 'QLNSDataSet.chucVu' table. You can move, or remove it, as needed.
        Me.ChucVuTableAdapter.Fill(Me.QLNSDataSet.chucVu)
        'TODO: This line of code loads data into the 'QLNSDataSet.phongBan' table. You can move, or remove it, as needed.
        Me.PhongBanTableAdapter.Fill(Me.QLNSDataSet.phongBan)
        'TODO: This line of code loads data into the 'QLNSDataSet.tienLuong' table. You can move, or remove it, as needed.
        Me.TienLuongTableAdapter.Fill(Me.QLNSDataSet.tienLuong)
        'TODO: This line of code loads data into the 'QLNSDataSet.nhanVien' table. You can move, or remove it, as needed.
        Me.NhanVienTableAdapter.Fill(Me.QLNSDataSet.nhanVien)

    End Sub
    Private Sub BtnThemMoi_Click(sender As Object, e As EventArgs) Handles btnThemMoi.Click
        BindingNavigatorAddNewItem.PerformClick()
        NhanVienBindingSource.AddNew()
    End Sub
    Private Sub BtnLuu_Click(sender As Object, e As EventArgs) Handles btnLuu.Click
        NhanVienBindingNavigatorSaveItem.PerformClick()
        Try
            Me.Validate()
            Me.NhanVienBindingSource.EndEdit()
            NhanVienTableAdapter.Update(QLNSDataSet.nhanVien)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnSua1_Click(sender As Object, e As EventArgs) Handles btnSua1.Click
        NhanVienBindingNavigatorSaveItem.PerformClick()
        Try
            Me.Validate()
            Me.NhanVienBindingSource.EndEdit()
            NhanVienTableAdapter.Update(QLNSDataSet.nhanVien)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub BtnXoa1_Click(sender As Object, e As EventArgs) Handles btnXoa1.Click
        BindingNavigatorDeleteItem.PerformClick()
        Try
            Me.Validate()
            NhanVienBindingSource.RemoveCurrent()
            Me.NhanVienBindingSource.EndEdit()
            NhanVienTableAdapter.Update(QLNSDataSet.nhanVien)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub BtnTim1_Click(sender As Object, e As EventArgs) Handles btnTim1.Click
        If myConnection.State = ConnectionState.Open Then
            myConnection.Close()
        Else
            connection()
        End If

        If radTimTatCaNhanVien1.Checked = True Then
            Dim dt As New DataTable
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            Dim key As New OleDbDataAdapter
            key = New OleDbDataAdapter("Select * from nhanVien", myConnection)
            key.Fill(dt)

            DataGridView2.DataSource = dt.DefaultView
            myConnection.Close()
        ElseIf radTimTheoMaNhanVien1.Checked = True Then
            Dim dt As New DataTable
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            Dim key As New OleDbDataAdapter
            key = New OleDbDataAdapter("Select * from nhanVien where maNV Like '%" & txtManhanVienTim.Text & "%'", myConnection)
            key.Fill(dt)

            DataGridView2.DataSource = dt.DefaultView
            myConnection.Close()

        ElseIf radTimTheoPhongBan1.Checked = True Then
            Dim dt As New DataTable
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            Dim key As New OleDbDataAdapter
            key = New OleDbDataAdapter("Select * from nhanVien where maPB like '%" & cboPhongBanTim.SelectedItem.ToString() & "%'", myConnection)
            key.Fill(dt)

            DataGridView2.DataSource = dt.DefaultView
            myConnection.Close()
        ElseIf radTimTheoPhanLoaiNhanVien1.Checked = True Then
            Dim dt As New DataTable
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            Dim key As New OleDbDataAdapter
            key = New OleDbDataAdapter("Select * from nhanVien where maLoai like '%" & cboPhanLoaiNhanVienTim.SelectedItem.ToString() & "%'", myConnection)
            key.Fill(dt)
            DataGridView2.DataSource = dt.DefaultView
            myConnection.Close()
        ElseIf radTimTheoDieuKien1.Checked = True Then
            Dim dt As New DataTable
            Dim ds As New DataSet
            ds.Tables.Add(dt)
            Dim key As New OleDbDataAdapter
            If txtHoTenTim1.TextLength > 0 And txtDiaChiTim1.TextLength = 0 And txtDienThoaiTim1.TextLength = 0 Then
                'Họ Tên
                key = New OleDbDataAdapter("Select * from nhanVien where hotenNV like '%" & txtHoTenTim1.Text & "%'", myConnection)
            ElseIf txtDiaChiTim1.TextLength > 0 And txtDiaChiTim1.TextLength > 0 And txtDienThoaiTim1.TextLength = 0 Then
                'Họ Tên, Địa Chỉ
                key = New OleDbDataAdapter("Select * from nhanVien where hotenNV like '%" & txtHoTenTim1.Text & "%'" & "AND diaChi like'%" & txtDiaChiTim1.Text & "%'", myConnection)
            ElseIf txtDiaChiTim1.TextLength > 0 And txtDiaChiTim1.TextLength > 0 And txtDienThoaiTim1.TextLength > 0 Then
                'Họ Tên, Địa Chỉ, SĐT
                key = New OleDbDataAdapter("Select * from nhanVien where hotenNV like '%" & txtHoTenTim1.Text & "%'" & "AND diaChi like'%" & txtDiaChiTim1.Text & "%'" & "AND dienThoai like'%" & txtDienThoaiTim1.Text & "%'", myConnection)
            ElseIf txtHoTenTim1.TextLength = 0 And txtDiaChiTim1.TextLength > 0 And txtDienThoaiTim1.TextLength = 0 Then
                'Địa Chỉ
                key = New OleDbDataAdapter("Select * from diaChi where diaChi like '%" & txtDiaChiTim1.Text & "%'", myConnection)
            ElseIf txtHoTenTim1.TextLength = 0 And txtDiaChiTim1.TextLength > 0 And txtDienThoaiTim1.TextLength > 0 Then
                'Địa chỉ, SĐT
                key = New OleDbDataAdapter("Select * from diaChi where diaChi like '%" & txtDiaChiTim1.Text & "%'" & "AND dienThoai like'%" & txtDienThoaiTim1.Text & "%'", myConnection)
            Else
                'SĐT
                key = New OleDbDataAdapter("Select * from nhanVien where dienThoai like '%" & txtDienThoaiTim1.Text & "%'", myConnection)
            End If
            key.Fill(dt)
            DataGridView2.DataSource = dt.DefaultView
            myConnection.Close()
        End If
    End Sub
    Private Sub TxtHoTenTim1_TextChanged(sender As Object, e As EventArgs) Handles txtHoTenTim1.TextChanged
        radTimTheoDieuKien1.Checked = True
    End Sub

    Private Sub TxtDiaChiTim1_TextChanged(sender As Object, e As EventArgs) Handles txtDiaChiTim1.TextChanged
        radTimTheoDieuKien1.Checked = True
    End Sub

    Private Sub TxtDienThoaiTim1_TextChanged(sender As Object, e As EventArgs) Handles txtDienThoaiTim1.TextChanged
        radTimTheoDieuKien1.Checked = True
    End Sub

    Private Sub CboPhanLoaiNhanVienTim_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPhanLoaiNhanVienTim.SelectedIndexChanged
        radTimTheoPhanLoaiNhanVien1.Checked = True
    End Sub

    Private Sub TxtManhanVienTim_TextChanged(sender As Object, e As EventArgs) Handles txtManhanVienTim.TextChanged
        radTimTheoMaNhanVien1.Checked = True
    End Sub

    Private Sub CboPhongBanTim_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPhongBanTim.SelectedIndexChanged
        radTimTheoPhongBan1.Checked = True
    End Sub

    Private Sub BtnTongLuong_Click(sender As Object, e As EventArgs) Handles btnTongLuong.Click
        myConnection.Close()
        connection()
        Dim sum As String
        sum = "UPDATE tienLuong SET tongLuong = ((luongCanBan + luongNgay * soNgayLam) - luongNgay * soNgayNghi + tienChuyenCan + tienThuong +
                tienChuyenMon + phuCapKhac + luongTheoGio * soGioLamThem)"
        RefreshData()
        cmd = New OleDbCommand(sum, myConnection)
        Try
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            myConnection.Close()
            RefreshData()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim logo As Image = Image.FromFile(Path.Combine(Application.StartupPath, "image\logo.png"))
        Dim Header As Font = New Drawing.Font("Consolas", 16)
        Dim Bold As Font = New Drawing.Font("Calibri Bold", 17)
        Dim addr As Font = New Drawing.Font("Calibri light", 13)

        e.Graphics.DrawImage(logo, 30, 28, 190, 190)
        e.Graphics.DrawString("CÔNG TY CỔ PHẦN CÔNG NGHỆ CAO DŨNG GRAMER" + vbNewLine, Header, Brushes.Black, 330, 50)
        e.Graphics.DrawString("DŨNG GRAMER HIGH TECHNOLOGY COMPANY" + vbNewLine, Header, Brushes.Black, 330, 75)
        e.Graphics.DrawString("BÁO CÁO QUỸ TIỀN LƯƠNG, THÙ LAO, TIỀN THƯỞNG" + vbNewLine, Bold, Brushes.Black, 220, 250)
        e.Graphics.DrawString("Địa chỉ: số 125 đường Minh Khai, phường Minh Khai, quận Hai Bà Trưng, Thành phố Hà Nội, Việt Nam"
            + vbNewLine, addr, Brushes.Black, 330, 120)
        e.Graphics.DrawString("Điện thoại: 033.4565.999" + vbNewLine, addr, Brushes.Black, 330, 145)
        e.Graphics.DrawString("Email: Dung.dev.gramer@gmail.com" + vbNewLine, addr, Brushes.Black, 330, 170)


        Dim fmt As StringFormat = New StringFormat(StringFormatFlags.LineLimit)
        fmt.LineAlignment = StringAlignment.Center
        fmt.Trimming = StringTrimming.EllipsisCharacter
        Dim y As Int32 = e.MarginBounds.Top + 230
        Dim rc As Rectangle
        Dim x As Int32
        Dim h As Int32 = 0
        Dim row As DataGridViewRow

        ' print the header text for a new page
        '   use a grey bg just like the control
        If newpage Then
            row = TienLuongDataGridView.Rows(mRow)
            x = e.MarginBounds.Left - 65
            For Each cell As DataGridViewCell In row.Cells
                ' since we are printing the control's view,
                ' skip invidible columns
                If cell.Visible Then
                    rc = New Rectangle(x, y, cell.Size.Width, cell.Size.Height)

                    e.Graphics.FillRectangle(Brushes.LightGray, rc)
                    e.Graphics.DrawRectangle(Pens.Black, rc)

                    ' reused in the data pront - should be a function
                    Select Case TienLuongDataGridView.Columns(cell.ColumnIndex).DefaultCellStyle.Alignment
                        Case DataGridViewContentAlignment.BottomRight,
                             DataGridViewContentAlignment.MiddleRight
                            fmt.Alignment = StringAlignment.Far
                            rc.Offset(-1, 0)
                        Case DataGridViewContentAlignment.BottomCenter,
                            DataGridViewContentAlignment.MiddleCenter
                            fmt.Alignment = StringAlignment.Center
                        Case Else
                            fmt.Alignment = StringAlignment.Near
                            rc.Offset(2, 0)
                    End Select

                    e.Graphics.DrawString(TienLuongDataGridView.Columns(cell.ColumnIndex).HeaderText,
                                                TienLuongDataGridView.Font, Brushes.Black, rc, fmt)
                    x += rc.Width
                    h = Math.Max(h, rc.Height)
                End If
            Next
            y += h

        End If
        newpage = False

        ' now print the data for each row
        Dim thisNDX As Int32
        For thisNDX = mRow To TienLuongDataGridView.RowCount - 1
            ' no need to try to print the new row
            If TienLuongDataGridView.Rows(thisNDX).IsNewRow Then Exit For

            row = TienLuongDataGridView.Rows(thisNDX)
            x = e.MarginBounds.Left
            h = 0

            ' reset X for data
            x = e.MarginBounds.Left - 65

            ' print the data
            For Each cell As DataGridViewCell In row.Cells
                If cell.Visible Then
                    rc = New Rectangle(x, y, cell.Size.Width, cell.Size.Height)

                    e.Graphics.DrawRectangle(Pens.Black, rc)

                    Select Case TienLuongDataGridView.Columns(cell.ColumnIndex).DefaultCellStyle.Alignment
                        Case DataGridViewContentAlignment.BottomRight,
                             DataGridViewContentAlignment.MiddleRight
                            fmt.Alignment = StringAlignment.Far
                            rc.Offset(-1, 0)
                        Case DataGridViewContentAlignment.BottomCenter,
                            DataGridViewContentAlignment.MiddleCenter
                            fmt.Alignment = StringAlignment.Center
                        Case Else
                            fmt.Alignment = StringAlignment.Near
                            rc.Offset(2, 0)
                    End Select

                    e.Graphics.DrawString(cell.FormattedValue.ToString(),
                                          TienLuongDataGridView.Font, Brushes.Black, rc, fmt)

                    x += rc.Width
                    h = Math.Max(h, rc.Height)
                End If

            Next
            y += h
            ' next row to print
            mRow = thisNDX + 1

            If y + h > e.MarginBounds.Bottom Then
                e.HasMorePages = True
                ' mRow -= 1   causes last row to rePrint on next page
                newpage = True
                Return
            End If
        Next
    End Sub
    Private Sub BtnHoaDon_Click(sender As Object, e As EventArgs) Handles btnHoaDon.Click
        PrintDocument1.DefaultPageSettings.Landscape = True
        mRow = 0
        newpage = True
        PrintDocument1.Print()

    End Sub
    Private Sub BtnThemMoiLuong_Click(sender As Object, e As EventArgs) Handles btnThemMoiLuong.Click
        TienLuongBindingSource.AddNew()
    End Sub

    Private Sub BtnLuuLuong_Click(sender As Object, e As EventArgs) Handles btnLuuLuong.Click
        Try
            Me.Validate()
            Me.TienLuongBindingSource.EndEdit()
            TienLuongTableAdapter.Update(QLNSDataSet.tienLuong)
            BtnTongLuong_Click(sender, e)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub BtnXoaLuong_Click(sender As Object, e As EventArgs) Handles btnXoaLuong.Click
        Try
            Me.Validate()
            TienLuongBindingSource.RemoveCurrent()
            Me.TienLuongBindingSource.EndEdit()
            TienLuongTableAdapter.Update(QLNSDataSet.tienLuong)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnThemMoiPB_Click(sender As Object, e As EventArgs) Handles btnThemMoiPB.Click
        PhongBanBindingSource.AddNew()
    End Sub

    Private Sub BtnLuuPB_Click(sender As Object, e As EventArgs) Handles btnLuuPB.Click
        Try
            Me.Validate()
            Me.PhongBanBindingSource.EndEdit()
            PhongBanTableAdapter.Update(QLNSDataSet.phongBan)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnXoaPB_Click(sender As Object, e As EventArgs) Handles btnXoaPB.Click
        Try
            Me.Validate()
            PhongBanBindingSource.RemoveCurrent()
            Me.PhongBanBindingSource.EndEdit()
            PhongBanTableAdapter.Update(QLNSDataSet.phongBan)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnThemMoiCV_Click(sender As Object, e As EventArgs) Handles btnThemMoiCV.Click
        ChucVuBindingSource.AddNew()
    End Sub

    Private Sub BtnLuuCV_Click(sender As Object, e As EventArgs) Handles btnLuuCV.Click
        Try
            Me.Validate()
            Me.ChucVuBindingSource.EndEdit()
            ChucVuTableAdapter.Update(QLNSDataSet.chucVu)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnXoaCV_Click(sender As Object, e As EventArgs) Handles btnXoaCV.Click
        Try
            Me.Validate()
            ChucVuBindingSource.RemoveCurrent()
            Me.ChucVuBindingSource.EndEdit()
            ChucVuTableAdapter.Update(QLNSDataSet.chucVu)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnThemMoiHD_Click(sender As Object, e As EventArgs) Handles btnThemMoiHD.Click
        LoaiNhanVienBindingSource.AddNew()
    End Sub

    Private Sub BtnLuuHD_Click(sender As Object, e As EventArgs) Handles btnLuuHD.Click
        Try
            Me.Validate()
            Me.LoaiNhanVienBindingSource.EndEdit()
            LoaiNhanVienTableAdapter.Update(QLNSDataSet.loaiNhanVien)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnXoaHD_Click(sender As Object, e As EventArgs) Handles btnXoaHD.Click
        Try
            Me.Validate()
            LoaiNhanVienBindingSource.RemoveCurrent()
            Me.LoaiNhanVienBindingSource.EndEdit()
            LoaiNhanVienTableAdapter.Update(QLNSDataSet.loaiNhanVien)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://fb.me/dung.dev.gramer")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("https://github.com/DungGramer")
    End Sub

    Private Sub AccordionControlElement2_Click(sender As Object, e As EventArgs) Handles AccordionControlElement2.Click
        tabThemNhanVien.Show()
        tabSuaXoa.Hide()
        tabTimKiem.Hide()
        tabLuong.Hide()
        tabPhongBan.Hide()
        tabChucVu.Hide()
        tabLoaiHopDong.Hide()
        tabTienLuong.Hide()
        tabPreview.Hide()
    End Sub

    Private Sub AccordionControlElement3_Click(sender As Object, e As EventArgs) Handles AccordionControlElement3.Click
        tabSuaXoa.Show()
        tabThemNhanVien.Hide()
        tabTimKiem.Hide()
        tabLuong.Hide()
        tabPhongBan.Hide()
        tabChucVu.Hide()
        tabLoaiHopDong.Hide()
        tabTienLuong.Hide()
        tabPreview.Hide()
    End Sub

    Private Sub AccordionControlElement4_Click(sender As Object, e As EventArgs) Handles AccordionControlElement4.Click
        tabTimKiem.Show()
        tabThemNhanVien.Hide()
        tabSuaXoa.Hide()
        tabLuong.Hide()
        tabPhongBan.Hide()
        tabChucVu.Hide()
        tabLoaiHopDong.Hide()
        tabTienLuong.Hide()
        tabPreview.Hide()
    End Sub

    Private Sub AccordionControlElement6_Click(sender As Object, e As EventArgs) Handles AccordionControlElement6.Click
        tabLuong.Show()
        tabThemNhanVien.Hide()
        tabSuaXoa.Hide()
        tabTimKiem.Hide()
        tabPhongBan.Hide()
        tabChucVu.Hide()
        tabLoaiHopDong.Hide()
        tabTienLuong.Hide()
        tabPreview.Hide()
    End Sub

    Private Sub AccordionControlElement8_Click(sender As Object, e As EventArgs) Handles AccordionControlElement8.Click
        tabPhongBan.Show()
        tabThemNhanVien.Hide()
        tabSuaXoa.Hide()
        tabTimKiem.Hide()
        tabLuong.Hide()
        tabChucVu.Hide()
        tabLoaiHopDong.Hide()
        tabTienLuong.Hide()
        tabPreview.Hide()
    End Sub

    Private Sub AccordionControlElement9_Click(sender As Object, e As EventArgs) Handles AccordionControlElement9.Click
        tabChucVu.Show()
        tabThemNhanVien.Hide()
        tabSuaXoa.Hide()
        tabTimKiem.Hide()
        tabLuong.Hide()
        tabPhongBan.Hide()
        tabLoaiHopDong.Hide()
        tabTienLuong.Hide()
        tabPreview.Hide()
    End Sub

    Private Sub AccordionControlElement10_Click(sender As Object, e As EventArgs) Handles AccordionControlElement10.Click
        tabLoaiHopDong.Show()
        tabThemNhanVien.Hide()
        tabSuaXoa.Hide()
        tabTimKiem.Hide()
        tabLuong.Hide()
        tabPhongBan.Hide()
        tabChucVu.Hide()
        tabTienLuong.Hide()
        tabPreview.Hide()
    End Sub

    Private Sub AccordionControlElement11_Click(sender As Object, e As EventArgs) Handles AccordionControlElement11.Click
        tabTienLuong.Show()
        tabThemNhanVien.Hide()
        tabSuaXoa.Hide()
        tabTimKiem.Hide()
        tabLuong.Hide()
        tabPhongBan.Hide()
        tabChucVu.Hide()
        tabLoaiHopDong.Hide()
        tabPreview.Hide()
    End Sub

    Private Sub AccordionControlElement13_Click(sender As Object, e As EventArgs) Handles AccordionControlElement13.Click
        tabPreview.Show()
        tabThemNhanVien.Hide()
        tabSuaXoa.Hide()
        tabTimKiem.Hide()
        tabLuong.Hide()
        tabPhongBan.Hide()
        tabChucVu.Hide()
        tabLoaiHopDong.Hide()
        tabTienLuong.Hide()
    End Sub

End Class
