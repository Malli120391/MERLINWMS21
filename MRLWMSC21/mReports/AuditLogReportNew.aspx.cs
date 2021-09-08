using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using MRLWMSC21Common;
using System.Data;
using System.Globalization;

namespace MRLWMSC21.mReports
{
    public partial class AuditLogReportNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Audit / Table Audit Report");
            }
        }

        [WebMethod]
        public static List<AuditHeader> GetData(int CategoryID, int RefID)
        {
            //CategoryID = 1;
            //RefID = 2452;
            DataSet ds = DB.GetDS("EXEC [dbo].[USP_HIST_FetchAuditTrail] @OrderTypeID = " + CategoryID + ", @OrderID = " + RefID, false);
            return PrepareAuditList(ds);
        }

        public static List<AuditHeader> PrepareAuditList(DataSet ds)
        {
            List<AuditHeader> oAuditHeaderList = new List<AuditHeader>();
            HashSet<int> hs = new HashSet<int>();
            int HeaderID = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string _sOperation = "";
                HeaderID = Convert.ToInt32(ds.Tables[0].Rows[i]["HIST_TRN_HistoryHeader_ID"]);
                if (!hs.Contains(HeaderID))
                {
                    hs.Add(HeaderID);

                    AuditHeader oAuditHeader = new AuditHeader();
                    oAuditHeader.HIST_TRN_HistoryHeader_ID = HeaderID;
                    oAuditHeader.TableName = ds.Tables[0].Rows[i]["TableName"].ToString();
                    //oAuditHeader.IsDataInserted = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsDataInserted"].ToString());
                    //oAuditHeader.IsDataUpdated = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsDataUpdated"].ToString());
                    //oAuditHeader.IsDataDeleted = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsDataDeleted"].ToString());
                    if (Convert.ToBoolean(ds.Tables[0].Rows[i]["IsDataInserted"].ToString()))
                        _sOperation = "Insert";
                    else if (Convert.ToBoolean(ds.Tables[0].Rows[i]["IsDataUpdated"].ToString()))
                        _sOperation = "Update";
                    else if (Convert.ToBoolean(ds.Tables[0].Rows[i]["IsDataDeleted"].ToString()))
                        _sOperation = "Delete";

                    oAuditHeader.Operation = _sOperation;
                    oAuditHeader.ActivityUserName = ds.Tables[0].Rows[i]["ActivityUserName"].ToString();
                    oAuditHeader.ActivityTimestamp = ds.Tables[0].Rows[i]["ActivityTimestamp"].ToString();
                    oAuditHeader.Details = GetAuditDetailsList(HeaderID, ds.Tables[1]);

                    oAuditHeaderList.Add(oAuditHeader);                   
                }
            }
            return oAuditHeaderList;
        }

        public static List<AuditDetails> GetAuditDetailsList(int HeaderID,DataTable dt)
        {
            List<AuditDetails> oAuditDetailsList = new List<AuditDetails>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (HeaderID == Convert.ToInt32(dt.Rows[i]["HIST_TRN_HistoryHeader_ID"]))
                {
                    AuditDetails oAuditDetails = new AuditDetails();
                    oAuditDetails.DataPoint = dt.Rows[i]["DataPoint"].ToString();                   
                    oAuditDetails.OldValue = dt.Rows[i]["OldValue"].ToString();
                    oAuditDetails.NewValue = dt.Rows[i]["NewValue"].ToString();
                    oAuditDetailsList.Add(oAuditDetails);
                }
            }

            return oAuditDetailsList;
        }
    }

   

    public class AuditHeader {
        public int HIST_TRN_HistoryHeader_ID { get; set; }
        public string TableName { get; set; }
        public string Operation { get; set; }
        public bool IsDataInserted { get; set; }
        public bool IsDataUpdated { get; set; }
        public bool IsDataDeleted { get; set; }       
        public string ActivityUserName { get; set; }
        public string ActivityTimestamp { get; set; }
        public List<AuditDetails> Details { get; set; }
    }

    public class AuditDetails {
        public int HIST_TRN_HistoryHeader_ID { get; set; }
        public string DataPoint { get; set; }     
        public string OldValue { get; set; }
        public string NewValue { get; set; }

    }


}