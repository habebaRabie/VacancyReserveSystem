<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VacancyType.aspx.cs" Inherits="HoildaysCalcApp.VecType" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Vacancy Types</h1>
    <h4><asp:Literal ID="ltErrorVac" runat="server" /></h4>

    <div>
        <h5 class="lead">Add New Vacancy Type</h5>
        <div>
            <label for="txtnewVacType">Vacancy's Name:</label><br />
            <asp:TextBox ID="txtnewVacType" runat="server"></asp:TextBox><br />

        </div>
        
         <br />
        <asp:Button CssClass="btn btn-primary" ID="btnInsertNewVacancy" runat="server" Text="Add New Vacancy" OnClick="btnInsertNewVacancy_Click"/><br /><br />
    </div>
    <div>
        <h5 class="lead" >The current Vacancy's Types found on the system</h5>
        <asp:GridView ID="gvVacancies" CssClass="table table-striped color-table" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvVacancies_RowDeleting" OnRowEditing="gvVacancies_RowEditing" OnRowUpdating="gvVacancies_RowUpdating" OnRowCancelingEdit="gvVacancies_RowCancelingEdit">
            <Columns>
                <asp:BoundField DataField="vac_ID" HeaderText="Vacancy No"  />
                <asp:BoundField DataField="vac_name" HeaderText="Vacancy"  />
    
                <asp:CommandField ShowEditButton="true" />
                <asp:CommandField ShowDeleteButton="true" />
            
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
