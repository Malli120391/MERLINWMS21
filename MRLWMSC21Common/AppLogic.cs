//#define SMTPDOTNET
#define QQMAIL

using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;
using System.Net.Mail;
using System.Web.Util;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security.Principal;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using System.Drawing;
using System.Xml.Serialization;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;

using System.Web.Mail;


using System.Web.Configuration;

#if QQMAIL
using qqMail;
#endif

#if SMTPDOTNET
using SmtpDotNet;
#endif

//using AspDotNetStorefrontEncrypt;

namespace MRLWMSC21Common
{
    /// <summary>
    /// Summary description for AppLogic.
    /// </summary>
    public sealed class AppLogic
    {

        static public int NumProductsInDB = 0; // set to # of products in the db on Application_Start. Not updated thereafter
        static public bool CachingOn = false;  // set to true in Application_Start if AppConfig:CacheMenus=true

        static public Hashtable AppConfigTable; // LOADED in application_start of the respective web project
        static public Hashtable StringResourceTable; // Hashtable OF Hashtables. First Hashtable index is LocaleSetting. Second Hashtable index is String resource name
        static private Hashtable ImageFilenameCache = new Hashtable(); // Caching is ALWAYS on for images, cache of category/section/product/etc image filenames from LookupImage. Added on first need
        // the innova admin HTML editor adds images so they can be viewed in the admin site, so set this to true (via AppConfig) 
        // if you want ../images to be replaced with images so images resolve on the store side, applies to any HTML field in the db.
        static public bool ReplaceImageURLFromAssetMgr = false;
        static public XmlDocument Customerlevels;

        static public int MicropayProductID = 0;
        static public int MicropayVariantID = 0;
        static public int AdHocProductID = 0;
        static public int AdHocVariantID = 0;

        static public readonly String ro_DefaultProductXmlPackage = "product.variantsinrightbar.xml.config";
        static public readonly String ro_DefaultProductPackXmlPackage = "product.packproduct.xml.config";
        static public readonly String ro_DefaultProductKitXmlPackage = "product.kitproduct.xml.config";
        static public readonly String ro_DefaultEntityXmlPackage = "entity.grid.xml.config";

        static public readonly String ro_CCNotStoredString = "Not Stored";
        static public readonly String ro_TXModeAuthCapture = "AUTH CAPTURE";
        static public readonly String ro_TXModeAuthOnly = "AUTH";
        static public readonly String ro_TXStateAuthorized = "AUTHORIZED";
        static public readonly String ro_TXStateCaptured = "CAPTURED";
        static public readonly String ro_TXStateVoided = "VOIDED";
        static public readonly String ro_TXStateRefunded = "REFUNDED";
        static public readonly String ro_TXStateFraud = "FRAUD";
        static public readonly String ro_TXStateUnknown = "UNKNOWN"; // possible, but not used
        static public readonly String ro_TXStatePending = "PENDING"; // possible, but not used at this time. authorize.net is only gateway to possibly return this

        static public readonly String ro_OK = "OK";

        static public readonly String ro_SKUMicropay = "MICROPAY";

        static public readonly String ro_PMMicropay = "MICROPAY";
        static public readonly String ro_PMCreditCard = "CREDITCARD";
        static public readonly String ro_PMECheck = "ECHECK";
        static public readonly String ro_PMRequestQuote = "REQUESTQUOTE";
        static public readonly String ro_PMCOD = "COD";
        static public readonly String ro_PMCODMoneyOrder = "CODMONEYORDER";
        static public readonly String ro_PMCODCompanyCheck = "CODCOMPANYCHECK";
        static public readonly String ro_PMCODNet30 = "CODNET30";
        static public readonly String ro_PMPurchaseOrder = "PURCHASEORDER";
        static public readonly String ro_PMPayPal = "PAYPAL";
        static public readonly String ro_PMPayPalExpress = "PAYPALEXPRESS";
        static public readonly String ro_PMCheckByMail = "CHECKBYMAIL";
        static public readonly String ro_PMBypassGateway = "BYPASSGATEWAY";

        // these are pulled from string resources:
        static public readonly String ro_PMMicropayForDisplay = "(!pm.micropay.display!)";
        static public readonly String ro_PMCreditCardForDisplay = "(!pm.creditcard.display!)";
        static public readonly String ro_PMECheckForDisplay = "(!pm.echeck.display!)";
        static public readonly String ro_PMRequestQuoteForDisplay = "(!pm.requestquote.display!)";
        static public readonly String ro_PMCODForDisplay = "(!pm.cod.display!)";
        static public readonly String ro_PMCODMoneyOrderForDisplay = "(!pm.codmoneyorder.display!)";
        static public readonly String ro_PMCODCompanyCheckForDisplay = "(!pm.codcompanycheck.display!)";
        static public readonly String ro_PMCODNet30ForDisplay = "(!pm.codnet30.display!)";
        static public readonly String ro_PMPurchaseOrderForDisplay = "(!pm.purchaseorder.display!)";
        static public readonly String ro_PMPayPalForDisplay = "(!pm.paypal.display!)";
        static public readonly String ro_PMPayPalExpressForDisplay = "(!pm.paypalexpress.display!)";
        static public readonly String ro_PMCheckByMailForDisplay = "(!pm.checkbymail.display!)";
        static public readonly String ro_PMBypassGatewayForDisplay = "(!pm.bypassgateway.display!)";

       
        public AppLogic() { }

        static public bool IsAdminSite
        {
            get
            {
                //return CommonLogic.GetThisPageName(true).ToLowerInvariant().IndexOf(AppLogic.AppConfig("AdminDir").ToLowerInvariant() + "/") != -1;
                //bool ia = CommonLogic.ApplicationBool("IsAdminSite");
              //  bool ia = (String)HttpContext.Current.Items["IsAdminSite"] == "true";
                return true;
            }
        }

     
        static public bool IPIsRestricted(String IPAddress)
        {
            return (DB.GetSqlN("select count(*) as N from RestrictedIP where IPAddress=" + DB.SQuote(IPAddress)) > 0);
        }

    
        public static void ApplicationStart()
        {
            AppLogic.ClearCache();
            HttpContext.Current.Application.Clear();
            HttpContext.Current.Application.RemoveAll();
          
            AppLogic.LoadStringResourcesFromDB(true);
            AppLogic.AppConfigTable = AppLogic.LoadAppConfigsFromDB();

            if (CommonLogic.Application("EncryptKey").Length == 0 || CommonLogic.Application("EncryptKey") == "WIZARD" || CommonLogic.Application("EncryptKey") == "TBD")
            {
                throw new ArgumentException("You must enter your EncryptKey in your web.config and /admin/web.config files. The EncryptKey in both files must be exactly the same! Open the web.config files in Notepad, and see the instructions.");
            }
            AppLogic.ReplaceImageURLFromAssetMgr = AppLogic.AppConfigBool("ReplaceImageURLFromAssetMgr");
            AppLogic.CachingOn = AppLogic.AppConfigBool("CacheMenus");
           
            ImageFilenameCache.Clear();
            string CustLevelXml = string.Empty;
          
            Customerlevels = new XmlDocument();
            Customerlevels.LoadXml(CustLevelXml);
           
        }

    
        // input CardNumber can be in plain text or encrypted, doesn't matter:
        public static String SafeDisplayCardNumber(String CardNumber)
        {
            String CardNumberDecrypt = AppLogic.UnmungeString(CardNumber);
            if (CardNumberDecrypt.StartsWith("Error"))
            {
                CardNumberDecrypt = CardNumber;
            }
            if (CardNumberDecrypt == AppLogic.ro_CCNotStoredString)
            {
                return String.Empty;
            }
            if (CardNumberDecrypt.Length > 4)
            {
                //return "*".PadLeft(CardNumberDecrypt.Length - 4, '*') + CardNumberDecrypt.Substring(CardNumberDecrypt.Length - 4, 4);
                return "****" + CardNumberDecrypt.Substring(CardNumberDecrypt.Length - 4, 4);
            }
            else
            {
                return String.Empty;
            }
        }

        // input CardExtraCode can be in plain text or encrypted, doesn't matter:
        public static String SafeDisplayCardExtraCode(String CardExtraCode)
        {
            if (CardExtraCode.Length == 0)
            {
                return String.Empty;
            }
            String CardExtraCodeDecrypt = AppLogic.UnmungeString(CardExtraCode);
            if (CardExtraCodeDecrypt.StartsWith("Error"))
            {
                CardExtraCodeDecrypt = CardExtraCode;
            }
            return "*".PadLeft(CardExtraCodeDecrypt.Length, '*');
        }

        // returns empty string, or decrypted card extra code from appropriate session state location
        public static String GetCardExtraCodeFromSession(int CustomerID)
        {
            String CardExtraCode = String.Empty;
            if (CommonLogic.ApplicationBool("ServerFarm"))
            {
                CardExtraCode = CustomerSession.StaticGetVal("CardExtraCode", CustomerID);
            }
            else
            {
                CardExtraCode = CommonLogic.Session("CardExtraCode");
            }
            if (CardExtraCode.Length != 0)
            {
                CardExtraCode = AppLogic.UnmungeString(CardExtraCode);
            }
            return CardExtraCode;
        }

        public static void StoreCardExtraCodeInSession(String CardExtraCode, int CustomerID)
        {
            if (!CardExtraCode.StartsWith("*"))
            {
                if (CommonLogic.ApplicationBool("ServerFarm"))
                {
                    if (CardExtraCode.Length == 0)
                    {
                        CustomerSession.StaticClearVal("CardExtraCode", CustomerID);
                    }
                    else
                    {
                        CustomerSession sess = new CustomerSession(CustomerID);
                        sess.SetVal("CardExtraCode", AppLogic.MungeString(CardExtraCode), System.DateTime.Now.AddHours(1));
                        sess = null;
                    }
                }
                else
                {
                    HttpContext.Current.Session["CardExtraCode"] = AppLogic.MungeString(CardExtraCode);
                }
            }
        }

        // input CardNumber can be in plain text or encrypted, doesn't matter:
        public static String AdminViewCardNumber(String CardNumber)
        {
            String CardNumberDecrypt = AppLogic.UnmungeString(CardNumber);
            if (CardNumberDecrypt.StartsWith("Error"))
            {
                CardNumberDecrypt = CardNumber;
            }
            if (AppLogic.IsAdminSite)
            {
                return CardNumberDecrypt;
            }
            else
            {
                return SafeDisplayCardNumber(CardNumber);
            }
        }

        // input CardNumber can be in plain text or encrypted, doesn't matter:
        public static String SafeDisplayCardNumberLast4(String CardNumber)
        {
            String CardNumberDecrypt = AppLogic.UnmungeString(CardNumber);
            if (CardNumberDecrypt.StartsWith("Error"))
            {
                CardNumberDecrypt = CardNumber;
            }
            if (CardNumberDecrypt == AppLogic.ro_CCNotStoredString)
            {
                return String.Empty;
            }
            if (CardNumberDecrypt.Length >= 4)
            {
                return CardNumberDecrypt.Substring(CardNumberDecrypt.Length - 4, 4);
            }
            else
            {
                return String.Empty;
            }
        }

     

