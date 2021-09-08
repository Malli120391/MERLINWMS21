using MRLWMSC21_Library.WMS_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceControl
{
    public class WMSController
    {

        public static Object Process(ServiceCall FUNC_EXEC_CODE, Object input)
        {
            try
            {
                if (ServicePool.ServiceGroups.ContainsKey(FUNC_EXEC_CODE.ToString().Trim()))
                {
                    //module identification
                    ServiceController rootController = ServicePool.ServiceGroups[FUNC_EXEC_CODE.ToString().Trim()];
                    return rootController.RequestDispatcher(FUNC_EXEC_CODE.ToString().Trim(), input);
                }
                else
                {   
                    throw new ServiceException(10, "The specified functionality not implemented yet. [" + FUNC_EXEC_CODE + "]");
                }
            }
            catch (ServiceException se)
            {
                //log writing 
                throw se;
            }

        }
    }
}
