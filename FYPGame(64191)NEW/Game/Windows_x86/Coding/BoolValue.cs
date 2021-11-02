using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class BoolValue : ScriptableObject, ISerializationCallbackReceiver
{
    public bool initialValue;

    [NonSerialized]
    public bool runTimeVal;
    public void OnBeforeSerialize()
    {
        runTimeVal = initialValue;
    }

    public void OnAfterDeserialize()
    {

    }
}
