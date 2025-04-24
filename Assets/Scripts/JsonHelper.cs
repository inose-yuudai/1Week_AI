using UnityEngine;


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string wrapped = "{\"items\":" + json + "}";
        return JsonUtility.FromJson<Wrapper<T>>(wrapped).items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}
