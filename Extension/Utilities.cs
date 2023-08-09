using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utilities
{
    private static float gravity = 10f;
    /// <summary>
    /// (floor, ceiling) -> (min, max)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="floor"></param>
    /// <param name="ceiling"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float GetValue(float value, float floor, float ceiling, float min, float max)
    {
        return (ceiling == floor) ? 0 : (value - floor) * (max - min) / (ceiling - floor) + min;
    }

    public static double Lerp(double a, double b, float t) => a + (b - a) * Mathf.Clamp01(t);
    public static float Lerp(float a, float b, float t) => a + (b - a) * Mathf.Clamp01(t);

    //tinh toan ve parabol theo thoi gian
    public static Vector2 CalculateLinePoint(Vector2 throwPoint, Vector2 force, float wind, float t)
    {
        float x = (force.x * t) + (wind * t * t * 0.5f);
        float y = (force.y * t) - (gravity * t * t * 0.5f);
        return new Vector2(x + throwPoint.x, y + throwPoint.y);
    }

    //tinh toan ty le velocity de ban toi 1 diem cho trc
    public static float CaculateRatioVelocity(Vector2 throwPoint, Vector2 force, Vector2 targetPoint, float wind)
    {
        float dx = targetPoint.x - throwPoint.x;
        float dy = targetPoint.y - throwPoint.y;

        //return Mathf.Sqrt((gravity * dx * dx) / (2 * force.x * ( dx * force.y + dy * force.x)));

        //float t = Mathf.Sqrt(2 * (dx * force.y - dy * force.x) / (wind * force.y + gravity * force.x));
        float t = Mathf.Sqrt(2 * (dx * force.y - dy * force.x) / (wind * force.y + gravity * force.x));

        float vx = (dx - wind * t * t * 0.5f) / t;
        //float vy = (dy + gravity * t * t * 0.5f) / t;

        //float x = vx * t + 0.5f * wind * t * t + throwPoint.x;
        //float y = vy * t - 0.5f * gravity * t * t + throwPoint.y;

        return vx / force.x;
    }

    public static float TimeToTarget(Vector2 throwPoint, Vector2 force, Vector2 targetPoint, float wind)
    {
        float dx = targetPoint.x - throwPoint.x;
        float dy = targetPoint.y - throwPoint.y;

        float t = Mathf.Sqrt(2 * (dx * force.y - dy * force.x) / (wind * force.y + gravity * force.x));

        return t;
    }


    //ep kieu string sang enum
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    //random thu tu mot list
    public static List<T> SortOrder<T>(List<T> list, int amount)
    {
        return list.OrderBy(d => System.Guid.NewGuid()).Take(amount).ToList();
    }

    //lay ket qua theo ty le xac suat
    public static bool Chance(float rand, float max = 100)
    {
        return UnityEngine.Random.Range(0, max) < rand;
    }

    //random gia enum trong mot kieu enum
    private static System.Random random = new System.Random();
    public static T RandomEnumValue<T>() where T : Enum
    {
        var v = System.Enum.GetValues(typeof(T));
        return (T)v.GetValue(random.Next(v.Length));
    }

    //lay so luong cac phan tu trong enum
    public static int GetEnumCount<T>() where T : Enum
    {
        return Enum.GetNames(typeof(T)).Length;
    }

    //dau vao 1 array, tra ve 1 phan tu duoc random
    public static T TakeRandom<T>(params T[] ts)
    {
        return ts[UnityEngine.Random.Range(0, ts.Length)];
    }
    
    //lay phan tu cuoi trong mang hoac list
    public static T Last<T>(this T[] ts) => ts[^1]; 
    //-> cai nay kieu nhu dich bit nguoc tu cuoi ve dau 1 phan tu
    public static T Last<T>(this List<T> ts) => ts[^1];

    //random 1 phan tu trong array
    public static T RandomItem<T>(this T[] ts) => ts[UnityEngine.Random.Range(0, ts.Length)];
    public static T RandomItem<T>(this List<T> ts) => ts[UnityEngine.Random.Range(0, ts.Count)];

    //lay vi tri tuong doi trong hang
    //index = 0 -> max - 1
    public static Vector3 GetPositionInRow(int index, int max, Vector3 midPoint, Vector3 space)
    {
        float firstIndex = ((float)max - 1) * 0.5f;
        return midPoint + space * (index - firstIndex);
    }


    //save lai data prefab, scriptable object
    public static void SaveAssetEditor(UnityEngine.Object go)
    {
#if UNITY_EDITOR

        UnityEditor.Undo.RegisterCompleteObjectUndo(go, "Save level data");
        UnityEditor.EditorUtility.SetDirty(go);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
