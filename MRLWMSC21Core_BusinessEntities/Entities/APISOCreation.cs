using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class APISOCreation
    {
        private List<SOCreationDTO> _orderReuest;
        public List<SOCreationDTO> OrderReuest
        {
            get { return _orderReuest; }
            set { _orderReuest = value; }
        }

        public class SOCreationDTO
        {
            private string _TenantCode;
            private string _WareHouseCode;
            private string _InvoiceNO;
            private string _SONumber;
            private string _SODate;
            private string _CustomerCode;
            private string _CustomerName;
            private string _CustomerPhoneNo;
            private string _CustomerEmail;
            private string _CustomerAddress;
            private string _CustomerZipcode;
            private string _AWBNo;
            private string _Courier;
            private string _Priority;
            private string _DueDate;
            private string _Notes;
            private string _OBDNumber;
            private string _Result;
            private List<SOOrderDetailDTO> _orderDetails;

            public string TenantCode
            {
                get { return _TenantCode; }
                set { _TenantCode = value; }
            }
            public string WareHouseCode
            {
                get { return _WareHouseCode; }
                set { _WareHouseCode = value; }
            }
            public string InvoiceNO
            {
                get { return _InvoiceNO; }
                set { _InvoiceNO = value; }
            }
            public string SONumber
            {
                get { return _SONumber; }
                set { _SONumber = value; }
            }
            public string SODate
            {
                get { return _SODate; }
                set { _SODate = value; }
            }
            public string CustomerName
            {
                get { return _CustomerName; }
                set { _CustomerName = value; }
            }
            public string CustomerPhoneNo
            {
                get { return _CustomerPhoneNo; }
                set { _CustomerPhoneNo = value; }
            }
            public string CustomerEmail
            {
                get { return _CustomerEmail; }
                set { _CustomerEmail = value; }
            }
            public string CustomerAddress
            {
                get { return _CustomerAddress; }
                set { _CustomerAddress = value; }
            }
            public string CustomerZipcode
            {
                get { return _CustomerZipcode; }
                set { _CustomerZipcode = value; }
            }
            public string AWBNo
            {
                get { return _AWBNo; }
                set { _AWBNo = value; }
            }
            public string Courier
            {
                get { return _Courier; }
                set { _Courier = value; }
            }
            public string Priority
            {
                get { return _Priority; }
                set { _Priority = value; }
            }
            public string DueDate
            {
                get { return _DueDate; }
                set { _DueDate = value; }
            }
            public string Notes
            {
                get { return _Notes; }
                set { _Notes = value; }
            }
            public string OBDNumber
            {
                get { return _OBDNumber; }
                set { _OBDNumber = value; }
            }
            public string Result
            {
                get { return _Result; }
                set { _Result = value; }
            }
            public List<SOOrderDetailDTO> OrderDetails
            {
                get { return _orderDetails; }
                set { _orderDetails = value; }
            }

            public string CustomerCode
            {
                get { return _CustomerCode; }
                set { _CustomerCode = value; }
            }

            public class SOOrderDetailDTO
            {
                private string _SONumber;
                private string _SOQty;
                private string _MaterialCode;
                private string _UoM;
                private string _UoMQty;
                private string _MfgDate;
                private string _ExpDate;
                private string _BatchNo;
                private string _ProjectRefNo;
                private string _SerialNo;
                private string _MRP;


                public string Quantity
                {
                    get { return _SOQty; }
                    set { _SOQty = value; }
                }

                public string Mcode
                {
                    get { return _MaterialCode; }
                    set { _MaterialCode = value; }
                }

                public string UoM
                {
                    get { return _UoM; }
                    set { _UoM = value; }
                }
                public string UoMQty
                {
                    get { return _UoMQty; }
                    set { _UoMQty = value; }
                }
                public string MFGDate
                {
                    get { return _MfgDate; }
                    set { _MfgDate = value; }
                }

                public string EXPDate
                {
                    get { return _ExpDate; }
                    set { _ExpDate = value; }

                }

                public string BatchNo
                {
                    get { return _BatchNo; }
                    set { _BatchNo = value; }
                }

                public string ProjRefNo
                {
                    get { return _ProjectRefNo; }
                    set { _ProjectRefNo = value; }
                }

                public string SerailNo
                {
                    get { return _SerialNo; }
                    set { _SerialNo = value; }
                }

                public string MRP
                {
                    get { return _MRP; }
                    set { _MRP = value; }
                }

                public string SONumber
                {
                    get { return _SONumber; }
                    set { _SONumber = value; }
                }
            }


        }
    }
}
