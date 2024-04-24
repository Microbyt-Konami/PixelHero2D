using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

public interface ISerializable
{
    JObject Serialize();
    void DeSerialized(string jsonString);
    string GetJsonKey();
}