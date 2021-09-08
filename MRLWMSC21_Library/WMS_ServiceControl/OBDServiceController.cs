using MRLWMSC21_Library.WMS_ServiceImpl;
using MRLWMSC21_Library.WMS_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceControl
{
    class OBDServiceController : ServiceController
    {
        public object RequestDispatcher(string FUNC_EXEC_CODE, object input)
        {
            try
            {
                if (IsResourceValid(FUNC_EXEC_CODE, input) && ServicePool.AvailServiceNames.ContainsKey(FUNC_EXEC_CODE.Trim()))
                {
                    WMSService serviceImpl = ServicePool.AvailServiceNames[FUNC_EXEC_CODE.Trim()];
                    return serviceImpl.Process(FUNC_EXEC_CODE, input);
                }
                else
                {
                    throw new ServiceException(10, "The specified functionality not implemented yet. [" + FUNC_EXEC_CODE + "]");
                }
            }
            catch (ServiceException se)
            {
                throw se;
            }
        }

        private bool IsResourceValid(string FUNC_EXEC_CODE, object input)
        {
            try
            {
                Convert.ChangeType(input, ResourceSuggestion.WMS_FUNC_RESOURCE_TYPE[FUNC_EXEC_CODE]);
                return true;
            }
            catch (Exception ex)
            {
                throw new ServiceException(15, "invalid input object, require valid object type:" + ResourceSuggestion.WMS_FUNC_RESOURCE_TYPE[FUNC_EXEC_CODE].GetType() + " Message: " + ex.Message);
            }
        }
    }
}
