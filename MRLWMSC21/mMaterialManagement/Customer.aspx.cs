/////////////////////////////////////////////////////
// Author     : Gopi Vurukuti 
// Created On : 27 Dec 2017  
// Description: To Create Storage Locations.

//---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class Customer : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            
             Page.Theme = "Orders";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "New Customer");
            }
        }

        [WebMethod]
        public static string GetCustomerList()
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            string Json = "";
            string selectMMList;
            try
            {
                //Account and Tenant filteration

                CustomPrincipal customp = HttpContext.Current.User as CustomPrincipal;
                selectMMList = "Exec USP_GEN_GetCustomerInfo ";
                selectMMList = selectMMList + "@AccountID_New = " + customp.AccountID.ToString() + ",@UserTypeID_New = " + customp.UserTypeID.ToString() + ",@TenantID_New = " + customp.TenantID.ToString() + ",@UserID_New=" + customp.UserID.ToString();

                DataSet ds = DB.GetDS(selectMMList, false);
                //DataSet ds = DB.GetDS("Exec USP_GEN_GetCustomerInfo ", false);
                Json = JsonConvert.SerializeObject(ds);
            }

            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }
        [WebMethod]
        public static int SetCustomer(int CusId, string Inxml)
        {


            CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
            int Status = 0;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[USP_GEN_UpsertCustomer] ");
                sb.Append(" @inputDataXml='" + Inxml + "'");
                sb.Append(" ,@CustomerID=" + CusId);
                sb.Append(" ,@CreatedBy=" + customprinciple.UserID.ToString());
                Status= DB.GetSqlN(sb.ToString());
                return Status;
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
                
            }
            return Status;
        }

        [WebMethod]
        //public static string SetAddress(int AddressId, string Inxml, float Latitude, float Longitude,string DeliveryPoint)
        //{
        //    string Status = "";
        //    string latitude = Latitude.ToString();
        //    string longitude = Longitude.ToString();
        //    CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append(" Exec [dbo].[GEN_MST_SET_Addresses]");
        //        sb.Append(" @inputDataXml='" + Inxml + "'");
        //        sb.Append(" ,@GEN_MST_Address_ID=" + AddressId + ",@Latitude=" + latitude + ",@Longitude=" + longitude+ ",@DeliveryPoint='"+ DeliveryPoint + "',@UpdatedBy=" + customprinciple.UserID.ToString()+"");
        //        DB.ExecuteSQL(sb.ToString());
        //        Status = "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        Status = "Failed";
        //    }
        //    return Status;
        //}

        public static string SetAddress(int AddressId, string Inxml, string DeliveryPoint)
        {
            string Status = "";
            //string latitude = Latitude.ToString();
            //string longitude = Longitude.ToString();
            CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[GEN_MST_SET_Addresses]");
                sb.Append(" @inputDataXml='" + Inxml + "'");
                //sb.Append(" ,@GEN_MST_Address_ID=" + AddressId + ",@Latitude=" + latitude + ",@Longitude=" + longitude+ ",@DeliveryPoint='"+ DeliveryPoint + "',@UpdatedBy=" + customprinciple.UserID.ToString()+"");
                sb.Append(" ,@GEN_MST_Address_ID=" + AddressId + ",@DeliveryPoint='" + DeliveryPoint + "',@UpdatedBy=" + customprinciple.UserID.ToString() + "");
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }

        [WebMethod]
        public static string DeleteAddress(string StrId)
        {
            string Status;

            int ExistSO = DB.GetSqlN("select Count(ADR.GEN_MST_Address_ID) AS N from GEN_MST_Addresses ADR join ORD_SODetails SOD on ADR.GEN_MST_Address_ID = SOD.GEN_MST_Address_ID where adr.GEN_MST_Address_ID = " + Convert.ToInt32(StrId));
           if(ExistSO>0)
            {
                return "Exist";
            }
              try
            {

                CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[GEN_MST_DELETE_Addresses]");
                sb.Append("@PK=" + StrId);
                sb.Append(",@UpdatedBy=" + customprinciple.UserID.ToString());
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }

            return Status;
        }
    }
}