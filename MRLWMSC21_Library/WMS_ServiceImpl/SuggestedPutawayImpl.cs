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
    class SuggestedPutawayImpl : WMSService
    {
        delegate object SuggestedPutaway(object input);

        public object Process(string FUNC_EXEC_CODE, object input)
        {
            try
            {

                if (ServicePool.ServiceImplRef.ContainsKey(FUNC_EXEC_CODE.Trim()) || ServicePool.ServiceImplRef[FUNC_EXEC_CODE].Trim() != "")
                {
                    MethodInfo methodInfo = typeof(SuggestedPutawayImpl).GetMethod(ServicePool.ServiceImplRef[FUNC_EXEC_CODE]);
                    SuggestedPutaway dataAccess = (SuggestedPutaway)Delegate.CreateDelegate(typeof(SuggestedPutaway), null, methodInfo);
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



        public object GetSuggestedPutawayInfo(object input)
        {
            //type casting based on resource suggestion
            IDictionary<int, SupplierInvoice> supplierInv = null;
            IDictionary<int, SupplierInvoice> invoiceSuggestedBins = new Dictionary<int, SupplierInvoice>();
            Inbound inbound = null;
            if (input != null)
            {
                inbound = (Inbound)input;
                supplierInv = inbound.invoices;
                for (int index = 0; index < supplierInv.Count; index++)
                {
                    var supplierInvoice = supplierInv.ElementAt(index);
                    invoiceSuggestedBins.Add(supplierInvoice.Key, prepareSuggestedPutaway(supplierInvoice.Value));
                }
                inbound.invoices = invoiceSuggestedBins;
            }
            else
            {
                throw new ServiceException(16,"the given input cannot be null");
            }
            
            if(!HoldSuggestedPutawayInfo(inbound))
                throw new ServiceException(17, "Suggested location information not persisted");

            return inbound;
        }


        //root method for suggested putaway
        private SupplierInvoice prepareSuggestedPutaway(SupplierInvoice SI)
        {
           
            SupplierInvoice InvoiceWithSuggestedLocations = new SupplierInvoice();

            if (SI != null && SI.InvoiceLineItems != null && SI.InvoiceLineItems.Count > 0)
            {
               
                InvoiceWithSuggestedLocations = SI;
                IList<LineItem> lineItemWithSuggestedBin = new List<LineItem>();
                IList<LineItem> palletizedItemList = new List<LineItem>();
                IList<LineItem> binLevelItemList = new List<LineItem>();

                bool IsPalletizedEnabled = false;
                if (ServicePool.wms_config.GetPutawayConfig().ContainsKey("PALLET_LOADING"))
                {
                    int value = Convert.ToInt16(ServicePool.wms_config.GetPutawayConfig().ContainsKey("PALLET_LOADING").ToString());
                    IsPalletizedEnabled = (value == 1);
                }

                foreach (LineItem li in SI.InvoiceLineItems)
                {
                    if (!li.GRN_STATUS && (li.INV_QTY - li.processedDocQty) != 0)
                    {
                        if (li.IsPalletized && IsPalletizedEnabled)
                            palletizedItemList.Add(li);
                        else
                            binLevelItemList.Add(li);
                    }
                    else
                    {
                        lineItemWithSuggestedBin.Add(li);
                    }
                }

                if (binLevelItemList.Count > 0)
                {
                    BinLevelPutaway binLevelPutaway = new BinLevelPutaway(SI, binLevelItemList, ServicePool.wms_config.GetPutawayConfig());
                    IList<LineItem> updatedLineItems = binLevelPutaway.getSuggestedPutawayInfo();
                    if (updatedLineItems != null)
                    {
                        foreach (LineItem lineItem in updatedLineItems)
                        {
                            lineItemWithSuggestedBin.Add(lineItem);
                        }
                    }
                    binLevelPutaway = null;
                }

                //palletized putaway 
                if (palletizedItemList.Count > 0)
                {
                    PalletizedPutaway palletizedPutaway = new PalletizedPutaway(SI, palletizedItemList, ServicePool.wms_config.GetPutawayConfig());
                    IList<LineItem> updatedLineItems = palletizedPutaway.getSuggestedPutawayInfo();
                    if (updatedLineItems != null)
                    {
                        foreach (LineItem lineItem in updatedLineItems)
                        {
                            lineItemWithSuggestedBin.Add(lineItem);
                        }
                    }
                    palletizedPutaway = null;
                }
                InvoiceWithSuggestedLocations.InvoiceLineItems = lineItemWithSuggestedBin;
            }
            
            return InvoiceWithSuggestedLocations;
        }


        public bool HoldSuggestedPutawayInfo(object input) 
        {
            object obj = DTOController.DataToStoredProcedure(DMLExecuteCode.HOLD_SUGGESTED_LOCATION,input);
            return (bool)obj;
        }

    }


    internal class PalletizedPutaway
    {
        //getting empty pallets or half filled pallets 
        //pallet and location mapping (COMPLETE EMPTY BIN)
        //same group material same pallet 

        IList<LineItem> palletizedLineItems = null;
        IDictionary<string, object> putawayConfig = null;
        SupplierInvoice SI = null;
        public PalletizedPutaway(SupplierInvoice invoice, IList<LineItem> lineItems, IDictionary<string, object> config)
        {
            SI = invoice;
            palletizedLineItems = lineItems;
            putawayConfig = config;
        }

        public IList<LineItem> getSuggestedPutawayInfo()
        {
            return null;
        }
    }
    
    internal class BinLevelPutaway
    {
        IList<LineItem> lineItems = null;
        IDictionary<string, object> putawayConfig = null;
        SupplierInvoice SI = null;
        IDictionary<int, decimal> logicalBinVolume = null;
        IDictionary<int, decimal> logicalBinWeight = null;
        Boolean FAST_MOVING = false;
        decimal FAST_MOVING_THRESHOLD = 60;

        public BinLevelPutaway(SupplierInvoice invoice, IList<LineItem> items, IDictionary<string, object> config)
        {
            SI = invoice;
            lineItems = items;
            putawayConfig = config;

            //Customer returns 
            if (SI.SupplierSysId == -1)
                SI.SupplierSysId = 0;

            logicalBinVolume = new Dictionary<int, decimal>();
            logicalBinWeight = new Dictionary<int, decimal>();

            FAST_MOVING = putawayConfig.ContainsKey("FAST_MOVING_GOOD") && Convert.ToInt16(putawayConfig["FAST_MOVING_GOOD"].ToString()) == 1;
            FAST_MOVING_THRESHOLD = putawayConfig.ContainsKey("FAST_MOVING_GOOD_THRESHOLD") ? Convert.ToDecimal(putawayConfig["FAST_MOVING_GOOD_THRESHOLD"].ToString()) : 60;
            loadSuggestedBinWeightVolume();

        }

        
        private void loadSuggestedBinWeightVolume() 
        {
            CommonCriteria commCriteria =  new CommonCriteria();
            commCriteria.INBOUNDID = SI.InboundId;
            commCriteria.TENANTID = SI.TenantID;
            commCriteria.SUPPLIERID =SI.SupplierSysId;

            object DBResult = DAOController.GetData(DRLExecuteCode.SUGGESTED_LOCATION_OCCUPANCY, new DBCriteria().SUGGESTED_LOCATION_OCCUPANCY(commCriteria));
            
            if (DBResult != null)
            {
                foreach(LocationOccupancy lo in (IList<LocationOccupancy>)DBResult)
                {
                    logicalBinVolume.Add(lo.LocationID,lo.VolumeOccupancy);
                    logicalBinWeight.Add(lo.LocationID, lo.WeightOccupancy);
                }
            }

        }

        public IList<LineItem> getSuggestedPutawayInfo()
        {
            IList<LineItem> lineItemWithSuggestedBin = new List<LineItem>();
            try
            {
                foreach (LineItem li in lineItems)
                {
                    CommonCriteria criteria = new CommonCriteria() { TENANTID = 0, MATERIAL_BIN = true, MATERIALID = li.MaterialID };
                    Object DBResult = DAOController.GetDataFromStoredProcedure(DRLExecuteCode.MATERIAL_BIN_INFO_BY_MATERIALID, new DBCriteria().MATERIAL_BIN_INFO_BY_MATERIALID(criteria));
                    IList<MaterialBinInfo> materialBins = (IList<MaterialBinInfo>)DBResult;
                    if (DBResult != null && materialBins.Count > 0)
                    {
                        //previous receiving information based bin suggestion
                        materialBins = materialBins.OrderByDescending(o => o.QTY).ThenBy(o => o.TranDate).ToList();
                        LineItem ItemWithSuggestedLocation = prepareSuggestedLocations(li, materialBins, SI.TenantID, SI.SupplierSysId);

                        if (ItemWithSuggestedLocation != null)
                            lineItemWithSuggestedBin.Add(ItemWithSuggestedLocation);
                        else
                            lineItemWithSuggestedBin.Add(li);
                    }
                    else
                    {
                        //choose random locations to putaway, based on quantity
                        //based on location code, zone, material group, material type based location selection
 
                        LineItem ItemWithSuggestedLocation = prepareSuggestedLocations(li, SI.TenantID, SI.SupplierSysId);
                        if (ItemWithSuggestedLocation != null)
                            lineItemWithSuggestedBin.Add(ItemWithSuggestedLocation);
                        else
                            lineItemWithSuggestedBin.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;    
            }
            return lineItemWithSuggestedBin;
        }



        private LineItem prepareSuggestedLocations(LineItem lineItem, IList<MaterialBinInfo> materialBins, int TenantID = 0, int SupplierID = 0)
        {
            LineItem updateLineItem = null;

            if (lineItem != null && materialBins != null && materialBins.Count > 0)
            {
            
                //conversion for CAR/PALLET/EA/BOX
                lineItem.QTY_Base_UOM =  (lineItem.INV_QTY - lineItem.processedDocQty) * lineItem.Base_UOM_QTY * lineItem.INV_UOM_QTY;
                updateLineItem = lineItem;
                updateLineItem.SuggestedLocations = new List<SuggestedLocation>();
                
                //decimal quantityNeedToFill = (lineItem.INV_QTY - lineItem.processedDocQty);
                decimal quantityNeedToFill = lineItem.QTY_Base_UOM;

                MaterialMaster materialSaleInfo = (MaterialMaster)new MaterialServiceImpl().GetMaterialSalesOrderRate(new MaterialMaster() { MaterialId = lineItem.MaterialID,MCode=lineItem.MaterialCode });
                bool IsFastMovingGood = materialSaleInfo.IsFastMovingGood;
                
                //HERE CONVERSION FACTOR MUST BE INVOLVE
                //1) suggesting existed location, already filled locations
                
                foreach (MaterialBinInfo MBinDim in materialBins)
                {
                    if (quantityNeedToFill >= 1)
                    {
                        LocationOccupancy lo = getLocationOccupancy(MBinDim.LocationID);
                        SuggestedLocation suggestedLocation = getSuggestedLocation(lo, lineItem, quantityNeedToFill);
                        if (suggestedLocation != null)
                        {
                            quantityNeedToFill -= suggestedLocation.QTY;
                            updateLineItem.SuggestedLocations.Add(suggestedLocation);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                //need to get nearest empty locations based on location code, to hold this materials
                //looking for nearest locations 
                if (quantityNeedToFill >= 1)
                {
                    foreach (MaterialBinInfo MBinDim in materialBins)
                    {
                        IList<LocationOccupancy> emptyLocs = getNearByLocationByBinCode(fetchActiveZoneCode(MBinDim.LocationCode, "{2}{2}{1}{2}"), TenantID, SupplierID);
                        if (emptyLocs != null && quantityNeedToFill >= 1)
                        {
                            foreach (LocationOccupancy lo in emptyLocs)
                            {
                                if (quantityNeedToFill >= 1)
                                {
                                    SuggestedLocation suggestedLocation = getSuggestedLocation(lo, lineItem, quantityNeedToFill);
                                    if (suggestedLocation != null)
                                    {
                                        quantityNeedToFill -= suggestedLocation.QTY;
                                        updateLineItem.SuggestedLocations.Add(suggestedLocation);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }


                    //WAREHOUSE LEVEL CHECKING FOR EMPTY LOCATIONS
                    if (quantityNeedToFill >= 1)
                    {
                        string warehouseCode = GetWarehouseCode(SI.StoreRefId);
                        Object DBResult = DAOController.GetData(DRLExecuteCode.ZONE_LIST_BY_WAREHOUSE, warehouseCode);
                        IList<WarehouseZone> zones = (IList<WarehouseZone>)DBResult;

                        if (zones != null && zones.Count > 0)
                        {
                            foreach (WarehouseZone zone in zones)
                            {

                                IList<LocationOccupancy> emptyLocs = getNearByLocationByBinCode(zone.ZoneCode.Trim() + "%", TenantID, SupplierID);
                                if (emptyLocs != null && emptyLocs.Count > 0)
                                {
                                    foreach (LocationOccupancy lo in emptyLocs)
                                    {
                                        if (quantityNeedToFill >= 1)
                                        {
                                            SuggestedLocation suggestedLocation = getSuggestedLocation(lo, lineItem, quantityNeedToFill);
                                            if (suggestedLocation != null)
                                            {
                                                quantityNeedToFill -= suggestedLocation.QTY;
                                                updateLineItem.SuggestedLocations.Add(suggestedLocation);
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    if (quantityNeedToFill < 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return updateLineItem;
        }

        

        private LineItem prepareSuggestedLocations(LineItem lineItem, int TenantID = 0, int SupplierID = 0)
        {
            //findout default locations, this block for new materials
            //tenant & supplier specific locations
            LineItem updateLineItem = null;
            if (lineItem != null && !lineItem.GRN_STATUS)
            {
                //conversion for CAR/PALLET/EA/BOX
                lineItem.QTY_Base_UOM = (lineItem.INV_QTY - lineItem.processedDocQty) * lineItem.Base_UOM_QTY * lineItem.INV_UOM_QTY;
                updateLineItem = lineItem;
                updateLineItem.SuggestedLocations = new List<SuggestedLocation>();
               
                //decimal quantityNeedToFill = (lineItem.INV_QTY - lineItem.processedDocQty);
                decimal quantityNeedToFill = lineItem.QTY_Base_UOM;
                if (quantityNeedToFill >= 1)
                {
                  
                    //need to fetch zone codes based on material group, previous received zone list might be helpful  
                    //need to fetch zone codes based on inbound storeref number & warehouseID
                    
                    string warehouseCode = GetWarehouseCode(SI.StoreRefId);
                    Object DBResult = DAOController.GetData(DRLExecuteCode.ZONE_LIST_BY_WAREHOUSE, warehouseCode);
                    IList<WarehouseZone> zones = (IList<WarehouseZone>)DBResult;
                    
                    if (zones != null && zones.Count > 0)
                    {
                        foreach (WarehouseZone zone in zones)
                        {
                            IList<LocationOccupancy> emptyLocs = getNearByLocationByBinCode(zone.ZoneCode.Trim() + "%", TenantID, SupplierID);
                            if (emptyLocs != null && quantityNeedToFill >= 1)
                            {
                                foreach (LocationOccupancy lo in emptyLocs)
                                {
                                    if (quantityNeedToFill >= 1)
                                    {
                                        SuggestedLocation suggestedLocation = getSuggestedLocation(lo, lineItem, quantityNeedToFill);
                                        if (suggestedLocation != null)
                                        {
                                            quantityNeedToFill -= suggestedLocation.QTY;
                                            updateLineItem.SuggestedLocations.Add(suggestedLocation);
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            if (quantityNeedToFill < 1)
                            {
                                break;
                            }
                        }
                    }
                }

                if (quantityNeedToFill > 1)
                {
                    throw new ServiceException(15, "there is no enough bins for TENANT/SUPPLIER, please configure new bins as per the item dim");
                }
            }
            return updateLineItem;
        }
               


        //resource method-1
        private IList<LocationOccupancy> getNearByLocationByBinCode(string LocationCode, int TenantID, int SupplierID)
        {
            CommonCriteria criteria = new CommonCriteria() { SUPPLIERID = SupplierID, TENANTID = TenantID, MATERIAL_BIN = false, LOCATIONID = 0, LOCATION_LIKE_CODE = LocationCode, NEAR_BY_LOC = true };
            Object DBResult = DAOController.GetDataFromStoredProcedure(DRLExecuteCode.LOCATION_LIST_BY_ZONECODE, new DBCriteria().LOCATION_LIST_BY_ZONECODE(criteria));
            if (DBResult != null)
            {
                return (IList<LocationOccupancy>)DBResult;
            }
            return null;
        }



        //resource method-2
        private LocationOccupancy getLocationOccupancy(int LocationID, string LocationCode = "")
        {
            CommonCriteria criteria = new CommonCriteria() { TENANTID = 0, MATERIAL_BIN = false, LOCATIONID = LocationID, LOCATION_LIKE_CODE = LocationCode, NEAR_BY_LOC = true };
            Object BinOccupancy = DAOController.GetDataFromStoredProcedure(DRLExecuteCode.LOCATION_OCCUPANCY_BY_ID, new DBCriteria().LOCATION_OCCUPANCY_BY_ID(criteria));
            
            if (BinOccupancy != null)
            {
                LocationOccupancy lo = (LocationOccupancy)BinOccupancy;
                return lo;
            }
            return null;
        }

        //resource method-3
        private string fetchActiveZoneCode(string str, string pattern)
        {
            //here pattern is the location/bin code format
            if (str.Length > 5)
            {
                return str.Substring(0, 4) + "%";
            }
            return null;
        }

        
        private SuggestedLocation getSuggestedLocation(LocationOccupancy lo, LineItem lineItem, decimal quantityNeedtoFill = 0.0m)
        {
            //FILTER LOCATION INFORMATION, BASED ON WEIGHT AND AVAIL VOLUME.
            SuggestedLocation suggestedLocation = null;
            decimal BIN_MAX_VOLUME = putawayConfig.ContainsKey("BIN_VOLUME_OCCUPANCY") ? Convert.ToDecimal(putawayConfig["BIN_VOLUME_OCCUPANCY"].ToString()) : 80;
            decimal BIN_MAX_WEIGHT = putawayConfig.ContainsKey("BIN_MAX_WEIGHT") ? Convert.ToDecimal(putawayConfig["BIN_MAX_WEIGHT"].ToString()) : 90;

            if (lo != null && lo.WeightHoldPercentage < BIN_MAX_WEIGHT && lo.VolOccupiedPercentage < BIN_MAX_VOLUME && quantityNeedtoFill >= 1)
            {
               
                //WEIGHT & VOLUME CONSTRAINT BOTH SHOULD BE 
                if (!logicalBinVolume.ContainsKey(lo.LocationID) || !logicalBinWeight.ContainsKey(lo.LocationID))
                {
                    logicalBinVolume.Add(lo.LocationID, 0);
                    logicalBinWeight.Add(lo.LocationID, 0);
                }



                decimal weightRemain = ((lo.MaxWeight * (BIN_MAX_WEIGHT * 0.01m)) - lo.WeightOccupancy) - logicalBinWeight[lo.LocationID];
                decimal volumeRemain = ((lo.Volume * (BIN_MAX_VOLUME * 0.01m)) - lo.VolumeOccupancy) - logicalBinVolume[lo.LocationID];

                //VOLUME CONSTRAINT CHECKING
                int fesibleVolumeBaseQty = (int)(volumeRemain / lineItem.dimensions.MVOLUME);
                int fesibleWeightBaseQty = (int)(weightRemain / lineItem.dimensions.MWEIGHT);

                decimal quantityCanHold = fesibleVolumeBaseQty <= fesibleWeightBaseQty ? fesibleVolumeBaseQty : fesibleWeightBaseQty;

                if (quantityCanHold >= 1)
                {
                    if (quantityNeedtoFill < quantityCanHold)
                        quantityCanHold = quantityNeedtoFill;

                    logicalBinVolume[lo.LocationID] += quantityCanHold * lineItem.dimensions.MVOLUME;
                    logicalBinWeight[lo.LocationID] += quantityCanHold * lineItem.dimensions.MWEIGHT;

                    suggestedLocation = new SuggestedLocation();
                    suggestedLocation.QTY = quantityCanHold;

                    suggestedLocation.SuggestedQTY = suggestedLocation.QTY;
                    suggestedLocation.InvoiceLineId = lineItem.InvoiceSysLineId;
                    suggestedLocation.SuggestedVolume = quantityCanHold * lineItem.dimensions.MVOLUME;
                    suggestedLocation.SuggestedWeight = quantityCanHold * lineItem.dimensions.MWEIGHT;
                    suggestedLocation.LocationCode = lo.LocationCode;
                    suggestedLocation.LocationID = lo.LocationID;
                }
            }
            return suggestedLocation;
        }
        
        private string GetWarehouseCode(string storeRefNo)
        {
            string WarehouseCode = "";
            if (storeRefNo != null && storeRefNo.Trim() != "")
            {
                return storeRefNo.Substring(0, storeRefNo.Length - 7);
            }
            return WarehouseCode;
        }

    }


}
