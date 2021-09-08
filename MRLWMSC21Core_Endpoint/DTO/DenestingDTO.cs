using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class DenestingDTO
    {

        private string _userID;
        private string _putawayLocation;
        private string _putawayPallet;
        private string _serialNumber;
        private string _mop;
        private decimal _mrp;
        private string _batchNumber;
        private List<JobDTO> _jobList;

        public string UserID { get => _userID; set => _userID = value; }
        public List<JobDTO> JobList { get => _jobList; set => _jobList = value; }
        public string PutawayLocation { get => _putawayLocation; set => _putawayLocation = value; }
        public string PutawayPallet { get => _putawayPallet; set => _putawayPallet = value; }
        public string SerialNumber { get => _serialNumber; set => _serialNumber = value; }
        public string Mop { get => _mop; set => _mop = value; }
        public decimal Mrp { get => _mrp; set => _mrp = value; }
        public string BatchNumber { get => _batchNumber; set => _batchNumber = value; }
    }
    public class JobDTO
    {
        private string _jobRefNumber;
        private string _parentSKU;
        private string _receiveExpectedQuantity;
        private string _receivedQuantity;
        private string _consumedQuantity;
        private string _parentSKUDesc;

        public string JobRefNumber { get => _jobRefNumber; set => _jobRefNumber = value; }
        public string ParentSKU { get => _parentSKU; set => _parentSKU = value; }
        public string ReceiveExpectedQuantity { get => _receiveExpectedQuantity; set => _receiveExpectedQuantity = value; }
        public string ReceivedQuantity { get => _receivedQuantity; set => _receivedQuantity = value; }
        public string ConsumedQuantity { get => _consumedQuantity; set => _consumedQuantity = value; }
        public string ParentSKUDesc { get => _parentSKUDesc; set => _parentSKUDesc = value; }
    }
}