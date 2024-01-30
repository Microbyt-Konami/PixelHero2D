using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UIManager : MonoBehaviour
{
    private ItemsManager itemsManager;
    private GUIStyle myStyle = new GUIStyle();
    private Dictionary<string, string> typeItemsTexts = new Dictionary<string, string>();

    public void SetStringEntry(string typeItem, string variableAndEntry)
    {
        typeItemsTexts[typeItem] = variableAndEntry;
    }

    public void SetCoinSpinStringEntry(string variableAndEntry) => SetStringEntry("CoinSpin", variableAndEntry);
    public void SetCoinShineStringEntry(string variableAndEntry) => SetStringEntry("CoinShine", variableAndEntry);
    public void SetPickHeartStringEntry(string variableAndEntry) => SetStringEntry("PickHeart", variableAndEntry);

    // Start is called before the first frame update
    void Start()
    {
        itemsManager = FindAnyObjectByType<ItemsManager>();
        myStyle.fontSize = 40;
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.normal.textColor = Color.white;
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        foreach (var item in itemsManager.Items)
            GUILayout.Label($"{typeItemsTexts.GetValueOrDefault(item.tag, item.tag)}: {item.ItemsPendingToUnlock}", myStyle);
        GUILayout.EndVertical();
    }
}
