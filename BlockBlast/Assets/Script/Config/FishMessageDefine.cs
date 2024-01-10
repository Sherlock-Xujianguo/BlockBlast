
public static class FishMessageDefine
{
    public static readonly string OnDragBlock = "OnDragBlock";
    public static readonly string OnReleaseBlock = "OnReleaseBlock";
    public static readonly string OnPlaceBlock = "OnPlaceBlock";
    public static readonly string OnScoreUpdate = "OnScoreUpdate";
}

public struct OnDragBlockMessageData
{
    public BaseBlockComp BlockComp;
}

public struct OnReleaseBlockMessageData
{
    public BaseBlockComp BlockComp;
}

public struct OnPlaceBlockMessageData
{
    public BaseBlockComp BlockComp;
}

public struct OnScoreUpdateMessageData
{
    public long old_score;
    public long new_score;
}