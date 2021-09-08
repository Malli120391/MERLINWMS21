using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MRLWMSC21Common.Interfaces;
using System.Web;

namespace MRLWMSC21Common
{
    public class InboundTrack: IInboundTrack
    {

        private int m_InboundTrackingID;
        private int m_OBDTrackingID;
        private string m_StoreRefNo;
        private string m_DocReceivedDate;
        private int m_ShipmentTypeID;
        private int m_IsRTRPublished;
        private string m_ShipmentLocation;
        private string m_ReferedStoreIDs;
        private string m_PONumber;
        private int m_TenantID;
        private int m_CreatedBy;
        private int m_UpdatedBy;
        private string m_UpdatedOn;
        public string m_WHName;
        public string m_Account;
        public int m_AccountID;
        //private string  m_DepartmentID;

        private string m_PurchaseGroupID;
        private string  m_CoordinatorID;
        private string  m_Supplier;
        private string m_SupplierName;
        
        private string m_TenantName;
        private string m_ShipmentType;
        private string m_ClearanceCompany;
        


        private string m_SupplierInvoiceNo;
        private int m_ConsignmentNoteTypeID;
        private string m_ConsignmentNoteTypeValue;
        private string  m_ConsignmentNoteTypeDate;

        private int m_NoofPackagesInDocument;
        private string   m_GrossWeight;
        private decimal  m_CBM;


        private string m_ShipmentExpectedDate;
        private string m_ShipmentReceivedDate;
        private string m_ShipmentVerifiedDate;

        private int m_HasDiscrepancy;
        private int m_NoofPackagesReceived;

        
        private string m_OffloadingTime;



        //private String m_ProjectedVehicleDetails;
        //private String m_ActualVehicleDetails;

        
        private string m_RecieptConfirmDate;

        private int m_IsChargesRequired;

        private int m_FreightCompanyID;
        private string m_FreightInvoiceNo;
        private decimal m_FreightAmount;
        private string m_FreightInvoiceDate;

        
             
        private int m_ClearenceCompanyID;
        private string m_ClearenceInvoiceNo;
        private string m_ClearenceInvoiceDate;
        private decimal m_ClearenceAmount;
        private string m_ClearenceBillsSentDate;
        private string m_ClearenceBillReceivedDate;

        //private string m_GRNNumber;
        //private DateTime m_GRNDate;
        //private int m_GRNDoneBy;

        private int m_InBoundStatusID;

        private int m_InitiatedBy;
            
        private int m_IsFrieghtBillCleared;
        private int m_IsClearenceBillCleared;

        private int m_Deleted;
        private int m_IsActive;

        private string m_RemarksBy_Ini;
        private string m_RemarksBy_StoreIncharge;
        private string m_Remarks_Billing;
        private string m_Remarks_InDiscrepancy;

        private string m_Disc_PackagesRecieved;
        private string m_Disc_Remarks;
        private int m_Disc_CheckedBy;
        private string m_Disc_CheckedDate;

        private string m_Disc_VerifiedBy;
        private string m_Disc_VerifiedDate;
        private int m_DiscStatusID;
        private int m_IsCleared;

        private int m_PriorityLevel;
        private string m_PriorityDateTime;
        private string m_DockLocation; //================= Added By M.D.Prasad On 31-Dec-2019 ====================//

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;


        #region ---- Constructors ------

        public InboundTrack() { } // for serialization ONLY!

        public InboundTrack(int prmInboundTrackingID)
        {
            m_InboundTrackingID = prmInboundTrackingID;

            LoadFromDB();

        }

        public InboundTrack(int prmInboundTrackingID, int StoreID)
        {
            m_InboundTrackingID = prmInboundTrackingID;
            
            LoadFromDB(StoreID);

        }

        #endregion


        #region ------ Properties -----------

        public int TenantID
        {
            get { return m_TenantID; }
            set { m_TenantID = value; }
        }

        public int CreatedBy
        {
            get { return m_CreatedBy; }
            set { m_CreatedBy = value; }
        }

        public int UpdatedBy
        {
            get { return m_UpdatedBy; }
            set { m_UpdatedBy = value; }
        }

        public int InboundTrackingID
        {
            get { return m_InboundTrackingID; }
            set { m_InboundTrackingID = value; }
        }

        public int OBDTrackingID
        {
            get { return m_OBDTrackingID; }
            set { m_OBDTrackingID = value; }
        }

        
        public string StoreRefNumber
        {
            get { return m_StoreRefNo; }
            set { m_StoreRefNo = value; }
        }

       public string DocReceivedDate
        {
            get { return m_DocReceivedDate; }
            set { m_DocReceivedDate = value; }
        }

