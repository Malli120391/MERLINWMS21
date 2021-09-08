using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class InboundDetailsDTO
    {
        private int _InboundID;
        private string _StoreRefNo;


        private DateTime _DocReceivedDate;

        private int _ShipmentTypeID;
        private int _OutboundID;
        private int _SupplierID;
        private int _ConsignmentNoteTypeID;
        private int _FreightCompanyID;
        private int _ClearanceCompanyID;
        private int _InboundStatusID;
        private int _InitiatedByUserID;
        private int _TenantID;
        private int _PriorityLevel;
        private int _CreatedByUserID;
        
        private int _GRNDoneBy;

     
        private bool _IsDeleted;
        private bool _IsActive;
        
       
        

        public int InboundID { get => _InboundID; set => _InboundID = value; }
        public string StoreRefNo { get => _StoreRefNo; set => _StoreRefNo = value; }
        public DateTime DocReceivedDate { get => _DocReceivedDate; set => _DocReceivedDate = value; }
        public int ShipmentTypeID { get => _ShipmentTypeID; set => _ShipmentTypeID = value; }
        public int OutboundID { get => _OutboundID; set => _OutboundID = value; }
        public int SupplierID { get => _SupplierID; set => _SupplierID = value; }
        public int ConsignmentNoteTypeID { get => _ConsignmentNoteTypeID; set => _ConsignmentNoteTypeID = value; }
        public int FreightCompanyID { get => _FreightCompanyID; set => _FreightCompanyID = value; }
        public int ClearanceCompanyID { get => _ClearanceCompanyID; set => _ClearanceCompanyID = value; }
        public int InboundStatusID { get => _InboundStatusID; set => _InboundStatusID = value; }
        public int InitiatedByUserID { get => _InitiatedByUserID; set => _InitiatedByUserID = value; }
        public int TenantID { get => _TenantID; set => _TenantID = value; }
        public int PriorityLevel { get => _PriorityLevel; set => _PriorityLevel = value; }
        public int CreatedByUserID { get => _CreatedByUserID; set => _CreatedByUserID = value; }
        public int GRNDoneBy { get => _GRNDoneBy; set => _GRNDoneBy = value; }
        
        public bool IsDeleted { get => _IsDeleted; set => _IsDeleted = value; }
        public bool IsActive { get => _IsActive; set => _IsActive = value; }
       

    }
}