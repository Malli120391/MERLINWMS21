using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Common
{
    public class TracklineMLabel
    {
        private string _MCode;
        private string _MDescription;
        private string _MDescriptionLong;
        private string _AltMCode;
        private string _OEMPartNo;
        private string _ParentMCode;
        private string _Description;
        private string _BatchNo;
        private string _SerialNo;
        private int _KitPlannerID;
        private int _KitChildrenCount;
        private decimal _InvQty;
        private string _MfgDate;
        private string _ExpDate;
        private string _PrinterType;
        private string _PrinterIP;
        private Boolean _IsBoxLabelReq;
        private String _Result;
        private String _StrRefNo;
        private String _ReqNo;
        private String _OBDNumber;
        private String _KitCode;
        private bool _IsPositiveRecall;
        private bool _IsQtyNeedToPrint;
        private String _PrintQty;
        private string _QrCode;
        private string _Location;
        private DateTime _GRNDate;
        private int _Dpi;
        private string _Length;
        private string _Width;
        private string _LabelType;
        private string _Lineno;
        private string _ProjectNo;
        private string _DuplicatePrints;
        private string _mrp;
        private string _HUSize;
        private string _HUNo;
        public TracklineMLabel() { }


        public String PrintQty
        {
            get { return _PrintQty; }
            set { _PrintQty = value; }
        }
        public String Duplicateprints
        {
            get { return _DuplicatePrints; }
            set { _DuplicatePrints = value; }
        }

        public String Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        public String ProjectNo
        {
            get { return _ProjectNo; }
            set { _ProjectNo = value; }
        }



        public bool IsQtyNeedToPrint
        {
            get { return _IsQtyNeedToPrint; }
            set { _IsQtyNeedToPrint = value; }
        }

        public bool IsPositiveRecall
        {
            get { return _IsPositiveRecall; }
            set { _IsPositiveRecall = value; }
        }
        public String QrCode
        {
            get { return _QrCode; }
            set { _QrCode = value; }
        }

        public String MCode
        {
            get { return _MCode; }
            set { _MCode = value; }
        }


        public String MDescription
        {
            get { return _MDescription; }
            set { _MDescription = value; }
        }


        public String MDescriptionLong
        {
            get { return _MDescriptionLong; }
            set { _MDescriptionLong = value; }
        }

        public String OBDNumber
        {
            get { return _OBDNumber; }
            set { _OBDNumber = value; }
        }

        public String KitCode
        {
            get { return _KitCode; }
            set { _KitCode = value; }
        }

        public String ParentMCode
        {
            get { return _ParentMCode; }
            set { _ParentMCode = value; }
        }
        public String AltMCode
        {
            get { return _AltMCode; }
            set { _AltMCode = value; }
        }


        public String OEMPartNo
        {
            get { return _OEMPartNo; }
            set { _OEMPartNo = value; }
        }

        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public String BatchNo
        {
            get { return _BatchNo; }
            set { _BatchNo = value; }
        }

        public String SerialNo
        {
            get { return _SerialNo; }
            set { _SerialNo = value; }
        }

        public int KitPlannerID
        {
            get { return _KitPlannerID; }
            set { _KitPlannerID = value; }
        }


        public int KitChildrenCount
        {
            get { return _KitChildrenCount; }
            set { _KitChildrenCount = value; }
        }
        public string MfgDate
        {
            get { return _MfgDate; }
            set { _MfgDate = value; }
        }

        public string ExpDate
        {
            get { return _ExpDate; }
            set { _ExpDate = value; }
        }

        public string HUNo
        {
            get { return _HUNo; }
            set { _HUNo = value; }
        }

        public string HUSize
        {
            get { return _HUSize; }
            set { _HUSize = value; }
        }
        public DateTime GRNDate
        {
            get { return _GRNDate; }
            set { _GRNDate = value; }
        }

        public decimal InvQty
        {
            get { return _InvQty; }
            set { _InvQty = value; }
        }

        public String PrinterType
        {
            get { return _PrinterType; }
            set { _PrinterType = value; }
        }

        public String PrinterIP
        {
            get { return _PrinterIP; }
            set { _PrinterIP = value; }
        }

        public Boolean IsBoxLabelReq
        {
            get { return _IsBoxLabelReq; }
            set { _IsBoxLabelReq = value; }
        }

        public String Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        public String StrRefNo
        {
            get { return _StrRefNo; }
            set { _StrRefNo = value; }
        }


        public String ReqNo
        {
            get { return _ReqNo; }
            set { _ReqNo = value; }
        }
        public int Dpi
        {
            get { return _Dpi; }
            set { _Dpi = value; }
        }
        public string Length
        {
            get { return _Length; }
            set { _Length = value; }
        }
        public string Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        public string LabelType
        {
            get { return _LabelType; }
            set { _LabelType = value; }
        }
        public String Lineno
        {
            get { return _Lineno; }
            set { _Lineno = value; }
        }

        public string Mrp { 
            get {  return _mrp; }
            set { _mrp = value; }         
             }
        
       
    }
}