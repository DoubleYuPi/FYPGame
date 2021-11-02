﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ItemsCodeDescAtr))]

public class ItemCodeDescDraw : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.BeginChangeCheck();

            var newValue = EditorGUI.IntField(new Rect(position.x, position.y,position.width,position.height/2),label,property.intValue);

            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Name", GetItemDescription(property.intValue));

            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }
        }


        EditorGUI.EndProperty();
    }

    private string GetItemDescription(int itemCode)
    {
        ItemList itemList;

        itemList = AssetDatabase.LoadAssetAtPath("Assets/Scriptable Object Aset/Item/New Item List.asset", typeof(ItemList)) as ItemList;

        List<ItemDetails> itemDetails = itemList.itemDetails;

        ItemDetails itemDetail = itemDetails.Find(x => x.itemCode == itemCode);

        if (itemDetail != null)
        {
            return itemDetail.itemName;
        }
        else
        {
            return "";
        }
    }
}
