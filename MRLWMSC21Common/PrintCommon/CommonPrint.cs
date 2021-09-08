using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common.PrintCommon
{
    public class CommonPrint
    {


        ZPL zplString = new ZPL();
        //Commented ON 01-JAN-2019 BY Prasad ===========//
        public string PrintBarcodeLabel(TracklineMLabel Mlabel)
        {
            try
            {
                string result = null;

                string barcodestring = null;
                // string barcodestring = Mlabel.MCode+"|"+Mlabel.BatchNo+"|"+Mlabel.SerialNo+"|"+Mlabel.MfgDate.ToString("dd-MM-yy")+"|"+Mlabel.ExpDate.ToString("dd-MM-yy")+"|"+Mlabel.KitPlannerID.ToString();

                // if (Mlabel.Length == "3.00" && Mlabel.Width == "3.00")   
                // {

                //    barcodestring = Mlabel.MCode + "|" + Mlabel.BatchNo + "|" + Mlabel.SerialNo + "|" + Mlabel.KitPlannerID.ToString() + "|" + Mlabel.Lineno;
                // }
                //else
                // {

                Mlabel.HUSize = Mlabel.HUSize == "" || Mlabel.HUSize == "0" ? "1" : Mlabel.HUSize;
                Mlabel.HUNo = Mlabel.HUNo == "" || Mlabel.HUNo == "0" ? "1"  : Mlabel.HUNo;

                barcodestring = Mlabel.MCode + "|" + Mlabel.BatchNo + "|" + Mlabel.SerialNo + "|" + String.Format("{0:dd-MM-yy}", Mlabel.MfgDate) + "|" + String.Format("{0:dd-MM-yy}", Mlabel.ExpDate) + "|" + Mlabel.ProjectNo + "|" + Mlabel.KitCode.ToString() + "|" + Mlabel.Mrp + "|" + Mlabel.Lineno + "|" + Mlabel.HUNo + "|" + Mlabel.HUSize;
                //}
                result = DB.GetSqlS("EXEC [dbo].[sp_GetZPLString] @Dpi=" + Mlabel.Dpi + " , @Length=" + Mlabel.Length + ", @Width = " + Mlabel.Width + ", @LabelType = " + DB.SQuote(Mlabel.LabelType));
                if (result != "" && result != null)
                {

                    result = result.Replace("barcodegeneratorcodewithmfgandexp", barcodestring);
                    result = result.Replace("@SKU", "Part# :" + " " + Mlabel.MCode);

                    result = result.Replace("@Desc", Mlabel.MDescription);

                    result = result.Replace("@MDescLong", Mlabel.MDescriptionLong);


                    if (Mlabel.BatchNo != "")
                    {
                        result = result.Replace("@BatchNo", "Batch # /Lot :" + " " + Mlabel.BatchNo);
                    }
                    else
                    {
                        result = result.Replace("@BatchNo", "Batch # /Lot :" + "" + "");
                    }


                    if (Mlabel.MfgDate.ToString() != "")
                    {

                        result = result.Replace("@MfgDate", "Mfg.Date :" + " " + String.Format("{0:dd-MM-yy}", Mlabel.MfgDate));
                    }
                    else
                    {
                        result = result.Replace("@MfgDate", "Mfg.Date :" + "" + "");
                    }




                    if (Mlabel.ExpDate.ToString() != "")
                    {
                        result = result.Replace("@Exp.Date", "Exp.Date :" + " " + String.Format("{0:dd-MM-yy}", Mlabel.ExpDate));
                    }
                    else
                    {
                        result = result.Replace("@Exp.Date", "Exp.Date :" + "" + "");
                    }


                    if (Mlabel.SerialNo != "")
                    {
                        result = result.Replace("@Serial No.", "Serial # :" + "" + Mlabel.SerialNo);
                    }
                    else
                    {
                        result = result.Replace("@Serial No.", "Serial # :" + "" + "");
                    }

                    if (Mlabel.ProjectNo != "")
                    {
                        result = result.Replace("@Project Ref No.", "Project Ref # :" + "" + Mlabel.ProjectNo);
                    }
                    else
                    {
                        result = result.Replace("@Project Ref No.", "Project Ref # :" + "" + "");
                    }
                    if (Mlabel.Mrp != "")
                    {
                        result = result.Replace("@MRP", "MRP : " + "" + Mlabel.Mrp + " /-");
                    }
                    else
                    {
                        result = result.Replace("@MRP", "MRP :" + "" + "");
                    }

                    if (Mlabel.KitPlannerID != 0)
                    {
                        result = result.Replace("@Kit Id", "Kit ID :" + "" + Mlabel.KitPlannerID.ToString());
                    }
                    else
                    {
                        result = result.Replace("@Kit Id", "Kit ID  :" + "" + "");
                    }

                    if (Mlabel.GRNDate != DateTime.MinValue)
                    {
                        if (Mlabel.GRNDate.ToString() != "")
                        {
                            result = result.Replace("@GRN date", "GRN Date :" + "" + Mlabel.GRNDate.ToString("dd-MM-yy"));
                        }
                        else
                        {
                            result = result.Replace("@GRN date", "GRN Date :" + "" + "");
                        }

                    }
                    else
                    {
                        result = result.Replace("@GRN date", "GRN Date :" + "" + "");
                    }

                    if (Mlabel.Location != "")
                    {
                        result = result.Replace("@Location", "Location : " + Mlabel.Location);
                    }
                    else
                    {
                        result = result.Replace("@Location", "Location : " + "" + "");
                    }

                    if (Mlabel.HUSize != "")
                    {
                        result = result.Replace("@HUSize", "HU : " + Mlabel.HUNo + "/" + Mlabel.HUSize);
                    }
                    else
                    {
                        result = result.Replace("@HUSize", "HU : " + "" + "");
                    }
                }

                if (Mlabel.IsBoxLabelReq != true)
                {
                    result = result.Replace("@NoofLabels", Mlabel.PrintQty);


                    Mlabel.Duplicateprints = "0";
                    result = result.Replace("@DuplicatePrints", Mlabel.Duplicateprints);
                    //Commented ON 01-JAN-2019 BY Prasad // //zplString.printUsingIPnew(Mlabel.PrinterIP, 9100, 0, 0, result);

                    result = zplString.printUsingIPnew_ZPL(Mlabel.PrinterIP, 9100, 0, 0, result);
                    return result;
                }
                else
                {


                    result = result.Replace("@NoofLabels", (Convert.ToInt16(Mlabel.PrintQty) + 1).ToString());
                    Mlabel.Duplicateprints = "1";

                    result = result.Replace("@DuplicatePrints", Mlabel.Duplicateprints);

                    //Commented ON 01-JAN-2019 BY Prasad // //zplString.printUsingIPnew(Mlabel.PrinterIP, 9100, 0, 0, result);

                    result = zplString.printUsingIPnew_ZPL(Mlabel.PrinterIP, 9100, 0, 0, result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }         





    }


    }

      

