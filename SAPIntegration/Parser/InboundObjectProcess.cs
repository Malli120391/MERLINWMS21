using SAPIntegration.INOUT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPIntegration.Parser
{
    public class InboundObjectProcess
    {
        public static IDictionary<string, string> InboundObject = new Dictionary<string, string>() { 
        { "<MATMAS01>", "MATERIAL" }, { "<ORDERS05>", "ORDERS_PURCHASE" }, { "<ORDERS02>", "ORDERS_SALES" }, { "<INFREC01>", "POINFO" }, 
        { "<DEBMAS06>", "CUSTOMER" }, { "<CREMAS05>", "SUPPLIER" } ,{ "<ARTMAS02>", "MATERIAL_RETAIL" }
        };

        public string msgFromSap;

        public InboundObjectProcess(string msg)
        {
            this.msgFromSap = msg;
        }

        public void processInboundMsg()
        {
            if (this.msgFromSap != null && this.msgFromSap.Trim() != "")
            {
                Type inbound_type = null;
                for (int i = 0; i < InboundObject.Count(); i++)
                {
                    var item = InboundObject.ElementAt(i);
                    if (msgFromSap.Contains(item.Key.Trim()))
                    {
                        inbound_type = getInboundObjectType(item.Value.Trim());
                        break;
                    }
                }

                if (inbound_type != null)
                {

                    object inboundObject = SerializeDeserialize.Deserialize(inbound_type, msgFromSap, false);
                    if (inboundObject != null)
                    {
                        if (!parseInboundObject(inbound_type, inboundObject))
                        {
                            throw new InvParserException("Inbound object parsing problem", 3);
                        }
                    }
                    else
                    {
                        throw new InvParserException("Inbound object not deserialized some mandatory information is missing", 2);
                    }

                }
                else
                {
                    throw new InvParserException("Inbound object not found", 1);
                }
            }
        }


        private Type getInboundObjectType(string InboundCode)
        {
            if (InboundCode.Trim() != "")
            {
                Type temp_type = null;
                switch (InboundCode.Trim())
                {
                    case "MATERIAL":
                        temp_type = typeof(MATMAS01);
                        break;
                    case "ORDERS_PURCHASE":
                        temp_type = typeof(ORDERS05);
                        break;
                    case "ORDERS_SALES":
                        temp_type = typeof(ORDERS02);
                        break;
                    case "POINFO":
                        temp_type = typeof(INFREC01);
                        break;
                    case "CUSTOMER":
                        temp_type = typeof(DEBMAS06);
                        break;
                    case "SUPPLIER":
                        temp_type = typeof(CREMAS05);
                        break;
                    case "MATERIAL_RETAIL":
                        temp_type = typeof(ARTMAS02);
                        break;
                    default:
                        temp_type = null; break;
                }
                return temp_type;
            }
            return null;
        }


        private bool parseInboundObject(Type type, Object obj)
        {

            bool processStatus = false;
            switch (type.Name.Trim())
            {
                case "MATMAS01":
                    processStatus = new ItemManager(new InboundParser(obj).getInboundItemObject()).saveOrUpdate();
                    break;
                case "ARTMAS02":
                    processStatus = new ItemManager(new InboundParser(obj).getInboundRetailItemObject()).saveOrUpdate();
                    break;
                case "ORDERS05":
                    /*
                    Need to find Sales Order/Purchase Order
                    */

                    if (isPurchaseOrder(obj))
                    {
                        processStatus = new PurchaseOrderManager(new InboundParser(obj).getInboundPOObject()).saveOrUpdate();
                            //OrderManager(new InboundParser(obj).getInboundPOObject()).saveOrUpdate();
                    }
                    else
                    {
                        processStatus = new OrderManager(new InboundParser(obj).getInboundSOObject()).saveOrUpdate();
                    }
                    break;
                case "ORDERS02":
                    processStatus = new SalesOrderManager(new InboundParser(obj).getInboundSOObject()).saveOrUpdate();
                    break;

                case "INFREC01":
                    processStatus = new PurchaseOrderManager(new InboundParser(obj).getInboundPOObject()).saveOrUpdate();
                    break;
                case "DEBMAS06":
                    processStatus = new CustomerManager(new InboundParser(obj).getInboundCustomerObject()).saveOrUpdate();
                    break;
                case "CREMAS05":
                    processStatus = new VendorManager(new InboundParser(obj).getInboundVendorObject()).saveOrUpdate();
                    break;
                default:
                    break;
            }

            return processStatus;
        }

        private bool isPurchaseOrder(Object obj)
        {
            bool isPurchaseOrder = false;
            ORDERS05 order = (ORDERS05)obj;
            IList<string> IdocOrderRef = null;
            IList<string> IdocOrderOrganization = null;
            if (order.IDOC.E1EDK02 != null && order.IDOC.E1EDK02.Count() > 0)
            {
                IdocOrderRef = new List<string>();

                foreach (ORDERS05IDOCE1EDK02 docRef in order.IDOC.E1EDK02)
                {
                    IdocOrderRef.Add(docRef.QUALF);
                }
            }

            if (order.IDOC.E1EDK14 != null && order.IDOC.E1EDK14.Count() > 0)
            {
                IdocOrderOrganization = new List<string>();
                foreach (ORDERS05IDOCE1EDK14 docOrg in order.IDOC.E1EDK14)
                {
                    IdocOrderOrganization.Add(docOrg.QUALF);
                }
            }


            if (IdocOrderOrganization != null && IdocOrderOrganization.Count() > 0)
            {
                IList<string> qualifierList = new List<string>() { "009", "011", "012", "013", "014" };
                foreach (string qualifier in IdocOrderOrganization)
                {
                    if (qualifierList.Contains(qualifier))
                    {
                        isPurchaseOrder = true;
                        break;
                    }
                }
            }

            if (!isPurchaseOrder)
            {
                foreach (string qualifier in IdocOrderRef)
                {
                    if (qualifier.Equals("001"))
                    {
                        isPurchaseOrder = true;
                        break;

                    }
                }
            }
            return isPurchaseOrder;
        }


        private bool isSalesOrder()
        {
            return true;
        }



    }
}
