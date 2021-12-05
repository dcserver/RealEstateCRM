<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="RealEstateCRM.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Error</title>    
 <link href="../css/bootstrap.css" rel="stylesheet" type="text/css" />
 <!-- GOOGLE FONT -->
 <link href="https://fonts.googleapis.com/css?family=Poppins%7CQuicksand:500,700" rel="stylesheet" />
<style>      
.error-template {
    padding: 20px 15px;
    text-align: center;
}
.error-actions {
    margin-top:15px;
    margin-bottom:15px;
}
    .error-details {
    color:#fff;
    font-size: 18px;
    }

.error-actions .btn {
     margin-right:10px;
}
    .oops {
    color:#fff;
    }
    </style>
    </head>
<body style="background-color:#44b8dd">
    <form id="form1" runat="server">
        <div>
            <div class="container">
                <div class="row">
                    <div class="error-template">
                        <img src="images/title1.png" />
                        <h2 class="oops">Oops! 404 Not Found</h2>
                        <div class="error-details">
                            Sorry, an error has occured, Requested page not found!
                        </div>
                        <div class="error-ations">
                            <a href="Dashboard.aspx" class="btn btn-primary">
                                <i class="icon-home icon-white"></i>Back to Dashboard </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
