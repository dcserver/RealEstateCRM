<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="PassbookDetails.aspx.cs" Inherits="RealEstateCRM.PassbookDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h5 class="mb-0"><strong>PassBook Details</strong></h5>
    <span class="text-secondary">Dashboard <i class="fa fa-angle-right"></i>PassBook Details</span>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header">Plot PassBook Details</h6>
                    <asp:Label ID="lblstatus" runat="server"  Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-4 text-right pb-3">
                    <a href="PassbookList.aspx" class="btn btn-round btn-theme"><i class="fa fa-list"></i>&nbsp;PassBook List</a>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Project<span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlProjects" runat="server" CssClass="custom-select" OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="P" AppendDataBoundItems="true">
                            <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvProjects" ErrorMessage="Please Select Project" ControlToValidate="ddlProjects" InitialValue="0" SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ValidationGroup="P"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">PassBook No<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPassbookNo" runat="server" CssClass="form-control form-control-primary" placeholder="PassBookNo" Enabled="false" Text="0"></asp:TextBox>
                    </div>                    
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Plots : <asp:Label ID="lblSize" runat="server"></asp:Label></label>
                        <asp:DropDownList ID="ddlPlots" runat="server" CssClass="custom-select" OnSelectedIndexChanged="ddlPlots_SelectedIndexChanged" AutoPostBack="true"  AppendDataBoundItems="true">
                            <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ErrorMessage="Please Select Plot" ControlToValidate="ddlPlots" InitialValue="0" SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ValidationGroup="P"></asp:RequiredFieldValidator>
                    </div>                    
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">DateOfJoin<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtDateOfJoin" runat="server" CssClass="form-control form-control-primary" placeholder="Date Of Join" ValidationGroup="P" TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDareOfJoin" runat="server" ErrorMessage="Please Enter DateOfJoin" ControlToValidate="txtDateOfJoin" Display="Dynamic" ForeColor="Red" ValidationGroup="P"></asp:RequiredFieldValidator>                        
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Customer Name<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control form-control-primary" placeholder="Name" ValidationGroup="P"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please Enter Customer Name" ControlToValidate="txtName" Display="Dynamic" ForeColor="Red" ValidationGroup="P"></asp:RequiredFieldValidator>
                    </div>                    
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Register Name<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtRegisterName" runat="server" CssClass="form-control form-control-primary" placeholder="Name" ValidationGroup="P"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please Enter Register Name" ControlToValidate="txtRegisterName" Display="Dynamic" ForeColor="Red" ValidationGroup="P"></asp:RequiredFieldValidator>
                    </div>
                </div>
                 <div class="form-group row">
                     <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Nominee</label>
                        <asp:TextBox ID="txtNominee" runat="server" CssClass="form-control form-control-primary" placeholder="Name" ValidationGroup="P"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Mobile</label>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control form-control-primary" placeholder="Mobile" ValidationGroup="P" MaxLength="10"></asp:TextBox>
                    </div>                    
                     <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Address</label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control form-control-primary" placeholder="Customer Address" ValidationGroup="P"></asp:TextBox>                        
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
                        <label for="validationCustom01">Total Plot Amount<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtTotalAmount" TextMode="Number" runat="server" CssClass="form-control form-control-primary" placeholder="Total Amount" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Commission<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtCommission" TextMode="Number" runat="server" CssClass="form-control form-control-primary" placeholder="Commission" ValidationGroup="P" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter MaintainanceCharges" ControlToValidate="txtCommission" Display="Dynamic" ForeColor="Red" ValidationGroup="P"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">TDS(5%)<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtTDS" TextMode="Number" runat="server" CssClass="form-control form-control-primary" placeholder="TDS" ValidationGroup="P" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Enter TDS" ControlToValidate="txtTDS" Display="Dynamic" ForeColor="Red" ValidationGroup="P"></asp:RequiredFieldValidator>
                    </div>                                      
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Eligibility<span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtEligibility" TextMode="Number" runat="server" CssClass="form-control form-control-primary" placeholder="Total Eligibility" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Adjustment</label>
                        <asp:TextBox ID="txtAdjustment" runat="server" CssClass="form-control form-control-primary" placeholder="Total Adjustment" OnTextChanged="txtAdjustment_TextChanged" AutoPostBack="true" TextMode="Number"></asp:TextBox>
                    </div>
                    <div class="col-sm-4 col-12">
                        <label for="validationCustom01">Total Commission</label>
                        <asp:TextBox ID="txtFinalComission" runat="server" CssClass="form-control form-control-primary" placeholder="Total Commission" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-4 col-12">
                        <asp:Button ID="btnSubmit" runat="server" Text="Add Plot Entry" OnClick="btnSubmit_Click" CssClass="btn btn-primary" ValidationGroup="P" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" CssClass="btn btn-primary" Visible="false" ValidationGroup="P" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="btn btn-danger" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
