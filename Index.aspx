<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="RealEstateCRM.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="description" content="Reat Estate Management application" />
    <meta name="keywords" content="Real estate, Real statet magangement" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <!--Meta Responsive tag-->
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />

    <!--Bootstrap CSS-->
    <link rel="stylesheet" href="assets/css/bootstrap.min.css" />
    <!--Custom style.css-->
    <link rel="stylesheet" href="assets/css/quicksand.css" />
    <link rel="stylesheet" href="assets/css/style.css" />
    <!--Font Awesome-->
    <link rel="stylesheet" href="assets/css/fontawesome-all.min.css" />
    <link rel="stylesheet" href="assets/css/fontawesome.css" />

    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->

    <title>Welcome to Vaishnavi Developers</title>
</head>
<body>
    <form id="form1" runat="server">
        <!--Login Wrapper-->

        <div class="container-fluid login-wrapper">
            <div class="login-box">
                <h1 class="text-center mb-5"><i class="fa fa-builing text-primary"></i>&nbsp;Vaishnavi Developers</h1>
                <div class="row">
                    <div class="col-md-6 col-sm-6 col-12 login-box-info">
                        <h3 class="mb-4">Back Office Portal</h3>
                        <p class="mb-4">Welcome to Vaishavi Developers.Please use the office login to enter</p>
                    </div>
                    <div class="col-md-6 col-sm-6 col-12 login-box-form p-4">
                        <h3 class="mb-2">Login</h3>
                        <small class="text-muted bc-description">Sign in with your credentials</small>
                        <div class="mt-2">
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text" id="basic-addon1"><i class="fa fa-user"></i></span>
                                </div>
                                <asp:TextBox ID="txtUsername" CssClass="form-control mt-0" placeholder="Username" runat="server" aria-describedby="basic-addon1"></asp:TextBox>
                            </div>

                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text" id="basic-addon2"><i class="fa fa-lock"></i></span>
                                </div>
                                <asp:TextBox ID="txtPassword" CssClass="form-control mt-0" TextMode="Password" placeholder="Password" runat="server" aria-describedby="basic-addon2"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-theme btn-block p-2 mb-1" OnClick="btnLogin_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--Login Wrapper-->
    </form>
    <!-- Page JavaScript Files-->
    <script src="assets/js/jquery.min.js"></script>
    <script src="assets/js/jquery-1.12.4.min.js"></script>
    <!--Popper JS-->
    <script src="assets/js/popper.min.js"></script>
    <!--Bootstrap-->
    <script src="assets/js/bootstrap.min.js"></script>

    <!--Custom Js Script-->
    <script src="assets/js/custom.js"></script>
    <!--Custom Js Script-->
</body>
</html>
