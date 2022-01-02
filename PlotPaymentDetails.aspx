<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="PlotPaymentDetails.aspx.cs" Inherits="RealEstateCRM.PlotPaymentDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h5 class="mb-0"><strong>Plot Payments Details</strong></h5>
    <span class="text-secondary">Dashboard <i class="fa fa-angle-right"></i>&nbsp;Plot Payment Details</span>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header">Plot Payment Details</h6>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Project<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlProjects" runat="server" CssClass="custom-select" OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true">
                            <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Passbook No<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlPassbook" runat="server" CssClass="custom-select" OnSelectedIndexChanged="ddlPassbook_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvProjects" ErrorMessage="Please Select Passbook No" ControlToValidate="ddlPassbook" InitialValue="0" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">ReceiptNo<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control form-control-primary" placeholder="ReceiptNo"></asp:TextBox>
                    </div>                    
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Total Plot Amount<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtTotalPlotamount" runat="server" CssClass="form-control form-control-primary" placeholder="Total Plot Amount" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Facing Charges<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtFacing" runat="server" CssClass="form-control form-control-primary" placeholder="Facing" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Maintainance<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtMaintainance" runat="server" CssClass="form-control form-control-primary" placeholder="Maintainance" Enabled="false"></asp:TextBox>
                    </div>                   
                </div>
                <div class="form-group row">
                     <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Pending Amount<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPendingAmount" runat="server" CssClass="form-control form-control-primary" placeholder="Pending Amount" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Customer Name<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control form-control-primary" placeholder="CustomerName" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Mobile<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control form-control-primary" placeholder="Mobile" Enabled="false"></asp:TextBox>
                    </div>                    
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Plot Size<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtSize" runat="server" CssClass="form-control form-control-primary" placeholder="Size" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Payment Method<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="custom-select">
                            <asp:ListItem Text="GooglePay" Value="GooglePay"></asp:ListItem>
                            <asp:ListItem Text="PhonePay" Value="PhonePay"></asp:ListItem>
                            <asp:ListItem Text="Internet Bank" Value="InternetBank"></asp:ListItem>
                            <asp:ListItem Text="Cheque" Value="Cheque"></asp:ListItem>
                            <asp:ListItem Text="Paytm" Value="Paytm"></asp:ListItem>
                            <asp:ListItem Text="UPI" Value="UPI"></asp:ListItem>
                            <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Cheque or Transaction<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPaymentReference" runat="server" CssClass="form-control form-control-primary" placeholder="cheque no or transaction no"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPaymentReference" runat="server" ErrorMessage="Please Enter PaymentReference" ControlToValidate="txtPaymentReference" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>                    
                </div>
                <div class="form-group row">
                        <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Amount<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control form-control-primary" placeholder="Amount" TextMode="Number"></asp:TextBox>
                    </div>
                    </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <asp:Button ID="btnSubmit" runat="server" Text="Add" OnClick="btnSubmit_Click" CssClass="btn btn-primary" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="btn btn-danger" />
                        <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                        <asp:HiddenField ID="hdnAmount" runat="server" />
                        <asp:HiddenField ID="hdnPlotNo" runat="server" />
                        <asp:HiddenField ID="hdnPendingAmount" runat="server" />
                        <asp:HiddenField ID="hdnpdfReceipt" runat="server" />
                        <asp:HiddenField ID="hdnpdfPaymentDate" runat="server" />
                        <asp:HiddenField ID="hdnpdfAmount" runat="server" />
                        <asp:HiddenField ID="hdnpdfpdfPassbookNo" runat="server" />
                        <asp:HiddenField ID="hdnpdfPassbookId" runat="server" />
                        <asp:HiddenField ID="hdnpdfPaymentDetails" runat="server" />
                        <asp:HiddenField ID="hdnpdfPaymentMethod" runat="server" />
                        <asp:HiddenField ID="hdnpdfProjectId" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="table-responsive product-list" id="htmlDiv" runat="server"></div>
        <asp:Label ID="lblBalance" runat="server" Visible="false"></asp:Label>
    </div>
</asp:Content>
