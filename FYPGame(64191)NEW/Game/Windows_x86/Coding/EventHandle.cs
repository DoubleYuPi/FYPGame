using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandle
{
    public static event Action DropSelectedItemEvent;

    public static void CallDropSelectedItemEvent()
    {
        if(DropSelectedItemEvent != null)
        {
            DropSelectedItemEvent();
        }
    }

    public static event Action RemoveSelectedItemFromIinventory;

    public static void CallRemoveSelectedItemFromInventory()
    {
        if(RemoveSelectedItemFromIinventory != null)
        {
            RemoveSelectedItemFromIinventory();
        }
    }

   public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;
   
    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation,List<InventoryItem> inventoryList)
    {
        if (InventoryUpdatedEvent != null)
        {
            InventoryUpdatedEvent(inventoryLocation, inventoryList);
        }
    }

    public static event Action BeforeSceneFadeOut;

    public static void CallBeforeSceneFadeOut()
    {
        if (BeforeSceneFadeOut != null)
        {
            BeforeSceneFadeOut();
        }
    }

    public static event Action BeforeSceneUnload;

    public static void CallBeforeSceneUnload()
    {
        if (BeforeSceneUnload != null)
        {
            BeforeSceneUnload();
        }
    }

    public static event Action AfterSceneLoadEvent;

    public static void CallAfterSceneLoadEvent()
    {
        if (AfterSceneLoadEvent != null)
        {
            AfterSceneLoadEvent();
        }
    }

    public static event Action AfterSceneFadeIn;

    public static void CallAfterSceneFadeIn()
    {
        if (AfterSceneFadeIn != null)
        {
            AfterSceneFadeIn();
        }
    }

    // Advance game minute
    public static event Action<int, int, int> AdvanceGameMinuteEvent;

    public static void CallAdvanceGameMinuteEvent(int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameMinuteEvent != null)
        {
            AdvanceGameMinuteEvent(gameHour, gameMinute, gameSecond);
        }
    }

    // Advance game hour
    public static event Action<int, int, int> AdvanceGameHourEvent;

    public static void CallAdvanceGameHourEvent(int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameHourEvent != null)
        {
            AdvanceGameHourEvent(gameHour, gameMinute, gameSecond);
        }
    }

    // Advance game day
    public static event Action<int, int, int> AdvanceGameDayEvent;

    public static void CallAdvanceGameDayEvent(int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameDayEvent != null)
        {
            AdvanceGameDayEvent(gameHour, gameMinute, gameSecond);
        }
    }
}
