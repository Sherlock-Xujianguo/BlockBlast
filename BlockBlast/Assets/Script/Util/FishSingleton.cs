using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSingleton<T> : FishSimpleClass where T : new()
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
