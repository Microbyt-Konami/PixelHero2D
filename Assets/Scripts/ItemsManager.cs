using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    // Variables
    private PlayerExtrasTracker playerExtrasTracker;
    private Dictionary<string, ItemType> items = new Dictionary<string, ItemType>();
    private int orderCurrent = 1;
    
    
    public List<string> GameObjectNamesItemsCatched {get;} = new List<string>();
    public ICollection<ItemType> Items => items.Values;

    void Start()
    {
        playerExtrasTracker = FindAnyObjectByType<PlayerExtrasTracker>();
        orderCurrent = 1;
        if (items.Count > 0)
            items.Clear();

        var types = transform.Find("Types");

        for (var i = 0; i < types.childCount; i++)
        {
            var itemChild = types.GetChild(i);
            var itemType = itemChild.GetComponent<ItemType>();

            itemType.ResetItemsPendingToUnlock();
            items.Add(itemChild.tag, itemType);
        }
        //FindAnyObjectByType<SaveDataGame>().ObjectsToSerialize.Add(this);
    }

    public bool CatchIt(ItemController controller, bool addInList = false)
    {
        if (!items.TryGetValue(controller.tag, out var item))
            return false;

        item.CatchIt();
        if (addInList)
            GameObjectNamesItemsCatched.Add(controller.gameObject.name);
        if (orderCurrent == item.OrderToUnlock)
        {
            if (item.ItemsPendingToUnlock == 0)
            {
                orderCurrent++;
                if (item.CanDoubleJump)
                    playerExtrasTracker.CanDoubleJump = true;
                if (item.CanDash)
                    playerExtrasTracker.CanDash = true;
                if (item.CanEnterBallMode)
                    playerExtrasTracker.CanEnterBallMode = true;
                if (item.CanDropBombs)
                    playerExtrasTracker.CanDropBombs = true;
            }
        }

        return true;
    }
}
