<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="CommissionPayments.aspx.cs" Inherits="RealEstateCRM.CommissionPayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <h5 class="mb-0"><strong>Commissions Payments</strong></h5>
    <span class="text-secondary">Dashboard <i class="fa fa-angle-right"></i>&nbsp;Commissions Payments</span>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header">Commissions Payments List</h6>
                </div>
                <div class="col-sm-4 text-right pb-3">
                    <a href="CommissionPaymentDetails.aspx" class="btn btn-round btn-theme"><i class="fa fa-plus"></i>&nbsp;Commissions Details</a>
                </div>
            </div>
            <div class="table-responsive product-list" id="htmlDiv" runat="server">
            </div>
        </div>
    </div>
</asp:Content>
