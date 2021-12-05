<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="Plots.aspx.cs" Inherits="RealEstateCRM.Plots" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h5 class="mb-0"><strong>Plots</strong></h5>
    <span class="text-secondary">Home <i class="fa fa-angle-right"></i>&nbsp;Plots</span>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header">Plot Details</h6>
                     <asp:Label ID="lblstatus" runat="server"></asp:Label>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Project</label>
                    <asp:DropDownList ID="ddlProjects" runat="server" CssClass="custom-select" OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                      <asp:RequiredFieldValidator runat="server"  ID="rfvProjects" ErrorMessage="Please Select Project" ControlToValidate="ddlProjects" InitialValue="0" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">PlotNo</label>
                    <asp:TextBox ID="txtPlotNo" runat="server" CssClass="form-control form-control-primary" placeholder="Plot No" TextMode="Number" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPlotNo" runat="server" ErrorMessage="Please Enter PlotNo" ControlToValidate="txtPlotNo" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>

                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="custom-select">
                        <asp:ListItem Text="Available" Value="A"></asp:ListItem>
                        <asp:ListItem Text="Sold" Value="S"></asp:ListItem>
                        <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                        <asp:ListItem Text="Registered " Value="R"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Plot Size(Sq Yards)</label>
                    <asp:TextBox ID="txtSize" runat="server" CssClass="form-control form-control-primary" placeholder="Plot Size in Sq.Yards" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSize" runat="server" ErrorMessage="Please Enter Size" ControlToValidate="txtSize" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Plot Sale Price</label>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control form-control-primary" placeholder="Amount in INR" TextMode="Number"></asp:TextBox>
                </div>
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Plot Facing</label>
                    <asp:DropDownList ID="ddlFacing" runat="server" CssClass="custom-select">
                        <asp:ListItem Text="East" Value="E"></asp:ListItem>
                        <asp:ListItem Text="West" Value="W"></asp:ListItem>
                        <asp:ListItem Text="North" Value="N"></asp:ListItem>
                        <asp:ListItem Text="South" Value="S"></asp:ListItem>
                        <asp:ListItem Text="NorthEast" Value="NE"></asp:ListItem>
                        <asp:ListItem Text="SouthEast" Value="SE"></asp:ListItem>
                        <asp:ListItem Text="SouthWest" Value="SW"></asp:ListItem>
                        <asp:ListItem Text="NorthWest" Value="NE"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Maintainance Charges</label>
                    <asp:TextBox ID="txtMaintainance" runat="server" CssClass="form-control form-control-primary" placeholder="Maintainance Charges per SQ. Yard" Text="200" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMaintainance" runat="server" ErrorMessage="Please Enter Maintainance Charges " ControlToValidate="txtMaintainance" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                 <div class="col-sm-4 col-12">
                    <label for="validationCustom01">Facing Charges</label>
                    <asp:TextBox ID="txtFacingCharges" runat="server" CssClass="form-control form-control-primary" placeholder="Facing Charges" Text="200" TextMode="Number"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-4 col-12">
                    <asp:Button ID="btnSubmit" runat="server" Text="Add" OnClick="btnSubmit_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" CssClass="btn btn-primary" Visible="false" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="btn btn-danger" />
                </div>
            </div>
        </div>
    </div>
    <div class="mt-4 mb-4 p-3 bg-white border shadow-sm lh-sm">
        <div class="product-list">
            <div class="row border-bottom mb-4">
                <div class="col-sm-8 pt-2">
                    <h6 class="mb-4 bc-header">Plots</h6>
                </div>
            </div>
            <div class="table-responsive product-list" id="htmlDiv" runat="server">
            </div>
        </div>
    </div>
</asp:Content>
