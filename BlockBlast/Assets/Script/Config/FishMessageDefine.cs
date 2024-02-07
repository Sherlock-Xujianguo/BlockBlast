
public static class FishMessageDefine
{
    public static readonly string OnDragBlock = "OnDragBlock";
    public static readonly string OnReleaseBlock = "OnReleaseBlock";
    public static readonly string OnPlaceBlock = "OnPlaceBlock";
    public static readonly string OnScoreUpdate = "OnScoreUpdate";
    public static readonly string OnComboUpdate = "OnComboUpdate";
    public static readonly string OnBlastBlock = "OnBlastBlock";
    public static readonly string FinishPlaceBlock = "FinishPlaceBlock";
    public static readonly string Restart = "Restart";
    
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

public struct OnBlastBlockMessageData
{
    public int TotalBlastRowAndColumn;
    public int Row;
    public int Column;
}

public struct FinishPlaceBlockMessageData
{

}

public struct OnComboUpdateMessageData
{
    public int ComboCount;
}