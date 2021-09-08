using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class UserRoles
    {
        private int _UserID;
        private int _RoleID;

        private int _WarehouseID;

        private string _Role;

        private bool _IsView;
        private bool _IsUpdate;
        private bool _IsDelete;

        public int UserID { get => _UserID; set => _UserID = value; }
        public int RoleID { get => _RoleID; set => _RoleID = value; }
        public string Role { get => _Role; set => _Role = value; }
        public bool IsView { get => _IsView; set => _IsView = value; }
        public bool IsUpdate { get => _IsUpdate; set => _IsUpdate = value; }
        public bool IsDelete { get => _IsDelete; set => _IsDelete = value; }
        public int WarehouseID { get => _WarehouseID; set => _WarehouseID = value; }
    }
}