        public string UpdatedOn
        {
            get { return m_UpdatedOn; }
            set { m_UpdatedOn = value; }
        }

        public int ShipmentTypeID
       {
           get { return m_ShipmentTypeID; }
           set { m_ShipmentTypeID = value; }
       }

       public int IsRTRPublished
       {
           get { return m_IsRTRPublished; }
           set { m_IsRTRPublished = value; }
       }

        

       public String ShipmentLocation
       {
           get { return m_ShipmentLocation; }
           set { m_ShipmentLocation = value; }
       }

        public String ReferedStoreIDs
        {
            get { return m_ReferedStoreIDs; }
            set { m_ReferedStoreIDs = value; }
        }

        public String PONumber
        {
            get { return m_PONumber; }
            set { m_PONumber = value; }
        }
        //================= Added By M.D.Prasad On 31-Dec-2019 ====================//
        public String DockLocation
        {
            get { return m_DockLocation; }
            set { m_DockLocation = value; }
        }
        //================= Added By M.D.Prasad On 31-Dec-2019 ====================//
        public int AccountID
        {
            get { return m_AccountID; }
            set { m_AccountID = value; }
        }

        public String WHName
        {
            get { return m_WHName; }
            set { m_WHName = value; }
        }

        public String Account
        {
            get { return m_Account; }
            set { m_Account = value; }
        }
        /*
        public string  DepartmentID
        {
            get { return m_DepartmentID; }
            set { m_DepartmentID = value; }
        }
        */

        public string PurchaseGroupID
        {
            get { return m_PurchaseGroupID ; }
            set { m_PurchaseGroupID = value; }
        }

        
        public string CoordinatorID
        {
            get { return m_CoordinatorID; }
            set { m_CoordinatorID = value; }
        }
        
        public String Supplier
        {
            get { return m_Supplier; }
            set { m_Supplier = value; }
        }

        public String SupplierName
        {
            get { return m_SupplierName; }
            set { m_SupplierName = value; }
        }

        public String SupplierInvoiceNo
        {
            get { return m_SupplierInvoiceNo; }
            set { m_SupplierInvoiceNo = value; }
        }

        public int ConsignmentNoteTypeID
        {
            get { return m_ConsignmentNoteTypeID; }
            set { m_ConsignmentNoteTypeID = value; }
        }

        public String ConsignmentNoteTypeValue
        {
            get { return m_ConsignmentNoteTypeValue; }
            set { m_ConsignmentNoteTypeValue = value; }
        }


        public string ConsignmentNoteTypeDate
        {
            get { return m_ConsignmentNoteTypeDate; }
            set { m_ConsignmentNoteTypeDate = value; }
        }


        public String TenantName
        {
            get { return m_TenantName; }
            set { m_TenantName = value; }
        }

        public String ShipmentType
        {
            get { return m_ShipmentType; }
            set { m_ShipmentType = value; }
        }

        public String ClearanceCompany
        {
            get { return m_ClearanceCompany; }
            set { m_ClearanceCompany = value; }
        }



        public int NoofPackagesInDocument
        {
            get { return m_NoofPackagesInDocument; }
            set { m_NoofPackagesInDocument = value; }
        }

        public int NoofPackagesReceived
        {
            get { return m_NoofPackagesReceived; }
            set { m_NoofPackagesReceived = value; }
        }

        

        public string GrossWeight
        {
            get { return m_GrossWeight; }
            set { m_GrossWeight = value; }
        }

        public decimal  CBM
        {
            get { return m_CBM ; }
            set { m_CBM = value; }
        }


        public string ShipmentExpectedDate
        {
            get { return m_ShipmentExpectedDate; }
            set { m_ShipmentExpectedDate = value; }
        }

        //public String ProjectedVehicleDetails
        //{
        //    get { return m_ProjectedVehicleDetails ; }
        //    set { m_ProjectedVehicleDetails = value; }
        //}

        public string OffloadingTime
        {
            get { return m_OffloadingTime; }
            set { m_OffloadingTime = value; }
        }

        public int PriorityLevel
        {
            get { return m_PriorityLevel; }
            set { m_PriorityLevel = value; }
        }


        public string PriorityDateTime
        {
            get { return m_PriorityDateTime; }
            set { m_PriorityDateTime = value; }
        }
        

        //public String ActualVehicleDetails
        //{
        //    get { return m_ActualVehicleDetails; }
        //    set { m_ActualVehicleDetails = value; }
        //}

        public string ShipmentReceivedDate
        {
            get { return m_ShipmentReceivedDate; }
            set { m_ShipmentReceivedDate = value; }
        }

