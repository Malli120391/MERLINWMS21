using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static MRLWMSC21.mMaterialManagement.BOM;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class BOMList : System.Web.UI.Page
    {
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "BOM List");
        }
        [WebMethod]
        public static List<BOMData> GetBOMHeaderData(BOMListSearch obj)
        {
            List<BOMData> olst = new List<BOMData>();
            try
            {
                DataSet ds = DB.GetDS("exec [dbo].[SP_GetBOMHeaderData] @BOMHeaderId=" + 0+ ",@BOMRefNo="+DB.SQuote(obj.BomRefNo)+ ",@Tenantid="+obj.TenantId+ ",@MMid="+obj.MMID+ ",@Accountid="+cp1.AccountID, false);
                int sno = 0;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new BOMData()
                    {
                        SNO = ++sno,
                        BOMRefNo = (row["BOMRefNo"]).ToString(),
                        TenantId = (row["TenantID"]).ToString(),
                        Tenant = (row["TenantCode"]).ToString(),
                        MCode = (row["MCode"]).ToString(),
                        MMID = (row["MaterialMasterID"]).ToString(),
                        UOM = (row["UOM"]).ToString(),
                        UOMID = (row["MaterialMaster_UOMId"]).ToString(),
                        Remarks = (row["remarks"]).ToString(),
                        BOMID = (row["BOMHeaderId"]).ToString(),
                        AccountId = (row["AccountID"]).ToString(),
                        Account = (row["AccountCode"]).ToString(),
                        CreatedDate = (row["createdon"]).ToString(),
                        CreatedUser = (row["FirstName"]).ToString(),
                    });
                }

            }
            catch (Exception e)
            {
                return null;
            }
            return olst;

        }
        public class BOMListSearch
        {
            public string TenantId{get;set;}
            public string Tenant{get;set;}
            public string BomRefNo{get;set;}
            public string PartNo{get;set;}
            public string MMID{get;set;}
        }

    }
}