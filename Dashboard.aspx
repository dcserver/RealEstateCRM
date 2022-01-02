<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="RealEstateCRM.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h5 class="mb-3"><strong>Dashboard</strong></h5>
    <!--Dashboard widget-->
    <div class="mt-1 mb-3 button-container">
        <div class="row pl-0" id="htmlDiv" runat="server">            
        </div>
    </div>
    <div class="mt-1 mb-3 button-container">
        <div class="row pl-0" id="htmlPlotDiv" runat="server">            
        </div>
    </div>
</asp:Content>
