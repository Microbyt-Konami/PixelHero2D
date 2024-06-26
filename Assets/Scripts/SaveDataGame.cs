using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;
using System;
using System.Text;

public class SaveDataGame : MonoBehaviour
{
    byte[] key = { 92, 149, 74, 167, 16, 8, 60, 7, 187, 86, 156, 215, 179, 194, 146, 89, 99, 103, 181, 126, 196, 223, 67, 218, 120, 47, 234, 144, 64, 133, 235, 141 };
    byte[] iVector = { 245, 94, 116, 78, 187, 235, 49, 7, 213, 231, 91, 248, 26, 63, 174, 12 };

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

        /*
        AesManaged aesManaged = new AesManaged();

        aesManaged.GenerateKey();
        aesManaged.GenerateIV();

        print("Random key:");
        PrintByteArray(aesManaged.Key);

        Console.WriteLine("\nRandom iVector:");
        PrintByteArray(aesManaged.IV);

        void PrintByteArray(byte[] byteArray)
        {
            var sb = new StringBuilder();
            foreach (byte b in byteArray)
            {
                sb.Append(b);
                sb.Append(',');
            }

            print(sb.ToString());
        }
        */
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        // Nos aseguraramos de que los objetos se ha cargado correctamente y por tanto todos los objetos que implementen ISerializable se han cargado

        bool isLoadData;
        try
        {
            isLoadData = LoadData();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            isLoadData = false;
        }
        if (!isLoadData)
            ResetData();
    }

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

        //Debug.Log(filePath + "\n" + jDataSave);
        Debug.Log(filePath);

        // StreamWriter sw = new StreamWriter(filePath);
        // sw.WriteLine(jDataSave.ToString());
        // sw.Close();

        byte[] encryptedSaveGame = Encrypt(jDataSave.ToString());
        File.WriteAllBytes(filePath, encryptedSaveGame);
    }

    bool LoadData()
    {
        string filePath = Application.persistentDataPath + "/saveData.sdg";

        if (!File.Exists(filePath))
            return false;

        // StreamReader sr = new StreamReader(filePath);
        // string jsonString = sr.ReadToEnd();

        // sr.Close();

        byte[] decryptedSaveGame = File.ReadAllBytes(filePath);
        string jsonString = Decrypt(decryptedSaveGame);

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

    byte[] Encrypt(string plainText)
    {
        AesManaged aesManaged = new AesManaged();
        ICryptoTransform cryptoTransform = aesManaged.CreateEncryptor(key, iVector);
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
        StreamWriter streamWriter = new StreamWriter(cryptoStream);
        streamWriter.Write(plainText);

        streamWriter.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return memoryStream.ToArray();
    }

    string Decrypt(byte[] encryptedText)
    {
        AesManaged aesManaged = new AesManaged();
        ICryptoTransform decryptTransform = aesManaged.CreateDecryptor(key, iVector);
        MemoryStream memoryStream = new MemoryStream(encryptedText);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptTransform, CryptoStreamMode.Read);
        StreamReader streamReader = new StreamReader(cryptoStream);

        string decryptedPlainText = streamReader.ReadToEnd();
        streamReader.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return decryptedPlainText;
    }
}
