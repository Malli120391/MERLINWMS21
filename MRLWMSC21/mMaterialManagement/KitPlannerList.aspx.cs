using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Data;
using System.Security.Principal;
using Neodynamic;
using System.Web.Services;

// Module Name : Material Management
// Usecase Ref.: Kit Planner_UC_005
// DevelopedBy : Naresh P
// Created On  : 05/10/2013
// Modified On : 24/03/2015


namespace MRLWMSC21.mMaterialManagement
{
    public partial class KitPlannerList : System.Web.UI.Page
    {
        protected string selectKPParentList = "";
        protected string selectKPChildList = "";
        private CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected int CreatedBy = 0;
        protected int TenantID = 0;
        protected int RequestedBy = 0;
        private String SOCheck = "", POCheck = "";
        public static CustomPrincipal cp1 = null;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "MasterData";
            cp1 = HttpContext.Current.User as CustomPrincipal;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Kit Planner List");
            }

        }


       


      

        [WebMethod]
        public static List<KITHeader> GetKitPlannerList(KitListSearch obj)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<KITHeader> olst = new List<KITHeader>();
            if(obj.MMID=="")
            {
                obj.MMID = "0";
            }
            try
            {
                DataSet ds = DB.GetDS("[dbo].[sp_MMT_GetKitPlannerParentList] @TenantID="+ obj.TenantId+ ",@MMid="+ obj.MMID+ ",@AccountID_New="+cp1.AccountID, false);
                int SNO = 0;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new KITHeader()
                    {
                        SNO = ++SNO,
                        KItId = (row["KitPlannerID"]).ToString(),
                        KitCode = (row["KitCode"]).ToString(),
                        tenantId = (row["TenantID"]).ToString(),
                        tenant = (row["CompanyName"]).ToString(),
                        Description = (row["MDescription"]).ToString(),
                        KitPartNo = (row["MCode"]).ToString(),
                        KitTypeid = (row["KitTypeID"]).ToString(),
                        KitType = (row["kittype"]).ToString(),
                        UOM = (row["BUoM"]).ToString(),
                        CreatedUser = (row["FirstName"]).ToString(),
                        CreatedDate = (row["createdon"]).ToString()





                    });
                }

            }
            catch (Exception e)
            {
                return null;
            }
            return olst;

        }
        public class KitListSearch
        {
            public string TenantId { get; set; }
            public string Tenant { get; set; }
            public string PartNo { get; set; }
            public string MMID { get; set; }
            public string KitCode { get; set; }
            public string KitId { get; set; }

           
        }
        public class KITHeader
        {
            public int SNO;
            public string KItId { get; set; }
            public string KitCode { get; set; }
            public string tenantId { get; set; }
            public string tenant { get; set; }
            public string KitPartNo { get; set; }
            public string Description { get; set; }
            public string UOM { get; set; }
            public string KitType { get; set; }
            public string KitTypeid { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedUser { get; set; }

        }

    }
}