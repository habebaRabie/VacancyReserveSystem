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

        <div>
            <asp:Label class="lead" style="color: #333; background-color: #f0f0f0; text-align:center" ID="WelcomeEmp" runat="server" EnableViewState="false" />

        </div>

        <div>
            <h5 class="lead" >The Annual Report for  Vacations</h5>
            <asp:GridView ID="gvEmpReport" CssClass="table table-striped color-table" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvEmpReport_RowDeleting" OnRowEditing="gvEmpReport_RowEditing" OnRowUpdating="gvEmpReport_RowUpdating" OnRowCancelingEdit="gvEmpReport_RowCancelingEdit">
                <Columns>
                     <asp:BoundField DataField="vac_name" HeaderText="Vacation Name"  />
                     <asp:BoundField DataField="start_date" HeaderText="Start Date" DataFormatString="{0:yyyy-MM-dd}"  />
                    <asp:BoundField DataField="end_date" HeaderText="End Date"  DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="holiday_days_number" HeaderText="Number of Days" />
        
                    <asp:CommandField ShowEditButton="true" />
                    <asp:CommandField ShowDeleteButton="true" />
                
                </Columns>
            </asp:GridView>
        </div>

        <br />
        <asp:Label class="lead" style="color: #333; background-color: #f0f0f0;" ID="ErrorLabelEmpVac" runat="server" EnableViewState="false" />

    </div>
</asp:Content>
