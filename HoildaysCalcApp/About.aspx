<%@ Page Title="AGAZTY" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="HoildaysCalcApp.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %></h2>
        <p>We take from the employee the start date and the end date of his vacancy, and calculate the working days within those input days to see if could take
            this holiday or he exceeds the possible vacancy days.</p>
    </main>
</asp:Content>