        public string ShipmentVerifiedDate
        {
            get { return m_ShipmentVerifiedDate; }
            set { m_ShipmentVerifiedDate = value; }
        }
        
        

        public int HasDiscrepancy
        {
            get { return m_HasDiscrepancy; }
            set { m_HasDiscrepancy = value; }
        }

        public string RecieptConfirmDate
        {
            get { return m_RecieptConfirmDate; }
            set { m_RecieptConfirmDate = value; }
        }





        public int IsChargesRequired
        {
            get { return m_IsChargesRequired; }
            set { m_IsChargesRequired = value; }
        }


        public int FreightCompanyID
        {
            get { return m_FreightCompanyID; }
            set { m_FreightCompanyID = value; }
        }

        public String FreightInvoiceNo
        {
            get { return m_FreightInvoiceNo; }
            set { m_FreightInvoiceNo = value; }
        }

        public decimal FreightAmount
        {
            get { return m_FreightAmount; }
            set { m_FreightAmount = value; }
        }

        public string FreightInvoiceDate
        {
            get { return m_FreightInvoiceDate; }
            set { m_FreightInvoiceDate = value; }
        }

      

        public int ClearenceCompanyID
        {
            get { return m_ClearenceCompanyID; }
            set { m_ClearenceCompanyID = value; }
        }

        public String ClearenceInvoiceNo
        {
            get { return m_ClearenceInvoiceNo; }
            set { m_ClearenceInvoiceNo = value; }
        }

        public string  ClearenceInvoiceDate
        {
            get { return m_ClearenceInvoiceDate; }
            set { m_ClearenceInvoiceDate = value; }
        }


        

        public decimal ClearenceAmount
        {
            get { return m_ClearenceAmount; }
            set { m_ClearenceAmount = value; }
        }

        public string ClearanceBillsSentDate
        {
            get { return m_ClearenceBillsSentDate; }
            set { m_ClearenceBillsSentDate = value; }
        }

        public string ClearanceBillReceivedDate
        {
            get { return m_ClearenceBillReceivedDate; }
            set { m_ClearenceBillReceivedDate = value; }
        }

        public int InBoundStatusID
        {
            get { return m_InBoundStatusID; }
            set { m_InBoundStatusID = value; }
        }

        public int InitiatedBy
        {
            get { return m_InitiatedBy; }
            set { m_InitiatedBy = value; }
        }

        public int IsFrieghtBillCleared
        {
            get { return m_IsFrieghtBillCleared; }
            set { m_IsFrieghtBillCleared = value; }
        }


        public int IsClearenceBillCleared
        {
            get { return m_IsClearenceBillCleared; }
            set { m_IsClearenceBillCleared = value; }
        }

        public int Deleted
        {
            get { return m_Deleted; }
            set { m_Deleted = value; }
        }

        public int IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }

        public string  RemarksBy_Ini
        {
            get { return m_RemarksBy_Ini; }
            set { m_RemarksBy_Ini = value; }
        }

         public string  RemarksBy_StoreIncharge
        {
            get { return m_RemarksBy_StoreIncharge; }
            set { m_RemarksBy_StoreIncharge = value; }
        }


         public string Remarks_Billing
         {
             get { return m_Remarks_Billing ; }
             set { m_Remarks_Billing = value; }
         }

         public string Remarks_InDiscrepancy
         {
             get { return m_Remarks_InDiscrepancy; }
             set { m_Remarks_InDiscrepancy = value; }
         }


         public string Disc_PackagesRecieved
         {
             get { return m_Disc_PackagesRecieved; }
             set { m_Disc_PackagesRecieved = value; }
         }

         public string Disc_Remarks
         {
             get { return m_Disc_Remarks; }
             set { m_Disc_Remarks = value; }
         }

         public int Disc_CheckedBy
         {
             get { return m_Disc_CheckedBy; }
             set { m_Disc_CheckedBy = value; }
         }

         public string Disc_CheckedDate
         {
             get { return m_Disc_CheckedDate; }
             set { m_Disc_CheckedDate = value; }
         }

         public string Disc_VerifiedBy
         {
             get { return m_Disc_VerifiedBy; }
             set { m_Disc_VerifiedBy = value; }
         }

         public string Disc_VerifiedDate
         {
             get { return m_Disc_VerifiedDate; }
             set { m_Disc_VerifiedDate = value; }
         }


         public int DiscStatusID
         {
             get { return m_DiscStatusID; }
             set { m_DiscStatusID = value; }
         }

         public int IsCleared
         {
             get { return m_IsCleared; }
             set { m_IsCleared = value; }
         }
         
         
         

        #endregion

