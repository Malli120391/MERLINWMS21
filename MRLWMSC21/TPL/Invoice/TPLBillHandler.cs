using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Data;
using MRLWMSC21.TPL.ActivityRate;
using System.Net;
using System.Net.Mail;

namespace MRLWMSC21.TPL.Invoice
{
    public class TPLBillHandler
    {

        public IList<string> ErrorList;
        ActivityRateHandler rateHandler;
        IDictionary<int, IDictionary<int, StorageCharge>> InHouseItemsWithCharges;
        public IList<CalculatedStorageCharge> actualStorageCharges;
        public bool functionalFlow;
        public IDictionary<int, InvoiceRate> InvoiceRates;


        public TPLBillHandler()
        {
            rateHandler = new ActivityRateHandler();
            ErrorList = new List<string>();
            this.functionalFlow = true;
            actualStorageCharges = null;
            InHouseItemsWithCharges = null;
            InvoiceRates = null;
            InvoiceRates = new Dictionary<int, InvoiceRate>();
        }
        
        public IDictionary<int, TaxationInfo> getTaxationInfo(String stateCode, DateTime fromDate, DateTime toDate)
        {

            string _fromDate = CommonFunctions.dateTimetoString(fromDate, "yyyy-MM-dd");
            string _toDate = CommonFunctions.dateTimetoString(toDate, "yyyy-MM-dd");
            IDictionary<int, TaxationInfo> taxInfo = new Dictionary<int, TaxationInfo>();

            if (_fromDate != "" && _toDate != "")
            {

                string sqlStatement = "select T.TaxationID,AT.ActivityRateTypeID,TaxCode,Percentage,StateCode from TPL_Taxation T JOIN TPL_ActivityTaxation AT on AT.TaxationID = T.TaxationID where T.IsActive = 1 and T.IsDeleted =0 and AT.IsActive=1 and AT.IsDeleted=0 and T.FromDate <= '" + _fromDate + "' and T.ToDate >= '" + _toDate + "'";
                DataSet TaxationInfoSet = DB.GetDS(sqlStatement, false);

                if (TaxationInfoSet != null)
                {
                    DataTable table = TaxationInfoSet.Tables[0];
                    foreach (DataRow row in table.Rows)
                    {
                        TaxationInfo temp = new TaxationInfo();
                        temp.stateCode = row["StateCode"].ToString();
                        temp.taxationID = Convert.ToInt16(row["TaxationID"].ToString());
                        temp.taxCode = row["TaxCode"].ToString();
                        temp.percentage = Convert.ToDecimal(row["Percentage"].ToString());
                        int activityID = Convert.ToInt16(row["ActivityRateTypeID"].ToString());

                        if (!taxInfo.ContainsKey(activityID))
                        {
                            taxInfo.Add(activityID, temp);
                        }
                        else
                        {
                            taxInfo[activityID] = temp;
                        }
                    }
                }
            }

            return taxInfo;

        }

        public IDictionary<int, RateGroup> getActiveRates(int tenantID, DateTime fromDate, DateTime toDate)
        {

            string _fromDate = CommonFunctions.dateTimetoString(fromDate, "yyyy-MM-dd");
            string _toDate = CommonFunctions.dateTimetoString(toDate, "yyyy-MM-dd");

            if (_fromDate != "" && _toDate != "")
            {

                string sqlStatement = "[dbo].[sp_TPL_GetActiveTenantRates] @TenantID=" + tenantID + ",@fromDate='" + _fromDate + "',@toDate='" + _toDate + "'";
                DataSet activeRateSet = DB.GetDS(sqlStatement, false);

                if (activeRateSet != null)
                {
                    foreach (DataTable table in activeRateSet.Tables)
                    {
                        return rateHandler.filterAvailableRates(table);
                    }
                }
            }

            return null;
        }

        public IList<RateInfo> getActiveRateListNew(int tenantID, DateTime fromDate, DateTime toDate)
        {
            string _fromDate = CommonFunctions.dateTimetoString(fromDate, "yyyy-MM-dd");
            string _toDate = CommonFunctions.dateTimetoString(toDate, "yyyy-MM-dd");

            if (_fromDate != "" && _toDate != "")
            {

                string sqlStatement = "[dbo].[sp_TPL_GetActiveTenantRates] @TenantID=" + tenantID + ",@fromDate='" + _fromDate + "',@toDate='" + _toDate + "'";
                DataSet activeRateSet = DB.GetDS(sqlStatement, false);

                if (activeRateSet != null)
                {
                    foreach (DataTable table in activeRateSet.Tables)
                    {
                        return rateHandler.getActiveRateList(table);
                    }
                }
            }

            return null;
        }

        public IDictionary<string, TransactionCharge> getListOfTransactions(int tenantID, DateTime fromDate, DateTime toDate)
        {

            string _fromDate = CommonFunctions.dateTimetoString(fromDate, "yyyy-MM-dd");
            string _toDate = CommonFunctions.dateTimetoString(toDate, "yyyy-MM-dd");

            if (_fromDate != "" && _toDate != "")
            {

                string sqlStatement = "[dbo].[sp_TPL_GetTenantAccessorialChargeDetails] @TenantID=" + tenantID + ",@fromDate='" + _fromDate + "',@toDate='" + _toDate + "'";
                DataSet activeRateSet = DB.GetDS(sqlStatement, false);

                if (activeRateSet != null)
                {
                    foreach (DataTable table in activeRateSet.Tables)
                    {
                        return rateHandler.getTransactionalCharges(table);
                    }
                }
            }

            return null;
        }

