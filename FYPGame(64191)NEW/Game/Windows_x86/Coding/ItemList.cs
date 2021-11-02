using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item List")]
public class ItemList : ScriptableObject
{
    [SerializeField]
    public List<ItemDetails> itemDetails;
}