        #region ------- Functions-------

        private void LoadFromDB()
        {
            String GetOBDTrackSQL = "";
            
            if(m_InboundTrackingID != 0)
                GetOBDTrackSQL = "EXEC [dbo].[sp_INB_GetInBoundReceivedDetails]  @AccountID_New="+cp.AccountID +",@TenantID_New="+cp.TenantID +", @InboundID =" + m_InboundTrackingID ;
            else
                GetOBDTrackSQL = "EXEC [dbo].[sp_INB_GetInBoundReceivedDetails]   @InboundID =" + m_InboundTrackingID ;



            using (IDataReader rs = DB.GetRS(GetOBDTrackSQL))
            {
                while (rs.Read())
                {

                    m_InboundTrackingID = DB.RSFieldInt(rs, "InboundID");
                    //m_OBDTrackingID = DB.RSFieldInt(rs, "OBDTrackingID");

                    m_StoreRefNo = DB.RSField(rs, "StoreRefNo");
                    m_DocReceivedDate = DB.RSFieldDateTime(rs, "DocReceivedDate").ToString("dd-MMM-yyyy");

                    m_ShipmentTypeID = DB.RSFieldInt(rs, "ShipmentTypeID");
                    m_InBoundStatusID = DB.RSFieldInt(rs, "InboundStatusID");
                    m_IsRTRPublished = DB.RSFieldTinyInt(rs, "IsRTRPublished");
                    m_ShipmentLocation = DB.RSField(rs, "ShipmentLocation");

                    m_Account = DB.RSField(rs, "Account");
                    m_AccountID = DB.RSFieldInt(rs, "m_AccountID");
                    m_WHName = DB.RSField(rs, "WHCode");

                    //IDataReader dr = DB.GetRS("select WarehouseID from INB_RefWarehouse_Details where IsActive=1 AND IsDeleted=0 AND InboundID=" + m_InboundTrackingID);

                    //while(dr.Read())
                    //    m_ReferedStoreIDs += DB.RSFieldInt(dr, "WarehouseID")+",";
                    //dr.Close();

                    m_ReferedStoreIDs = DB.RSFieldInt(rs, "WarehouseID").ToString();

                    //m_PONumber = DB.RSField(rs, "PONumber");
                    //m_DepartmentID = DB.RSField(rs, "DepartmentID");

                    //m_PurchaseGroupID = DB.RSField(rs, "PurchaseGroupID");

                    //m_CoordinatorID = DB.RSField(rs, "CoordinatorID");
                    m_Supplier = DB.RSFieldInt(rs, "SupplierID").ToString();
                    //m_SupplierName = DB.GetSqlS("select SupplierName AS S from MMT_Supplier where SupplierID="+m_Supplier);

                    m_SupplierName = DB.RSField(rs, "SupplierName");

                    m_DockLocation = DB.RSField(rs, "Location"); //================= Added By M.D.Prasad On 31-Dec-2019 ====================//

                    // m_SupplierInvoiceNo = DB.RSField(rs, "SupplierInvoiceNo");

                    m_ConsignmentNoteTypeID = DB.RSFieldInt(rs, "ConsignmentNoteTypeID");
                    m_ConsignmentNoteTypeValue = DB.RSField(rs, "ConsignmentNoteTypeValue");

                    m_NoofPackagesInDocument = DB.RSFieldInt(rs, "NoofPackagesInDocument");
                    m_NoofPackagesReceived = DB.RSFieldInt(rs, "NoofPackagesReceived");
                    m_GrossWeight = DB.RSFieldDecimal(rs, "GrossWeight").ToString();
                    m_CBM = DB.RSFieldDecimal(rs, "CBM");

                    m_RecieptConfirmDate = DB.RSFieldDateTime(rs, "ReceiptConfirmDate").ToString("dd-MMM-yyyy");

                    //  m_IsChargesRequired = DB.RSFieldInt(rs, "IsChargesRequired");
                    m_FreightCompanyID = DB.RSFieldInt(rs, "FreightCompanyID");
                    m_FreightInvoiceNo = DB.RSField(rs, "FreightInvoiceNo");
                    m_FreightAmount = DB.RSFieldDecimal(rs, "FreightAmount");



                    if (rs["ConsignmentNoteTypeDate"].ToString() != "")
                        m_ConsignmentNoteTypeDate = DB.RSFieldDateTime(rs, "ConsignmentNoteTypeDate").ToString("dd-MMM-yyyy");


                    if (rs["FreightInvoiceDate"].ToString() != "")
                        m_FreightInvoiceDate = DB.RSFieldDateTime(rs, "FreightInvoiceDate").ToString("dd-MMM-yyyy");
                    else
                        m_FreightInvoiceDate = null;



                    m_ClearenceCompanyID = DB.RSFieldInt(rs, "ClearanceCompanyID");
                    m_ClearenceInvoiceNo = DB.RSField(rs, "ClearanceInvoiceNo");


                    if (rs["ClearanceInvoiceDate"].ToString() != "")
                        m_ClearenceInvoiceDate = DB.RSFieldDateTime(rs, "ClearanceInvoiceDate").ToString("dd-MMM-yyyy");
                    else
                        m_ClearenceInvoiceDate = null;

                    m_ClearenceAmount = DB.RSFieldDecimal(rs, "ClearanceAmount");


                    //Shipment Expected Date
                    if (rs["ShipmentExpectedDate"].ToString() != "")
                        m_ShipmentExpectedDate = DB.RSFieldDateTime(rs, "ShipmentExpectedDate").ToString("dd-MMM-yyyy");
                    else
                        m_ShipmentExpectedDate = null;



                    m_InitiatedBy = DB.RSFieldInt(rs, "CreatedBy");
                    m_Deleted = DB.RSFieldTinyInt(rs, "IsDeleted");
                    m_IsActive = DB.RSFieldTinyInt(rs, "IsActive");
                    m_RemarksBy_Ini = DB.RSField(rs, "RemarksBy_Ini");
                    m_RemarksBy_StoreIncharge = DB.RSField(rs, "RemarksBy_StoreIncharge");
                    m_Remarks_Billing = DB.RSField(rs, "Remarks_Billing");
                    m_Remarks_InDiscrepancy = DB.RSField(rs, "Remarks_InDiscrepancy");

                    /*
                        m_Disc_Remarks = DB.RSField(rs, "Disc_PackagesRecieved");
                        m_Disc_CheckedBy = DB.RSFieldInt(rs, "Disc_CheckedBy");
                        m_Disc_CheckedDate =   DB.RSFieldDateTime(rs, "Disc_CheckedDate");
                        m_Disc_VerifiedBy = DB.RSField(rs, "Disc_VerifiedBy");
                        m_Disc_VerifiedDate = DB.RSFieldDateTime(rs, "Disc_VerifiedDate");
                        m_DiscStatusID = DB.RSFieldInt(rs, "DiscStatusID");
                    */
                    //  m_IsCleared = DB.RSFieldTinyInt(rs, "IsCleared");


                    m_PriorityLevel = DB.RSFieldInt(rs, "PriorityLevel");

                    if (rs["PriorityDateTime"].ToString() != "")
                        m_PriorityDateTime = DB.RSFieldDateTime(rs, "PriorityDateTime").ToString();
                    else
                        m_PriorityDateTime = null;

                    m_TenantID = DB.RSFieldInt(rs, "TenantID");

                    m_TenantName = DB.RSField(rs, "TenantName");
                    m_ShipmentType = DB.RSField(rs, "ShipmentType");
                    m_ClearanceCompany = DB.RSField(rs, "ClearanceCompany");



                }
                rs.Close();
            }


        }

