<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorkingDaysCalc.aspx.cs" Inherits="HoildaysCalcApp.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main aria-labelledby="title">
        <h3>Working days calculator</h3>
        <div>
            <div>
                <label class="lead" style="color: #333; background-color: #f0f0f0;">Name</label>
                <asp:TextBox ID="EmployeeNameTextBox" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvName" ControlToValidate="EmployeeNameTextBox" ErrorMessage="Required" Display="Dynamic" runat="server" />

            </div>
            <div>
                <label class="lead" style="color: #333; background-color: #f0f0f0;">Start Date</label>

                <asp:Calendar AutoPostback = "false" EnableViewState="true" ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="250px">    
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt"></DayHeaderStyle><NextPrevStyle VerticalAlign="Bottom" Font-Bold="True" Font-Size="8pt" ForeColor="#333333"></NextPrevStyle>
                    <OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
                    <SelectedDayStyle BackColor="#333399" ForeColor="White"></SelectedDayStyle>
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="0px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399"></TitleStyle>
                    <TodayDayStyle BackColor="#CCCCCC"></TodayDayStyle>
                </asp:Calendar>
                <br />
            </div>

            <div>
                <label class="lead" style="color: #333; background-color: #f0f0f0;">End Date</label>

                <asp:Calendar AutoPostback = "false" EnableViewState="true" ID="Calendar2" runat="server" OnSelectionChanged="Calendar2_SelectionChanged" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="250px">    
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt"></DayHeaderStyle><NextPrevStyle VerticalAlign="Bottom" Font-Bold="True" Font-Size="8pt" ForeColor="#333333"></NextPrevStyle>
                    <OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
                    <SelectedDayStyle BackColor="#333399" ForeColor="White"></SelectedDayStyle>
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="0px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399"></TitleStyle>
                    <TodayDayStyle BackColor="#CCCCCC"></TodayDayStyle>
                </asp:Calendar>
                <br />
            </div>
            <div>
                <asp:Button CssClass="btn btn-primary" ID="CalculateButton" runat="server" Text="Calculate the Working Days" OnClick="CalculateButton_Click" /><br />
            </div>
            <br />
            <asp:Label class="lead" style="color: #333; background-color: #f0f0f0;" ID="ResultLabel" runat="server" EnableViewState="false" />
            <div>
                <asp:Button CssClass="btn btn-primary" ID="AddWorkingDaysButton" runat="server" Text="Add Working Days to VacDays" OnClick="AddWorkingDaysButton_Click" Visible="false" />
            </div>
        </div>
    </main>
</asp:Content>

