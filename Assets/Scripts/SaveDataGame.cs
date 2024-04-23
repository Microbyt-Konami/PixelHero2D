using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;

public class SaveDataGame : MonoBehaviour
{
    private List<ISerializable> _objectsToSerialize;
    private List<string> itemsGOCatched = new List<string>();

    public List<ISerializable> ObjectsToSerialize => _objectsToSerialize;
    public List<string> ItemsGOCatched => itemsGOCatched;

    public void ResetData()
    {
        string filePath = Application.persistentDataPath + "/saveData.sdg";

        if (File.Exists(filePath))
            File.Delete(filePath);
        itemsGOCatched.Clear();
    }

    void Awake()
    {
        _objectsToSerialize = new List<ISerializable>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        // Asegurarse de que los objetos se ha cargado correctamente y por tanto todos los objetos que implementen ISerializable se han cargado
        if (!LoadData())
            ResetData();
    }

    // Update is called once per frame
    // void Update()
    // {

    // }

    void OnApplicationQuit()
    {
        SaveData();
    }

    void SaveData()
    {
        string filePath = Application.persistentDataPath + "/saveData.sdg";
        JObject jDataSave = new JObject();

        jDataSave.Add("ItemsCatched", JArray.FromObject(itemsGOCatched));
        foreach (var item in _objectsToSerialize)
            jDataSave.Add(item.GetJsonKey(), item.Serialize());

        StreamWriter sw = new StreamWriter(filePath);
        Debug.Log(filePath + "\n" + jDataSave);

        sw.WriteLine(jDataSave.ToString());
        sw.Close();
    }

    bool LoadData()
    {
        string filePath = Application.persistentDataPath + "/saveData.sdg";

        if (!File.Exists(filePath))
            return false;

        StreamReader sr = new StreamReader(filePath);
        string jsonString = sr.ReadToEnd();

        sr.Close();

        JObject jDataSave = JObject.Parse(jsonString);
        JArray itemsCatched = (JArray)jDataSave["ItemsCatched"];

        if (itemsCatched == null || itemsCatched.Count == 0)
            return false;

        itemsGOCatched = itemsCatched.ToObject<List<string>>();
        foreach (var item in itemsGOCatched)
        {
            var go = GameObject.Find(item);

            if (go != null)
                Destroy(go);
        }

        foreach (var item in _objectsToSerialize)
        {
            string jsonItem = jDataSave[item.GetJsonKey()].ToString();

            item.DeSerialized(jsonItem);
        }

        return true;
    }
}