        public static string ImportProductList(string importtext)
        {
            if (importtext.Trim() == String.Empty)
            {
                return "No data to import";
            }

            using (SqlConnection dbconn = new SqlConnection())
            {
                dbconn.ConnectionString = DB.GetDBConn();
                dbconn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = dbconn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "aspdnsf_ImportProductPricing_XML";

                    cmd.Parameters.Add(new SqlParameter("@pricing", SqlDbType.NText));

                    cmd.Parameters["@pricing"].Value = importtext;

                    cmd.ExecuteNonQuery();
                }
                    dbconn.Close();
                    return String.Empty;
               
            }

        }
        static public int MaxMenuSize()
        {
            int tmp = AppLogic.AppConfigUSInt("MaxMenuSize");
            if (tmp == 0)
            {
                tmp = 25;
            }
            return tmp;
        }

     
        // returns comma separate list of skin id's found on the web site, e.g. 1,2,3,4
        static public String FindAllSkins()
        {
            String CacheName = "FindAllSkins";
            if (AppLogic.CachingOn)
            {
                String s = (String)HttpContext.Current.Cache.Get(CacheName);
                if (s != null)
                {
                    return s;
                }
            }
            StringBuilder tmpS = new StringBuilder(1024);
            int MaxNumberSkins = AppLogic.AppConfigUSInt("MaxNumberSkins");
            if (MaxNumberSkins == 0)
            {
                MaxNumberSkins = 10;
            }
            for (int i = 0; i <= 100; i++)
            {
                String FN = CommonLogic.SafeMapPath(CommonLogic.IIF(AppLogic.IsAdminSite, "../", String.Empty) + "skins/skin_" + i.ToString() + "/template.ascx");
                if (CommonLogic.FileExists(FN))
                {
                    if (tmpS.Length != 0)
                    {
                        tmpS.Append(",");
                    }
                    tmpS.Append(i.ToString());
                }
            }

            if (CachingOn)
            {
                HttpContext.Current.Cache.Insert(CacheName, tmpS.ToString(), null, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()), TimeSpan.Zero);
            }
            return tmpS.ToString();
        }

   

        public static int SessionTimeout()
        {
            int ST = AppLogic.AppConfigUSInt("SessionTimeoutInMinutes");
            if (ST == 0)
            {
                ST = 20;
            }
            return ST;
        }

        public static int CacheDurationMinutes()
        {
            int ST = AppLogic.AppConfigUSInt("CacheDurationMinutes");
            if (ST == 0)
            {
                ST = 20;
            }
            return ST;
        }

        static public String GetUserMenu(bool IsAnon, int SkinID, String m_LocaleSetting)
        {
            StringBuilder tmpS = new StringBuilder(1000);
            tmpS.Append("<div id=\"userMenu\" class=\"menu\">\n");
            if (IsAnon)
            {
                tmpS.Append("<a class=\"menuItem\" href=\"signin.aspx\">" + GetString("skinbase.cs.4", SkinID, m_LocaleSetting) + "</a>\n");
                tmpS.Append("<a class=\"menuItem\" href=\"account.aspx\">" + GetString("skinbase.cs.6", SkinID, m_LocaleSetting) + "</a>\n");
            }
            else
            {
                tmpS.Append("<a class=\"menuItem\" href=\"account.aspx\">" + GetString("skinbase.cs.7", SkinID, m_LocaleSetting) + "</a>\n");
                tmpS.Append("<a class=\"menuItem\" href=\"signout.aspx\">" + GetString("skinbase.cs.5", SkinID, m_LocaleSetting) + "</a>\n");
            }
            tmpS.Append("</div>\n");
            return tmpS.ToString();
        }

        static public String NoPictureImageURL(bool icon, int SkinID, String LocaleSetting)
        {
            return AppLogic.LocateImageURL("skins/skin_" + SkinID.ToString() + "/images/nopicture" + CommonLogic.IIF(icon, "icon", String.Empty) + ".gif", LocaleSetting);
        }

        // given an input image string like /skins/skin_1/images/shoppingcart.gif
        // tries to resolve it to the proper locale by:
        // /skins/skin_1/images/shoppingcart.LocaleSetting.gif first
        // /skins/skin_1/images/shoppingcart.WebConfigLocale.gif second
        // /skins/skin_1/images/shoppingcart.gif last
        static public String LocateImageURL(String ImageName, String LocaleSetting)
        {
            String CacheName = "LocateImageURL_" + ImageName + "_" + LocaleSetting;
            if (AppLogic.CachingOn)
            {
                String s = (String)HttpContext.Current.Cache.Get(CacheName);
                if (s != null)
                {
                    if (CommonLogic.ApplicationBool("DumpSQL"))
                    {
                        HttpContext.Current.Response.Write("Cache Hit Found!<br />\n");
                    }
                    return s;
                }
            }
            int i = ImageName.LastIndexOf(".");
            String url = String.Empty;
            if (i == -1)
            {
                url = ImageName; // no extension??
            }
            else
            {
                String Extension = ImageName.Substring(i);
                url = ImageName.Substring(0, i) + "." + LocaleSetting + Extension;
                if (!CommonLogic.FileExists(url))
                {
                    url = ImageName.Substring(0, i) + "." + Localization.GetWebConfigLocale() + Extension;
                }
                if (!CommonLogic.FileExists(url))
                {
                    url = ImageName.Substring(0, i) + Extension;
                }
            }
            if (CachingOn)
            {
                HttpContext.Current.Cache.Insert(CacheName, url, null, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()), TimeSpan.Zero);
            }
            return url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImageName">Image filename with or without the extension</param>
        /// <param name="ImageType">e.g. Category, Section, Product</param>
        /// <param name="LocaleSetting">Viewing Locale</param>
        /// <returns>full path to the image</returns>
        static private String LocateImageURL(String ImageName, String ImageType, String ImgSize, String LocaleSetting)
        {
            try
            {
                ImageName = ImageName.Trim();
                string WebConfigLocale = "." + Localization.GetWebConfigLocale();
                string IPath = GetImagePath(ImageType, ImgSize, true);
                if (LocaleSetting.Trim() != String.Empty)
                {
                    LocaleSetting = "." + LocaleSetting;
                }
                bool UseCache = !IsAdminSite;

                //Used for ImageFilenameOverride
                if (ImageName.ToLower().EndsWith(".jpg") || ImageName.ToLower().EndsWith(".gif") || ImageName.ToLower().EndsWith(".png"))
                {
                    String[] imagepaths = { IPath + ImageName.Replace(".", LocaleSetting + "."), IPath + ImageName.Replace(".", WebConfigLocale + "."), IPath + ImageName };
                    foreach (string ImagePath in imagepaths)
                    {
                        if (UseCache && ImageFilenameCache.ContainsKey(ImagePath) && ((String)ImageFilenameCache[ImagePath]).Length > 1)
                        {
                            return (String)ImageFilenameCache[ImagePath];
                        }
                        else if (CommonLogic.FileExists(ImagePath))
                        {
                            if (UseCache)
                            {
                                ImageFilenameCache[ImagePath] = GetImagePath(ImageType, ImgSize, false) + ImageName;
                                return (String)ImageFilenameCache[ImagePath];
                            }
                            else
                            {
                                return GetImagePath(ImageType, ImgSize, false) + ImageName;
                            }
                        }
                        if (UseCache && (ImageFilenameCache[ImagePath] == null || (String)ImageFilenameCache[ImagePath] == String.Empty)) ImageFilenameCache[ImagePath] = "0";
                    }
                    return String.Empty;
                }
                else //all other image name formats (i.e. productid, sku)
                {
                    String[] imageext = { ".jpg", ".gif", ".png" };
                    foreach (string ext in imageext)
                    {
                        String[] locales = { LocaleSetting, WebConfigLocale, String.Empty };
                        foreach (string locale in locales)
                        {
                            string ImagePath = IPath + ImageName + locale + ext;
                            if (UseCache && ImageFilenameCache.ContainsKey(ImagePath) && ((String)ImageFilenameCache[ImagePath]).Length > 1)
                            {
                                return (String)ImageFilenameCache[ImagePath];
                            }
                            else if (CommonLogic.FileExists(ImagePath))
                            {
                                if (UseCache)
                                {
                                    ImageFilenameCache[ImagePath] = GetImagePath(ImageType, ImgSize, false) + ImageName + locale + ext;
                                    return (String)ImageFilenameCache[ImagePath];
                                }
                                else
                                {
                                    return GetImagePath(ImageType, ImgSize, false) + ImageName + locale + ext;
                                }
                            }
                            if (UseCache && (ImageFilenameCache[ImagePath] == null || (String)ImageFilenameCache[ImagePath] == String.Empty)) ImageFilenameCache[ImagePath] = "0";
                        }
                    }
                    return String.Empty;
                }
            }
            catch
            {
                return String.Empty;
            }

        }

        static public String LocateImageURL(String ImageName)
        {
            return AppLogic.LocateImageURL(ImageName, Thread.CurrentThread.CurrentUICulture.Name);
        }

        static private String LocateImageURLAK(int CountryID, int CategoryID, String ImageName, String ImageType, String ImgSize, String LocaleSetting)
        {
            try
            {
                ImageFilenameCache.Clear();

                ImageName = ImageName.Trim();
                string WebConfigLocale = "." + Localization.GetWebConfigLocale();
                string IPath = GetImagePathAK(CountryID, CategoryID,ImgSize, true);
                if (LocaleSetting.Trim() != String.Empty)
                {
                    LocaleSetting = "." + LocaleSetting;
                }
                bool UseCache = !IsAdminSite;

                //Used for ImageFilenameOverride
                if (ImageName.ToLower().EndsWith(".jpg") || ImageName.ToLower().EndsWith(".gif") || ImageName.ToLower().EndsWith(".png"))
                {
                    String[] imagepaths = { IPath + ImageName.Replace(".", LocaleSetting + "."), IPath + ImageName.Replace(".", WebConfigLocale + "."), IPath + ImageName };
                    foreach (string ImagePath in imagepaths)
                    {
                        if (UseCache && ImageFilenameCache.ContainsKey(ImagePath) && ((String)ImageFilenameCache[ImagePath]).Length > 1)
                        {
                            return (String)ImageFilenameCache[ImagePath];
                        }
                        else if (CommonLogic.FileExists(ImagePath))
                        {
                            if (UseCache)
                            {

                                ImageFilenameCache[ImagePath] = GetImagePathAK(CountryID, CategoryID,ImgSize, false) + ImageName;
                                return (String)ImageFilenameCache[ImagePath];
                            }
                            else
                            {
                                return GetImagePath(ImageType, ImgSize, false) + ImageName;
                            }
                        }
                        if (UseCache && (ImageFilenameCache[ImagePath] == null || (String)ImageFilenameCache[ImagePath] == String.Empty)) ImageFilenameCache[ImagePath] = "0";
                    }
                    return String.Empty;
                }
                else //all other image name formats (i.e. productid, sku)
                {
                    String[] imageext = { ".jpg", ".gif", ".png" };
                    foreach (string ext in imageext)
                    {
                        String[] locales = { LocaleSetting, WebConfigLocale, String.Empty };
                        foreach (string locale in locales)
                        {
                            string ImagePath = IPath + ImageName + locale + ext;
                            if (UseCache && ImageFilenameCache.ContainsKey(ImagePath) && ((String)ImageFilenameCache[ImagePath]).Length > 1)
                            {
                                return (String)ImageFilenameCache[ImagePath];
                            }
                            else if (CommonLogic.FileExists(ImagePath))
                            {
                                if (UseCache)
                                {
                                    ImageFilenameCache[ImagePath] = GetImagePathAK(CountryID, CategoryID, ImgSize, false) + ImageName + locale + ext;
                                    return (String)ImageFilenameCache[ImagePath];
                                }
                                else
                                {
                                    return GetImagePathAK(CountryID, CategoryID, ImgSize, false) + ImageName + locale + ext;
                                }
                            }
                            if (UseCache && (ImageFilenameCache[ImagePath] == null || (String)ImageFilenameCache[ImagePath] == String.Empty)) ImageFilenameCache[ImagePath] = "0";
                        }
                    }
                    return String.Empty;
                }
            }
            catch
            {
                return String.Empty;
            }

        }


        static public String WriteTabbedContents(String tabDivName, int selectedTabIdx, bool includeTabJSDriverFile, String[] names, String[] values)
        {
            StringBuilder tmpS = new StringBuilder(10000);
            if (includeTabJSDriverFile)
            {
                tmpS.Append(CommonLogic.ReadFile("jscripts/tabs.js", true));
            }
            tmpS.Append("<div class=\"tab-container\" id=\"" + tabDivName + "\" width=\"100%\">\n");
            tmpS.Append("<ul class=\"tabs\">\n");
            for (int i = names.GetLowerBound(0); i <= names.GetUpperBound(0); i++)
            {
                tmpS.Append("<li><a href=\"#\" onClick=\"return showPane('" + tabDivName + "_pane" + (i + 1).ToString() + "', this)\" id=\"" + tabDivName + "_tab" + (i + 1).ToString() + "\">" + names[i] + "</a></li>\n");
            }
            tmpS.Append("</ul>\n");
            tmpS.Append("<div class=\"tab-panes\" width=\"100%\">\n");
            for (int i = names.GetLowerBound(0); i <= names.GetUpperBound(0); i++)
            {
                tmpS.Append("<div id=\"" + tabDivName + "_pane" + (i + 1).ToString() + "\" width=\"100%\" style=\"overflow:auto;height:600px;\">\n");
                tmpS.Append(values[i]);
                tmpS.Append("</div>\n");
            }
            tmpS.Append("</div>\n");
            tmpS.Append("</div>\n");
            tmpS.Append("<script language=\"JavaScript1.3\">\nsetupPanes('" + tabDivName + "', '" + tabDivName + "_tab" + CommonLogic.IIF(selectedTabIdx == 0, "1", selectedTabIdx.ToString()) + "');\n</script>\n");
            return tmpS.ToString();
        }

        static public String GenerateInnovaEditor(String FormFieldName, String EditorMode)
        {
            /*
            if (!AppLogic.AppConfigBool("TurnOffHtmlEditorInAdminSite"))
            {
                String tmpS = CommonLogic.ReadFile(CommonLogic.SafeMapPath("scripts/innova.js"), false);
                tmpS = tmpS.Replace("(!FormFieldName!)", FormFieldName);
                return tmpS;
            }
            else
            {
                return String.Empty;
            }
            */

            String tmpS = CommonLogic.ReadFile(CommonLogic.SafeMapPath("jscripts/innova.js"), false);
            tmpS = tmpS.Replace("(!FormFieldName!)", FormFieldName);
            return tmpS;
        }

        static public String GetLocaleEntryFields(String fieldVal, String baseFormFieldName, bool useTextArea, bool htmlEncodeIt, bool isRequired, String requiredFieldMissingPrompt, int maxLength, int displaySize, int displayRows, int displayCols, bool HTMLOk,bool IsDefaultText, String DefaultText)
        {
            String MasterLocale = Localization.GetWebConfigLocale();
            StringBuilder tmpS = new StringBuilder(4096);
            String ThisLocale = String.Empty;

            if (displayRows == 0)
            {
                displayRows = 5;
            }
            if (displayCols == 0)
            {
                displayCols = 80;
            }

            if (AppLogic.NumLocaleSettingsInstalled() < 2)
            {
                // for only 1 locale, just store things directly for speed:
                ThisLocale = MasterLocale;
                String FormFieldName = baseFormFieldName; // + "_" + ThisLocale.Replace("-","_");
                String ThisLocaleValue = fieldVal; // XmlCommon.GetLocaleEntry(fieldVal,ThisLocale,false);
                if (fieldVal.StartsWith("<ml>") || fieldVal.StartsWith("&lt;ml&gt;"))
                {
                    ThisLocaleValue = XmlCommon.GetLocaleEntry(fieldVal, ThisLocale, false);
                }
                if (htmlEncodeIt)
                {
                    ThisLocaleValue = HttpContext.Current.Server.HtmlEncode(ThisLocaleValue);
                }
                if (useTextArea)
                {
                    tmpS.Append("<div id=\"id" + FormFieldName + "\" style=\"height: 1%;\">");
                    tmpS.Append("<textarea style=\"width: 100%;\" rows=\"" + displayRows.ToString() + "\" id=\"" + FormFieldName + "\" name=\"" + FormFieldName + "\">" + ThisLocaleValue);
                    if (ThisLocaleValue.Length<=0 && IsDefaultText)
                        {
                            tmpS.Append(DefaultText);
                        }
                    tmpS.Append("</textarea>\n");
                    if (HTMLOk && !AppLogic.AppConfigBool("TurnOffHtmlEditorInAdminSite"))
                    {
                        tmpS.Append(AppLogic.GenerateInnovaEditor(FormFieldName, "HTMLBody"));
                    }
                    tmpS.Append("</div>");
                }
                else
                {
                    tmpS.Append("<input maxLength=\"" + maxLength + "\" size=\"" + displaySize + "\" id=\"" + FormFieldName + "\" name=\"" + FormFieldName + "\" value=\"");
                    if (ThisLocaleValue.Length <= 0 && IsDefaultText)
                    {
                        tmpS.Append(DefaultText + "\">");
                    }
                    else
                    {
                        tmpS.Append( ThisLocaleValue + "\">");
                    }
                }
                if (isRequired)
                {
                    tmpS.Append("<input type=\"hidden\" name=\"" + FormFieldName + "_vldt\" value=\"[req][blankalert=" + requiredFieldMissingPrompt + " (" + ThisLocale + ")]\">\n");
                }
            }
            else
            {
                DataSet ds = DB.GetDS("select * from LocaleSetting  " + DB.GetNoLock() + " order by displayorder,description", true, System.DateTime.Now.AddHours(1));

                tmpS.Append("<div class=\"tab-container\" id=\"" + baseFormFieldName + "Div\">\n");
                tmpS.Append("<ul class=\"tabs\">\n");
                int i = 1;
                int SelIdx = 1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    tmpS.Append("<li><a href=\"#\" onClick=\"return showPane('" + baseFormFieldName + "_pane" + i.ToString() + "', this)\" id=\"" + baseFormFieldName + "_tab" + i.ToString() + "\"><span>" + DB.RowField(row, "Description") + "</span></a></li>\n");
                    if (DB.RowField(row, "Name") == MasterLocale)
                    {
                        SelIdx = i;
                    }
                    i++;
                }
                tmpS.Append("</ul>\n");

                i = 1;
                tmpS.Append("<div class=\"tab-panes\">\n");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    tmpS.Append("<div id=\"" + baseFormFieldName + "_pane" + i.ToString() + "\">\n");
                    ThisLocale = DB.RowField(row, "Name");
                    String FormFieldName = baseFormFieldName + "_" + ThisLocale.Replace("-", "_");
                    String ThisLocaleValue = XmlCommon.GetLocaleEntry(fieldVal, ThisLocale, false);
                    if (htmlEncodeIt)
                    {
                        ThisLocaleValue = HttpContext.Current.Server.HtmlEncode(ThisLocaleValue);
                    }

                    if (useTextArea)
                    {
                        tmpS.Append("<div id=\"id" + FormFieldName + "\" style=\"height: 1%;\">");
                        tmpS.Append("<textarea rows=\"" + displayRows.ToString() + "\" cols=\"" + displayCols.ToString() + "\" id=\"" + FormFieldName + "\" name=\"" + FormFieldName + "\">" + ThisLocaleValue + "</textarea>\n");
                        if (HTMLOk && !AppLogic.AppConfigBool("TurnOffHtmlEditorInAdminSite"))
                        {
                            tmpS.Append(AppLogic.GenerateInnovaEditor(FormFieldName, "HTMLBody"));
                        }
                        tmpS.Append("</div>");
                    }
                    else
                    {
                        tmpS.Append("<input maxLength=\"" + maxLength + "\" size=\"" + displaySize + "\" id=\"" + FormFieldName + "\" name=\"" + FormFieldName + "\" value=\"" + ThisLocaleValue + "\">");
                    }

                    if (isRequired && ThisLocale == MasterLocale)
                    {
                        tmpS.Append("<input type=\"hidden\" name=\"" + FormFieldName + "_vldt\" value=\"[req][blankalert=" + requiredFieldMissingPrompt + " (" + ThisLocale + ")]\">\n");
                    }
                    //tmpS.Append(" (" + ThisLocale + ")");
                    tmpS.Append("\n");
                    tmpS.Append("</div>\n");
                    i++;
                }
                tmpS.Append("</div>\n");
                tmpS.Append("</div>\n");
                ds.Dispose();
                tmpS.Append("\n<script language=\"JavaScript1.3\">\nsetupPanes('" + baseFormFieldName + "Div', '" + baseFormFieldName + "_tab" + SelIdx.ToString() + "');\n</script>");
            }
            return tmpS.ToString();
        }

        static public void UpdateNumLocaleSettingsInstalled()
        {
#if PRO
			// only ML feature
#else
            String CacheName = "NumLocaleSettingsInstalled";
            int N = DB.GetSqlN("select count(*) as N from LocaleSetting" + DB.GetNoLock());
            HttpContext.Current.Cache.Insert(CacheName, N.ToString(), null, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()), TimeSpan.Zero);
