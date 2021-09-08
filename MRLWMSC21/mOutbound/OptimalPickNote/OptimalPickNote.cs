using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MRLWMSC21Common;

namespace MRLWMSC21.mOutbound.OptimalPickNote
{

    /*
     To Generate Optimal Pick information Based on avail Location Data.
     */

    public class OptimalPickNote
    {

        private int OBD_Number;
        private IDictionary<int, SOLineItemData> SOInfo;
        private IList<RawLiveStock> liveStockInfo;
        private IDictionary<string, IList<RawLiveStock>> optimalPickList;
        private IDictionary<int, string> soMSPs;
        private bool IsStockRestrict;

        public OptimalPickNote(int OBDID)
        {
            this.OBD_Number = OBDID;
            SOInfo = new Dictionary<int, SOLineItemData>();
            liveStockInfo = new List<RawLiveStock>();
            IsStockRestrict = false;
        }

        public OptimalPickNote(int OBDID, bool falg)
        {
            this.OBD_Number = OBDID;
            SOInfo = new Dictionary<int, SOLineItemData>();
            liveStockInfo = new List<RawLiveStock>();
            IsStockRestrict = falg;
        }


        public IDictionary<int, SOLineItemData> getSOInfo()
        {
            return SOInfo;
        }
        
        public IDictionary<int, string> getSOMSPs()
        {
            return soMSPs;
        }

        public IDictionary<string, IList<RawLiveStock>> getOptimalPickList(string mcode)
        {

            try
            {

                string pickNoteSQLStatement = "EXEC [SP_SUGGESTED_PICKING_DATA_FOR_PICKNOTE] @OutboundID=" + OBD_Number + ",@MCode='" + mcode + "'";
                DataSet dsAvailStockData = DB.GetDS(pickNoteSQLStatement, false);

                DataTable dtMaterialStorageParameters = null;
                if (dsAvailStockData.Tables.Count >= 2)
                {
                    dtMaterialStorageParameters = dsAvailStockData.Tables[2];
                    soMSPs = new Dictionary<int, string>();
                    foreach (DataRow dr in dtMaterialStorageParameters.Rows)
                    {
                        soMSPs.Add(Convert.ToInt32(dr[0].ToString()), dr[1].ToString());
                    }

                }

                if (dsAvailStockData.Tables.Count >= 1)
                {

                    SOInfo = initSODetails(dsAvailStockData.Tables[0]);
                    updateSODataWithAvailStock(dsAvailStockData.Tables[1]);

                    if (IsStockRestrict)
                    {

                        removeDamagedItemsFromSOData();

                        for (int i = 0; i < SOInfo.Count; i++)
                        {
                            var item = SOInfo.ElementAt(i);
                            SOLineItemData soLineItem = item.Value;
                            IList<LiveStockData> liveStockTemp = soLineItem.StockData;
                            soLineItem.StockData = liveStockTemp.OrderBy(o => o.GoodsMovementID).ToList();
                        }

                    }

                    liveStockInfo = getOptimalRouteWiseData();

                    liveStockInfo = liveStockInfo.OrderBy(o => o.LocGenID).ToList();

                    optimalPickList = getGroupLocationInfo(liveStockInfo);

                }
                return optimalPickList;
            }
            catch (Exception ex)
            {
                //require to write log here
                CommonLogic.createErrorNode("Manikanta", "OptimalPickNote", "OptimalPickNote", "Eliminate Damaged QTY", ex.StackTrace);
            }
            return null;
        }



        private IDictionary<string, IList<RawLiveStock>> getGroupLocationInfo(IList<RawLiveStock> orderedStock)
        {

            if (orderedStock != null)
            {
                IDictionary<string, IList<RawLiveStock>> tempGroupedStock = new Dictionary<string, IList<RawLiveStock>>();
                try
                {
                    foreach (RawLiveStock stock in orderedStock)
                    {
                        if (!tempGroupedStock.ContainsKey(stock.LocationCode))
                        {
                            tempGroupedStock.Add(stock.LocationCode, new List<RawLiveStock>());
                        }
                        tempGroupedStock[stock.LocationCode].Add(stock);
                    }
                }
                catch (Exception e)
                {
                    //need to write log 
                }
                return tempGroupedStock;
            }
            return null;
        }

