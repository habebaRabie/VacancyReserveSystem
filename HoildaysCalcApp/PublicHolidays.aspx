<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PublicHolidays.aspx.cs" Inherits="HoildaysCalcApp.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Public Holidays</h1>
    <h4><asp:Literal ID="ltError" runat="server" /></h4>

    <div>
        <h5 class="lead">Add New Public Holiday</h5>
        <div>
            <label for="newHolidayStartDate">Holiday's Date:</label><br />

             <asp:Calendar AutoPostback = "false" EnableViewState="true" ID="newHolidayStartDate" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="250px">    
                 <DayHeaderStyle Font-Bold="True" Font-Size="8pt"></DayHeaderStyle><NextPrevStyle VerticalAlign="Bottom" Font-Bold="True" Font-Size="8pt" ForeColor="#333333"></NextPrevStyle>
                 <OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
                 <SelectedDayStyle BackColor="#333399" ForeColor="White"></SelectedDayStyle>
                 <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="0px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399"></TitleStyle>
                 <TodayDayStyle BackColor="#CCCCCC"></TodayDayStyle>
             </asp:Calendar>
             <br />

        </div>

        <div>
            <label for="newHolidayEndDate">Holiday's Date:</label><br />

             <asp:Calendar AutoPostback = "false" EnableViewState="true" ID="newHolidayEndDate" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="250px">    
                 <DayHeaderStyle Font-Bold="True" Font-Size="8pt"></DayHeaderStyle><NextPrevStyle VerticalAlign="Bottom" Font-Bold="True" Font-Size="8pt" ForeColor="#333333"></NextPrevStyle>
                 <OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
                 <SelectedDayStyle BackColor="#333399" ForeColor="White"></SelectedDayStyle>
                 <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="0px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399"></TitleStyle>
                 <TodayDayStyle BackColor="#CCCCCC"></TodayDayStyle>
             </asp:Calendar>
             <br />

        </div>
        <div>
            <label for="txtNewHolidayDesc">Holiday's Description:</label><br />
            <asp:TextBox ID="txtNewHolidayDesc" runat="server"></asp:TextBox><br />
        </div>
         <br />
        <asp:Button CssClass="btn btn-primary" ID="btnInsertNewHoliday" runat="server" Text="Add New Holiday" OnClick="btnInsertNewHoliday_Click"/><br /><br />
    </div>
    <div>
        <h5 class="lead" >The current Public Holidays found on the system</h5>
        <asp:GridView ID="gvHolidays" CssClass="table table-striped color-table" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvHolidays_RowDeleting" OnRowEditing="gvHolidays_RowEditing" OnRowUpdating="gvHolidays_RowUpdating" OnRowCancelingEdit="gvHolidays_RowCancelingEdit">
            <Columns>
                <asp:BoundField DataField="holiday_desc" HeaderText="Holiday"  />
                <asp:BoundField DataField="holiday_date" HeaderText="Holiday Date" DataFormatString="{0:yyyy-MM-dd}" />
        
                <asp:CommandField ShowEditButton="true" />
                <asp:CommandField ShowDeleteButton="true" />
                
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
