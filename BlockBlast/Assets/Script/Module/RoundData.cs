

public class RoundData : FishSingleton<RoundData>, IInitable
{
    public enum RoundState
    {
        RandomRound = 0,
        EasyRound = 1,
        EasyRoundPlus = 2,
        HardRound = 3,
        HardRoundPlus = 4,
    }

    public RoundState CurrentRoundState = RoundState.RandomRound;

    int HardStateCombo = 0;

    public void Init()
    {
        CurrentRoundState = RoundState.RandomRound;

        ScoreData AwakeScoreData = ScoreData.GetInstnace;
        UpdateRoundState();

    }

    public void UpdateRoundState()
    {
        RoundState StateBuff = RoundState.RandomRound;

        int PlayerMaxScore = ScoreData.GetInstnace.GetPlayerMaxScore();
        int PlayerWeekMaxScore = ScoreData.GetInstnace.GetPlayerWeekMaxScore();
        int PlayerCurrentScore = ScoreData.GetInstnace.GetPlayerCurrentScore();
        int ComboCount = ScoreData.GetInstnace.GetComboCount() - 1; // 第一次消除data侧算Combo=1，设计上是combo的开始（=0）
        int BlockOnBoardCount = CheckerBoardData.GetInstnace.CurrentBlockOnBoardCount;
        int EmptyBoardCount = CheckerBoardData.GetInstnace.CurrentEmptyCount;

        // 首次出块
        if (BlockGeneratorData.GetInstnace.GenerateStep == 0)
        {
            if (PlayerMaxScore < 2000)
            {
                CurrentRoundState = RoundState.EasyRound;
            }
            else if (PlayerMaxScore < 2500)
            {
                CurrentRoundState = RoundState.HardRound;
            }
            else
            {
                CurrentRoundState = RoundState.RandomRound;
            }
            return;
        }

        bool EasyRoundState_1 = PlayerMaxScore < 2000 && PlayerCurrentScore < 2000 && BlockOnBoardCount >= 15;
        bool EasyRoundState_2 = PlayerWeekMaxScore < 2000 && PlayerCurrentScore < 2000 && BlockOnBoardCount >= 15 && BlockOnBoardCount <= 52;
        if (EasyRoundState_1 || EasyRoundState_2)
        {
            StateBuff = RoundState.EasyRound;
        }

        bool EasyRoundPlusState_1 = CurrentRoundState == RoundState.EasyRound;
        bool EasyRoundPlusState_2 = ComboCount < 5;
        bool EasyRoundPlusState_3 = EmptyBoardCount > 48;
        if (EasyRoundPlusState_1 && EasyRoundPlusState_2 && EasyRoundPlusState_3)
        {
            StateBuff = RoundState.EasyRoundPlus;
        }

        if (StateBuff != RoundState.RandomRound)
        {
            CurrentRoundState = StateBuff;
            HardStateCombo = 0;
            return;
        }

        float WeekScoreRate = PlayerCurrentScore * 1.0f / PlayerWeekMaxScore;
        bool HardRoundState_1 = (WeekScoreRate >= 0.15 && WeekScoreRate <= 0.2)
                                || (WeekScoreRate >= 0.3 && WeekScoreRate <= 0.4)
                                || (WeekScoreRate >= 0.55 && WeekScoreRate <= 0.7)
                                || (WeekScoreRate >= 0.85 && WeekScoreRate <= 0.95)
                                || (WeekScoreRate >= 1.05 && WeekScoreRate <= 1.3);
        bool HardRoundState_2 = BlockOnBoardCount >= 20;
        bool HardRoundState_3 = HardStateCombo < 2;
        if (HardRoundState_1 && HardRoundState_2 && HardRoundState_3)
        {
            StateBuff = RoundState.HardRound;
            HardStateCombo++;
        }

        bool HardRoundPlusState_1 = HardRoundState_1;
        bool HardRoundPlusState_2 = HardStateCombo == 3; // 已经有两次HardRound
        if (HardRoundPlusState_1 && HardRoundPlusState_2)
        {
            StateBuff = RoundState.HardRoundPlus;
            HardStateCombo = 0;
        }

        if (StateBuff != RoundState.RandomRound)
        {
            CurrentRoundState = StateBuff;
            return;
        }

        CurrentRoundState = StateBuff;
    }

    bool IsEasyRound()
    {
        
        return false;
    }
}
