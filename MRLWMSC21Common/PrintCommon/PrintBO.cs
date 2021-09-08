using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common.PrintCommon
{
    public class PrintBO
    {
        private string _barcodeString;
        private int _noOfLables;
        private int _duplicatePrints;
        private string _iPAddress;
        private int _PrinterDPI;
        private string _Mdescription;
        private string _accountName;
        public string BarcodeString { get; set; }
        public int NoOfLables { get; set; }
        public int DuplicatePrints { get; set; }
        public string IPAddress { get; set; }
        public int PrinterDPI { get; set; }
        public string Mdescription { get; set; }
        public string Location { get; set; }
        public bool IsLeftDirection { get; set; }
        public string AccountName { get => _accountName; set => _accountName = value; }


        //public string BarcodeString { get => _barcodeString; set => _barcodeString = value; }
        //public int NoOfLables { get => _noOfLables; set => _noOfLables = value; }
        //public int DuplicatePrints { get => _duplicatePrints; set => _duplicatePrints = value; }
        //public string IPAddress { get => _iPAddress; set => _iPAddress = value; }
        //public int PrinterDPI { get => _PrinterDPI; set => _PrinterDPI = value; }
        //public string Mdescription { get => _Mdescription; set => _Mdescription = value; }
        //public string Location { get => _location; set => _location = value; }
        //public bool IsLeftDirection { get => isLeftDirection; set => isLeftDirection = value; }

        private string _location;
        private bool isLeftDirection;
    }
}
