using System;
using SketchKnights.Scripts.Controller;
using UnityEngine;

public static class Utils
{
    public static void ApplyLayerToChildren(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            ApplyLayerToChildren(child.gameObject, layer);
        }
    }
    
    public static string WeaponStyleToTag(WeaponStyle style){
        switch (style)
        {
            case WeaponStyle.Guard:
                return "Guard";
            case WeaponStyle.Sword:
                return "Sword";
            default:
                throw new Exception("WeaponStyle:"+style+" not supported");
        }
    }
    
    public static readonly int SelfLayer = LayerMask.NameToLayer("Self");
    public static readonly int OtherLayer = LayerMask.NameToLayer("Other");
}