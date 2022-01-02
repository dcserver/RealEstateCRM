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
                if (Session["RoleName"].ToString() == "SuperAdmin")
                {
                    GenerateSuperAdminMenu();
                }
                else if (Session["RoleName"].ToString() == "Admin")
                {
                    GenerateAdminMenu();
                }
                else
                {
                    Response.Redirect("~/Index.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                Response.Redirect("~/Index.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            
        }
        private void GenerateAdminMenu()
        {
            string htmlData = string.Empty;
            htmlData += "<ul class='sidebar-menu mt-4 mb-4'>" +
                                "<li class='parent'>" +
                                    "<a href='Dashboard.aspx'><i class='fa fa-dashboard mr-3'></i>" +
                                        "<span class='none'>Dashboard <i class='pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                "</li>" +
                                "<li class='parent'>" +
                                    "<a href='#' onclick='toggle_menu('projects'); return false'><i class='fa fa-tasks mr-3'></i>" +
                                        "<span class='none'>Projects <i class='fa fa-angle-down pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                    "<ul class='children' id='projects'>" +
                                        "<li class='child'><a href='Projects.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Projects</a></li>" +
                                        "<li class='child'><a href='Plots.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Plots</a></li>" +
                                        "<li class='child'><a href='PassbookList.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Passbook</a></li>" +
                                        "<li class='child'><a href='PDocuments.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Documents</a></li>" +
                                        "<li class='child'><a href='CommissionEntry.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Commission Entry</a></li>" +
                                        "<li class='child'><a href='CancellationsList.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Cancellations</a></li>" +
                                    "</ul>" +
                                "</li>" +
                                "<li class='parent'>" +
                                    "<a href='#' onclick='toggle_menu('Payments'); return false'><i class='fa fa-tasks mr-3'></i>" +
                                        "<span class='none'>Payments <i class='fa fa-angle-down pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                    "<ul class='children' id='Payments'>" +
                                        "<li class='child'><a href='PlotPayments.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Plot Payments</a></li>" +
                                        "<li class='child'><a href='CommissionPayments.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Commission Payments</a></li>" +
                                    "</ul>" +
                                "</li>" +
                                "<li class='parent'>" +
                                    "<a href='Logout.aspx'><i class='fa fa-sign-out mr-3'></i>" +
                                        "<span class='none'>Logout <i class='pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                "</li>" +
                            "</ul>";
            htmlMenu.InnerHtml = htmlData;
        }
        private void GenerateSuperAdminMenu()
        {
            string htmlData = string.Empty;
            htmlData += "<ul class='sidebar-menu mt-4 mb-4'>" +
                                "<li class='parent'>" +
                                    "<a href='Dashboard.aspx'><i class='fa fa-dashboard mr-3'></i>" +
                                        "<span class='none'>Dashboard <i class='pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                "</li>" +
                                "<li class='parent'>" +
                                    "<a href='#' onclick='toggle_menu('projects'); return false'><i class='fa fa-tasks mr-3'></i>" +
                                        "<span class='none'>Projects <i class='fa fa-angle-down pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                    "<ul class='children' id='projects'>" +
                                        "<li class='child'><a href='Projects.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Projects</a></li>" +
                                        "<li class='child'><a href='Plots.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Plots</a></li>" +
                                        "<li class='child'><a href='PassbookList.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Passbook</a></li>" +
                                        "<li class='child'><a href='PDocuments.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Documents</a></li>" +
                                        "<li class='child'><a href='CommissionEntry.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Commission Entry</a></li>" +
                                        "<li class='child'><a href='CancellationsList.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Cancellations</a></li>" +
                                    "</ul>" +
                                "</li>" +
                                "<li class='parent'>" +
                                    "<a href='#' onclick='toggle_menu('Payments'); return false'><i class='fa fa-tasks mr-3'></i>" +
                                        "<span class='none'>Payments <i class='fa fa-angle-down pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                    "<ul class='children' id='Payments'>" +
                                        "<li class='child'><a href='PlotPayments.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Plot Payments</a></li>" +
                                        "<li class='child'><a href='CommissionPayments.aspx' class='ml-4'><i class='fa fa-angle-right mr-2'></i>Commission Payments</a></li>" +
                                    "</ul>" +
                                "</li>" +
                                "<li class='parent'>" +
                                    "<a href='Users.aspx'><i class='fa fa-users mr-3'></i>" +
                                        "<span class='none'>Users <i class='pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                "</li>" +
                                "<li class='parent'>" +
                                    "<a href='Logout.aspx'><i class='fa fa-sign-out mr-3'></i>" +
                                        "<span class='none'>Logout <i class='pull-right align-bottom'></i></span>" +
                                    "</a>" +
                                "</li>" +
                            "</ul>";
            htmlMenu.InnerHtml = htmlData;
        }
    }
}