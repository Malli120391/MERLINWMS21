using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MRLWMSC21Common.Interfaces;
using System.Web;



namespace MRLWMSC21Common
{
    public sealed class OBDTrack : IOBDTrack
    {
        public CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;

        private int m_OBDTrackingID;
        private int m_DocumentTypeID;
        private string  m_OBDNumber;
        private string m_OBDDate;
        private string m_CustomerName;
        private int m_CustomerID;
        private int m_RequestedByID;
        private int m_TenantID;
        private string m_TenantName;
        private int m_CreatedBy;
        private int m_LastModifiedBy;
        

        private int m_MMDivisionID;
        private int m_MMDepartmentID;
        private int m_IsDNPublished;
        private string m_ReferedStoreIDs;
        private string m_GuidanceToStoreDT;
        private string m_RemByIni_OnCreation;
        private int m_InitiatedBy;
        private string m_OBDReceivedDT;
        private int m_NoofLines;
        private decimal m_TotalQuantity;
        private int m_PickedBy;
        private string  m_CheckedBy;
        private string m_SentForPGI_DT;
        private string m_RemByStoreIncharge;
        private int m_StoreInchargeID;
        private string  m_ReceivedforPGI_DT;
        private string m_RemByIni_AfterPGI;
        private int m_PGIDoneBy;
        private string m_PGIDone_DT;
        private int m_InstructionModeID;
        private string m_Requester;
        private string m_DocumentNumber;
        private string m_DocumentReceivedDate;
        private string m_DeliveryDate;
        private string m_DelivertTime;
        private int m_DeliveredBy;
        private string m_DriverName;
        private string m_ReceivedBy;
        private string m_RemBy_DeliveryIncharge;
        private int  m_DeliveryStatusID;
        private bool m_Deleted;
        private bool m_IsActive;
        private string m_InitiatedByFirstName;
        private string m_PickedByFirstName;
        
        private string m_StoreInchargeFirstName;
        private string m_DeliveredByFirstName;
        private int m_TransferedtoStoreID;
        private string m_InstructionModeType;
        private string m_DeliveryStatus;

        private string m_GDRPONo;
        private string m_GDRInvoiceNo;
        private string  m_GDRInvoiceDate;
        private string m_GDRSAPRefNo;

        private string m_GDRRequestBy;
        private string m_GDRDriverName;
        private string m_GDRPreparedBy;
        private string m_GDRApprovedBy;



        private int m_IsReservationDelivery;

        private int m_IsGDRApproved;
        private int m_GDRApprovedByID;



        private int m_OBVehicalTypeID1;
        private int m_OBVehicalTypeID2;
        private int m_OBVehicalTypeID3;

        private int m_OBVehicalQty1;
        private int m_OBVehicalQty2;
        private int m_OBVehicalQty3;

        

        private int m_AOBVehicalTypeID1;
        private int m_AOBVehicalTypeID2;
        private int m_AOBVehicalTypeID3;

        private int m_AOBVehicalQty1;
        private int m_AOBVehicalQty2;
        private int m_AOBVehicalQty3;

        private string m_AOBVehicalRegNumbers1;
        private string m_AOBVehicalRegNumbers2;
        private string m_AOBVehicalRegNumbers3;


        private int m_StoreID;

        private int m_RefWHID;

        private int m_OBDTrack_WHID;
        
        private int m_IsDCRReceived;
       
        private int m_PriorityLevel;
        private string m_PriorityDateTime;

        private string m_Customer;
        private string m_RequestedBy;


        private int m_PackedBy;
        private string m_PackedOn;
        private string m_PackingRemarks;
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        #region ---- Constructors ------

        public OBDTrack() { } // for serialization ONLY!

        public OBDTrack(int prmOBDTrackingID)
        {
            m_OBDTrackingID = prmOBDTrackingID;
            LoadFromDB();

        }

        public OBDTrack(int prmOBDTrackingID, int StoreID)
        {
            m_OBDTrackingID = prmOBDTrackingID;
            

        }


        #endregion


        #region ------ Properties -----------


     

        public int OBDTrackingID
        {
            get { return m_OBDTrackingID; }
            set { m_OBDTrackingID = value; }
        }

        public int OBDTrack_WHID
        {
            get { return m_OBDTrack_WHID; }
            set { m_OBDTrack_WHID = value; }
        }

        public int RefWHID
        {
            get { return m_RefWHID; }
            set { m_RefWHID = value; }
        }

        public int TenantID
        {
            get { return m_TenantID; }
            set { m_TenantID = value; }
        }

        public string TenantName
        {
            get { return m_TenantName; }
            set { m_TenantName = value; }
        }

        

        
        public int CreatedBy
        {
            get { return m_CreatedBy; }
            set { m_CreatedBy = value; }
        }

        public int LastModifiedBy
        {
            get { return m_LastModifiedBy; }
            set { m_LastModifiedBy = value; }
        }
        

        public int DocumentTypeID
        {
            get { return m_DocumentTypeID; }
            set { m_DocumentTypeID = value; }
        }

        public string  OBDNumber
        {
            get { return m_OBDNumber; }
            set { m_OBDNumber = value; }
        }

        public string  OBDDate
        {
            get { return m_OBDDate; }
            set { m_OBDDate = value; }
        }


        public String CustomerName
        {
            get { return m_CustomerName; }
            set { m_CustomerName = value; }
        }

        public int CustomerID
        {
            get { return m_CustomerID; }
            set { m_CustomerID = value; }
        }


        public int RequestedByID
        {
            get { return m_RequestedByID; }
            set { m_RequestedByID = value; }
        }

        public String RequestedBy
        {
            get { return m_RequestedBy; }
            set { m_RequestedBy = value; }
        }





        public int MMDivisionID
        {
            get { return m_MMDivisionID; }
            set { m_MMDivisionID = value; }
        }



        public int MMDepartmentID
        {
            get { return m_MMDepartmentID; }
            set { m_MMDepartmentID = value; }
        }

