Imports System.Net
Imports System.IO
Imports System.Threading
Imports System.Reflection

Public Class _Default
    Inherits System.Web.UI.Page

#Region "Properties"

    Public Shared ReadOnly Property Music() As FreeMusic
        Get
            If Not IsNothing(HttpContext.Current.Application("Music")) Then
                Return CType(HttpContext.Current.Application("Music"), FreeMusic)
            Else
                Dim myMusic As New FreeMusic
                myMusic.Login("4521252528", "10ruavy0ix")
                HttpContext.Current.Application("Music") = myMusic
                Return myMusic
            End If
        End Get
    End Property

    Public Shared Property Songs As List(Of FreeMusic.Song)
        Get
            Return HttpContext.Current.Session("Songs")
        End Get
        Set(ByVal value As List(Of FreeMusic.Song))
            HttpContext.Current.Session("Songs") = value
        End Set
    End Property

    Public Property SortExpression As String
        Get
            If IsNothing(ViewState("SortExpression")) Then
                ViewState("SortExpression") = ""
            End If
            Return ViewState("SortExpression")
        End Get
        Set(value As String)
            ViewState("SortExpression") = value
        End Set
    End Property

    Public Property SortAscending As Boolean
        Get
            If IsNothing(ViewState("SortAscending")) Then
                ViewState("SortAscending") = False
            End If
            Return ViewState("SortAscending")
        End Get
        Set(value As Boolean)
            ViewState("SortAscending") = value
        End Set
    End Property

#End Region

#Region "Searching"

    Private Sub RenderData()
        For i As Integer = 0 To Songs.Count - 1
            Songs(i).Id = i
        Next
        rpData.DataSource = Songs
        rpData.DataBind()
    End Sub

    Private Sub DidSearch(ByVal txt As String)
        phSearch.Visible = False
        phResults.Visible = True

        txtSearch2.Text = txt
        txtSearch.Text = txt

        Dim result As List(Of FreeMusic.Song) = Music.Search(txtSearch2.Text)
        Songs = result

        RenderData()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        DidSearch(txtSearch.Text)
    End Sub

    Private Sub btnSearch2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch2.Click
        DidSearch(txtSearch2.Text)
    End Sub

    Protected Sub rpData_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rpData.ItemCommand
        If e.CommandName = "Download" AndAlso Songs.Count > e.CommandArgument Then
            Dim mySong As FreeMusic.Song = Songs(e.CommandArgument)

            ' Make request
            Dim req As HttpWebRequest
            req = WebRequest.Create(mySong.Url)
            req.Method = "GET"

            ' Get streams
            Dim res As HttpWebResponse = req.GetResponse
            Dim length As Integer = res.ContentLength
            Dim responseStream As Stream = res.GetResponseStream
            Dim writeStream As Stream = Response.OutputStream
            Response.Clear()
            Response.ContentType = "audio/mpeg"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" + CleanIllegalChars(mySong.Artist + " - " + mySong.Title + ".mp3"""))
            responseStream.CopyTo(writeStream)
            responseStream.Flush()
            Response.Flush()
            Response.End()

            ' Close streams
            responseStream.Close()
            writeStream.Close()
            res.Close()
        End If
    End Sub

#End Region

#Region "Sorting"

    Private Sub lbHeader_Click(sender As Object, e As System.EventArgs) Handles lbQuantity.Click, lbArtist.Click, lbTitle.Click, lbDuration.Click, lbBitrate.Click, lbSize.Click
        Dim name As String = CType(sender, LinkButton).ID.Substring(2)

        If SortExpression <> name Then
            SortExpression = name
            SortAscending = True
        Else
            SortAscending = Not SortAscending
        End If

        Songs.Sort(New SongComparer(SortExpression, SortAscending))

        RenderData()
    End Sub

    Public Class SongComparer
        Implements IComparer(Of FreeMusic.Song)

        Private SortExpression As String
        Private SortAscending As Boolean

        Public Sub New(SortExpression As String, SortAscending As Boolean)
            Me.SortExpression = SortExpression
            Me.SortAscending = SortAscending
        End Sub

        Public Function Compare(x As FreeMusic.Song, y As FreeMusic.Song) As Integer Implements System.Collections.Generic.IComparer(Of FreeMusic.Song).Compare
            Dim info As PropertyInfo = x.GetType.GetProperty(SortExpression)
            Dim obj1 As IComparable = info.GetValue(x, Nothing)
            Dim obj2 As IComparable = info.GetValue(y, Nothing)
            Return If(SortAscending, obj1.CompareTo(obj2), obj2.CompareTo(obj1))
        End Function
    End Class

#End Region

#Region "Render helpers"

    Protected Shared Function RenderDuration(ByVal duration As Integer) As String
        Dim length As New TimeSpan(0, 0, duration)
        Return length.Minutes & ":" & length.Seconds.ToString.PadLeft(2, "0")
    End Function

    Protected Shared Function RenderSize(ByVal size As Integer) As String
        Return String.Format("{0:0.00}", Math.Round(size / 1024 / 1024, 2))
    End Function

    Public Shared Function CleanIllegalChars(ByVal Input As String) As String
        Return Regex.Replace(Input, ":|\?|\\|\*|\""|<|>|\||%", String.Empty)
    End Function



#End Region

#Region "Web Methods"

    <System.Web.Services.WebMethod()> _
    Public Shared Function FetchDetails(ByVal id As Integer) As Dictionary(Of String, String)
        If Songs.Count > id Then
            Dim detailedSong As FreeMusic.Song = Songs.Find(Function(s) s.Id = id)
            If detailedSong.Bitrate = 0 AndAlso detailedSong.Size = 0 Then
                detailedSong = Music.FetchDetail(detailedSong)
            End If
            Return New Dictionary(Of String, String) From {{"id", id}, {"size", RenderSize(detailedSong.Size)}, {"bitrate", detailedSong.Bitrate}}
        End If
        Return Nothing
    End Function

#End Region

#Region "Page events"

    Private Sub frm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles frm.Load
        If Not Page.IsPostBack AndAlso Not Request.QueryString("auth") = "ok" Then
            Response.Clear()
            Response.Write("Auth")
            Response.End()
        End If
    End Sub

#End Region

End Class