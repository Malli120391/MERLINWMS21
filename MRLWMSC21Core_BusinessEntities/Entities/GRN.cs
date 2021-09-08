using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class GRN
    {

        private int _InvoiceID;
        private int _POHeaderID;
        private int _PODetailsID;
        private int _LineNumber;
        private int _MaterialMasterID;
        private int _MaterialTransactionID; 

        private string _GRNNumber;
        private string _GRNDoneBy;
        private string _GRNDoneByID;
        
        private DateTime _GRNTimestamp;
        private bool _IsPostedToSAP;

        public DateTime GRNTimestamp { get => _GRNTimestamp; set => _GRNTimestamp = value; }
        public string GRNNumber { get => _GRNNumber; set => _GRNNumber = value; }
        public string GRNDoneBy { get => _GRNDoneBy; set => _GRNDoneBy = value; }
        public string GRNDoneByID { get => _GRNDoneByID; set => _GRNDoneByID = value; }
        public int InvoiceID { get => _InvoiceID; set => _InvoiceID = value; }
        public int POHeaderID { get => _POHeaderID; set => _POHeaderID = value; }
        public int PODetailsID { get => _PODetailsID; set => _PODetailsID = value; }
        public int LineNumber { get => _LineNumber; set => _LineNumber = value; }
        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public int MaterialTransactionID { get => _MaterialTransactionID; set => _MaterialTransactionID = value; }
        public bool IsPostedToSAP { get => _IsPostedToSAP; set => _IsPostedToSAP = value; }
    }
}
