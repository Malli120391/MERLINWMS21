using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using MRLWMSC21Common.Interfaces;


namespace MRLWMSC21Common
{
    public class AndroidSerialDataStructures 
    {
       public AndroidSerialDataStructures() { }

       [Serializable]
       public class GraphData
       {
           //Constructor
           public GraphData() { }
           /*
           private String[]  _mInOutActData;

           public String[] InOutActData
           {
               get { return _mInOutActData; }
               set { _mInOutActData = value; }
           }
           */

           public String[] InOutActData;
       }

/// 
       [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18034")]
       [System.Diagnostics.DebuggerStepThroughAttribute()]
       [System.ComponentModel.DesignerCategoryAttribute("code")]
       [Serializable, XmlRoot(ElementName = "Tab_SIT")]
       

       public partial class Tab_SIT
       {
           private int inBoundTrackingIDField;
           private bool inBoundTrackingIDFieldSpecified;
           private string storeRefNoField;
           private System.DateTime docReceivedDateField;
           private bool docReceivedDateFieldSpecified;
           private string shipmentTypeField;
           private byte isRTRPublishedField;
           private bool isRTRPublishedFieldSpecified;
           private string refStoresField;
           private string shipmentLocationField;
           private string supplierField;
           private int clearenceCompanyIDField;
           private bool clearenceCompanyIDFieldSpecified;
           private int priorityLevelField;
           private bool priorityLevelFieldSpecified;
           private System.DateTime priorityDateTimeField;
           private bool priorityDateTimeFieldSpecified;
           private string departmentIDField;
           private int lineItemCountField;
           private bool lineItemCountFieldSpecified;
           /// 
           public int InBoundTrackingID
           {
               get
               {
                   return this.inBoundTrackingIDField;
               }
               set
               {
                   this.inBoundTrackingIDField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool InBoundTrackingIDSpecified
           {
               get
               {
                   return this.inBoundTrackingIDFieldSpecified;
               }
               set
               {
                   this.inBoundTrackingIDFieldSpecified = value;
               }
           }
           /// 
           public string StoreRefNo
           {
               get
               {
                   return this.storeRefNoField;
               }
               set
               {
                   this.storeRefNoField = value;
               }
           }
           /// 
           public System.DateTime DocReceivedDate
           {
               get
               {
                   return this.docReceivedDateField;
               }
               set
               {
                   this.docReceivedDateField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool DocReceivedDateSpecified
           {
               get
               {
                   return this.docReceivedDateFieldSpecified;
               }
               set
               {
                   this.docReceivedDateFieldSpecified = value;
               }
           }
           /// 
           public string ShipmentType
           {
               get
               {
                   return this.shipmentTypeField;
               }
               set
               {
                   this.shipmentTypeField = value;
               }
           }
           /// 
           public byte IsRTRPublished
           {
               get
               {
                   return this.isRTRPublishedField;
               }
               set
               {
                   this.isRTRPublishedField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool IsRTRPublishedSpecified
           {
               get
               {
                   return this.isRTRPublishedFieldSpecified;
               }
               set
               {
                   this.isRTRPublishedFieldSpecified = value;
               }
           }
           /// 
           public string RefStores
           {
               get
               {
                   return this.refStoresField;
               }
               set
               {
                   this.refStoresField = value;
               }
           }
           /// 
           public string ShipmentLocation
           {
               get
               {
                   return this.shipmentLocationField;
               }
               set
               {
                   this.shipmentLocationField = value;
               }
           }
           /// 
           public string Supplier
           {
               get
               {
                   return this.supplierField;
               }
               set
               {
                   this.supplierField = value;
               }
           }
           /// 
           public int ClearenceCompanyID
           {
               get
               {
                   return this.clearenceCompanyIDField;
               }
               set
               {
                   this.clearenceCompanyIDField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool ClearenceCompanyIDSpecified
           {
               get
               {
                   return this.clearenceCompanyIDFieldSpecified;
               }
               set
               {
                   this.clearenceCompanyIDFieldSpecified = value;
               }
           }
           /// 
           public int PriorityLevel
           {
               get
               {
                   return this.priorityLevelField;
               }
               set
               {
                   this.priorityLevelField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool PriorityLevelSpecified
           {
               get
               {
                   return this.priorityLevelFieldSpecified;
               }
               set
               {
                   this.priorityLevelFieldSpecified = value;
               }
           }
           /// 
           public System.DateTime PriorityDateTime
           {
               get
               {
                   return this.priorityDateTimeField;
               }
               set
               {
                   this.priorityDateTimeField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool PriorityDateTimeSpecified
           {
               get
               {
                   return this.priorityDateTimeFieldSpecified;
               }
               set
               {
                   this.priorityDateTimeFieldSpecified = value;
               }
           }
           /// 
           public string DepartmentID
           {
               get
               {
                   return this.departmentIDField;
               }
               set
               {
                   this.departmentIDField = value;
               }
           }
           /// 
           public int LineItemCount
           {
               get
               {
                   return this.lineItemCountField;
               }
               set
               {
                   this.lineItemCountField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool LineItemCountSpecified
           {
               get
               {
                   return this.lineItemCountFieldSpecified;
               }
               set
               {
                   this.lineItemCountFieldSpecified = value;
               }
           }
       }
       /// 
       [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18034")]
       [System.SerializableAttribute()]
       [System.Diagnostics.DebuggerStepThroughAttribute()]
       [System.ComponentModel.DesignerCategoryAttribute("code")]
       [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "NS_Tab_SIT")]
       [System.Xml.Serialization.XmlRootAttribute(Namespace = "NS_Tab_SIT", IsNullable = false)]
       public partial class Tab_SIP
       {
           private int inBoundTrackingIDField;
           private bool inBoundTrackingIDFieldSpecified;
           private string storeRefNoField;
           private System.DateTime docReceivedDateField;
           private bool docReceivedDateFieldSpecified;
           private string shipmentTypeField;
           private byte isRTRPublishedField;
           private bool isRTRPublishedFieldSpecified;
           private string refStoresField;
           private string shipmentLocationField;
           private string supplierField;
           private int clearenceCompanyIDField;
           private bool clearenceCompanyIDFieldSpecified;
           private System.DateTime shipmentExpectedDateField;
           private bool shipmentExpectedDateFieldSpecified;
           private int inBoundStatusIDField;
           private bool inBoundStatusIDFieldSpecified;
           private int priorityLevelField;
           private bool priorityLevelFieldSpecified;
           private System.DateTime priorityDateTimeField;
           private bool priorityDateTimeFieldSpecified;
           private string departmentIDField;
           private int lineItemCountField;
           private bool lineItemCountFieldSpecified;
           private System.DateTime shipmentReceivedOnField;
           private bool shipmentReceivedOnFieldSpecified;
           /// 
           public int InBoundTrackingID
           {
               get
               {
                   return this.inBoundTrackingIDField;
               }
               set
               {
                   this.inBoundTrackingIDField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool InBoundTrackingIDSpecified
           {
               get
               {
                   return this.inBoundTrackingIDFieldSpecified;
               }
               set
               {
                   this.inBoundTrackingIDFieldSpecified = value;
               }
           }
           /// 
           public string StoreRefNo
           {
               get
               {
                   return this.storeRefNoField;
               }
               set
               {
                   this.storeRefNoField = value;
               }
           }
           /// 
           public System.DateTime DocReceivedDate
           {
               get
               {
                   return this.docReceivedDateField;
               }
               set
               {
                   this.docReceivedDateField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool DocReceivedDateSpecified
           {
               get
               {
                   return this.docReceivedDateFieldSpecified;
               }
               set
               {
                   this.docReceivedDateFieldSpecified = value;
               }
           }
           /// 
           public string ShipmentType
           {
               get
               {
                   return this.shipmentTypeField;
               }
               set
               {
                   this.shipmentTypeField = value;
               }
           }
           /// 
           public byte IsRTRPublished
           {
               get
               {
                   return this.isRTRPublishedField;
               }
               set
               {
                   this.isRTRPublishedField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool IsRTRPublishedSpecified
           {
               get
               {
                   return this.isRTRPublishedFieldSpecified;
               }
               set
               {
                   this.isRTRPublishedFieldSpecified = value;
               }
           }
           /// 
           public string RefStores
           {
               get
               {
                   return this.refStoresField;
               }
               set
               {
                   this.refStoresField = value;
               }
           }
           /// 
           public string ShipmentLocation
           {
               get
               {
                   return this.shipmentLocationField;
               }
               set
               {
                   this.shipmentLocationField = value;
               }
           }
           /// 
           public string Supplier
           {
               get
               {
                   return this.supplierField;
               }
               set
               {
                   this.supplierField = value;
               }
           }
           /// 
           public int ClearenceCompanyID
           {
               get
               {
                   return this.clearenceCompanyIDField;
               }
               set
               {
                   this.clearenceCompanyIDField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool ClearenceCompanyIDSpecified
           {
               get
               {
                   return this.clearenceCompanyIDFieldSpecified;
               }
               set
               {
                   this.clearenceCompanyIDFieldSpecified = value;
               }
           }
           /// 
           public System.DateTime ShipmentExpectedDate
           {
               get
               {
                   return this.shipmentExpectedDateField;
               }
               set
               {
                   this.shipmentExpectedDateField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool ShipmentExpectedDateSpecified
           {
               get
               {
                   return this.shipmentExpectedDateFieldSpecified;
               }
               set
               {
                   this.shipmentExpectedDateFieldSpecified = value;
               }
           }
           /// 
           public int InBoundStatusID
           {
               get
               {
                   return this.inBoundStatusIDField;
               }
               set
               {
                   this.inBoundStatusIDField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool InBoundStatusIDSpecified
           {
               get
               {
                   return this.inBoundStatusIDFieldSpecified;
               }
               set
               {
                   this.inBoundStatusIDFieldSpecified = value;
               }
           }
           /// 
           public int PriorityLevel
           {
               get
               {
                   return this.priorityLevelField;
               }
               set
               {
                   this.priorityLevelField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool PriorityLevelSpecified
           {
               get
               {
                   return this.priorityLevelFieldSpecified;
               }
               set
               {
                   this.priorityLevelFieldSpecified = value;
               }
           }
           /// 
           public System.DateTime PriorityDateTime
           {
               get
               {
                   return this.priorityDateTimeField;
               }
               set
               {
                   this.priorityDateTimeField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool PriorityDateTimeSpecified
           {
               get
               {
                   return this.priorityDateTimeFieldSpecified;
               }
               set
               {
                   this.priorityDateTimeFieldSpecified = value;
               }
           }
           /// 
           public string DepartmentID
           {
               get
               {
                   return this.departmentIDField;
               }
               set
               {
                   this.departmentIDField = value;
               }
           }
           /// 
           public int LineItemCount
           {
               get
               {
                   return this.lineItemCountField;
               }
               set
               {
                   this.lineItemCountField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool LineItemCountSpecified
           {
               get
               {
                   return this.lineItemCountFieldSpecified;
               }
               set
               {
                   this.lineItemCountFieldSpecified = value;
               }
           }
           /// 
           public System.DateTime ShipmentReceivedOn
           {
               get
               {
                   return this.shipmentReceivedOnField;
               }
               set
               {
                   this.shipmentReceivedOnField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool ShipmentReceivedOnSpecified
           {
               get
               {
                   return this.shipmentReceivedOnFieldSpecified;
               }
               set
               {
                   this.shipmentReceivedOnFieldSpecified = value;
               }
           }
       }
       /// 
       [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18034")]
       [System.SerializableAttribute()]
       [System.Diagnostics.DebuggerStepThroughAttribute()]
       [System.ComponentModel.DesignerCategoryAttribute("code")]
       [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "NS_Tab_SIT")]
       [System.Xml.Serialization.XmlRootAttribute(Namespace = "NS_Tab_SIT", IsNullable = false)]
       public partial class Tab_GRNPending
       {
           private int inBoundTrackingIDField;
           private bool inBoundTrackingIDFieldSpecified;
           private string storeRefNoField;
           private System.DateTime docReceivedDateField;
           private bool docReceivedDateFieldSpecified;
           private string shipmentTypeField;
           private byte isRTRPublishedField;
           private bool isRTRPublishedFieldSpecified;
           private string refStoresField;
           private string shipmentLocationField;
           private string supplierField;
           private int clearenceCompanyIDField;
           private bool clearenceCompanyIDFieldSpecified;
           private int inBoundStatusIDField;
           private bool inBoundStatusIDFieldSpecified;
           private int priorityLevelField;
           private bool priorityLevelFieldSpecified;
           private System.DateTime priorityDateTimeField;
           private bool priorityDateTimeFieldSpecified;
           private string purchaseGroupIDField;
           private System.DateTime shipmentReceivedOnField;
           private bool shipmentReceivedOnFieldSpecified;
           private System.DateTime shipmentVerifiedOnField;
           private bool shipmentVerifiedOnFieldSpecified;
           private int lineItemCountField;
           private bool lineItemCountFieldSpecified;
           /// 
           public int InBoundTrackingID
           {
               get
               {
                   return this.inBoundTrackingIDField;
               }
               set
               {
                   this.inBoundTrackingIDField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool InBoundTrackingIDSpecified
           {
               get
               {
                   return this.inBoundTrackingIDFieldSpecified;
               }
               set
               {
                   this.inBoundTrackingIDFieldSpecified = value;
               }
           }
           /// 
           public string StoreRefNo
           {
               get
               {
                   return this.storeRefNoField;
               }
               set
               {
                   this.storeRefNoField = value;
               }
           }
           /// 
           public System.DateTime DocReceivedDate
           {
               get
               {
                   return this.docReceivedDateField;
               }
               set
               {
                   this.docReceivedDateField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool DocReceivedDateSpecified
           {
               get
               {
                   return this.docReceivedDateFieldSpecified;
               }
               set
               {
                   this.docReceivedDateFieldSpecified = value;
               }
           }
           /// 
           public string ShipmentType
           {
               get
               {
                   return this.shipmentTypeField;
               }
               set
               {
                   this.shipmentTypeField = value;
               }
           }
           /// 
           public byte IsRTRPublished
           {
               get
               {
                   return this.isRTRPublishedField;
               }
               set
               {
                   this.isRTRPublishedField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool IsRTRPublishedSpecified
           {
               get
               {
                   return this.isRTRPublishedFieldSpecified;
               }
               set
               {
                   this.isRTRPublishedFieldSpecified = value;
               }
           }
           /// 
           public string RefStores
           {
               get
               {
                   return this.refStoresField;
               }
               set
               {
                   this.refStoresField = value;
               }
           }
           /// 
           public string ShipmentLocation
           {
               get
               {
                   return this.shipmentLocationField;
               }
               set
               {
                   this.shipmentLocationField = value;
               }
           }
           /// 
           public string Supplier
           {
               get
               {
                   return this.supplierField;
               }
               set
               {
                   this.supplierField = value;
               }
           }
           /// 
           public int ClearenceCompanyID
           {
               get
               {
                   return this.clearenceCompanyIDField;
               }
               set
               {
                   this.clearenceCompanyIDField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool ClearenceCompanyIDSpecified
           {
               get
               {
                   return this.clearenceCompanyIDFieldSpecified;
               }
               set
               {
                   this.clearenceCompanyIDFieldSpecified = value;
               }
           }
           /// 
           public int InBoundStatusID
           {
               get
               {
                   return this.inBoundStatusIDField;
               }
               set
               {
                   this.inBoundStatusIDField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool InBoundStatusIDSpecified
           {
               get
               {
                   return this.inBoundStatusIDFieldSpecified;
               }
               set
               {
                   this.inBoundStatusIDFieldSpecified = value;
               }
           }
           /// 
           public int PriorityLevel
           {
               get
               {
                   return this.priorityLevelField;
               }
               set
               {
                   this.priorityLevelField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool PriorityLevelSpecified
           {
               get
               {
                   return this.priorityLevelFieldSpecified;
               }
               set
               {
                   this.priorityLevelFieldSpecified = value;
               }
           }
           /// 
           public System.DateTime PriorityDateTime
           {
               get
               {
                   return this.priorityDateTimeField;
               }
               set
               {
                   this.priorityDateTimeField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool PriorityDateTimeSpecified
           {
               get
               {
                   return this.priorityDateTimeFieldSpecified;
               }
               set
               {
                   this.priorityDateTimeFieldSpecified = value;
               }
           }
           /// 
           public string PurchaseGroupID
           {
               get
               {
                   return this.purchaseGroupIDField;
               }
               set
               {
                   this.purchaseGroupIDField = value;
               }
           }
           /// 
           public System.DateTime ShipmentReceivedOn
           {
               get
               {
                   return this.shipmentReceivedOnField;
               }
               set
               {
                   this.shipmentReceivedOnField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool ShipmentReceivedOnSpecified
           {
               get
               {
                   return this.shipmentReceivedOnFieldSpecified;
               }
               set
               {
                   this.shipmentReceivedOnFieldSpecified = value;
               }
           }
           /// 
           public System.DateTime ShipmentVerifiedOn
           {
               get
               {
                   return this.shipmentVerifiedOnField;
               }
               set
               {
                   this.shipmentVerifiedOnField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool ShipmentVerifiedOnSpecified
           {
               get
               {
                   return this.shipmentVerifiedOnFieldSpecified;
               }
               set
               {
                   this.shipmentVerifiedOnFieldSpecified = value;
               }
           }
           /// 
           public int LineItemCount
           {
               get
               {
                   return this.lineItemCountField;
               }
               set
               {
                   this.lineItemCountField = value;
               }
           }
           /// 
           [System.Xml.Serialization.XmlIgnoreAttribute()]
           public bool LineItemCountSpecified
           {
               get
               {
                   return this.lineItemCountFieldSpecified;
               }
               set
               {
                   this.lineItemCountFieldSpecified = value;
               }
           }
       }



    }
}