        public int IsDNPublished
        {
            get { return m_IsDNPublished; }
            set { m_IsDNPublished = value; }
        }

        
        public String ReferedStoreIDs
        {
            get { return m_ReferedStoreIDs; }
            set { m_ReferedStoreIDs = value; }
        }


        public int StoreID
        {
            get { return m_StoreID; }
            set { m_StoreID = value; }
        }

        public string  GuidanceToStoreDT
        {
            get { return m_GuidanceToStoreDT; }
            set { m_GuidanceToStoreDT = value; }
        }


        public String RemByIni_OnCreation
        {
            get { return m_RemByIni_OnCreation; }
            set { m_RemByIni_OnCreation = value; }
        }


        public int InitiatedBy
        {
            get { return m_InitiatedBy; }
            set { m_InitiatedBy = value; }
        }


        public string OBDReceivedDT
        {
            get { return m_OBDReceivedDT; }
            set { m_OBDReceivedDT = value; }
        }


        public int NoofLines
        {
            get { return m_NoofLines; }
            set { m_NoofLines = value; }
        }


        public decimal TotalQuantity
        {
            get { return m_TotalQuantity; }
            set { m_TotalQuantity = value; }
        }

        public int PickedBy
        {
            get { return m_PickedBy ; }
            set { m_PickedBy = value; }
        }

        public String CheckedBy
        {
            get { return m_CheckedBy ; }
            set { m_CheckedBy = value; }
        }

        public string SentForPGI_DT
        {
            get { return m_SentForPGI_DT; }
            set { m_SentForPGI_DT = value; }
        }


        public String RemBy_StoreIncharge
        {
            get { return m_RemByStoreIncharge; }
            set { m_RemByStoreIncharge = value; }
        }
        

        public String RemByIni_AfterPGI
        {
            get { return m_RemByIni_AfterPGI; }
            set { m_RemByIni_AfterPGI = value; }
        }

        public string PGIDoneDate
        {
            get { return m_PGIDone_DT; }
            set { m_PGIDone_DT = value; }
        }



        public int PGIDoneBy
        {
            get { return m_PGIDoneBy; }
            set { m_PGIDoneBy = value; }
        }

        public int InstructionModeID
        {
            get { return m_InstructionModeID; }
            set { m_InstructionModeID = value; }
        }


        public int TransferedtoStoreID
        {
            get { return m_TransferedtoStoreID ; }
            set { m_TransferedtoStoreID = value; }
        }

        public String Requester
        {
            get { return m_Requester ; }
            set { m_Requester = value; }
        }

        public String DocumentNumber
        {
            get { return m_DocumentNumber; }
            set { m_DocumentNumber = value; }
        }

        public string DocumentReceivedDate
        {
            get { return m_DocumentReceivedDate; }
            set { m_DocumentReceivedDate = value; }
        }

        public string DeliveryDate
        {
            get { return m_DeliveryDate; }
            set { m_DeliveryDate = value; }
        }

       
        public int DeliveredBy
        {
            get { return m_DeliveredBy; }
            set { m_DeliveredBy = value; }
        }



        public String DriverName
        {
            get { return m_DriverName; }
            set { m_DriverName = value; }
        }

        public String ReceivedBy
        {
            get { return m_ReceivedBy; }
            set { m_ReceivedBy = value; }
        }

        public String RemBy_DeliveryIncharge
        {
            get { return m_RemBy_DeliveryIncharge; }
            set { m_RemBy_DeliveryIncharge = value; }
        }


        public int DeliveryStatusID
        {
            get { return m_DeliveryStatusID; }
            set { m_DeliveryStatusID = value; }
        }


        public Boolean  Deleted
        {
            get { return m_Deleted; }
            set { m_Deleted = value; }
        }


        public Boolean IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }


        public int StoreInchargeID
        {
            get { return m_StoreInchargeID; }
            set { m_StoreInchargeID = value; }
        }

        public string ReceivedforPGIDate
        {
            get { return m_ReceivedforPGI_DT; }
            set { m_ReceivedforPGI_DT = value; }
        }

        public String InitiatedByFirstName
        {
            get { return m_InitiatedByFirstName; }
            
        }

        public String PickedByFirstName
        {
            get { return m_PickedByFirstName; }

        }

       

        public String StoreInchargeFirstName
        {
            get { return m_StoreInchargeFirstName; }

        }

        public String DeliveredByFirstName
        {
            get { return m_DeliveredByFirstName; }

        }



        public String InstructionModeType
        {
            get { return InstructionModeType; }

        }

        public String DeliveryStatus
        {
            get { return m_DeliveryStatus; }

        }


        public String GDRPONo
        {
            get { return m_GDRPONo; }
            set { m_GDRPONo = value; }
        }


        public String GDRInvoiceNo
        {
            get { return m_GDRInvoiceNo; }
            set { m_GDRInvoiceNo = value; }
        }


        public string GDRInvoiceDate
        {
            get { return m_GDRInvoiceDate; }
            set { m_GDRInvoiceDate = value; }
        }

        public String GDRSAPRefNo
        {
            get { return m_GDRSAPRefNo; }
            set { m_GDRSAPRefNo = value; }
        }




        public int IsReservationDelivery
        {
            get { return m_IsReservationDelivery; }
            set { m_IsReservationDelivery = value; }
        }

        public int IsGDRApproved
        {
            get { return m_IsGDRApproved; }
            set { m_IsGDRApproved = value; }
        }

        public int GDRApprovedByID
        {
            get { return m_GDRApprovedByID; }
            set { m_GDRApprovedByID = value; }
        }

        public string GDRRequestBy
        {
            get { return m_GDRRequestBy; }
            set { m_GDRRequestBy = value; }
        }

        public string GDRPreparedBy
        {
            get { return m_GDRPreparedBy; }
            set { m_GDRPreparedBy = value; }
        }

