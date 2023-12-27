using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private ItemsManager itemsManager;
    private GUIStyle myStyle = new GUIStyle();

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
            GUILayout.Label($"{item.tag}: {item.ItemsPendingToUnlock}", myStyle);
        GUILayout.EndVertical();
    }
}
