using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static MRLWMSC21.mMaterialManagement.JobOrder;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class JobOrderList : System.Web.UI.Page
    {
        public static CustomPrincipal cp1 = null;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "JOB Order List");
        }
        [WebMethod]
        public static List<JOBOrderHeader> GetJOBHeaderData(JOBListSearch obj)
        {
            List<JOBOrderHeader> olst = new List<JOBOrderHeader>();
            try
            {
                DataSet ds = DB.GetDS("[dbo].[SP_GetJOBHeaderData] @JOBOrderHeaderID=" + obj.JOBID+ ",@TenantId="+obj.TenantId+ ",@BOMRefID="+obj.BOMID+ ",@Accountid="+cp1.AccountID, false);
                int sno = 0;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new JOBOrderHeader()
                    {
                        SNO = ++sno,
                        JOBOrderId = (row["JOBOrderHeaderID"]).ToString(),
                        JOBRefNo = (row["joborderrefno"]).ToString(),
                        TenantId = (row["TenantID"]).ToString(),
                        Tenant = (row["TenantName"]).ToString(),
                        AccountId = (row["AccountID"]).ToString(),
                        Account = (row["Account"]).ToString(),
                        BOMId = (row["bomheaderid"]).ToString(),
                        BOMRefNo = (row["bomrefno"]).ToString(),
                        Quantity =Convert.ToInt32 (row["quantity"]),
                        JobOrderTypeId = (row["AccountID"]).ToString(),
                        Status = (row["KitJobOrderStatus"]).ToString(),
                        StatusId = (row["JobOrderStatusID"]).ToString(),
                        CreatedDate = (row["createdon"]).ToString(),
                        CreatedUser = (row["FirstName"]).ToString(),
                        MCode = (row["MCode"]).ToString(),
                        UOM = (row["UOM"]).ToString(),
                        JOBOrderType = Convert.ToInt32(row["JOBOrderTypeID"]) == 1 ? "Nesting" : "De Nesting",




                    });
                }

            }
            catch (Exception e)
            {
                return null;
            }
            return olst;

        }
        public class JOBListSearch
        {
            public string TenantId { get; set; }
            public string Tenant { get; set; }
            public string BomRefNo { get; set; }
            public string BOMID { get; set; }
            public string JOBID { get; set; }
            public string JOBRefNo { get; set; }
        }
    }
}