        public string GDRDriverName
        {
            get { return m_GDRDriverName; }
            set { m_GDRDriverName = value; }
        }

        public string GDRApprovedBy
        {
            get { return m_GDRApprovedBy; }
            set { m_GDRApprovedBy = value; }
        }
        
     
        public int IsDCRReceived
        {
            get { return m_IsDCRReceived; }
            set { m_IsDCRReceived = value; }
        }


        public int OBVehicalTypeID1
        {
            get { return m_OBVehicalTypeID1 ; }
            set { m_OBVehicalTypeID1 = value; }
        }


        public int OBVehicalQty1
        {
            get { return m_OBVehicalQty1 ; }
            set { m_OBVehicalQty1 = value; }
        }

        

        public int OBVehicalTypeID2
        {
            get { return m_OBVehicalTypeID2; }
            set { m_OBVehicalTypeID2 = value; }
        }

        public int OBVehicalQty2
        {
            get { return m_OBVehicalQty2; }
            set { m_OBVehicalQty2 = value; }
        }

       

        public int OBVehicalTypeID3
        {
            get { return m_OBVehicalTypeID3; }
            set { m_OBVehicalTypeID3 = value; }
        }


        public int OBVehicalQty3
        {
            get { return m_OBVehicalQty3; }
            set { m_OBVehicalQty3 = value; }
        }

       
        
        public int AOBVehicalTypeID1
        {
            get { return m_AOBVehicalTypeID1 ; }
            set { m_AOBVehicalTypeID1 = value; }
        }


        public int AOBVehicalQty1
        {
            get { return m_AOBVehicalQty1 ; }
            set { m_AOBVehicalQty1 = value; }
        }

        public string AOBVehicalRegNumbers1
        {
            get { return m_AOBVehicalRegNumbers1; }
            set { m_AOBVehicalRegNumbers1 = value; }
        }

        public int AOBVehicalTypeID2
        {
            get { return m_AOBVehicalTypeID2; }
            set { m_AOBVehicalTypeID2 = value; }
        }


        public int AOBVehicalQty2
        {
            get { return m_AOBVehicalQty2; }
            set { m_AOBVehicalQty2 = value; }
        }

        public string AOBVehicalRegNumbers2
        {
            get { return m_AOBVehicalRegNumbers2; }
            set { m_AOBVehicalRegNumbers2 = value; }
        }

        public int AOBVehicalTypeID3
        {
            get { return m_AOBVehicalTypeID3; }
            set { m_AOBVehicalTypeID3 = value; }
        }


        public int AOBVehicalQty3
        {
            get { return m_AOBVehicalQty3; }
            set { m_AOBVehicalQty3 = value; }
        }

        public string AOBVehicalRegNumbers3
        {
            get { return m_AOBVehicalRegNumbers3; }
            set { m_AOBVehicalRegNumbers3 = value; }
        }

        public int PriorityLevel
        {
            get { return m_PriorityLevel ; }
            set { m_PriorityLevel = value; }
        }


        public string PriorityDateTime
        {
            get { return m_PriorityDateTime ; }
            set { m_PriorityDateTime = value; }
        }

        public String Customer
        {
            get { return m_Customer; }
            set { m_Customer = value; }
        }


        public int PackedBy
        {
            get { return m_PackedBy; }
            set { m_PackedBy = value; }
        }

        public string PackedOn
        {
            get { return m_PackedOn; }
            set { m_PackedOn = value; }
        }

        public string PackingRemarks
        {
            get { return m_PackingRemarks; }
            set { m_PackingRemarks = value; }
        }
 public string DelivertTime
        {
            get { return m_DelivertTime; }
            set { m_DelivertTime = value; }
        }

 public string InstructionModeType1
        {
            get { return m_InstructionModeType; }
            set { m_InstructionModeType = value; }
        }        // public string DelivertTime { get => m_DelivertTime; set => m_DelivertTime = value; }
        //public string InstructionModeType1 { get => m_InstructionModeType; set => m_InstructionModeType = value; }

        #endregion

        #region ------- Functions-------

