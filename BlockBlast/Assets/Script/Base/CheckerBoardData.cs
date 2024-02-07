﻿
using System.Collections.Generic;

public class CheckerBoardData : FishSingleton<CheckerBoardData>, IInitable
{
    public int CheckBoardSize = 8;
    // 0: 空 1：准备放置： 2：已放置
    public short[][] BoardValue;

    public int CurrentBlockOnBoardCount = 0;
    public int CurrentEmptyCount = 0;

    public void Init()
    {
        CurrentEmptyCount = CheckBoardSize * CheckBoardSize;

        BoardValue = new short[CheckBoardSize][];
        for (int i = 0; i < CheckBoardSize; i++)
        {
            BoardValue[i] = new short[CheckBoardSize];
        }

        RegisterMessage<FinishPlaceBlockMessageData>(FishMessageDefine.FinishPlaceBlock, OnFinishPlaceBlock);
    }


    public void SetBoardReady(int i, int j)
    {
        BoardValue[i][j] = 1;
    }

    public void SetBoardPlaced(int i, int j)
    {
        BoardValue[i][j] = 2;
    }

    public bool IsBoardClear(int i, int j)
    {
        return BoardValue[i][j] == 0;
    }

    public bool IsBoardReady(int i, int j)
    {
        return BoardValue[i][j] == 1;
    }

    public bool IsBoardPlaced(int i, int j)
    {
        return BoardValue[i][j] == 2;
    }

    public void ClearBoardValue()
    {
        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                BoardValue[i][j] = 0;
            }
        }
    }

    public void ClearBoardValue(int i, int j)
    {
        BoardValue[i][j] = 0;
    }


    public void GetGoalRowAndColumn(out List<int> i_GoalIndex, out List<int> j_GoalIndex)
    {
        i_GoalIndex = new List<int>();
        for (int i = 0; i < CheckBoardSize; i++)
        {
            bool canGoal = true;
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (!IsBoardPlaced(i, j))
                {
                    canGoal = false;
                    break;
                }
            }
            if (canGoal)
            {
                i_GoalIndex.Add(i);
            }
        }

        j_GoalIndex = new List<int>();
        for (int j = 0; j < CheckBoardSize; j++)
        {
            bool canGoal = true;
            for (int i = 0; i < CheckBoardSize; i++)
            {
                if (!IsBoardPlaced(i, j))
                {
                    canGoal = false;
                    break;
                }
            }
            if (canGoal)
            {
                j_GoalIndex.Add(j);
            }
        }
    }

    public void GetReadyRowAndColumn(out List<int> i_GoalIndex, out List<int> j_GoalIndex)
    {
        i_GoalIndex = new List<int>();
        for (int i = 0; i < CheckBoardSize; i++)
        {
            bool canGoal = true;
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (! (IsBoardPlaced(i, j) || IsBoardReady(i, j) ))
                {
                    canGoal = false;
                    break;
                }
            }
            if (canGoal)
            {
                i_GoalIndex.Add(i);
            }
        }

        j_GoalIndex = new List<int>();
        for (int j = 0; j < CheckBoardSize; j++)
        {
            bool canGoal = true;
            for (int i = 0; i < CheckBoardSize; i++)
            {
                if (! (IsBoardPlaced(i, j) || IsBoardReady(i, j) ))
                {
                    canGoal = false;
                    break;
                }
            }
            if (canGoal)
            {
                j_GoalIndex.Add(j);
            }
        }
    }

    public bool HasRoomForBlock(BlockData blockData)
    {
        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (!IsBoardClear(i, j))
                {
                    continue;
                }

                int offsetIndex = 0;
                for (offsetIndex = 0; offsetIndex < blockData.Size; offsetIndex++)
                {
                    int[] offset = blockData.Offset[offsetIndex];
                    if (i + offset[0] >= CheckBoardSize || i + offset[0] < 0 ||
                        j + offset[1] >= CheckBoardSize || j + offset[1] < 0 ||
                        !IsBoardClear(i + offset[0], j + offset[1]))
                    {
                        break;
                    }
                }
                if (offsetIndex == blockData.Size)
                {
                    return true;
                }
            }
        }


        return false;
    }


    public void OnFinishPlaceBlock(FinishPlaceBlockMessageData finishPlaceBlockMessageData)
    {
        CurrentBlockOnBoardCount = 0;
        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (IsBoardPlaced(i, j))
                {
                    CurrentBlockOnBoardCount++;
                }
            }
        }

        CurrentEmptyCount = CheckBoardSize * CheckBoardSize - CurrentBlockOnBoardCount;
    }

}