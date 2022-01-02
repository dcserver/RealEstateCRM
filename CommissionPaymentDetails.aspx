<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="CommissionPaymentDetails.aspx.cs" Inherits="RealEstateCRM.CommissionPaymentDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h5 class="mb-0"><strong>Commission Payments Details</strong></h5>
    <span class="text-secondary">Dashboard <i class="fa fa-angle-right"></i>&nbsp;Commission Payment Details</span>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header">Commission Details</h6>
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
                        <asp:DropDownList ID="ddlPassbook" runat="server" CssClass="custom-select" OnSelectedIndexChanged="ddlPassbookNo_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true">
                            <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">VoucherNo</label>
                        <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control form-control-primary" placeholder="VoucherNo"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Marketer<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlEmployees" runat="server" CssClass="custom-select" OnSelectedIndexChanged="ddlEmployees_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true">
                            <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Pending</label>
                        <asp:TextBox ID="txtPendingAmount" runat="server" CssClass="form-control form-control-primary" placeholder="Pending Amount" TextMode="Number" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Paid</label>
                        <asp:TextBox ID="txtPaidAmount" runat="server" CssClass="form-control form-control-primary" placeholder="Paid Amount" Enabled="false"></asp:TextBox>
                    </div>                    
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Advance</label>
                        <asp:TextBox ID="txtAdvance" runat="server" CssClass="form-control form-control-primary" placeholder="Advance Amount" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Eligible Commission</label>
                        <asp:TextBox ID="txtCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Commission" TextMode="Number" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Amount<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control form-control-primary" placeholder="Amount" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="Please Enter Amount" ControlToValidate="txtAmount" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>                    
                </div>                
                    <div class="form-group row">
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
                        <label for="validationCustom01">Cheque or Transaction no<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPaymentReference" runat="server" CssClass="form-control form-control-primary" placeholder="cheque no or transaction no"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPaymentReference" runat="server" ErrorMessage="Please Enter Payment Reference" ControlToValidate="txtPaymentReference" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Payment Details</label>
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control form-control-primary" placeholder="Payment Details"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Payment Type<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="custom-select">
                            <asp:ListItem Text="CommissionPay" Value="CommissionPay"></asp:ListItem>
                            <asp:ListItem Text="Advance" Value="Advance"></asp:ListItem>
                            <asp:ListItem Text="Adjustment" Value="Adjustment"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <asp:Button ID="btnSubmit" runat="server" Text="Add" OnClick="btnSubmit_Click" CssClass="btn btn-primary" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="btn btn-danger" />
                    </div>
                </div>
            </div>
        </div>
        <div class="table-responsive product-list" id="htmlDiv" runat="server"></div>
    </div>
</asp:Content>
