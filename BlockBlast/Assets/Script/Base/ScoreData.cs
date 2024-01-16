

using UnityEngine;

public class ScoreData : FishSingleton<ScoreData>, IInitable
{

    long CurrentCheckerBoardScore = 0;
    long ScoreBuffer = 0;

    int ComboCount = 0;
    int MissCount = 0;
    static readonly int TolerateMissCount = 3;


    public void Init()
    {
        RegisterMessage<OnPlaceBlockMessageData>(FishMessageDefine.OnPlaceBlock, OnPlaceBlock);
        RegisterMessage<OnScoreUpdateMessageData>(FishMessageDefine.OnScoreUpdate, OnScoreUpdate);
        RegisterMessage<OnBlastBlockMessageData>(FishMessageDefine.OnBlastBlock, OnBlastBlock);
        RegisterMessage<FinishPlaceBlockMessageData>(FishMessageDefine.FinishPlaceBlock, FinishPlaceBlock);
    }

    void OnPlaceBlock(OnPlaceBlockMessageData PlaceData)
    {
        if (PlaceData.BlockComp == null || PlaceData.BlockComp.BlockData == null)
        {
            return;
        }

        ScoreBuffer += PlaceData.BlockComp.BlockData.Size;

        MissCount++;
    }

    void OnBlastBlock(OnBlastBlockMessageData BlastData)
    {
        MissCount = 0;
        ComboCount++;

        OnComboUpdateMessageData onComboUpdateMessageData = new OnComboUpdateMessageData();
        onComboUpdateMessageData.ComboCount = ComboCount;
        SendMessage(FishMessageDefine.OnComboUpdate, onComboUpdateMessageData);

        int totalBlastRAndC = BlastData.TotalBlastRowAndColumn;
        int BlastRate = totalBlastRAndC * (totalBlastRAndC - 1);
        int BlastScore = 0;
        if (BlastRate <= 0)
        {
            BlastRate = 1;
        }
        BlastScore = 10 * BlastRate;

        if (ComboCount > 0)
        {
            ScoreBuffer += ComboCount * BlastScore;
        }

        ScoreBuffer += BlastScore;
    }

    void FinishPlaceBlock(FinishPlaceBlockMessageData FinishPlaceData)
    {
        if (MissCount >= TolerateMissCount)
        {
            if (ComboCount > 0)
            {
                OnComboUpdateMessageData onComboUpdateMessageData = new OnComboUpdateMessageData();
                onComboUpdateMessageData.ComboCount = 0;
                SendMessage(FishMessageDefine.OnComboUpdate, onComboUpdateMessageData);
            }
            MissCount = 0;
            ComboCount = 0;
        }

        if (ScoreBuffer > 0)
        {
            OnScoreUpdateMessageData onScoreUpdateData = new OnScoreUpdateMessageData();
            onScoreUpdateData.old_score = CurrentCheckerBoardScore;
            onScoreUpdateData.new_score = CurrentCheckerBoardScore + ScoreBuffer;
            SendMessage(FishMessageDefine.OnScoreUpdate, onScoreUpdateData);
        }
        CurrentCheckerBoardScore += ScoreBuffer;
        ScoreBuffer = 0;
    }

    void OnScoreUpdate(OnScoreUpdateMessageData onScoreUpdateMessageData)
    {
        Debug.Log(string.Format("Score {0}", onScoreUpdateMessageData.new_score));
    }
}