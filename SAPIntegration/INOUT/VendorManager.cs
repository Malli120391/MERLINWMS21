using SAPIntegration.BusinessObjects;
using SAPIntegration.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRLWMSC21Common;

namespace SAPIntegration.INOUT
{
    public class VendorManager
    {
        private InvVendor invVendor;
        private bool isDelete;
        private bool isCreate;
        private bool isUpdate;

        public VendorManager(object obj) 
        {
            try
            {
                invVendor = (InvVendor)obj;
                isUpdate = false;
                isDelete = false;
                isCreate = true;
            }
            catch (Exception e)
            {
                throw new InvParserException(e.Message, 5);
            }
        }
        
        public bool saveOrUpdate()
        {

            bool status = false;

            try
            {

                if (invVendor == null)
                return false;

                StringBuilder SqlString = new StringBuilder();
                int CurrencyID = 0; int CountryMasterID = 0;
                int SupplierID = 0; int AddressBookID = 0;

                CurrencyID = CommonDAO.GetCurrencyID(invVendor.currency.currencyCode);
                CountryMasterID = CommonDAO.GetCountryID(invVendor.countryCode);

                SupplierID = DB.GetSqlN("select SupplierID AS N from MMT_Supplier where SupplierName=" + DB.SQuote(invVendor.organization));

                SqlString.Append("Declare @NewUpdateSupplierID int ;   EXEC  [sp_MMT_UpsertSupplier]    ");

                SqlString.Append("    @SupplierID=" + SupplierID);

                SqlString.Append(",@AddressBookID=" + (AddressBookID == 0 ? "NULL" : AddressBookID.ToString()));
                SqlString.Append(",@SupplierName='" + (invVendor.organization == null ? "NULL" : invVendor.organization) + "'");
                SqlString.Append(",@TenantID=1");
                SqlString.Append(",@SearchTerm='" + invVendor.vendorCode + "'");
                SqlString.Append(",@SupplierCode='" + invVendor.vendorCode + "'");
               
                SqlString.Append(",@Address1="+ (invVendor.contactAddress != null && invVendor.contactAddress.Count>0 ? (invVendor.contactAddress[0].adressLine1!=null && invVendor.contactAddress[0].adressLine1!="" ? DB.SQuote(invVendor.contactAddress[0].adressLine1):"NULL") : "NULL" ));
                SqlString.Append(",@Address2=" + (invVendor.contactAddress != null && invVendor.contactAddress.Count > 0 ? (invVendor.contactAddress[0].adressLine2 != null && invVendor.contactAddress[0].adressLine2 != "" ? DB.SQuote(invVendor.contactAddress[0].adressLine2) : "NULL") : "NULL") );
                SqlString.Append(",@CountryMasterID=" + (CountryMasterID == 0 ? "NULL" : CountryMasterID.ToString()) + "");
                SqlString.Append(",@Phone1=''");
                SqlString.Append(",@Phone2=''");
                SqlString.Append(",@Mobile=''");
                SqlString.Append(",@Fax=''");
                SqlString.Append(",@EmailAddress=''");
                SqlString.Append(",@PCP=''");
                SqlString.Append(",@PCPTitle=''");
                SqlString.Append(",@PCPContactNumber=''");
                SqlString.Append(",@PCPEmail=''");
                SqlString.Append(",@BankName=''");
                SqlString.Append(",@BankAddress=''");
                SqlString.Append(",@BankCountryID=NULL");
                SqlString.Append(",@AccountNo=''");
                SqlString.Append(",@SortCodeORBLZCode=''");
                SqlString.Append(",@IBANNo=''");
                SqlString.Append(",@SwiftCode=''");
                SqlString.Append(",@CurrencyID=" + (CurrencyID == 0 ? "NULL" : CurrencyID.ToString()) + "");
                SqlString.Append(",@RequestedBy=1");
                SqlString.Append(",@IsApproved=1");
                SqlString.Append(",@IsFirstEdit=1");
                SqlString.Append(",@LastEditedByID=1");
                SqlString.Append(",@SupplierCodeAprEditCount=1");
                SqlString.Append(",@CreatedBy=1");
                SqlString.Append(",@IsActive=1");
                SqlString.Append(",@NewSupplierID=@NewUpdateSupplierID OUTPUT ;     Select @NewUpdateSupplierID as N");

                if (DB.GetSqlN(SqlString.ToString()) == 0)
                {
                    // ApplicationCommon.WriteLog("Admin", "WMS Dynamics Service", "RTSMOUT", " Error while updating Supplier details   ", "RTSMOUT");

                }


                status = true;
            }
            catch (Exception ex) {

                CommonLogic.createErrorNode("SAPUSER", "VendorManager", ex.Source, ex.Message, ex.StackTrace);

                return false;
            }

            return status;
        }

        public bool deleteVendor()
        {
            return true;
        }

        public bool saveVendor()
        {
            return true;
        }
    }
}
