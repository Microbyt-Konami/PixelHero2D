using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    // Variables
    private PlayerExtrasTracker playerExtrasTracker;
    private Dictionary<string, ItemType> items = new Dictionary<string, ItemType>();
    private int orderCurrent = 1;

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
    }

    public void CatchIt(ItemController controller)
    {
        if (!items.TryGetValue(controller.tag, out var item))
            return;

        if (orderCurrent == item.OrderToUnlock)
        {
            item.CatchIt();
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
    }
}
