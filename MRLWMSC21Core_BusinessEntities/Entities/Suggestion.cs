using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class Suggestion
    {

        private int _InboundID;
        private int _TransferRequestID;
        private int _TransferRequestDetailsID;
        private int _OutboundID;
        private int _VLPDID;
        private int _MaterialMasterID;
        private int _LocationID;
        private int _GEN_SuggestionsStatusDetail_ID;
        private int _GEN_SuggestionsStatus_ID;

        private string _MaterialCode;
        private string _BatchNo;
        private string _SerialNo;
        private string _ProjectRefNo;
        private string _ErrorMessage;
        private string _LocationCode;

        private DateTime _MfgDate;
        private DateTime _ExpDate;
        private DateTime _GRNDate;

        private decimal _Quantity;

        private int _ErrorCode;
        private int _SubErrorCode;

        private int _SuggestedPutawayID;


        public int InboundID { get => _InboundID; set => _InboundID = value; }
        public int TransferRequestID { get => _TransferRequestID; set => _TransferRequestID = value; }
        public int TransferRequestDetailsID { get => _TransferRequestDetailsID; set => _TransferRequestDetailsID = value; }
        public int OutboundID { get => _OutboundID; set => _OutboundID = value; }
        public int VLPDID { get => _VLPDID; set => _VLPDID = value; }
        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public int LocationID { get => _LocationID; set => _LocationID = value; }
        public int GEN_SuggestionsStatusDetail_ID { get => _GEN_SuggestionsStatusDetail_ID; set => _GEN_SuggestionsStatusDetail_ID = value; }
        public int GEN_SuggestionsStatus_ID { get => _GEN_SuggestionsStatus_ID; set => _GEN_SuggestionsStatus_ID = value; }
        public string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string ProjectRefNo { get => _ProjectRefNo; set => _ProjectRefNo = value; }
        public string ErrorMessage { get => _ErrorMessage; set => _ErrorMessage = value; }
        public DateTime MfgDate { get => _MfgDate; set => _MfgDate = value; }
        public DateTime ExpDate { get => _ExpDate; set => _ExpDate = value; }
        public DateTime GRNDate { get => _GRNDate; set => _GRNDate = value; }
        public decimal Quantity { get => _Quantity; set => _Quantity = value; }
        public int ErrorCode { get => _ErrorCode; set => _ErrorCode = value; }
        public int SubErrorCode { get => _SubErrorCode; set => _SubErrorCode = value; }
        public string MaterialCode { get => _MaterialCode; set => _MaterialCode = value; }
        public string LocationCode { get => _LocationCode; set => _LocationCode = value; }
        public int SuggestedPutawayID { get => _SuggestedPutawayID; set => _SuggestedPutawayID = value; }
    }
}
