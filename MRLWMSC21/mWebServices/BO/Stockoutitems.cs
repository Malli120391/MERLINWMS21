using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.mWebServices.BO
{
    public class Stockoutitems
    {

        private int _MaterialMasterID;
        private int _LocationID;
        private int _IsCycleCountON;
        private int _OBDID;
        private int _KitPlannerID;
        private String _MaterialCode;
        private String _MDescription;
   
        private String _SOHeaderID;
        private String _BUoM;
        private String _BUoMQty;
        private String _MaterialMaster_SUoMID;
        private String _MUoM;
        private String _MUoMQty;
        private String _SUoM;
        private String _SUoMQty;
        private String _CustPOUoM;
        private String _CustPOUoMQty;
        private String _MaterialGroup;
        private String _SOQuantity;
        private int _IsDamaged;
        private int _HasDiscripency;
        private int _AsIs;
        private int _IsNC;
        private int _IsPositiveRecall;
        private String _MeasurementTypeID;
        private decimal _ConvertionFactionValue;
        private decimal _ConvertionInMUom;
        private int _containerId;

        public int ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

     
        public decimal ConvertionInMUom
        {
            get { return _ConvertionInMUom; }
            set { _ConvertionInMUom = value; }
        }

        public decimal ConvertionFactionValue
        {
            get { return _ConvertionFactionValue; }
            set { _ConvertionFactionValue = value; }
        }

        public string MeasurementTypeID
        {
            get { return _MeasurementTypeID; }
            set { _MeasurementTypeID = value; }
        }

        public int IsPositiveRecall
        {
            get { return _IsPositiveRecall; }
            set { _IsPositiveRecall = value; }
        }

        public int IsNC
        {
            get { return _IsNC; }
            set { _IsNC = value; }
        }

        public int AsIs
        {
            get { return _AsIs; }
            set { _AsIs = value; }
        }
   


        private string _SODetailsID;


        public string SODetailsID
        {
            get { return _SODetailsID; }
            set { _SODetailsID = value; }
        }
        public int MaterialMasterID
        {
            get { return _MaterialMasterID; }
            set { _MaterialMasterID = value; }
        }
        public int LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }
        public int IsCycleCountON
        {
            get { return _IsCycleCountON; }
            set { _IsCycleCountON = value; }
        }
        public int OBDID
        {
            get { return _OBDID; }
            set { _OBDID = value; }
        }
        public int KitPlannerID
        {
            get { return _KitPlannerID; }
            set { _KitPlannerID = value; }
        }


        public String MaterialCode
        {
            get { return _MaterialCode; }
            set { _MaterialCode = value; }
        }


        public String MDescription
        {
            get { return _MDescription; }
            set { _MDescription = value; }
        }

        public String SOHeaderID
        {
            get { return _SOHeaderID; }
            set { _SOHeaderID = value; }
        }


        public String BUoM
        {
            get { return _BUoM; }
            set { _BUoM = value; }
        }


        public String BUoMQty
        {
            get { return _BUoMQty; }
            set { _BUoMQty = value; }
        }


        public String MaterialMaster_SUoMID
        {
            get { return _MaterialMaster_SUoMID; }
            set { _MaterialMaster_SUoMID = value; }
        }


        public String MUoM
        {
            get { return _MUoM; }
            set { _MUoM = value; }
        }


        public String MUoMQty
        {
            get { return _MUoMQty; }
            set { _MUoMQty = value; }
        }


        public String SUoM
        {
            get { return _SUoM; }
            set { _SUoM = value; }
        }


        public String SUoMQty
        {
            get { return _SUoMQty; }
            set { _SUoMQty = value; }
        }

        public String CustPOUoM
        {
            get { return _CustPOUoM; }
            set { _CustPOUoM = value; }
        }

        public String CustPOUoMQty
        {
            get { return _CustPOUoMQty; }
            set { _CustPOUoMQty = value; }
        }


        public String MaterialGroup
        {
            get { return _MaterialGroup; }
            set { _MaterialGroup = value; }
        }


        public String SOQuantity
        {
            get { return _SOQuantity; }
            set { _SOQuantity = value; }
        }

        public int IsDamaged
        {
            get { return _IsDamaged; }
            set { _IsDamaged = value; }
        }

        public int HasDiscripency
        {
            get { return _HasDiscripency; }
            set { _HasDiscripency = value; }
        }

    }
}