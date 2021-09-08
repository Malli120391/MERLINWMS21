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
    public class CustomerManager
    {
        private InvCustomer customer;
        private bool isUpdate;
        private bool isDelete;
        private bool isCreate;

        public CustomerManager(object obj) 
        {
            try {
                customer = (InvCustomer)obj;
                isUpdate = false;
                isDelete = false;
                isCreate = true;
            }catch(Exception e){
                throw new InvParserException(e.Message,5);
            }
        }

        public bool saveOrUpdate() 
        {
            bool status = false;

            try
            {

                StringBuilder SqlString = new StringBuilder();
                int CurrencyID = 0; int CountryMasterID = 0;
                int CustomerID = 0; int AddressBookID = 0;

                InvCurrency invCurrecy = customer.currency;

                CurrencyID = CommonDAO.GetCurrencyID( ( invCurrecy!=null ? invCurrecy.currencyCode : ""));
                CountryMasterID = CommonDAO.GetCountryID( ( ( customer.countryCode!= null && customer.countryCode!="" )  ? customer.countryCode:""));


                SqlString.Append("Declare @NewUpdateCustomerID int ;  ");
                SqlString.Append("EXEC [dbo].[sp_GEN_UpsertCustomer]    ");

                CustomerID = DB.GetSqlN("select CustomerID AS N from GEN_Customer where CustomerName=" + DB.SQuote(customer.DBAName + " ( " + customer.custCode +" )"));



                SqlString.Append("   @CustomerID=" + CustomerID);
                SqlString.Append(",@AddressBookID=" + (AddressBookID == 0 ? "NULL" : AddressBookID.ToString()));
                SqlString.Append(",@CustomerName=" + ((customer.DBAName != null && customer.DBAName != "") ? DB.SQuote(customer.DBAName + " ( " + customer.custCode + " )") : "NULL"));
                SqlString.Append(",@TenantID=1");
                SqlString.Append(",@Address1=" + ((customer.postalAddress != null && customer.postalAddress.Count > 0) ? DB.SQuote(customer.postalAddress[0].adressLine1) : "NULL"));
                SqlString.Append(",@Address2=''");
                SqlString.Append(",@City=''");
                SqlString.Append(",@State=''");
                SqlString.Append(",@Zip=''");
                SqlString.Append(",@CountryMasterID=" + (CountryMasterID == 0 ? "NULL" : CountryMasterID.ToString()));
                SqlString.Append(",@Phone1=''");
                SqlString.Append(",@Phone2=''");
                SqlString.Append(",@Mobile=''");
                SqlString.Append(",@Fax=''");
                SqlString.Append(",@EmailAddress=''");
                SqlString.Append(",@WebSite=''");
                SqlString.Append(",@PCP=''");
                SqlString.Append(",@PCPTitle=''");
                SqlString.Append(",@PCPContactNumber=''");
                SqlString.Append(",@PCPEmail=''");
                SqlString.Append(",@BankName=''");
                SqlString.Append(",@BankAddress=''");
                SqlString.Append(",@BankCountryID=" + (CountryMasterID == 0 ? "NULL" : CountryMasterID.ToString()));
                SqlString.Append(",@AccountNo=''");
                SqlString.Append(",@IBANNo=''");
                SqlString.Append(",@CurrencyID=" + (CurrencyID == 0 ? "NULL" : CurrencyID.ToString()));
                SqlString.Append(",@IsHidden=0");
                SqlString.Append(",@CreatedBy=1");
                SqlString.Append(",@IsActive=1");
                SqlString.Append(",@IsDeleted=0");

                SqlString.Append(",@NewCustomerID= @NewUpdateCustomerID OUTPUT ;  ");

                SqlString.Append("Select @NewUpdateCustomerID as N  ");

                if (DB.GetSqlN(SqlString.ToString()) == 0)
                {
                    //ApplicationCommon.WriteLog("Admin", "WMS JDE Service", "RTCMOUT", " Error while updating customer details   ", "RTCMOUT");
                }


                status = true;
            }
            catch (Exception ex) {

                CommonLogic.createErrorNode("SAPUSER", "Customer Manager", ex.Source, ex.Message, ex.StackTrace);
            }

            return status;
        }

        public bool saveCustomer() 
        {
            return false;
        }

        public bool deleteCustomer() 
        {
            return false;
        }
    }

}
