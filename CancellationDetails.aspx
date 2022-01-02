<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="CancellationDetails.aspx.cs" Inherits="RealEstateCRM.CancellationDetails" %>

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
                    <asp:Label ID="lblstatus" runat="server"></asp:Label>
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
                        <label for="validationCustom01">Plot No<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPlotNo" runat="server" CssClass="form-control form-control-primary" placeholder="Plot No" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">DateOfJoin<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtDateOfJoin" runat="server" CssClass="form-control form-control-primary" placeholder="Date Of Join" ValidationGroup="P" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Customer Name<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control form-control-primary" placeholder="Name" ValidationGroup="P" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Mobile</label>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control form-control-primary" placeholder="Mobile" ValidationGroup="P" MaxLength="10" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Plot Amount<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPlotAmount" TextMode="Number" runat="server" CssClass="form-control form-control-primary" placeholder="Plot Amount" ValidationGroup="P" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTotalAmount" runat="server" ErrorMessage="Please Enter Plot Amount" ControlToValidate="txtPlotAmount" Display="Dynamic" ForeColor="Red" ValidationGroup="P"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Maintainance Charges<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtMaintainanceCharges" TextMode="Number" runat="server" CssClass="form-control form-control-primary" placeholder="Maintainance Charges" ValidationGroup="P" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter MaintainanceCharges" ControlToValidate="txtMaintainanceCharges" Display="Dynamic" ForeColor="Red" ValidationGroup="P"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Facing Charges<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtFacingCharges" TextMode="Number" runat="server" CssClass="form-control form-control-primary" placeholder="Facing Charges" ValidationGroup="P" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter FacingCharges" ControlToValidate="txtFacingCharges" Display="Dynamic" ForeColor="Red" ValidationGroup="P"></asp:RequiredFieldValidator>
                    </div>
                </div>                
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">TotalAmount</label>
                        <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control form-control-primary" placeholder="Paid Amount" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Paid</label>
                        <asp:TextBox ID="txtPaidAmount" runat="server" CssClass="form-control form-control-primary" placeholder="Pending Amount" TextMode="Number" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Pending</label>
                        <asp:TextBox ID="txtPendingAmount" runat="server" CssClass="form-control form-control-primary" placeholder="Pending Amount" TextMode="Number" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Refund Amount</label>
                        <asp:TextBox ID="txtRefundAmount" runat="server" CssClass="form-control form-control-primary" placeholder="Refund Amount"></asp:TextBox>
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
                        <label for="validationCustom01">Cheque or Transaction no<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPaymentReference" runat="server" CssClass="form-control form-control-primary" placeholder="cheque no or transaction no"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPaymentReference" runat="server" ErrorMessage="Please Enter Payment Reference" ControlToValidate="txtPaymentReference" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <asp:Button ID="btnSubmit" runat="server" Text="Cancel Passbook" OnClick="btnSubmit_Click" CssClass="btn btn-primary" />
                        <asp:HiddenField ID="hdnPlotId" runat="server" />
                    </div>
                </div>
            </div>
        </div>
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
</asp:Content>
