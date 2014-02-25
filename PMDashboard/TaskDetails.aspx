<%@ Page Title="" Language="C#" MasterPageFile="~/Plugins/PMDashboard/Site.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeFile="TaskDetails.aspx.cs"
    Inherits="PMDashboard.ProjectDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
            border-style: solid;
            border-width: 1px;
        }
        
        .heading
        {
            width: 30%;
            font-weight: 500;
        }
        
        .time
        {
            text-align: center;
            font-family: Arial;
            font-size: 28px;
            font-weight: bold; /* options are normal, bold, bolder, lighter */
            font-style: normal; /* options are normal or italic */
            color: Red; /* change color using the hexadecimal color codes for HTML */
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ValidationSummary ID="vSummary" runat="server" ShowMessageBox="true" ValidationGroup="Task"
        ShowSummary="false" />
    <a href="Dashboard.aspx" style="text-decoration:none;color:black;"><img src="Images/home_icon.png"  alt="Home Page"/></a>
    <br />
    <br />
    <asp:Label ID="lblType" runat="server" Font-Size="14px" Font-Bold="true">
    </asp:Label>&nbsp;&nbsp;
    <asp:Label ID="lblProjectName" runat="server" Font-Size="14px" Font-Bold="true">
    </asp:Label>
    <br />
    <br />
    <%--  <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>--%>
    <asp:Button ID="btnAddTask" runat="server" Text="Add New Task" OnClick="btnAddTask_Click" />
    <br />
    <br />
    <asp:HiddenField ID="hiddTaskID" runat="server" />
    <div id="divCombo" runat="server">
        <table cellpadding="0" cellspacing="0" width="50%">
            <tr>
                <td>
                    Assign To
                </td>
                <td>
                    <asp:DropDownList ID="ddlAssigned" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAssigned_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    Status
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dlStatus_SelectedIndexChanged">
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                        <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
                        <asp:ListItem Text="In Process" Value="In Process"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <br />
    <div id="divGrid" runat="server" style="height: 300px; overflow-x: hidden; overflow-y: auto">
        <asp:GridView ID="gvTask" runat="server" AutoGenerateColumns="False" Width="98%"
            BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
            CellPadding="3" OnRowCommand="gvTask_RowCommand" EmptyDataText="No Task Found">
            <Columns>
                <asp:BoundField HeaderText="FileID" Visible="false" DataField="FileID" />
                <asp:BoundField HeaderText="Task Name" DataField="Name" />
                <asp:BoundField HeaderText="Description" DataField="Description" />
                <asp:BoundField HeaderText="Start Date" DataField="TaskStartDate" />
                <asp:BoundField HeaderText="End Date" DataField="TaskEndDate" />
                <asp:BoundField HeaderText="Tech Assigned" DataField="AssignedTo" />
                <asp:BoundField HeaderText="Status" DataField="Status" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnViewDetails" runat="server" CommandArgument='<%# Eval("TaskID") %>'
                            CommandName="OpenEdit" CssClass="topopupr">Open/Edit</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="View Document" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnViewDocument" runat="server" CommandArgument='<%# Eval("FileID") %>'
                            CommandName="ViewDocument" CssClass="topopupr">View</asp:LinkButton>
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
    <div class="clear">
    </div>
    <div id="divTask" runat="server" visible="false">
        <div>
            <b>* Required Fields</b>
        </div>
        <div id="divTime" runat="server" visible="false" style="padding-top:5px;">
            <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="4" cellspacing="4" class="style1" border="1">
                        <tr>
                            <td class="heading">
                                <b>Start Clock</b>
                            </td>
                            <td>
                                <asp:Button ID="btnStart" runat="server" Text="Start" Width="205px" TabIndex="8"
                                    OnClick="btnStart_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="heading">
                                <b>Total time performed on task today</b>
                            </td>
                            <td>
                                <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                                </asp:Timer>
                                &nbsp;<asp:Label ID="lblTotalTimeToday" runat="server" TabIndex="9" CssClass="time"></asp:Label>
                                <span style="float: right; font-weight: bold">dd:hh:mm:ss </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="heading">
                                <b>Overall time performed on task</b>
                            </td>
                            <td>
                                &nbsp;<asp:Label ID="lblOverallTime" runat="server" TabIndex="10" CssClass="time"></asp:Label>
                                <span style="float: right; font-weight: bold">dd:hh:mm:ss </span>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                    <asp:AsyncPostBackTrigger ControlID="btnStart" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <%--</ContentTemplate>
                        <Triggers>
                            
                        </Triggers>
                    </asp:UpdatePanel>--%>
        </div>
        <table cellpadding="4" cellspacing="4" class="style1" border="1">
            <tr>
                <td class="heading">
                    <b>Task Name<span style="font-size: 14px;">*</span></b>
                </td>
                <td>
                    <asp:TextBox ID="txtTaskNm" runat="server" Width="100%" TabIndex="1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtTaskNm"
                        ErrorMessage="Task name is required." Display="None" SetFocusOnError="true" ValidationGroup="Task">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="heading">
                    <b>Task Description</b>
                </td>
                <td>
                    <asp:TextBox ID="txtTaskDesc" runat="server" TextMode="MultiLine" Width="100%" Height="200px"
                        TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="heading">
                    <b>Task Start Date<span style="font-size: 14px;">*</span></b>
                </td>
                <td>
                    <asp:TextBox ID="txtStartDate" runat="server" TextMode="SingleLine" Width="100%"
                        AutoCompleteType="None" TabIndex="3"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtStartDate">
                    </cc1:CalendarExtender>
                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                        ErrorMessage="Start date is required." Display="None" SetFocusOnError="true"
                        ValidationGroup="Task">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="heading">
                    <b>Task End Date<span style="font-size: 14px;">*</span></b>
                </td>
                <td>
                    <asp:TextBox ID="txtEndDate" runat="server" TextMode="SingleLine" Width="100%" TabIndex="4"
                        AutoCompleteType="None"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtEndDate">
                    </cc1:CalendarExtender>
                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate"
                        ErrorMessage="End date is required." Display="None" SetFocusOnError="true" ValidationGroup="Task">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="heading">
                    <b>Assign To<span style="font-size: 14px;">*</span></b>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAssignTo" runat="server" ToolTip="Select a user" TabIndex="5" CausesValidation="true" ValidationGroup="Task">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvddlAssignTo" runat="server" ControlToValidate="ddlAssignTo" InitialValue="---Select---" 
                        ErrorMessage="Assign to is required." Display="None" SetFocusOnError="true" ValidationGroup="Task" >
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="heading">
                    <b>What has been performed on the task?</b>
                </td>
                <td>
                    <asp:TextBox ID="txtTaskPer" runat="server" TextMode="MultiLine" Width="100%" Height="200px"
                        TabIndex="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="heading">
                    <b>What is left to do on the task?</b>
                </td>
                <td>
                    <asp:TextBox ID="txtTaskLeft" runat="server" TextMode="MultiLine" Width="100%" Height="200px"
                        TabIndex="7"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="heading">
                    <b>Task status</b>
                </td>
                <td>
                    &nbsp;<asp:RadioButton ID="rbtnPending" runat="server" GroupName="Task" Text="In Progress"
                        Checked="true" />
                    &nbsp;<asp:RadioButton ID="rbtnComplete" runat="server" GroupName="Task" Text="Completed" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="11" OnClick="btnSave_Click"
                        CausesValidation="true" ValidationGroup="Task" />

                           <asp:Button ID="btnCancel" runat="server" Text="Cancel" TabIndex="12" OnClick="btnCancel_Click"
                        CausesValidation="false" ValidationGroup="Task" />
                </td>
            </tr>
        </table>
    </div>
  
</asp:Content>
