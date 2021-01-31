using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionMethods
{
    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.ElementAt(UnityEngine.Random.Range(0, enumerable.Count()));
    }
    public static TValue RandomValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        return dictionary.Values.Random();
    }
    public static TKey RandomKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        return dictionary.Keys.Random();
    }

    public static Bounds GetBoundsWithChildren(this GameObject gameObject)
    {
        // GetComponentsInChildren() also returns components on gameobject which you call it on
        // you don't need to get component specially on gameObject
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        // If renderers.Length = 0, you'll get OutOfRangeException
        // or null when using Linq's FirstOrDefault() and try to get bounds of null
        Bounds bounds = renderers.Length > 0 ? renderers[0].bounds : new Bounds();

        // Or if you like using Linq
        // Bounds bounds = renderers.Length > 0 ? renderers.FirstOrDefault().bounds : new Bounds();

        // Start from 1 because we've already encapsulated renderers[0]
        for (int i = 1; i < renderers.Length; i++)
        {
            if (renderers[i].enabled)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
        }

        return bounds;
    }

    public static void FocusOn(this Camera camera, GameObject focusedObject, float marginPercentage)
    {
        Bounds bounds = focusedObject.GetBoundsWithChildren();
        float maxExtent = bounds.extents.magnitude;
        float minDistance = (maxExtent * marginPercentage) / Mathf.Sin(Mathf.Deg2Rad * camera.fieldOfView / 2f);
        camera.transform.position = focusedObject.transform.position - Vector3.forward * minDistance;
        camera.nearClipPlane = minDistance - maxExtent;
    }
}