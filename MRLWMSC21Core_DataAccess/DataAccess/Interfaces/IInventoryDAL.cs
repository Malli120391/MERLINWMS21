using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;

namespace MRLWMSC21Core.DataAccess.Interfaces
{
    interface IInventoryDAL
    {

        Inventory ReceiveInventory(Inventory oInventory);

        List<Inventory> ReceiveInventory(List<Inventory> lInventory);

        GoodsMovement MoveInventory(GoodsMovement oGoodsMovement);

        List<GoodsMovement> MoveInventory(List<GoodsMovement> lGoodsMovement);

        List<Inventory> GetActiveStock(SearchCriteria oCriteria);

        List<Inventory> UpdateInventory(List<Inventory> oInventory);


        Inventory ReceiveExcessInventory(Inventory oInventory);


        GoodsMovement ChangeMRP(GoodsMovement oMovement);



    }
}
