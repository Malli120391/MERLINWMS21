using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class Vehicle
    {
        private int _VehicleID;

        private string _VehicleNumber;

        private List<User> _AssignedUsers;

        private decimal _DocumentQuantity;

        private decimal _ReceivedQuantity;

        private List<Inbound> _InboundList;

        private decimal _VehicleReceivedQuantity;

        private decimal _VehicleInventoryQuantity;

        public int VehicleID { get => _VehicleID; set => _VehicleID = value; }
        public string VehicleNumber { get => _VehicleNumber; set => _VehicleNumber = value; }
        public List<User> AssignedUsers { get => _AssignedUsers; set => _AssignedUsers = value; }
        public decimal DocumentQuantity { get => _DocumentQuantity; set => _DocumentQuantity = value; }
        public decimal ReceivedQuantity { get => _ReceivedQuantity; set => _ReceivedQuantity = value; }
        public List<Inbound> InboundList { get => _InboundList; set => _InboundList = value; }
        public decimal VehicleReceivedQuantity { get => _VehicleReceivedQuantity; set => _VehicleReceivedQuantity = value; }
        public decimal VehicleInventoryQuantity { get => _VehicleInventoryQuantity; set => _VehicleInventoryQuantity = value; }
    }
}