        private IDictionary<int, SOLineItemData> initSODetails(DataTable dtSODetails)
        {

            IDictionary<int, SOLineItemData> SOInfoTemp = new Dictionary<int, SOLineItemData>();
            try
            {

                foreach (DataRow drLineItemDetails in dtSODetails.Rows)
                {

                    SOLineItemData soDataLine = new SOLineItemData();
                    soDataLine.ConvesionValue = (decimal)drLineItemDetails["ConvesionValue"];
                    soDataLine.KitPlannerID = (int)drLineItemDetails["KitPlannerID"];
                    soDataLine.LineNo = (int)drLineItemDetails["LineNumber"];

                    soDataLine.MaterialID = (int)drLineItemDetails["MaterialMasterID"];
                    soDataLine.MCode = (String)drLineItemDetails["MCode"];
                    soDataLine.MDesc = drLineItemDetails["MDescription"].ToString();

                    soDataLine.SODetailsID = (int)drLineItemDetails["SODetailsID"];
                    soDataLine.SOID = (int)drLineItemDetails["SOHeaderID"];
                    soDataLine.SO_QTY = (decimal)drLineItemDetails["SOQuantity"];
                    soDataLine.SO_UOM = (String)drLineItemDetails["IUoM"];
                    soDataLine.SO_UOM_QTY = (decimal)drLineItemDetails["IUoMQty"];

                    soDataLine.Base_UOM = (String)drLineItemDetails["BUoM"];
                    soDataLine.Base_UOM_QTY = (decimal)drLineItemDetails["BUoMQty"];

                    soDataLine.StockData = new List<LiveStockData>();
                    string MaterialStoregeValues = drLineItemDetails["MSPValues"].ToString();

                    if (MaterialStoregeValues != null && MaterialStoregeValues.Length > 0)
                    {
                        soDataLine.RequestedSOMSP = new Dictionary<int, String>();
                        foreach (String Value in MaterialStoregeValues.Split(','))
                        {
                            string[] ParamData = Value.Split('|');
                            if (ParamData.Length == 2)
                                soDataLine.RequestedSOMSP.Add(Convert.ToInt16(ParamData[0]), ParamData[1]);
                        }

                    }

                    SOInfoTemp.Add(soDataLine.SODetailsID, soDataLine);
                }
                return SOInfoTemp;
            }
            catch (Exception ex)
            {
                //Log Writing is required here.
                CommonLogic.createErrorNode("Manikanta", "OptimalPickNote", "initSODetails", "SO Details Initialtion", ex.StackTrace);
                return null;
            }
        }


        private void removeDamagedItemsFromSOData()
        {
            IDictionary<int, SOLineItemData> tempData = null;
            tempData = new Dictionary<int, SOLineItemData>();
            tempData = SOInfo;

            for (int i = 0; i < tempData.Count; i++)
            {
                var item = tempData.ElementAt(i);

                foreach (LiveStockData lsd in item.Value.StockData)
                {
                    if (lsd.HasDiscrepency == 1 || lsd.IsDamaged == 1)
                    {
                        string tempText = lsd.NoOfUnits + "|" + lsd.batchNo + "|" + lsd.availQty + "|" + lsd.Quantity + "|" + lsd.HUBox;
                        SOInfo[lsd.SODetailsID].StockData = updateStockInformation(SOInfo[lsd.SODetailsID].StockData, tempText);
                    }
                }
            }

        }