        private void LoadFromDB(int StoreID)
        {
            String GetOBDTrackSQL = "";

            if (m_InboundTrackingID != 0)
                GetOBDTrackSQL = "EXEC sp_IB_GetInBoundReceivedDetails @InboundTrackingID =" + m_InboundTrackingID + ", @StoreRefNo='',@StoreID=" + StoreID.ToString();
            else
                GetOBDTrackSQL = "EXEC sp_IB_GetInBoundReceivedDetails @InboundTrackingID =" + m_InboundTrackingID + ", @StoreRefNo=" + DB.SQuote(m_StoreRefNo) + ",@StoreID=" + StoreID.ToString();


            using (IDataReader rs = DB.GetRS(GetOBDTrackSQL))
            { 
            if (!rs.Read())
                {
                    LoadFromDB();
                    rs.Close();
                    return;
                }
            while (rs.Read())
            {

                m_InboundTrackingID = DB.RSFieldInt(rs, "InboundTrackingID");
                m_OBDTrackingID = DB.RSFieldInt(rs, "OBDTrackingID");
                m_StoreRefNo = DB.RSField(rs, "StoreRefNo");
                m_DocReceivedDate = DB.RSFieldDateTime(rs, "DocReceivedDate").ToString();
                m_ShipmentTypeID = DB.RSFieldInt(rs, "ShipmentTypeID");
                m_IsRTRPublished = DB.RSFieldTinyInt(rs, "IsRTRPublished");
                
                m_ShipmentLocation = DB.RSField(rs, "ShipmentLocation");
                m_ReferedStoreIDs = DB.RSField(rs, "ReferedStoreIDs");
                m_PONumber = DB.RSField(rs, "PONumber");
                //m_DepartmentID = DB.RSField(rs, "DepartmentID");
                m_PurchaseGroupID = DB.RSField(rs, "PurchaseGroupID");
                m_CoordinatorID = DB.RSField(rs, "CoordinatorID");
                m_Supplier = DB.RSField(rs, "Supplier");
                m_SupplierInvoiceNo = DB.RSField(rs, "SupplierInvoiceNo");
                m_ConsignmentNoteTypeID = DB.RSFieldInt(rs, "ConsignmentNoteTypeID");
                m_ConsignmentNoteTypeValue = DB.RSField(rs, "BLNo");
                m_NoofPackagesInDocument = DB.RSFieldInt(rs, "NoofPackagesInDocument");
                m_NoofPackagesReceived = DB.RSFieldInt(rs, "NoofPackagesReceived");
                m_GrossWeight = DB.RSField(rs, "GrossWeight");
                m_CBM = DB.RSFieldDecimal(rs, "CBM");

                m_RecieptConfirmDate = DB.RSFieldDateTime(rs, "ReceiptConfirmDate").ToString();

                m_IsChargesRequired = DB.RSFieldInt(rs, "IsChargesRequired");

                
                if (rs["AWBBLDate"].ToString() != "")
                    m_ConsignmentNoteTypeDate = DB.RSFieldDateTime(rs, "AWBBLDate").ToString("dd/MM/yyyy");
                else
                    m_ConsignmentNoteTypeDate = null;


                m_FreightCompanyID = DB.RSFieldInt(rs, "FreightCompanyID");
                m_FreightInvoiceNo = DB.RSField(rs, "FreightInvoiceNo");

                m_FreightAmount = DB.RSFieldDecimal(rs, "FreightAmount");

                if (rs["FreightInvoiceDate"].ToString() != "")
                    m_FreightInvoiceDate = DB.RSFieldDateTime(rs, "FreightInvoiceDate").ToString("dd/MM/yyyy");
                else
                    m_FreightInvoiceDate = null;

                m_ClearenceCompanyID = DB.RSFieldInt(rs, "ClearenceCompanyID");
                m_ClearenceInvoiceNo = DB.RSField(rs, "ClearenceInvoiceNo");

                if (rs["ClearanceInvoiceDate"].ToString() != "")
                    m_ClearenceInvoiceDate = DB.RSFieldDateTime(rs, "ClearanceInvoiceDate").ToString("dd/MM/yyyy");
                else
                    m_ClearenceInvoiceDate = null;

                m_ClearenceAmount = DB.RSFieldDecimal(rs, "ClearenceAmount");

                if (rs["ClearanceBillsSentDate"].ToString() != "")
                    m_ClearenceBillsSentDate = DB.RSFieldDateTime(rs, "ClearanceBillsSentDate").ToString("dd/MM/yyyy");
                else
                    m_ClearenceBillsSentDate = null;

                if (rs["ClearanceBillReceivedDate"].ToString() != "")
                    m_ClearenceBillReceivedDate = DB.RSFieldDateTime(rs, "ClearanceBillReceivedDate").ToString("dd/MM/yyyy");
                else
                    m_ClearenceBillReceivedDate = null;


                //m_GRNNumber = DB.RSField(rs, "GRNNumber");

                if (rs["Offloadtime"].ToString() != "")
                    m_OffloadingTime = DB.RSFieldTime(rs, "Offloadtime").ToString();
                else
                    m_OffloadingTime = null;


               


                //m_ProjectedVehicleDetails = DB.RSField(rs, "ProjectedVehicleDetails");
                //m_ActualVehicleDetails = DB.RSField(rs, "ActualVehicleDetails");


                //Shipment Expected Date
                if (rs["ShipmentExpectedDate"].ToString() != "")
                    m_ShipmentExpectedDate = DB.RSFieldDateTime(rs, "ShipmentExpectedDate").ToString("dd/MM/yyyy");
                else
                    m_ShipmentExpectedDate = null;

                //Shipment Received Date
                if (rs["ShipmentReceivedOn"].ToString() != "")
                    m_ShipmentReceivedDate = DB.RSFieldDateTime(rs, "ShipmentReceivedOn").ToString();
                else
                    m_ShipmentReceivedDate = null;

                //Shipment Verified Date
                if (rs["ShipmentVerifiedOn"].ToString() != "")
                    m_ShipmentVerifiedDate = DB.RSFieldDateTime(rs, "ShipmentVerifiedOn").ToString();
                else
                    m_ShipmentVerifiedDate = null;




                //GRN Date
                //if (rs["GRNDate"].ToString() != "")
                //    m_GRNDate = DB.RSFieldDateTime(rs, "GRNDate");
                //else
                //    m_GRNDate = DateTime.MinValue;


                m_HasDiscrepancy = DB.RSFieldTinyInt(rs, "HasDiscrepancy");

                //m_GRNDoneBy = DB.RSFieldInt(rs, "GRNDoneBy");
                m_InBoundStatusID = DB.RSFieldInt(rs, "InBoundStatusID");
                m_InitiatedBy = DB.RSFieldInt(rs, "InitiatedBy");
                m_IsFrieghtBillCleared = DB.RSFieldTinyInt(rs, "IsFrieghtBillCleared");
                m_IsClearenceBillCleared = DB.RSFieldTinyInt(rs, "IsClearenceBillCleared");
                m_Deleted = DB.RSFieldTinyInt(rs, "Deleted");
                m_IsActive = DB.RSFieldTinyInt(rs, "IsActive");
                m_RemarksBy_Ini = DB.RSField(rs, "RemarksBy_Ini");
                m_RemarksBy_StoreIncharge = DB.RSField(rs, "RemarksBy_StoreIncharge");
                m_Remarks_Billing = DB.RSField(rs, "Remarks_Billing");
                m_Remarks_InDiscrepancy = DB.RSField(rs, "Remarks_InDiscrepancy");


                m_Disc_Remarks = DB.RSField(rs, "Disc_Remarks");
                m_Disc_PackagesRecieved = DB.RSField(rs, "Disc_PackagesRecieved");
                m_Disc_CheckedBy = DB.RSFieldInt(rs, "Disc_CheckedBy");
                m_Disc_CheckedDate = DB.RSFieldDateTime(rs, "Disc_CheckedDate").ToString("dd/MM/yyyy");
                m_Disc_VerifiedBy = DB.RSField(rs, "Disc_VerifiedBy");
                m_Disc_VerifiedDate = DB.RSFieldDateTime(rs, "Disc_VerifiedDate").ToString("dd/MM/yyyy");
                m_DiscStatusID = DB.RSFieldInt(rs, "DiscStatusID");
                m_IsCleared = DB.RSFieldTinyInt(rs, "IsCleared");

                m_PriorityLevel = DB.RSFieldInt(rs, "PriorityLevel");

                if (rs["PriorityDateTime"].ToString() != "")
                    m_PriorityDateTime = DB.RSFieldDateTime(rs, "PriorityDateTime").ToString();
                else
                    m_PriorityDateTime = null;
            }
            rs.Close();
                }


        }

