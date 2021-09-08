using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MRLWMSC21Common;
using System.Data;
using System.Collections;
using System.ServiceModel.Web;
using System.Xml;

namespace MRLWMSC21WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFalcon" in both code and config file together.
    [ServiceContract]
    public interface IFalcon
    {
        [OperationContract]
        void DoWork();
        [OperationContract]
        string PostIDocToFalcon(XmlDocument IdocInfo,string Client,string IdocNumber);

        [OperationContract]
        string PostWMSScanDataAsIDOC(string IdocInfo, string Client, string RefNumber,string IDOCName,string SiteCode);



    }
}