        public TenantInfo getTenantDetails(int TenantID)
        {

            try
            {
                string sqlStatement = "EXEC [dbo].[sp_TPL_GetTenantPrimaryInfo] @TenantID = " + TenantID;
                DataSet tenantDataSet = DB.GetDS(sqlStatement, false);

                if (tenantDataSet != null)
                {

                    foreach (DataTable table in tenantDataSet.Tables)
                    {

                        TenantInfo tenantInformation = new TenantInfo();
                        TenantAddress tenantAddress = new TenantAddress();
                        BankData bankData = new BankData();

                        foreach (DataRow row in table.Rows)
                        {

                            int _tenantID = Convert.ToInt16(row["TenantID"].ToString());
                            string _tenantName = row["TenantName"].ToString();
                            string _companyName = row["CompanyDBA"].ToString();
                            string _pemail = row["PCPEmail"].ToString();

                            int CurrencyID = Convert.ToInt16(row["CurrencyID"].ToString());
                            string CurrencyCode = (row["Code"].ToString());
                           // string accountNo = row["AccountNo"].ToString();
                            //string BankAddress = row["BankAddress"].ToString();
                           // string City = row["City"].ToString();
                            //string BankName = row["BankName"].ToString();
                            string zip = row["ZIP"].ToString();
                            string Address1 = row["Address1"].ToString();
                            string Address2 = row["Address2"].ToString();
                            string Fax = row["Fax"].ToString();
                            string EMail = row["EMail"].ToString();
                            string state = row["State"].ToString();
                            string countryName = row["CountryName"].ToString();
                            string CountryCode = row["CountryCode"].ToString();
                            int previousInvoiceCount = Convert.ToInt16(row["PreviousInvoice"].ToString());
                            int billTypeID = Convert.ToInt16(row["BillTypeID"].ToString());

                            //bankData.accountNo = accountNo;
                            bankData.addressLine1 = Address1;
                            bankData.addressLine2 = Address2;
                            //bankData.bankName = BankName;
                            //bankData.city = City;
                            bankData.holderName = _companyName;

                            tenantAddress.addressLine1 = Address1;
                            tenantAddress.addressLine2 = Address2;
                            tenantAddress.addressTypeID = 2;
                            //tenantAddress.city = City;
                            tenantAddress.countryName = countryName;
                            tenantAddress.email = _pemail;
                            tenantAddress.zip = zip;

                            tenantInformation.TenantID = _tenantID;
                            tenantInformation.address = tenantAddress;
                            tenantInformation.bankData = bankData;
                            tenantInformation.billTypeID = billTypeID;
                            tenantInformation.currencyCode = CurrencyCode;
                            tenantInformation.currencyID = CurrencyID;
                            tenantInformation.previousInvoice = previousInvoiceCount;

                            tenantInformation.InvoiceID = "INV" + Convert.ToString(previousInvoiceCount + 1).PadLeft(4, '0');
                            tenantInformation.primaryEmail = EMail;
                            tenantInformation.tenantName = _tenantName;
                            tenantInformation.companyName = _companyName;

                        }
                        return tenantInformation;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new INVTPLException("Exception while getting tenant primary information: " + ex.Message);
            }

            return null;
        }

        public IDictionary<int, IDictionary<int, StorageCharge>> getStoredItemsCharges(int tenantID, DateTime fromDate, DateTime toDate)
        {


            string _fromDate = CommonFunctions.dateTimetoString(fromDate, "yyyy-MM-dd");
            string _toDate = CommonFunctions.dateTimetoString(toDate, "yyyy-MM-dd");


            if (_fromDate != "" && _toDate != "")
            {
                string sqlStatement = "[dbo].[sp_TPL_GetStoredItemsDetails] @TenantID=" + tenantID + ",@StartDate='" + _fromDate + "',@EndDate='" + _toDate + "'";
                DataSet storageItemsset = DB.GetDS(sqlStatement, false);
                if (storageItemsset != null)
                {
                    foreach (DataTable table in storageItemsset.Tables)
                    {
                        return getStoredItems(table);
                    }
                }
            }
            return null;
        }

        public IDictionary<int, IDictionary<int, StorageCharge>> getStoredItems(DataTable table)
        {

            if (table != null)
            {
                try
                {

                    IDictionary<int, IDictionary<int, StorageCharge>> storedItems = new Dictionary<int, IDictionary<int, StorageCharge>>();

                    foreach (DataRow row in table.Rows)
                    {

                        string MaterialAvailDate = row["Mdate"].ToString();
                        string MaterialCode = row["MCode"].ToString();
                        decimal capacityPerBin = Convert.ToDecimal(row["CapacityPerBin"].ToString());
                        decimal availQuantity = Convert.ToDecimal(row["AvailableQty"].ToString());
                        string MaterialDesc = row["MDescription"].ToString();
                        string UOM = row["UoM"].ToString();
                        int UomID = Convert.ToInt16(row["UoMID"].ToString());
                        int MMID = Convert.ToInt16(row["MaterialMasterID"].ToString());

                        int StorageType = Convert.ToInt16(row["StorageType"].ToString());
                        int spaceUtilization = Convert.ToInt16(row["SpaceUtilizationID"].ToString());

                        decimal Weight = Convert.ToDecimal(row["MWeight"].ToString());
                        decimal Area = Convert.ToDecimal(row["MArea"].ToString());
                        decimal Volume = Convert.ToDecimal(row["MVolume"].ToString());


                        StorageCharge sc = null;
                        MaterialStorageCharge MSC = new MaterialStorageCharge();
                        MSC.quantity = availQuantity;
                        MSC.materialAvailDate = MaterialAvailDate;

                        if (storedItems.ContainsKey(spaceUtilization))
                        {
                            IDictionary<int, StorageCharge> storeSpecItem = storedItems[spaceUtilization];
                            if (!storeSpecItem.ContainsKey(MMID))
                            {
                                sc = new StorageCharge();
                                sc.materialID = MMID;
                                sc.materialMasterCode = MaterialCode;
                                sc.materialName = MaterialDesc;
                                sc.UOM = UOM;
                                sc.UomID = UomID;

                                sc.area = Area;
                                sc.weight = Weight;
                                sc.storageType = StorageType;
                                sc.spaceUtilizationID = spaceUtilization;
                                sc.volume = Volume;

                                sc.materialStorageCharges = new List<MaterialStorageCharge>();
                                sc.perBinQuantity = capacityPerBin;
                            }
                            else
                            {
                                sc = storeSpecItem[MMID];
                            }

                            sc.materialStorageCharges.Add(MSC);
                            if (storeSpecItem.ContainsKey(MMID))
                            {
                                storeSpecItem[MMID] = sc;
                            }
                            else
                            {
                                storeSpecItem.Add(MMID, sc);
                            }
                            storedItems[spaceUtilization] = storeSpecItem;
                        }
                        else
                        {
                            IDictionary<int, StorageCharge> storeSpecItem = new Dictionary<int, StorageCharge>();
                            sc = new StorageCharge();
                            sc.materialID = MMID;
                            sc.materialMasterCode = MaterialCode;
                            sc.materialName = MaterialDesc;

                            sc.area = Area;
                            sc.weight = Weight;
                            sc.spaceUtilizationID = spaceUtilization;
                            sc.volume = Volume;

                            sc.materialStorageCharges = new List<MaterialStorageCharge>();

                            sc.UOM = UOM;
                            sc.UomID = UomID;
                            sc.perBinQuantity = capacityPerBin;

                            sc.materialStorageCharges.Add(MSC);
                            storeSpecItem.Add(MMID, sc);
                            storedItems.Add(spaceUtilization, storeSpecItem);
                        }


                    }
                    return storedItems;
                }
                catch (Exception e)
                {
                    throw new INVTPLException("exception while fitering store data: " + e.StackTrace);
                }
            }
            return null;
        }

        public IDictionary<int, RateInfo> getDefaultRateInformation(DateTime fromDate, DateTime todate)
        {
            try
            {


                string sqlStatement = "[dbo].sp_TPL_GetDefaultRateInformation  @EndDate='" + todate.ToString("yyyy-MM-dd") + "',@BeginDate= '" + fromDate.ToString("yyyy-MM-dd") + "'";
                DataSet defaultRates = DB.GetDS(sqlStatement, false);

                if (defaultRates != null)
                {
                    foreach (DataTable table in defaultRates.Tables)
                    {
                        return rateHandler.getDefaultRates(table);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new INVTPLException("exception while getting default rates:" + ex.Message);
            }

            return null;
        }

        public IDictionary<int, IDictionary<int, StorageCharge>> applyChargesOnInhouseItems(IList<RateInfo> rates, IDictionary<int, IDictionary<int, StorageCharge>> storedItems)
        {

            try
            {

                if (rates != null && storedItems != null)
                {

                    this.InHouseItemsWithCharges = new Dictionary<int, IDictionary<int, StorageCharge>>();
                    this.actualStorageCharges = new List<CalculatedStorageCharge>();

                    //STORAGE CONDITIONS
                    //1-bin based storage charge.
                    //2-conditional bin
                    //3-Hazardious goods
                    //4-other

                    //STORAGE RATE ACTIVITY TYPES
                    //8-General Warehouse material occupied area 
                    //9-Warehouse Conditional bin
                    //10-General Warehouse - Constant bin base
                    //14-General Warehouse material occupied volume
                    //15-General Warehouse material weight 
                    //16-General Warehouse rental service

                    //SPACE UTILIZATION
                    //1	Material area base
                    //2	Material volume
                    //3	Material weight 
                    //4	Constant bin base
                    //5	Space rental 


                    int _ItemStorageTypeID = 4;
                    IDictionary<int, int> activityStorageTypeDic = new Dictionary<int, int>()
                    {
                        {10,4},{9,4},{8,1},{15,3},{16,5},{14,2}
                    };

                    IDictionary<int, string> storageChargesMsg = new Dictionary<int, string>()
                    {
                        {1,"Material Area"},{2,"Material Volume"},{3,"Material Weight"},{4,"Constant Bin"},{5,"Material Rental"}
                    };



                    if (storedItems != null)
                    {

                        for (int ind = 0; ind < storedItems.Count; ind++)
                        {

                            IDictionary<int, StorageCharge> spaceWiseItems = null;
                            var TempItem = storedItems.ElementAt(ind);
                            spaceWiseItems = TempItem.Value;
                            RateInfo _temp = null;
                            int spaceUtilization = TempItem.Key;

                            foreach (RateInfo rate in rates)
                            {
                                int RateTypeID = rate.RateTypeID;
                                if (rate.GroupID != 2)
                                {
                                    continue;
                                }

                                if (activityStorageTypeDic.ContainsKey(RateTypeID))
                                {
                                    _ItemStorageTypeID = activityStorageTypeDic[RateTypeID];
                                    if (_ItemStorageTypeID == spaceUtilization)
                                    {
                                        _temp = rate;
                                        break;
                                    }
                                }
                            }

                            if (_temp != null)
                            {
                                IDictionary<int, StorageCharge> afterRateApply = null;
                                switch (spaceUtilization)
                                {
                                    case 1:
                                        afterRateApply = getAreaBasedChargeCalculation(_temp, spaceWiseItems);
                                        break;
                                    case 2:
                                        afterRateApply = getMaterialVolumeBasedCalculation(_temp, spaceWiseItems);
                                        break;
                                    case 3:
                                        afterRateApply = getMaterialWeightBasedCalculation(_temp, spaceWiseItems);
                                        break;
                                    case 4:
                                        afterRateApply = applyBinChargeOnStoredItems(_temp, spaceWiseItems);
                                        break;
                                    case 5:
                                        afterRateApply = getItemWiseRentalCharges(_temp, spaceWiseItems);
                                        break;
                                    default:
                                        afterRateApply = null;
                                        break;
                                }

                                if (afterRateApply != null)
                                {
                                    this.InHouseItemsWithCharges.Add(spaceUtilization, afterRateApply);
                                }
                                else
                                {
                                    CommonFunctions.writeFunctionalLog("Error while calculating (" + storageChargesMsg[spaceUtilization] + ")");
                                    ErrorList.Add("Problem while calculating storage charges");
                                    this.functionalFlow = false;
                                }
                            }
                            else
                            {
                                CommonFunctions.writeFunctionalLog("There is no tariffs are assigned for " + storageChargesMsg[spaceUtilization]);
                                ErrorList.Add("There is no tariffs are created for storage charges");
                                this.functionalFlow = false;
                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                this.functionalFlow = false;
                throw new INVTPLException("exception while applying charges on stored items:" + ex.Message);
            }
            return this.InHouseItemsWithCharges;
        }

        public decimal getDiscountPrice(decimal chargePrice, decimal disc)
        {

            if (disc != 0 && chargePrice != 0)
            {
                decimal temp = (disc / 100) * chargePrice;
                return chargePrice - temp;
            }
            else
            {
                return chargePrice;
            }

        }

        public IDictionary<int, StorageCharge> getAreaBasedChargeCalculation(RateInfo rate, IDictionary<int, StorageCharge> _storedItems)
        {
            try
            {

                IDictionary<int, StorageCharge> ItemsStorageCharges = new Dictionary<int, StorageCharge>();

                if (rate != null && _storedItems != null)
                {

                    RateInfo rateInfo = rate;
                    CalculatedStorageCharge CSC = new CalculatedStorageCharge();
                    String CurrencyCode = rateInfo.currencyCode;
                    String rateName = rateInfo.rateName;

                    int totalMaterialCount = _storedItems.Count;
                    decimal rateDisc = rateInfo.discount;
                    int rateID = rateInfo.rateID;
                    decimal UnitCost = rateInfo.price;
                    int rateUomID = rateInfo.UomID;

                    CSC.chargeName = rateName;
                    CSC.rateID = rateID;
                    CSC.currencyID = rate.currencyID;
                    CSC.activityID = rate.RateTypeID;

                    decimal _tempRateTotalPrice = 0.00m;
                    decimal _tempRateTotalDisc = 0.00m;
                    decimal _tempRateDisc = rateDisc;

                    IDictionary<int, decimal> conversionFactor = new Dictionary<int, decimal>()
                    {
                        {75,0.0001m}
                    };


                    for (int ind1 = 0; ind1 < totalMaterialCount; ind1++)
                    {

                        var item2 = _storedItems.ElementAt(ind1);
                        int MMID = item2.Key;
                        StorageCharge _tempStorageCharge = item2.Value;

                        if (_tempStorageCharge != null)
                        {

                            StorageCharge calculatedStorages = new StorageCharge();
                            calculatedStorages.materialID = _tempStorageCharge.materialID;
                            calculatedStorages.materialMasterCode = _tempStorageCharge.materialMasterCode;
                            calculatedStorages.materialName = _tempStorageCharge.materialName;
                            calculatedStorages.UOM = _tempStorageCharge.UOM;
                            calculatedStorages.UomID = _tempStorageCharge.UomID;
                            calculatedStorages.spaceUtilizationID = _tempStorageCharge.spaceUtilizationID;
                            calculatedStorages.area = _tempStorageCharge.area;
                            calculatedStorages.unitMeasure = "CM2";
                            calculatedStorages.rateUom = rateInfo.UOM;
                            decimal ConversionalUnitVal = _tempStorageCharge.area;


                            if (_tempStorageCharge.UomID != rateUomID)
                            {
                                if (conversionFactor.ContainsKey(rateUomID))
                                {
                                    ConversionalUnitVal *= conversionFactor[rateUomID];
                                }
                                else
                                {
                                    CommonFunctions.writeFunctionalLog("problem while converting UOM Assigned Rate UOM:" + rateInfo.UOM);
                                    ErrorList.Add("problem while converting UOM Assigned Rate UOM:" + rateInfo.UOM);
                                }
                            }


                            IList<MaterialDetailedCharges> detailedCharges;
                            if (_tempStorageCharge.charges != null)
                            {
                                detailedCharges = _tempStorageCharge.charges;
                            }
                            else
                            {
                                detailedCharges = new List<MaterialDetailedCharges>();
                            }

                            MaterialDetailedCharges detailedCharge = new MaterialDetailedCharges();
                            detailedCharge.chargeName = rateName;
                            detailedCharge.rateID = rateID;
                            decimal _tempPrice = 0;
                            decimal _tempDiscPrice = 0;

                            IList<MaterialStorageCharge> recordLevelCharges = new List<MaterialStorageCharge>();
                            foreach (MaterialStorageCharge msc in _tempStorageCharge.materialStorageCharges)
                            {

                                MaterialStorageCharge _tempMSC = new MaterialStorageCharge();
                                IList<Charge> _charges;

                                if (msc.charges != null)
                                {
                                    _charges = msc.charges;
                                }
                                else
                                {
                                    _charges = new List<Charge>();
                                }

                                Charge charge = new Charge();
                                decimal _recordLevelCharge = 0;

                                _tempMSC.materialAvailDate = msc.materialAvailDate;
                                _tempMSC.quantity = msc.quantity;

                                charge.rateID = rateID;
                                charge.chargeName = rateName;

                                _recordLevelCharge = msc.quantity * UnitCost * ConversionalUnitVal;
                                charge.price = _recordLevelCharge;
                                charge.costAfterDisc = getDiscountPrice(_recordLevelCharge, rateDisc);
                                charge.discount = rateDisc;
                                charge.unitCost = UnitCost;
                                charge.isUnitCost = false;

                                _tempPrice += _recordLevelCharge;
                                _tempDiscPrice += charge.costAfterDisc;

                                _charges.Add(charge);
                                _tempMSC.charges = _charges;
                                recordLevelCharges.Add(_tempMSC);
                            }

                            detailedCharge.price = _tempPrice;
                            detailedCharge.disc = rateDisc;
                            detailedCharge.discPrice = _tempDiscPrice;

                            detailedCharges.Add(detailedCharge);
                            calculatedStorages.charges = detailedCharges;
                            calculatedStorages.materialStorageCharges = recordLevelCharges;

                            if (!ItemsStorageCharges.ContainsKey(calculatedStorages.materialID))
                            {
                                ItemsStorageCharges.Add(calculatedStorages.materialID, calculatedStorages);
                            }
                            else
                            {
                                ItemsStorageCharges[calculatedStorages.materialID] = calculatedStorages;
                            }

                            _tempRateTotalPrice += _tempPrice;
                            _tempRateTotalDisc += _tempDiscPrice;

                        }

                    }

                    CSC.discount = (_tempRateDisc != 0)?_tempRateDisc:Convert.ToDecimal("0.00");
                    CSC.costAfterDisc = _tempRateTotalDisc;
                    CSC.price = _tempRateTotalPrice;
                    CSC.currencyCode = CurrencyCode;
                    this.actualStorageCharges.Add(CSC);
                }
                return ItemsStorageCharges;

            }
            catch (Exception ex)
            {
                throw new Exception("exception while filtering storage charges:" + ex.Message);
            }
        }

        public IDictionary<int, StorageCharge> getItemWiseRentalCharges(RateInfo rate, IDictionary<int, StorageCharge> _storedItems)
        {
            try
            {

                IDictionary<int, StorageCharge> ItemsStorageCharges = new Dictionary<int, StorageCharge>();
                if (rate != null && _storedItems != null)
                {

                    RateInfo rateInfo = rate;
                    CalculatedStorageCharge CSC = new CalculatedStorageCharge();
                    String CurrencyCode = rateInfo.currencyCode;
                    String rateName = rateInfo.rateName;

                    int totalMaterialCount = _storedItems.Count;
                    decimal rateDisc = rateInfo.discount;
                    int rateID = rateInfo.rateID;
                    decimal UnitCost = rateInfo.price;
                    int rateUomID = rateInfo.UomID;

                    CSC.chargeName = rateName;
                    CSC.rateID = rateID;
                    CSC.currencyID = rate.currencyID;
                    CSC.activityID = rate.RateTypeID;

                    decimal _tempRateTotalPrice = 0;
                    decimal _tempRateTotalDisc = 0;
                    decimal _tempRateDisc = rateDisc;

                    IDictionary<int, decimal> conversionFactor = new Dictionary<int, decimal>()
                    {
                        {17,0.083m},{18,1.00m}
                    };

                    for (int ind1 = 0; ind1 < totalMaterialCount; ind1++)
                    {

                        var item2 = _storedItems.ElementAt(ind1);
                        int MMID = item2.Key;
                        StorageCharge _tempStorageCharge = item2.Value;

                        if (_tempStorageCharge != null)
                        {

                            StorageCharge calculatedStorages = new StorageCharge();

                            calculatedStorages.materialID = _tempStorageCharge.materialID;
                            calculatedStorages.materialMasterCode = _tempStorageCharge.materialMasterCode;
                            calculatedStorages.materialName = _tempStorageCharge.materialName;
                            calculatedStorages.UOM = _tempStorageCharge.UOM;
                            calculatedStorages.UomID = _tempStorageCharge.UomID;
                            calculatedStorages.spaceUtilizationID = _tempStorageCharge.spaceUtilizationID;
                            calculatedStorages.rentalConstantCharge = 1.00m;
                            calculatedStorages.unitMeasure = "EA";
                            calculatedStorages.rateUom = rateInfo.UOM;

                            decimal ConversionalUnitVal = 1;

                            if (_tempStorageCharge.UomID != rateUomID)
                            {
                                if (conversionFactor.ContainsKey(rateUomID))
                                {
                                    ConversionalUnitVal = conversionFactor[rateUomID];
                                }
                                else
                                {
                                    CommonFunctions.writeFunctionalLog("problem while calculating Item rental charges, assigned rate UOM: " + rateInfo.UOM);
                                }
                            }

                            IList<MaterialDetailedCharges> detailedCharges;
                            if (_tempStorageCharge.charges != null)
                            {
                                detailedCharges = _tempStorageCharge.charges;
                            }
                            else
                            {
                                detailedCharges = new List<MaterialDetailedCharges>();
                            }

                            MaterialDetailedCharges detailedCharge = new MaterialDetailedCharges();
                            detailedCharge.chargeName = rateName;
                            detailedCharge.rateID = rateID;
                            decimal _tempPrice = 0;
                            decimal _tempDiscPrice = 0;

                            IList<MaterialStorageCharge> recordLevelCharges = new List<MaterialStorageCharge>();
                            foreach (MaterialStorageCharge msc in _tempStorageCharge.materialStorageCharges)
                            {

                                MaterialStorageCharge _tempMSC = new MaterialStorageCharge();
                                IList<Charge> _charges;

                                if (msc.charges != null)
                                {
                                    _charges = msc.charges;
                                }
                                else
                                {
                                    _charges = new List<Charge>();
                                }

                                Charge charge = new Charge();

                                decimal _recordLevelCharge = 0;
                                _tempMSC.materialAvailDate = msc.materialAvailDate;
                                _tempMSC.quantity = msc.quantity;

                                charge.rateID = rateID;
                                charge.chargeName = rateName;

                                _recordLevelCharge = UnitCost * msc.quantity * ConversionalUnitVal;
                                charge.price = _recordLevelCharge;
                                charge.costAfterDisc = getDiscountPrice(_recordLevelCharge, rateDisc);
                                charge.discount = rateDisc;
                                charge.perBinCapacity = 0.00m;
                                charge.unitCost = UnitCost;
                                charge.isUnitCost = false;

                                _tempPrice += _recordLevelCharge;
                                _tempDiscPrice += charge.costAfterDisc;

                                _charges.Add(charge);
                                _tempMSC.charges = _charges;
                                recordLevelCharges.Add(_tempMSC);
                            }

                            detailedCharge.price = _tempPrice;
                            detailedCharge.disc = rateDisc;
                            detailedCharge.discPrice = _tempDiscPrice;

                            detailedCharges.Add(detailedCharge);
                            calculatedStorages.charges = detailedCharges;
                            calculatedStorages.materialStorageCharges = recordLevelCharges;

                            if (!ItemsStorageCharges.ContainsKey(calculatedStorages.materialID))
                            {
                                ItemsStorageCharges.Add(calculatedStorages.materialID, calculatedStorages);
                            }
                            else
                            {
                                ItemsStorageCharges[calculatedStorages.materialID] = calculatedStorages;
                            }

                            _tempRateTotalPrice += _tempPrice;
                            _tempRateTotalDisc += _tempDiscPrice;

                        }

                    }

                    CSC.discount = _tempRateDisc;
                    CSC.costAfterDisc = _tempRateTotalDisc;
                    CSC.price = _tempRateTotalPrice;
                    CSC.currencyCode = CurrencyCode;
                    this.actualStorageCharges.Add(CSC);
                }
                return ItemsStorageCharges;

            }
            catch (Exception ex)
            {
                throw new Exception("exception while filtering storage charges:" + ex.Message);
            }
        }

        public IDictionary<int, StorageCharge> getMaterialVolumeBasedCalculation(RateInfo rate, IDictionary<int, StorageCharge> _storedItems)
        {
            try
            {

                IDictionary<int, StorageCharge> ItemsStorageCharges = new Dictionary<int, StorageCharge>();

                if (rate != null && _storedItems != null)
                {

                    RateInfo rateInfo = rate;
                    CalculatedStorageCharge CSC = new CalculatedStorageCharge();
                    String CurrencyCode = rateInfo.currencyCode;
                    String rateName = rateInfo.rateName;

                    int totalMaterialCount = _storedItems.Count;
                    decimal rateDisc = rateInfo.discount;
                    int rateID = rateInfo.rateID;
                    decimal UnitCost = rateInfo.price;
                    int rateUomID = rateInfo.UomID;

                    CSC.chargeName = rateName;
                    CSC.rateID = rateID;
                    CSC.currencyID = rate.currencyID;
                    CSC.activityID = rate.RateTypeID;

                    decimal _tempRateTotalPrice = 0;
                    decimal _tempRateTotalDisc = 0;
                    decimal _tempRateDisc = rateDisc;

                    IDictionary<int, decimal> conversionFactor = new Dictionary<int, decimal>()
                    {
                        {2,0.061m},{34,0.000001m}
                    };


                    for (int ind1 = 0; ind1 < totalMaterialCount; ind1++)
                    {

                        var item2 = _storedItems.ElementAt(ind1);
                        int MMID = item2.Key;
                        StorageCharge _tempStorageCharge = item2.Value;

                        if (_tempStorageCharge != null)
                        {

                            StorageCharge calculatedStorages = new StorageCharge();
                            calculatedStorages.materialID = _tempStorageCharge.materialID;
                            calculatedStorages.materialMasterCode = _tempStorageCharge.materialMasterCode;
                            calculatedStorages.materialName = _tempStorageCharge.materialName;
                            calculatedStorages.UOM = _tempStorageCharge.UOM;
                            calculatedStorages.UomID = _tempStorageCharge.UomID;
                            calculatedStorages.spaceUtilizationID = _tempStorageCharge.spaceUtilizationID;
                            calculatedStorages.volume = _tempStorageCharge.volume;

                            calculatedStorages.unitMeasure = "CM3";
                            calculatedStorages.rateUom = rateInfo.UOM;

                            decimal ConvertedUnitVal = _tempStorageCharge.volume;
                            if (conversionFactor.ContainsKey(rateUomID))
                            {
                                ConvertedUnitVal *= conversionFactor[rateUomID];
                            }
                            else
                            {
                                CommonFunctions.writeFunctionalLog("problem while calculating material volume base charges, assigned rate UOM:" + rateInfo.UOM);
                            }

                            IList<MaterialDetailedCharges> detailedCharges;
                            if (_tempStorageCharge.charges != null)
                            {
                                detailedCharges = _tempStorageCharge.charges;
                            }
                            else
                            {
                                detailedCharges = new List<MaterialDetailedCharges>();
                            }

                            MaterialDetailedCharges detailedCharge = new MaterialDetailedCharges();
                            detailedCharge.chargeName = rateName;
                            detailedCharge.rateID = rateID;
                            decimal _tempPrice = 0;
                            decimal _tempDiscPrice = 0;

                            IList<MaterialStorageCharge> recordLevelCharges = new List<MaterialStorageCharge>();
                            foreach (MaterialStorageCharge msc in _tempStorageCharge.materialStorageCharges)
                            {

                                MaterialStorageCharge _tempMSC = new MaterialStorageCharge();
                                IList<Charge> _charges;

                                if (msc.charges != null)
                                {
                                    _charges = msc.charges;
                                }
                                else
                                {
                                    _charges = new List<Charge>();
                                }

                                Charge charge = new Charge();
                                decimal _recordLevelCharge = 0;
                                _tempMSC.materialAvailDate = msc.materialAvailDate;
                                _tempMSC.quantity = msc.quantity;

                                charge.rateID = rateID;
                                charge.chargeName = rateName;

                                _recordLevelCharge = UnitCost * msc.quantity * ConvertedUnitVal;
                                charge.price = _recordLevelCharge;
                                charge.costAfterDisc = getDiscountPrice(_recordLevelCharge, rateDisc);
                                charge.discount = rateDisc;
                                charge.unitCost = UnitCost;
                                charge.isUnitCost = false;

                                _tempPrice += _recordLevelCharge;
                                _tempDiscPrice += charge.costAfterDisc;

                                _charges.Add(charge);
                                _tempMSC.charges = _charges;
                                recordLevelCharges.Add(_tempMSC);
                            }

                            detailedCharge.price = _tempPrice;
                            detailedCharge.disc = rateDisc;
                            detailedCharge.discPrice = _tempDiscPrice;

                            detailedCharges.Add(detailedCharge);
                            calculatedStorages.charges = detailedCharges;
                            calculatedStorages.materialStorageCharges = recordLevelCharges;

                            if (!ItemsStorageCharges.ContainsKey(calculatedStorages.materialID))
                            {
                                ItemsStorageCharges.Add(calculatedStorages.materialID, calculatedStorages);
                            }
                            else
                            {
                                ItemsStorageCharges[calculatedStorages.materialID] = calculatedStorages;
                            }

                            _tempRateTotalPrice += _tempPrice;
                            _tempRateTotalDisc += _tempDiscPrice;

                        }

                    }

                    CSC.discount = _tempRateDisc;
                    CSC.costAfterDisc = _tempRateTotalDisc;
                    CSC.price = _tempRateTotalPrice;
                    CSC.currencyCode = CurrencyCode;
                    this.actualStorageCharges.Add(CSC);
                }
                return ItemsStorageCharges;

            }
            catch (Exception ex)
            {
                throw new Exception("exception while filtering storage charges:" + ex.Message);
            }
        }

        public IDictionary<int, StorageCharge> getMaterialWeightBasedCalculation(RateInfo rate, IDictionary<int, StorageCharge> _storedItems)
        {

            try
            {

                //20 G	Gram
                //26 KG	Kilogram
                //29 KT	Kilotonne

                IDictionary<int, StorageCharge> ItemsStorageCharges = new Dictionary<int, StorageCharge>();

                if (rate != null && _storedItems != null)
                {

                    RateInfo rateInfo = rate;
                    CalculatedStorageCharge CSC = new CalculatedStorageCharge();
                    String CurrencyCode = rateInfo.currencyCode;
                    String rateName = rateInfo.rateName;

                    int totalMaterialCount = _storedItems.Count;
                    decimal rateDisc = rateInfo.discount;
                    int rateID = rateInfo.rateID;
                    decimal UnitCost = rateInfo.price;
                    int rateUomID = rateInfo.UomID;
                    string rateUOM = rateInfo.UOM;


                    IDictionary<int, decimal> conversionFactor = new Dictionary<int, decimal>()
                    {
                        {20,1000},{26,1},{29,0.000001m}
                    };

                    CSC.chargeName = rateName;
                    CSC.rateID = rateID;
                    CSC.currencyID = rate.currencyID;
                    CSC.activityID = rate.RateTypeID;

                    decimal _tempRateTotalPrice = 0;
                    decimal _tempRateTotalDisc = 0;
                    decimal _tempRateDisc = rateDisc;

                    for (int ind1 = 0; ind1 < totalMaterialCount; ind1++)
                    {
                        var item2 = _storedItems.ElementAt(ind1);
                        int MMID = item2.Key;
                        StorageCharge _tempStorageCharge = item2.Value;


                        if (_tempStorageCharge != null)
                        {

                            StorageCharge calculatedStorages = new StorageCharge();
                            calculatedStorages.materialID = _tempStorageCharge.materialID;
                            calculatedStorages.materialMasterCode = _tempStorageCharge.materialMasterCode;
                            calculatedStorages.materialName = _tempStorageCharge.materialName;
                            calculatedStorages.UOM = _tempStorageCharge.UOM;
                            calculatedStorages.UomID = _tempStorageCharge.UomID;
                            calculatedStorages.spaceUtilizationID = _tempStorageCharge.spaceUtilizationID;
                            calculatedStorages.weight = _tempStorageCharge.weight;
                            calculatedStorages.unitMeasure = "KG";
                            calculatedStorages.rateUom = rateInfo.UOM;
                            decimal convertedUnitVal = _tempStorageCharge.weight;

                            if (_tempStorageCharge.UomID != rateUomID)
                            {
                                decimal factor = conversionFactor.ContainsKey(rateUomID) ? conversionFactor[rateUomID] : 0;
                                if (factor == 0)
                                {
                                    CommonFunctions.writeFunctionalLog("Invalid  UOM: " + rateUOM + " while calculating material weight based charges ");
                                }
                                convertedUnitVal = _tempStorageCharge.weight * factor;
                            }


                            IList<MaterialDetailedCharges> detailedCharges;
                            if (_tempStorageCharge.charges != null)
                            {
                                detailedCharges = _tempStorageCharge.charges;
                            }
                            else
                            {
                                detailedCharges = new List<MaterialDetailedCharges>();
                            }

                            MaterialDetailedCharges detailedCharge = new MaterialDetailedCharges();
                            detailedCharge.chargeName = rateName;
                            detailedCharge.rateID = rateID;
                            decimal _tempPrice = 0;
                            decimal _tempDiscPrice = 0;

                            IList<MaterialStorageCharge> recordLevelCharges = new List<MaterialStorageCharge>();
                            foreach (MaterialStorageCharge msc in _tempStorageCharge.materialStorageCharges)
                            {

                                MaterialStorageCharge _tempMSC = new MaterialStorageCharge();
                                IList<Charge> _charges;

                                if (msc.charges != null)
                                {
                                    _charges = msc.charges;
                                }
                                else
                                {
                                    _charges = new List<Charge>();
                                }

                                Charge charge = new Charge();

                                decimal _recordLevelCharge = 0;
                                _tempMSC.materialAvailDate = msc.materialAvailDate;
                                _tempMSC.quantity = msc.quantity;
                                charge.rateID = rateID;
                                charge.chargeName = rateName;

                                _recordLevelCharge = UnitCost * msc.quantity * convertedUnitVal;
                                charge.price = _recordLevelCharge;
                                charge.costAfterDisc = getDiscountPrice(_recordLevelCharge, rateDisc);
                                charge.discount = rateDisc;
                                charge.unitCost = UnitCost;
                                charge.isUnitCost = false;


                                _tempPrice += _recordLevelCharge;
                                _tempDiscPrice += charge.costAfterDisc;

                                _charges.Add(charge);
                                _tempMSC.charges = _charges;
                                recordLevelCharges.Add(_tempMSC);
                            }

                            detailedCharge.price = _tempPrice;
                            detailedCharge.disc = rateDisc;
                            detailedCharge.discPrice = _tempDiscPrice;

                            detailedCharges.Add(detailedCharge);
                            calculatedStorages.charges = detailedCharges;
                            calculatedStorages.materialStorageCharges = recordLevelCharges;

                            if (!ItemsStorageCharges.ContainsKey(calculatedStorages.materialID))
                            {
                                ItemsStorageCharges.Add(calculatedStorages.materialID, calculatedStorages);
                            }
                            else
                            {
                                ItemsStorageCharges[calculatedStorages.materialID] = calculatedStorages;
                            }

                            _tempRateTotalPrice += _tempPrice;
                            _tempRateTotalDisc += _tempDiscPrice;

                        }

                    }

                    CSC.discount = _tempRateDisc;
                    CSC.costAfterDisc = _tempRateTotalDisc;
                    CSC.price = _tempRateTotalPrice;
                    CSC.currencyCode = CurrencyCode;
                    this.actualStorageCharges.Add(CSC);
                }
                return ItemsStorageCharges;

            }
            catch (Exception ex)
            {
                throw new Exception("exception while filtering storage charges:" + ex.Message);
            }
        }

        public IDictionary<int, StorageCharge> applyBinChargeOnStoredItems(RateInfo rate, IDictionary<int, StorageCharge> _storedItems)
        {

            /*
                Here no need to check the Bin level charges & no need to compare material receving UoMID.
                and rest of the cases you should compare actul materil receiving UoMID and allocated rate UomID.
            */

            try
            {

                IDictionary<int, StorageCharge> ItemsStorageCharges = new Dictionary<int, StorageCharge>();
                if (rate != null && _storedItems != null)
                {

                    RateInfo rateInfo = rate;
                    CalculatedStorageCharge CSC = new CalculatedStorageCharge();
                    String CurrencyCode = rateInfo.currencyCode;
                    String rateName = rateInfo.rateName;

                    int totalMaterialCount = _storedItems.Count;
                    decimal rateDisc = rateInfo.discount;
                    int rateID = rateInfo.rateID;
                    decimal UnitCost = rateInfo.price;
                    int rateUomID = rateInfo.UomID;

                    CSC.chargeName = rateName;
                    CSC.rateID = rateID;
                    CSC.currencyID = rate.currencyID;
                    CSC.activityID = rate.RateTypeID;

                    decimal _tempRateTotalPrice = 0;
                    decimal _tempRateTotalDisc = 0;
                    decimal _tempRateDisc = rateDisc;

                    for (int ind1 = 0; ind1 < totalMaterialCount; ind1++)
                    {

                        var item2 = _storedItems.ElementAt(ind1);
                        int MMID = item2.Key;
                        StorageCharge _tempStorageCharge = item2.Value;

                        if (_tempStorageCharge != null)
                        {

                            StorageCharge calculatedStorages = new StorageCharge();
                            calculatedStorages.materialID = _tempStorageCharge.materialID;
                            calculatedStorages.materialMasterCode = _tempStorageCharge.materialMasterCode;
                            calculatedStorages.perBinQuantity = _tempStorageCharge.perBinQuantity;
                            calculatedStorages.materialName = _tempStorageCharge.materialName;
                            calculatedStorages.UOM = _tempStorageCharge.UOM;
                            calculatedStorages.UomID = _tempStorageCharge.UomID;
                            calculatedStorages.spaceUtilizationID = _tempStorageCharge.spaceUtilizationID;
                            calculatedStorages.unitMeasure = "EA";
                            calculatedStorages.rateUom = rateInfo.UOM;

                            decimal perBinQuantity = _tempStorageCharge.perBinQuantity;

                            IList<MaterialDetailedCharges> detailedCharges;
                            if (_tempStorageCharge.charges != null)
                            {
                                detailedCharges = _tempStorageCharge.charges;
                            }
                            else
                            {
                                detailedCharges = new List<MaterialDetailedCharges>();
                            }

                            MaterialDetailedCharges detailedCharge = new MaterialDetailedCharges();
                            detailedCharge.chargeName = rateName;
                            detailedCharge.rateID = rateID;
                            decimal _tempPrice = 0;
                            decimal _tempDiscPrice = 0;

                            IList<MaterialStorageCharge> recordLevelCharges = new List<MaterialStorageCharge>();
                            foreach (MaterialStorageCharge msc in _tempStorageCharge.materialStorageCharges)
                            {

                                MaterialStorageCharge _tempMSC = new MaterialStorageCharge();
                                IList<Charge> _charges;

                                if (msc.charges != null)
                                {
                                    _charges = msc.charges;
                                }
                                else
                                {
                                    _charges = new List<Charge>();
                                }

                                Charge charge = new Charge();
                                decimal _recordLevelCharge = 0;

                                _tempMSC.materialAvailDate = msc.materialAvailDate;
                                _tempMSC.quantity = msc.quantity;

                                charge.rateID = rateID;
                                charge.chargeName = rateName;

                                _recordLevelCharge = getRecordLevelBinCost(msc.quantity, perBinQuantity, UnitCost);
                                charge.price = _recordLevelCharge;
                                charge.costAfterDisc = getDiscountPrice(_recordLevelCharge, rateDisc);
                                charge.discount = rateDisc;
                                charge.perBinCapacity = perBinQuantity;
                                charge.unitCost = UnitCost;
                                charge.isUnitCost = false;

                                _tempPrice += _recordLevelCharge;
                                _tempDiscPrice += charge.costAfterDisc;

                                _charges.Add(charge);
                                _tempMSC.charges = _charges;
                                recordLevelCharges.Add(_tempMSC);
                            }

                            detailedCharge.price = _tempPrice;
                            detailedCharge.disc = rateDisc;
                            detailedCharge.discPrice = _tempDiscPrice;

                            detailedCharges.Add(detailedCharge);
                            calculatedStorages.charges = detailedCharges;
                            calculatedStorages.materialStorageCharges = recordLevelCharges;

                            if (!ItemsStorageCharges.ContainsKey(calculatedStorages.materialID))
                            {
                                ItemsStorageCharges.Add(calculatedStorages.materialID, calculatedStorages);
                            }
                            else
                            {
                                ItemsStorageCharges[calculatedStorages.materialID] = calculatedStorages;
                            }

                            _tempRateTotalPrice += _tempPrice;
                            _tempRateTotalDisc += _tempDiscPrice;

                        }

                    }

                    CSC.discount = _tempRateDisc;
                    CSC.costAfterDisc = _tempRateTotalDisc;
                    CSC.price = _tempRateTotalPrice;
                    CSC.currencyCode = CurrencyCode;
                    this.actualStorageCharges.Add(CSC);
                }
                return ItemsStorageCharges;

            }
            catch (Exception ex)
            {
                throw new Exception("exception while filtering storage charges:" + ex.Message);
            }

        }

        public IList<RateInfo> updateRatesWithDefaultRates(IList<RateInfo> rates, IDictionary<int, RateInfo> defaultRates)
        {

            IList<RateInfo> _tempRates = rates;

            if (rates != null && defaultRates != null)
            {
                for (int index = 0; index < defaultRates.Count; index++)
                {

                    bool _IsRequired = true;
                    var item = defaultRates.ElementAt(index);
                    RateInfo _defaultRate = item.Value;

                    int RateID = item.Key;
                    int GroupID = _defaultRate.GroupID;
                    int RateTypeID = _defaultRate.RateTypeID;

                    foreach (RateInfo activeRate in rates)
                    {
                        if (activeRate.RateTypeID == RateTypeID && activeRate.rateID == RateID && activeRate.GroupID == GroupID)
                        {
                            _IsRequired = false;
                            break;
                        }
                    }

                    if (_IsRequired)
                    {
                        _tempRates.Add(_defaultRate);
                    }

                }

            }

            return _tempRates;
        }

        public Decimal getRecordLevelBinCost(decimal quantity, decimal perbinUnit, decimal unitCost)
        {
            if (unitCost != 0 && quantity != 0)
            {
                return (quantity / perbinUnit) * unitCost;
            }
            return 0;
        }

        public SummaryPage prepareSummaryPage(IList<CalculatedStorageCharge> calculatedPrice, IDictionary<string, TransactionCharge> accessorial, TenantInfo tenantData, IDictionary<int, RateInfo> defaultRates)
        {

            try
            {

                string invoiceCurrency = tenantData.currencyCode;
                int invoiceCurrencyID = tenantData.currencyID;

                SummaryPage summary = new SummaryPage();
                summary.rates = new List<TPLRate>();
                summary.serviceCharge = new ServiceCharge();
                decimal _totalRatesPrice = 0;
                summary.currency = invoiceCurrency;
                summary.rateCurrency = invoiceCurrency;
                summary.currencyID = invoiceCurrencyID;
                summary.rateCurrencyID = invoiceCurrencyID;

                if (calculatedPrice != null)
                {
                    foreach (CalculatedStorageCharge csc in calculatedPrice)
                    {
                        TPLRate _tplRate = new TPLRate();
                        _tplRate.rateName = csc.chargeName;
                        _tplRate.price = csc.costAfterDisc;
                        _tplRate.pricePerUnit = csc.price;
                        _tplRate.quantity = Convert.ToDecimal("1.00");
                        _tplRate.currency = csc.currencyCode;
                        summary.rateCurrency = csc.currencyCode;
                        summary.rateCurrencyID = csc.currencyID;
                        _totalRatesPrice += csc.costAfterDisc; 
                        summary.rates.Add(_tplRate);

                        if (!InvoiceRates.ContainsKey(csc.rateID))
                        {
                            InvoiceRate rate = new InvoiceRate();
                            rate.rateID = csc.rateID;
                            rate.price = csc.costAfterDisc;
                            rate.unitPrice = csc.price;
                            rate.discount = csc.discount;
                            rate.currencyID = csc.currencyID;
                            rate.rateName = csc.chargeName;
                            rate.QTY = Convert.ToDecimal("1.00");
                            rate.activityID = csc.activityID;
                            rate.currencyCode = csc.currencyCode;
                            InvoiceRates.Add(csc.rateID, rate);
                        }
                        else
                        {
                            InvoiceRates[csc.rateID].price += _tplRate.price;
                            InvoiceRates[csc.rateID].QTY += 1;
                        }

                    }

                }

                if (accessorial != null)
                {


                    for (int index = 0; index < accessorial.Count; index++)
                    {
                        decimal _tempTransactionAmount = 0;
                        var item = accessorial.ElementAt(index);
                        string tranKey = item.Key;
                        TransactionCharge tranCharge = item.Value;
                        int TranTypeID = tranCharge.transactionType;
                        string transactionCurrency = "";
                        int TrancurrencyID = 0;

                        foreach (AccessorialCharge assess in tranCharge.charges)
                        {
                            _totalRatesPrice += assess.quantity * assess.unitCost;
                            transactionCurrency = assess.currency;
                            _tempTransactionAmount += assess.quantity * assess.unitCost;
                            TrancurrencyID = assess.currencyID;

                            if (!InvoiceRates.ContainsKey(assess.rateID))
                            {
                                InvoiceRate rate = new InvoiceRate();
                                rate.rateID = assess.rateID;
                                rate.discount = assess.disc;
                                rate.price = assess.unitCost * assess.quantity;
                                rate.currencyID = assess.currencyID;
                                rate.currencyCode = assess.currency;
                                rate.rateName = assess.activityName;
                                rate.activityID = assess.activityID;
                                rate.unitPrice = assess.priceWithoutDisc * assess.quantity;
                                rate.QTY = Convert.ToDecimal("1.00");
                                InvoiceRates.Add(assess.rateID, rate);
                            }
                            else
                            {
                                InvoiceRates[assess.rateID].price += assess.unitCost * assess.quantity;
                                InvoiceRates[assess.rateID].QTY += 1;
                            }
                        }

                        TPLRate _tplRate = new TPLRate();
                        _tplRate.currency = transactionCurrency;
                        _tplRate.quantity = Convert.ToDecimal("1.00");
                        _tplRate.pricePerUnit = _tempTransactionAmount;
                        _tplRate.price = _tempTransactionAmount;
                        summary.rateCurrency = transactionCurrency;
                        summary.rateCurrencyID = TrancurrencyID;
                        string _tempRateName = (TranTypeID == 1) ? "Inbound Transaction " : "Outbound Transaction ";
                        _tplRate.rateName = _tempRateName + " #" + Convert.ToString(tranCharge.transactionID) + " Charges";
                        summary.rates.Add(_tplRate);
                    }


                }

                summary._totalRatesPrice = _totalRatesPrice;
                if (summary.rateCurrency.Trim() != summary.currency.Trim())
                {
                    decimal _temp = 0;
                    _temp = convertedAmount(summary.rateCurrency.Trim(), summary.currency.Trim(), _totalRatesPrice, DateTime.Now);
                    summary.price = _temp;
                }
                else
                {
                    summary.price = _totalRatesPrice;
                }
                return summary;

            }
            catch (Exception e)
            {
                throw new INVTPLException("Exception while preparing summary page:" + e.StackTrace);
            }

        }

        public decimal convertedAmount(string fromCode, string toCode, decimal amount, DateTime conversionDate)
        {
            decimal temp = getExchangeRateValue(fromCode, toCode, conversionDate.ToString("yyyy-MM-dd"));
            return temp * amount;
        }

        public SummaryPage getUpdatedSummaryWithMiscellaneousRates(SummaryPage summary, IList<RateInfo> activeRates, int previousInvCount=0)
        {
            SummaryPage _tempSummary = null;
            if (summary != null)
            {
                _tempSummary = summary;
            }
            else
            {
                _tempSummary = new SummaryPage();
            }

            try
            {
                IList<int> excludeCharges = new List<int>();
                excludeCharges.Add(1);
                excludeCharges.Add(2);
                excludeCharges.Add(3);


                if (activeRates != null)
                {
                    
                    foreach (RateInfo rate in activeRates)
                    {
                        if (excludeCharges.Contains(rate.GroupID))
                        {
                            continue;
                        }
                        else
                        {

                            if (previousInvCount != 0 && rate.GroupID == 5 && rate.RateTypeID==13)
                            {
                                //no need to charge setup fee again...
                                //13 - rate type ID
                                //setup fee need to add only once
                                continue;
                            }

                            TPLRate _tempRate = new TPLRate();
                            _tempRate.currency = rate.currencyCode;
                            _tempRate.pricePerUnit = rate.price;
                            _tempRate.price = rate.price;
                            _tempRate.quantity = Convert.ToDecimal("1.00");
                            _tempRate.rateName = rate.rateName;
                            _tempSummary.rates.Add(_tempRate);
                            _tempSummary._totalRatesPrice += rate.price;
                            
                            if (!InvoiceRates.ContainsKey(rate.rateID))
                            {
                                InvoiceRate INVrate = new InvoiceRate();
                                INVrate.rateID = rate.rateID;
                                INVrate.price = rate.price;
                                INVrate.activityID = rate.RateTypeID;
                                INVrate.currencyCode = rate.currencyCode;
                                INVrate.currencyID = rate.currencyID;
                                INVrate.rateName = rate.rateName;
                                INVrate.QTY = Convert.ToDecimal("1.00");
                                INVrate.unitPrice = rate.price;
                                INVrate.discount = Convert.ToDecimal("0.00");
                                InvoiceRates.Add(rate.rateID, INVrate);
                            }
                            else
                            {
                                InvoiceRates[rate.rateID].price += rate.price;
                                InvoiceRates[rate.rateID].QTY += 1;
                            }

                        }

                    }
                }

                if (_tempSummary.rateCurrency.Trim() != _tempSummary.currency.Trim())
                {
                    _tempSummary.price = convertedAmount(_tempSummary.rateCurrency.Trim(), _tempSummary.currency.Trim(), _tempSummary._totalRatesPrice, DateTime.Now);
                }
                else
                {
                    _tempSummary.price = _tempSummary._totalRatesPrice;
                }
            }
            catch (Exception e)
            {
                throw new INVTPLException("Problem with updating miscellaneous rates: " + e.StackTrace);
            }
            return _tempSummary;
        }

        public decimal getExchangeRateValue(string fromCurr, string toCurr, string date)
        {
            decimal exchage = 0;
            string sqlStr = "select top 1 Value FROM TPL_CurrencyRate CR,TPL_Currency  C,TPL_Currency  C2 WHERE CR.FROMCurrencyID = C.TPLCurrencyID and CR.ToCurrencyID = C2.TPLCurrencyID  and C.CurrencyCode = '" + fromCurr + "' and C2.CurrencyCode = '" + toCurr + "'  and RateResponseDate <= '" + date + "'   and Value != 0  order by RateResponseDate desc ";
            DataSet exchageSet = DB.GetDS(sqlStr, false);
            if (exchageSet != null)
            {
                foreach (DataTable table in exchageSet.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        exchage = Convert.ToDecimal(row["Value"].ToString());
                    }
                }
            }
            else
            {
                exchage = 0;
            }
            return exchage;
        }

        public IDictionary<string, TransactionCharge> getUpdatableTransactionsWithDefaultRates(IDictionary<string, TransactionCharge> accessorial, TenantInfo tenantData, IDictionary<int, RateInfo> defaultRates)
        {
            IDictionary<string, TransactionCharge> _tempAccessorial;
            _tempAccessorial = accessorial;

            try
            {
                string invoiceCurrency = tenantData.currencyCode;
                if (accessorial != null)
                {

                    for (int index = 0; index < accessorial.Count; index++)
                    {
                        var item = accessorial.ElementAt(index);
                        string tranKey = item.Key;
                        TransactionCharge tranCharge = item.Value;
                        int TranTypeID = tranCharge.transactionType;
                        bool defaultRateFlag = false;

                        foreach (AccessorialCharge assess in tranCharge.charges)
                        {

                            if (defaultRates != null && defaultRates.ContainsKey(assess.rateID))
                            {
                                if (defaultRates[assess.rateID].GroupID == 1 && (defaultRates[assess.rateID].RateTypeID == 1 || defaultRates[assess.rateID].RateTypeID == 4))
                                {
                                    defaultRateFlag = true;
                                }
                            }

                        }


                        if (defaultRates != null && !defaultRateFlag)
                        {
                            RateInfo _DefaultRateTemp = null;

                            for (int index1 = 0; index1 < defaultRates.Count; index1++)
                            {
                                var item2 = defaultRates.ElementAt(index1);
                                int RateID = item2.Key;
                                RateInfo rateInfoTemp = item2.Value;

                                if (rateInfoTemp.GroupID == 1)
                                {
                                    if (rateInfoTemp.RateTypeID == 1 && TranTypeID == 1)
                                    {
                                        _DefaultRateTemp = rateInfoTemp;
                                        break;
                                    }

                                    if (rateInfoTemp.RateTypeID == 4 && TranTypeID == 2)
                                    {
                                        _DefaultRateTemp = rateInfoTemp;
                                        break;
                                    }
                                }
                            }

                            if (_DefaultRateTemp != null)
                            {
                                AccessorialCharge AC = new AccessorialCharge();
                                AC.unitCost = _DefaultRateTemp.price;
                                AC.priceWithoutDisc = _DefaultRateTemp.price;
                                AC.disc = Convert.ToDecimal("0.00");
                                AC.quantity = Convert.ToDecimal("1.00");
                                AC.rateID = _DefaultRateTemp.rateID;
                                AC.activityName = _DefaultRateTemp.rateName;
                                AC.currency = _DefaultRateTemp.currencyCode;
                                AC.UOM = "EA";
                                AC.activityID = _DefaultRateTemp.RateTypeID;
                                AC.currencyID = _DefaultRateTemp.currencyID;
                                _tempAccessorial[tranKey].charges.Add(AC);
                            }

                        }

                    }

                }

            }

            catch (Exception e)
            {
                throw new INVTPLException("Exception while preparing summary page:" + e.Message);
            }

            return _tempAccessorial;
        }

        public bool sendInvoiceToTenant(TenantInfo tenantData, NetworkCredential credential)
        {
            try
            {

                InvoiceMail mailInfo = tenantData.mailInfo;
                MailAddress from = new MailAddress(mailInfo.FromAddress);
                MailMessage mail = new MailMessage();

                foreach (string toAdd in mailInfo.ToAddress)
                {
                    mail.To.Add(toAdd);
                }

                mail.From = from;
                mail.Subject = mailInfo.Subject;
                mail.Body = mailInfo.Body;

                SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.gmail.com";
                smtp.Host = "smtpout.asia.secureserver.net";
                //smtp.Port = 587;
                smtp.Port = 25;

                smtp.Credentials = credential;
                smtp.EnableSsl = false;

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(mailInfo.PdfLocation);
                mail.Attachments.Add(attachment);
                smtp.Send(mail);

            }
            catch (Exception e)
            {
                CommonFunctions.writeFunctionalLog("problem while ");
                throw new INVTPLException("exception while sending invoice to tenant: " + e.StackTrace);
            }
            return true;
        }

        public bool holdInvoiceRates(TenantInfo tenantData, SummaryPage summary, IDictionary<int, InvoiceRate> InvoiceRatesTemp)
        {

            bool flag = true;
            string invoiceInfo = "";

            try
            {
                if (summary != null)
                {
                    invoiceInfo = tenantData.InvoiceID + "|" + Math.Round(summary.price, 2).ToString() + "|" + summary.currencyID.ToString() + "|" + tenantData.TenantID + "|" + tenantData.fromDateTime.ToString("yyyy-MM-dd") + "|" + tenantData.toDateTime.ToString("yyyy-MM-dd");
                }

                if (InvoiceRatesTemp.Count >= 1 && invoiceInfo.Trim() != "")
                {

                    for (int index = 0; index < InvoiceRatesTemp.Count; index++)
                    {
                        var item = InvoiceRatesTemp.ElementAt(index);
                        InvoiceRate invoiceRate = item.Value;
                        if (invoiceRate.price != 0 && invoiceRate.rateID != 0)
                        {
                            invoiceInfo += "," + Math.Round(invoiceRate.price, 2).ToString() + "|" + invoiceRate.rateID.ToString();
                        }
                    }

                    string insertSQL = "Declare @UpdateResult int ; EXEC [dbo].[sp_TPL_HoldInvoiceInformation] @InvoiceInformation='" + invoiceInfo.Trim() + "', @Result=@UpdateResult OUTPUT; Select @UpdateResult n; ";
                    int result = DB.GetSqlN(insertSQL);
                    if (result != 1)
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception e)
            {
                throw new INVTPLException("exception while holding invoice information: " + e.StackTrace);
            }
            return flag;
        }

        public SummaryPage prepareInvoiceRatesWithVAT(SummaryPage summary, IDictionary<int, TaxationInfo> texInfo)
        {

            Tax TotalTaxTemp = new Tax();
            IDictionary<int, InvoiceRate> InvoiceRatesTmp = new Dictionary<int, InvoiceRate>();
            TotalTaxTemp.price = 0;
            String ratesCurrency = "";
                        
            if (InvoiceRates != null && InvoiceRates.Count >= 1)
            {
                for (int ind = 0; ind < InvoiceRates.Count; ind++)
                {
                    var Item = InvoiceRates.ElementAt(ind);
                    InvoiceRate tempInvRate = Item.Value;
                    
                    if (!InvoiceRatesTmp.ContainsKey(Item.Key))
                    {
                        InvoiceRatesTmp.Add(Item.Key, Item.Value);
                    }
                    else
                    {
                        InvoiceRatesTmp[Item.Key] = Item.Value;
                    }
                    
                    
                    if (texInfo.ContainsKey(tempInvRate.activityID))
                    {
                        TaxationInfo Taxtemp = null;
                        Taxtemp = texInfo[tempInvRate.activityID];
                        ratesCurrency = tempInvRate.currencyCode;
                        
                        if (Taxtemp != null)
                        {

                            decimal price = InvoiceRatesTmp[Item.Key].price * (Taxtemp.percentage / 100);
                            TaxationInfo tempTaxation = new TaxationInfo();
                            tempTaxation = Taxtemp;
                            tempTaxation.price = price;
                            TotalTaxTemp.price += price;
                            InvoiceRatesTmp[Item.Key].taxInfo = tempTaxation;
                        }

                    }

                }

                decimal TaxationPrice = TotalTaxTemp.price;
                summary._totalRatesPrice += TotalTaxTemp.price;
                TotalTaxTemp.convertedPrice = TaxationPrice;

                if(summary.currency.Trim() != ratesCurrency.Trim())
                {
                    TotalTaxTemp.convertedPrice = convertedAmount(ratesCurrency, summary.currency.Trim(), TaxationPrice, DateTime.Now);
                }

                summary.price += TotalTaxTemp.convertedPrice;
                summary.totalTax = TotalTaxTemp;
                InvoiceRates = InvoiceRatesTmp;

            }

            return summary;
        }
                

    }



}