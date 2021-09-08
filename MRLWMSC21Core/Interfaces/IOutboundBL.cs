using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;

namespace MRLWMSC21Core.Business.Interfaces
{
    interface IOutboundBL
    {

        /* BEGIN: Methods for Picking */

        /// <summary>
        /// Used to retrieve the list of Picklists available for picking.
        /// </summary>
        /// <param name="oCriteria">Search Criteria object to identify the Logged In User.</param>
        /// <returns>List of Picklists.</returns>
        List<Picklist> GetPicklistsForPicking(SearchCriteria oCriteria);

        /// <summary>
        /// Fetch Inventory List for a PickList.
        /// </summary>
        /// <param name="oPicklist">PickList object for Picklist reference.</param>
        /// <returns>List of type PickListInventory</returns>
        List<PickListInventory> FetchPicklistInventory(Picklist oPicklist, SearchCriteria oCriteria);

        /// <summary>
        /// Fetch the next item from the picklist for picking.
        /// </summary>
        /// <param name="oPicklist"></param>
        /// <returns></returns>
        PickListInventory GetNextPicklistItemForPicking(Picklist oPicklist, SearchCriteria oCriteria);

        /// <summary>
        /// Validates the location at picking scan.
        /// </summary>
        /// <param name="oLocation">Location Object</param>
        /// <returns>Boolean value identifying if the Location is a valid location.</returns>
        bool ValidateLocationAtPicking(Location oLocation);

        /// <summary>
        /// Validates the Pallet Scanned at picking.
        /// </summary>
        /// <param name="oPallet">Pallet Object identyfind Pallet Code or Pallet ID</param>
        /// <returns>Boolean Value identifying if the Pallet exists.</returns>
        bool ValidatePalletAtPicking(Pallet oPallet);

        /// <summary>
        /// Pick Inventory from a Location.
        /// </summary>
        /// <param name="oInventory">Inventory object identifying the material being picked.</param>
        /// <returns>Returns a boolean value confirming material Pick.</returns>
        Inventory PickInventory(Inventory oInventory, out bool IsOldMRP, string NewRSNForPartialPicking = null);

        /// <summary>
        /// Flags the bin for a cycle count and regenerates the Picking Suggestion.
        /// </summary>
        /// <param name="oInventory"> Inventory Object of the Inventory to be picked. </param>
        /// <returns>Object of type PickListInventory.</returns>
       PickListInventory MarkMaterialNotFound(PickListInventory oInventory);

        PickListInventory MarkMaterialDamaged(Inventory oInventory, string SLoc);

        /* END: Methods for Picking */

        PickingPreferences FetchPickingPreferences(SearchCriteria oCriteria);

        /* BEGIN: Methods for Loading */

            // DONE
        List<LoadSheet> FetchLoadSheetList(SearchCriteria oCriteria);


        // DONE
        List<LoadItem> FetchInventoryForLoadSheet(LoadSheet oLoadSheet);

        // NOT REQUIRED
        Inventory FetchNextInventoryItemForLoadSheet(LoadSheet oLoadSheet);


        // DONE
        Inventory ConfirmLoading(Inventory oInventory);


        bool LoadingComplete(LoadSheet oLoadSheet);


        Inventory RevertLoading(Inventory oInventory);



        /* END: Methods for Loading */

    }

}
