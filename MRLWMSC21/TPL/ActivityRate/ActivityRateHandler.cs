using MRLWMSC21.TPL.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRLWMSC21.TPL.ActivityRate
{
    public class ActivityRateHandler
    {

        public IDictionary<int, RateGroup> rateGroups;
    
        public IDictionary<string, TransactionCharge> accessorialCharges;

        public ActivityRateHandler()
        {
            this.rateGroups = new Dictionary<int, RateGroup>();
            this.accessorialCharges = new Dictionary<string, TransactionCharge>();
        }
        
        public void filterAvailableRates(DataRow row)
        {

            try
            {
                string activityType = row["ActivityRateType"].ToString();
                int activityTypeID = Convert.ToInt16(row["ActivityRateTypeID"].ToString());

                int tariffID = Convert.ToInt16(row["ActivityRateID"].ToString());
                string tariffName = row["ActivityRateName"].ToString();
                decimal price = Convert.ToDecimal(row["UnitCost"].ToString());
                int UomID = Convert.ToInt16(row["UoMID"].ToString());
                string UOM = row["UoM"].ToString();
                int currencyID = Convert.ToInt16(row["CurrencyID"].ToString());
                string currencyCode = row["Code"].ToString();
                decimal discountOnGroup = Convert.ToDecimal(row["discountOnGroup"].ToString());
                decimal discountOnRate = Convert.ToDecimal(row["discountOnRate"].ToString());
                string disType = row["DisType"].ToString();
                string FromDate = row["fromDate"].ToString();
                string ToDate = row["toDate"].ToString();



                int groupID = Convert.ToInt16(row["ActivityRateGroupID"].ToString());
                string groupName = row["ActivityRateGroup"].ToString();

                RateGroup rateGroup;
                RateGroupType rateActivityType;
                RateInfo rateData;

                if (!this.rateGroups.ContainsKey(groupID))
                {
                    rateGroup = new RateGroup();
                    rateGroup.rateGroupID = groupID;
                    rateGroup.rateGroupName = groupName;
                    rateGroup.groupActivityTypes = new Dictionary<int, RateGroupType>();
                }
                else
                {
                    rateGroup = this.rateGroups[groupID];
                }

                if (!rateGroup.groupActivityTypes.ContainsKey(activityTypeID))
                {
                    rateActivityType = new RateGroupType();
                    rateActivityType.rateTypeID = activityTypeID;
                    rateActivityType.rateTypeName = activityType;
                    rateActivityType.rates = new List<RateInfo>();
                }
                else
                {
                    rateActivityType = rateGroup.groupActivityTypes[activityTypeID];
                }

                rateData = new RateInfo();
                rateData.currencyCode = currencyCode;
                rateData.currencyID = currencyID;
                rateData.discount = (disType == "R") ? discountOnRate : ((disType == "G") ? discountOnGroup : 0);
                rateData.price = price;
                rateData.UomID = UomID;
                rateData.UOM = UOM;
                rateData.rateName = tariffName;
                rateData.rateID = tariffID;
                rateData.fromDate = FromDate;
                rateData.toDate = ToDate;

                rateActivityType.rates.Add(rateData);
                
                rateGroup.groupActivityTypes.Add(activityTypeID, rateActivityType);
                
                this.rateGroups.Add(groupID, rateGroup);

            }
            catch (Exception ex)
            {
                //log writing required..
                throw new INVTPLException("Exception while getting active charges: " + ex.Message);
            }

        }
                        
        public IDictionary<int, RateGroup> filterAvailableRates(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                filterAvailableRates(row);
            }
            return this.rateGroups;
        }
       
        public IDictionary<int, RateInfo> getDefaultRates(DataTable table)
        {
            try
            {

                IDictionary<int, RateInfo> defaultRates =  new Dictionary<int,RateInfo>();

                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        int groupID = Convert.ToInt16(row["GroupID"].ToString());
                        string groupName = row["GroupName"].ToString();
                        string GroupActivityType = row["GroupActivityType"].ToString();
                        int GroupActivityTypeID = Convert.ToInt16(row["GroupActivityTypeID"].ToString());

                        string RateName = row["RateName"].ToString();
                        int RateID = Convert.ToInt16(row["RateID"].ToString());
                        string FromDate = row["FromDate"].ToString();
                        string ToDate = row["ToDate"].ToString();
                        int CurrencyID = Convert.ToInt16(row["CurrencyID"].ToString());
                        string CurrencyCode = row["CurrencyCode"].ToString();
                        decimal price = Convert.ToDecimal(row["Cost"].ToString());
                        int UomID = Convert.ToInt16(row["UoMID"].ToString());
                        string UOM = row["UoM"].ToString();

                        RateInfo rateInfo = new RateInfo();

                        rateInfo.discount = Convert.ToDecimal("0.00");
                        rateInfo.currencyCode = CurrencyCode;
                        rateInfo.currencyID = CurrencyID;
                        rateInfo.fromDate = FromDate;
                        rateInfo.toDate = ToDate;
                        rateInfo.GroupID = groupID;
                        rateInfo.RateTypeID = GroupActivityTypeID;
                        rateInfo.price = price;
                        rateInfo.rateName = RateName;
                        rateInfo.rateID = RateID;
                        rateInfo.UOM = UOM;
                        rateInfo.UomID = UomID;
                        defaultRates.Add(RateID,rateInfo);

                    }
                    return defaultRates;
                }

            }
            catch (Exception ex)
            {
                throw new INVTPLException("exception while getting default rates:" + ex.Message);
            }

            return null;

        }
        
        public decimal getTransactionAccessorialChargeIncludeDisc(decimal unitPrice,decimal discount) 
        {
            decimal temp = 0;
            if (discount != 0)
            {
                temp = unitPrice - (unitPrice) * discount / 100;
            }
            else 
            {
                temp = unitPrice;
            }
            return temp;
        }

        public IDictionary<string, TransactionCharge> getTransactionalCharges(DataTable table)
        {

            try
            {
                foreach (DataRow row in table.Rows)
                {

                    int currecnyID = Convert.ToInt16(row["CurrencyID"].ToString());
                    string currencyCode = row["Code"].ToString();
                    string rateName = row["ActivityRateName"].ToString();
                    int rateID = Convert.ToInt16(row["ActivityRateID"].ToString());
                    int activityID = Convert.ToInt16(row["ActivityRateTypeID"].ToString()); 
                    int TranType = Convert.ToInt16(row["TransactionType"].ToString());
                    decimal unitCost = Convert.ToDecimal(row["UnitCost"].ToString());
                    decimal discount = Convert.ToDecimal(row["DisPercentage"].ToString());
                    string tranDate = Convert.ToDateTime(row["transactionDate"].ToString()).ToString("dd/MM/yyyy");
                    string tranKey = Convert.ToInt16(row["TranID"].ToString()) + "_" + Convert.ToInt16(row["TransactionType"].ToString());
                    int tranID = Convert.ToInt16(row["TranID"].ToString());
                    string UOM = row["UOM"].ToString();
                    int UomID = Convert.ToInt16(row["UoMID"].ToString());
                    decimal quantity = Convert.ToDecimal(row["Quantity"].ToString());
                    string tranRefNum = row["tranRefInfo"].ToString();

                    //apply discounts on transactions
                    decimal unitCst = unitCost;
                    unitCost = getTransactionAccessorialChargeIncludeDisc(unitCost, discount);

                    TransactionCharge transCharge;
                    AccessorialCharge accessorialCharge = new AccessorialCharge();

                    accessorialCharge.activityName = rateName;
                    accessorialCharge.unitCost = unitCost;
                    accessorialCharge.UOM = UOM;
                    accessorialCharge.UoMID = UomID;
                    accessorialCharge.quantity = quantity;
                    accessorialCharge.activityID = activityID;
                    accessorialCharge.currency = currencyCode;
                    accessorialCharge.currencyID = currecnyID;
                    accessorialCharge.rateID = rateID;
                    accessorialCharge.disc = discount;
                    accessorialCharge.priceWithoutDisc = unitCst;

                    if (!accessorialCharges.ContainsKey(tranKey))
                    {
                        transCharge = new TransactionCharge();
                        transCharge.date = tranDate;
                        transCharge.charges = new List<AccessorialCharge>();
                        transCharge.transactionID = tranID;
                        transCharge.transactionType = TranType;
                        transCharge.transactionRefNumber = tranRefNum;
                        transCharge.transactionName = (TranType == 1) ? "INBOUND" : "OUTBOUND";
                    }
                    else
                    {
                        transCharge = accessorialCharges[tranKey];
                    }

                    transCharge.charges.Add(accessorialCharge);
                    if (!accessorialCharges.ContainsKey(tranKey))
                    {
                        accessorialCharges.Add(tranKey, transCharge);
                    }
                    else 
                    {
                        accessorialCharges[tranKey] = transCharge;
                    }

                }
                return accessorialCharges;
            }
            catch (Exception ex)
            {
                throw new INVTPLException("exception while filtering transactional charges: " + ex.Message);
            }

        }
        
        public IList<RateInfo> getActiveRateList(DataTable table) 
        {

            try
            {
                IList<RateInfo> _rates = new List<RateInfo>();
                foreach (DataRow row in table.Rows)
                {
                    string activityType = row["ActivityRateType"].ToString();
                    int activityTypeID = Convert.ToInt16(row["ActivityRateTypeID"].ToString());
                    int tariffID = Convert.ToInt16(row["ActivityRateID"].ToString());
                    string tariffName = row["ActivityRateName"].ToString();
                    decimal price = Convert.ToDecimal(row["UnitCost"].ToString());
                    int UomID = Convert.ToInt16(row["UoMID"].ToString());
                    string UOM = row["UoM"].ToString();
                    int currencyID = Convert.ToInt16(row["CurrencyID"].ToString());
                    string currencyCode = row["Code"].ToString();
                    decimal discountOnGroup = Convert.ToDecimal(row["discountOnGroup"].ToString());
                    decimal discountOnRate = Convert.ToDecimal(row["discountOnRate"].ToString());
                    string disType = row["DisType"].ToString();
                    string FromDate = row["fromDate"].ToString();
                    string ToDate = row["toDate"].ToString();
                    int groupID = Convert.ToInt16(row["ActivityRateGroupID"].ToString());
                    string groupName = row["ActivityRateGroup"].ToString();
                    int allocationID = Convert.ToInt16(row["RateAllocationID"].ToString());

                    RateInfo rateData;
                    rateData = new RateInfo();
                    rateData.currencyCode = currencyCode;
                    rateData.currencyID = currencyID;
                    rateData.discount = (disType == "R") ? discountOnRate : ((disType == "G") ? discountOnGroup : 0.00m);
                    rateData.price = price;
                    rateData.UomID = UomID;
                    rateData.UOM = UOM;
                    rateData.rateName = tariffName;
                    rateData.rateID = tariffID;
                    rateData.fromDate = FromDate;
                    rateData.toDate = ToDate;
                    rateData.RateTypeID = activityTypeID;
                    rateData.GroupID = groupID;
                    rateData.rateAllocationID = allocationID;
                    _rates.Add(rateData);
                   
                }
                return _rates;
            }catch(Exception ex)
            {
                    //log write
                throw new INVTPLException("exception while getting rates: "+ex.StackTrace);
            }
           
        }

    }


}