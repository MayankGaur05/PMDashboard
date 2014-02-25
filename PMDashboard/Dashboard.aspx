<%@ Page Title="" Language="C#" MasterPageFile="~/_FileBound.master" AutoEventWireup="true"
    Inherits="PMDashboard.Dashboard" CodeFile="Dashboard.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
            padding: 5px;
        }
        .style2
        {
            width: 158px;
            padding: 5px;
        }
        .style3
        {
            width: 100%;
            border-style: solid;
            border-width: 1px;
            padding: 5px;
        }
        .style4
        {
            width: 450px;
            padding: 5px;
        }
        .style5
        {
            width: 426px;
            height: 29px;
            padding: 5px;
        }
        .style6
        {
            height: 29px;
            padding: 5px;
        }
        .cell_heading
        {
            width: 130px;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .grid
        {
            width: 100%;
            height: 140px;
            overflow-x: hidden;
            overflow-y: auto;
        }
        .ProjectGrid
        {
            width: 650px;
        }
        .HeaderRow
        {
            color: White;
            background-color: #006699;
            font-weight: bold;
        }
        
        
        .header
        {
            font-weight: bold;
            position: absolute;
            background-color: White;
        }
    </style>
    <link href="Styles/Popup.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"> </script>
    <script type="text/javascript">
        function GetClick(day) {
            var button = document.getElementById('<%= Button1.ClientID %>');
            var hiddate = document.getElementById('<%= hiddDate.ClientID %>');
            if (button != null) {
                hiddate.value = day;
                button.click();
            }
            else {
                alert('fail');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <asp:HiddenField ID="hiddDate" runat="server" />
    <table class="style1" border="1px" cellspacing="4">
        <tr>
            <td class="style2">
                &nbsp;<%--<img id="imgEmp" runat="server" src="EmployessImages/no-image-icon-md.png" alt="Employee" style="height: 79px;
                    width: 142px" />--%>
                <asp:Image ID="imgEmp" runat="server" ImageUrl="EmployessImages/no-image-icon-md.png"
                    alt="Employee" Style="height: 79px; width: 142px" />
            </td>
            <td valign="top" style="padding: 5px">
                <asp:Label ID="lblEmpName" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblEmpMail" runat="server"></asp:Label><br />
                <asp:Label ID="lblEmpPhone" runat="server"></asp:Label>
            </td>
            <td align="right" valign="top" style="padding: 5px;">
                &nbsp;<img id="imgComp" runat="server" src="CompanyLogo/chetu-logo.png" align="top"
                    alt="CompanyLogo" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table cellpadding="4" cellspacing="4" class="style3" width="100%">
        <tr>
            <td valign="top">
                <table width="100%" border="1">
                    <tr>
                        <td colspan="2" align="center">
                            <h2>
                                Open Projects</h2>
                        </td>
                    </tr>
                    <tr>
                        <td class="ProjectGrid" align="left" valign="top">
                            <%-- <div style="width: 100%; height: 140px; overflow: auto">--%>
                            <asp:Panel ID="Panel1" runat="server" Height="120px" Width="100%" ScrollBars="Vertical">
                                <asp:GridView ID="gvProject" runat="server" AutoGenerateColumns="False" Width="100%"
                                    ShowHeader="true" Height="120px" OnRowCommand="gvProject_RowCommand" BackColor="White"
                                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="grid">
                                    <Columns>
                                        <asp:BoundField HeaderText="FileID" Visible="false" DataField="FileID" />
                                        <asp:BoundField HeaderText="ProjectName" DataField="ProjectName" />
                                        <asp:BoundField HeaderText="Project Manger" DataField="PMAssigned" />
                                        <asp:BoundField HeaderText="Tech Assigned" DataField="TechAssigned" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnViewDetails" runat="server" CommandArgument='<%# Eval("FileID") %>'
                                                    CommandName="VeiwDetails" class="topopupr">Project Info</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnTaskDetails" runat="server" CommandArgument='<%# Eval("FileID") %>'
                                                    CommandName="ViewTaskDetails" class="topopupr">View Task Details</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </asp:Panel>
                            <%--</div>--%>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" align="right">
                <asp:Calendar ID="calProject" runat="server" OnDayRender="calProject_DayRender" BackColor="White"
                    BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" DayNameFormat="Shortest"
                    Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" Height="200px" Width="220px">
                    <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                    <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                    <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                    <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True"
                        Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                    <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                    <WeekendDayStyle BackColor="#CCCCFF" />
                </asp:Calendar>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table cellpadding="4" cellspacing="4" class="style3" width="100%">
        <tr>
            <td valign="top">
                <table width="100%" border="1">
                    <tr>
                        <td colspan="2" align="center">
                            <h2>
                                Open Tickets</h2>
                        </td>
                    </tr>
                    <tr>
                        <td class="ProjectGrid" align="left" valign="top">
                            <div style="width: 100%; height: 140px; overflow-x: hidden; overflow-y: auto">
                                <asp:GridView ID="gvTicket" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#CCCCCC" BorderStyle="None" Width="100%" BorderWidth="1px" CellPadding="3"
                                    OnRowCommand="gvTicket_RowCommand">
                                    <Columns>
                                        <asp:BoundField HeaderText="FileID" Visible="false" DataField="FileID" />
                                        <asp:BoundField HeaderText="Ticket Number" DataField="TicketNumber" />
                                        <asp:BoundField HeaderText="Ticket Name" DataField="TicketName" />
                                        <asp:BoundField HeaderText="Priority" DataField="TicketPriority" />
                                        <asp:BoundField HeaderText="Tech Assigned" DataField="TechAssigned" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnTicketInfo" runat="server" CommandArgument='<%# Eval("FileID") %>'
                                                    CommandName="TicketInfo" class="topopupr">Ticket Info</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnTaskDetails" runat="server" CommandArgument='<%# Eval("FileID") %>'
                                                    CommandName="ViewTaskDetails" class="topopupr">View Task Details</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <td valign="top" align="right">
                    <asp:Calendar ID="calcTicket" runat="server" SelectedDate="2013-05-15" BackColor="White"
                        BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" DayNameFormat="Shortest"
                        Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" Height="200px" Width="220px"
                        OnDayRender="calcTicket_DayRender">
                        <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                        <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                        <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True"
                            Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                        <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                        <WeekendDayStyle BackColor="#CCCCFF" />
                    </asp:Calendar>
                </td>
            </td>
        </tr>
    </table>
    <br />
    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="panelPopup"
        TargetControlID="btnDefault" BackgroundCssClass="ModalPopupBG" CancelControlID="btnClose">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="panelPopup" runat="server" Style="font-family: lucida grande,tahoma,verdana,arial,sans-serif;
        background: none repeat scroll 0 0 #FFFFFF; border: 10px solid #ccc; border-radius: 3px 3px 3px 3px;
        color: #333333; display: none; font-size: 14px; left: 50%; position: fixed; top: 20%;
        width: 800px; z-index: 2;">
        <div id="toPopup2">
            <div class="close">
                <asp:ImageButton ID="btnClose" runat="server" ImageUrl="~/Plugins/PMDashboard/Images/closebox.jpg"
                    Width="32px" Height="32px" />
            </div>
            <span class="ecs_tooltip">Press Esc to close <span class="arrow"></span></span>
            <div id="popup_content">
                <!--your content start-->
                <div id="divDetails" runat="server">
                    <table cellpadding="4" cellspacing="4" class="style3" width="50%">
                        <tr>
                            <td valign="top">
                                <table width="100%" border="1">
                                    <tr>
                                        <td colspan="4" align="center" valign="middle">
                                            <h2>
                                                <b>Customer Information</b>
                                            </h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_heading" align="left" valign="top">
                                            &nbsp;Company Name
                                        </td>
                                        <td>
                                            &nbsp;<asp:Label ID="lblCompName" runat="server"></asp:Label>
                                        </td>
                                        <td class="cell_heading" align="left" valign="top">
                                            &nbsp;Contact Person
                                        </td>
                                        <td>
                                            &nbsp;<asp:Label ID="lblContactPerson" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_heading" align="left" valign="top">
                                            &nbsp;Address
                                        </td>
                                        <td>
                                            &nbsp;<asp:Label ID="lblCustomerAddress" runat="server"></asp:Label>
                                        </td>
                                        <td class="cell_heading" align="left" valign="top">
                                            &nbsp;Email-ID
                                        </td>
                                        <td>
                                            &nbsp;<asp:Label ID="lblCutsomerEmail" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="rowProject" runat="server" visible="false">
                                        <td colspan="4">
                                            <table width="100%" border="1">
                                                <tr>
                                                    <td colspan="4" align="center" valign="middle">
                                                        <h2>
                                                            <b>Project Information</b>
                                                        </h2>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;Project Name
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblProjectName" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;Description
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblProDesc" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;PM Assigned
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblPMAssigned" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;Tech Assigned
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblTechAssigned" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;Start Date
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblStartDate" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;End Date
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblEndDate" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="rowTicket" runat="server" visible="false">
                                        <td colspan="4">
                                            <table width="100%" border="1">
                                                <tr>
                                                    <td colspan="4" align="center" valign="middle">
                                                        <h2>
                                                            <b>Ticket Information</b>
                                                        </h2>
                                                    </td>
                                                </tr>
                                                <tr id="Tr2" runat="server">
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;Ticket Number
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblTicketNumber" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;Ticket Name
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblTicketName" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;Priority
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblPriority" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;Tech Assigned
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblTicTech" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;Start Date
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblTicStart" runat="server"></asp:Label>
                                                    </td>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;End Date
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblTicEnd" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_heading" align="left" valign="top">
                                                        &nbsp;Description
                                                    </td>
                                                    <td colspan="3">
                                                        &nbsp;<asp:Label ID="lblTicDesc" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center" valign="middle">
                                            <h2>
                                                <b>IT Contact Information</b>
                                            </h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_heading" align="left" valign="top">
                                            &nbsp;IT Contact
                                        </td>
                                        <td>
                                            &nbsp;<asp:Label ID="lblITContact" runat="server"></asp:Label>
                                        </td>
                                        <td class="cell_heading" align="left" valign="top">
                                            &nbsp;IT Phone
                                        </td>
                                        <td>
                                            &nbsp;<asp:Label ID="lblITPhone" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_heading" align="left" valign="top">
                                            &nbsp;IT Email
                                        </td>
                                        <td>
                                            &nbsp;<asp:Label ID="lblITEmail" runat="server"></asp:Label>
                                        </td>
                                        <td class="cell_heading" align="left" valign="top">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div id="divGrid" runat="server" visible="false">
                <%--Project Grid--%>
                    <asp:GridView ID="gvPro" runat="server" AutoGenerateColumns="False" Width="100%"
                        OnRowCommand="gvProject_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3">
                        <Columns>
                            <asp:BoundField HeaderText="FileID" Visible="false" DataField="FileID" />
                            <asp:BoundField HeaderText="ProjectName" DataField="ProjectName" />
                            <asp:BoundField HeaderText="Project Maanger" DataField="PMAssigned" />
                            <asp:BoundField HeaderText="Task Name" DataField="Name" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnTaskDetails" runat="server" CommandArgument='<%# Eval("FileID") +":" + Eval("TaskID") %>'
                                        CommandName="ViewTaskDetails" class="topopupr">View Task Details</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                    <%--Ticket Grid--%>
                    <asp:GridView ID="gvTick" runat="server" AutoGenerateColumns="False" Width="100%"
                        OnRowCommand="gvProject_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3">
                        <Columns>
                            <asp:BoundField HeaderText="FileID" Visible="false" DataField="FileID" />
                            <asp:BoundField HeaderText="Task Name" DataField="Name" />
                            <asp:BoundField HeaderText="Tech Assigned" DataField="AssignedTo" />
                            <asp:BoundField HeaderText="Priority" DataField="Priority" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnTaskDetails" runat="server" CommandArgument='<%# Eval("FileID") +":" + Eval("TaskID") %>'
                                        CommandName="ViewTaskDetails" class="topopupr">View Task Details</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                </div>
            </div>
            <!--your content end-->
        </div>
        <!--toPopup end-->
        <div class="loader">
        </div>
        <div id="backgroundPopup">
        </div>
    </asp:Panel>
    <asp:Button ID="btnDefault" runat="server" OnClick="btnDefault_Click" Style="display: none" />
    <asp:Button ID="Button1" runat="server" OnClick="btnDefault_Click" Style="display: none" />
    <br />
</asp:Content>
