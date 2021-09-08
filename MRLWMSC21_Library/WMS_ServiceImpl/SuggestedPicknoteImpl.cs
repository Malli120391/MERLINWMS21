using MRLWMSC21_Library.WMS_DBCommon;
using MRLWMSC21_Library.WMS_ServiceObjects;
using MRLWMSC21_Library.WMS_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceImpl
{


    internal class SuggestedPicknoteImpl : WMSService
    {
        delegate object SuggestedPicknote(object input);

        public object Process(string FUNC_EXEC_CODE, object input)
        {
            try
            {
                if (ServicePool.ServiceImplRef.ContainsKey(FUNC_EXEC_CODE.Trim()) || ServicePool.ServiceImplRef[FUNC_EXEC_CODE].Trim() != "")
                {
                    MethodInfo methodInfo = typeof(SuggestedPicknoteImpl).GetMethod(ServicePool.ServiceImplRef[FUNC_EXEC_CODE]);
                    SuggestedPicknote dataAccess = (SuggestedPicknote)Delegate.CreateDelegate(typeof(SuggestedPicknote), null, methodInfo);
                    return dataAccess(input);
                }
                else
                {
                    throw new ServiceException(10, "the requested FUNC_EXEC_CODE not implemented under SuggestedPutawayImpl. [" + FUNC_EXEC_CODE + "]");
                }
            }
            catch (ServiceException se)
            {
                throw se;
            }
        }


        public object GetSuggestedPicknote(object input)
        {
            AvailableStockData availStock = null;
            IDictionary<string, object> PICK_CONFIG = ServicePool.wms_config.GetPickingConfig();
            try
            {
                if (input != null)
                {
                    availStock = (AvailableStockData)input;
                    if (availStock != null && availStock.orders.Count > 0)
                    {
                        PickNoteManager picknote = new PickNoteManager(PICK_CONFIG, availStock.MSP);
                        for (int index = 0; index < availStock.orders.Count; index++)
                        {
                            var item = availStock.orders.ElementAt(index);
                            availStock.orders[item.Key] = picknote.GetSuggestedPickSlip(item.Value);
                        }
                    }

                    IList<SuggestedStockInfo> suggestedStockList = GetSortedStockInfo(availStock.orders);
                    if (suggestedStockList != null && suggestedStockList.Count > 0)
                    {
                        availStock.suggestedStockData = (PICK_CONFIG.ContainsKey("SNAKE_PATH") && PICK_CONFIG["SNAKE_PATH"].ToString().Equals("1")) ? suggestedStockList.OrderBy(O => O.LocationSysGenId).ToList() : suggestedStockList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException(13, "Exception while getting suggested bins to pick items: " + ex.Message);
            }
            return availStock;
        }


        public object GetRestrictedPicknote(object input)
        {
            AvailableStockData availStock = null;
            IDictionary<string, object> PICK_CONFIG = ServicePool.wms_config.GetPickingConfig();
            try
            {
                if (input != null)
                {
                    availStock = (AvailableStockData)input;

                    if (availStock != null && availStock.orders.Count > 0)
                    {
                        PickNoteManager picknote = new PickNoteManager(PICK_CONFIG, availStock.MSP);
                        for (int index = 0; index < availStock.orders.Count; index++)
                        {
                            var item = availStock.orders.ElementAt(index);
                            availStock.orders[item.Key] = picknote.GetRestrictedPickSlip(item.Value);
                        }
                    }

                    IList<SuggestedStockInfo> suggestedStockList = GetSortedStockInfo(availStock.orders);
                    if (suggestedStockList != null && suggestedStockList.Count > 0)
                    {
                        availStock.suggestedStockData = (PICK_CONFIG.ContainsKey("SNAKE_PATH") && PICK_CONFIG["SNAKE_PATH"].ToString().Equals("1")) ? suggestedStockList.OrderBy(O => O.LocationSysGenId).ToList() : suggestedStockList;
                    }

                    /*
                    if (PICK_CONFIG.ContainsKey("FEFO") && PICK_CONFIG["FEFO"].ToString().Equals("1"))
                    {
                        soline.AvailStock = soline.AvailStock.OrderByDescending(O => O.InitiatedQTY).ThenBy(Obj => Obj.dummyExpiry).ToList();
                    }*/

                }
            }
            catch (Exception ex)
            {
                throw new ServiceException(13, "Exception while getting suggested bins to pick items: " + ex.Message);
            }
            return availStock;
        }





        private IList<SuggestedStockInfo> GetSortedStockInfo(IDictionary<int, SalesOrder> orders)
        {
            IList<SuggestedStockInfo> suggestedStockList = new List<SuggestedStockInfo>();

            for (int index = 0; index < orders.Count; index++)
            {
                var item = orders.ElementAt(index);
                foreach (LineItem lineItem in item.Value.SOLineItems)
                {
                    foreach (SuggestedLocation suggestedLoc in lineItem.SuggestedLocations)
                    {
                        SuggestedStockInfo suggestedStock = new SuggestedStockInfo();
                        suggestedStock.InitiatedQTY = suggestedLoc.InitiatedQTY;
                        suggestedStock.SuggestedQTY = suggestedLoc.SuggestedQTY;
                        suggestedStock.IsMatchedMSP = suggestedLoc.IsMatchedMSP;
                        suggestedStock.MSPS = suggestedLoc.MSPS;
                        suggestedStock.LocCode = suggestedLoc.LocationCode;
                        suggestedStock.LocId = suggestedLoc.LocationID;
                        suggestedStock.LocationSysGenId = suggestedLoc.LocationSysGenId;
                        suggestedStock.LineNo = lineItem.LineSeqId;
                        suggestedStock.OutboundId = item.Value.OutboundId;
                        suggestedStock.MaterialCode = lineItem.MaterialCode;
                        suggestedStock.MaterialDesc = lineItem.MDesc;
                        suggestedStock.MaterialId = lineItem.MaterialID;

                        suggestedStock.SODetailsId = lineItem.SODetailsID;
                        suggestedStock.SOId = item.Key;
                        suggestedStock.SONumber = item.Value.SONumber;
                        suggestedStock.SOQTY = lineItem.SO_QTY;
                        suggestedStock.SOUOMQTY = lineItem.SO_UOM_QTY;
                        suggestedStock.SOUOM = lineItem.SO_UOM;
                        suggestedStock.SOUOMId = lineItem.SO_UOMID;
                        suggestedStock.QTY = suggestedLoc.QTY;

                        suggestedStock.IsDamaged = suggestedLoc.IsDamaged;
                        suggestedStock.HasDiscrepancy = suggestedLoc.HasDiscrepancy;
                        suggestedStock.KitPlannerId = suggestedLoc.KitPlannerId;

                        suggestedStockList.Add(suggestedStock);
                    }
                }
            }

            return suggestedStockList;

        }

    }

    internal class PickNoteManager
    {

        IDictionary<string, object> PICK_CONFIG;
        IDictionary<int, MSP> MSP;

        public PickNoteManager(IDictionary<string, object> config, IDictionary<int, MSP> MSPS)
        {
            PICK_CONFIG = config;
            MSP = MSPS;
        }


        public SalesOrder GetSuggestedPickSlip(SalesOrder SO)
        {
            if (SO != null && SO.SOLineItems.Count > 0)
            {
                foreach (LineItem soline in SO.SOLineItems)
                {
                    soline.SuggestedLocations = new List<SuggestedLocation>();

                    foreach (MaterialStockInfo mstock in soline.AvailStock)
                    {

                        //skip damaged items 
                        if (PICK_CONFIG.ContainsKey("DAMAGED_GOOD_SUGGESTION") && mstock.IsDamaged && !PICK_CONFIG["DAMAGED_GOOD_SUGGESTION"].ToString().Equals("1"))
                            continue;

                        SuggestedLocation suggestedLocation = getSuggestedLocation(soline, mstock);

                        if (suggestedLocation != null)
                            soline.SuggestedLocations.Add(suggestedLocation);
                    }
                }
            }
            return SO;
        }


        private SuggestedLocation getSuggestedLocation(LineItem soline, MaterialStockInfo mstock)
        {
            if (soline != null && mstock != null)
            {


                SuggestedLocation suggestedLocation = new SuggestedLocation();
                suggestedLocation.LocationID = mstock.LocationSysId;
                suggestedLocation.LocationCode = mstock.LocationCode;
                suggestedLocation.LocationSysGenId = GenerateLocationSerialNo(mstock.LocationCode);
                suggestedLocation.InitiatedQTY = mstock.InitiatedQTY;
                suggestedLocation.SuggestedQTY = 0;
                suggestedLocation.QTY = mstock.QTY;
                suggestedLocation.IsDamaged = mstock.IsDamaged;
                suggestedLocation.HasDiscrepancy = mstock.HasDiscrepancy;
                suggestedLocation.KitPlannerId = mstock.KitPlannerId;
                suggestedLocation.MSPS = mstock.MSPS;

                if (soline.MSPS != null && soline.MSPS.Count > 0 && soline.IsMSPRequest && mstock.MSPS != null && mstock.MSPS.Count > 0)
                {

                    bool tempMspMatch = true;
                    foreach (MSP msp in mstock.MSPS)
                    {
                        if (soline.MSPS.ContainsKey(msp.MSPSysId))
                            tempMspMatch = (msp.NativeValue.Trim() == soline.MSPS[msp.MSPSysId].NativeValue.Trim());
                        else if (soline.MSPS != null && soline.MSPS.Count > 0)
                            tempMspMatch = false;

                        if (!tempMspMatch)
                            break;
                    }
                    suggestedLocation.IsMatchedMSP = tempMspMatch;
                }
                else
                {
                    suggestedLocation.IsMatchedMSP = true;
                }
                return suggestedLocation;
            }
            else
            {
                return null;
            }
        }




        public SalesOrder GetRestrictedPickSlip(SalesOrder SO)
        {
            if (SO != null && SO.SOLineItems.Count > 0)
            {
                IDictionary<int, decimal> SOLinePickableQTY = new Dictionary<int, decimal>();
                IDictionary<int, decimal> SOLineRequiredQTY = new Dictionary<int, decimal>();

                foreach (LineItem soline in SO.SOLineItems)
                {
                    if (!SOLineRequiredQTY.ContainsKey(soline.SODetailsID))
                    {
                        SOLineRequiredQTY.Add(soline.SODetailsID, soline.SO_QTY);
                        SOLinePickableQTY.Add(soline.SODetailsID, 0);
                    }
                    decimal holdQuantity = soline.processedDocQty;

                    if (soline.SuggestedLocations == null)
                        soline.SuggestedLocations = new List<SuggestedLocation>();


                    //EXPIRY_GOODS_SUGGESTION
                    if (soline.SO_QTY > soline.processedDocQty)
                    {
                        //Sort the line items as per the Configuration
                        //FEFO APPROACH BASED ON CONFIGURATION priority-1
                        //FIFO approach based on config information priority-2

                        List<MaterialStockInfo> stockTemp = soline.AvailStock.ToList();
                        if (PICK_CONFIG.ContainsKey("FEFO") && PICK_CONFIG["FEFO"].ToString().Equals("1"))
                        {
                            //(a, b) => a.CompareTo(b)
                            //stockTemp = stockTemp.OrderBy(O => O.dummyExpiry).ToList();
                            stockTemp.Sort((x, y) => x.dummyExpiry.CompareTo(y.dummyExpiry));
                            stockTemp = stockTemp.OrderByDescending(O => O.InitiatedQTY).ToList();
                        }
                        else
                        {
                            if (PICK_CONFIG.ContainsKey("FIFO") && PICK_CONFIG["FIFO"].ToString().Equals("1"))
                                stockTemp = stockTemp.OrderByDescending(O => O.InitiatedQTY).ThenBy(Obj => Obj.SysTranId).ToList();
                            else
                                stockTemp = stockTemp.OrderByDescending(O => O.InitiatedQTY).ToList();
                        }


                        foreach (MaterialStockInfo mstock in stockTemp)
                        {
                            if (PICK_CONFIG.ContainsKey("DAMAGED_GOOD_SUGGESTION") && mstock.IsDamaged && !PICK_CONFIG["DAMAGED_GOOD_SUGGESTION"].ToString().Equals("1"))
                            {
                                //Damaged goods are will not suggested in restricted pick note
                                //This block execution depends on configuration information,DAMAGED_GOOD_SUGGESTION=false; 
                                if (SOLineRequiredQTY[soline.SODetailsID] > SOLinePickableQTY[soline.SODetailsID])
                                    continue;
                                else
                                    break;
                            }
                            else if (SOLineRequiredQTY[soline.SODetailsID] > SOLinePickableQTY[soline.SODetailsID])
                            {
                                if (PICK_CONFIG.ContainsKey("EXPIRY_GOODS_SUGGESTION") && !PICK_CONFIG["EXPIRY_GOODS_SUGGESTION"].ToString().Equals("1") && mstock.MSPS != null)
                                {
                                    //expiry goods not suggested 
                                    bool isExpiry = false;
                                    foreach (MSP mspCheck in mstock.MSPS)
                                    {
                                        //expiry check
                                        if (mspCheck.MSPSysId == 2 && ((DateTime)(mspCheck.Value)).Date < DateTime.Now.Date)
                                        {
                                            isExpiry = true;
                                            break;
                                        }
                                    }

                                    if (isExpiry)
                                        continue;
                                }

                                SuggestedLocation suggestedLocation = getSuggestedLocation(soline, mstock);
                                if (suggestedLocation != null)
                                {
                                    suggestedLocation.SuggestedQTY = mstock.QTY <= (SOLineRequiredQTY[soline.SODetailsID] - holdQuantity) ? mstock.QTY : (SOLineRequiredQTY[soline.SODetailsID] - holdQuantity);
                                    holdQuantity += suggestedLocation.SuggestedQTY;
                                    SOLinePickableQTY[soline.SODetailsID] += suggestedLocation.SuggestedQTY + suggestedLocation.InitiatedQTY;
                                    soline.SuggestedLocations.Add(suggestedLocation);
                                }
                            }
                            else
                            {
                                //no need of additional quantity once restricted quantity is logically hold
                                break;
                            }
                        }
                    }
                }
            }
            return SO;
        }



        private int GenerateLocationSerialNo(string code)
        {
            //S1 01 D 01 
            if (code.Length == 7)
            {
                int zone = (int)Convert.ToChar(code.Substring(0, 1));
                int rack = Convert.ToInt16(code.Substring(2, 2));
                int bay = (int)Convert.ToChar(code.Substring(4, 1));
                int bin = Convert.ToInt32(code.Substring(5, 2));
                return (zone * 100000) + (rack * 1000) + (bay * 100) + (bin * 10);
            }
            return 0;
        }

    }

}
