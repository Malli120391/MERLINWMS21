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
    class MaterialServiceImpl : WMSService
    {
        delegate object MaterialServiceAccess(object input);

        public object Process(string FUNC_EXEC_CODE, object input)
        {
            try
            {
                if (ServicePool.ServiceImplRef.ContainsKey(FUNC_EXEC_CODE.Trim()) || ServicePool.ServiceImplRef[FUNC_EXEC_CODE].Trim() != "")
                {
                    MethodInfo methodInfo = typeof(SuggestedPicknoteImpl).GetMethod(ServicePool.ServiceImplRef[FUNC_EXEC_CODE]);
                    MaterialServiceAccess dataAccess = (MaterialServiceAccess)Delegate.CreateDelegate(typeof(MaterialServiceAccess), null, methodInfo);
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

        public object GetMaterialConsumptionRate(object input)
        {
            if (input != null)
            {
                MaterialMaster material = (MaterialMaster)input;
                CommonCriteria criteria = new CommonCriteria();
                criteria.MATERIALID = Convert.ToInt32(material.MaterialId);
                Object DBResult = DAOController.GetDataFromStoredProcedure(DRLExecuteCode.MATERIAL_INWARD_OUTWARD_BY_ID, new DBCriteria().MATERIAL_INWARD_OUTWARD_BY_ID(criteria));

                IList<MaterialInwardOutward> materialInOut = (IList<MaterialInwardOutward>)DBResult;

                if (DBResult != null && materialInOut.Count > 0)
                {

                }

            }
            return null;
        }



        public object GetMaterialSalesOrderRate(object input)
        {
            MaterialMaster material = null;
            if (input != null)
            {
                material = (MaterialMaster)input;
                CommonCriteria criteria = new CommonCriteria();
                criteria.MATERIALID = Convert.ToInt32(material.MaterialId);
                Object DBResult = DAOController.GetDataFromStoredProcedure(DRLExecuteCode.MATERIAL_OUTWARD_BY_ID, new DBCriteria().MATERIAL_OUTWARD_BY_ID(criteria));
                IList<MaterialInwardOutward> materialOut = (IList<MaterialInwardOutward>)DBResult;
                material.inwardOutward = materialOut;

                int totalOutwards = 0;
                if (materialOut != null && materialOut.Count > 0)
                {
                    IList<int> tempOrders = new  List<int>();
                    foreach (MaterialInwardOutward outward in materialOut)
                    {
                        if (!tempOrders.Contains(outward.SOSysId))
                        {
                            tempOrders.Add(outward.SOSysId);
                            totalOutwards++;
                        }
                    }
                }

                if (ServicePool.wms_config.GetPutawayConfig().ContainsKey("THRESHOLD_FASTMOVIND_GOOD")) 
                {
                    material.IsFastMovingGood = totalOutwards >= Convert.ToInt16(ServicePool.wms_config.GetPutawayConfig().ContainsKey("THRESHOLD_FASTMOVIND_GOOD"));
                    material.materialStat = new MaterialStatistics() { ConsumptionFrom = DateTime.Now, ConsumptionTo = DateTime.Now.AddMonths(-1), NoOfOrders = totalOutwards, AvgConsumption = totalOutwards, ConsumptionRate = totalOutwards };
                }

            }
            return material;
        }

    }

}
