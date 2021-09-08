using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MRLWMSC21_Library.Parser
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

                bool isValidMsg = isValidationRequire ? ValidateSchema(ascii.GetString(mem.ToArray())) : true;
                if (isValidMsg)
                {
                    return ascii.GetString(mem.ToArray());
                }
                else
                {
                    throw new Exception("invalid object, please have a look on input object");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object Deserialize(Type typeToDeserialize, string xmlString, bool isValidationRequire = false)
        {
            try
            {

                bool isValidInputMessage = (isValidationRequire) ? ValidateSchema(xmlString) : true;
                if (isValidInputMessage)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(xmlString);
                    MemoryStream mem = new MemoryStream(bytes);
                    XmlSerializer ser = new XmlSerializer(typeToDeserialize);
                    return ser.Deserialize(mem);
                }
                else
                {
                    throw new Exception("invalid message, please have a look on input message");
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