#endif
        }

        static public int NumLocaleSettingsInstalled()
        {
            int N = 0; // can't ever be 0 really ;)
#if PRO
			N = 1;
#else
            String CacheName = "NumLocaleSettingsInstalled";
            if (AppLogic.CachingOn)
            {
                String s = (String)HttpContext.Current.Cache.Get(CacheName);
                if (s != null)
                {
                    if (CommonLogic.ApplicationBool("DumpSQL"))
                    {
                        HttpContext.Current.Response.Write("Cache Hit Found!<br />\n");
                    }
                    N = Localization.ParseUSInt(s);
                }
            }
            if (N == 0)
            {
                N = DB.GetSqlN("select count(*) as N from LocaleSetting " + DB.GetNoLock());
            }
            if (N == 0)
            {
                N = 1;
            }
            if (CachingOn)
            {
                HttpContext.Current.Cache.Insert(CacheName, N.ToString(), null, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()), TimeSpan.Zero);
            }

#endif
            return N;
        }

        static public String FormLocaleXml(String baseFormFieldName)
        {
            if (AppLogic.NumLocaleSettingsInstalled() > 1)
            {
                StringBuilder tmpS = new StringBuilder(4096);
                tmpS.Append("<ml>");
                DataSet ds = DB.GetDS("select * from LocaleSetting  " + DB.GetNoLock() + " order by displayorder,description", true, System.DateTime.Now.AddHours(1));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    String ThisLocale = DB.RowField(row, "Name");
                    String FormFieldName = baseFormFieldName + "_" + ThisLocale.Replace("-", "_");
                    String FormFieldVal = CommonLogic.Form(FormFieldName);
                    if (FormFieldVal.Length != 0)
                    {
                        tmpS.Append("<locale name=\"" + ThisLocale + "\">");
                        tmpS.Append(XmlCommon.XmlEncode(FormFieldVal));
                        tmpS.Append("</locale>");
                    }
                }
                ds.Dispose();
                tmpS.Append("</ml>");
                return tmpS.ToString();
            }
            else
            {
                return CommonLogic.Form(baseFormFieldName);
            }
        }

   
   
        static public String MungeString(String s)
        {
            String EncryptKey = CommonLogic.Application("EncryptKey");
            if (EncryptKey.Trim().ToUpperInvariant() == "REGISTRY")
            {
                try
                {
                    WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfig("EncryptKey.RegistryLocation"));
                    EncryptKey = reg.Read(AppLogic.AppConfig("EncryptKey.RegistryKey"));
                    reg = null;
                }
                catch (System.Security.SecurityException)
                {
                    throw new Exception("Cannot read registry values, probably the site is running in Medium Trust");
                }
            }
            if (EncryptKey.Trim().ToUpperInvariant() == "CONFIG")
            {
                EncryptKey = System.Web.Configuration.WebConfigurationManager.AppSettings[AppLogic.AppConfig("EncryptKey.RegistryKey")];
            }
            String tmpS = Encrypt.EncryptData(EncryptKey, s); // we removed s.ToLower() from this call in v5.8!! 
            return tmpS;
        }

        static public String UnmungeString(String s)
        {
            String EncryptKey = CommonLogic.Application("EncryptKey");
            if (EncryptKey.Trim().ToUpperInvariant() == "REGISTRY")
            {
                try
                {
                    WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfig("EncryptKey.RegistryLocation"));
                    EncryptKey = reg.Read(AppLogic.AppConfig("EncryptKey.RegistryKey"));
                    reg = null;
                }
                catch (System.Security.SecurityException)
                {
                    throw new Exception("Cannot read registry values, probably the site is running in Medium Trust");
                }
            }
            if (EncryptKey.Trim().ToUpperInvariant() == "CONFIG")
            {
                EncryptKey = System.Web.Configuration.WebConfigurationManager.AppSettings[AppLogic.AppConfig("EncryptKey.RegistryKey")];
            }
            String tmpS = Encrypt.DecryptData(EncryptKey, s);
            return tmpS;
        }

  

     
        static public void ExecuteLoginRules(int CurrentCustomerID)
        {

            String CurIPAddress = CommonLogic.ServerVariables("REMOTE_ADDR");
            String SQLLogin = String.Format("UPDATE Customer SET LastIPAddress ='{0}', LastLogin=getdate()  WHERE CustomerID={1}", CurIPAddress, CurrentCustomerID.ToString());
            DB.ExecuteSQL(SQLLogin);

        }


        static public String GetCountryBar(String currentLocaleSetting)
        {
            return String.Empty; // this token was discountinued due to too many cross browser issues. use (!COUNTRYSELECTLIST!) instead.
        }

        static public String GetLocaleSelectList(String currentLocaleSetting)
        {
            if (AppLogic.NumLocaleSettingsInstalled() < 2)
            {
                return String.Empty;
            }
            String CacheName = "GetLocaleSelectList_" + currentLocaleSetting;
            if (AppLogic.CachingOn)
            {
                String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
                if (Menu != null)
                {
                    if (CommonLogic.ApplicationBool("DumpSQL"))
                    {
                        HttpContext.Current.Response.Write("Cache Hit Found!<br />\n");
                    }
                    return Menu;
                }
            }

            StringBuilder tmpS = new StringBuilder(4096);
            DataSet ds = DB.GetDS("select * from LocaleSetting  " + DB.GetNoLock() + " order by displayorder,description", true, System.DateTime.Now.AddHours(1));
            if (ds.Tables[0].Rows.Count > 0)
            {
                tmpS.Append("<!-- COUNTRY SELECT LIST -->\n");
                tmpS.Append("<select size=\"1\" onChange=\"self.location='setlocale.aspx?LocaleSetting=' + document.getElementById('CountrieselectList').value\" id=\"CountrieselectList\" name=\"CountrieselectList\" class=\"CountrieselectList\">");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    tmpS.Append("<option value=\"" + DB.RowField(row, "Name") + "\" " + CommonLogic.IIF(currentLocaleSetting == DB.RowField(row, "Name"), " selected ", "") + ">" + DB.RowField(row, "Description") + "</option>");
                }
                tmpS.Append("</select>");
                tmpS.Append("<!-- END COUNTRY SELECT LIST -->\n");
            }
            ds.Dispose();

            if (CachingOn)
            {
                HttpContext.Current.Cache.Insert(CacheName, tmpS.ToString(), null, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()), TimeSpan.Zero);
            }
            return tmpS.ToString();
        }




      
        static public String TransactionMode()
        {
            String tmpS = AppLogic.AppConfig("TransactionMode").Trim().ToUpperInvariant();
            if (tmpS.Length == 0)
            {
                tmpS = AppLogic.ro_TXModeAuthOnly; // forcefully set SOME default!
            }
            return tmpS;
        }

        static public bool TransactionModeIsAuthCapture()
        {
            return TransactionMode() != AppLogic.ro_TXModeAuthOnly;
        }

        static public bool TransactionModeIsAuthOnly()
        {
            return !TransactionModeIsAuthCapture();
        }

        static public Hashtable LoadAppConfigsFromDB()
        {
            IDataReader rs = DB.GetRS("Select * from AppConfig " + DB.GetNoLock());
            Hashtable ht = new Hashtable();
            while (rs.Read())
            {
                String key = DB.RSField(rs, "Name");
                // ignore dups, first one in wins:
                if (!ht.Contains(key.ToLowerInvariant()))
                {
                    // undocumented feature: allow web.config to override appconfig parm:
                    String theVal = CommonLogic.Application(key);
                    if (theVal.Length == 0)
                    {
                        theVal = DB.RSField(rs, "ConfigValue");
                    }
                    ht.Add(key.ToLowerInvariant(), theVal);
                }
            }
            rs.Close();
            return ht;
        }

      

        static public void LoadStringResourcesFromDB(bool TryToReload)
        {
            // load all strings into cache:
            StringResourceTable = new Hashtable();
            DataSet ds = DB.GetDS("select * from LocaleSetting " + DB.GetNoLock() + " order by displayorder,description", true, System.DateTime.Now.AddHours(1));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                String ThisLocale = DB.RowField(row, "Name");
           
                Hashtable ThisLocaleHT;
                if (!StringResourceTable.Contains(ThisLocale))
                {
                    ThisLocaleHT = new Hashtable();
                    StringResourceTable.Add(ThisLocale, ThisLocaleHT);
                }
                else
                {
                    ThisLocaleHT = (Hashtable)StringResourceTable[ThisLocale];
                }
                IDataReader rsstrings = DB.GetRS("Select * from StringResource " + DB.GetNoLock() + " where LocaleSetting=" + DB.SQuote(ThisLocale) + " order by Name");
                while (rsstrings.Read())
                {
                    // ignore dups, first one in wins:
                    if (!ThisLocaleHT.Contains(DB.RSField(rsstrings, "Name").ToLowerInvariant()))
                    {
                        ThisLocaleHT.Add(DB.RSField(rsstrings, "Name").ToLowerInvariant(), DB.RSField(rsstrings, "ConfigValue"));
                    }
                }
                rsstrings.Close();
            }
            ds.Dispose();
        }

        static public bool AnyCustomerHasUsedCoupon(String CouponCode)
        {
            return (DB.GetSqlN("select count(ordernumber) as N from orders  " + DB.GetNoLock() + " where lower(CouponCode)=" + DB.SQuote(CouponCode.ToLower())) != 0);
        }

        static public int GetNumberOfCouponUses(String CouponCode)
        {
            int tmp = 0;
            IDataReader rs = DB.GetRS("Select NumUses from coupon  " + DB.GetNoLock() + " where lower(CouponCode)=" + DB.SQuote(CouponCode.ToLower()));
            if (rs.Read())
            {
                tmp = DB.RSFieldInt(rs, "NumUses");
            }
            rs.Close();
            return tmp;
        }

        static public bool MicropayIsEnabled()
        {
            return (AppLogic.AppConfigBool("MicroPay.Enabled") || AppLogic.AppConfig("PaymentMethods").ToUpperInvariant().IndexOf(AppLogic.ro_PMMicropay) != -1);
        }

        static public void RecordCouponUsage(int CustomerID, String CouponCode)
        {
            if (CouponCode.Length != 0)
            {
                try
                {
                    DB.ExecuteSQL("update coupon set NumUses=NumUses+1 where lower(CouponCode)=" + DB.SQuote(CouponCode.ToLower()));
                    DB.ExecuteSQL("insert into CouponUsage(CustomerID,CouponCode) values(" + CustomerID.ToString() + "," + DB.SQuote(CouponCode) + ")");
                }
                catch { }
            }
        }


        static public String GetJSPopupRoutines()
        {
            StringBuilder tmpS = new StringBuilder(2500);
            tmpS.Append("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
            tmpS.Append("function popupwh(title,url,w,h)\n");
            tmpS.Append("	{\n");
            tmpS.Append("	window.open('popup.aspx?title=' + title + '&src=' + url,'Popup" + CommonLogic.GetRandomNumber(1, 100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,width=' + w + ',height=' + h + ',left=0,top=0');\n");
            tmpS.Append("	return (true);\n");
            tmpS.Append("	}\n");
            tmpS.Append("function popuptopicwh(title,topic,w,h,scrollbars)\n");
            tmpS.Append("	{\n");
            tmpS.Append("	window.open('popup.aspx?title=' + title + '&topic=' + topic,'Popup" + CommonLogic.GetRandomNumber(1, 100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + scrollbars + ',resizable=no,copyhistory=no,width=' + w + ',height=' + h + ',left=0,top=0');\n");
            tmpS.Append("	return (true);\n");
            tmpS.Append("	}\n");
            tmpS.Append("function popuporderoptionwh(title,id,w,h,scrollbars)\n");
            tmpS.Append("	{\n");
            tmpS.Append("	window.open('popup.aspx?title=' + title + '&orderoptionid=' + id,'Popup" + CommonLogic.GetRandomNumber(1, 100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + scrollbars + ',resizable=no,copyhistory=no,width=' + w + ',height=' + h + ',left=0,top=0');\n");
            tmpS.Append("	return (true);\n");
            tmpS.Append("	}\n");
            tmpS.Append("function popupkitgroupwh(title,kitgroupid,w,h,scrollbars)\n");
            tmpS.Append("	{\n");
            tmpS.Append("	window.open('popup.aspx?title=' + title + '&kitgroupid=' + kitgroupid,'Popup" + CommonLogic.GetRandomNumber(1, 100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + scrollbars + ',resizable=no,copyhistory=no,width=' + w + ',height=' + h + ',left=0,top=0');\n");
            tmpS.Append("	return (true);\n");
            tmpS.Append("	}\n");
            tmpS.Append("function popupkititemwh(title,kititemid,w,h,scrollbars)\n");
            tmpS.Append("	{\n");
            tmpS.Append("	window.open('popup.aspx?title=' + title + '&kititemid=' + kititemid,'Popup" + CommonLogic.GetRandomNumber(1, 100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + scrollbars + ',resizable=no,copyhistory=no,width=' + w + ',height=' + h + ',left=0,top=0');\n");
            tmpS.Append("	return (true);\n");
            tmpS.Append("	}\n");
            tmpS.Append("function popup(title,url)\n");
            tmpS.Append("	{\n");
            tmpS.Append("	popupwh(title,url,600,375);\n");
            tmpS.Append("	return (true);\n");
            tmpS.Append("	}\n");
            tmpS.Append("function popuptopic(title,topic,scrollbars)\n");
            tmpS.Append("	{\n");
            tmpS.Append("	popuptopicwh(title,topic,600,375,scrollbars);\n");
            tmpS.Append("	return (true);\n");
            tmpS.Append("	}\n");
            tmpS.Append("</script>\n");
            return tmpS.ToString();
        }

             
        static public void CheckDemoExpiration()
        {

            System.Net.IPHostEntry T = System.Net.Dns.GetHostEntry("www.aspdotnetstorefront.com");
            for (int A = 0; A <= T.AddressList.Length - 1; A++)
            {
                if (T.AddressList[A].ToString().StartsWith("192.168") || T.AddressList[A].ToString().StartsWith("127.0"))
                {
                    throw new Exception("Cracked version not allowed.");
                }
            }
            String DemoKey = CommonLogic.Application("DemoKey");
            if (DemoKey.Length == 0)
            {
                throw (new ArgumentException(GetString("common.cs.30", 1, Thread.CurrentThread.CurrentUICulture.Name)));
            }
            String stat = CommonLogic.AspHTTP("http://www.aspdotnetstorefront.com/getdemoexpiry.aspx?demolocation=" + DemoKey + "&fromappconfig=true");
            if (stat.Length == 0)
            {
                throw (new ArgumentException(GetString("common.cs.31", 1, Thread.CurrentThread.CurrentUICulture.Name)));
            }
            if (stat.IndexOf("Demo License OK") == -1)
            {
                throw (new ArgumentException(GetString("common.cs.32", 1, Thread.CurrentThread.CurrentUICulture.Name)));
            }
        }

       
        static public String GetNewsBoxExpanded(bool LinkHeadline, bool ShowCopy, int showNum, bool IncludeFrame, bool useCache, String teaser, int SkinID, String LocaleSetting)
        {
            String CacheName = "GetNewsBoxExpanded_" + showNum.ToString() + "_" + teaser + "_" + SkinID.ToString() + "_" + LocaleSetting;
            if (AppLogic.CachingOn && useCache)
            {
                String cachedData = (String)HttpContext.Current.Cache.Get(CacheName);
                if (cachedData != null)
                {
                    if (CommonLogic.ApplicationBool("DumpSQL"))
                    {
                        HttpContext.Current.Response.Write("Cache Hit Found!<br />\n");
                    }
                    return cachedData;
                }
            }

            StringBuilder tmpS = new StringBuilder(10000);
            DataSet ds = DB.GetDS("select * from News " + DB.GetNoLock() + " where ExpiresOn>getdate() and Deleted=0 and Published=1 order by CreatedON desc", AppLogic.CachingOn, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()));
            if (ds.Tables[0].Rows.Count > 0)
            {

                if (IncludeFrame)
                {
                    tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + AppLogic.AppConfig("HeaderBGColor") + "\">\n");
                    tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
                    tmpS.Append("<a href=\"news.aspx\"><img src=\"" + AppLogic.LocateImageURL("skins/Skin_" + SkinID.ToString() + "/images/newsexpanded.gif") + "\" border=\"0\" /></a><br />");
                    tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + AppLogic.AppConfig("BoxFrameStyle") + "\">\n");
                    tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
                }

                tmpS.Append("<p><b>" + teaser + "</b></p>\n");


                tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\">\n");
                int i = 1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (i > showNum)
                    {
                        tmpS.Append("<tr><td colspan=\"2\"><hr size=\"1\" color=\"#" + AppLogic.AppConfig("MediumCellColor") + "\"/><a href=\"news.aspx\">more...</a></td></tr>");
                        break;
                    }
                    if (i > 1)
                    {
                        tmpS.Append("<tr><td colspan=\"2\"><hr size=\"1\" color=\"#" + AppLogic.AppConfig("MediumCellColor") + "\"/></td></tr>");
                    }
                    tmpS.Append("<tr>");
                    tmpS.Append("<td width=\"15%\" align=\"left\" valign=\"top\">\n");
                    tmpS.Append("<b>" + Localization.ToNativeShortDateString(DB.RowFieldDateTime(row, "CreatedOn")) + "</b>");
                    tmpS.Append("</td>");
                    tmpS.Append("<td align=\"left\" valign=\"top\">\n");
                    String Hdl = DB.RowFieldByLocale(row, "Headline", LocaleSetting);
                    if (Hdl.Length == 0)
                    {
                        Hdl = CommonLogic.Ellipses(DB.RowFieldByLocale(row, "NewsCopy", LocaleSetting), 50, true);
                    }
                    tmpS.Append("<div align=\"left\">");
                    if (LinkHeadline)
                    {
                        tmpS.Append("<a href=\"news.aspx?showarticle=" + DB.RowFieldInt(row, "NewsID").ToString() + "\">");
                    }
                    tmpS.Append("<b>");
                    tmpS.Append(Hdl);
                    tmpS.Append("</b>");
                    if (LinkHeadline)
                    {
                        tmpS.Append("</a>");
                    }
                    tmpS.Append("</div>");
                    if (ShowCopy)
                    {
                        tmpS.Append("<div align=\"left\"><br/>" + HttpContext.Current.Server.HtmlDecode(DB.RowFieldByLocale(row, "NewsCopy", LocaleSetting)) + "</div>");
                    }
                    tmpS.Append("</td>");
                    tmpS.Append("</tr>");
                    i++;
                }
                tmpS.Append("</table>\n");
                ds.Dispose();

                if (IncludeFrame)
                {
                    tmpS.Append("</td></tr>\n");
                    tmpS.Append("</table>\n");
                    tmpS.Append("</td></tr>\n");
                    tmpS.Append("</table>\n");
                }
            }

            if (CachingOn && useCache)
            {
                HttpContext.Current.Cache.Insert(CacheName, tmpS.ToString(), null, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()), TimeSpan.Zero);
            }
            return tmpS.ToString();
        }



        public static String MakeProperPhoneFormat(String PhoneNumber)
        {
            return PhoneNumber;
            //			if(PhoneNumber.Length == 0)
            //			{
            //				return String.Empty;
            //			}
            //			if(PhoneNumber.Substring(0,1) != "1")
            //			{
            //				PhoneNumber = "1" + PhoneNumber;
            //			}
            //			String newS = String.Empty;
            //			String validDigits = "0123456789";
            //			for(int i = 1; i<= PhoneNumber.Length; i++)
            //			{
            //				if(validDigits.IndexOf(PhoneNumber[i-1]) != -1)
            //				{
            //					newS = newS + PhoneNumber[i-1];
            //				}
            //			}
            //			return newS;
        }

        public static String GetAdminDir()
        {
            String AdminDir = AppLogic.AppConfig("AdminDir");
            if (AdminDir.Length == 0)
            {
                AdminDir = "admin";
            }
            if (AdminDir.EndsWith("/"))
            {
                AdminDir = AdminDir.Substring(0, AdminDir.Length - 1);
            }
            return AdminDir;
        }

       

        public static bool OnLiveServer()
        {
            return (CommonLogic.ServerVariables("HTTP_HOST").ToUpperInvariant().IndexOf(AppLogic.AppConfig("LiveServer").ToUpperInvariant()) != -1);
        }

        public static void SessionStart()
        {
            // after session has started, the Customer classes USES session["CustomerID"] to track the customer (if any)
            // theoretically, if the Customer class COULD rebuild the customer object from the CustomerGUID cookie on every page, but that
            // would be innefficient, so this session build does it ONCE on session start to map CustomerGUID to CustomerID, or to make
            // a new customer record if there are none, and one is required (and delayed customer creation is not set)
            // admin site works just a little different...no anon records should be created ever...

            // Also, if in a serverfarm, Application_PreBeginRequest forces session rebuild EVERY page, because we have no idea if they
            // changed identities "between" server visits, where they could already have an "old" session (out of date now), so 
            // must force session update on every page

            // if not in serverfarm, session_start calls this

            // since this session build from cookie works on any web server, it is farmable in terms of customer management
            // (i.e. the customer session will be rebuilt on each web server in a farm, if their session is shared between servers.

            // SPECIAL CASES: DO NOT create a session for the gateway callbacks!
            if (CommonLogic.ServerVariables("SCRIPT_NAME").ToUpperInvariant().IndexOf("WORLDPAYRETURN.ASPX") != -1)
            {
                return;
            }

         
            int CustomerID = 0;
            // get the cookie, if any:
            String CustomerGUID = HttpContext.Current.User.Identity.Name;

            bool rebuilt = false;
            // if there was a cookie, try to get the CustomerID corresponding to it:
            if (CustomerGUID.Length > 0)
            {
                try
                {
                    // found cookie, restore session from cookie:
                    String sql = "select CustomerID,CustomerLevelID,CustomerGUID from customer  " + DB.GetNoLock() + " where deleted=0 and CustomerGUID=" + DB.SQuote(CustomerGUID);
                    IDataReader rs = DB.GetRS(sql);
                    if (rs.Read())
                    {
                        rebuilt = true;
                        HttpContext.Current.Session["CustomerID"] = DB.RSFieldInt(rs, "CustomerID").ToString();
                        HttpContext.Current.Session["CustomerLevelID"] = DB.RSFieldInt(rs, "CustomerLevelID").ToString();
                        HttpContext.Current.Session["CustomerGUID"] = DB.RSFieldGUID(rs, "CustomerGUID");
                    }
                    rs.Close();
                }
                catch { }
            }

          
            if (rebuilt && CustomerID != 0)
            {
                // age their customersession data (in the db):
                CustomerSession.StaticClear(CustomerID);
            }

            HttpContext.Current.Session.Timeout = AppLogic.SessionTimeout();

        }

     
        static public String GetTechTalkBox(int SkinID)
        {
            String CacheName = "GetTechTalkBox_" + SkinID.ToString();
            if (AppLogic.CachingOn)
            {
                String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
                if (Menu != null)
                {
                    if (CommonLogic.ApplicationBool("DumpSQL"))
                    {
                        HttpContext.Current.Response.Write("Cache Hit Found!<br />\n");
                    }
                    return Menu;
                }
            }

            StringBuilder tmpS = new StringBuilder(10000);
            tmpS.Append("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + AppLogic.AppConfig("HeaderBGColor") + "\">\n");
            tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
            tmpS.Append("<a href=\"techtalk.aspx\"><img src=\"" + AppLogic.LocateImageURL("skins/Skin_" + SkinID.ToString() + "/images/learn.gif") + "\" border=\"0\" /></a><br />");
            tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + AppLogic.AppConfig("BoxFrameStyle") + "\">\n");
            tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");

            tmpS.Append("NOT IMPLEMENTED YET");

            tmpS.Append("</td></tr>\n");
            tmpS.Append("</table>\n");
            tmpS.Append("</td></tr>\n");
            tmpS.Append("</table>\n");
            if (CachingOn)
            {
                HttpContext.Current.Cache.Insert(CacheName, tmpS.ToString(), null, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()), TimeSpan.Zero);
            }
            return tmpS.ToString();
        }

        static public String GetTechTalkBoxExpanded(int SkinID)
        {
            String CacheName = "GetTechTalkBoxExpanded_" + SkinID.ToString();
            if (AppLogic.CachingOn)
            {
                String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
                if (Menu != null)
                {
                    if (CommonLogic.ApplicationBool("DumpSQL"))
                    {
                        HttpContext.Current.Response.Write("Cache Hit Found!<br />\n");
                    }
                    return Menu;
                }
            }


            StringBuilder tmpS = new StringBuilder(10000);
            tmpS.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + AppLogic.AppConfig("HeaderBGColor") + "\">\n");
            tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
            tmpS.Append("<a href=\"techtalk.aspx\"><img src=\"" + AppLogic.LocateImageURL("skins/Skin_" + SkinID.ToString() + "/images/learnexpanded.gif") + "\" border=\"0\" /></a><br />");
            tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + AppLogic.AppConfig("BoxFrameStyle") + "\">\n");
            tmpS.Append("<tr><td width=\"100%\" align=\"left\" valign=\"top\">\n");

            tmpS.Append("NOT IMPLEMENTED YET");

            tmpS.Append("</td></tr>\n");
            tmpS.Append("</table>\n");
            tmpS.Append("</td></tr>\n");
            tmpS.Append("</table>\n");
            if (CachingOn)
            {
                HttpContext.Current.Cache.Insert(CacheName, tmpS.ToString(), null, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()), TimeSpan.Zero);
            }
            return tmpS.ToString();
        }


        static public String GetSearchBox(int SkinID, String LocaleSetting)
        {
            String CacheName = "GetSearchBox_" + SkinID.ToString() + "_" + LocaleSetting;
            if (AppLogic.CachingOn)
            {
                String Menu = (String)HttpContext.Current.Cache.Get(CacheName);
                if (Menu != null)
                {
                    if (CommonLogic.ApplicationBool("DumpSQL"))
                    {
                        HttpContext.Current.Response.Write("Cache Hit Found!<br />\n");
                    }
                    return Menu;
                }
            }

            StringBuilder tmpS = new StringBuilder(10000);
            tmpS.Append("<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border-style: solid; border-width: 0px; border-color: #" + AppLogic.AppConfig("HeaderBGColor") + "\">\n");
            tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");
            tmpS.Append("<img src=\"" + AppLogic.LocateImageURL("skins/Skin_" + SkinID.ToString() + "/images/search.gif", LocaleSetting) + "\" border=\"0\" /><br />");
            tmpS.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"" + AppLogic.AppConfig("BoxFrameStyle") + "\">\n");
            tmpS.Append("<tr><td align=\"left\" valign=\"top\">\n");

            tmpS.Append("<script type=\"text/javascript\" Language=\"JavaScript\">\n");
            tmpS.Append("function SearchBoxForm_Validator(theForm)\n");
            tmpS.Append("{\n");
            tmpS.Append("  submitonce(theForm);\n");
            tmpS.Append("  if (theForm.SearchTerm.value.length < " + AppLogic.AppConfig("MinSearchStringLength") + ")\n");
            tmpS.Append("  {\n");
            tmpS.Append("    alert('" + String.Format(GetString("common.cs.66", SkinID, LocaleSetting), AppLogic.AppConfig("MinSearchStringLength")) + "');\n");
            tmpS.Append("    theForm.SearchTerm.focus();\n");
            tmpS.Append("    submitenabled(theForm);\n");
            tmpS.Append("    return (false);\n");
            tmpS.Append("  }\n");
            tmpS.Append("  return (true);\n");
            tmpS.Append("}\n");
            tmpS.Append("</script>\n");

            tmpS.Append("<form style=\"margin-top: 0px; margin-bottom: 0px;\" name=\"SearchBoxForm\" action=\"searchadv.aspx\" method=\"GET\" onsubmit=\"return SearchBoxForm_Validator(this)\">\n");
            tmpS.Append("<input type=\"hidden\" name=\"IsSubmit\" value=\"true\" />" + GetString("common.cs.82", SkinID, LocaleSetting) + " <input name=\"SearchTerm\" size=\"10\" /><img src=\"images/spacer.gif\" width=\"4\" height=\"4\" /><INPUT NAME=\"submit\" TYPE=\"Image\" ALIGN=\"absmiddle\" src=\"" + AppLogic.LocateImageURL("skins/Skin_" + SkinID.ToString() + "/images/go.gif") + "\" border=\"0\" />\n");
            tmpS.Append("</form>");
            tmpS.Append("</td></tr>\n");
            tmpS.Append("</table>\n");
            tmpS.Append("</td></tr>\n");
            tmpS.Append("</table>\n");
            if (CachingOn)
            {
                HttpContext.Current.Cache.Insert(CacheName, tmpS.ToString(), null, System.DateTime.Now.AddMinutes(AppLogic.CacheDurationMinutes()), TimeSpan.Zero);
            }
            return tmpS.ToString();
        }



        public static String GetAppConfigName(int AppConfigID)
        {
            String tmp = String.Empty;
            IDataReader rs = DB.GetRS("Select name Name AppConfig " + DB.GetNoLock() + " where AppConfigID=" + AppConfigID.ToString());
            if (rs.Read())
            {
                tmp = DB.RSField(rs, "Name");
            }
            rs.Close();
            return tmp;
        }


        public static void ClearCache()
        {
            ImageFilenameCache.Clear();
            AppLogic.AppConfigTable = AppLogic.LoadAppConfigsFromDB();
            IDictionaryEnumerator en = HttpContext.Current.Cache.GetEnumerator();
            while (en.MoveNext())
            {
                HttpContext.Current.Cache.Remove(en.Key.ToString());
            }
        }

        public static String GetImagePath(String EntityOrObjectName, String Size, bool fullPath)
        {
            String pth = String.Empty;
            String pthPrefix = String.Empty;
            if (AppLogic.IsAdminSite)
            {
                pthPrefix = "../";
            }
            pth = pthPrefix + "images/" + EntityOrObjectName;
            if (Size.Length != 0)
            {

                // FOllowing one line is added to return a medium size image instead of an icon in the Product Lsit pages
                if (Size=="icon") Size="medium";
                pth += "/" + Size.ToLower();
            }
            pth += "/";
            //Now have a _full_ url pth which will take into account any virtual directory mappings
            if (fullPath)
            {
                pth = CommonLogic.SafeMapPath(pth); //AppLogic.AppConfig("StoreFilesPath");
            }
            return pth;
        }

        public static String GetImagePathAK(int CountryID, int CategoryID,  String Size, bool fullPath)
        {
            String pth = String.Empty;
            String pthPrefix = String.Empty;
            if (AppLogic.IsAdminSite)
            {
                pthPrefix = "../";
            }
            pth = pthPrefix + "orders/" + CountryID.ToString() + "/" + CategoryID.ToString();
            if (Size.Length != 0)
            {

                // FOllowing one line is added to return a medium size image instead of an icon in the Product Lsit pages
                //if (Size == "icon") Size = "medium";
                pth += "/" + Size.ToLower();
            }
            pth += "/";
            //Now have a _full_ url pth which will take into account any virtual directory mappings
            if (fullPath)
            {
                pth = CommonLogic.SafeMapPath(pth); //AppLogic.AppConfig("StoreFilesPath");
            }
            return pth;
        }


        public static void SetCookie(String cookieName, String cookieVal, TimeSpan ts)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(cookieName)
                {
                    Value = HttpContext.Current.Server.UrlEncode(cookieVal),
                    HttpOnly = false,
                    Secure = true
                };
                DateTime dt = DateTime.Now;
                cookie.Expires = dt.Add(ts);
                if (AppLogic.OnLiveServer())
                {
                    cookie.Domain = AppLogic.AppConfig("LiveServer");
                }
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch
            { }
        }

        public static void SetSessionCookie(String cookieName, String cookieVal)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(cookieName)
                {
                    Value = HttpContext.Current.Server.UrlEncode(cookieVal),
                    Secure = true,
                    HttpOnly = false
                };
                if (AppLogic.OnLiveServer())
                {
                    cookie.Domain = AppLogic.AppConfig("LiveServer");
                }
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch
            { }
        }


        static public void SendSimpleMail(String subject, String body, bool useHTML, String fromname, String toaddress, String toname, String bccaddresses, String server)
        {
         
            System.Web.Mail.MailMessage emailMesg ;

            emailMesg = new System.Web.Mail.MailMessage
            {
                From = fromname,
                To = toaddress,
                Subject = subject,
                Body = body,
                Priority = System.Web.Mail.MailPriority.High
            };
            SmtpMail.Send(emailMesg);


        }


        static public void SendMail(String subject, String body, bool useHTML)
        {
            SendMail(subject, body, useHTML, AppLogic.AppConfig("MailMe_FromAddress"), AppLogic.AppConfig("MailMe_FromName"), AppLogic.AppConfig("MailMe_ToAddress"), AppLogic.AppConfig("MailMe_ToName"), String.Empty, AppLogic.AppConfig("MailMe_Server"));
        }

        static public void SendMail(String subject, String body, bool useHTML, String fromaddress, String fromname, String toaddress, String toname, String bccaddresses, String server)
        {
            SendMail(subject, body, useHTML, fromaddress, fromname, toaddress, toname, bccaddresses, String.Empty, server);
        }

        // mask errors on store site, better to have a lost receipt than crash the site
        // on admin site, throw exceptions
        static public void SendMail(String subject, String body, bool useHTML, String fromaddress, String fromname, String toaddress, String toname, String bccaddresses, String ReplyTo, String server)
        {
#if SMTPDOTNET
			// SMTP.NET COMPONENT:
			SmtpServer smtp = new SmtpServer();  
			if(server.Length != 0)
			{
				smtp.ServerAddress = server;
			}
			if(AppLogic.AppConfig("MailMe_Pwd").Length != 0 && AppLogic.AppConfig("MailMe_User").Length != 0)
			{
				smtp.AuthLogin = AppLogic.AppConfig("MailMe_User");
				smtp.AuthPassword = AppLogic.AppConfig("MailMe_Pwd");
			}

			smtp.FromAddress = fromaddress; 
			if(fromname.Length != 0)
			{
				smtp.FromFriendly = fromname;
			}
			smtp.ToAddress = toaddress; 
			if(toname.Length != 0)
			{
				smtp.ToFriendly = toname;
			}
			if(bccaddresses.Length != 0)
			{
				if(bccaddresses.IndexOf(";") != -1)
				{
					String[] bcclist = bccaddresses.Split(';');
					foreach(String bccS in bcclist)
					{
						smtp.AddRecipient(bccS,bccS,SmtpDotNet.AddressTypes.BCC);
					}
				}
				else if(bccaddresses.IndexOf(",") != -1)
				{
					String[] bcclist = bccaddresses.Split(',');
					foreach(String bccS in bcclist)
					{
						smtp.AddRecipient(bccS,bccS,SmtpDotNet.AddressTypes.BCC);
					}
				}
				else
				{
					smtp.BCCAddress = bccaddresses;
				}
			}
			if(ReplyTo.Length != 0)
			{
				smtp.ReplyToAddress = ReplyTo;
				smtp.ReplyToFriendly = ReplyTo;
			}
			smtp.BodyFormat = (BodyFormatTypes)CommonLogic.IIF(useHTML , (int)BodyFormatTypes.HTML , (int)BodyFormatTypes.PLAIN);
			smtp.Subject = subject;
			smtp.Body = body;
			ReturnCodes nRC = smtp.Send(); 
			if (nRC != ReturnCodes.SUCCESS || smtp.EmailCountBad != 0 )
			{

				throw new ArgumentException("Mail Error #" + nRC + " occurred - " + smtp.LastError + " - rejected " + smtp.EmailCountBad + " Messages");
			}

#else
#if QQMAIL
            //if (AppLogic.AppConfigBool("SendEMailViaQQMail"))
            if (AppLogic.AppConfigBool("SendEMailViaQQMail")==false)
            {
                qqMail.qqSmtp objMail = new qqMail.qqSmtp
                {
                    Subject = subject
                };
                if (useHTML)
                {
                    objMail.HtmlBody = body;
                    objMail.MailFormat = qqMail.qqSmtp.EnumMailFormat.html;
                }
                else
                {
                    objMail.TextBody = body;
                    objMail.MailFormat = qqMail.qqSmtp.EnumMailFormat.text;
                }
                objMail.From = fromaddress;
                objMail.FromName = fromname;
                objMail.To = toaddress;
                objMail.Bcc = bccaddresses;
                if (ReplyTo.Length != 0)
                {
                    objMail.ReplyTo = ReplyTo;
                }
                objMail.UseDirectSend = AppLogic.AppConfigBool("QQMailDirectSend");
                if (server.ToUpperInvariant() != "TBD" && server.ToUpperInvariant() != "MAIL.YOURDOMAIN.COM" && server.Length != 0)
                {
                    objMail.SmtpServer = server;
                }
                if (AppLogic.AppConfig("MailMe_User").Length != 0)
                {
                    objMail.SmtpUser = AppLogic.AppConfig("MailMe_User");
                }
                if (AppLogic.AppConfig("MailMe_Pwd").Length != 0)
                {
                    objMail.SmtpPass = AppLogic.AppConfig("MailMe_Pwd");
                }
                objMail.TimeOut = 60;
                try
                {
                    objMail.SendMail();
                    if (!objMail.isOk)
                    {
                        if (AppLogic.IsAdminSite)
                        {
                            throw new ArgumentException(objMail.ErrDes);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (AppLogic.IsAdminSite)
                    {
                        throw new ArgumentException("Mail Error occurred - " + CommonLogic.GetExceptionDetail(ex, "<br />"));
                    }
                }
            }
            else
            {
#endif
            if (server.ToUpperInvariant() != "TBD" && server.ToUpperInvariant() != "MAIL.YOURDOMAIN.COM" && server.Length != 0)
            {
                // USE BUILT IN .NET MAIL (WHICH SUCKS)
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(new MailAddress(fromaddress, fromname), new MailAddress(toaddress, toname));
                if (ReplyTo.Length != 0)
                {
                    msg.ReplyTo = new MailAddress(ReplyTo);
                }
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = useHTML;
                if (bccaddresses.Length != 0)
                {
                    MailAddressCollection mc = new MailAddressCollection();
                    foreach (String s in bccaddresses.Split(new char[] { ',', ';' }))
                    {
                        msg.Bcc.Add(new MailAddress(s));
                    }
                }
                SmtpClient client = new SmtpClient(server);
                if (AppLogic.AppConfig("MailMe_User").Length != 0)
                {
                    System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(AppLogic.AppConfig("MailMe_User"), AppLogic.AppConfig("MailMe_Pwd"));
                    client.UseDefaultCredentials = false;
                    client.Credentials = SMTPUserInfo;
                }
                else
                {
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                try
                {
                    client.Send(msg);
                }
                catch (Exception ex)
                {
                    if (AppLogic.IsAdminSite)
                    {
                        throw new ArgumentException("Mail Error occurred - " + CommonLogic.GetExceptionDetail(ex, "<br />"));
                    }
                }
                msg.Dispose();
            }
            else
            {
                if (AppLogic.IsAdminSite)
                {
                    throw new ArgumentException("Invalid Mail Server: " + server);
                }
            }
#if QQMAIL
            }
#endif

#endif
        }

        static public bool HasBadWords(String s)
        {
            s = s.ToUpper();
            IDataReader rs = DB.GetRS("select upper(Word) as BadWord from BadWord " + DB.GetNoLock() + " where LocaleSetting=" + DB.SQuote(Thread.CurrentThread.CurrentUICulture.Name));
            while (rs.Read())
            {
                if (s.IndexOf(DB.RSField(rs, "BadWord")) != -1)
                {
                    rs.Close();
                    return true;
                }
            }
            rs.Close();
            return false;
        }


        // gets roles of current httpcontext user, prior set by SetRoles
        public static String GetRoles()
        {
            String tmpS = String.Empty;
            try
            {
                TracklinePrincipal p = (TracklinePrincipal)Thread.CurrentPrincipal;
                tmpS = p.m_Roles;
            }
            catch { }
            return tmpS;
        }


        public static int GetMicroPayProductID()
        {
            int result = 0;

            IDataReader rs = DB.GetRS("select ProductID from Product  " + DB.GetNoLock() + " where deleted=0 and SKU='MICROPAY'");
            if (rs.Read())
            {
                result = DB.RSFieldInt(rs, "ProductID");
            }
            rs.Close();

            return result;
        }

        public static int GetAdHocProductID()
        {
            int result = 0;

            IDataReader rs = DB.GetRS("select ProductID from Product  " + DB.GetNoLock() + " where deleted=0 and SKU='ADHOCCHARGE'");
            if (rs.Read())
            {
                result = DB.RSFieldInt(rs, "ProductID");
            }
            rs.Close();

            return result;
        }

        public static int GetLocaleSettingID(String LocaleSetting)
        {
            int tmp = 0;
            DataSet ds = DB.GetDS("select * from LocaleSetting  " + DB.GetNoLock() + " order by displayorder,description", true, System.DateTime.Now.AddHours(1));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (LocaleSetting == DB.RowField(row, "Name"))
                {
                    tmp = DB.RowFieldInt(row, "LocaleSettingID");
                }
            }
            ds.Dispose();
            return tmp;
        }

        public static decimal GetMicroPayBalance(int CustomerID)
        {
            decimal result = System.Decimal.Zero;

            if (CustomerID != 0)
            {
                IDataReader rs = DB.GetRS(String.Format("select MicroPayBalance from Customer  " + DB.GetNoLock() + " where CustomerID={0}", CustomerID));
                if (rs.Read())
                {
                    result = DB.RSFieldDecimal(rs, "MicroPayBalance");
                }
                rs.Close();
            }
            return result;
        }

        /// <summary>
        /// Sets the Roles of the logged in user and adds them to their security Principal
        /// This must be called in Application_AuthenticateRequest of Global.asax
        /// </summary>
        public static void SetRoles()
        {
            if (HttpContext.Current.Request.IsAuthenticated) //We know who they are
            {
                ArrayList roleList = new ArrayList(50); //List of Role Strings
                string UserGUID = HttpContext.Current.User.Identity.Name;
                int UserID = 0;
                string  UserTypeIDs = "";
                DateTime SubscriptionExpiresOn = DateTime.Now;

                IDataReader rs = null;
                try
                {
                    if (UserGUID.Length != 0)
                    {

                        rs = DB.GetRS("select * from User  " + DB.GetNoLock() + " where IsActive=1 and UserGUID=" + DB.SQuote(UserGUID));
                        if (rs.Read())
                        {
                            UserID = DB.RSFieldInt(rs, "UserID");
                            // get the CustomerLevelID for later
                            UserTypeIDs = DB.RSField(rs, "UserTpyeID");
                            //SubscriptionExpiresOn = DB.RSFieldDateTime(rs, "SubscriptionExpiresOn");
                        }
                        else
                        {
                            UserGUID = String.Empty; // some kind of error, return blank info
                        }
                        rs.Close();
                    }
                }
                catch { }

                // Add whatever role strings required.
                // UserLevel string is a good possibility. Allow access passed on userlevel.
                // Use the SKUs of products the user has purchased as their Role strings. 
                // This way the SKU can be added to the Web.Config <authorization> section to allow acces in a protected directory.

                //Everybody that is not anonymous Gets "Free"
                roleList.Add("Free");

                if (UserGUID.Length != 0)
                {
                    /*
                    // Admins and super users rule!
                    if (User.StaticIsAdminUser(CustomerID))
                    {
                        roleList.Add("Admin");
                    }
                    if (User.StaticIsAdminSuperUser(CustomerID))
                    {
                        roleList.Add("SuperAdmin");
                    }
                    //Check Subscriber Expiration
                    if (SubscriptionExpiresOn.CompareTo(DateTime.Now) > 0)
                    {
                        roleList.Add("Subscriber");
                    }
                    */
                    try
                    {
                        if (UserTypeIDs  != "")
                        {
                            rs = DB.GetRS(String.Format("select UserType from UserType " + DB.GetNoLock() + " where UserTypeID IN {0}", UserTypeIDs));
                            while (rs.Read())
                            {
                                roleList.Add(DB.RSField(rs, "UserType"));
                            }
                            rs.Close();
                        }
                    }
                    catch { }
                    string[] roles = (string[])roleList.ToArray(typeof(string));
                    //HttpContext.Current.User = new GenericPrincipal(HttpContext.Current.User.Identity,roles);
                    HttpContext.Current.User = new TracklinePrincipal(HttpContext.Current.User.Identity, roles);
                }
            }
        }

     

        // ----------------------------------------------------------------
        //
        // STRINGRESOURCE SUPPORT ROUTINES
        //
        // ----------------------------------------------------------------
        public static void SetStringResource(String Name, String LocaleSetting, String ConfigValue, bool ClearCache)
        {
            if (readonly_WCLocale.Length == 0)
            {
                readonly_WCLocale = Localization.GetWebConfigLocale();
            }
            if (StringResourceTable == null)
            {
                LoadStringResourcesFromDB(true);
            }
            Hashtable ht = (Hashtable)StringResourceTable[readonly_WCLocale];
            if (ht == null)
            {
                LoadStringResourcesFromDB(true);
                ht = (Hashtable)StringResourceTable[readonly_WCLocale];
            }
            if (ht.Count == 0)
            {
                LoadStringResourcesFromDB(true);
            }

            if (DB.GetSqlN("select count(Name) as N from StringResource " + DB.GetNoLock() + " where lower(Name)=" + DB.SQuote(Name.ToLowerInvariant())) == 0)
            {
                DB.ExecuteSQL(String.Format("insert into StringResource(Name,LocaleSetting,ConfigValue) values({0},{1},{2})", DB.SQuote(Name.ToLowerInvariant()), DB.SQuote(LocaleSetting), DB.SQuote(ConfigValue)));
                ((Hashtable)StringResourceTable[LocaleSetting]).Add(Name.ToLowerInvariant(), ConfigValue);
            }
            else
            {
                DB.ExecuteSQL(String.Format("update StringResource set ConfigValue={0} where lower(name)={1} and LocaleSetting={2}", DB.SQuote(ConfigValue), DB.SQuote(Name.ToLowerInvariant()), DB.SQuote(LocaleSetting)));
                ((Hashtable)StringResourceTable[LocaleSetting])[Name.ToLowerInvariant()] = ConfigValue;
            }
            if (ClearCache)
            {
                AppLogic.ClearCache();
            }
        }

        static String readonly_WCLocale = String.Empty; // for speed
        static public string GetString(string key, int SkinID, String LocaleSetting)
        {
            // undocumented diagnostic mode:
            if (AppLogic.AppConfigBool("ShowStringResourceKeys"))
            {
                return key;
            }
            if (readonly_WCLocale.Length == 0)
            {
                readonly_WCLocale = Localization.GetWebConfigLocale();
            }
            if (StringResourceTable == null)
            {
                LoadStringResourcesFromDB(true);
            }
            Hashtable ht = (Hashtable)StringResourceTable[readonly_WCLocale];
            if (ht == null)
            {
                LoadStringResourcesFromDB(true);
                ht = (Hashtable)StringResourceTable[readonly_WCLocale];
            }
            if (ht.Count == 0)
            {
                LoadStringResourcesFromDB(true);
            }

            // NOTE: The SkinID parameter is Not Used Currently. One String Resource file per "site", not skin :)
            String result = key;
            ht = (Hashtable)StringResourceTable[LocaleSetting];
            // do language fallback to master store master language Hashtable, if the LocaleSetting Hashtable wasn't found and
            // the locale requested was not already the master locale
            if (ht == null && LocaleSetting != readonly_WCLocale)
            {
                ht = (Hashtable)StringResourceTable[readonly_WCLocale];
            }
            if (ht != null)
            {
                if (ht.Count == 0)
                {
                    // string resources are not loaded? try to reload to be safe:
                    AppLogic.LoadStringResourcesFromDB(false);
                    // send e-mail also, as this is NOT something that you should ever have to do, and if it happens often, the site
                    // performance will be VERY slow
                    if (AppLogic.AppConfig("MailMe_Server").Length != 0 && AppLogic.AppConfig("MailMe_Server").ToUpperInvariant() != "TBD")
                    {
                        try
                        {
                            String Explanation = "This message means that your site is flushing application memory for some (unknown) reason. If you get this e-mail more then very rarely, it could be a big performance impact, and you should check with your hosting provider about this issue. Their server may be running low of RAM or something is causing their application asp.net memory caches to get flushed. This could cause your site to send up to 1000 database queries to the store on every single page load. If our store sends you an e-mail (which is VERY rare), it's not for a minor issue. First, please check with your hosting provider.";
                            AppLogic.SendMail(AppLogic.AppConfig("StoreName") + " String Table Empty Incident", "String Tables Empty, Reloaded at " + System.DateTime.Now.ToString() + ".<br><br>" + Explanation, false, AppLogic.AppConfig("MailMe_FromAddress"), AppLogic.AppConfig("MailMe_FromName"), AppLogic.AppConfig("MailMe_ToAddress"), AppLogic.AppConfig("MailMe_ToName"), String.Empty, AppLogic.AppConfig("MailMe_Server"));
                        }
                        catch { }
                    }
                    ht = (Hashtable)StringResourceTable[LocaleSetting];
                }
                String tmp = key.ToLower();
                if (ht.Contains(tmp))
                {
                    result = ht[tmp].ToString();
                }
                //else
                //{
                //     try to find it in the db, and add it forcefully, to avoid so many support tickets due to caching and people complaining they have missing strings on their site!
                //    String SVal = tmp;
                //    IDataReader rs = DB.GetRS("select * from StringResource where Name=" + DB.SQuote(tmp) + " and LocaleSetting=" + DB.SQuote(LocaleSetting));
                //    if (rs.Read())
                //    {
                //        SVal = DB.RSField(rs, "ConfigValue");
                //    }
                //    rs.Close();
                //    try
                //    {
                //        ht.Add(tmp, SVal);
                //    }
                //    catch { }
                //    result = SVal;
            }
            return result;
        }


        // ----------------------------------------------------------------
        //
        // APPCONFIG SUPPORT ROUTINES
        //
        // ----------------------------------------------------------------
        public static void SetAppConfig(String Name, String ConfigValue, bool ClearCache)
        {
            if (DB.GetSqlN("select count(name) as N from AppConfig " + DB.GetNoLock() + " where lower(name)=" + DB.SQuote(Name.ToLowerInvariant())) == 0)
            {
                DB.ExecuteSQL("insert into AppConfig(name,configvalue) values(" + DB.SQuote(Name.ToLowerInvariant()) + "," + DB.SQuote(ConfigValue) + ")");
                AppConfigTable.Add(Name.ToLowerInvariant(), ConfigValue);
            }
            else
            {
                DB.ExecuteSQL("update AppConfig set ConfigValue=" + DB.SQuote(ConfigValue) + " where lower(name)=" + DB.SQuote(Name.ToLowerInvariant()));
                AppConfigTable[Name.ToLowerInvariant()] = ConfigValue;
            }
            if (ClearCache)
            {
                AppLogic.ClearCache();
            }
        }

        public static String AppConfig(String paramName)
        {
            String tmpS = String.Empty;
            try
            {
                tmpS = AppConfigTable[paramName.ToLowerInvariant()].ToString();
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        public static bool AppConfigBool(String paramName)
        {
            String tmp = AppConfig(paramName).ToUpperInvariant();
            if (tmp == "TRUE" || tmp == "YES" || tmp == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int AppConfigUSInt(String paramName)
        {
            String tmpS = AppConfig(paramName);
            return Localization.ParseUSInt(tmpS);
        }

        public static long AppConfigUSLong(String paramName)
        {
            String tmpS = AppConfig(paramName);
            return Localization.ParseUSLong(tmpS);
        }

        public static Single AppConfigUSSingle(String paramName)
        {
            String tmpS = AppConfig(paramName);
            return Localization.ParseUSSingle(tmpS);
        }

        public static Double AppConfigUSDouble(String paramName)
        {
            String tmpS = AppConfig(paramName);
            return Localization.ParseUSDouble(tmpS);
        }

        public static Decimal AppConfigUSDecimal(String paramName)
        {
            String tmpS = AppConfig(paramName);
            return Localization.ParseUSDecimal(tmpS);
        }

        public static DateTime AppConfigUSDateTime(String paramName)
        {
            return Localization.ParseUSDateTime(AppConfig(paramName));
        }

        public static int AppConfigNativeInt(String paramName)
        {
            String tmpS = AppConfig(paramName);
            return Localization.ParseNativeInt(tmpS);
        }

        public static long AppConfigNativeLong(String paramName)
        {
            String tmpS = AppConfig(paramName);
            return Localization.ParseNativeLong(tmpS);
        }

        public static Single AppConfigNativeSingle(String paramName)
        {
            String tmpS = AppConfig(paramName);
            return Localization.ParseNativeSingle(tmpS);
        }

        public static Double AppConfigNativeDouble(String paramName)
        {
            String tmpS = AppConfig(paramName);
            return Localization.ParseNativeDouble(tmpS);
        }

        public static Decimal AppConfigNativeDecimal(String paramName)
        {
            String tmpS = AppConfig(paramName);
            return Localization.ParseNativeDecimal(tmpS);
        }

        public static DateTime AppConfigNativeDateTime(String paramName)
        {
            return Localization.ParseNativeDateTime(AppConfig(paramName));
        }


        public static Boolean SendMailInWebMail(String ToMail, String FromMail, String FromName, String SubjectTxt, String BodyTxt, Boolean IsHTML)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage
            {
                From = new MailAddress(FromMail, FromName)
            };
            message.To.Add(new MailAddress(ToMail));
            message.Subject = SubjectTxt;
            message.Body = BodyTxt;
            message.IsBodyHtml = IsHTML;

            SmtpClient smtp = new SmtpClient(CommonLogic.Application("SMTPMailServer"))
            {
                Credentials = new System.Net.NetworkCredential(CommonLogic.Application("SenderEmail"), CommonLogic.Application("SenderPWD")),
                Port = CommonLogic.ApplicationNativeInt("SMTPPort"),
                EnableSsl = false
            };

            try
            {
                smtp.Send(message);
            }
            catch
            {
                return false;
            }
            return true;

        }


        public static Boolean SendMailInWebMail(System.Net.Mail.MailMessage message)
        {
            //System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new MailAddress(CommonLogic.Application("SenderEmail"), "MRP Tool");
            //message.To.Add(new MailAddress(ToMail));
            //message.Subject = SubjectTxt;
            //message.Body = BodyTxt;
            //message.IsBodyHtml = IsHTML;

            message.IsBodyHtml = true;
            //SmtpClient smtp = new SmtpClient("exchange.atc.com.kw");
            //smtp.Credentials = new System.Net.NetworkCredential("trackline@atc.com.kw", "123Qwe");
            //smtp.Port = 25;
            SmtpClient smtp = new SmtpClient(CommonLogic.Application("SMTPMailServer"))
            {
                //smtp.Credentials = new System.Net.NetworkCredential(CommonLogic.Application("SenderEmail"), CommonLogic.Application("SenderPWD"));
                Credentials = new System.Net.NetworkCredential(),
                Port = CommonLogic.ApplicationNativeInt("SMTPPort"),
                EnableSsl = false,
                Timeout = 15000 // Timeout set to 5000 milliseconds (ie 5 seconds);
            };

            try
            {
                /*
                if (DateTime.Now.TimeOfDay.Hours > 16 && (DateTime.Now.TimeOfDay.Hours < 20))
                {
                    //Dont send mails between 5:00 pm and 8:00PM
                    return false;
                }
                {
                    smtp.Send(message);
                   // return true;
                }
                */

                //Commented on 09/03/2013 on Temporary basis and removed on 10/03/2013
                //Commented on 16/03/2013 on Temporary basis and removed on 17/03/2013
                
                smtp.Send(message);
                return true;

            }
            catch(Exception ex)
            {
                return false;
            }
            return true;

        }

    }

    public sealed class TracklinePrincipal : GenericPrincipal
    {
        public String m_Roles;

        public TracklinePrincipal(System.Security.Principal.IIdentity identity, string[] roles)
            : base(identity, roles)
        {
            StringBuilder tmpS = new StringBuilder(250);
            foreach (String s in roles)
            {
                if (tmpS.Length != 0)
                {
                    tmpS.Append(",");
                }
                tmpS.Append(s);
            }
            m_Roles = tmpS.ToString();
        }
    }


    
}
