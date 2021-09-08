using System;
using System.Web;
using System.Security;
using System.Configuration;
using System.Web.SessionState;
using System.Web.Caching;
using System.Web.Util;
using System.Data;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Resources;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MRLWMSC21Common
{
	/// <summary>
	/// Summary description for XmlCommon.
	/// </summary>
	public class XmlCommon
	{

		public XmlCommon() 
		{
		}

        static public String SerializeObject(Object pObject, System.Type objectType) 
		{
			try 
			{
				String XmlizedString = null;

               // XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
               // xmlnsEmpty.Add("", "http://www.wow.thisworks.com/2010/05");


				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(objectType);

                XmlTextWriter XmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                /*
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Encoding = Encoding.UTF8,
                    Indent = true,
                    NewLineOnAttributes = true,
                };

				
               // XmlTextWriter XmlTextWriter = XmlWriter.Create(memoryStream, settings);
               // XmlWriter XmlTextWriter = XmlWriter.Create(memoryStream, settings);


                XmlTextWriter.Indentation = 1;
                XmlTextWriter.Formatting = Formatting.Indented;
                XmlTextWriter.IndentChar = '\n';
                */
                //XmlTextWriter.Settings.Encoding  = Encoding.UTF8 ;
                //XmlTextWriter.Settings.Indent = true;
                //XmlTextWriter.Settings.NewLineOnAttributes  = true;

                //xs.Serialize(XmlTextWriter, pObject, xmlnsEmpty);
                xs.Serialize(XmlTextWriter, pObject);
				memoryStream = (MemoryStream)XmlTextWriter.BaseStream;
                
				XmlizedString = CommonLogic.UTF8ByteArrayToString(memoryStream.ToArray());
				return XmlizedString;
			}
			catch (Exception ex)
			{
				return CommonLogic.GetExceptionDetail(ex,"\n");
			}
		}



        static public object DeSerializeLoginUserData(string xmlOfAnObject) 
        {
           THHTWSData.LoginUserData   myObject = new THHTWSData.LoginUserData();
           System.IO.StringReader read = new StringReader(xmlOfAnObject); 
           
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(myObject.GetType()); 
            //System.Xml.XmlReader reader = new XmlTextReader(read);
            XmlTextReader reader = new XmlTextReader(read);

            try
            {
                myObject = (THHTWSData.LoginUserData)serializer.Deserialize(reader);
                return myObject; } 
            catch(Exception ex) { 
                throw ;
            } 
            finally 
            { 
                reader.Close();
                read.Close();
                read.Dispose();
            } 
        }


        public static THHTWSData.LoginUserData DeserializeLUD(string xmlOfAnObject)
        {
            MemoryStream stream = new MemoryStream();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(xmlOfAnObject);
            stream.Write(bytes, 0, bytes.Length);
            XmlSerializer formatter = new XmlSerializer(typeof(THHTWSData.LoginUserData));
            System.Xml.XmlTextReader xmlReader = new System.Xml.XmlTextReader(stream);
            //stream.Seek(0, 0);
            using (xmlReader)
            {
                //return formatter.Deserialize(xmlReader) as THHTWSData.LoginUserData;

                THHTWSData.LoginUserData thisLUD = (THHTWSData.LoginUserData) formatter.Deserialize(xmlReader);
                return thisLUD;
            }
        }


        public static string SerializeLUDToString(THHTWSData.LoginUserData cUser)
             {
                 System.IO.MemoryStream stream = new System.IO.MemoryStream();
                 System.Xml.Serialization.XmlSerializer formatter = new XmlSerializer(typeof(THHTWSData.LoginUserData));
                 System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(stream,System.Text.Encoding.UTF8);
                 xmlWriter.Flush();
                 stream.Seek(0,0);
                 formatter.Serialize(xmlWriter, cUser);
                 xmlWriter.WriteEndDocument();
                 return System.Text.Encoding.UTF8.GetString(stream.GetBuffer());
             }

        public static string  SerializeObject(Object objToSerialize)
        {
            XmlSerializer ser = new XmlSerializer(objToSerialize.GetType());
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(sb);
            ser.Serialize(writer, objToSerialize);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            
            return writer.ToString();
        }

        public static object DeSerializeAnObject(String  xmldocString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmldocString);

            XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
            THHTWSData.LoginUserData objType = new THHTWSData.LoginUserData();
            XmlSerializer ser = new XmlSerializer(objType.GetType());
            object obj = ser.Deserialize(reader);
            return obj;
        } 

		static public String FormatXml(XmlDocument inputXml)
		{
			StringWriter writer = new StringWriter();
            XmlTextWriter XmlWriter = new XmlTextWriter(writer)
            {
                Formatting = Formatting.Indented,
                Indentation = 2
            };
            inputXml.WriteTo(XmlWriter);
			return writer.ToString();
		}

		public static String PrettyPrintXml(String Xml)
		{
			String Result = Xml;
            if (Xml.Length != 0)
            {
                Xml = Xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                try
                {
                    // Load the XmlDocument with the Xml.
                    XmlDocument D = new XmlDocument();
                    D.LoadXml(Xml);
                    MemoryStream MS = new MemoryStream();
                    XmlTextWriter W = new XmlTextWriter(MS, Encoding.Unicode)
                    {
                        Formatting = Formatting.Indented
                    };

                    // Write the Xml into a formatting XmlTextWriter
                    D.WriteContentTo(W);
                    W.Flush();
                    MS.Flush();

                    // Have to rewind the MemoryStream in order to read
                    // its contents.
                    MS.Position = 0;

                    // Read MemoryStream contents into a StreamReader.
                    StreamReader SR = new StreamReader(MS);

                    // Extract the text from the StreamReader.
                    String FormattedXml = SR.ReadToEnd();

                    Result = FormattedXml;

                    try
                    {
                        MS.Close();
                        MS = null;
                        W.Close();
                        W = null;
                    }
                    catch { }
                }
                catch { }
            }
			return Result;
		}

		// strips illegal Xml characters:
		static public String XmlEncode(String S)
		{
			if (S == null) 
			{
				return null; 
			}
			S=Regex.Replace(S,@"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]","");
			return XmlEncodeAsIs(S);
		}

		// leaves whatever data is there, and just XmlEncodes it:
		static public String XmlEncodeAsIs(String S)
		{
			if (S == null) 
			{
				return null; 
			}
			StringWriter sw = new StringWriter();
			XmlTextWriter xwr = new XmlTextWriter(sw);
			xwr.WriteString(S);
			String sTmp = sw.ToString();
			xwr.Close();
			sw.Close();
			return sTmp;
		}

		// strips illegal Xml characters:
		static public String XmlEncodeAttribute(String S)
		{
			if (S == null) 
			{
				return null; 
			}
			S=Regex.Replace(S,@"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]","");
			return XmlEncodeAttributeAsIs(S);
		}

		// leaves whatever data is there, and just XmlEncodes it:
		static public String XmlEncodeAttributeAsIs(String S)
		{
			if (S == null) 
			{
				return null; 
			}
			StringWriter sw = new StringWriter();
			XmlTextWriter xwr = new XmlTextWriter(sw);
			xwr.WriteString(S);
			String sTmp = sw.ToString();
			xwr.Close();
			sw.Close();
			return sTmp.Replace("\"","&quot;");
		}
		
		static public String XmlEncodeComment(String S)
		{
			if (S == null) 
			{
				return null; 
			}
			return S.Replace("--","- -"); // -- combination is not allowed, everything else is valid
		}
		
		static public String XmlDecode(String S)
		{
			StringBuilder tmpS = new StringBuilder(S);
			String sTmp = tmpS.Replace("&quot;","\"").Replace("&apos;","'").Replace("&lt;","<").Replace("&gt;",">").Replace("&amp;","&").ToString();
			return sTmp;
		}

		// ----------------------------------------------------------------
		//
		// SIMPLE Xml FIELD ROUTINES
		//
		// ----------------------------------------------------------------

		public static String GetLocaleEntry(String S, String LocaleSetting, bool fallBack)
		{
#if PRO
			return S;
#else
			String tmpS = String.Empty;
			if(S.Length == 0)
			{
				return tmpS;
			}
			if(S.StartsWith("&lt;ml&gt;"))
			{
				S = XmlDecode(S);
			}
			if(S.StartsWith("<ml>"))
			{
				String WebConfigLocale = Localization.GetWebConfigLocale();
				if(AppLogic.AppConfigBool("UseXmlDOMForLocaleExtraction"))
				{
					try
					{
						XmlDocument doc = new XmlDocument();
						doc.LoadXml(S);
						XmlNode node = doc.DocumentElement.SelectSingleNode("//locale[@name=\"" + LocaleSetting + "\"]");
						if (fallBack && (node == null))
						{
							node = doc.DocumentElement.SelectSingleNode("//locale[@name=\"" + WebConfigLocale + "\"]");
						}
						if(node != null)
						{
							tmpS = node.InnerText.Trim();
						}
						if(tmpS.Length != 0)
						{
							tmpS = XmlCommon.XmlDecode(tmpS);
						}
					}
					catch {}
				}
				else
				{
					// for speed, we are using lightweight simple string token extraction here, not full Xml DOM for speed
					// return what is between <locale name=\"en-US\">...</locale>, Xml Decoded properly.
					// we have a good locale field formatted field, so try to get desired locale:
					if(S.IndexOf("<locale name=\"" + LocaleSetting + "\">") != -1)
					{
						tmpS = CommonLogic.ExtractToken(S,"<locale name=\"" + LocaleSetting + "\">","</locale>");
					}
					else if (fallBack && (S.IndexOf("<locale name=\"" + WebConfigLocale + "\">") != -1))
					{
						tmpS = CommonLogic.ExtractToken(S,"<locale name=\"" + WebConfigLocale + "\">","</locale>");
					}
					else
					{
						tmpS = String.Empty;
					}
					if(tmpS.Length != 0)
					{
						tmpS = XmlCommon.XmlDecode(tmpS);
					}
				}
			}
			else
			{
				tmpS = S; // for backwards compatibility...they have no locale info, so just return the field.
			}
			return tmpS;
#endif
		}


		// assumes this "xmlnode" n has <ml>...</ml> markup on it!
		public static String GetLocaleEntry(XmlNode n, String LocaleSetting, bool fallBack)
		{
			String tmpS = String.Empty;
#if PRO
			if(n != null)
			{
				tmpS = n.InnerText.Trim(); // for backwards compatibility...they have no locale info, so just return the field.
			}
#else
			if(n != null)
			{
				if(n.InnerText.StartsWith("&lt;ml&gt;"))
				{
					return GetLocaleEntry(XmlDecode(n.InnerText),LocaleSetting,fallBack);
				}
				if(n.InnerXml.StartsWith("<ml>"))
				{
					String WebConfigLocale = Localization.GetWebConfigLocale();
					try
					{
						XmlNode node = n.SelectSingleNode("ml/locale[@name=\"" + LocaleSetting + "\"]");
						if (fallBack && (node == null))
						{
							node = n.SelectSingleNode("ml/locale[@name=\"" + WebConfigLocale + "\"]");
						}
						if(node != null)
						{
							tmpS = node.InnerText.Trim();
						}
						if(tmpS.Length != 0)
						{
							tmpS = XmlCommon.XmlDecode(tmpS);
						}
					}
					catch {}
				}
				else
				{
					tmpS = n.InnerText.Trim(); // for backwards compatibility...they have no locale info, so just return the field.
				}
			}
#endif
			return tmpS;
		}
		

		
		public static String XmlField(XmlNode node, String fieldName)
		{
			String fieldVal = String.Empty;
			try
			{
				fieldVal = node.SelectSingleNode(@fieldName).InnerText.Trim();
			}
			catch {} // node might not be there
			return fieldVal;
		}

		public static String XmlFieldByLocale(XmlNode node, String fieldName, String LocaleSetting)
		{
			String fieldVal = String.Empty;
#if PRO
			XmlNode n = node.SelectSingleNode(@fieldName);
			if(n != null)
			{
				fieldVal = n.InnerText.Trim();
			}
#else
			XmlNode n = node.SelectSingleNode(@fieldName);
			if(n != null)
			{
				if(n.InnerXml.StartsWith("&lt;ml&gt;"))
				{
					fieldVal = GetLocaleEntry(XmlCommon.XmlDecode(n.InnerText.Trim()),LocaleSetting,true);
				}
				if(n.InnerXml.StartsWith("<ml>"))
				{
					fieldVal = GetLocaleEntry(n,LocaleSetting,true);
				}
				else
				{
					fieldVal = n.InnerText.Trim();
				}
			}
			if(fieldVal.StartsWith("<ml>"))
			{
				fieldVal = GetLocaleEntry(fieldVal,LocaleSetting,true);
			}
#endif
			return fieldVal;
			
		}

		public static bool XmlFieldBool(XmlNode node, String fieldName)
		{
			String tmp = XmlField(node,fieldName).ToUpperInvariant();
			if(tmp == "TRUE" || tmp == "YES" || tmp == "1")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static int XmlFieldUSInt(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long XmlFieldUSLong(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single XmlFieldUSSingle(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double XmlFieldUSDouble(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static decimal XmlFieldUSDecimal(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSCurrency(tmpS);
		}

		public static DateTime XmlFieldUSDateTime(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int XmlFieldNativeInt(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long XmlFieldNativeLong(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single XmlFieldNativeSingle(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double XmlFieldNativeDouble(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static decimal XmlFieldNativeDecimal(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime XmlFieldNativeDateTime(XmlNode node, String fieldName)
		{
			String tmpS = XmlField(node,fieldName);
			return Localization.ParseNativeDateTime(tmpS);
		}

		// ----------------------------------------------------------------
		//
		// SIMPLE Xml ATTRIBUTE ROUTINES
		//
		// ----------------------------------------------------------------

		public static String XmlAttribute(XmlNode node, String AttributeName)
		{
			String AttributeVal = String.Empty;
			try
			{
				AttributeVal = node.Attributes[AttributeName].InnerText.Trim();
			}
			catch {} // node might not be there
			return AttributeVal;
		}

		public static bool XmlAttributeBool(XmlNode node, String AttributeName)
		{
			String tmp = XmlAttribute(node,AttributeName).ToUpperInvariant();
			if(tmp == "TRUE" || tmp == "YES" || tmp == "1")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static int XmlAttributeUSInt(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSInt(tmpS);
		}

		public static long XmlAttributeUSLong(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSLong(tmpS);
		}

		public static Single XmlAttributeUSSingle(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSSingle(tmpS);
		}

		public static Double XmlAttributeUSDouble(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSDouble(tmpS);
		}

		public static decimal XmlAttributeUSDecimal(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSDecimal(tmpS);
		}

		public static DateTime XmlAttributeUSDateTime(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseUSDateTime(tmpS);
		}

		public static int XmlAttributeNativeInt(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeInt(tmpS);
		}

		public static long XmlAttributeNativeLong(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeLong(tmpS);
		}

		public static Single XmlAttributeNativeSingle(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeSingle(tmpS);
		}

		public static Double XmlAttributeNativeDouble(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeDouble(tmpS);
		}

		public static decimal XmlAttributeNativeDecimal(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeDecimal(tmpS);
		}

		public static DateTime XmlAttributeNativeDateTime(XmlNode node, String AttributeName)
		{
			String tmpS = XmlAttribute(node,AttributeName);
			return Localization.ParseNativeDateTime(tmpS);
		}

		public static String GetXPathEntry(String S, String XPath)
		{
			String tmpS = String.Empty;
			if(S.Length == 0)
			{
				return tmpS;
			}
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(S);
				XmlNode node = doc.DocumentElement.SelectSingleNode(XPath);
				if(node != null)
				{
					tmpS = node.InnerText;
				}
				if(tmpS.Length != 0)
				{
					tmpS = XmlCommon.XmlDecode(tmpS);
				}
			}
			catch {}
			return tmpS;
		}

	}
}
