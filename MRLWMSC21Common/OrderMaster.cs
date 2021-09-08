using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common
{
    [Serializable]
    public sealed class OrderMaster
    {

        public OrderMaster() { }


        #region  ----------  Purchage Order -------------------

        [Serializable]
        public class POHeader
        {
            public String PONumber { set; get; }
            public String Instructions { set; get; }
            public String Remarks { set; get; }
            public String POType { set; get; }
            public String OrderedCompany { set; get; }
            public String Currency { set; get; }
            public String Supplier { set; get; }
            public String OrderedBy { set; get; }
            public String POStatus { set; get; }


            public int POStatusID { get; set; }
            public int POTypeID { get; set; }
            public int SupplierID { get; set; }
            public int DepartmentID { get; set; }
            public int DivisionID { get; set; }
            public int RequestedBy { get; set; }

            public int CurrencyID { get; set; }
            public int CreatedBy { get; set; }
            public int TenantID { get; set; }

            public decimal TotalValue { get; set; }
            public decimal ExchangeRate { get; set; }
            public decimal POTax { get; set; }

            public DateTime PODate { get; set; }
            public DateTime DateRequested { get; set; }
            public DateTime DateDue { get; set; }

        }

        [Serializable]
        public class PODetail
        {
            public String RequirementNumber { set; get; }
            public String MCode { set; get; }

            public int POHeaderID { get; set; }
            public int LineNumber { get; set; }
            public int MaterialMasterID { get; set; }
            public int KitPlannerID { get; set; }
            public int IsKitParent { get; set; }
            public int MaterialMaster_PUoMID { get; set; }
            public int OldLineNo { get; set; }
            public int CreatedBy { get; set; }

            public decimal POQuantity { get; set; }

        }

        [Serializable]
        public class SupplierInvoiceHeader
        {
            public String InvoiceNumber { set; get; }
            public String InvVATCode { set; get; }

            public int POHeaderID { get; set; }
            public int SupplierID { get; set; }
            public int CurrencyID { get; set; }

            public int NoofPackages { get; set; }
            public int InvCountryofOriginID { get; set; }
            public int CreatedBy { get; set; }

            public decimal ExchangeRate { get; set; }
            public decimal InvoiceValue { get; set; }
            public decimal NetWeight { get; set; }
            public decimal GrossWeight { get; set; }

            public DateTime InvoiceDate { get; set; }

        }

        [Serializable]
        public class SupplierInvoiceDetail
        {
            public int PODetailsID { get; set; }
            public int SupplierInvoiceID { get; set; }
            public int MaterialMaster_InvUoMID { get; set; }
            public int InvCountryofOriginID { get; set; }
            public int CreatedBy { get; set; }

            public decimal InvoiceQuantity { get; set; }
            public decimal InvDiscountInPercentage { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Tax { get; set; }

        }


        #endregion  ----------  Purchage Order -------------------


        [Serializable]
        public class MaterialStorageParameter
        {
            public int DetailID { get; set; }
            public int MaterialStorageParameterID { get; set; }

            public String Value { set; get; }
        }


        #region  ----------  Work Order -------------------

        [Serializable]
        public class WOHeader
        {
            public String WONumber { set; get; }
            public String ProjectCode { set; get; }
            public String RequirementNumber { set; get; }
            public String ShipmentAddress1 { set; get; }
            public String ShipmentAddress2 { set; get; }
            public String City { set; get; }
            public String Province { set; get; }
            public String Zip { set; get; }
            public String Mobile { set; get; }
            public String SOType { set; get; }
            public String Currency { set; get; }
            public String Customer { set; get; }


            public int CustomerID { get; set; }
            public int SOStatusID { get; set; }
            public int SOTypeID { get; set; }
            public int DepartmentID { get; set; }
            public int DivisionID { get; set; }
            public int UserID { get; set; }
            public int RequestedBy { get; set; }
            public int CurrencyID { get; set; }
            public int CreatedBy { get; set; }
            public int TenantID { get; set; }
            public int SupplierInvoiceID { get; set; }
            public int FreightCompanyID { get; set; }
            public int CountryMasterID { get; set; }
            
            public decimal SOTax { get; set; }
            public decimal NetValue { get; set; }
            public decimal GrossValue { get; set; }

            public DateTime SODate { get; set; }

        }
        
        [Serializable]
        public class WODetail
        {
            public String VATCode { set; get; }

            public String MCode { set; get; }
            public String UOM { set; get; }
            public String CustomerPO { set; get; }

            public int SOHeaderID { get; set; }
            public int LineNumber { get; set; }
            public int MaterialMasterID { get; set; }
            public int KitPlannerID { get; set; }
            public int IsKitParent { get; set; }
            public int MaterialMaster_SUoMID { get; set; }
            public int IsNonConfirmity { get; set; }
            public int HasDiscrepancy { get; set; }
            public int CountryofOriginID { get; set; }
            public int CustomerPOID { get; set; }
            public int MaterialMaster_CustPOUoMID { get; set; }
            public int OldLineNo { get; set; }
            public int CreatedBy { get; set; }

            public decimal SOQuantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal CustPOQuantity { get; set; }
            public decimal SODiscountInPercentage { get; set; }
            
        }

        [Serializable]
        public class CustomerPO
        {
            public String CustPONumber { set; get; }
            
            public int SOHeaderID { get; set; }
            public int CurrencyID { get; set; }

            public int CreatedBy { get; set; }

            public decimal ExchangeRate { get; set; }
            public decimal CustPOValue { get; set; }
        
            public DateTime CustPODate { get; set; }

        }

        #endregion  ----------  Work Order -------------------






    }

   







    





}
