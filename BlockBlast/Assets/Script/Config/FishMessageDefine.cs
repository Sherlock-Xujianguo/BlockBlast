using System.Collections;
using UnityEngine;

public static class FishMessageDefine
{
    public static readonly string OnDragBlock = "OnDragBlock";
}

public struct OnDragBlockMessageData
{
    public BaseBlockComp BlockComp;
}