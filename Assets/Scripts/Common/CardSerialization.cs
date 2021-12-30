using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Reflection;
using SimpleJSON;

public class CardSerialization : MonoBehaviour
{
    public static string SerializeCards(CardController cardController)
    {
        var json = new JSONObject();
        CreateJSONNodes(cardController, json);
        return json.ToString();
    }

    public static void CreateJSONNodes(CardController cardController, JSONNode parentNode)
    {
        var vars = GetFields(cardController);
       
        foreach (var v in vars)
        {
            var type = v.FieldType;
            if (type == typeof(CardsInfo))
            {
                var value = (CardsInfo)v.GetValue(cardController);
                var jsonObject = new JSONObject();
                for (int i = 0; i < value.Count; i++)
                {
                    var slotJsonObject = new JSONObject();
                    slotJsonObject.Add(value.cardSlots[i].card.Id.ToString(), value.cardSlots[i].level);
                    jsonObject.Add("slot_" + i.ToString(), slotJsonObject);
                }
                parentNode.Add(v.Name, jsonObject);
            }
        }
    }

    private static IEnumerable<FieldInfo> GetFields(CardController cardController)
    {
        return cardController.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(
                v => Attribute.IsDefined(v, typeof(CardSerializeAttribute)));
    }

    public static void DeserializeCards(string json, CardController cardController)
    {

        var jsonObject = JSON.Parse(json);
        var vars = GetFields(cardController);
        foreach (var v in vars)
        {
            if (jsonObject.HasKey(v.Name))
            {
                var node = jsonObject[v.Name];
                var type = v.FieldType;
                if (type == typeof(CardsInfo))
                {
                    var info = new CardsInfo();
                    info.cardSlots = new List<CardSlot>();                   
                    for (int i = 0; i < node.Count; i++)
                    {
                        var slotJson = node[i];
                        foreach (var s in slotJson)
                        {
                            var card = cardController.GetCardVariant(s.Key);
                            info.AddSlot(card, s.Value.AsInt);
                        }
                    }
                    v.SetValue(cardController, info);
                }
            }
        }
    }
}
