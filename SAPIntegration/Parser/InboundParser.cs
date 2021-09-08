using SAPIntegration.BusinessObjects;
using SAPIntegration.INOUT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPIntegration.Parser
{
    class InboundParser
    {

        private Object inboundObj;

        public InboundParser(Object obj)
        {
            inboundObj = obj;
        }


        //7-customer error code
        public object getInboundCustomerObject()
        {
            InvCustomer customer = new InvCustomer();
            DEBMAS06 debitor = null;

            try
            {

                debitor = (DEBMAS06)inboundObj;
                //primary information
                if (debitor.IDOC != null && debitor.IDOC.E1KNA1M != null)
                {
                    DEBMAS06IDOCE1KNA1M masterData = debitor.IDOC.E1KNA1M;

                    if (masterData.NAME2 != null)
                        customer.lname = masterData.NAME2;

                    if (masterData.NAME1 != null)
                    {
                        customer.fname = masterData.NAME1;
                        customer.DBAName = masterData.NAME1;
                    }

                    if (masterData.KUNNR != null)
                    {
                        customer.custCode = masterData.KUNNR;//customer code
                        customer.custID = Convert.ToInt32(masterData.KUNNR);
                    }

                    if (masterData.E1KNKAM != null)
                        customer.currency = new InvCurrency() { currencyCode = masterData.E1KNKAM.WAERS };

                    if (masterData.ORT01 != null)
                        customer.city = masterData.ORT01;

                    if (masterData.LAND1 != null)
                        customer.countryCode = masterData.LAND1;

                    if (masterData.SPRAS_ISO != null)
                        customer.langCode = masterData.SPRAS_ISO;


                    if (masterData.ORT01 != null)
                    {
                        AddressBook address = new AddressBook();
                        if (masterData.STRAS != null)
                        {
                            address.adressLine1 = masterData.STRAS;
                            address.street2 = masterData.STRAS;
                        }

                        address.city = masterData.ORT01;
                        address.country = masterData.LAND1;

                        if (masterData.ORT02 != null)
                            address.street1 = masterData.ORT02;

                        if (masterData.PSTLZ != null)
                            address.zip = masterData.PSTLZ;

                        if (masterData.REGIO != null)
                            address.adressLine2 = masterData.REGIO;

                        customer.postalAddress = new List<AddressBook>();
                        customer.postalAddress.Add(address);
                    }



                    if (masterData.E1KNBKM != null && masterData.E1KNBKM.Count() > 0)
                    {
                        customer.bankData = new List<InvBankData>();
                        foreach (DEBMAS06IDOCE1KNA1ME1KNBKM sapCustBank in masterData.E1KNBKM)
                        {
                            InvBankData bankData = new InvBankData();
                            if (sapCustBank.BANKN != null)
                                bankData.AccountNumber = sapCustBank.BANKN;

                            if (sapCustBank.BANKA != null)
                                bankData.BankName = sapCustBank.BANKA;

                            if (sapCustBank.BANKL != null)
                                bankData.BankCode = sapCustBank.BANKL;

                            if (sapCustBank.KOINH != null)
                                bankData.HolderName = sapCustBank.KOINH;

                            if (sapCustBank.KOINH_N != null)
                                bankData.HolderName = sapCustBank.KOINH_N;

                            if (sapCustBank.SWIFT != null)
                                bankData.SwiftIBAN = sapCustBank.SWIFT;

                            if (sapCustBank.ORT01 != null || sapCustBank.STRAS != null)
                            {
                                AddressBook bankAdd = new AddressBook();
                                if (sapCustBank.ORT01 != null)
                                {
                                    bankAdd.city = sapCustBank.ORT01;
                                    bankAdd.adressLine1 = sapCustBank.ORT01;
                                }

                                if (sapCustBank.PSKTO != null)
                                    bankAdd.zip = sapCustBank.PSKTO;

                                if (sapCustBank.STRAS != null)
                                {
                                    bankAdd.adressLine2 = sapCustBank.STRAS;
                                    bankAdd.street1 = sapCustBank.STRAS;
                                }
                                bankData.address = bankAdd;
                            }
                            customer.bankData.Add(bankData);
                        }
                    }
                }
                return customer;
            }
            catch (Exception e)
            {
                throw new InvParserException("inbound object identification problem: " + e.Message, 7);
            }

        }



        public object getInboundRetailItemObject()
        {
            try
            {

                ARTMAS02 sapm = (ARTMAS02)inboundObj;
                if (sapm != null && sapm.IDOC.EDI_DC40 != null && sapm.IDOC.E1BPE1MATHEAD != null)
                {

                    //header information
                    ARTMAS02IDOCE1BPE1MATHEAD sapMH = sapm.IDOC.E1BPE1MATHEAD;

                    //description
                    ARTMAS02IDOCE1BPE1MAKTRT sapMD = null;
                    if (sapm.IDOC.E1BPE1MAKTRT != null && sapm.IDOC.E1BPE1MAKTRT.Count == 1)
                        sapMD = sapm.IDOC.E1BPE1MAKTRT[0];

                    //packed information
                    ARTMAS02IDOCE1BPE1MARARTX sapmMeasure = null;
                    if (sapm.IDOC.E1BPE1MAKTRT != null && sapm.IDOC.E1BPE1MARARTX.Count == 1)
                        sapmMeasure = sapm.IDOC.E1BPE1MARARTX[0];

                    IList<InvItem> invItems = new List<InvItem>();

                    //list of materials
                    foreach (ARTMAS02IDOCE1BPE1MARART sapms in sapm.IDOC.E1BPE1MARART)
                    {
                        InvItem item = new InvItem();

                        if (sapms.MATERIAL != null)
                        {
                            item.ItemID = sapms.MATERIAL;
                            item.partNumber = sapms.MATERIAL;
                        }

                        if (sapms.CREATED_BY != null)
                            item.createdUser = sapms.CREATED_BY;

                        if (sapms.LAST_CHNGE != null && !sapms.LAST_CHNGE.Trim().Equals("00000000"))
                            item.lastUpdated = sapms.LAST_CHNGE;

                        if (sapMH.MATL_TYPE != null)
                            item.ItemType = sapMH.MATL_TYPE;

                        if (sapMH.MATL_GROUP != null)
                            item.ProductGroup = sapMH.MATL_GROUP;

                        item.UOMS = new List<BusinessUOM>();
                        if (sapms.BASE_UOM != null)
                        {
                            BusinessUOM baseUom = new BusinessUOM();
                            baseUom.UOMCode = sapms.BASE_UOM;
                            baseUom.UOMTypeCode = "Base";
                            item.UOMS.Add(baseUom);
                        }

                        item.measures = new List<Measure>();

                        if (sapms.ALLOWED_WT != null && !sapms.ALLOWED_WT.Trim().Equals("0.000"))
                        {
                            Measure itemMea = new Measure();
                            itemMea.measureCode = "Net Weigh";
                            itemMea.value = Convert.ToDecimal(sapms.ALLOWED_WT);

                            if (sapmMeasure != null && sapmMeasure.PACK_WT_UN_ISO != null)
                                itemMea.UOM = sapmMeasure.PACK_WT_UN_ISO;

                            item.measures.Add(itemMea);
                        }

                        if (sapms.ALLWD_VOL != null && !sapms.ALLWD_VOL.Trim().Equals("0.000"))
                        {
                            Measure itemMea = new Measure();
                            itemMea.measureCode = "Volume";
                            itemMea.value = Convert.ToDecimal(sapms.ALLWD_VOL);

                            if (sapmMeasure != null && sapmMeasure.PACK_VO_UN_ISO != null)
                                itemMea.UOM = sapmMeasure.PACK_VO_UN_ISO;

                            item.measures.Add(itemMea);
                        }


                        if (sapMD != null && sapMD.MATL_DESC != null)
                        {
                            item.description = sapMD.MATL_DESC;
                        }

                        if (sapmMeasure != null && sapmMeasure.PO_UNIT_ISO != null && !sapmMeasure.PO_UNIT_ISO.Trim().Equals("X"))
                        {
                            item.UOMS.Add(new BusinessUOM() { UOMCode = sapmMeasure.PO_UNIT_ISO, UOMTypeCode = "Purchase UOM" });
                        }

                        if (sapmMeasure != null && sapmMeasure.BASE_UOM_ISO != null && !sapmMeasure.BASE_UOM_ISO.Trim().Equals("X"))
                        {
                            item.UOMS.Add(new BusinessUOM() { UOMCode = sapmMeasure.BASE_UOM_ISO, UOMTypeCode = "Base UOM" });
                        }

                        invItems.Add(item);

                    }
                    return invItems;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Write Line: " + e.StackTrace);
                throw new InvParserException("exception: " + e.Message + " " + e.StackTrace, 9);
            }
            return null;
        }
        

        public object getInboundItemObject()
        {
            try
            {

                MATMAS01 sapm = (MATMAS01)inboundObj;
                if (sapm != null && sapm.IDOC.E1MARAM != null && sapm.IDOC.E1MARAM.Count() > 0)
                {
                    IList<InvItem> invItems = new List<InvItem>();
                    foreach (MATMAS01IDOCE1MARAM sapms in sapm.IDOC.E1MARAM)
                    {
                        InvItem item = new InvItem();
                        if (sapms.MATNR != null)
                        {
                            item.ItemID = sapms.MATNR;
                            item.partNumber = sapms.MATNR;
                        }

                        if (sapms.ERNAM != null)
                            item.createdUser = sapms.ERNAM;

                        if (sapms.LAEDA != null && !sapms.LAEDA.Trim().Equals("00000000"))
                            item.lastUpdated = sapms.LAEDA;

                        if (sapms.MTART != null)
                            item.ItemType = sapms.MTART;

                        if (sapms.MATKL != null)
                            item.ProductGroup = sapms.MATKL;

                        item.UOMS = new List<BusinessUOM>();
                        if (sapms.MEINS != null)
                        {
                            BusinessUOM baseUom = new BusinessUOM();
                            baseUom.UOMCode = sapms.MEINS;
                            baseUom.UOMTypeCode = "Base";
                            item.UOMS.Add(baseUom);
                        }

                        item.measures = new List<Measure>();
                        if (sapms.BRGEW != null && !sapms.BRGEW.Trim().Equals("0.000"))
                        {
                            Measure itemMea = new Measure();
                            itemMea.measureCode = "Gross Weigh";
                            itemMea.value = Convert.ToDecimal(sapms.BRGEW);

                            if (sapms.GEWEI != null)
                                itemMea.UOM = sapms.GEWEI;

                            item.measures.Add(itemMea);
                        }

                        if (sapms.NTGEW != null && !sapms.NTGEW.Trim().Equals("0.000"))
                        {
                            Measure itemMea = new Measure();
                            itemMea.measureCode = "Net Weigh";
                            itemMea.value = Convert.ToDecimal(sapms.NTGEW);

                            if (sapms.GEWEI != null)
                                itemMea.UOM = sapms.GEWEI;

                            item.measures.Add(itemMea);
                        }

                        if (sapms.VOLUM != null && !sapms.VOLUM.Trim().Equals("0.000"))
                        {
                            Measure itemMea = new Measure();
                            itemMea.measureCode = "Volume";
                            itemMea.value = Convert.ToDecimal(sapms.VOLUM);

                            if (sapms.VOLEH != null)
                                itemMea.UOM = sapms.VOLEH;

                            item.measures.Add(itemMea);
                        }

                        if (sapms.LAENG != null && !sapms.LAENG.Trim().Equals("0.000"))
                        {
                            Measure itemMea = new Measure();
                            itemMea.measureCode = "Length";
                            itemMea.value = Convert.ToDecimal(sapms.LAENG);

                            if (sapms.MEABM != null)
                                itemMea.UOM = sapms.MEABM;

                            item.measures.Add(itemMea);
                        }


                        if (sapms.BREIT != null && !sapms.BREIT.Trim().Equals("0.000"))
                        {
                            Measure itemMea = new Measure();
                            itemMea.measureCode = "Width";
                            itemMea.value = Convert.ToDecimal(sapms.BREIT);

                            if (sapms.MEABM != null)
                                itemMea.UOM = sapms.MEABM;

                            item.measures.Add(itemMea);
                        }

                        if (sapms.HOEHE != null && !sapms.HOEHE.Trim().Equals("0.000"))
                        {
                            Measure itemMea = new Measure();
                            itemMea.measureCode = "Height";
                            itemMea.value = Convert.ToDecimal(sapms.HOEHE);

                            if (sapms.MEABM != null)
                                itemMea.UOM = sapms.MEABM;

                            item.measures.Add(itemMea);
                        }

                        if (sapms.E1MAKTM != null && sapms.E1MAKTM.Count() > 0)
                        {

                            foreach (MATMAS01IDOCE1MARAME1MAKTM mDesc in sapms.E1MAKTM)
                            {
                                if (mDesc.SPRAS_ISO.Trim().Equals("EN") && mDesc.MAKTX != null)
                                {
                                    item.description = mDesc.MAKTX;
                                    break;
                                }
                            }
                        }

                        /*
                        if (sapms.E1MARMM != null && sapms.E1MARMM.Count() > 0)
                        {
                            foreach (MATMAS01IDOCE1MARAME1MARMM uoms in sapms.E1MARMM)
                            {

                            }
                        }*/

                        if (sapms.E1MBEWM != null && sapms.E1MBEWM.Count() > 0)
                        {
                            item.price = new List<ItemPrice>();
                            foreach (MATMAS01IDOCE1MARAME1MBEWM mvaluation in sapms.E1MBEWM)
                            {
                                ItemPrice price = new ItemPrice();
                                price.price = (mvaluation.STPRS != null) ? Convert.ToDecimal(mvaluation.STPRS) : 0.00m;

                                if (mvaluation.PEINH != null)
                                {
                                    price.uom = new BusinessUOM();
                                    price.uom.UOMCode = mvaluation.PEINH;
                                }
                            }
                        }

                        invItems.Add(item);

                    }
                    return invItems;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Write Line: " + e.StackTrace);
                throw new InvParserException("exception: " + e.Message + " " + e.StackTrace, 9);
            }
            return null;
        }





        public object getInboundVendorObject()
        {

            try
            {
                CREMAS05 sapVend = (CREMAS05)inboundObj;
                InvVendor vendor = null;
                if (sapVend != null && sapVend.IDOC != null && sapVend.IDOC.E1LFA1M != null)
                {
                    CREMAS05IDOCE1LFA1M vendms = sapVend.IDOC.E1LFA1M;
                    vendor = new InvVendor();

                    if (vendms.LIFNR != null)
                        vendor.vendorCode = vendms.LIFNR;

                    if (vendms.ERDAT != null)
                        vendor.createdOn = vendms.ERDAT;

                    if (vendms.ANRED != null)
                        vendor.PCPTitle = vendms.ANRED;

                    if (vendms.ERNAM != null)
                        vendor.createdUser = vendms.ERNAM;

                    if (vendms.KTOKK != null)
                        vendor.vendorGroup = vendms.KTOKK;

                    vendor.contactAddress = new List<AddressBook>();
                    if (vendms.LAND1 != null)
                    {
                        vendor.countryCode = vendms.LAND1;
                        AddressBook add = new AddressBook();
                        add.country = vendms.LAND1;

                        if (vendms.ORT01 != null)
                        {
                            add.city = vendms.ORT01;
                            add.adressLine1 = vendms.ORT01;
                        }

                        if (vendms.ORT02 != null)
                        {
                            add.street1 = vendms.ORT02;
                            add.adressLine2 = vendms.ORT02;
                        }

                        if (vendms.SORTL != null)
                        {
                            add.street2 = vendms.SORTL;
                        }
                        vendor.contactAddress.Add(add);
                    }

                    if (vendms.MCOD1 != null || vendms.MCOD2 != null || vendms.MCOD3 != null)
                    {
                        vendor.searchText = new List<string>();
                        if (vendms.MCOD1 != null)
                            vendor.searchText.Add(vendms.MCOD1);

                        if (vendms.MCOD2 != null)
                            vendor.searchText.Add(vendms.MCOD2);

                        if (vendms.MCOD3 != null)
                            vendor.searchText.Add(vendms.MCOD3);

                    }
                    if (vendms.NAME1 != null)
                    {
                        vendor.PCP = vendms.NAME1;
                        vendor.DBA = vendms.NAME1;
                        vendor.organization = vendms.NAME1;
                    }

                    if (vendms.E1LFM1M != null && vendms.E1LFM1M.Count > 0)
                    {
                        if (vendms.E1LFM1M[0].WAERS != null)
                        {
                            vendor.currency = new InvCurrency() { currencyCode = vendms.E1LFM1M[0].WAERS };
                        }
                        else
                        {
                            vendor.currency = new InvCurrency() { };
                        }
                    }
                    else
                    {
                        vendor.currency = new InvCurrency() { };
                    }

                    if (vendms.E1LFBKM != null && vendms.E1LFBKM.Count() > 0)
                    {
                        vendor.bankData = new List<InvBankData>();

                        foreach (CREMAS05IDOCE1LFA1ME1LFBKM sapCustBank in vendms.E1LFBKM)
                        {
                            InvBankData bankData = new InvBankData();
                            if (sapCustBank.BANKN != null)
                                bankData.AccountNumber = sapCustBank.BANKN;

                            if (sapCustBank.BANKA != null)
                                bankData.BankName = sapCustBank.BANKA;

                            if (sapCustBank.BANKL != null)
                                bankData.BankCode = sapCustBank.BANKL;

                            if (sapCustBank.KOINH != null)
                                bankData.HolderName = sapCustBank.KOINH;

                            if (sapCustBank.SWIFT != null)
                                bankData.SwiftIBAN = sapCustBank.SWIFT;

                            if (sapCustBank.ORT01 != null || sapCustBank.STRAS != null)
                            {
                                AddressBook bankAdd = new AddressBook();
                                if (sapCustBank.ORT01 != null)
                                {
                                    bankAdd.city = sapCustBank.ORT01;
                                    bankAdd.adressLine1 = sapCustBank.ORT01;
                                }

                                if (sapCustBank.PSKTO != null)
                                    bankAdd.zip = sapCustBank.PSKTO;

                                if (sapCustBank.STRAS != null)
                                {
                                    bankAdd.adressLine2 = sapCustBank.STRAS;
                                    bankAdd.street1 = sapCustBank.STRAS;
                                }
                                bankData.address = bankAdd;
                            }
                            vendor.bankData.Add(bankData);
                        }
                    }

                }
                return vendor;
            }
            catch (Exception e)
            {
                throw new InvParserException("exception while parsing vendor object: " + e.Message, 10);
            }
        }


        

        public object getInboundPOObject()
        {
            InvPurchaseOrderInfo purchaseOrder = null;
            purchaseOrder = new InvPurchaseOrderInfo();


            if (inboundObj != null)
            {
                purchaseOrder.vendorInvoices = new List<InvInvoice>();

                InvInvoice invoice = new InvInvoice();
                purchaseOrder.vendorInvoices.Add(invoice);

                InvVendor vendor = new InvVendor();

                purchaseOrder.partnerAddress = new List<AddressBook>();
                purchaseOrder.vendor = vendor;


                ORDERS05 orderSAP = (ORDERS05)inboundObj;
                if (orderSAP.IDOC.E1EDK01 != null)
                {
                    ORDERS05IDOCE1EDK01 genInfo = orderSAP.IDOC.E1EDK01;
                    if (genInfo.CURCY != null)
                    {
                        invoice.currency = new InvCurrency() { currencyCode = genInfo.CURCY };
                    }

                    if (genInfo.RECIPNT_NO != null)
                    {
                        vendor.vendorID = Convert.ToInt32(genInfo.RECIPNT_NO);
                        vendor.vendorCode = genInfo.RECIPNT_NO;
                        vendor.TenantID = 1;
                        if (genInfo.BELNR != null)
                            purchaseOrder.POCode = genInfo.BELNR;
                    }

                }

                /*  if (orderSAP.IDOC.E1EDK03 != null && orderSAP.IDOC.E1EDK03.Count() > 0)
                    {
                        foreach (ORDERS05IDOCE1EDK03 headerData in orderSAP.IDOC.E1EDK03)
                        {
                            //requested delivery date
                            //Idoc generated date
                        }
                    }
                 */

                //header reference data
                //E1EDK02

                if (orderSAP.IDOC.E1EDK02 != null && orderSAP.IDOC.E1EDK02.Count() > 0)
                {

                    foreach (ORDERS05IDOCE1EDK02 refData in orderSAP.IDOC.E1EDK02)
                    {
                        if (refData.QUALF.Equals("001"))
                        {
                            purchaseOrder.POCode = refData.BELNR;
                            purchaseOrder.orderID = refData.BELNR;
                            purchaseOrder.PODate = refData.DATUM;
                            break;
                        }
                    }
                }

                //vendor group information
                if (orderSAP.IDOC.E1EDKA1 != null && orderSAP.IDOC.E1EDKA1.Count() > 0)
                {

                    ORDERS05IDOCE1EDKA1 partnerInfo = orderSAP.IDOC.E1EDKA1[0];
                    if (partnerInfo.ORGTX != null)
                        vendor.organization = partnerInfo.ORGTX;

                    if (partnerInfo.PARTN != null)
                        vendor.partyNo = partnerInfo.PARTN;

                    if (partnerInfo.PAORG != null)
                        vendor.vendorGroup = partnerInfo.PAORG;

                    if (partnerInfo.BNAME != null)
                        vendor.PCP = partnerInfo.BNAME;

                }

                //partner informatio 
                if (orderSAP.IDOC.E1EDKA1 != null && orderSAP.IDOC.E1EDKA1.Count() > 0)
                {
                    int length = orderSAP.IDOC.E1EDKA1.Count();
                    ORDERS05IDOCE1EDKA1 partnerInfo = orderSAP.IDOC.E1EDKA1[length - 1];

                    AddressBook address = new AddressBook();

                    if (partnerInfo.ORT01 != null)
                        address.city = partnerInfo.ORT01;

                    if (partnerInfo.ORT02 != null)
                        address.street1 = partnerInfo.ORT02;

                    if (partnerInfo.STRAS != null)
                        address.street2 = partnerInfo.STRAS;

                    if (partnerInfo.LAND1 != null)
                        address.country = partnerInfo.LAND1;

                    if (partnerInfo.NAME1 != null)
                        address.adressLine1 = partnerInfo.NAME1;

                    if (partnerInfo.NAME2 != null)
                        address.adressLine2 = partnerInfo.NAME2;

                    if (partnerInfo.PSTLZ != null)
                        address.zip = partnerInfo.PSTLZ;

                    purchaseOrder.partnerAddress.Add(address);
                }



                if (orderSAP.IDOC.E1EDKA1 != null && orderSAP.IDOC.E1EDKA1.Count() > 0)
                {
                    vendor.postalAddress = new List<AddressBook>();
                    foreach (ORDERS05IDOCE1EDKA1 partnerInfo in orderSAP.IDOC.E1EDKA1)
                    {
                        AddressBook address = new AddressBook();

                        if (partnerInfo.ORT01 != null)
                            address.city = partnerInfo.ORT01;

                        if (partnerInfo.ORT02 != null)
                            address.street1 = partnerInfo.ORT02;

                        if (partnerInfo.STRAS != null)
                            address.street2 = partnerInfo.STRAS;

                        if (partnerInfo.LAND1 != null)
                            address.country = partnerInfo.LAND1;

                        if (partnerInfo.NAME1 != null)
                            address.adressLine1 = partnerInfo.NAME1;

                        if (partnerInfo.NAME2 != null)
                            address.adressLine2 = partnerInfo.NAME2;

                        if (partnerInfo.PSTLZ != null)
                            address.zip = partnerInfo.PSTLZ;

                        vendor.postalAddress.Add(address);
                    }
                }

                //E1EDP01 Item General Data
                if (orderSAP.IDOC.E1EDP01 != null && orderSAP.IDOC.E1EDP01.Count() > 0)
                {
                    invoice.lineItems = new List<LineItem>();
                   

                    foreach (ORDERS05IDOCE1EDP01 genData in orderSAP.IDOC.E1EDP01)
                    {
                        LineItem lineItem = new LineItem();
                        lineItem.currencyCode = genData.CURCY != null ? genData.CURCY : invoice.currency.currencyCode;

                        InvItem item = new InvItem();
                        lineItem.item = item;

                        if (genData.POSEX !=null )
                        lineItem.LineNo =  Convert.ToInt32( genData.POSEX);

                        if (genData.POSEX != null)
                            item.ItemID = genData.POSEX;

                        if (genData.PSTYP != null)
                            item.ItemTypeID = Convert.ToInt16(genData.PSTYP);

                        if (genData.MENGE != null)
                            item.qtyPerLayer = Convert.ToDecimal(genData.MENGE);


                        item.UOMS = new List<BusinessUOM>();
                        BusinessUOM uomB = new BusinessUOM();

                        if (genData.MENEE != null)
                            uomB.UOMCode = genData.MENEE;

                        if (genData.MENGE != null)
                        {
                            uomB.priceQty = Convert.ToDecimal(genData.MENGE);
                            lineItem.purchQty = Convert.ToDecimal(genData.MENGE);
                            lineItem.qtyOrdered = Convert.ToDecimal(genData.MENGE);
                        }

                        if (genData.BMNG2 != null)
                            uomB.priceQty = Convert.ToDecimal(genData.BMNG2);

                        if (genData.PMENE != null)
                            uomB.UOMCode = genData.PMENE;

                        if (genData.VPREI != null)
                        {
                            uomB.price = Convert.ToDecimal(genData.VPREI);
                            lineItem.LinePrice = Convert.ToDecimal(genData.VPREI);
                        }

                        if (genData.PEINH != null)
                        {
                            uomB.priceUnit = Convert.ToDecimal(genData.PEINH);
                            
                            lineItem.unitPrice = Convert.ToDecimal(genData.PEINH);
                        }

                        item.UOMS.Add(uomB);

                        if (genData.E1EDP19 != null && genData.E1EDP19.Count() > 0)
                        {
                            foreach (ORDERS05IDOCE1EDP01E1EDP19 itemIdentification in genData.E1EDP19)
                            {
                                if (itemIdentification.QUALF.Equals("001"))
                                {
                                    item.ItemID = itemIdentification.IDTNR;
                                    item.partNumber = itemIdentification.IDTNR;
                                    break;
                                }
                            }
                        }

                        /*
                        if (genData.E1EDP20 != null && genData.E1EDP20.Count() > 0)
                        {
                            foreach (ORDERS05IDOCE1EDP01E1EDP20 scheduleInfo in genData.E1EDP20)
                            {
                                //schedule information
                            }
                        }*/

                        //lineNo++;
                        invoice.lineItems.Add(lineItem);
                    }

                }

            }
            return purchaseOrder;
        }


        //8-purchase order error code
        public object getInboundPurchaseInfoObject()
        {
            try
            {
                InvPurchaseOrderInfo purchaseOrder = null;
                if (inboundObj != null)
                {
                    INFREC01 sapPO = (INFREC01)inboundObj;
                    purchaseOrder = new InvPurchaseOrderInfo();
                    if (sapPO.IDOC != null && sapPO.IDOC.E1EINAM != null)
                    {
                        INFREC01IDOCE1EINAM poms = sapPO.IDOC.E1EINAM;
                    }
                }
                return purchaseOrder;
            }
            catch (Exception e)
            {
                throw new InvParserException(e.Message, 8);
            }

        }

        public object getInboundSOObject()
        {
            InvSalesOrderInfo salesOrder = new InvSalesOrderInfo();
            if (inboundObj != null)
            {
                ORDERS02 orderSAP = (ORDERS02)inboundObj;
                salesOrder.custInvoices = new List<InvInvoice>();
                InvInvoice invoice = new InvInvoice();

                InvCustomer cust = new InvCustomer();
                salesOrder.customer = cust;

                if (orderSAP.IDOC.E1EDK01 != null)
                {
                    ORDERS02IDOCE1EDK01 genInfo = orderSAP.IDOC.E1EDK01;
                    if (genInfo.CURCY != null)
                    {
                        invoice.currency = new InvCurrency() { currencyCode = genInfo.CURCY };
                    }

                    if (genInfo.RECIPNT_NO != null)
                    {
                        cust.custID = Convert.ToInt32(genInfo.RECIPNT_NO);
                        cust.custCode = genInfo.RECIPNT_NO;
                        
                        cust.TenantID = 1;
                        if (genInfo.BELNR != null)
                            salesOrder.SOCode = genInfo.BELNR;
                    }

                }

                if (orderSAP.IDOC.E1EDK02 != null && orderSAP.IDOC.E1EDK02.Count() > 0)
                {

                    foreach (ORDERS02IDOCE1EDK02 refData in orderSAP.IDOC.E1EDK02)
                    {
                        if (refData.QUALF.Equals("002"))
                        {
                            salesOrder.SOCode = refData.BELNR;
                            salesOrder.SODate = refData.DATUM;
                            break;
                        }
                    }
                }


                if (orderSAP.IDOC.E1EDKA1 != null && orderSAP.IDOC.E1EDKA1.Count() > 0)
                {

                    ORDERS02IDOCE1EDKA1 partnerInfo = orderSAP.IDOC.E1EDKA1[0];
                    if (partnerInfo.NAME1 != null)
                        cust.DBAName = partnerInfo.NAME1;

                    if (partnerInfo.PARTN != null)
                        cust.custCode = partnerInfo.PARTN;

                    if (partnerInfo.PAORG != null)
                        cust.custGroup = partnerInfo.PAORG;

                    if (partnerInfo.BNAME != null)
                        cust.fname = partnerInfo.BNAME;

                }

                salesOrder.partnerAddress = new List<AddressBook>();

                if (orderSAP.IDOC.E1EDKA1 != null && orderSAP.IDOC.E1EDKA1.Count() > 0)
                {
                    int length = orderSAP.IDOC.E1EDKA1.Count();
                    ORDERS02IDOCE1EDKA1 partnerInfo = orderSAP.IDOC.E1EDKA1[length - 1];

                    AddressBook address = new AddressBook();

                    if (partnerInfo.ORT01 != null)
                        address.city = partnerInfo.ORT01;

                    if (partnerInfo.ORT02 != null)
                        address.street1 = partnerInfo.ORT02;

                    if (partnerInfo.STRAS != null)
                        address.street2 = partnerInfo.STRAS;

                    if (partnerInfo.LAND1 != null)
                        address.country = partnerInfo.LAND1;

                    if (partnerInfo.NAME1 != null)
                        address.adressLine1 = partnerInfo.NAME1;

                    if (partnerInfo.NAME2 != null)
                        address.adressLine2 = partnerInfo.NAME2;

                    if (partnerInfo.PSTLZ != null)
                        address.zip = partnerInfo.PSTLZ;

                    salesOrder.partnerAddress.Add(address);
                }

                if (orderSAP.IDOC.E1EDKA1 != null && orderSAP.IDOC.E1EDKA1.Count() > 0)
                {
                    cust.postalAddress = new List<AddressBook>();

                    foreach (ORDERS02IDOCE1EDKA1 partnerInfo in orderSAP.IDOC.E1EDKA1)
                    {
                        AddressBook address = new AddressBook();

                        if (partnerInfo.ORT01 != null)
                            address.city = partnerInfo.ORT01;

                        if (partnerInfo.ORT02 != null)
                            address.street1 = partnerInfo.ORT02;

                        if (partnerInfo.STRAS != null)
                            address.street2 = partnerInfo.STRAS;

                        if (partnerInfo.LAND1 != null)
                            address.country = partnerInfo.LAND1;

                        if (partnerInfo.NAME1 != null)
                            address.adressLine1 = partnerInfo.NAME1;

                        if (partnerInfo.NAME2 != null)
                            address.adressLine2 = partnerInfo.NAME2;

                        if (partnerInfo.PSTLZ != null)
                            address.zip = partnerInfo.PSTLZ;

                        cust.postalAddress.Add(address);
                    }
                }

                //E1EDP01 Item General Data
                //InvCustomerPO
                salesOrder.CustPO = new List<InvCustomerPO>();

                InvCustomerPO CustPO = new InvCustomerPO();

                salesOrder.CustPO.Add(CustPO);

                if (orderSAP.IDOC.E1EDP01 != null && orderSAP.IDOC.E1EDP01.Count() > 0)
                {
                    CustPO.reqItems = new List<LineItem>();

                    foreach (ORDERS02IDOCE1EDP01 genData in orderSAP.IDOC.E1EDP01)
                    {
                        LineItem lineItem = new LineItem();
                        lineItem.currencyCode = genData.CURCY != null ? genData.CURCY : invoice.currency.currencyCode;

                        InvItem item = new InvItem();
                        lineItem.item = item;

                        if (genData.POSEX != null)
                            lineItem.LineNo = Convert.ToDecimal(genData.POSEX);

                        if (genData.POSEX != null)
                            item.ItemID = genData.POSEX;

                        if (genData.PSTYP != null)
                            item.ItemTypeID = Convert.ToInt16(genData.PSTYP);

                        if (genData.MENGE != null)
                            item.qtyPerLayer = Convert.ToDecimal(genData.MENGE);


                        item.UOMS = new List<BusinessUOM>();
                        BusinessUOM uomB = new BusinessUOM();

                        if (genData.MENEE != null)
                        {
                            uomB.UOMCode = genData.MENEE;
                            lineItem.UOMCode = genData.MENEE;
                        }

                        if (genData.MENGE != null)
                        {
                            uomB.priceQty = Convert.ToDecimal(genData.MENGE);
                            lineItem.salesQty = Convert.ToDecimal(genData.MENGE);
                            lineItem.qtyOrdered = Convert.ToDecimal(genData.MENGE);
                        }

                        if (genData.BMNG2 != null)
                            uomB.priceQty = Convert.ToDecimal(genData.BMNG2);

                        if (genData.PMENE != null)
                            uomB.UOMCode = genData.PMENE;

                        if (genData.VPREI != null)
                        {
                            uomB.price = Convert.ToDecimal(genData.VPREI);
                            lineItem.LinePrice = Convert.ToDecimal(genData.VPREI);
                        }

                        if (genData.PEINH != null)
                        {
                            uomB.priceUnit = Convert.ToDecimal(genData.PEINH);
                            //lineItem.LineNo = lineNo;
                            lineItem.unitPrice = Convert.ToDecimal(genData.PEINH);
                        }

                        item.UOMS.Add(uomB);

                        if (genData.E1EDP19 != null && genData.E1EDP19.Count() > 0)
                        {
                            foreach (ORDERS02IDOCE1EDP01E1EDP19 itemIdentification in genData.E1EDP19)
                            {
                                if (itemIdentification.QUALF.Equals("002"))
                                {
                                    item.ItemID = itemIdentification.IDTNR;
                                    item.partNumber = itemIdentification.IDTNR;
                                    break;
                                }
                            }
                        }

                        CustPO.reqItems.Add(lineItem);
                    }

                }


            }

            return salesOrder;
        }



    }
}
