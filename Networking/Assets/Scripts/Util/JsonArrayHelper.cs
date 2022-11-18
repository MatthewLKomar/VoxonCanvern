using System;
using UnityEngine;

public static class JsonArrayHelper
{
    public static T FromJson<T>(string json)
    {
        // serialized Wrapper object will have a root property Items to contain array
        //Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"Items\":" + json + "}");
        //Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return JsonUtility.FromJson<T>(json);
    }

    public static string ToJson<T>(T data)
    {
        return JsonUtility.ToJson(data);
    }

    public static string ToJson<T>(T array, bool prettyPrint)
    {
        return JsonUtility.ToJson(array, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
