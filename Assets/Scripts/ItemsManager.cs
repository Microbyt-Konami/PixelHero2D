using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemsManager : MonoBehaviour, ISerializable
{
    // Variables
    private PlayerExtrasTracker playerExtrasTracker;
    private Dictionary<string, ItemType> items = new Dictionary<string, ItemType>();
    private int orderCurrent = 1;

    public List<string> GameObjectNamesItemsCatched { get; } = new List<string>();
    public ICollection<ItemType> Items => items.Values;

    private class Prefs
    {
        public int CoinShineCatched;
        public int CoinSpinCatched;
        public int PickHeartCatched;
        public int OrderCurrent;
        public bool CanDoubleJump;
        public bool CanDash;
        public bool CanEnterBallMode;
        public bool CanDropBombs;

        public Prefs(ItemsManager itemManager)
        {
            foreach (var item in itemManager.items)
            {
                if (item.Key == "CoinShine")
                    CoinShineCatched = item.Value.ItemsCatched;
                else if (item.Key == "CoinSpin")
                    CoinSpinCatched = item.Value.ItemsCatched;
                else if (item.Key == "PickHeart")
                    PickHeartCatched = item.Value.ItemsCatched;
            }
            OrderCurrent = itemManager.orderCurrent;
            CanDoubleJump = itemManager.playerExtrasTracker.CanDoubleJump;
            CanDash = itemManager.playerExtrasTracker.CanDash;
            CanEnterBallMode = itemManager.playerExtrasTracker.CanEnterBallMode;
            CanDropBombs = itemManager.playerExtrasTracker.CanDropBombs;
        }

        public void Restore(ItemsManager itemsManager)
        {
            itemsManager.items["CoinShine"].ItemsCatched = CoinShineCatched;
            itemsManager.items["CoinSpin"].ItemsCatched = CoinSpinCatched;
            itemsManager.items["PickHeart"].ItemsCatched = PickHeartCatched;
            itemsManager.orderCurrent = OrderCurrent;
            itemsManager.playerExtrasTracker.CanDoubleJump = CanDoubleJump;
            itemsManager.playerExtrasTracker.CanDash = CanDash;
            itemsManager.playerExtrasTracker.CanEnterBallMode = CanEnterBallMode;
            itemsManager.playerExtrasTracker.CanDropBombs = CanDropBombs;
        }
    }

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
        FindAnyObjectByType<SaveDataGame>().ObjectsToSerialize.Add(this);
    }

    public bool CatchIt(ItemController controller)
    {
        if (!items.TryGetValue(controller.tag, out var item))
            return false;

        item.CatchIt();
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

    public JObject Serialize()
    {
        var prefs = new Prefs(this);
        string jsonString = JsonUtility.ToJson(prefs);
        JObject returnObject = JObject.Parse(jsonString);

        return returnObject;
    }

    public void DeSerialized(string jsonString)
    {
        var prefs = JsonUtility.FromJson<Prefs>(jsonString);

        prefs.Restore(this);
    }

    public string GetJsonKey() => "Items";
}
