using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FishTimeUtil
{
    public static int GetDay()
    {
        DateTime utcNow = DateTime.UtcNow;
        return utcNow.Day;
    }
}
