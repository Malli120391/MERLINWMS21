using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using MRLWMSC21Common;
using Newtonsoft.Json;

namespace MRLWMSC21.mInbound.BL
{
    public class Inbound
    {

        static readonly object _object = new object();

        public List<GRNDetails> FetchGRNDataForInbound(string InboundId, string PoheaderID, string supplierinvoiceid)
        {
            try
            {
                DataSet _dsResult = DB.GetDS("EXEC  [SP_GetInboundDataForGRN] @inboundID =" + InboundId + ",@PoheaderId=" + PoheaderID + ",@SupplierInvoiceID=" + supplierinvoiceid, false);
                List<GRNDetails> _lstGRN = new List<GRNDetails>();

                foreach (DataRow _drUnload in _dsResult.Tables[0].Rows)
                {
                    GRNDetails _grm = new GRNDetails
                    {
                        PONumber = _drUnload["PONumber"].ToString(),
                        InvoiceNumber = _drUnload["InvoiceNumber"].ToString(),
                        MCode = _drUnload["MCode"].ToString(),
                        MDescription = _drUnload["MDescription"].ToString(),
                        Quantity = Convert.ToInt32(_drUnload["Quantity"].ToString()),
                        MaterialTransactionID = Convert.ToInt32(_drUnload["MaterialTransactionID"].ToString()),
                        ReceivedDate = _drUnload["ReceivedDate"].ToString()
                    };
                    _lstGRN.Add(_grm);
                }
                return _lstGRN;
            }
            catch (Exception ex)
            {
                return null;

            }



        }

        public string CreateGRNEntryAndPostDatatoSAP(string InboundId, string PoheaderID, string supplierinvoiceid, List<GRNDetails> RcvdData, CustomPrincipal cp, string remarks)
        {


            lock (_object)
            {
                try
                {
                 
                    string strMaterialTransactionID = "";
                 
                    foreach (GRNDetails grn in RcvdData)
                    {
                        strMaterialTransactionID += grn.MaterialTransactionID + ",";

                    }
                    StringBuilder sbQuery = new StringBuilder();
                    sbQuery.Append("EXEC [dbo].[INB_CreateGRNAndConfirmStock] ");
                    sbQuery.Append("@MaterilaTransactionID =" + DB.SQuote(strMaterialTransactionID));
                    sbQuery.Append(",@InboundID =" + DB.SQuote(InboundId));
                    sbQuery.Append(",@POHeaderID =" + DB.SQuote(PoheaderID));
                    sbQuery.Append(",@SupplierInvoiceID =" + DB.SQuote(supplierinvoiceid));
                    sbQuery.Append(",@CreatedBy =" + cp.UserID);
                    int NewGRNID = DB.GetSqlN(sbQuery.ToString());


                    return "GRN Created successfully";

                }
                catch (Exception ex)
                {
                    return "Error : " + ex.Message;
                }
            }
 
        }

        public string GetGRNData(int InboundId)
        {

            DataSet DS = DB.GetDS("EXEC  sp_INB_GetGRNUpdateDetails @InboundID=" + InboundId, false);
            return JsonConvert.SerializeObject(DS);


        }



    }
    public class GRNDetails
    {
        private string _pONumber;
        private string _invoiceNumber;
        private string _mCode;
        private string _mDescription;
        private int _quantity;
        private int _materialTransactionID;
        private string _receivedDate;
        private string _vehicleNumber;

        public string PONumber { get => _pONumber; set => _pONumber = value; }
        public string InvoiceNumber { get => _invoiceNumber; set => _invoiceNumber = value; }
        public string MCode { get => _mCode; set => _mCode = value; }
        public string MDescription { get => _mDescription; set => _mDescription = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }
        public int MaterialTransactionID { get => _materialTransactionID; set => _materialTransactionID = value; }
        public string ReceivedDate { get => _receivedDate; set => _receivedDate = value; }
        public string VehicleNumber { get => _vehicleNumber; set => _vehicleNumber = value; }
    }
}