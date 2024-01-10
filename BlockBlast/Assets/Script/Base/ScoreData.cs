

using UnityEngine;

public class ScoreData : FishSingleton<ScoreData>, IInitable
{

    long CurrentCheckerBoardScore = 0;


    public void Init()
    {
        RegisterMessage<OnPlaceBlockMessageData>(FishMessageDefine.OnPlaceBlock, OnPlaceBlock);
        RegisterMessage<OnScoreUpdateMessageData>(FishMessageDefine.OnScoreUpdate, OnScoreUpdate);
    }

    void OnPlaceBlock(OnPlaceBlockMessageData PlaceData)
    {
        if (PlaceData.BlockComp == null || PlaceData.BlockComp.BlockData == null)
        {
            return;
        }

        long old_score = CurrentCheckerBoardScore;
        CurrentCheckerBoardScore += PlaceData.BlockComp.BlockData.Size;
        if (old_score != CurrentCheckerBoardScore)
        {
            OnScoreUpdateMessageData onScoreUpdateData = new OnScoreUpdateMessageData();
            onScoreUpdateData.old_score = old_score;
            onScoreUpdateData.new_score = CurrentCheckerBoardScore;
            SendMessage(FishMessageDefine.OnScoreUpdate, onScoreUpdateData);
        }
    }



    void OnScoreUpdate(OnScoreUpdateMessageData onScoreUpdateMessageData)
    {
        Debug.Log(string.Format("Score {0}", onScoreUpdateMessageData.new_score));
    }
}