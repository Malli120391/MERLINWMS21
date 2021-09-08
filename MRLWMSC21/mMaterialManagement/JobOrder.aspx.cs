using MRLWMSC21Common;
using MRLWMSC21.mOutbound.BL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class JobOrder : System.Web.UI.Page
    {
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "JOB Order Creation");
            cp1 = HttpContext.Current.User as CustomPrincipal;
           // int LCM = CalculateLCMForBOM(96);
        }
        [WebMethod]
        public static List<DropDownData> GetAccounts()
        {
            List<DropDownData> olst = new List<DropDownData>();
            DataSet ds = DB.GetDS("select accountid,account from GEN_Account where 0=" + cp1.AccountID + " or accountid=" + cp1.AccountID,false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["accountid"]),
                        Value = (row["account"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }

        [WebMethod]
        public static int GetCurrentAccount()
        {
            return cp1.AccountID;
        }

        [WebMethod]
        public static int UpsertJOBOrderHeader(JOBOrderHeader obj)
        {

            try
            {
                //DataSet ds=DB
                JobOrder job = new JobOrder();
                if(Convert.ToInt32(obj.JobOrderTypeId)==2)
                {
                    int LCM = job.CalculateLCMForBOM(Convert.ToInt32(obj.BOMId));
                    if (LCM > Convert.ToInt32(Convert.ToDecimal(obj.Quantity)))
                    {
                        return -1 * LCM;
                    }
                    else if (Convert.ToInt32(obj.Quantity) % LCM != 0)
                    {
                        return -1 * LCM;
                    }
                }
               
                int res = DB.GetSqlN("exec [SP_UpsertJOBOrderHeader] @WareHouseId="+obj.WareHouseId+",@JOBOrderId=" + obj.JOBOrderId + ",@TenantId=" + obj.TenantId + ",@BOMRefID=" + obj.BOMId + ",@JOBOrderTypeID="+obj.JobOrderTypeId+",@Quantity=" + obj.Quantity + ",@CreatedBy=" + cp1.UserID);
                return res;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        
        public  int CalculateLCMForBOM(int BOMHeaderID)
        {
            DataSet ds = DB.GetDS("select BOMQuantity from BOMDetails where Isdeleted=0 and IsActive=1 and BOMHeaderId=" + BOMHeaderID, false);
            int a=0;int b=0; int LCMVal = 0;
            try
            {
                for (int i= 0;i < ds.Tables[0].Rows.Count;i++)
                {
                    
                    if (ds.Tables[0].Rows.Count==1)
                    {
                        LCMVal= Convert.ToInt32(ds.Tables[0].Rows[i]["BOMQuantity"]);
                    }
                   else if(i==0)
                    {
                        a = Convert.ToInt32(ds.Tables[0].Rows[i]["BOMQuantity"]);
                        continue;
                    }
                   
                    else
                    { 
                        b= Convert.ToInt32(ds.Tables[0].Rows[i]["BOMQuantity"]);
                        LCMVal=findLCM(a, b);
                        a = LCMVal;
                    }
                   

                }
            }
            catch (Exception e)
            {
                return 0;
            }
            return LCMVal;


        }
        //method for finding LCM with parameters a and b
        public int findLCM(int a, int b) 
        {
            int num1, num2;                         //taking input from user by using num1 and num2 variables
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (int i = 1; i <= num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return i * num1;
                }
            }
            return num2;
        }
        [WebMethod]
        public static JOBOrderHeader GetJOBHeaderData(int jobid)
        {
            try
            {
                DataSet ds = DB.GetDS("[dbo].[SP_GetJOBHeaderData] @JOBOrderHeaderID=" + jobid, false);
                DataTable dtPrimaryInfo = ds.Tables[0];
                JOBOrderHeader obj = new JOBOrderHeader();
                if (dtPrimaryInfo.Rows.Count > 0)
                {
                    obj.JOBOrderId = dtPrimaryInfo.Rows[0]["JOBOrderHeaderID"].ToString();
                    obj.JOBRefNo = dtPrimaryInfo.Rows[0]["joborderrefno"].ToString();
                    obj.TenantId = dtPrimaryInfo.Rows[0]["TenantID"].ToString();
                    obj.Tenant = dtPrimaryInfo.Rows[0]["TenantName"].ToString();
                    obj.AccountId = dtPrimaryInfo.Rows[0]["AccountID"].ToString();
                    obj.Account = dtPrimaryInfo.Rows[0]["Account"].ToString();
                    obj.BOMId = dtPrimaryInfo.Rows[0]["bomheaderid"].ToString();
                    obj.BOMRefNo = dtPrimaryInfo.Rows[0]["bomrefno"].ToString();
                    obj.Quantity =Convert.ToInt32( dtPrimaryInfo.Rows[0]["quantity"]);
                    obj.JobOrderTypeId = dtPrimaryInfo.Rows[0]["JOBOrderTypeID"].ToString();
                    obj.Status = dtPrimaryInfo.Rows[0]["KitJobOrderStatus"].ToString();
                    obj.StatusId = dtPrimaryInfo.Rows[0]["JobOrderStatusID"].ToString();
                    obj.WareHouseId = Convert.ToInt32(dtPrimaryInfo.Rows[0]["WareHouseId"].ToString());
                }
                return obj;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        [WebMethod]
        public static ReleaseoutboundResult InitiateOutbound(int jobid)
        {
            ReleaseoutboundResult rst = new ReleaseoutboundResult();
            try
            {
                if(DB.GetSqlN("select count(*) as N from MMT_KitJobOrderHeader where JobOrderStatusID>1 and JOBOrderHeaderID="+ jobid)>0)
                {
                    rst.Status = -4;
                    return rst;
                }
                string Query = "Exec  [dbo].[SP_InitiateOutwardForJOBOrder] @JOBOrderHeaderId=" + jobid + ",@userid=" + cp1.UserID;
                DataSet ds = DB.GetDS(Query, false);
                if (ds.Tables[0].Rows[0]["N"].ToString() == "-1")
                {
                    rst.Status = -1;

                }
                else if (ds.Tables[0].Rows[0]["N"].ToString() == "-2")
                {
                    rst.Status = -2;

                }
                else
                {
                    rst.Status = 1;
                }
                rst.ResultData = DataTableToJSONWithJSONNet(ds.Tables[1]);
            }
            catch (Exception e)
            {

                return null;
            }

            return rst;
        }


        [WebMethod]
        public static ReleaseoutboundResult InitiateInward(int jobid)
        {
            ReleaseoutboundResult rst = new ReleaseoutboundResult();
            try
            {
                if (DB.GetSqlN("select count(*) as N from MMT_KitJobOrderHeader where JobOrderStatusID>3 and JOBOrderHeaderID=" + jobid) > 0)
                {
                    rst.Status = -4;
                    return rst;
                }
                string Query = "Exec  [dbo].[SP_InitiateInwardForJOBOrder] @JOBOrderHeaderId=" + jobid + ",@userid=" + cp1.UserID;
                DataSet ds = DB.GetDS(Query, false);
                //DB.GetDS(Query, false);
                if (ds.Tables[1].Rows[0]["N"].ToString() == "1")
                {
                    rst.Status = 1;

                }
                else if (ds.Tables[1].Rows[0]["N"].ToString() == "-1")
                {
                    rst.Status =-1 ;
                }
                else
                {
                    rst.Status = -2;
                }
               // rst.ResultData = DataTableToJSONWithJSONNet(ds.Tables[1]);
            }
            catch (Exception e)
            {

                rst.Status = -2;
                return rst;
            }

            return rst;
        }


        [WebMethod]
        public static string GetReleaseData(int jobid)
        {
           return  DataTableToJSONWithJSONNet(DB.GetDS("select soh.SONumber,soh.soheaderid,obd.OBDNumber,obd.outboundid,poh.PONumber,poh.poheaderid,inb.inboundid,inb.StoreRefNo from MMT_KitJobOrderHeader job join ORD_SOHeader soh on job.SOHeaderID = soh.SOHeaderID " +
                        "join OBD_Outbound obd on job.OutboundID = obd.OutboundID left join  ORD_POHeader poh on job.POHeaderId = poh.POHeaderID left join  INB_Inbound inb on job.InboundId = inb.InboundID  where JOBOrderHeaderID =" + jobid, false).Tables[0]);
        }
            public static string DataTableToJSONWithJSONNet(DataTable table)
        {


            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        [WebMethod]
        public static List<DropDownListDataFilter> getWareHouseData()
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getWareHouseDataData();
            }
            catch
            {
                return null;
            }

        }
        public class JOBOrderHeader
        {
            public int SNO { get; set; }
            public string JOBOrderId {get;set;} 
            public string JOBRefNo {get;set;} 
            public string TenantId {get;set;} 
            public string Tenant {get;set;} 
            public string AccountId {get;set;} 
            public string Account {get;set;} 
            public string BOMId {get;set;} 
            public string BOMRefNo {get;set;}
            public string Remarks {get;set;} 
            public int Quantity {get;set;} 
            public string JobOrderTypeId {get;set;} 
            public string Status {get;set;} 
            public string StatusId {get;set;} 
            public string CreatedDate { get; set; }
            public string CreatedUser { get; set; }
            public string MCode { get; set; }
            public string UOM { get; set; }
            public string JOBOrderType { get; set; }
            public int WareHouseId { get; set; }

        }
    }
}