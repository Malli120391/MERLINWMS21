using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceObjects
{
    class WMSConfig
    {
        private IDictionary<string, object> PUTAWAY_CONFIG;
        private IDictionary<string, object> PICKING_CONFIG;

        public IDictionary<string, object> GetPutawayConfig() 
        {
            return PUTAWAY_CONFIG;
        }

        public IDictionary<string, object> GetPickingConfig() 
        {
            return PICKING_CONFIG;
        }

        public void SetPickingConfig(IDictionary<string, object> input) 
        {
            PICKING_CONFIG = input;
        }

        public void SetPutawayConfig(IDictionary<string, object> input) 
        {
            PUTAWAY_CONFIG = input;
        }

    }
}
