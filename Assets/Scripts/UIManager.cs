using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private ItemsManager itemsManager;
    private SaveDataGame saveDataGame;

    private GUIStyle myStyle;
    private GUIStyle myStyleButton;
    private Dictionary<string, string> typeItemsTexts = new Dictionary<string, string>();

    public void SetStringEntry(string typeItem, string variableAndEntry)
    {
        typeItemsTexts[typeItem] = variableAndEntry;
    }

    public void SetCoinSpinStringEntry(string variableAndEntry) => SetStringEntry("CoinSpin", variableAndEntry);
    public void SetCoinShineStringEntry(string variableAndEntry) => SetStringEntry("CoinShine", variableAndEntry);
    public void SetPickHeartStringEntry(string variableAndEntry) => SetStringEntry("PickHeart", variableAndEntry);

    void Awake()
    {
        SetUpGUIStyles();
    }

    // Start is called before the first frame update
    void Start()
    {
        itemsManager = FindAnyObjectByType<ItemsManager>();
        saveDataGame = FindAnyObjectByType<SaveDataGame>();
    }

    private void OnGUI()
    {
        // if (myStyle == null || myStyleButton == null)
        //     SetUpGUIStyles();

        GUILayout.BeginVertical();
        foreach (var item in itemsManager.Items)
            GUILayout.Label($"{item.tag}: {item.ItemsPendingToUnlock}", myStyle);
        GUILayout.EndVertical();

        if (GUI.Button(new Rect(Screen.width - 295, 0, 295, 70), "RESET GAME", myStyleButton))
        {
            saveDataGame.ResetData();
            SceneManager.LoadScene(0);
        }
    }

    private void SetUpGUIStyles()
    {
        myStyle = new GUIStyle();
        myStyle.fontSize = 40;
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.normal.textColor = Color.white;

        myStyleButton = new GUIStyle();
        myStyleButton.fontSize = 40;
        myStyleButton.fontStyle = FontStyle.Bold;
        myStyleButton.normal.textColor = Color.white;
    }

}