        private IList<LiveStockData> updateStockInformation(IList<LiveStockData> SODetails_Stock, string damagedItemStr)
        {
            IList<LiveStockData> liveStockList = new List<LiveStockData>();

            int noOfUnits = Convert.ToInt16(damagedItemStr.Split('|')[0]);
            string batchNo = damagedItemStr.Split('|')[1];
            decimal availQty = Convert.ToDecimal(damagedItemStr.Split('|')[2]);
            decimal Quantity = Convert.ToDecimal(damagedItemStr.Split('|')[3]);
            int damagedBox = Convert.ToInt16(damagedItemStr.Split('|')[4]);

            IDictionary<int, decimal> eliminateDamagedQuantity = new Dictionary<int, decimal>();
            decimal stockAdjust = 0.0m;

            foreach (LiveStockData liveStock in SODetails_Stock)
            {

                if (stockAdjust < noOfUnits * availQty)
                {

                    if (liveStock.batchNo.Equals(batchNo) && liveStock.NoOfUnits == noOfUnits && liveStock.HUBox != damagedBox)
                    {

                        if (!eliminateDamagedQuantity.ContainsKey(liveStock.HUBox))
                            eliminateDamagedQuantity.Add(liveStock.HUBox, 0);
                        

                        if (eliminateDamagedQuantity[liveStock.HUBox] < availQty)
                        {
                            decimal tempEliminateQty = (liveStock.availQty >= (availQty - eliminateDamagedQuantity[liveStock.HUBox])) ? availQty - eliminateDamagedQuantity[liveStock.HUBox] : liveStock.availQty;
                            liveStock.availQty = (liveStock.availQty >= (availQty - eliminateDamagedQuantity[liveStock.HUBox])) ? liveStock.availQty - (availQty - eliminateDamagedQuantity[liveStock.HUBox]) : 0;
                            liveStock.Quantity = (liveStock.Quantity >= (Quantity - eliminateDamagedQuantity[liveStock.HUBox])) ? liveStock.Quantity - (Quantity - eliminateDamagedQuantity[liveStock.HUBox]) : 0;
                            eliminateDamagedQuantity[liveStock.HUBox] += tempEliminateQty;
                            stockAdjust += tempEliminateQty;

                            if (liveStock.availQty == 0 && liveStock.PickedQty ==0)
                                continue;
                        }
                    }
                    else if (liveStock.HUBox == damagedBox && liveStock.batchNo.Equals(batchNo) && liveStock.NoOfUnits == noOfUnits && (liveStock.HasDiscrepency == 1 || liveStock.IsDamaged == 1))
                    {
                        stockAdjust += availQty;
                        liveStock.availQty = 0;
                        liveStock.Quantity = 0;

                        if (liveStock.availQty == 0 && liveStock.PickedQty == 0)
                            continue;
                    }
                }
                liveStockList.Add(liveStock);
            }
            return liveStockList;
        }

