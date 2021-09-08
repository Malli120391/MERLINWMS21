using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRLWMSC21Common;
using System.Data;
using System.Web.UI.WebControls;


namespace MRLWMSC21Common
{
    public sealed class DesignLogic
    {
        // Load Quick Links based on Category
        public static string LoadQuickLinks(String CurUserTypeIDs, int CatID)

        {
            /*
            String vModulePath = "";
            switch (CatID)
            {
                case 1:
                    vModulePath = "~/mInbound";
                    break;
                case 2:
                    vModulePath = "~/mOutbound";
                    break;
                case 3:
                    vModulePath = "~/mInventory";
                    break;
                case 4:
                    vModulePath = "~/mOrders";
                    break;
                case 5:
                    vModulePath = "~/mMaterialManagement";
                    break;
                case 6:
                    vModulePath = "~/mReports";
                    break;

                default:
                    vModulePath = "";
                    break;


            }

            */
            StringBuilder sbMenuLinks = new StringBuilder();

            String IconPath = "../Images/blue_menu_icons/";
            String imgElement = "";
            IDataReader rsMenuLinks = DB.GetRS("EXEC [sp_GetMenuDataForCategory] @CurrentUserTypeIDs =" + DB.SQuote(CurUserTypeIDs) + ", @CategoryID=" + CatID);

            sbMenuLinks.Append("<table cellpadding='0' cellspacing='0' border='0'> <tr>");
            while (rsMenuLinks.Read())
            {
                //Set IconPath if data is not null
                imgElement = DB.RSField(rsMenuLinks, "IconFileName").ToString() == "" ? "" : "<img class='ImgFloat' src='" + IconPath + DB.RSField(rsMenuLinks, "IconFileName") + "' border='0'/>";

                // sbMenuLinks.Append("<td class='HotlinkCell' valign='center'><a href='" + vModulePath + DB.RSField(rsMenuLinks, "Link") + "' target='_blank'>" + imgElement + "</a><td>");
                sbMenuLinks.Append("<td ><a href='" + "../" + DB.RSField(rsMenuLinks, "Link") + "' class='SMenuLink'>" + imgElement + "<div class='SFloatText'>" + DB.RSField(rsMenuLinks, "MenuText") + "</div></a></td>");


            }
            rsMenuLinks.Close();
            sbMenuLinks.Append("</tr></table> ");
            return sbMenuLinks.ToString();

        }


        // Innner Page SubHeader

        public static void SetInnerPageSubHeading(System.Web.UI.Page curPage, String SubHeading)
        {

            ContentPlaceHolder cpHolder = curPage.Master.Master.FindControl("MainContent") as ContentPlaceHolder;
            Literal ltSubHeader = cpHolder.FindControl("ltFormSubHeading") as Literal;
            ltSubHeader.Text = SubHeading;
        }


        public static string LoadQuickLinks(String CurUserTypeIDs, int CatID,int TenantID)
        {
            StringBuilder sbMenuLinks = new StringBuilder();

            String IconPath = "../Images/blue_menu_icons/";
            String imgElement = "";

            
            using(IDataReader rsMenuLinks = DB.GetRS("EXEC [sp_GetChildMenuData] @CurrentUserTypeIDs =" + DB.SQuote(CurUserTypeIDs) + " ,@ParentMenuID=" + CatID + " ,@TenantID=" + TenantID + ",@ChildID=1,@IsHotLink=1"))
                {

                    sbMenuLinks.Append("<table cellpadding='0' cellspacing='0' border='0'> <tr>");
                    while (rsMenuLinks.Read())
                    {
                        //Set IconPath if data is not null
                        imgElement = DB.RSField(rsMenuLinks, "IconFileName").ToString() == "" ? "" : "<img class='ImgFloat' src='" + IconPath + DB.RSField(rsMenuLinks, "IconFileName") + "' border='0'/>";

                        // sbMenuLinks.Append("<td class='HotlinkCell' valign='center'><a href='" + vModulePath + DB.RSField(rsMenuLinks, "Link") + "' target='_blank'>" + imgElement + "</a><td>");
                        sbMenuLinks.Append("<td ><a href='" + "../" + DB.RSField(rsMenuLinks, "Link") + "' class='SMenuLink'>" + imgElement + "<div class='SFloatText'>" + DB.RSField(rsMenuLinks, "MenuText") + "</div></a></td>");

                    }
                    rsMenuLinks.Close();
            }
           
            sbMenuLinks.Append("</tr></table> ");
            return sbMenuLinks.ToString();

        }
    }
}
