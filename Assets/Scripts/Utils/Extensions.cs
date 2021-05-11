using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public static class Extensions
{
    public static void BindWithInrefaces<T>(this DiContainer container)
    {
        container.BindInterfacesAndSelfTo<T>().AsSingle().NonLazy();
    }

    public static void Log(this object obj, object message)
    {
        Debug.Log($"{obj.GetType().Name}: {message}");
    }

    public static void LogError(this object obj, object message)
    {
        Debug.LogError($"{obj.GetType().Name}: {message}");
    }

    public static T NextAfter<T>(this IEnumerable<T> source, T item, Func<T, T, bool> comparer) where T : class
    {
        IEnumerator<T> enumerator = source.GetEnumerator();

        T firstItem = null;

        while (enumerator.MoveNext())
        {
            if (comparer(enumerator.Current, item))
            {
                if (enumerator.MoveNext())
                    return enumerator.Current;

                return firstItem;
            }

            if (firstItem == null)
                firstItem = enumerator.Current;
        }

        return null;
    }
}
