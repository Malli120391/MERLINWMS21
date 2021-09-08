using MRLWMSC21Common;
using MRLWMSC21_Library.WMS_ServiceObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon.MasterDataDTO
{
    internal class LocationDTO : DBServiceController
    {
        delegate object LocationDataPersist(object input, int MSG_TYPE);

        public object processDBCall(string DB_CALL_CODE, object input, int IN_MSG_TYPE)
        {
            try
            {

                if (DBServicePool.DBServiceImplementRef.ContainsKey(DB_CALL_CODE.Trim()) || DBServicePool.DBServiceImplementRef[DB_CALL_CODE].Trim() != "")
                {
                    MethodInfo methodInfo = typeof(LocationDTO).GetMethod(DBServicePool.DBServiceImplementRef[DB_CALL_CODE]);
                    LocationDataPersist dataPersist = (LocationDataPersist)Delegate.CreateDelegate(typeof(LocationDataPersist), null, methodInfo);
                    return dataPersist(input, IN_MSG_TYPE);
                }
                else
                {
                    throw new NotImplementedException("the requested DB execution call not implemented under LocationDTO. [" + DB_CALL_CODE + "]");
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("the requested DB Execution Call not implemented yet. [" + DB_CALL_CODE + "] " + ex.Message);
            }
        }



        public object HoldSuggestedLocation(object input, int IN_MSG_TYPE)
        {

            if (input != null)
            {
                Inbound inbound = (Inbound)input;

                if (inbound.invoices != null)
                {
                    for (int index = 0; index < inbound.invoices.Count; index++)
                    {
                        SupplierInvoice si = inbound.invoices.ElementAt(index).Value;

                        if (si != null && si.InvoiceLineItems != null)
                        {
                            foreach (LineItem li in si.InvoiceLineItems)
                            {
                                if (li.SuggestedLocations != null && li.SuggestedLocations.Count > 0)
                                {
                                    foreach (SuggestedLocation sl in li.SuggestedLocations)
                                    {
                                        DB.ExecuteSQL("[dbo].[SP_SUGGESTED_LOCATIONS_OCCUPANCY_INFO] @LOCATIONID=" + sl.LocationID + ",@INBOUNDID=" + inbound.InboundId + ",@INVOICELINEID=" + sl.InvoiceLineId + ",@WEIGHT=" + sl.SuggestedWeight + ",@VOLUME=" + sl.SuggestedVolume);
                                    }
                                }
                            }
                        }
                    }
                }
                return true;

            }
            return false;
        }
    }
}
