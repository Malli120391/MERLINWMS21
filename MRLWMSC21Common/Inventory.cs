using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MRLWMSC21Common
{
    public class Inventory
    {
        public Inventory() { }

        public class MaterialPropertyIN
        {
            private String _MCode;
            private int _KitPlannerID;
            private String _Description;
            private String _GoodsMovementDetailsIDs;

            private Decimal _InvoiceQuantity;
            private Decimal _Quantity;
            private int _MTypeID;

            private string _MUoM;
            private int _MUoMID;
            private Decimal _MUoMQty;

            private string _BUoM;
            private int _BUoMID;
            private Decimal _BUoMQty;

            private int _PUoMID;
            private Decimal _PUoMQty;
            
            private string _InvUoM;
            private int _InvUoMID;
            private Decimal _InvUoMQty;

            private Decimal _AltUoMQty;

            private Boolean _IsMfgDateReqIN;
            private Boolean _IsExpiryDateReqIN;
            private Boolean _IsBatchOrLotReqIN;
            private Boolean _IsSerialReqIN;

            private Boolean _IsMfgDateReqOUT;
            private Boolean _IsExpiryDateReqOUT;
            private Boolean _IsBatchOrLotReqOUT;
            private Boolean _IsSerialReqOUT;


            private int _ShipmentTypeID;
            private int _StockTypeID;
            private int _MMPlantID;
            private String _POSORefNumber;

            private int _IsActive;
            private int _Deleted;


            public String MCode
            {
                get { return _MCode; }
                set { _MCode = value; }
            }

            public int KitPlannerID
            {
                get { return _KitPlannerID ; }
                set { _KitPlannerID = value; }
            }

            public String Description
            {
                get { return _Description; }
                set { _Description = value; }
            }

            public String GoodsMovementDetailsIDs
            {
                get { return _GoodsMovementDetailsIDs; }
                set { _GoodsMovementDetailsIDs = value; }
            }




            public Decimal InvoiceQuantity
            {
                get { return _InvoiceQuantity; }
                set { _InvoiceQuantity = value; }
            }

            public Decimal Quantity
            {
                get { return _Quantity; }
                set { _Quantity = value; }
            }

            public int IsActive
            {
                get { return _IsActive; }
                set { _IsActive = value; }
            }

            public int Deleted
            {
                get { return _Deleted; }
                set { _Deleted = value; }
            }

            public int MTypeID
            {
                get { return _MTypeID; }
                set { _MTypeID = value; }
            }

            public string  MUoM
            {
                get { return _MUoM; }
                set { _MUoM = value; }
            }

            public int MUoMID
            {
                get { return _MUoMID; }
                set { _MUoMID = value; }
            }

            public Decimal MUoMQty
            {
                get { return _MUoMQty; }
                set { _MUoMQty = value; }
            }



            
            public string BUoM
            {
                get { return _BUoM; }
                set { _BUoM = value; }
            }

            public int BUoMID
            {
                get { return _BUoMID; }
                set { _BUoMID = value; }
            }

            public Decimal BUoMQty
            {
                get { return _BUoMQty; }
                set { _BUoMQty = value; }
            }

            public int PUoMID
            {
                get { return _PUoMID; }
                set { _PUoMID = value; }
            }

            public Decimal PUoMQty
            {
                get { return _PUoMQty; }
                set { _PUoMQty = value; }
            }

            public string InvUoM
            {
                get { return _InvUoM; }
                set { _InvUoM = value; }
            }


            public int InvUoMID
            {
                get { return _InvUoMID; }
                set { _InvUoMID = value; }
            }

            public Decimal InvUoMQty
            {
                get { return _InvUoMQty; }
                set { _InvUoMQty = value; }
            }

            public Decimal AltUoMQty
            {
                get { return _AltUoMQty ; }
                set { _AltUoMQty = value; }
            }

            public Boolean IsMfgDateReqIN
            {
                get { return _IsMfgDateReqIN; }
                set { _IsMfgDateReqIN = value; }
            }

            public Boolean IsExpiryDateReqIN
            {
                get { return _IsExpiryDateReqIN; }
                set { _IsExpiryDateReqIN = value; }
            }

            public Boolean IsBatchOrLotReqIN
            {
                get { return _IsBatchOrLotReqIN; }
                set { _IsBatchOrLotReqIN = value; }
            }

            public Boolean IsSerialReqIN
            {
                get { return _IsSerialReqIN; }
                set { _IsSerialReqIN = value; }
            }


            public Boolean IsMfgDateReqOUT
            {
                get { return _IsMfgDateReqOUT; }
                set { _IsMfgDateReqOUT = value; }
            }

            public Boolean IsExpiryDateReqOUT
            {
                get { return _IsExpiryDateReqOUT; }
                set { _IsExpiryDateReqOUT = value; }
            }

            public Boolean IsBatchOrLotReqOUT
            {
                get { return _IsBatchOrLotReqOUT; }
                set { _IsBatchOrLotReqOUT = value; }
            }

            public Boolean IsSerialReqOUT
            {
                get { return _IsSerialReqOUT; }
                set { _IsSerialReqOUT = value; }
            }




            public int ShipmentTypeID
            {
                get { return _ShipmentTypeID; }
                set { _ShipmentTypeID = value; }
            }

            public int StockTypeID
            {
                get { return _StockTypeID; }
                set { _StockTypeID = value; }
            }

           

            public int MMPlantID
            {
                get { return _MMPlantID; }
                set { _MMPlantID = value; }
            }

            public String POSORefNumber
            {
                get { return _POSORefNumber; }
                set { _POSORefNumber = value; }
            }



            public static MaterialPropertyIN GetMaterialProperties(String MaterialCode, String ATCRef_IB_OBD, String LineNumber)
            {
                MaterialPropertyIN thisMatProp = new MaterialPropertyIN();

                IDataReader rsGetData = DB.GetRS("[sp_Inv_GetPOQuantityListForStoreRef] @CheckInvQty=0,@InboundTrackingID=0,@StoreRefNo=" + DB.SQuote(ATCRef_IB_OBD) + ",@WarehouseID='',@MCode=" + DB.SQuote(MaterialCode) + ",@PONumber='',@IsActive=0,@LineNumber=" + LineNumber);

                while (rsGetData.Read())
                {
                    thisMatProp.MCode = DB.RSField(rsGetData, "MCode");
                    thisMatProp.KitPlannerID  = DB.RSFieldInt(rsGetData, "KitPlannerID");
                    thisMatProp.Description = DB.RSField(rsGetData, "MDescription");
                    thisMatProp.GoodsMovementDetailsIDs = DB.RSField(rsGetData, "GoodsMovementDetailsIDs");

                    thisMatProp.MTypeID = DB.RSFieldInt(rsGetData, "MTypeID");

                    thisMatProp.MUoM = DB.RSField(rsGetData, "MUoM");
                    thisMatProp.MUoMID = DB.RSFieldInt(rsGetData, "MUoMID");
                    thisMatProp.MUoMQty = DB.RSFieldDecimal(rsGetData, "MUoMQty");

                    thisMatProp.BUoM = DB.RSField(rsGetData, "BUoM");
                    thisMatProp.BUoMID = DB.RSFieldInt(rsGetData, "BUoMID");
                    thisMatProp.BUoMQty = DB.RSFieldDecimal(rsGetData, "BUoMQty");


                    thisMatProp.PUoMID = DB.RSFieldInt(rsGetData, "PUoMID");
                    thisMatProp.PUoMQty = DB.RSFieldDecimal(rsGetData, "PUoMQty");

                    thisMatProp.InvUoM = DB.RSField(rsGetData, "InvUoM");
                    thisMatProp.InvUoMID = DB.RSFieldInt(rsGetData, "InvUoMID");
                    thisMatProp.InvUoMQty = DB.RSFieldDecimal(rsGetData, "InvUoMQty");

                    thisMatProp.AltUoMQty = DB.RSFieldDecimal(rsGetData, "AUoMQty");
                    
                    thisMatProp.ShipmentTypeID = DB.RSFieldInt(rsGetData, "ShipmentTypeID");

                    thisMatProp.StockTypeID = DB.RSFieldInt(rsGetData, "StockTypeID");
                    thisMatProp.MMPlantID = DB.RSFieldInt(rsGetData, "MMPlantID");
                    thisMatProp.POSORefNumber = DB.RSField(rsGetData, "POSORefNumber");

                    thisMatProp.IsMfgDateReqIN = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsMfgDateReqIN"));
                    thisMatProp.IsExpiryDateReqIN = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsExpiryDateReqIN"));
                    thisMatProp.IsBatchOrLotReqIN = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsBatchOrLotReqIN"));
                    thisMatProp.IsSerialReqIN = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsSerialReqIN"));

                    thisMatProp.IsMfgDateReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsMfgDateReqOUT"));
                    thisMatProp.IsExpiryDateReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsExpiryDateReqOUT"));
                    thisMatProp.IsBatchOrLotReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsBatchOrLotReqOUT"));
                    thisMatProp.IsSerialReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsSerialReqOUT"));

                    
                    thisMatProp.InvoiceQuantity = DB.RSFieldDecimal(rsGetData, "InvoiceQuantity");
                    thisMatProp.Quantity = DB.RSFieldDecimal(rsGetData, "Quantity");

                    thisMatProp.IsActive = DB.RSFieldTinyInt(rsGetData, "IsActive");
                    thisMatProp.Deleted = DB.RSFieldTinyInt(rsGetData, "Deleted");

                }
                rsGetData.Close();

                return thisMatProp;
            }

            

        }

        public class MaterialPropertyOUT
        {
            private String _MCode;
            private int _KitPlannerID;
            private String _Description;
            private String _GoodsMovementDetailsIDs;


            private int _MTypeID;
            
            private int _LineNumber;

            private int _MinPicUoMID;
            private Decimal _MinPickUoMQty;


            private int _BUoMID;
            private Decimal _BUoMQty;

            private int _SalesUoMID;
            private Decimal _SalesUoMQty;

            private Decimal _AltUoMQty;

            private Decimal _SOQuantity;
            private Decimal _QtyOnHand;


            private Boolean _IsMfgDateReqIN;
            private Boolean _IsExpiryDateReqIN;
            private Boolean _IsBatchOrLotReqIN;
            private Boolean _IsSerialReqIN;


            private Boolean _IsMfgDateReqOUT;
            private Boolean _IsExpiryDateReqOUT;
            private Boolean _IsBatchOrLotReqOUT;
            private Boolean _IsSerialReqOUT;



            private int _StockTypeID;
            private int _MMPlantID;
            private String _POSORefNumber;


            private String _MfgDates;
            private String _ExpDates;
            private String _BatchNos;
            private String _SerialNos;
            private String _Location;
            private String _LocationQty;
            private String _StockType;

            private int _IsActive;
            private int _Deleted;


            public String MCode
            {
                get { return _MCode; }
                set { _MCode = value; }
            }

            public int KitPlannerID
            {
                get { return _KitPlannerID ; }
                set { _KitPlannerID = value; }
            }

            public String Description
            {
                get { return _Description; }
                set { _Description = value; }
            }

            public String GoodsMovementDetailsIDs
            {
                get { return _GoodsMovementDetailsIDs; }
                set { _GoodsMovementDetailsIDs = value; }
            }

            public int MTypeID
            {
                get { return _MTypeID; }
                set { _MTypeID = value; }
            }

            public int LineNumber
            {
                get { return _LineNumber; }
                set { _LineNumber = value; }
            }

            public int MinPickUoMID
            {
                get { return _MinPicUoMID ; }
                set { _MinPicUoMID = value; }
            }

            public Decimal MinPickUoMQty
            {
                get { return _MinPickUoMQty ; }
                set { _MinPickUoMQty = value; }
            }

            public int BUoMID
            {
                get { return _BUoMID; }
                set { _BUoMID = value; }
            }

            public Decimal BUoMQty
            {
                get { return _BUoMQty; }
                set { _BUoMQty = value; }
            }




            public int SalesUoMID
            {
                get { return _SalesUoMID; }
                set { _SalesUoMID = value; }
            }

            public Decimal SalesUoMQty
            {
                get { return _SalesUoMQty; }
                set { _SalesUoMQty = value; }
            }

            public Decimal AltUoMQty
            {
                get { return _AltUoMQty; }
                set { _AltUoMQty = value; }
            }

            

            public Decimal SOQuantity
            {
                get { return _SOQuantity; }
                set { _SOQuantity = value; }
            }

            public Decimal QtyOnHand
            {
                get { return _QtyOnHand ; }
                set { _QtyOnHand = value; }
            }

            public int IsActive
            {
                get { return _IsActive; }
                set { _IsActive = value; }
            }

            public int Deleted
            {
                get { return _Deleted; }
                set { _Deleted = value; }
            }

            //IN
            public Boolean IsMfgDateReqIN
            {
                get { return _IsMfgDateReqIN; }
                set { _IsMfgDateReqIN = value; }
            }

            public Boolean IsExpiryDateReqIN
            {
                get { return _IsExpiryDateReqIN; }
                set { _IsExpiryDateReqIN = value; }
            }

            public Boolean IsBatchOrLotReqIN
            {
                get { return _IsBatchOrLotReqIN; }
                set { _IsBatchOrLotReqIN = value; }
            }

            public Boolean IsSerialReqIN
            {
                get { return _IsSerialReqIN; }
                set { _IsSerialReqIN = value; }
            }


            // OUT

            public Boolean IsMfgDateReqOUT
            {
                get { return _IsMfgDateReqOUT; }
                set { _IsMfgDateReqOUT = value; }
            }

            public Boolean IsExpiryDateReqOUT
            {
                get { return _IsExpiryDateReqOUT; }
                set { _IsExpiryDateReqOUT = value; }
            }

            public Boolean IsBatchOrLotReqOUT
            {
                get { return _IsBatchOrLotReqOUT; }
                set { _IsBatchOrLotReqOUT = value; }
            }

            public Boolean IsSerialReqOUT
            {
                get { return _IsSerialReqOUT; }
                set { _IsSerialReqOUT = value; }
            }


            public int StockTypeID
            {
                get { return _StockTypeID; }
                set { _StockTypeID = value; }
            }

            

            public int MMPlantID
            {
                get { return _MMPlantID; }
                set { _MMPlantID = value; }
            }

            public String POSORefNumber
            {
                get { return _POSORefNumber; }
                set { _POSORefNumber = value; }
            }

            public String MfgDates
            {
                get { return _MfgDates; }
                set { _MfgDates = value; }
            }

            public String ExpDates
            {
                get { return _ExpDates; }
                set { _ExpDates = value; }
            }


            public String BatchNos
            {
                get { return _BatchNos; }
                set { _BatchNos = value; }
            }

            public String SerialNos
            {
                get { return _SerialNos; }
                set { _SerialNos = value; }
            }

            public String Location
            {
                get { return _Location; }
                set { _Location = value; }
            }

            public String LocationQty
            {
                get { return _LocationQty; }
                set { _LocationQty = value; }
            }

            public String StockType
            {
                get { return _StockType; }
                set { _StockType = value; }
            }

            public static MaterialPropertyOUT GetMaterialProperties(int SOQID, int GoodsMovementDetailsID, String ATCRef_IB_OBD)
            {
                MaterialPropertyOUT thisMatProp = new MaterialPropertyOUT();

                IDataReader rsGetData = DB.GetRS("[sp_Inv_GetPOQuantityItemForOBDRef] @POQuantityOutboundID=" + SOQID + ",@OBDTrackingID=" + ATCRef_IB_OBD + ",@GoodsMovementDetailsID=" + GoodsMovementDetailsID + ",@WarehouseID=" + DB.SQuote(CommonLogic.Session("WarehouseID")));

                while (rsGetData.Read())
                {

                    thisMatProp.LineNumber = DB.RSFieldInt(rsGetData, "LineNumber");
                    thisMatProp.MCode = DB.RSField(rsGetData, "MCode");
                    thisMatProp.KitPlannerID  = DB.RSFieldInt(rsGetData, "KitPlannerID");
                    thisMatProp.Description = DB.RSField(rsGetData, "MDescription");
                    thisMatProp.GoodsMovementDetailsIDs = DB.RSField(rsGetData, "GoodsMovementDetailsIDs");

                    thisMatProp.MinPickUoMID = DB.RSFieldInt(rsGetData, "MUoMID");
                    thisMatProp.MinPickUoMQty = DB.RSFieldDecimal(rsGetData, "MUoMQty");

                    thisMatProp.BUoMID = DB.RSFieldInt(rsGetData, "BUoMID");
                    thisMatProp.BUoMQty = DB.RSFieldDecimal(rsGetData, "BUoMQty");

                    thisMatProp.SalesUoMID = DB.RSFieldInt(rsGetData, "SUoMID");
                    thisMatProp.SalesUoMQty = DB.RSFieldDecimal(rsGetData, "SUoMQty");
                    thisMatProp.AltUoMQty = DB.RSFieldDecimal(rsGetData, "AltUoMQty");

                    

                    thisMatProp.SOQuantity = DB.RSFieldDecimal(rsGetData, "Quantity");

                    
                    thisMatProp.StockTypeID = DB.RSFieldInt(rsGetData, "StockTypeID");
                    thisMatProp.MMPlantID = DB.RSFieldInt(rsGetData, "MMPlantID");
                    thisMatProp.POSORefNumber = DB.RSField(rsGetData, "SONumber");


                    thisMatProp.IsMfgDateReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsMfgDateReqOUT"));
                    thisMatProp.IsExpiryDateReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsExpiryDateReqOUT"));
                    thisMatProp.IsBatchOrLotReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsBatchOrLotReqOUT"));
                    thisMatProp.IsSerialReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsSerialReqOUT"));


                    thisMatProp.SOQuantity = DB.RSFieldDecimal(rsGetData, "Quantity");
                    thisMatProp.MfgDates = DB.RSField(rsGetData, "MfgDates");
                    thisMatProp.ExpDates = DB.RSField(rsGetData, "ExpDates");
                    thisMatProp.BatchNos = DB.RSField(rsGetData, "BatchNos");
                    thisMatProp.SerialNos = DB.RSField(rsGetData, "SerialNos");
                    thisMatProp.Location = DB.RSField(rsGetData, "Location");
                    thisMatProp.LocationQty = DB.RSField(rsGetData, "LocationQty");
                    thisMatProp.QtyOnHand = DB.RSFieldDecimal(rsGetData, "QtyOnHand");
                    thisMatProp.StockType = DB.RSField(rsGetData, "StockType");

                    thisMatProp.IsActive = DB.RSFieldTinyInt(rsGetData, "IsActive");
                    thisMatProp.Deleted = DB.RSFieldTinyInt(rsGetData, "Deleted");



                }
                rsGetData.Close();

                return thisMatProp;
            }


            public static MaterialPropertyOUT GetMaterialProperties(int SOQID, int MaterialMasterID, int LineNumber, int OBDTrackingID, int LocationID, int StockTypeID, int MMPlantID,  int KitID, DateTime MfgDate, DateTime ExpDate, String BatchNo, String SerialNo)
            {
                MaterialPropertyOUT thisMatProp = new MaterialPropertyOUT();

                //IDataReader rsGetData = DB.GetRS("[sp_Inv_GetPOQuantityItemForOBDRefNewLogic] =" + MaterialMasterID + ",@LineNumber=" + LineNumber + ",@GoodsMovementDetailsID=" + GoodsMovementDetailsID + ",@WarehouseID=" + DB.SQuote(CommonLogic.Session("WarehouseID")));


                StringBuilder strSQL = new StringBuilder();
                strSQL.Append("[sp_Inv_GetPOQuantityItemForOBDRefNewLogic] ");
                strSQL.Append("@POQuantityOutboundID=" + SOQID);
                strSQL.Append(",@MaterialMasterID=" + MaterialMasterID);
                strSQL.Append(",@LineNumber=" + LineNumber);
                strSQL.Append(",@OBDTrackingID=" + OBDTrackingID);
                strSQL.Append(",@LocationID=" + LocationID);
                strSQL.Append(",@StockTypeID=" + StockTypeID);
                strSQL.Append(",@MMPlantID=" + MMPlantID);
                strSQL.Append(",@KitID=" +  KitID.ToString());// (KitID==0 ? "NULL" : KitID.ToString()));
                strSQL.Append(",@MfgDate=" + (MfgDate == DateTime.MinValue ?  DB.DateQuote("01/01/1900") : DB.DateQuote(MfgDate.ToString("MM/dd/yyyy"))));
                strSQL.Append(",@ExpDate=" + (ExpDate == DateTime.MinValue ? DB.DateQuote("01/01/1900") : DB.DateQuote(ExpDate.ToString("MM/dd/yyyy"))));
                strSQL.Append(",@BatchNo=" + (BatchNo == "" ? "NULL" : DB.SQuote(BatchNo.Trim())));
                strSQL.Append(",@SerialNo=" + (SerialNo == "" ? "NULL" : DB.SQuote(SerialNo.Trim())));
                

                IDataReader rsGetData = DB.GetRS(strSQL.ToString());


                while (rsGetData.Read())
                {

                    thisMatProp.LineNumber = DB.RSFieldInt(rsGetData, "LineNumber");
                    thisMatProp.MCode = DB.RSField(rsGetData, "MCode");
                    thisMatProp.KitPlannerID = DB.RSFieldInt(rsGetData, "KitPlannerID");
                    thisMatProp.Description = DB.RSField(rsGetData, "MDescription");
                    thisMatProp.GoodsMovementDetailsIDs = DB.RSField(rsGetData, "GoodsMovementDetailsIDs");

                    thisMatProp.MinPickUoMID = DB.RSFieldInt(rsGetData, "MUoMID");
                    thisMatProp.MinPickUoMQty = DB.RSFieldDecimal(rsGetData, "MUoMQty");

                    thisMatProp.BUoMID = DB.RSFieldInt(rsGetData, "BUoMID");
                    thisMatProp.BUoMQty = DB.RSFieldDecimal(rsGetData, "BUoMQty");

                    thisMatProp.SalesUoMID = DB.RSFieldInt(rsGetData, "SUoMID");
                    thisMatProp.SalesUoMQty = DB.RSFieldDecimal(rsGetData, "SUoMQty");
                    thisMatProp.AltUoMQty = DB.RSFieldDecimal(rsGetData, "AltUoMQty");



                    thisMatProp.SOQuantity = DB.RSFieldDecimal(rsGetData, "Quantity");


                    thisMatProp.StockTypeID = DB.RSFieldInt(rsGetData, "StockTypeID");
                    thisMatProp.MMPlantID = DB.RSFieldInt(rsGetData, "MMPlantID");
                    thisMatProp.POSORefNumber = DB.RSField(rsGetData, "SONumber");


                    thisMatProp.IsMfgDateReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsMfgDateReqOUT"));
                    thisMatProp.IsExpiryDateReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsExpiryDateReqOUT"));
                    thisMatProp.IsBatchOrLotReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsBatchOrLotReqOUT"));
                    thisMatProp.IsSerialReqOUT = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsSerialReqOUT"));


                    thisMatProp.SOQuantity = DB.RSFieldDecimal(rsGetData, "Quantity");
                    thisMatProp.MfgDates = DB.RSField(rsGetData, "MfgDates");
                    thisMatProp.ExpDates = DB.RSField(rsGetData, "ExpDates");
                    thisMatProp.BatchNos = DB.RSField(rsGetData, "BatchNos");
                    thisMatProp.SerialNos = DB.RSField(rsGetData, "SerialNos");
                    thisMatProp.Location = DB.RSField(rsGetData, "Location");
                    thisMatProp.LocationQty = DB.RSField(rsGetData, "LocationQty");
                    thisMatProp.QtyOnHand = DB.RSFieldDecimal(rsGetData, "QtyOnHand");
                    thisMatProp.StockType = DB.RSField(rsGetData, "StockType");

                    thisMatProp.IsActive = DB.RSFieldTinyInt(rsGetData, "IsActive");
                    thisMatProp.Deleted = DB.RSFieldTinyInt(rsGetData, "Deleted");



                }
                rsGetData.Close();

                return thisMatProp;
            }


            public static MaterialPropertyOUT GetTransferItemProperties(String MaterialCode, String Location, String WarehouseIDs)
            {
                MaterialPropertyOUT thisMatProp = new MaterialPropertyOUT();

                IDataReader rsGetData = DB.GetRS("[sp_Inv_GetTransferItem_ForHHT] @MCode=" + DB.SQuote(MaterialCode) + ",@Location=" + DB.SQuote(Location) + ",@WarehouseID=" + DB.SQuote(WarehouseIDs));

                while (rsGetData.Read())
                {

                    //thisMatProp.LineNumber = DB.RSFieldInt(rsGetData, "LineNumber");
                    thisMatProp.MCode = DB.RSField(rsGetData, "MCode");
                    thisMatProp.Description = DB.RSField(rsGetData, "MDescription");
                    //thisMatProp.GoodsMovementDetailsIDs = DB.RSField(rsGetData, "GoodsMovementDetailsIDs");

                    thisMatProp.BUoMID = DB.RSFieldInt(rsGetData, "BUoMID");
                    thisMatProp.BUoMQty = DB.RSFieldDecimal(rsGetData, "BUoMQty");

                    //thisMatProp.SalesUoMID = DB.RSFieldInt(rsGetData, "SUoMID");
                    //thisMatProp.SalesUoMQty = DB.RSFieldDecimal(rsGetData, "SUoMQty");

                    //thisMatProp.SOQuantity = DB.RSFieldDecimal(rsGetData, "Quantity");


                    //thisMatProp.StockTypeID = DB.RSFieldInt(rsGetData, "StockTypeID");
                    //thisMatProp.MMGroupID = DB.RSFieldInt(rsGetData, "MMGroupID");
                    //thisMatProp.MMPlantID = DB.RSFieldInt(rsGetData, "MMPlantID");
                    //thisMatProp.POSORefNumber = DB.RSField(rsGetData, "SONumber");


                    
                    thisMatProp.IsExpiryDateReqIN = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsExpiryDateReqIN"));
                    thisMatProp.IsBatchOrLotReqIN = Convert.ToBoolean(DB.RSFieldTinyInt(rsGetData, "IsBatchOrLotReqIN"));
                    
                 

                    thisMatProp.QtyOnHand  = DB.RSFieldDecimal(rsGetData, "QtyOnHand");
                    thisMatProp.MfgDates = DB.RSField(rsGetData, "MfgDates");
                    thisMatProp.ExpDates = DB.RSField(rsGetData, "ExpDates");
                    thisMatProp.BatchNos = DB.RSField(rsGetData, "BatchNos");
                    thisMatProp.SerialNos = DB.RSField(rsGetData, "SerialNos");
                    //thisMatProp.Location = DB.RSField(rsGetData, "Location");
                    //thisMatProp.LocationQty = DB.RSField(rsGetData, "LocationQty");
                    thisMatProp.StockType = DB.RSField(rsGetData, "MMStockTypeIDs");
                    



                }
                rsGetData.Close();

                return thisMatProp;
            }

        }

        
    }
}