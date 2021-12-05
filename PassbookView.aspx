<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="PassbookView.aspx.cs" Inherits="RealEstateCRM.PassbookView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h5 class="mb-0"><strong>PassBook Details</strong></h5>
    <span class="text-secondary">Dashboard <i class="fa fa-angle-right"></i>&nbsp;PassBook Details</span>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header"><strong>Plot PassBook Details</strong></h6>
                </div>
                <div class="col-sm-2 text-right pb-3">
                    <a href="PassbookList.aspx" class="btn btn-round btn-theme"><i class="fa fa-list"></i>&nbsp;PassBook List</a>
                </div>
                <div class="col-sm-2 text-right pb-3">
                    <asp:Button ID="btnDownload" runat="server" Text="Download Passbook" OnClick="btnDownload_Click" CssClass="btn btn-round btn-theme" Visible="false" />
                </div>
            </div>
            <div class="col-sm-12">
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">PassBook No : </label>
                        <asp:Label ID="lblPassBookNo" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Plot No : </label>
                        <asp:Label ID="lblPlotNo" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Customer Name : </label>
                        <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Facing : </label>
                        <asp:Label ID="lblFacing" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Size(Sq Yards): </label>
                        <asp:Label ID="lblSize" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Plot Cost : </label>
                        <asp:Label ID="lblPlotCost" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Maintainance : </label>
                        <asp:Label ID="lblMaintainance" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Facing Charges: </label>
                        <asp:Label ID="lblFacingCharges" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Total Cost : </label>
                        <asp:Label ID="lblTotalCost" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header"><strong>Commission Account</strong></h6>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Total Commission : </label>
                    <asp:Label ID="lblTotalCommission" runat="server"></asp:Label>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">TDS(5%): </label>
                    <asp:Label ID="lblTDS" runat="server"></asp:Label>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Eligibility : </label>
                    <asp:Label ID="lblEligibility" runat="server"></asp:Label>
                </div>
            </div>
            <div class="form-group row">                
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Final Commission: </label>
                    <asp:Label ID="lblFinalCommission" runat="server"></asp:Label>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">ProjectName : </label>
                    <asp:Label ID="lblProjectName" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header"><strong>Commission Entry</strong></h6>
                </div>
            </div>
        </div>
        <div class="table-responsive product-list" id="htmlCDiv" runat="server"></div>
    </div>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header"><strong>Commission Payments</strong></h6>
                </div>
            </div>
        </div>
        <div class="table-responsive product-list" id="htmCPDiv" runat="server"></div>
    </div>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header"><strong>Plot Payments</strong></h6>
                </div>
            </div>
        </div>
        <div class="table-responsive product-list" id="htmlPDiv" runat="server"></div>
    </div>
     <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header"><strong>Passbook Documents</strong></h6>
                </div>
            </div>
        </div>
        <div class="table-responsive product-list" id="htmlDDiv" runat="server"></div>
    </div>
</asp:Content>
