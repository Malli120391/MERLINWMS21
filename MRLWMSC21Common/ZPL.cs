using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common
{
    public enum BarcodeType
    {
        CODE128,
        CODE39,
        QRCODE

    }

    public class ZPL : IDisposable
    {


        bool disposed = false;
        private string _ipAddress = string.Empty;
        private int _port = 9100;
        private string _zplString = string.Empty;
        private int _NumberOfLabels = 1;
        private int _duplicatePrints = 0;
        public ZPL()
        {

        }

        static IDictionary<string, string> barCodeType = new Dictionary<string, string>() 
        { 
            {"CODE128","^BC"},
             {"CODE39","^B3"},
             {"QRCODE","^BQ"}
        };


        public void Dispose()
        {
            //Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// this function will drawing a barcode in provided location
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <param name="barCodeString"></param>
        /// <param name="barCodeHeight"></param>
        /// <param name="type"></param>
        /// <param name="isChecksumRequired"></param>
        public void addBarcode(int xAxis, int yAxis, string barCodeString, int barCodeHeight, BarcodeType type, bool isChecksumRequired, int barcodeWidth)
        {
            _zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") + ",^FD" + barCodeString + "^FS";
            //_zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") + ",^FD" + barCodeString + "^FS";

            //_zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") +","+"A"+","+ "^FD^SN" + barCodeString + ",1,Y^FS";
            //^FO110,110^BC,50,,N,^FD1234567890^FS
        }
        //=================== Added By M.D.Prasad On 10-Apr-2020 For QR Code ====================//     
        public void addBarcodeForQRCodeContainer(int xAxis, int yAxis, string barCodeString, int barCodeHeight, BarcodeType type, bool isChecksumRequired, int barcodeWidth)
        {
            _zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "4" : "4") + ",^FDQA," + barCodeString + "^FS";
          
        }
        public void addBarcodeForQRCodeLocation(int xAxis, int yAxis, string barCodeString, int barCodeHeight, BarcodeType type, bool isChecksumRequired, int barcodeWidth)
        {
            _zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "5" : "5") + ",^FDQA," + barCodeString + "^FS";

        }

        //=================== END ====================//  

        public void addBarcodeForUniqueRSN(int xAxis, int yAxis, string barCodeString, int barCodeHeight, BarcodeType type, bool isChecksumRequired, int barcodeWidth)
        {
            _zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") + ",^FD^SN" + barCodeString + ",1,Y^FS";
            //_zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") +","+"A"+","+ "^FD^SN" + barCodeString + ",1,Y^FS";
            //^FO110,110^BC,50,,N,^FD1234567890^FS
        }




        public void addQRCode(int xaxis, int yaxis, string inputtext)
        {

            _zplString += "^FO" + xaxis + "," + yaxis + "^BQNN,2,8^FD^SN   " + inputtext + ",1,Y^FS";

        }

        public void generateTotecode(string FGPartNumber, string SONumber, string CustomerCode, string ProductionOrder, string BatchNumber, DateTime MFGDate, int Quantity, string BaleCode, string TransactionId)
        {
            string totecode = FGPartNumber + "|" + SONumber + "|" + CustomerCode + "|" + ProductionOrder + "|" + BatchNumber + "|" + MFGDate + "|" + Quantity + "|" + BaleCode + "|" + TransactionId;


            // _zplString += "^XA^FO50,50^CF0,30,30^GFA,2500,2500,25,,:g03IF,Y03KF,Y07KF8,X01IFI0E03F,X07FFJ07JFC,W01FFCJ07KF8,W03FF8I03MF,W0FFEJ0JFC0078,V01FFCI03IFEJ0C,V03FFJ0JF,V07FEI03IF8,V0FFCI07FFE,U03FFI01IF8,U03FEI03FFE,U07FEI07FFC,U0FF8001IF8,T01FFI03FFE,T03FFI07FFC,T07FCI0IF8,T0FFC001IF,T0FF8003FFE,S03FFI07FF8,S03FEI0IF,S07FE001FFE,S0FFC003FFC,S0FF8007FF8,Q061FFI0IF,Q063FE001FFEN03Q02,Q0F7FE003FFEN03Q06,Q0IFC007FFCN018P06,P01IF800IF8N018P0E,P01IF800IFO01CP0E,P03IF001FFEO01CP0E,P03FFE003FFCO01CP0E,P01FFE007FFCO01CP0E,P01FFC00IF8O01CP0E,Q0FB800IFP01CP0E,Q079801FFEP01CP0E,Q0310C3FFC0C006J0C1C00C00E0018F3003,Q0200E7FFC0C01FI01E1C03F00E0038FF00F8,S01E7FF81C07FC007E1C07F80E0038FF03FE,S03JF00C0FFE00FE1C0FFC0E0038FF07FF,S03JF00C0IF01FC1C1FFE0E0038FF07FF8,S07IFE00C1F1F03F01C3E1F0E0038E00F8F8,S0JFC00C3E0783C01C3C0F0E0038E00F03C,S0JFC00C3C0783C01C78078E0038E01E03C,S0JF800C3803C7801C78078E0038E01C01E,S07IFI0C7803C7001C70038E0038E01C01E,S03FFEI0C7001C7001C70038E0038E03800E,:S01F3CI0C7001C7001C6001CE0038E03800E,T0F1CI0C7001C7001C6001CE0038E03800E,T0E18I0C7001C7001C60018E0038E03800E,T04K0C7001C7001C6001CE0038E03800E,g0C7001C7003C7001CE0038E03800E,g0C7001C700387001CF0038E03C00E,g0C7001C780387001C70078F01C01E,g0C7001C780787801C70070701C01E,g0C7001C3C0F83C01C780F0781E03C,g0C7001C3C0F03C01C3C1F07C0F07C,g0C7001C1F3F03F01C3E3E03F0F8F8,g0C7001C1FFE01FC1C1FFC01F07FF8,Y01C7001C0FFC00FC1C1FFC01F07FF,Y01E7001C07FC007E3C0FF800F03FE,g0C7001C01EI01C1803EI0300F8,,:::::g01M08U04,gI08C0808K08N08403,gK02J01S04,hJ06,g02gI04,gO024J02029K04201,gH0C84I082I010I29040084011,gY04,,::::::::::::::::::^FS^FX  Second section with recipient address and permit information.^CFA,30^FO50,150^CF0,30,30^FD" + p1 + "^FS^FO50,190^FD" + p2 + "^FS^FO50,230^FDBat No:" + p3 + "^FS^FO50,270^FDExp Date:" + p4 + "^FS^FO50,270^FDExp Date:" + p5 + "^FS^FO50,270^FDExp Date:" + p6 + "^FS^FO50,270^FDExp Date:" + p7 + "^FS^FO50,270^FDExp Date:" + p8 + "^FS^FO650,10^BQNN,2,5^FD^SN" + totecode + ",1,Y^FS XZ";

            _zplString += "^FO10,10^CF0,30,30^GFA,2500,2500,25,,:g03IF,Y03KF,Y07KF8,X01IFI0E03F,X07FFJ07JFC,W01FFCJ07KF8,W03FF8I03MF,W0FFEJ0JFC0078,V01FFCI03IFEJ0C,V03FFJ0JF,V07FEI03IF8,V0FFCI07FFE,U03FFI01IF8,U03FEI03FFE,U07FEI07FFC,U0FF8001IF8,T01FFI03FFE,T03FFI07FFC,T07FCI0IF8,T0FFC001IF,T0FF8003FFE,S03FFI07FF8,S03FEI0IF,S07FE001FFE,S0FFC003FFC,S0FF8007FF8,Q061FFI0IF,Q063FE001FFEN03Q02,Q0F7FE003FFEN03Q06,Q0IFC007FFCN018P06,P01IF800IF8N018P0E,P01IF800IFO01CP0E,P03IF001FFEO01CP0E,P03FFE003FFCO01CP0E,P01FFE007FFCO01CP0E,P01FFC00IF8O01CP0E,Q0FB800IFP01CP0E,Q079801FFEP01CP0E,Q0310C3FFC0C006J0C1C00C00E0018F3003,Q0200E7FFC0C01FI01E1C03F00E0038FF00F8,S01E7FF81C07FC007E1C07F80E0038FF03FE,S03JF00C0FFE00FE1C0FFC0E0038FF07FF,S03JF00C0IF01FC1C1FFE0E0038FF07FF8,S07IFE00C1F1F03F01C3E1F0E0038E00F8F8,S0JFC00C3E0783C01C3C0F0E0038E00F03C,S0JFC00C3C0783C01C78078E0038E01E03C,S0JF800C3803C7801C78078E0038E01C01E,S07IFI0C7803C7001C70038E0038E01C01E,S03FFEI0C7001C7001C70038E0038E03800E,:S01F3CI0C7001C7001C6001CE0038E03800E,T0F1CI0C7001C7001C6001CE0038E03800E,T0E18I0C7001C7001C60018E0038E03800E,T04K0C7001C7001C6001CE0038E03800E,g0C7001C7003C7001CE0038E03800E,g0C7001C700387001CF0038E03C00E,g0C7001C780387001C70078F01C01E,g0C7001C780787801C70070701C01E,g0C7001C3C0F83C01C780F0781E03C,g0C7001C3C0F03C01C3C1F07C0F07C,g0C7001C1F3F03F01C3E3E03F0F8F8,g0C7001C1FFE01FC1C1FFC01F07FF8,Y01C7001C0FFC00FC1C1FFC01F07FF,Y01E7001C07FC007E3C0FF800F03FE,g0C7001C01EI01C1803EI0300F8,,:::::g01M08U04,gI08C0808K08N08403,gK02J01S04,hJ06,g02gI04,gO024J02029K04201,gH0C84I082I010I29040084011,gY04,,::::::::::::::::::^FS^FX  Second section with recipient address and permit information.^CFA,30^FO50,100^CF0,30,30^FD" + FGPartNumber + "^FS^FO50,130^FD SO#: " + SONumber + "^FS^FO50,160^FD" + CustomerCode + "^FS^FO50,190^FD" + ProductionOrder + "^FS^FO50,220^FD Batch: " + BatchNumber + "^FS^FO50,250^FD Mfg. Dt.: " + MFGDate.ToShortDateString() + "^FS^FO50,280^FD Bale Qty:" + Quantity + "^FS^FO50,310^FD Bale: " + BaleCode + "^FS^FO450,10^BQNN,2,3^FD^SN   " + totecode + ",1,Y^FS";

        }
        public void addIndAutoLogo(int xAxis, int yAxis)
        {
            _zplString += "^FO" + xAxis + "," + yAxis + "^GFA,1518,1518,22,,::::::T0FF8,R01JF8,Q01IFE078,Q0IFCI07JFC,P07FFCI01LFC,P0FFEI01NFC,O03FFJ0JFCJ03,O0FFCI07IFC,N03FF8001IFE,N0FFEI07IF,M01FF8001IFC,M07FFI07IF,M0FFCI0IFC,L03FF8003IF,L07FEI0IFE,L0FFC001IF8,K03FF8007IF,K07FFI0IFC,I018FFC003IFO08,I03DFF8007FFEO0CQ06,I07IF001IFCO0EQ0E,I0IFE003IFP0EP01C,I0IF8007FFEP0EP01C,I07FF800IFCP0EP01E,I03FF003IFQ0EP01C,I01CE107FFEQ0EP01C,J08038IFC1C00FJ0F0E007801C0039FF00F8,L07DIF01C03FE003F0E01FF01C0079FF03FE,L0JFE01C0IF00FF0E07FF81C0079FF07FF,K01JFC01C0IF80FC0E0IFC1C0079ED0FDFC,K03JF801C1E03C1F00E1F03E1C0039C01F03C,K01JF001C3C01E3C00E1E01E1C0079C01C01E,L0IFE001C3C00E3800E3C00F1C0079C03C00E,L07FFC001C7800E7800E380071C0079C03800F,L03E78001C7800E7800E380071C0079E03800F,L01C3I01C7800E7800E380071C0079E03800F,M08J01C7800E7800E380071C0079E03800F,R01C7800E3800E3C0071C0071E03800E,R01C7800E3C01E1C0071E00F0F03C01E,R01C7800E1C03C1E0070E00F0701E01E,R01C7800E1F0FC0F8070FC3E07C1F07C,R01C3800E0IF807F0707FFC03F0IF8,R01C7800E07FE003F8701FF801F03FF,R01C7800E01FCI0F8700FEI0701FC,,:::::::::::::::::^FS";
        }






        /// <summary>
        /// This function will draw a box with provided text
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fontSize"></param>
        /// <param name="boxThickness"></param>
        public void drawBoxWithText(int xAxis, int yAxis, int width, int height, int fontSize, int boxThickness, string strText)
        {
            //drawing box
            _zplString += "^CFF," + fontSize + "^FO" + xAxis + "," + yAxis + "^GB" + width + "," + height + "," + boxThickness + "^FS";

            //drawing text inside box
            int textXaxis = xAxis + 2;
            int textYaxis = yAxis + (height / 2);
            _zplString += "^FO" + textXaxis + "," + textYaxis + "^FD" + strText + "^FS";


        }

        public void drawText(int xAxis, int yAxis, int fontwidth, int fontheight, string strText)
        {
            //drawing box

            _zplString += "^FO" + xAxis + "," + yAxis + "^FPH^FWN^CF0," + fontheight + "," + fontwidth + "^FD" + strText + "^FS";

        }


        public void drawTextWithIncremental_UNIQUERSN(int xAxis, int yAxis, int fontwidth, int fontheight, string strText)
        {
            //drawing box
            _zplString += "^FO" + xAxis + "," + yAxis + "^FPH^FWN^CF0," + fontheight + "," + fontwidth + "^FD^SN" + strText + ",1,Y^FS";

        }



        public void drawBox(int xAxis, int yAxis, int width, int height, int boarderThicknes)
        {
            _zplString += "^FO" + xAxis + "," + yAxis + "^GB" + width + "," + height + "," + boarderThicknes + "^FS";
        }
        public void drawGraphicBox(int xAxis, int yAxis, int width, int height)
        {
            _zplString += "^CFF^FO" + xAxis + "," + yAxis + "^LRY^GB" + width + "," + height + "," + (height / 2) + "^FS";
        }
        public void drawPhoneNumber(int xAxis, int yAxis, int width, int height, string strText)
        {
            _zplString += "^FO" + xAxis + "," + yAxis + "^FPH^FWN^CF0,50,50^FD" + strText + "^FS";
        }
        public void DrawVerticalText(int xAxis, int yAxis, int fontwidth, int fontheight, string strText)
        {
            _zplString += "^CF0," + fontwidth + "," + fontheight + "^FO" + xAxis + "," + yAxis;

            _zplString += "^FPH^FWB^FD" + strText + "^FS";
        }

        public void drawTextWithBigFont(int xAxis, int yAxis, int width, int height, int fontSize, string strText)
        {
            //drawing box
            _zplString += "^CF0,50,40," + fontSize + "^FO" + xAxis + "," + yAxis;

            _zplString += "^FPH^FWN^FD^SN" + strText + ",1,Y^FS";


        }
        public void drawImage(int xaxis, int yaxis, string zplCodeForLogo)
        {
            _zplString += "^FO" + xaxis + "," + yaxis + zplCodeForLogo;

        }

        public string getZPLString()
        {
            return "^XA^LH10,10" + _zplString + "^PQ" + _NumberOfLabels + ",0," + _duplicatePrints + ",Y^XZ";
        }

        public string setgetZPLString(string ZPlstring)
        {
            string ZPLstring = ZPlstring;
            return ZPlstring;
        }
        public void clearScript()
        {
            _zplString = string.Empty;
        }

        /// <summary>
        /// Send the print command to printer
        /// </summary>
        /// <returns>return success or exception message</returns>
        public string printUsingIPnew(string ipAddress, int port, int numberofCopies, int duplicatePrints,string ZPLstring)
        {
            try
            {
                _NumberOfLabels = (numberofCopies * duplicatePrints);
                _duplicatePrints = duplicatePrints;
                this._ipAddress = ipAddress;
                this._port = port;


                // Open connection
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                client.Connect(_ipAddress, _port);

                // Write ZPL String to connection
                System.IO.StreamWriter writer =
                new System.IO.StreamWriter(client.GetStream());
                //getZPLString();
                writer.Write(ZPLstring);
                writer.Flush();
                //getZPLString();
                // Close Connection
                writer.Close();
                client.Close();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //================== ADDED BY M.D.PRASAD ON 25-08-18 ============
        public string printUsingIPnew_ZPL(string ipAddress, int port, int numberofCopies, int duplicatePrints, string ZPLstring)
        {
            try
            {
                _NumberOfLabels = (numberofCopies * duplicatePrints);
                _duplicatePrints = duplicatePrints;
                this._ipAddress = ipAddress;
                this._port = port;
                return ZPLstring;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //================== ADDED BY M.D.PRASAD ON 25-08-18 ============


        public string printUsingIP(string ipAddress, int port, int numberofCopies, int duplicatePrints)
        {
            try
            {
                _NumberOfLabels = (numberofCopies * duplicatePrints);
                _duplicatePrints = duplicatePrints;
                this._ipAddress = ipAddress;
                this._port = port;


                // Open connection
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                client.Connect(_ipAddress, _port);

                // Write ZPL String to connection
                System.IO.StreamWriter writer =
                new System.IO.StreamWriter(client.GetStream());
                //getZPLString();
                writer.Write(getZPLString());
                writer.Flush();
                //getZPLString();
                // Close Connection
                writer.Close();
                client.Close();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //================== ADDED BY M.D.PRASAD ON 25-08-18 ============
        public string printUsingIP_ZPL(string ipAddress, int port, int numberofCopies, int duplicatePrints)
        {
            try
            {
                _NumberOfLabels = (numberofCopies * duplicatePrints);           
                _duplicatePrints = duplicatePrints;
                this._ipAddress = ipAddress;
                this._port = port;
                return getZPLString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string printUsingIP_ZPL_DB(string zplstring,string ipAddress, int port, int numberofCopies, int duplicatePrints)
        {
            try
            {
                _zplString = zplstring;
                _NumberOfLabels = (numberofCopies * duplicatePrints);
                _duplicatePrints = duplicatePrints;
                this._ipAddress = ipAddress;
                this._port = port;
                return getZPLString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        //================== ADDED BY M.D.PRASAD ON 25-08-18 ============

        public string PrintUsingUSB(string printerName, int numberofCopies, int duplicatePrints)
        {

            _NumberOfLabels = (numberofCopies * duplicatePrints);
            _duplicatePrints = duplicatePrints;
            // Allow the user to select a printer.
            try
            {
                bool result = RawPrinterHelper.SendStringToPrinter(printerName, getZPLString());
                return "success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }

        public void drawTextWithIncremental(int xAxis, int yAxis, int fontwidth, int fontheight, string strText)
        {
            //drawing box
            _zplString += "^FO" + xAxis + "," + yAxis + "^FPH^FWN^CF0," + fontheight + "," + fontwidth + "^FD^SN" + strText + ",1,Y^FS";

        }
        public void addBarcodeWithIncremental(int xAxis, int yAxis, string barCodeString, int barCodeHeight, BarcodeType type, bool isChecksumRequired, int barcodeWidth)
        {
            //_zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") + "," + "^FD" + barCodeString + "^FS";
            _zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") + "," + "A" + "," + "^FD^SN" + barCodeString + ",1,Y^FS";
            //^FO110,110^BC,50,,N,^FD1234567890^FS
        }


        public int addBarcodeByDPI(int xAxis, int yAxis, string barCodeString, int barCodeHeight, BarcodeType type, bool isChecksumRequired, int barcodeWidth, bool dpiChaneRequired)
        {
            int incrementalvalue = 0;
            if (dpiChaneRequired)
            {
                barcodeWidth = 2;
                barCodeHeight = barCodeHeight + 30;
                incrementalvalue = 50;
            }

            _zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") + "," + "^FD" + barCodeString + "^FS";
            //_zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") +","+"A"+","+ "^FD^SN" + barCodeString + ",1,Y^FS";
            //^FO110,110^BC,50,,N,^FD1234567890^FS

            return incrementalvalue;

        }

        public int drawTextByDPI(int xAxis, int yAxis, int fontwidth, int fontheight, string strText, bool dpiChaneRequired)
        {
            int incrementedValue = 0;
            //drawing box
            // Changing 203 dpi to 300
            if (dpiChaneRequired)
            {
                decimal widthValue = Convert.ToDecimal(fontwidth) / Convert.ToDecimal(203);

                decimal heightValue = Convert.ToDecimal(fontheight) / Convert.ToDecimal(203);
                fontwidth = Convert.ToInt32((widthValue * 300));
                incrementedValue = (Convert.ToInt32((heightValue * 300)) - fontheight);
                fontheight = Convert.ToInt32((heightValue * 300));


            }
            _zplString += "^FO" + xAxis + "," + yAxis + "^FPH^FWN^CF0," + fontheight + "," + fontwidth + "^FD" + strText + "^FS";

            return incrementedValue;
        }



        public void addBarcodeWithAutoScale(int xAxis, int yAxis, string barCodeString, int barCodeHeight, BarcodeType type, bool isChecksumRequired, int barcodeWidth)
        {
            _zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") + "," + "A" + "," + "^FD" + barCodeString + "^FS";
            //_zplString += "^FPH^FWN^FO" + xAxis + "," + yAxis + "^BY" + barcodeWidth + barCodeType[type.ToString()] + "," + barCodeHeight + "," + (isChecksumRequired ? "Y" : "N") +","+"A"+","+ "^FD^SN" + barCodeString + ",1,Y^FS";
            //^FO110,110^BC,50,,N,^FD1234567890^FS
        }

        public void drawLine(int xAxis, int yAxis, int width, int height, int boxThickness)
        {
            _zplString += "^FO" + xAxis + "," + yAxis + "^GB" + width + "," + height + "," + boxThickness + "^FS";

        }



    }
}