        private void updateSODataWithAvailStock(DataTable dtAvailStock)
        {

            foreach (DataRow drActiveDetails in dtAvailStock.Rows)
            {

                LiveStockData availableStock = new LiveStockData();
                int SODetailsID = (int)drActiveDetails["SODetailsID"];

                if (!SOInfo.ContainsKey(SODetailsID))
                    continue;

                availableStock.GoodsMovementID = (int)drActiveDetails["GoodsMovementDetailsID"];
                availableStock.HUBox = 1;// (int)drActiveDetails["HandlingUnit"];
                availableStock.HasDiscrepency = Convert.ToInt32(drActiveDetails["HasDiscrepancy"]);
                availableStock.IsDamaged = Convert.ToInt32(drActiveDetails["IsDamaged"]);
                availableStock.KitPlannerID = (int)drActiveDetails["KitPlannerID"];
                availableStock.LocationCode = (String)drActiveDetails["Location"];
                availableStock.NoOfUnits = 1;// (int)drActiveDetails["NoofUnits"];

                availableStock.MSPValues = drActiveDetails["MSPValues"].ToString();
                availableStock.PickedQty = (decimal)drActiveDetails["PickedQty"];
                availableStock.LocationID = (int)drActiveDetails["LocationID"];
                availableStock.SODetailsID = (int)drActiveDetails["SODetailsID"];
                availableStock.POSODetailsID = (int)drActiveDetails["POSODetailsID"];
                availableStock.Quantity = (decimal)drActiveDetails["Quantity"];
                availableStock.availQty = (decimal)drActiveDetails["Quantity"];
                availableStock.batchNo = drActiveDetails["Batch"].ToString();
                availableStock.TransactionDocID = (int)drActiveDetails["TransactionDocID"];
                availableStock.WarehouseID = (int)drActiveDetails["WarehouseID"];
                string MaterialStoregeValues = drActiveDetails["MSPValues"].ToString();

                IDictionary<int, string> availableStockMaterialStorageValues = null;

                if (MaterialStoregeValues != null && MaterialStoregeValues.Length > 0)
                {
                    availableStockMaterialStorageValues = new Dictionary<int, String>();
                    foreach (String Value in MaterialStoregeValues.Split(','))
                    {
                        string[] paramData = Value.Split('|');
                        if (paramData.Length == 2)
                            availableStockMaterialStorageValues.Add(Convert.ToInt16(paramData[0]), paramData[1]);
                    }
                    availableStock.ItemMSPInfo = availableStockMaterialStorageValues;
                }

                availableStock.isMatchedMSP = true;
                IDictionary<int, string> soDetailsMaterialStorageValues = SOInfo[SODetailsID].RequestedSOMSP;

                if (soDetailsMaterialStorageValues != null && soDetailsMaterialStorageValues.Count > 0)
                {

                    if (availableStockMaterialStorageValues != null && availableStockMaterialStorageValues.Count > 0)
                    {
                        foreach (KeyValuePair<int, String> MSPValue in soDetailsMaterialStorageValues)
                        {
                            if (!availableStockMaterialStorageValues.ContainsKey(MSPValue.Key) || availableStockMaterialStorageValues[MSPValue.Key].Trim() != MSPValue.Value)
                            {
                                availableStock.isMatchedMSP = false;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        availableStock.isMatchedMSP = false;
                        continue;
                    }
                }

                SOInfo[SODetailsID].StockData.Add(availableStock);
            }

        }

        private IDictionary<string, IDictionary<int, decimal>> getPickedUpdates(IList<LiveStockData> pickListInfo)
        {
            IDictionary<string, IDictionary<int, decimal>> tempAdjustQuantity = new Dictionary<string, IDictionary<int, decimal>>();
            foreach (LiveStockData availStock in pickListInfo)
            {
                if (availStock.PickedQty != 0)
                {
                    string BatchTempKey = availStock.batchNo.Trim() + '^' + availStock.NoOfUnits;
                    if (!tempAdjustQuantity.ContainsKey(BatchTempKey))
                    {
                        tempAdjustQuantity.Add(BatchTempKey, new Dictionary<int, decimal>());
                        tempAdjustQuantity[BatchTempKey].Add(availStock.HUBox, availStock.PickedQty);
                    }
                    else
                    {
                        if (!tempAdjustQuantity[BatchTempKey].ContainsKey(availStock.HUBox))
                            tempAdjustQuantity[BatchTempKey].Add(availStock.HUBox, availStock.PickedQty);
                        else
                            tempAdjustQuantity[BatchTempKey][availStock.HUBox] += availStock.PickedQty;

                    }
                }
            }

            return tempAdjustQuantity;
        }

        private bool lineItemPickStatus(decimal so_quantity, IDictionary<string, IDictionary<int, decimal>> qtyAdjust)
        {
            decimal pickedQuantity = 0.0m;
            if (qtyAdjust != null && qtyAdjust.Count() > 0)
            {
                for (int i = 0; i < qtyAdjust.Count; i++)
                {
                    var itemPicked = qtyAdjust.ElementAt(i);
                    int NO_OF_BOX = Convert.ToInt16(itemPicked.Key.Split('^')[1]);
                    if (NO_OF_BOX == itemPicked.Value.Count())
                    {
                        decimal box_wise_picked_quantity = 0.0m;
                        for (int x = 0; x < itemPicked.Value.Count(); x++)
                        {
                            var itemPickedInner = itemPicked.Value.ElementAt(x);
                            box_wise_picked_quantity += itemPickedInner.Value;
                        }
                        pickedQuantity += box_wise_picked_quantity / NO_OF_BOX;
                    }
                }
            }
            return pickedQuantity == so_quantity;
        }

        private decimal lineItemPickInit(decimal so_quantity, IDictionary<string, IDictionary<int, decimal>> qtyAdjust)
        {
            decimal pickedQuantity = 0.0m;
            if (qtyAdjust != null && qtyAdjust.Count() > 0)
            {
                for (int i = 0; i < qtyAdjust.Count; i++)
                {
                    var itemPicked = qtyAdjust.ElementAt(i);
                    {
                        decimal box_wise_picked_quantity = 0.0m;
                        for (int x = 0; x < itemPicked.Value.Count(); x++)
                        {
                            var itemPickedInner = itemPicked.Value.ElementAt(x);
                            box_wise_picked_quantity = box_wise_picked_quantity > itemPickedInner.Value ? box_wise_picked_quantity : itemPickedInner.Value;
                        }
                        pickedQuantity += box_wise_picked_quantity ;
                    }
                }
            }
            return pickedQuantity;
        } 

        private IList<RawLiveStock> getOptimalRouteWiseData()
        {

            try
            {
                IDictionary<int, decimal> requiredStock = new Dictionary<int, decimal>();
                IDictionary<int, decimal> considerStock = new Dictionary<int, decimal>();

                for (int SOIndex = 0; SOIndex < SOInfo.Count; SOIndex++)
                {

                    var item = SOInfo.ElementAt(SOIndex);
                    IDictionary<string, IDictionary<int, decimal>> qtyAdjust = null;
                    IDictionary<string, decimal> pickedTeamAdjust = null;
                    
                    decimal distinct_initiation = 0.0m;

                    if (IsStockRestrict)
                    {
                        requiredStock.Add(item.Key, item.Value.SO_QTY);
                        considerStock.Add(item.Key, 0.0m);
                        qtyAdjust = getPickedUpdates(item.Value.StockData);
                        if (lineItemPickStatus(item.Value.SO_QTY, qtyAdjust))
                        {
                            continue;
                        }
                        else
                        {

                            if (qtyAdjust != null && qtyAdjust.Count() > 0)
                            {
                                distinct_initiation = lineItemPickInit(item.Value.SO_QTY, qtyAdjust);
                                item.Value.StockData = item.Value.StockData.OrderByDescending(o => o.PickedQty).ToList();
                                
                                pickedTeamAdjust = new Dictionary<string, decimal>();

                                if (qtyAdjust != null && qtyAdjust.Count() > 0)
                                {
                                    for (int i = 0; i < qtyAdjust.Count; i++)
                                    {
                                        var itemPicked = qtyAdjust.ElementAt(i);
                                        {
                                            decimal box_wise_picked_quantity = 0.0m;
                                            
                                            for (int x = 0; x < itemPicked.Value.Count(); x++)
                                            {
                                                var itemPickedInner = itemPicked.Value.ElementAt(x);
                                                box_wise_picked_quantity = box_wise_picked_quantity > itemPickedInner.Value ? box_wise_picked_quantity : itemPickedInner.Value;
                                            }
                                            pickedTeamAdjust.Add(itemPicked.Key,box_wise_picked_quantity);
                                        }
                                    }
                                }
                                qtyAdjust = new Dictionary<string, IDictionary<int, decimal>>();
                            }
                        }
                    }

                    foreach (LiveStockData availStock in item.Value.StockData)
                    {

                        if (IsStockRestrict)
                        {
                            if (requiredStock[item.Key] == considerStock[item.Key])
                                break;

                            if (requiredStock[item.Key] == considerStock[item.Key] || availStock.HasDiscrepency == 1 || availStock.IsDamaged == 1)
                                continue;
                        }

                        RawLiveStock liveStock = new RawLiveStock();
                        string BatchTempKey = availStock.batchNo.Trim() + '^' + availStock.NoOfUnits;
                        liveStock.PickedQty = availStock.PickedQty;
                        liveStock.Quantity = availStock.Quantity;
                        liveStock.AvailQty = availStock.availQty;
                        liveStock.pickableQty = 0;
                        liveStock.Mcode = item.Value.MCode;
                        liveStock.MDesc = item.Value.MDesc;
                        liveStock.MaterialID = item.Value.MaterialID;
                        liveStock.isMatchedMSP = availStock.isMatchedMSP;

                        //this block for suggested pick note generation 
                        if (IsStockRestrict)
                        {
                            if (availStock.availQty != 0)
                            {
                                decimal holdQuantity = 0.0m;
                                for (int x = 0; x < qtyAdjust.Count; x++)
                                {
                                    var itemBatch = qtyAdjust.ElementAt(x);
                                    decimal maxQty = 0.0m;
                                    for (int y = 0; y < itemBatch.Value.Count; y++)
                                    {
                                        var itemBatchHUBox = itemBatch.Value.ElementAt(y);
                                        if (y == 0)
                                            maxQty = itemBatchHUBox.Value;
                                        else
                                            maxQty = maxQty >= itemBatchHUBox.Value ? maxQty : itemBatchHUBox.Value;
                                    }
                                    holdQuantity += maxQty;
                                }

                                if (requiredStock[item.Key] > considerStock[item.Key])
                                {
                                    decimal MaxBatchItemFormQTY = 0.0m;
                                    if (!qtyAdjust.ContainsKey(BatchTempKey))
                                    {
                                        if (holdQuantity == requiredStock[item.Key])
                                            continue;
                                        qtyAdjust.Add(BatchTempKey, new Dictionary<int, decimal>());
                                        qtyAdjust[BatchTempKey].Add(availStock.HUBox, 0);

                                    }
                                    else
                                    {
                                        if (!qtyAdjust[BatchTempKey].ContainsKey(availStock.HUBox))
                                            qtyAdjust[BatchTempKey].Add(availStock.HUBox, 0);
                                        
                                        for (int tempX = 0; tempX < qtyAdjust[BatchTempKey].Count; tempX++)
                                        {
                                            var batchItemForm = qtyAdjust[BatchTempKey].ElementAt(tempX);
                                            if (tempX == 0)
                                                MaxBatchItemFormQTY = batchItemForm.Value;
                                            else
                                                MaxBatchItemFormQTY = MaxBatchItemFormQTY >= batchItemForm.Value ? MaxBatchItemFormQTY : batchItemForm.Value;
                                        }

                                        if (holdQuantity == requiredStock[item.Key] && MaxBatchItemFormQTY == qtyAdjust[BatchTempKey][availStock.HUBox])
                                            continue;
                                    }



                                    if (MaxBatchItemFormQTY == 0 && distinct_initiation == 0)
                                        MaxBatchItemFormQTY = requiredStock[item.Key] - holdQuantity;
                                    else if (distinct_initiation != 0)
                                        MaxBatchItemFormQTY = (pickedTeamAdjust != null && pickedTeamAdjust.ContainsKey(BatchTempKey)) ? pickedTeamAdjust[BatchTempKey] : (requiredStock[item.Key] - distinct_initiation);


                                    if (requiredStock[item.Key] != liveStock.PickedQty)
                                        liveStock.pickableQty = availStock.availQty >= (MaxBatchItemFormQTY - qtyAdjust[BatchTempKey][availStock.HUBox]) ? (MaxBatchItemFormQTY - qtyAdjust[BatchTempKey][availStock.HUBox]) : availStock.availQty;

                                    if ((liveStock.PickedQty) != 0 && (liveStock.pickableQty != 0))
                                        liveStock.pickableQty = liveStock.PickedQty >= liveStock.pickableQty ? 0.0m : liveStock.pickableQty - liveStock.PickedQty;
                                                                                                            
                                    if ((distinct_initiation == requiredStock[item.Key]) && (requiredStock[item.Key] == liveStock.PickedQty))
                                        liveStock.pickableQty = 0;
                                        
                                }

                                if (liveStock.PickedQty != 0)
                                    qtyAdjust[BatchTempKey][availStock.HUBox] += liveStock.PickedQty;

                                qtyAdjust[BatchTempKey][availStock.HUBox] += liveStock.pickableQty;
                            }
                            else
                            {
                                if (availStock.PickedQty != 0)
                                {
                                    if (!qtyAdjust.ContainsKey(BatchTempKey))
                                        qtyAdjust.Add(BatchTempKey, new Dictionary<int, decimal>());

                                    if (!qtyAdjust[BatchTempKey].ContainsKey(availStock.HUBox))
                                        qtyAdjust[BatchTempKey].Add(availStock.HUBox, availStock.PickedQty);

                                }
                                liveStock.pickableQty = 0;
                            }

                            if (qtyAdjust[BatchTempKey].Count() == availStock.NoOfUnits)
                            {
                                decimal holdQty = 0.0m;
                                decimal minQty = 0.0m;

                                for (int ind = 0; ind < qtyAdjust[BatchTempKey].Count(); ind++)
                                {
                                    var itemTmp = qtyAdjust[BatchTempKey].ElementAt(ind);
                                    holdQty += itemTmp.Value;
                                    if (ind == 0)
                                        minQty = itemTmp.Value;
                                    else
                                        minQty = minQty > itemTmp.Value ? itemTmp.Value : minQty;

                                }

                                if (holdQty != 0 && (holdQty % availStock.NoOfUnits) == 0)
                                    considerStock[item.Key] += minQty;
                            }

                        }


                        liveStock.GoodsMovementID = availStock.GoodsMovementID;
                        liveStock.HUBox = availStock.HUBox;
                        liveStock.HasDiscrepency = availStock.HasDiscrepency;

                        liveStock.IsDamaged = availStock.IsDamaged;
                        liveStock.KitPlannerID = availStock.KitPlannerID;
                        liveStock.LocationCode = availStock.LocationCode;

                        liveStock.LocGenID = getLocationGenID(liveStock.LocationCode.Trim());
                        liveStock.KitPlannerID = availStock.KitPlannerID;
                        liveStock.NoOfUnits = availStock.NoOfUnits;
                        liveStock.MSPValues = availStock.MSPValues;

                        liveStock.LocationID = availStock.LocationID;
                        liveStock.SODetailsID = availStock.SODetailsID;
                        liveStock.POSODetailsID = availStock.POSODetailsID;
                        liveStock.BatchNo = availStock.batchNo;
                        liveStock.TransactionDocID = availStock.TransactionDocID;
                        liveStock.WarehouseID = availStock.WarehouseID;
                        liveStockInfo.Add(liveStock);
                    }

                }

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode("Manikanta", "getOptimalRouteWiseData", "OptimalPickNote", "getting optimal pick list", ex.StackTrace);
            }
            return liveStockInfo;

        }




        private int getLocationGenID(string LocCode)
        {
            int GeneratedID = 0;

            /*
                <Rack>-<Phase/Zone>-<Columns/Bay>-<Height/Row>-<Bin>
                    01 A 01 A 0
                \d{2}[A-Z]{1}\d{2}[A-Z]{1}[0-3]{1}
            
             */

            if (LocCode.Length == 7)
            {
                //LocationCode s1 01 W 03

                int rack = Convert.ToInt16(LocCode.Substring(2, 2));
                int zone = (int)LocCode.Substring(0, 1).ToCharArray()[0];
                int col = 1;// Convert.ToInt16(LocCode.Substring(3, 2));
                int row = (int)LocCode.Substring(5, 1).ToCharArray()[0];
                int bin = Convert.ToInt16(LocCode.Substring(5, 2));
                
                return rack * 10000 + zone * 100000 + ((rack % 2) == 0 ? 2000 - (col * 20) : 2000 + (col * 20)) + (row * 2) + bin;
            }

            return GeneratedID;
        }



    }
}