        public int InitiateIBTracking()
        {
            try
            {

                //INSERT NEW OBD Tracking
                StringBuilder strUpdateInboundTracking = new StringBuilder(2500);


                strUpdateInboundTracking.Append("DECLARE @NewUpdateInboundID nvarchar(50);   ");
                strUpdateInboundTracking.Append("EXEC [dbo].[sp_INB_UpsertInbound]    ");
                strUpdateInboundTracking.Append("@StoreRefNo=" + DB.SQuote(m_StoreRefNo) + ",");
                strUpdateInboundTracking.Append("@DocReceivedDate=" + DB.DateQuote(m_DocReceivedDate) + ",");
                strUpdateInboundTracking.Append("@ShipmentTypeID=" +CommonLogic.IIF( m_ShipmentTypeID.ToString()!="0", m_ShipmentTypeID.ToString(),"NULL") + ",");
                strUpdateInboundTracking.Append("@IsChargesRequired=NULL" + ",");
                strUpdateInboundTracking.Append("@SupplierID=" + Convert.ToInt32(m_Supplier) + ",");
                strUpdateInboundTracking.Append("@ConsignmentNoteTypeID=" + CommonLogic.IIF(m_ConsignmentNoteTypeID.ToString()!="0",m_ConsignmentNoteTypeID.ToString(),"NULL") + ",");
                strUpdateInboundTracking.Append("@ConsignmentNoteTypeValue=" + DB.SQuote(m_ConsignmentNoteTypeValue) + ",");
                if (m_ConsignmentNoteTypeDate != null )
                    strUpdateInboundTracking.Append("@ConsignmentNoteTypeDate=" + DB.DateQuote(m_ConsignmentNoteTypeDate) + ",");
                else
                    strUpdateInboundTracking.Append("@ConsignmentNoteTypeDate=NULL,");
                strUpdateInboundTracking.Append("@NoofPackagesInDocument=" + m_NoofPackagesInDocument.ToString() + ",");
                strUpdateInboundTracking.Append("@GrossWeight=" + DB.SQuote(m_GrossWeight).ToString() + ",");
                strUpdateInboundTracking.Append("@CBM=" + m_CBM.ToString() + ",");

                strUpdateInboundTracking.Append("@ClearanceCompanyID=" + CommonLogic.IIF(m_ClearenceCompanyID.ToString()!="0",m_ClearenceCompanyID.ToString(),"NULL") + ",");
                strUpdateInboundTracking.Append("@ClearanceInvoiceNo=" + DB.SQuote(m_ClearenceInvoiceNo) + ",");

                if (m_ClearenceInvoiceDate != null)
                    strUpdateInboundTracking.Append("@ClearanceInvoiceDate=" + DB.DateQuote(m_ClearenceInvoiceDate) + ",");
                else
                    strUpdateInboundTracking.Append("@ClearanceInvoiceDate=NULL,");

                strUpdateInboundTracking.Append("@ClearanceAmount=" + m_ClearenceAmount.ToString() + ",");

                strUpdateInboundTracking.Append("@FreightCompanyID=" +CommonLogic.IIF(m_FreightCompanyID.ToString()!="0",m_FreightCompanyID.ToString(),"NULL") + ",");
                strUpdateInboundTracking.Append("@FreightInvoiceNo=" + DB.SQuote(m_FreightInvoiceNo) + ",");

                if (m_FreightInvoiceDate != null)
                    strUpdateInboundTracking.Append("@FreightInvoiceDate=" + DB.DateQuote(m_FreightInvoiceDate) + ",");
                else
                    strUpdateInboundTracking.Append("@FreightInvoiceDate=NULL,");

                strUpdateInboundTracking.Append("@FreightAmount=" + m_FreightAmount.ToString() + ",");





                strUpdateInboundTracking.Append("@PriorityLevel=" + m_PriorityLevel.ToString() + ",");

                if (m_PriorityDateTime != null)
                    strUpdateInboundTracking.Append("@PriorityDateTime=" + DB.SQuote(m_PriorityDateTime) + ",");
                else
                    strUpdateInboundTracking.Append("@PriorityDateTime=NULL,");
                strUpdateInboundTracking.Append("@RemarksBy_Ini=" + DB.SQuote(m_RemarksBy_Ini) + ",");
                strUpdateInboundTracking.Append("@CreatedBy=" + m_CreatedBy.ToString() + ",");
                strUpdateInboundTracking.Append("@UpdatedOn=" + DB.DateQuote(m_UpdatedOn) + ",");
                strUpdateInboundTracking.Append("@UpdatedBy=" + m_UpdatedBy.ToString() + ",");

                strUpdateInboundTracking.Append("@AccountID=" + cp.AccountID + ",");
                strUpdateInboundTracking.Append("@TenantID=" + m_TenantID.ToString() + ",");
                strUpdateInboundTracking.Append("@WarehouseIDs=" +DB.SQuote( m_ReferedStoreIDs.ToString() )+ ",");
                strUpdateInboundTracking.Append("@InboundID=" + m_InboundTrackingID.ToString()+",");
                strUpdateInboundTracking.Append("@InboundStatusID=1"  + ",");
                strUpdateInboundTracking.Append("@NewInboundID=@NewUpdateInboundID OUTPUT select @NewUpdateInboundID AS S");
                string retValue = DB.GetSqlS(strUpdateInboundTracking.ToString());
                m_InboundTrackingID = Convert.ToInt16(retValue.Split(',')[0]);
                m_StoreRefNo = retValue.Split(',')[1];

                return m_InboundTrackingID;


                //strUpdateInboundTracking.Append("@ShipmentLocation=" +(m_ShipmentLocation ==""? "NULL": DB.SQuote(m_ShipmentLocation)) + ",");
                //strUpdateInboundTracking.Append("@OBDTrackingID=" + (m_OBDTrackingID ==0? "NULL":m_OBDTrackingID.ToString() )+ ",");

                // strUpdateInboundTracking.Append("@ReferedStoreIDs=" + DB.SQuote(m_ReferedStoreIDs) + ",");
                //strUpdateInboundTracking.Append("@PONumber=" + DB.SQuote(m_PONumber) + ",");
                //strUpdateInboundTracking.Append("@DepartmentID=" + m_DepartmentID.ToString() + ",");
                //strUpdateInboundTracking.Append("@CoordinatorID=" + m_CoordinatorID.ToString() + ",");


                //strUpdateInboundTracking.Append("@SupplierInvoiceNo=" + DB.SQuote(m_SupplierInvoiceNo) + ",");
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion
    }


}
