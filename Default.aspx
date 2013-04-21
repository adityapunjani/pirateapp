<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="PirateOnline._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PirateOnline</title>
    <link href="Styles/Default.css" rel="stylesheet" type="text/css" />
    <link href="Styles/ui-lightness/jquery-ui-1.8.13.custom.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.6.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var sortexp = "<%=SortExpression.ToLower %>";
        var sortasc = <%=SortAscending.ToString.ToLower %>;
    </script>
    <script src="Scripts/Default.js" type="text/javascript"></script>
</head>
<body>
    <form id="frm" runat="server" autocomplete="off">

        <asp:PlaceHolder ID="phSearch" runat="server">

            <div class="looking"></div>
            <div class="bigsearchcontainer">
                <div class="bigsearchleft"></div>
                <div class="bigsearchmiddle">
                    <asp:TextBox ID="txtSearch" CssClass="bigsearchinput" runat="server" EnableViewState="false" />
                </div>
                <div class="bigsearchright">
                    <asp:ImageButton ID="btnSearch" runat="server" EnableViewState="false" ImageUrl="~/Images/BigSearch/BigSearchRight.png" />
                </div>
            </div>

        </asp:PlaceHolder>



        <asp:PlaceHolder ID="phResults" runat="server" Visible="false">

            <div class="smallsearchcontainer">
                <div class="smallsearchleft"></div>
                <div class="smallsearchmiddle">
                    <asp:TextBox ID="txtSearch2" CssClass="smallsearchinput" runat="server" EnableViewState="false" />
                </div>
                <div class="smallsearchright">
                    <asp:ImageButton ID="btnSearch2" runat="server" EnableViewState="false" ImageUrl="~/Images/SmallSearch/SmallSearchRight.png" />
                </div>
            </div>
        

            <div id="progressbar"></div>


            <div class="tablecontainer">

                <table width="700" class="resulttable" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="10" class="tableheadertopmiddle">
                            <div class="tableheadertopleft"></div>
                            <div class="tableheadertopright"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableleft"></td>
                        <td class="tableheader h_padding"></td>
                        <td class="tableheader h_qty"><div class="a_quantity" runat="server"><asp:LinkButton ID="lbQuantity" runat="server" CssClass="headerlink">Qty</asp:LinkButton></div></td>
                        <td class="tableheader h_artist"><div class="a_artist"><asp:LinkButton ID="lbArtist" runat="server" CssClass="headerlink">Artist</asp:LinkButton></div></td>
                        <td class="tableheader h_title"><div class="a_title"><asp:LinkButton ID="lbTitle" runat="server" CssClass="headerlink">Title</asp:LinkButton></div></td>
                        <td class="tableheader h_length"><div class="a_duration"><asp:LinkButton ID="lbDuration" runat="server" CssClass="headerlink">Length</asp:LinkButton></div></td>
                        <td class="tableheader h_bitrate"><div class="a_bitrate"><asp:LinkButton ID="lbBitrate" runat="server" CssClass="headerlink">Bitrate</asp:LinkButton></div></td>
                        <td class="tableheader h_size"><div class="a_size"><asp:LinkButton ID="lbSize" runat="server" CssClass="headerlink">Size (MB)</asp:LinkButton></div></td>
                        <td class="tableheader h_getfile">Get file</td>
                        <td class="tableright"></td>
                    </tr>
                    <asp:Repeater ID="rpData" runat="server">
                        <AlternatingItemTemplate>
                            <tr>
                                <td class="tableleft"></td>
                                <td class="tablerowalt"><asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("Id")%>' /></td>
                                <td class="tablerowalt"><%# Eval("Quantity")%></td>
                                <td class="tablerowalt"><%# Eval("Artist")%></td>
                                <td class="tablerowalt"><%# Eval("Title")%></td>
                                <td class="tablerowalt"><%# RenderDuration(Eval("Duration"))%></td>
                                <td class="tablerowalt"><%# Eval("Bitrate")%></td>
                                <td class="tablerowalt"><%# RenderSize(Eval("Size"))%></td>
                                <td class="tablerowalt">
                                    <asp:ImageButton ID="btnDownload" runat="server" CssClass="download" ImageUrl="~/Images/Button/Download.png" CommandName="Download" CommandArgument='<%#Eval("Id") %>' />
                                </td>
                                <td class="tableright"></td>
                            </tr>
                        </AlternatingItemTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="tableleft"></td>
                                <td class="tablerow"><asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("Id")%>' /></td>
                                <td class="tablerow"><%# Eval("Quantity")%></td>
                                <td class="tablerow"><%# Eval("Artist")%></td>
                                <td class="tablerow"><%# Eval("Title")%></td>
                                <td class="tablerow"><%# RenderDuration(Eval("Duration"))%></td>
                                <td class="tablerow"><%# Eval("Bitrate")%></td>
                                <td class="tablerow"><%# RenderSize(Eval("Size"))%></td>
                                <td class="tablerow">
                                    <asp:ImageButton ID="btnDownload" runat="server" CssClass="download" ImageUrl="~/Images/Button/Download.png" CommandName="Download" CommandArgument='<%#Eval("Id") %>' />
                                </td>
                                <td class="tableright"></td>
                            </tr>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <tr>
                                <td class="tableleft"></td>
                                <td class="tablehline" colspan="8"></td>
                                <td class="tableright"></td>
                            </tr>
                        </SeparatorTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="10" class="tablebottommiddle">
                            <div class="tablebottomleft"></div>
                            <div class="tablebottomright"></div>
                        </td>
                    </tr>
                </table>

                <div id="asdasd"></div>

            </div>

        </asp:PlaceHolder>

    </form>
</body>
</html>
