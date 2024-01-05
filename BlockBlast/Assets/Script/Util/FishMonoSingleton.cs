using System.Collections;
using UnityEngine;


public class FishMonoSingleton<T> : FishMonoClass where T : new()
{
    private static T instance;

    public static T GetInstnace
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

}