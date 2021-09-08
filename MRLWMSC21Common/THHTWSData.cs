using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace MRLWMSC21Common
{
   public sealed class THHTWSData
    {
       public THHTWSData() { }

       [Serializable]
       public class KeyValueStruct
       {
           private string _key;
           private string _val;


           public string Key
           {
               get { return _key; }
               set { _key = value; }
           }
           public string Value
           {
               get { return _val; }
               set { _val = value; }
           }

           //Constructor
           private KeyValueStruct() { }

           public KeyValueStruct(string key, string value)
           {
               _key = key;
               _val = value;
           }
       }
       [Serializable]
       public class KeyValueData
       {
           private string _key;
           private object _val;

           public object Val
           {
               get { return _val; }
               set { _val = value; }
           }


           public string Key
           {
               get { return _key; }
               set { _key = value; }
           }


           //Constructor
           private KeyValueData() { }

           public KeyValueData(string key, object value)
           {
               _key = key;
               _val = value;
           }
       }
       [Serializable]
       public class KeyValueData123
       {
           private string _key;
           private string _val;

            public string Val
            {
              get { return _val; }
              set { _val = value; }
            }

          


           public string Key
           {
               get { return _key; }
               set { _key = value; }
           }


           //Constructor
           private KeyValueData123() { }

           public KeyValueData123(string key, string value)
           {
               _key = key;
               Val = value;
           }
       }
       
       [Serializable]
       public class MaterialReceivedStuct
       {
           private int _GoodsMovementDetailsID;
           private int _LineNumber;
           private string _Location;
           private DateTime _MfgDate;
           private DateTime _ExpDate;
           private string _BatchNo;
           private string _SerialNo;
           //private Boolean _IsDamaged;
           private string _StockType;
           private Decimal _Quantity;


           public int GoodsMovementDetailsID
           {
               get { return _GoodsMovementDetailsID; }
               set { _GoodsMovementDetailsID = value; }
           }

           public int LineNumber
           {
               get { return _LineNumber; }
               set { _LineNumber = value; }
           }

           public string Location
           {
               get { return _Location; }
               set { _Location = value; }
           }
            public DateTime MfgDate //2
           {
               get { return _MfgDate; }
               set { _MfgDate = value; }
           }
            public DateTime ExpDate //3
           {
               get { return _ExpDate; }
               set { _ExpDate = value; }
           }
            public string BatchNo //4
           {
               get { return _BatchNo; }
               set { _BatchNo = value; }
           }

            public string SerialNo //5
            {
                get { return _SerialNo; }
                set { _SerialNo = value; }
            }

           // public Boolean IsDamaged
           //{
           //    get { return _IsDamaged; }
           //    set { _IsDamaged = value; }
           //}

            public string StockType //5
            {
                get { return _StockType; }
                set { _StockType = value; }
            }
           

            public Decimal Quantity
           {
               get { return _Quantity; }
               set { _Quantity = value; }
           }


            //Constructor
            private MaterialReceivedStuct()  {}


            public MaterialReceivedStuct(int GoodsMovementDetailsID, int LineNumber, string Location, DateTime MfgDate, DateTime ExpDate, string BatchNo, string SerialNo, String StockType, Decimal Quantity)
           {
               _GoodsMovementDetailsID = GoodsMovementDetailsID;
                _LineNumber = LineNumber;
               _Location = Location;
               _MfgDate = MfgDate;
               _ExpDate = ExpDate;
               _BatchNo = BatchNo;
               _SerialNo = SerialNo;
               _StockType = StockType;
               _Quantity = Quantity;

           }
       }



       [Serializable]
       public class StockOnHandStruct
       {
           private decimal _QtyOnHand;
           private string _BatchNo;
           private DateTime _ExpDate;
           private int  _StockTypeID;
           private int _PlantID;
           private int _KitPlannerID;
           private DateTime _MfgDate;
           private string _StockType;
           private string _SerialNo;






           public Decimal QtyOnHand //1
           {
               get { return _QtyOnHand; }
               set { _QtyOnHand = value; }
           }

           public string BatchNo //2
           {
               get { return _BatchNo; }
               set { _BatchNo = value; }
           }

           public DateTime ExpDate //3
           {
               get { return _ExpDate; }
               set { _ExpDate = value; }
           }

           public int KitPlannerID //4
           {
               get { return _KitPlannerID; }
               set { _KitPlannerID = value; }
           }


           public string StockType //5
           {
               get { return _StockType ; }
               set { _StockType = value; }
           }

           public string SerialNo //6
           {
               get { return _SerialNo; }
               set { _SerialNo = value; }
           }


           public DateTime MfgDate //7
           {
               get { return _MfgDate; }
               set { _MfgDate = value; }
           }

           public int StockTypeID //8
           {
               get { return _StockTypeID; }
               set { _StockTypeID = value; }
           }

           public int PlantID //9
           {
               get { return _PlantID; }
               set { _PlantID = value; }
           }
          



           //Constructor
           private StockOnHandStruct() { }


           public StockOnHandStruct(Decimal QtyOnHand, string BatchNo, string SerialNo, DateTime ExpDate, int StockTypeID, DateTime MfgDate, String StockType ,int PlantID)
           {
               _QtyOnHand = QtyOnHand;
               _BatchNo = BatchNo;
               _SerialNo = SerialNo;
               _ExpDate = ExpDate;
               _StockTypeID = StockTypeID;
               _MfgDate = MfgDate ;
               _StockType = StockType;
               _PlantID = PlantID;

           }
       }


       [Serializable]
       public class LoginUserData
       {
           
           private int _TenantID;
           private string _TenantName;

          
           private Guid _UserGUID;
           private int _UserID;
           private string _FirstName;
           private string _LastName;
           private string _Email;
           private string _Password;
           private string _roles;
           private string _WarehouseIDs;

           private string _Zones;
           private string _DepartmentIDs;

           private string _MachineIPAddress;
            private int _AccountID;
            private int _UserTypeID;
            private string _Account;

            public int TenantID
           {
               get { return _TenantID; }
               set { _TenantID = value; }
           }
            public int AccountID
            {
                get { return _AccountID; }
                set { _AccountID = value; }
            }

            public string Account
            {
                get { return _Account; }
                set { _Account = value; }
            }

            public int UserTypeID
            {
                get { return _UserTypeID; }
                set { _UserTypeID = value; }
            }
            public string TenantName
           {
               get { return _TenantName; }
               set { _TenantName = value; }
           }

             public Guid UserGUID
           {
               get { return _UserGUID; }
               set { _UserGUID = value; }
           }

             public int UserID
             {
                 get { return _UserID; }
                 set { _UserID = value; }
             }

           public string FirstName
           {
               get { return _FirstName; }
               set { _FirstName = value; }
           }

           public string LastName
           {
               get { return _LastName; }
               set { _LastName = value; }
           }

           public string Email
           {
               get { return _Email; }
               set { _Email = value; }
           }
           public string Password
           {
               get { return _Password; }
               set { _Password = value; }
           }

           public string Roles
           {
               get { return _roles; }
               set { _roles = value; }
           }

           public string Warehouses
           {
               get { return _WarehouseIDs; }
               set { _WarehouseIDs = value; }
           }

           public string SiteCodes
           {
               get { return _Zones; }
               set { _Zones = value; }
           }

           public string DepartmentIDs
           {
               get { return _DepartmentIDs; }
               set { _DepartmentIDs = value; }
           }

           public string MachineIPAddress
           {
               get { return _MachineIPAddress; }
               set { _MachineIPAddress = value; }
           }



           public LoginUserData() { }


           public LoginUserData(Guid UserGUID, int UserID, int TenantID, string TenantName, String FirstName, string LastName, string Email, string Roles, string WarehouseIDs,
               string Zones, string DepartmentIDs, string MachineIPAddress, int AccountID = 0, int UserTypeID = 0,string Account="")
           {
               _UserID = UserID;
               _UserGUID = UserGUID;
               _TenantID = TenantID;
               _TenantName = TenantName;
               _FirstName = FirstName;
               _LastName = LastName;
               _Email = Email;

               _roles = Roles.Trim();
               _WarehouseIDs = WarehouseIDs.Trim();
               _Zones = Zones.Trim();
               _DepartmentIDs = DepartmentIDs.Trim();
               _MachineIPAddress = MachineIPAddress; _AccountID = AccountID;
                _UserTypeID = UserTypeID;
                _Account = Account;
            }


           public Boolean  Login(String Email, String Password, String LoginIPAddress)
           {
               LoginUserData resUserDetails = new LoginUserData();
               Boolean Result = false;
               
               IDataReader rsUDetails = DB.GetRS(String.Format("Select TenantID,UserGUID,UserID FirstName,LastName,UserTypeID,WareHouseId,SiteCodes,DepartmentIDs from Users Where Email={0} AND Password={1}", DB.SQuote(Email), DB.SQuote(Password)));

               while (rsUDetails.Read())
               {
                   _UserID = DB.RSFieldInt(rsUDetails, "UserID");
                   _UserGUID = DB.RSFieldGUID(rsUDetails, "UserGUID") ;
                   _FirstName = DB.RSField(rsUDetails, "FirstName");
                   _LastName= DB.RSField(rsUDetails, "LastName");
                   _Email = Email;
                   _Password = Password;
                   _roles = DB.RSField(rsUDetails, "UserTypeID");
                   _WarehouseIDs = DB.RSField(rsUDetails, "WareHouseId");
                   _Zones= DB.RSField(rsUDetails, "SiteCodes");
                   _DepartmentIDs= DB.RSField(rsUDetails, "DepartmentIDs");
                   _MachineIPAddress = LoginIPAddress;
                   Result = true;
               }
               rsUDetails.Close();
               return Result;
           }

       }


    }
}
