using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class StorageLocationDTO
    {
        private string _SLocName;

        private string _SLocCode;

        private int _SLocID;

        private bool _IsLostAndFound;
        private string _IsDefault;
        public string IsDefault { get => _IsDefault; set => _IsDefault = value; }


        public string SLocName { get => _SLocName; set => _SLocName = value; }
        public string SLocCode { get => _SLocCode; set => _SLocCode = value; }
        public int SLocID { get => _SLocID; set => _SLocID = value; }
        public bool IsLostAndFound { get => _IsLostAndFound; set => _IsLostAndFound = value; }
    }
}