<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="CommissionEntry.aspx.cs" Inherits="RealEstateCRM.CommissionEntry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h5 class="mb-0"><strong>Commission Entry</strong></h5>
    <span class="text-secondary">Dashboard <i class="fa fa-angle-right"></i>&nbsp;Commission Entry</span>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header">Commission Entry</h6>
                    <asp:Label ID="lblstatus" runat="server" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-4 text-right pb-3">
                    <a href="PassbookList.aspx" class="btn btn-round btn-theme"><i class="fa fa-list"></i>&nbsp;Passbook List</a>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Project<span class="text-danger">*</span></label>
                    <asp:DropDownList ID="ddlProjects" runat="server" CssClass="custom-select" OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true">
                        <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Passbook No<span class="text-danger">*</span></label>
                    <asp:DropDownList ID="ddlPassbook" runat="server" CssClass="custom-select" OnSelectedIndexChanged="ddlPassbook_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true">
                        <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Plot Cost<span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtPlotPrice" runat="server" CssClass="form-control form-control-primary" placeholder="Plot Price" Enabled="false" Text="0"></asp:TextBox>
                </div>
                </div>
            <div class="form-group row">                
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Total Commission(30%)</label>
                    <asp:TextBox ID="txtTotalCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Commission Amount" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">TDS(5%)</label>
                    <asp:TextBox ID="txtTDS" runat="server" CssClass="form-control form-control-primary" placeholder="TDS" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Total Eligible Commission</label>
                    <asp:TextBox ID="txtTotalEligibleCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Commission" TextMode="Number" Enabled="false" Text="0"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Director Name</label>
                    <asp:TextBox ID="txtDirectoryName" runat="server" CssClass="form-control form-control-primary" placeholder="Name"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">(%)</label>
                    <asp:TextBox ID="txtDCommissionPercentage" runat="server" CssClass="form-control form-control-primary" placeholder="Commission(%)" TextMode="Number" OnTextChanged="txtDCommission_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Commission</label>
                    <asp:TextBox ID="txtDTCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Total Commission" TextMode="Number" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hdnDTDS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnDECommission" runat="server" Value="0" />
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Deputy Director</label>
                    <asp:TextBox ID="txtDeputyDirectory" runat="server" CssClass="form-control form-control-primary" placeholder="Name"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">(%)</label>
                    <asp:TextBox ID="txtDDCommissionPercentage" runat="server" CssClass="form-control form-control-primary" placeholder="Commission(%)" TextMode="Number" OnTextChanged="txtDDCommissionPercentage_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Commission</label>
                    <asp:TextBox ID="txtDDTCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Total Commission" TextMode="Number" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hdnDDTDS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnDDECommission" runat="server" Value="0" />
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Chief General Manager</label>
                    <asp:TextBox ID="txtCheifGeneralManager" runat="server" CssClass="form-control form-control-primary" placeholder="Name"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">(%)</label>
                    <asp:TextBox ID="txtCGMCommissionPercentage" runat="server" CssClass="form-control form-control-primary" placeholder="Commission(%)" TextMode="Number" OnTextChanged="txtCGMCommissionPercentage_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Commission</label>
                    <asp:TextBox ID="txtCGMTCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Total Commission" TextMode="Number" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hdnCGMTDS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCGMECommission" runat="server" Value="0" />
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">General Manager</label>
                    <asp:TextBox ID="txtGeneralManager" runat="server" CssClass="form-control form-control-primary" placeholder="Name"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">(%)</label>
                    <asp:TextBox ID="txtGMCommissionPercentage" runat="server" CssClass="form-control form-control-primary" placeholder="Commission(%)" TextMode="Number" OnTextChanged="txtGMCommissionPercentage_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Commission</label>
                    <asp:TextBox ID="txtGMTCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Total Commission" TextMode="Number" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hdnGMTDS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnGMECommission" runat="server" Value="0" />
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Deputy General Manager</label>
                    <asp:TextBox ID="txtDeputyGeneralManager" runat="server" CssClass="form-control form-control-primary" placeholder="Name"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">(%)</label>
                    <asp:TextBox ID="txtDGMCommissionPercentage" runat="server" CssClass="form-control form-control-primary" placeholder="Commission(%)" TextMode="Number" OnTextChanged="txtDGMCommissionPercentage_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Commission</label>
                    <asp:TextBox ID="txtDGMTCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Total Commission" TextMode="Number" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hdnDGMTDS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnDGMECommission" runat="server" Value="0" />
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Asst. General Manager</label>
                    <asp:TextBox ID="txtAsstGeneralManager" runat="server" CssClass="form-control form-control-primary" placeholder="Name"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">(%)</label>
                    <asp:TextBox ID="txtAGMCommissionPercentage" runat="server" CssClass="form-control form-control-primary" placeholder="Commission(%)" TextMode="Number" OnTextChanged="txtAGMCommissionPercentage_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Commission</label>
                    <asp:TextBox ID="txtAGMTCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Total Commission" TextMode="Number" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hdnAGMTDS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnAGMECommission" runat="server" Value="0" />
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Cheif Marketing Manager</label>
                    <asp:TextBox ID="txtCheifMarketingManager" runat="server" CssClass="form-control form-control-primary" placeholder="Name"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">(%)</label>
                    <asp:TextBox ID="txtCMMCommissionPercentage" runat="server" CssClass="form-control form-control-primary" placeholder="Commission(%)" TextMode="Number" OnTextChanged="txtCMMCommissionPercentage_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Commission</label>
                    <asp:TextBox ID="txtCMMTCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Total Commission" TextMode="Number" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hdnCMMTDS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCMMECommission" runat="server" Value="0" />
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Marketing Manager</label>
                    <asp:TextBox ID="txtMarketingManager" runat="server" CssClass="form-control form-control-primary" placeholder="Name"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">(%)</label>
                    <asp:TextBox ID="txtMMCommissionPercentage" runat="server" CssClass="form-control form-control-primary" placeholder="Commission(%)" TextMode="Number" OnTextChanged="txtMMCommissionPercentage_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Commission</label>
                    <asp:TextBox ID="txtMMTCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Total Commission" TextMode="Number" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hdnMMTDS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnMMECommission" runat="server" Value="0" />
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Marketing Officer</label>
                    <asp:TextBox ID="txtMarketingOfficer" runat="server" CssClass="form-control form-control-primary" placeholder="Name"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">(%)</label>
                    <asp:TextBox ID="txtMOCommissionPercentage" runat="server" CssClass="form-control form-control-primary" placeholder="Commission(%)" TextMode="Number" OnTextChanged="txtMOCommissionPercentage_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Commission</label>
                    <asp:TextBox ID="txtMOTCommission" runat="server" CssClass="form-control form-control-primary" placeholder="Total Commission" TextMode="Number" Enabled="false" Text="0"></asp:TextBox>
                </div>
                <asp:HiddenField ID="hdnMOTDS" runat="server" Value="0" />
                <asp:HiddenField ID="hdnMOECommission" runat="server" Value="0" />
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                   
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Total (%)</label>
                    <asp:TextBox ID="txtGrandPercentage" runat="server" CssClass="form-control form-control-primary" placeholder="Commission(%)" TextMode="Number" OnTextChanged="txtMOCommissionPercentage_TextChanged" AutoPostBack="true" Text="0"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <asp:Button ID="btnSubmit" runat="server" Text="Add" OnClick="btnSubmit_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="btn btn-danger" />
                    <asp:HiddenField ID="hdnPendingAmount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnPaidAmount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnAdvance" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnEligibleAmount" runat="server" Value="0" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
