using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RealEstateCRM
{
    public partial class SuperAdmin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RoleName"] != null)
            {
                GenerateMenu();
            }
            else
            {
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            
        }
        private void GenerateMenu()
        {
            string htmlData = string.Empty;
            htmlData += "<ul class='sidebar-menu mt-4 mb-4'>";
            htmlData += "<li class='parent'>" +
                                    "<a href='Dashboard.aspx'><i class='fa fa-dashboard mr-3'></i>" +
                                        "<span class='none'>Dashboard <i class='pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                "</li>";
            htmlData += "<li class='parent'>" +
                                    "<a href='#' onclick='toggle_menu('projects'); return false'><i class='fa fa-tasks mr-3'></i>" +
                                        "<span class='none'>Projects <i class='fa fa-angle-down pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                    "<ul class='children' id='projects'>";
            htmlData += "<li class='child'><a href='Projects.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Projects</a></li>";
            htmlData += "<li class='child'><a href='Plots.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Plots</a></li>";
            htmlData += "<li class='child'><a href='PassbookList.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Passbook</a></li>";
            htmlData += "<li class='child'><a href='PDocuments.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Documents</a></li>";
            htmlData += "<li class='child'><a href='CommissionEntry.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Commission Entry</a></li>";
            htmlData += "<li class='child'><a href='CancellationsList.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Cancellations</a></li>";
            htmlData += "</ul>" +
                                "</li>";
                                htmlData += "<li class='parent'>"+
            "<a href='#' onclick='toggle_menu('Payments'); return false'><i class='fa fa-tasks mr-3'></i>" +
                "<span class='none'>Payments <i class='fa fa-angle-down pull-right align-bottom'></i></span>" +
            "</a>" +
            "<ul class='children' id='Payments'>";
            htmlData += "<li class='child'><a href='PlotPayments.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Plot Payments</a></li>";
            htmlData += "<li class='child'><a href='CommissionPayments.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Commission Payments</a></li>";
            htmlData += "</ul>" +
                                "</li>";
            if (Session["RoleName"].ToString() == "SuperAdmin")
            {
                htmlData += "<li class='parent'>" +
                                    "<a href='Users.aspx'><i class='fa fa-users mr-3'></i>" +
                                        "<span class='none'>Users <i class='pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                "</li>";
            }
            htmlData += "<li class='parent'>" +
                                    "<a href='Logout.aspx'><i class='fa fa-sign-out mr-3'></i>" +
                                        "<span class='none'>Logout <i class='pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                "</li>";
            htmlData += "</ul>";
            htmlMenu.InnerHtml = htmlData;
        }
    }
}