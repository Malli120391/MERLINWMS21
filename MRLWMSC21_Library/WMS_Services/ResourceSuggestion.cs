using MRLWMSC21_Library.WMS_ServiceObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_Services
{
    public class ResourceSuggestion
    {
        //Activator.CreateInstance(type);
        public static IDictionary<string, Type> WMS_FUNC_RESOURCE_TYPE = new Dictionary<string, Type>()
        {
                {"MATERIAL_SALERATE_PERMONTH",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.MaterialMaster")},
                {"INB_CREATE",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Inbound")},
                {"INB_UPDATE",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Inbound")},
                {"INB_DELETE",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Inbound")},
                {"INB_SELECT",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Inbound")},
                {"INB_LIST",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Inbound")},

                {"INB_GRN",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LineItem")},
                {"INB_RECEIVE",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LineItem")},
                {"INB_PUTAWAY",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LineItem")},
                {"INB_RTR",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Inbound")},
                {"INB_SUGGESTED_PUTAWAY",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Inbound")},

                {"OBD_CREATE",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Outbound")},
                {"OBD_SELECT",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Outbound")},
                {"OBD_UPDATE",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Outbound")},
                {"OBD_DELETE",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Outbound")},

                {"OBD_LIST",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Outbound")},
                {"OBD_DPN",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Outbound")},
                {"OBD_PGI",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.Outbound")},

                {"INV_STOCK",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LiveStock")},
                {"INV_LIVE_STOCK",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LiveStock")},
                {"INV_STOCK_RESERVATION",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LiveStock")},
                {"INV_STOCK_RESERVATION_LINE",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LiveStock")},
                {"INV_STOCK_BY_MM",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LiveStock")},
                {"INV_STOCK_BY_TENANT",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LiveStock")},
                
                {"INV_ONHOLD_STOCK",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LiveStock")},
                {"INV_STOCK_TRANSFER",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LiveStock")},
                {"INV_STOCK_ADJUST",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.TransferOrder")},
                {"INV_STOCK_BY_MSP",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.LiveStock")},
                {"OBD_SUGGESTED_PICKNOTE",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.AvailableStockData")},
                {"OBD_RESTRICTED_PICKNOTE",Type.GetType("MRLWMSC21_Library.WMS_ServiceObjects.AvailableStockData")}
        };

        public static IDictionary<int, string> ErrorCode = new Dictionary<int, string>()
        {
            {1,"Execution code requires valid MaterialId to fetch its records"},
            {10,"specified serivce code is not implemented yet"},
            {15,"there is no locations to suggest, have a look on item dims and location management"}
        };

    }
}
