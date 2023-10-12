<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeReport.aspx.cs" Inherits="HoildaysCalcApp.EmpReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Employee Vacancy Report</h3>
    <div>
        <div>
            <label class="lead" style="color: #333; background-color: #f0f0f0;">Name</label>
            <asp:DropDownList ID="EmpNameDropDown" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="EmpNameDropDown_SelectedIndexChanged">
                <asp:ListItem Text="Select an Employee" Value="" />
            </asp:DropDownList>
        </div>

        
        <div id="pdfContent">
            <div>
                <asp:Label class="lead" style="color: #333; background-color: #f0f0f0; text-align:center" ID="WelcomeEmp" runat="server" EnableViewState="false" />
            </div>
            <br />
            <div>
                <asp:Label style="color: #333; background-color: #f0f0f0; line-height: 0.5;" ID="EmpVacDaysRemain" runat="server" EnableViewState="false" />
            </div>

            <div>
                <asp:GridView ID="gvEmpReport" CssClass="table table-striped color-table" runat="server" AutoGenerateColumns="false">
                    <Columns>
                            <asp:BoundField DataField="vac_name" HeaderText="Vacation Name"  />
                            <asp:BoundField DataField="start_date" HeaderText="Start Date" DataFormatString="{0:yyyy-MM-dd}"  />
                        <asp:BoundField DataField="end_date" HeaderText="End Date"  DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="holiday_days_number" HeaderText="Number of Days" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        
        <div>
            <asp:Button CssClass="btn btn-primary" ID="ExportPDF" runat="server" Text="Export to PDF" OnClientClick="copyAndPrint(); return false;" EnableViewState="false" />
        </div>

        <br />
        <asp:Label class="lead" style="color: #333; background-color: #f0f0f0;" ID="ErrorLabelEmpVac" runat="server" EnableViewState="false" />
    </div>


   <script type="text/javascript">
       function copyAndPrint() {
           var sourceDiv = document.getElementById('pdfContent');
           var newWindow = window.open('', '', 'width=600,height=600');
           newWindow.document.write('<html><head><title>Print</title></head><body>');
           newWindow.document.write(sourceDiv.innerHTML);
           newWindow.document.write('</body></html>');
           newWindow.document.close();
           newWindow.print();
           return false; // Prevent the default postback
       }
   </script>

</asp:Content>


