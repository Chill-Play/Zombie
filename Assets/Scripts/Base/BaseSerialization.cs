using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Reflection;


public static class BaseSerialization
{

    public static string SerializeBase(BaseObject[] baseObjects)
    {
        var json = new JSONObject();
        for(int i = 0; i < baseObjects.Length; i++)
        {
            var objectNode = new JSONObject();
            CreateJSONNodes(baseObjects[i], objectNode);
            json.Add(baseObjects[i].Id, objectNode);
        }


        return json.ToString();
    }

    public static void CreateJSONNodes(BaseObject baseObject, JSONNode parentNode)
    {
        var vars = GetFields(baseObject);
        foreach (var v in vars)
        {
            var type = v.FieldType;
            if (type == typeof(float)) //YandereDev, no switch for types
            {
                parentNode.Add(v.Name, (float)v.GetValue(baseObject));
            }
            else if (type == typeof(int))
            {
                parentNode.Add(v.Name, (int)v.GetValue(baseObject));
            }
            else if (type == typeof(string))
            {
                parentNode.Add(v.Name, (string)v.GetValue(baseObject));
            }
            else if (type == typeof(bool))
            {
                parentNode.Add(v.Name, (bool)v.GetValue(baseObject));
            }
            else if(type == typeof(ResourcesInfo))
            {
                var value = (ResourcesInfo)v.GetValue(baseObject);
                var jsonObject = new JSONObject();
                foreach(var slot in value.Slots)
                {
                    jsonObject.Add(slot.type.saveId, slot.count);
                }
                parentNode.Add(v.Name, jsonObject);
            }
            else
            {
                Debug.LogError("Type is not supported for serialization : " + v.FieldType);
            }
        }
    }

    private static IEnumerable<FieldInfo> GetFields(BaseObject baseObject)
    {
        return baseObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(
                v => Attribute.IsDefined(v, typeof(BaseSerializeAttribute)));
    }

    public static void DeserializeBase(string json, BaseObject[] baseObjects)
    {

        var jsonObject = JSON.Parse(json);
        foreach(var obj in baseObjects)
        {
            if (jsonObject.HasKey(obj.Id))
            {
                var vars = GetFields(obj);
                foreach (var v in vars)
                {
                    if(jsonObject[obj.Id].HasKey(v.Name))
                    {
                        var node = jsonObject[obj.Id][v.Name];
                        var type = v.FieldType;
                        if (type == typeof(float)) //YandereDev, no switch for types
                        {
                            v.SetValue(obj, node.AsFloat);
                        }
                        else if (type == typeof(int))
                        {
                            v.SetValue(obj, node.AsInt);
                        }
                        else if (type == typeof(string))
                        {
                            v.SetValue(obj, node.ToString().Replace("\"", "")); //Find better way to handle ""
                        }
                        else if (type == typeof(bool))
                        {
                            v.SetValue(obj, node.AsBool);
                        }
                        else if (type == typeof(ResourcesInfo))
                        {
                            var info = new ResourcesInfo();
                            foreach(var n in node)
                            {
                                // var data = n.Value;
                                var t = ResourcesController.Instance.GetResourceType(n.Key);
                                info.AddSlot(t, n.Value.AsInt);
                                //v.SetValue(n.Value);
                            }
                            v.SetValue(obj, info);
                        }
                    }
                }
            }
            obj.BaseAfterDeserialize();
        }
    }
}