        private void LoadFromDB()
        {
           
            String GetOBDTrackSQL = "EXEC [dbo].[sp_OBD_GetOutboundDetails] @AccountID_New="+cp.AccountID+", @OutboundID=" + m_OBDTrackingID.ToString();
               IDataReader rs = DB.GetRS(GetOBDTrackSQL);
               while (rs.Read())
               {

                   m_OBDTrackingID = DB.RSFieldInt(rs, "OutboundID");
                    m_OBDNumber = DB.RSField(rs, "OBDNumber");
                    m_DocumentTypeID = DB.RSFieldInt(rs, "DocumentTypeID");

                   if( rs["OBDDate"].ToString() !="")
                        m_OBDDate = DB.RSFieldDateTime(rs, "OBDDate").ToString("dd-MMM-yyyy");
                   else
                        m_OBDDate = null ;

                   m_TenantID = DB.RSFieldInt(rs, "TenantID");
                   m_TenantName = DB.RSField(rs, "TenantName");

                   m_CreatedBy = DB.RSFieldInt(rs, "CreatedBy");

                   m_CustomerID = DB.RSFieldInt(rs, "CustomerID");

                    m_RequestedByID = DB.RSFieldInt(rs, "RequestedBy");
                    
                    m_MMDivisionID  = DB.RSFieldInt(rs, "DivisionID");
                    m_MMDepartmentID=DB.RSFieldInt(rs, "DepartmentID");
                    m_IsDNPublished = DB.RSFieldTinyInt(rs, "IsDNPublished");
                    
                   // m_ReferedStoreIDs= DB.RSField(rs, "ReferedStoreIDs");
                    //m_GuidanceToStoreDT= DB.RSFieldDateTime(rs, "GuidanceToStoreDT");
                    m_RemByIni_OnCreation=DB.RSField(rs, "RemByIni_OnCreation");
                    m_InitiatedBy=DB.RSFieldInt(rs, "InitiatedBy");
                    //m_ReceivedforPGI_DT= DB.RSFieldDateTime(rs, "ReceivedforPGI_DT");
                    m_RemByIni_AfterPGI = DB.RSField(rs, "RemByIni_AfterPGI");
                    m_PGIDoneBy = DB.RSFieldInt(rs, "PGIDoneBy");

                //if (rs["PGIDoneOn"].ToString() != "")
                //{
                //    m_PGIDone_DT= DB.RSFieldDateTime(rs, "PGIDoneOn").ToString("dd-MMM-yyyy");
                //}
                //else
                //{
                //    m_PGIDone_DT = null;
                //}
                   m_PGIDone_DT = ( DB.RSFieldDateTime(rs, "PGIDoneOn") == DateTime.MinValue ? null: DB.RSFieldDateTime(rs, "PGIDoneOn").ToString() );
                    m_DeliveryStatusID=  DB.RSFieldInt(rs, "DeliveryStatusID");
                    m_Deleted  =DB.RSFieldBool(rs, "IsDeleted");
                    m_IsActive=DB.RSFieldBool(rs, "IsActive");
                   // m_InitiatedByFirstName=DB.RSField(rs, "InitiatedByFirstName");
                    m_DeliveryStatus=DB.RSFieldInt(rs, "DeliveryStatusID").ToString();
                   //GDR Info
                   /*
                    m_GDRPONo = DB.RSField(rs, "GDRPONo");
                    m_GDRInvoiceNo = DB.RSField(rs, "GDRInvoiceNo");
                    m_GDRInvoiceDate = DB.RSFieldDateTime(rs, "GDRInvoiceDate");
                    m_GDRSAPRefNo = DB.RSField(rs, "GDRSAPRefNo");

                    

                    m_GDRRequestBy = DB.RSField(rs, "GDRRequestedBy");
                    m_GDRDriverName = DB.RSField(rs, "GDRDriverName");
                    m_GDRPreparedBy = DB.RSField(rs, "GDRPreparedBy");
                    m_GDRApprovedBy = DB.RSField(rs, "GDRApprovedBy");
                    
                    */

                    m_IsReservationDelivery = DB.RSFieldTinyInt(rs, "IsReservationDelivery"); 
                    m_PriorityLevel = DB.RSFieldInt(rs, "PriorityLevel");
                    
                    if (rs["PriorityDateTime"].ToString() != "")
                        m_PriorityDateTime = DB.RSFieldDateTime(rs, "PriorityDateTime").ToString();
                    else
                        m_PriorityDateTime = null;

                    m_ReferedStoreIDs = "";

                    //IDataReader dr = DB.GetRS("select WarehouseID from OBD_RefWarehouse_Details where IsActive=1  and IsDeleted=0 and OutboundID=" + m_OBDTrackingID);

                    //while (dr.Read())
                    //    m_ReferedStoreIDs += DB.RSFieldInt(dr, "WarehouseID") + ",";
                    //dr.Close();

                    m_ReferedStoreIDs = DB.RSField(rs, "ReferedStores");
                    m_Customer = DB.RSField(rs, "Customer");
                    m_RequestedBy = DB.RSField(rs, "FirstName");

               }
               rs.Close();
                                    

        }

        public void LoadFromDB(int OutBoundID,int StoreID)
        {

            String GetOBDTrackSQL = "EXEC [dbo].[sp_OBD_GetOutboundTrackingWHDetails] @OutboundID=" + OutBoundID.ToString() + " , @RefStoreID=" + StoreID.ToString();
            IDataReader rs = DB.GetRS(GetOBDTrackSQL);

            if (!rs.Read()) {

                m_OBDReceivedDT = null;
                m_NoofLines = 0;
                m_TotalQuantity = 0;
                m_SentForPGI_DT = null;
                m_RemByStoreIncharge = "";
                m_StoreInchargeID = 0;
                m_PickedBy = 0;
                m_OBDTrack_WHID = 0; 
                m_CheckedBy ="";

                m_InstructionModeID = 0;
                m_Requester = "";
                m_DocumentReceivedDate =null;
                m_TransferedtoStoreID = 0;
                m_DocumentNumber = "";
                m_DocumentReceivedDate = null;
                m_DeliveredBy=0;
                m_DriverName="";
                m_DeliveryDate= null;
                m_DelivertTime = null;
                m_ReceivedBy="";
                m_IsDCRReceived=0;
                m_RemBy_DeliveryIncharge = "";

            }
            else
            {


                m_OBDReceivedDT = (DB.RSFieldDateTime(rs, "OBDReceivedOn") == DateTime.MinValue ? DateTime.UtcNow.ToString("dd-MMM-yyyy HH:mm:ss") : DB.RSFieldDateTime(rs, "OBDReceivedOn").ToString());
               // DB.RSFieldDateTime(rs, "OBDReceivedOn").ToString("yyyy-MM-dd");
                m_NoofLines = DB.RSFieldInt(rs, "NoofLines");
                m_TotalQuantity = Convert.ToInt32(DB.RSFieldDecimal(rs, "TotalQuantity"));
                //m_SentForPGI_DT = (DB.RSFieldDateTime(rs, "SentForPGIOn") == DateTime.MinValue ? DateTime.UtcNow.ToString("dd-MMM-yyyy HH:mm:ss") : DB.RSFieldDateTime(rs, "SentForPGIOn").ToString());
                //m_SentForPGI_DT = DB.RSFieldDateTime(rs, "SentForPGIOn").ToString();
                m_SentForPGI_DT = DB.RSField(rs, "SentForPGIOn").ToString();
                m_RemByStoreIncharge = DB.RSField(rs, "RemByStoreIncharge");
                m_StoreInchargeID = DB.RSFieldInt(rs, "StoreInchargeID");
                m_CheckedBy = DB.RSFieldInt(rs, "CheckedBy").ToString();
                m_DocumentReceivedDate = DB.RSField(rs, "DocumentReceivedDate");
                //m_DocumentReceivedDate = DB.RSFieldDateTime(rs, "DocumentReceivedDate").ToLongDateString();
                //m_DocumentReceivedDate = ((DB.RSFieldDateTime(rs, "DocumentReceivedDate") == DateTime.MinValue) ? DateTime.UtcNow.ToString("dd-MMM-yyyy") : DB.RSFieldDateTime(rs, "DeliveryDate").ToString().Split(' ')[0]);
                m_PickedBy = DB.RSFieldInt(rs, "PickedBy");
                m_TransferedtoStoreID = DB.RSFieldInt(rs, "TransferedToWarehouseID");
                m_OBDTrack_WHID = DB.RSFieldInt(rs, "OutboundTracking_WarehouseID");

                m_InstructionModeID = DB.RSFieldInt(rs, "InstructionModeID");
                m_Requester = DB.RSField(rs, "Requester").ToString();
                m_DocumentNumber = DB.RSField(rs, "DocumentNumber");
                m_DeliveredBy = DB.RSFieldInt(rs, "DeliveredBy");
                m_DriverName = DB.RSField(rs, "DriverName");

                //m_DeliveryDate = DB.RSFieldDateTime(rs, "DeliveryDate").ToString();
                m_DeliveryDate = DB.RSField(rs, "DeliveryDate").ToString();
                m_DelivertTime = DB.RSField(rs, "DeliveryTime").ToString();
                //m_DeliveryDate = ((DB.RSFieldDateTime(rs, "DeliveryDate") == DateTime.MinValue) ? DateTime.UtcNow.ToString("dd-MMM-yyyy") : DB.RSFieldDateTime(rs, "DeliveryDate").ToString().ToString().Split(' ')[0]);
                m_ReceivedBy = DB.RSField(rs, "ReceivedBy");
                m_IsDCRReceived = DB.RSFieldTinyInt(rs, "IsPODReceived");
                m_RemBy_DeliveryIncharge = DB.RSField(rs, "RemByDeliveryIncharge");


            }
            


            rs.Close();


        }
        
