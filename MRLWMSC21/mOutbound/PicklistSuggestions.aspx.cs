using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mOutbound
{
    public partial class PickListSuggestions : System.Web.UI.Page
    {
        public static CustomPrincipal cp1;
        protected void Page_Load(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            
        }
      
        [WebMethod]
        public static List<PickMaterilsList> GetPickNoteData(string OBDID)
        {
            DataSet _dsResult = DB.GetDS("EXEC [USP_OBD_GetPickingSuggestions] @OBDID =" + OBDID, false);

            List<PickMaterilsList> _picklist = new List<PickMaterilsList>();
            if (_dsResult != null && _dsResult.Tables.Count != 0)
            {
                foreach (DataRow _drUnload in _dsResult.Tables[0].Rows)
                {
                    _picklist.Add(new PickMaterilsList()
                    {
                        LineNumber = _drUnload["LineNumber"].ToString(),
                        PickrefNo = _drUnload["PickListRefNo"].ToString(),
                        Mcode = _drUnload["MCode"].ToString(),
                        MDescription = _drUnload["MDescription"].ToString(),
                        ReqQty = _drUnload["Quantity"].ToString(),
                        CustomerName = _drUnload["SONumber"].ToString(),
                        MUoMQty = _drUnload["MUoMQty"].ToString(),
                        objAssignedList = PickListAssignedData(_drUnload["SODetailsID"].ToString(), _dsResult.Tables[1])
                    });


                }
            }
            return _picklist;
        }
        public static List<AssignedList> PickListAssignedData(string picklistdetailsid, DataTable dt)
        {
            List<AssignedList> obj = new List<AssignedList>();
            foreach (DataRow _drUnload in dt.Rows)
            {
                if (Convert.ToInt32(picklistdetailsid) == (Convert.ToInt32(_drUnload["SODetailsID"])))
                {
                    obj.Add(new AssignedList()
                    {

                        SODetailsID = _drUnload["SODetailsID"].ToString(),
                        Location = _drUnload["Location"].ToString(),
                        AssignedQty = Convert.ToDecimal(_drUnload["AssignedQuantity"]),
                        MFGDate = _drUnload["MfgDate"].ToString(),
                        ExpDate = _drUnload["ExpDate"].ToString(),
                        PickedQty = Convert.ToDecimal(_drUnload["PickedQty"]),
                        RequiredQty = Convert.ToDecimal(_drUnload["AssignedQuantity"]),
                        CartonCode = _drUnload["CartonCode"].ToString(),
                        TotalVolume = _drUnload["TotalVolume"].ToString(),
                        MMID  = _drUnload["MaterialMasterID"].ToString()
                    });
                }

            }
            return obj;
        }

        [WebMethod]
        public static string DownloadExcel(string OBDID,string OBDNumber)
        {
            DataSet _dsResult = DB.GetDS("EXEC [USP_OBD_GetPickingSuggestions]@IsForExcel=1,@OBDID =" + OBDID, false);
            string fileName = "PickListAssignedData" + OBDNumber + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
            //MRLWMSC21Common.ExcellCommon.ExportCyclecountDetailsData(_dsResult.Tables[0], "PickListAssignedData", new List<int>());
            MRLWMSC21Common.CommonLogic.ExportCyclecountDetailsData(_dsResult.Tables[0], fileName, new List<int>());
            return fileName;
        }



        public class PickMaterilsList
        {
            public string LineNumber { get; set; }
            public string PickrefNo { get; set; }
            public string MaterialPriority { get; set; }
            public string Mcode { get; set; }
            public string MDescription { get; set; }
            public string CustomerName { get; set; }
            public string ReqQty { get; set; }
            public string MUoMQty { get; set; }
           
            public List<AssignedList> objAssignedList { get; set; }
        }
        public class AssignedList
        {
            public string SODetailsID { get; set; }
            public string Location { get; set; }
            public string SLOC { get; set; }
            public string CartonCode { get; set; }
            public string BatchNo { get; set; }
            public decimal RequiredQty { get; set; }
            public decimal AssignedQty { get; set; }
            public decimal PickedQty { get; set; }
            public string MFGDate { get; set; }
            public string ExpDate { get; set; }
            public string TotalVolume { get; set; }
            public string MMID { get; set; }
        }
       







    }
}