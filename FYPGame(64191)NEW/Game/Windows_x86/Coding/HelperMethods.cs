using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperMethods
{
    public static bool GetComponentsAtBoxLocation<T>(out List<T> listComponentsAtBoxPos, Vector2 point, Vector2 size, float angle)
    {
        bool found = false;
        List<T> componentList = new List<T>();

        Collider2D[] collider2DArr = Physics2D.OverlapBoxAll(point, size, angle);

        for (int i=0; i<collider2DArr.Length; i++)
        {
            T tComponent = collider2DArr[i].gameObject.GetComponentInParent<T>();
            if (tComponent != null)
            {
                found = true;
                componentList.Add(tComponent);
            }
            else
            {
                tComponent = collider2DArr[i].gameObject.GetComponentInChildren<T>();
                if (tComponent != null)
                {
                    found = true;
                    componentList.Add(tComponent);
                }
            }
        }

        listComponentsAtBoxPos = componentList;

        return found;
    }
}
