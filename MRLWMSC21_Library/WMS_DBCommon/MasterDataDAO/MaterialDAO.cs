using MRLWMSC21Common;
using MRLWMSC21_Library.WMS_ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon.MasterDataDAO
{
    delegate object MaterialDataAccess(object input, int IN_MSG_TYPE);

    internal class MaterialDAO : DBServiceController
    {
        
        public object processDBCall(string DB_CALL_CODE, object input, int IN_MSG_TYPE)
        {
            try
            {

                if (DBServicePool.DBServiceImplementRef.ContainsKey(DB_CALL_CODE.Trim()) || DBServicePool.DBServiceImplementRef[DB_CALL_CODE].Trim() != "")
                {
                    MethodInfo methodInfo = typeof(MaterialDAO).GetMethod(DBServicePool.DBServiceImplementRef[DB_CALL_CODE]);
                    MaterialDataAccess dataAccess = (MaterialDataAccess)Delegate.CreateDelegate(typeof(MaterialDataAccess), null, methodInfo);
                    return dataAccess(input, IN_MSG_TYPE);
                }
                else
                {
                    throw new NotImplementedException("the requested DB execution call not implemented under MaterialDAO. [" + DB_CALL_CODE + "]");
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("the requested DB Execution Call not implemented yet. [" + DB_CALL_CODE + "] " + ex.Message);
            }
        }



        public IList<MaterialInwardOutward> GetMaterialConsumptionById(object input, int IN_MSG_TYPE)
        {
            IList<MaterialInwardOutward> materialInOutList = null;
            if (input != null && IN_MSG_TYPE == 2)
            {
                //getting data through procedure call.
                IDictionary<string, object> criteria = (IDictionary<string, object>)input;
                string drlStatement = "[dbo].[SP_SUGGESTED_PUTAWAY_MATERIAL_STAT] @MATERIALID=" + Convert.ToInt32(criteria["MATERIALID"]) + ",@MATERIAL_STAT=" + criteria["MATERIAL_STAT"].ToString();
                DataSet inwardSet = DB.GetDS(drlStatement, false);

                if (inwardSet != null && inwardSet.Tables.Count > 0)
                {
                    materialInOutList = new List<MaterialInwardOutward>();
                    foreach (DataRow row in inwardSet.Tables[0].Rows)
                    {
                        MaterialInwardOutward inout = new MaterialInwardOutward();
                        inout.InwardQty = Convert.ToDecimal(row["INQTY"].ToString());
                        inout.OutwardQty = Convert.ToDecimal(row["OUTQTY"].ToString());

                        DateTime tranDate = (row["IN_TRANDATE"].ToString() != "") ? DateTime.Parse(row["IN_TRANDATE"].ToString()) : DateTime.Parse(row["OUT_TRANDATE"].ToString());
                        inout.OutwardTranDate = tranDate;
                        inout.InwardTranDate = tranDate;
                        inout.SupplierSysInvId = Convert.ToInt32(row["SUPPLIERINVOICEID"]);
                        materialInOutList.Add(inout);
                    }
                }
            }
            return materialInOutList;
        }


        public IList<MaterialInwardOutward> GetMaterialOutwardInfoById(object input, int IN_MSG_TYPE)
        {
            IList<MaterialInwardOutward> materialInOutList = null;
            if (input != null && IN_MSG_TYPE == 2)
            {
                //getting data through procedure call.
                IDictionary<string, object> criteria = (IDictionary<string, object>)input;
                string drlStatement = "[dbo].[SP_SUGGESTED_PUTAWAY_MATERIAL_STAT] @MATERIALID=" + Convert.ToInt32(criteria["MATERIALID"]) + ",@MATERIAL_STAT=" + criteria["MATERIAL_STAT"].ToString() + ",@ORDER_STAT="+ criteria["ORDER_STAT"].ToString();
                DataSet inwardSet = DB.GetDS(drlStatement, false);

                if (inwardSet != null && inwardSet.Tables.Count > 0)
                {
                    materialInOutList = new List<MaterialInwardOutward>();
                    foreach (DataRow row in inwardSet.Tables[0].Rows)
                    {                     

                        MaterialInwardOutward inout = new MaterialInwardOutward();
                        DateTime tranDate = DateTime.Parse(row["TRANDATE"].ToString());
                        inout.OutwardTranDate = tranDate;                        

                        inout.OutwardQty = Convert.ToDecimal(row["SO_OUT_QTY"].ToString());
                        inout.MCode = row["MCode"].ToString();
                        inout.MaterialId = Convert.ToInt32(row["MaterialMasterID"]);
                        inout.SOSysId = Convert.ToInt32(row["SOHeaderID"]);
                        inout.TransactionDocId = Convert.ToInt32(row["TransactionDocID"]);
                        
                        materialInOutList.Add(inout);
                    }
                }
            }
            return materialInOutList;
        }



        public object GetMaterialBinInfo(object input, int IN_MSG_TYPE)
        {
            if (input != null)
            {
                return fetchMaterialBinInfo((IDictionary<string, object>)input, IN_MSG_TYPE);
            }
            else
            {
                return null;
            }
        }


        private IList<MaterialBinInfo> fetchMaterialBinInfo(IDictionary<string, object> input, int msg_type)
        {
            IList<MaterialBinInfo> itemBins = null;

            if (input != null)
            {
                DataSet ItemBinInfo = DB.GetDS("[dbo].[SP_SUGGESTED_PUTAWAY_LOCATIONS_INFO] @MATERIALID=" + input["MATERIALID"].ToString() + ",@NEARBY_LOC =0,@MATERIAL_BIN_RESULT =1", false);

                if (ItemBinInfo != null && ItemBinInfo.Tables.Count > 0)
                {
                    itemBins = new List<MaterialBinInfo>();
                    foreach (DataRow row in ItemBinInfo.Tables[0].Rows)
                    {
                        MaterialBinInfo itemBin = new MaterialBinInfo();
                        itemBin.Base_QTY = Convert.ToDecimal(row["QTY"].ToString());
                        itemBin.LocationID = Convert.ToInt32(row["LocationID"]);
                        itemBin.LocationCode = (row["Location"]).ToString();
                        itemBin.MaterialID = Convert.ToInt32(row["MaterialMasterID"]);
                        itemBin.MCode = (row["MCode"]).ToString();
                        itemBin.QTY = Convert.ToDecimal(row["DOC_QTY"]);
                        itemBin.QTY_UOM = (row["UoM"]).ToString();
                        itemBin.QTY_UOMID = Convert.ToInt16(row["UoMID"]);
                        itemBin.SuppierLineId = Convert.ToInt32(row["POSODetailsID"]);

                        itemBin.TranDate = DateTime.Parse(row["TRANDATE"].ToString());
                        //itemBin.TranDate = DateTime.ParseExact(row["TRANDATE"].ToString(), "yyyy-MM-dd", null);
                        itemBins.Add(itemBin);
                    }
                }
            }
            return itemBins;
        }



        public MaterialMaster GetMaterialInfoById(object input, int msg_type)
        {
            MaterialMaster material = null;
            if (input != null && msg_type == 1)
            {
                //getting through query call
                int MaterialId = (int)input;
                string drlStatement = "SELECT MaterialMasterID,isnull(MLength,0) MLength,isnull(MWeight,0) MWeight,isnull(MWidth,0) MWidth,isnull(MHeight,0) MHeight,isnull(MTypeID,1) MTypeID,isnull(MGroupID,0) MGroupID,isnull(IsPalletizedMaterial,0) IsPalletizedMaterial,MCode,isnull(CustomerPartNumber,'') CustomerPartNumber,isnull(MDescription,'') MDescription, ISNULL(SearchText,'') SearchText, ISNULL(StorageConditionID,1) StorageConditionID FROM MMT_MaterialMaster where MaterialMasterID=" + MaterialId + " AND IsDeleted=0 ";

                DataSet materialSet = DB.GetDS(drlStatement, false);
                if (materialSet != null && materialSet.Tables != null && materialSet.Tables[0] != null)
                {
                    foreach (DataRow row in materialSet.Tables[0].Rows)
                    {
                        material = new MaterialMaster();
                        material.Length = Convert.ToDecimal(row["MLength"].ToString());
                        material.Height = Convert.ToDecimal(row["MHeight"].ToString());
                        material.Weight = Convert.ToDecimal(row["MWeight"].ToString());
                        material.Width = Convert.ToDecimal(row["MWidth"].ToString());
                        material.MaterialGroupID = Convert.ToInt32(row["MGroupID"].ToString());
                        material.MaterialTypeID = Convert.ToInt32(row["MTypeID"].ToString());
                        material.MCode = row["MCode"].ToString();
                        material.Description = row["MDescription"].ToString();

                    }
                }

            }
            else if (input != null && msg_type == 2)
            {
                //getting through procedure call
            }
            return material;
        }
        
    }
}