        public int InitiateOBDTracking()
        {
            try
            {
               
                //INSERT NEW OBD Tracking
                StringBuilder strUpdateOBDTracking = new StringBuilder(2500);

                strUpdateOBDTracking.Append("DECLARE @NewUpdateOutboundID int;  ");
                strUpdateOBDTracking.Append("EXEC  [sp_OBD_UpsertOutbound] ");

                strUpdateOBDTracking.Append("@OBDNumber=" + DB.SQuote(m_OBDNumber) + ",");
                strUpdateOBDTracking.Append("@DocumentTypeID=" + m_DocumentTypeID + ",");
                strUpdateOBDTracking.Append("@OBDDate=" +  (m_OBDDate == null ? "NULL" : DB.SQuote(m_OBDDate))  + ",");

                //strUpdateOBDTracking.Append("@CustomerName=" + DB.SQuote(m_CustomerName) + ",");
                strUpdateOBDTracking.Append("@CustomerID=" + m_CustomerID + ",");
                strUpdateOBDTracking.Append("@RequestedBy=" + m_RequestedByID + ",");

                strUpdateOBDTracking.Append("@DivisionID=" + CommonLogic.IIF(m_MMDivisionID.ToString()!="0" ,m_MMDivisionID.ToString(),"NULL") + ",");

                strUpdateOBDTracking.Append("@DepartmentID=" +  CommonLogic.IIF( m_MMDepartmentID.ToString()!="0",m_MMDepartmentID.ToString(),"NULL") + ",");
                strUpdateOBDTracking.Append("@WarehouseIDs=" + DB.SQuote(m_ReferedStoreIDs) + ",");

                strUpdateOBDTracking.Append("@RemByIni_OnCreation=" + DB.SQuote(m_RemByIni_OnCreation) + ",");
                strUpdateOBDTracking.Append("@DeliveryStatusID=" + m_DeliveryStatusID.ToString() + ",");
                strUpdateOBDTracking.Append("@IsReservationDelivery=" + m_IsReservationDelivery.ToString() + ",");
                strUpdateOBDTracking.Append("@IsDNPublished=" + m_IsDNPublished.ToString() +",");
                strUpdateOBDTracking.Append("@TenantID=" + m_TenantID.ToString() + ",");


                strUpdateOBDTracking.Append("@PriorityLevel=" + m_PriorityLevel.ToString() + ",");

                if (m_PriorityDateTime != null)
                    strUpdateOBDTracking.Append("@PriorityDateTime=" + DB.SQuote(m_PriorityDateTime) + ",");
                else
                    strUpdateOBDTracking.Append("@PriorityDateTime=NULL,");
                strUpdateOBDTracking.Append("@LastModifiedBy=" + m_LastModifiedBy.ToString() + ",");
                strUpdateOBDTracking.Append("@InitiatedBy=" + m_InitiatedBy.ToString() + ",");
                strUpdateOBDTracking.Append("@CreatedBy=" + m_CreatedBy.ToString() + ",");
                strUpdateOBDTracking.Append("@OutboundID=" + m_OBDTrackingID.ToString() +",    ");
                strUpdateOBDTracking.Append("  @NewOutboundID=@NewUpdateOutboundID OUTPUT     ;");
                strUpdateOBDTracking.Append("  select @NewUpdateOutboundID AS N");


                
                
                int retValue = DB.GetSqlN(strUpdateOBDTracking.ToString());
                m_OBDTrackingID = retValue;
                return retValue;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool UpdateOBDTracking_Initiator()
        {
            
                //UPDATE  OBD Tracking
                try
                {

                
                    StringBuilder strUpdateOBDTracking = new StringBuilder(2500);

                    strUpdateOBDTracking.Append("DECLARE @NewUpdateOutboundID int;  ");
                    strUpdateOBDTracking.Append("EXEC  [sp_OBD_UpsertOutbound] ");

                    strUpdateOBDTracking.Append("@OBDNumber=" + DB.SQuote(m_OBDNumber) + ",");
                    strUpdateOBDTracking.Append("@DocumentTypeID=" + m_DocumentTypeID + ",");
                    strUpdateOBDTracking.Append("@OBDDate=" + (m_OBDDate == null ? "NULL" : DB.SQuote(m_OBDDate)) + ",");

                    //strUpdateOBDTracking.Append("@CustomerName=" + DB.SQuote(m_CustomerName) + ",");
                    strUpdateOBDTracking.Append("@CustomerID=" + m_CustomerID + ",");
                    strUpdateOBDTracking.Append("@RequestedBy=" + m_RequestedByID + ",");

                    strUpdateOBDTracking.Append("@DivisionID=" +  m_MMDivisionID.ToString() + ",");

                    strUpdateOBDTracking.Append("@DepartmentID=" +  m_MMDepartmentID.ToString() + ",");
                    strUpdateOBDTracking.Append("@WarehouseIDs=" + DB.SQuote(m_ReferedStoreIDs) + ",");

                    strUpdateOBDTracking.Append("@RemByIni_OnCreation=" + DB.SQuote(m_RemByIni_OnCreation) + ",");
                    strUpdateOBDTracking.Append("@DeliveryStatusID=" + m_DeliveryStatusID.ToString() + ",");
                    strUpdateOBDTracking.Append("@IsReservationDelivery=" + m_IsReservationDelivery.ToString() + ",");
                    strUpdateOBDTracking.Append("@IsDNPublished=" + m_IsDNPublished.ToString() + ",");
                    strUpdateOBDTracking.Append("@TenantID=" + m_TenantID.ToString() + ",");


                    strUpdateOBDTracking.Append("@PriorityLevel=" + m_PriorityLevel.ToString() + ",");

                    if (m_PriorityDateTime != null)
                        strUpdateOBDTracking.Append("@PriorityDateTime=" + DB.SQuote(m_PriorityDateTime) + ",");
                    else
                        strUpdateOBDTracking.Append("@PriorityDateTime=NULL,");
                    strUpdateOBDTracking.Append("@LastModifiedBy=" + m_LastModifiedBy.ToString() + ",");
                    strUpdateOBDTracking.Append("@InitiatedBy=" + m_InitiatedBy.ToString() + ",");
                    strUpdateOBDTracking.Append("@CreatedBy=" + m_CreatedBy.ToString() + ",");
                    strUpdateOBDTracking.Append("@OutboundID=" + m_OBDTrackingID.ToString() + ",    ");
                    strUpdateOBDTracking.Append("  @NewOutboundID=@NewUpdateOutboundID OUTPUT     ;");
                    strUpdateOBDTracking.Append("  select @NewUpdateOutboundID AS N");




                    int retValue = DB.GetSqlN(strUpdateOBDTracking.ToString());
                    m_OBDTrackingID = retValue;

                    if (retValue != 0)
                        return true;
                    else
                        return false;

                    
                }
                catch (Exception ex)
                {
                    return false;
                }
                
                
            
        }
          
        public int UpdateOBDTracking_SentForPGI()
        {
          
            try
            {
               // Updaing the Pick N Check Process and Sending it to PGI
                StringBuilder strUpdateOBDTracking = new StringBuilder(2500);
                
                strUpdateOBDTracking.Append("DECLARE @NewUpdateOutboundID int;  ");
                strUpdateOBDTracking.Append("EXEC [sp_OBD_UpsertOutboundTracking_Warehouse_New]      ");

                strUpdateOBDTracking.Append("  @OutboundID=" + m_OBDTrackingID.ToString() + ",");
                strUpdateOBDTracking.Append("@StoreInchargeID=" + m_StoreInchargeID.ToString() + ",");
                strUpdateOBDTracking.Append("@OB_RefWarehouse_DetailsID=" + m_RefWHID.ToString() + ",");
                strUpdateOBDTracking.Append("@OBDReceivedOn=" + (m_OBDReceivedDT == null ? "NULL" : DB.SQuote(m_OBDReceivedDT)) + ",");
                strUpdateOBDTracking.Append("@TotalQuantity=" + m_TotalQuantity.ToString() + ",");
                strUpdateOBDTracking.Append("@NoofLines=" + m_NoofLines.ToString() + ",");
                strUpdateOBDTracking.Append("@PickedBy=" + m_PickedBy.ToString() + ",");
                strUpdateOBDTracking.Append("@CheckedBy=" + m_CheckedBy + ",");

                strUpdateOBDTracking.Append("@RemByStoreIncharge=" + DB.SQuote(m_RemByStoreIncharge) + ",");
                strUpdateOBDTracking.Append("@SentForPGIOn=" + (m_SentForPGI_DT == null ? "NULL" : DB.SQuote(m_SentForPGI_DT)) + ",");
                strUpdateOBDTracking.Append("@TransferedToWarehouseID=" +m_TransferedtoStoreID+",");
                strUpdateOBDTracking.Append("@TenantID=" + m_TenantID + ",");
                strUpdateOBDTracking.Append("@CreatedBy=" + m_CreatedBy + ",");
                strUpdateOBDTracking.Append("@WarehouseID=" + m_StoreID + ",");
                strUpdateOBDTracking.Append("@OutboundTracking_WarehouseID=" + m_OBDTrack_WHID  + ",");
                strUpdateOBDTracking.Append("@NewOutboundTracking_WarehouseID=@NewUpdateOutboundID OUTPUT     ;");
                strUpdateOBDTracking.Append("  select @NewUpdateOutboundID AS N ;");

                int Result = DB.GetSqlN(strUpdateOBDTracking.ToString());

                return Result;

                //if (DB.GetSqlN(strUpdateOBDTracking.ToString()) != 0)
                //{
                //   // DB.ExecuteSQL("     Update OBD_Outbound set DeliveryStatusID=" + 5 + " where OutboundID=" + m_OBDTrackingID);
                //    return Result;
                //}
                //else {
                //    return false;
                //}

                
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool UpdateOBDTracking_Delivered()
        {
            try
            {
                //UPDATE  OBD Delivered Transaction
                StringBuilder strUpdateOBDTracking = new StringBuilder(2500);
                strUpdateOBDTracking.Append("BEGIN TRANSACTION   ");
                strUpdateOBDTracking.Append("     UPDATE  OBD_OutboundTracking_Warehouse SET   ");
                strUpdateOBDTracking.Append("TransferedToWarehouseID=" + m_TransferedtoStoreID.ToString() + ",");
                strUpdateOBDTracking.Append("InstructionModeID=" + (m_InstructionModeID == 0 ? "NULL" : m_InstructionModeID.ToString()) + ",");
                strUpdateOBDTracking.Append("Requester=" + DB.SQuote(m_Requester) + ",");
                strUpdateOBDTracking.Append("DocumentNumber=" + DB.SQuote(m_DocumentNumber) + ",");
                strUpdateOBDTracking.Append("DocumentReceivedDate=" + (m_DocumentReceivedDate == null ? "NULL" : DB.SQuote(m_DocumentReceivedDate)) + ",");
                strUpdateOBDTracking.Append("DeliveryDate=" + (m_DeliveryDate == null ? "NULL" : DB.SQuote(m_DeliveryDate)) + ",");
                strUpdateOBDTracking.Append("DeliveredBy=" + m_DeliveredBy.ToString() + ",");
                strUpdateOBDTracking.Append("DriverName=" + DB.SQuote(m_DriverName) + ",");
                strUpdateOBDTracking.Append("ReceivedBy=" + DB.SQuote(m_ReceivedBy) + ",");
                strUpdateOBDTracking.Append("RemByDeliveryIncharge=" + DB.SQuote(m_RemBy_DeliveryIncharge) + ",");

                strUpdateOBDTracking.Append("IsPODReceived=" + m_IsDCRReceived.ToString());
                strUpdateOBDTracking.Append(" WHERE OutboundID=" + m_OBDTrackingID.ToString());
                strUpdateOBDTracking.Append(" AND OB_RefWarehouse_DetailsID=" + m_RefWHID.ToString());

                strUpdateOBDTracking.Append(" ;    Update OBD_Outbound set DeliveryStatusID="+m_DeliveryStatusID+" where OutboundID="+m_OBDTrackingID);
                
                strUpdateOBDTracking.Append("    COMMIT");
                
                DB.ExecuteSQL(strUpdateOBDTracking.ToString());

                DB.ExecuteSQL("[dbo].[sp_INB_CloseSOStatus]  @OutboundID=" + m_OBDTrackingID + ",@SOStatusID=4,@Status=1,@UpdatedBy="+customprinciple.UserID.ToString());

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateOBDTracking_Transfered()
        {
            try
            {
                //UPDATE  OBD Delivered Transaction
                StringBuilder strUpdateOBDTracking = new StringBuilder(2500);
                //strUpdateOBDTracking.Append("BEGIN TRANSACTION ");
                strUpdateOBDTracking.Append("UPDATE  OBDTracking_WareHouse SET ");
                strUpdateOBDTracking.Append("TransferedToStoreID=" + m_TransferedtoStoreID.ToString() + ",");
                strUpdateOBDTracking.Append("InstructionModeID=" + m_InstructionModeID.ToString() + ",");
                strUpdateOBDTracking.Append("Requester=" + DB.SQuote(m_Requester) + ",");
                strUpdateOBDTracking.Append("DocumentNumber=" + DB.SQuote(m_DocumentNumber) + ",");
                strUpdateOBDTracking.Append("DocumentReceivedDate=" + (m_DocumentReceivedDate == null ? "NULL" : DB.SQuote(m_DocumentReceivedDate)) + ",");
                strUpdateOBDTracking.Append("DeliveryDate=" + (m_DeliveryDate==null?"NULL": DB.SQuote(m_DeliveryDate) )+ ",");
                strUpdateOBDTracking.Append("DeliveredBy=" + m_DeliveredBy.ToString() + ",");
                strUpdateOBDTracking.Append("DriverName=" + DB.SQuote(m_DriverName) + ",");
                strUpdateOBDTracking.Append("ReceivedBy=" + DB.SQuote(m_ReceivedBy) + ",");
                strUpdateOBDTracking.Append("RemBy_DeliveryIncharge=" + DB.SQuote(m_RemBy_DeliveryIncharge) + ",");
                //strUpdateOBDTracking.Append("DeliveryStatusID=" + m_DeliveryStatusID.ToString());
                strUpdateOBDTracking.Append("IsDCRReceived=" + m_IsDCRReceived.ToString());
                strUpdateOBDTracking.Append(" WHERE OBDTrackingID=" + m_OBDTrackingID.ToString() );
                strUpdateOBDTracking.Append(" AND StoreID=" + m_StoreID.ToString());
               // strUpdateOBDTracking.Append(" UPDATE OBDTracking SET DeliveryStatusID=" + m_DeliveryStatusID.ToString() + ",PriorityLevel =Null,PriorityDateTime=Null WHERE OBDTrackingID=" + m_OBDTrackingID.ToString());
               // DB.ExecuteSQL(strUpdateOBDTracking.ToString());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        } 

        public int UpdateOBDTracking_PGIDone()
        {
            try
            {
                // Updaing the PGI Process and Sending it to Delivery
                StringBuilder strUpdateOBDTracking = new StringBuilder(2500);

                strUpdateOBDTracking.Append("DECLARE @NewUpdateOutboundID int;  ");
                strUpdateOBDTracking.Append("EXEC [sp_OBD_UpsertPGI] ");

                strUpdateOBDTracking.Append("  @OutboundID=" + m_OBDTrackingID.ToString() + ",");
                strUpdateOBDTracking.Append("@RemByIni_AfterPGI=" + (m_RemByIni_AfterPGI=="" ? "NULL": DB.SQuote(m_RemByIni_AfterPGI.ToString())) + ",");
                strUpdateOBDTracking.Append("@PGIDoneBy=" + m_PGIDoneBy.ToString() +",");
                strUpdateOBDTracking.Append("@PGIDoneOn=" +DB.SQuote(m_PGIDone_DT) + ",");
                strUpdateOBDTracking.Append("@DocumentTypeID=" + DocumentTypeID + ",");
                strUpdateOBDTracking.Append("@OB_RefWarehouse_DetailsID=" + RefWHID + ",");
                strUpdateOBDTracking.Append("@UserID="+ PackedBy + ",");

                strUpdateOBDTracking.Append("@UpdatedBy=" + customprinciple.UserID.ToString() + ",");

                
                strUpdateOBDTracking.Append("@NewOutboundID=@NewUpdateOutboundID OUTPUT ;");
                strUpdateOBDTracking.Append("  select @NewUpdateOutboundID AS N ;");

                int Result = DB.GetSqlN(strUpdateOBDTracking.ToString());

                return Result;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int UpdateOBDTracking_Packing()
        {
            try
            {
                // Updaing the PGI Process and Sending it to Delivery
                StringBuilder strUpdateOBDTracking = new StringBuilder(2500);

                strUpdateOBDTracking.Append("DECLARE @NewUpdateOutboundID int;  ");
                strUpdateOBDTracking.Append("EXEC [sp_OBD_UpsertPacking] ");

                strUpdateOBDTracking.Append("  @OutboundID=" + m_OBDTrackingID.ToString() + ",");
                strUpdateOBDTracking.Append("@PackedOn=" + (m_PackedOn == "" ? "NULL" :DB.SQuote(m_PackedOn.ToString())) + ",");
                strUpdateOBDTracking.Append("@PackedBy=" + m_PackedBy.ToString() + ",");
                strUpdateOBDTracking.Append("@PackingRemarks=" + (m_PackingRemarks == "" ? "NULL" : DB.SQuote(m_PackingRemarks)) + ",");
                strUpdateOBDTracking.Append("@WarehouseID=" + m_StoreID + ",");
                strUpdateOBDTracking.Append("@UpdatedBy=" + customprinciple.UserID.ToString() + ",");
                

                strUpdateOBDTracking.Append("@NewOutboundID=@NewUpdateOutboundID OUTPUT ;");
                strUpdateOBDTracking.Append("  select @NewUpdateOutboundID AS N ;");

                int Result =  DB.GetSqlN(strUpdateOBDTracking.ToString());

                return Result;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int UpdateOBDTracking_Delivery()
        {
            try
            {
                //UPDATE  OBD Delivered Transaction
                StringBuilder strUpdateOBDTracking = new StringBuilder(2500);

                strUpdateOBDTracking.Append("DECLARE @NewUpdateOutboundID int;  ");
                strUpdateOBDTracking.Append("EXEC [sp_OBD_UpsertDelivery] ");
                strUpdateOBDTracking.Append("  @OutboundID=" + m_OBDTrackingID.ToString() + ",");
                strUpdateOBDTracking.Append("@TransferedToWarehouseID=" + (m_TransferedtoStoreID == 0 ? "NULL" : m_TransferedtoStoreID.ToString()) + ",");
                strUpdateOBDTracking.Append("@InstructionModeID=" + (m_InstructionModeID == 0 ? "NULL" : m_InstructionModeID.ToString()) + ",");
                strUpdateOBDTracking.Append("@Requester=" + (m_Requester=="" ? "NULL" : DB.SQuote(m_Requester)) + ",");
                strUpdateOBDTracking.Append("@DocumentNumber=" + (m_DocumentNumber=="" ? "NULL" : DB.SQuote(m_DocumentNumber)) + ",");
                strUpdateOBDTracking.Append("@DocumentReceivedDate=" + (m_DocumentReceivedDate == null ? "NULL" : DB.SQuote(m_DocumentReceivedDate)) + ",");
                strUpdateOBDTracking.Append("@DeliveryDate=" + (m_DeliveryDate == null ? "NULL" : DB.SQuote(m_DeliveryDate)) + ",");
                strUpdateOBDTracking.Append("@DeliveredBy=" + m_DeliveredBy.ToString() + ",");
                strUpdateOBDTracking.Append("@DriverName=" + (m_DriverName == "" ? "NULL" : DB.SQuote(m_DriverName)) + ",");
                strUpdateOBDTracking.Append("@ReceivedBy=" + (m_ReceivedBy == "" ? "NULL" : DB.SQuote(m_ReceivedBy)) + ",");
                strUpdateOBDTracking.Append("@RemByDeliveryIncharge=" + (m_RemBy_DeliveryIncharge == "" ? "NULL" : DB.SQuote(m_RemBy_DeliveryIncharge)) + ",");
                strUpdateOBDTracking.Append("@IsPODReceived=" + m_IsDCRReceived.ToString() + ",");
                strUpdateOBDTracking.Append("@OB_RefWarehouse_DetailsID=" + m_RefWHID.ToString() + ",");
                strUpdateOBDTracking.Append("@DeliveryStatusID=" + m_DeliveryStatusID + ",");
                //strUpdateOBDTracking.Append("@WareHouseID=" + m_TransferedtoStoreID + ",");
                strUpdateOBDTracking.Append("@NewOutboundID=@NewUpdateOutboundID OUTPUT,");
                strUpdateOBDTracking.Append("@UpdatedBy=" + customprinciple.UserID.ToString());
                strUpdateOBDTracking.Append("  select @NewUpdateOutboundID AS N ;");

                int Result = DB.GetSqlN(strUpdateOBDTracking.ToString());

                //DB.ExecuteSQL("[dbo].[sp_INB_CloseSOStatus]  @OutboundID=" + m_OBDTrackingID + ",@SOStatusID=4,@Status=1");
                return Result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        #endregion
    }

    
}
