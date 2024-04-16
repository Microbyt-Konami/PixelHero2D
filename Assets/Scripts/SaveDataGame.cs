using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;

public class SaveDataGame : MonoBehaviour
{
    private List<ISerializable> _objectsToSerialize;

    public List<ISerializable> ObjectsToSerialize => _objectsToSerialize;

    public void ResetData()
    {
        string filePath = Application.persistentDataPath + "/saveData.sdg";

        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    void Awake()
    {
        _objectsToSerialize = new List<ISerializable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
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

        foreach (var item in _objectsToSerialize)
        {
            string jsonItem = jDataSave[item.GetJsonKey()].ToString();

            item.DeSerialized(jsonItem);
        }

        return true;
    }
}
