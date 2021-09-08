using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MRLWMSC21WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITallyIntegrationService" in both code and config file together.
    [ServiceContract]
    public interface ITallyIntegrationService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        string PostIDocToFalconString(String IdocInfo, String Client, String IdocNumber);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "XMLRequest")]
        //[WebInvoke(UriTemplate = "XMLRequest", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        string PostIDocToFalconXML(MemoryStream xmlDocument, String CompanyName, String IDocInfo);



    }
}
