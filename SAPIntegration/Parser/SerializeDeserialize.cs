using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace SAPIntegration.Parser
{
    public class SerializeDeserialize
    {


        public static string xsdPath;
        public static string xmlPath;

        public static string Serialize(object objectToSerialize, bool isValidationRequire = false)
        {
            try
            {
                MemoryStream mem = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(objectToSerialize.GetType());
                ser.Serialize(mem, objectToSerialize);
                ASCIIEncoding ascii = new ASCIIEncoding();

                if (!isValidationRequire)
                {
                    return ascii.GetString(mem.ToArray());
                }
                else if (ValidateSchema(ascii.GetString(mem.ToArray())))
                {
                    return ascii.GetString(mem.ToArray());
                }
                else
                {
                  return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while doing something: " + ex.Message);
                return null;
            }
        }

        public static object Deserialize(Type typeToDeserialize, string xmlString, bool isValidationRequire = false)
        {
            try
            {

                if (!isValidationRequire)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(xmlString);
                    MemoryStream mem = new MemoryStream(bytes);
                    XmlSerializer ser = new XmlSerializer(typeToDeserialize);
                    return ser.Deserialize(mem);

                }
                else if (ValidateSchema(xmlString))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(xmlString);
                    MemoryStream mem = new MemoryStream(bytes);
                    XmlSerializer ser = new XmlSerializer(typeToDeserialize);
                    return ser.Deserialize(mem);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while marshalling xml object: " + ex.Message+"\n\n");
                Console.WriteLine("Exception while marshalling xml object: " + ex.StackTrace);
                return null;
            }
        }

        public static bool ValidateSchema(string text)
        {
            try
            {
                XmlDocument xmld = new XmlDocument();
                xmld.LoadXml(text);
                xmld.Schemas.Add(null, xsdPath);
                xmld.Validate(ValidationCallBack);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            Console.WriteLine("Message: " + e.Message + "/" + e.Exception.StackTrace);
            throw new Exception();
        }
    }
}
