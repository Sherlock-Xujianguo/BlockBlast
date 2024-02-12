

using UnityEngine;
using System;

public class ScoreData : FishSingleton<ScoreData>, IInitable
{

    // 当前显示在屏幕上的得分
    int CurrentCheckerBoardScore = 0;

    // 单次计算得分的中间缓存
    int ScoreBuffer = 0;

    // 最高分
    int PlayerMaxScore = 0;

    // 最高分更新时间戳 (天)
    int PlayerMaxScoreTimeStamp = 0;

    // 周最高分
    int PlayerWeekMaxScore = 0;

    // 周最高分更新时间戳 (天)
    int PlayerWeekMaxScoreTimeStamp = 0;

    int ComboCount = 0;
    int MissCount = 0;
    static readonly int TolerateMissCount = 3;


    public void Init()
    {
        PlayerMaxScore = PlayerPrefs.GetInt("PlayerMaxScore", 0);
        PlayerWeekMaxScore = PlayerPrefs.GetInt("PlayerWeekMaxScore", 0);
        PlayerMaxScoreTimeStamp = PlayerPrefs.GetInt("PlayerMaxScoreTimeStamp", 0);
        PlayerWeekMaxScoreTimeStamp = PlayerPrefs.GetInt("PlayerWeekMaxScoreTimeStamp", 0);

        if (FishTimeUtil.GetDay()/7 - PlayerWeekMaxScoreTimeStamp/7 > 1)
        {
            PlayerWeekMaxScore = 0;
        }

        RegisterMessage<OnPlaceBlockMessageData>(FishMessageDefine.OnPlaceBlock, OnPlaceBlock);
        RegisterMessage<OnScoreUpdateMessageData>(FishMessageDefine.OnScoreUpdate, OnScoreUpdate);
        RegisterMessage<OnBlastBlockMessageData>(FishMessageDefine.OnBlastBlock, OnBlastBlock);
        RegisterMessage<FinishPlaceBlockMessageData>(FishMessageDefine.FinishPlaceBlock, FinishPlaceBlock);
    }

    public void Restart()
    {
        CurrentCheckerBoardScore = 0;
        ScoreBuffer = 0;
        ComboCount = 0;
        MissCount = 0;

        OnComboUpdateMessageData onComboUpdateMessageData = new OnComboUpdateMessageData();
        onComboUpdateMessageData.ComboCount = ComboCount;
        SendMessage(FishMessageDefine.OnComboUpdate, onComboUpdateMessageData);

        OnScoreUpdateMessageData onScoreUpdateData = new OnScoreUpdateMessageData();
        onScoreUpdateData.old_score = CurrentCheckerBoardScore;
        onScoreUpdateData.new_score = CurrentCheckerBoardScore;
        SendMessage(FishMessageDefine.OnScoreUpdate, onScoreUpdateData);
    }

    public int GetPlayerMaxScore()
    {
        return PlayerMaxScore;
    }

    public int GetPlayerWeekMaxScore()
    {
        return PlayerWeekMaxScore;
    }

    public int GetPlayerCurrentScore()
    {
        return CurrentCheckerBoardScore;
    }

    public int GetComboCount()
    {
        return ComboCount;
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

        if (CurrentCheckerBoardScore > PlayerMaxScore)
        {
            PlayerMaxScore = CurrentCheckerBoardScore;
            PlayerMaxScoreTimeStamp = FishTimeUtil.GetDay();
            PlayerPrefs.SetInt("PlayerMaxScore", PlayerMaxScore);
            PlayerPrefs.SetInt("PlayerMaxScoreTimeStamp", PlayerMaxScoreTimeStamp);
        }

        if (CurrentCheckerBoardScore > PlayerWeekMaxScore)
        {
            PlayerWeekMaxScore = CurrentCheckerBoardScore;
            PlayerWeekMaxScoreTimeStamp = FishTimeUtil.GetDay();
            PlayerPrefs.SetInt("PlayerWeekMaxScore", PlayerWeekMaxScore);
            PlayerPrefs.SetInt("PlayerWeekMaxScoreTimeStamp", PlayerWeekMaxScoreTimeStamp);
        }
    }

    void OnScoreUpdate(OnScoreUpdateMessageData onScoreUpdateMessageData)
    {
        Debug.Log(string.Format("Score {0}", onScoreUpdateMessageData.new_score));
    }
}