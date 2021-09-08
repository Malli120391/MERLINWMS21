using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Library;

namespace MRLWMSC21Core.Entities
{
    public class HouseKeepingJob
    {
        private string _TransferRequestNumber;


        private int _TransferRequestID;
        private int _TransferTypeID;


        private string _TransferType;
        private string _Remarks;
        private int _StatusID;
        

        public string TransferRequestNumber { get => _TransferRequestNumber; set => _TransferRequestNumber = value; }
        public int TransferRequestID { get => _TransferRequestID; set => _TransferRequestID = value; }
        public int TransferTypeID { get => _TransferTypeID; set => _TransferTypeID = value; }
        public string TransferType { get => _TransferType; set => _TransferType = value; }
        public string Remarks { get => _Remarks; set => _Remarks = value; }
        public int StatusID { get => _StatusID; set => _StatusID = value; }
    }
}
