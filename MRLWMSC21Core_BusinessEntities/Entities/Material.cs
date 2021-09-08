using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class Material
    {

        private int _MaterialMasterID;
        private string _MCode;

        private decimal _MRP;
        private int _MOP;

        private int _GenericSKUID;
        private string _GenericSKUCode;

        private int _ColorID;
        private string _ColorCode;

        private int _PageFormatID;
        private string _PageFormat;

        private int _CategoryID;
        private string _Category;

        private int _DivisionID;
        private string _Division;

        private string _MaterialShortDescription;

        private bool _IsParent;
        private bool _IsFinishedGoods;
        private bool _IsRawMaterial;
        private bool _IsConsumables;

        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public string MCode { get => _MCode; set => _MCode = value; }
        public decimal MRP { get => _MRP; set => _MRP = value; }
        public int MOP { get => _MOP; set => _MOP = value; }
        public int GenericSKUID { get => _GenericSKUID; set => _GenericSKUID = value; }
        public string GenericSKUCode { get => _GenericSKUCode; set => _GenericSKUCode = value; }
        public int ColorID { get => _ColorID; set => _ColorID = value; }
        public string ColorCode { get => _ColorCode; set => _ColorCode = value; }
        public int PageFormatID { get => _PageFormatID; set => _PageFormatID = value; }
        public string PageFormat { get => _PageFormat; set => _PageFormat = value; }
        public int CategoryID { get => _CategoryID; set => _CategoryID = value; }
        public string Category { get => _Category; set => _Category = value; }
        public int DivisionID { get => _DivisionID; set => _DivisionID = value; }
        public string Division { get => _Division; set => _Division = value; }
        public string MaterialShortDescription { get => _MaterialShortDescription; set => _MaterialShortDescription = value; }
        public bool IsParent { get => _IsParent; set => _IsParent = value; }
        public bool IsFinishedGoods { get => _IsFinishedGoods; set => _IsFinishedGoods = value; }
        public bool IsRawMaterial { get => _IsRawMaterial; set => _IsRawMaterial = value; }
        public bool IsConsumables { get => _IsConsumables; set => _IsConsumables = value; }
    }
}
