
public class CheckerBoardData : FishSingleton<CheckerBoardData>, IInitable
{
    public int CheckBoardSize = 8;
    // 0: 空 1：准备放置： 2：已放置
    public short[][] BoardValue;

    public void Init()
    {
        BoardValue = new short[CheckBoardSize][];
        for (int i = 0; i < CheckBoardSize; i++)
        {
            BoardValue[i] = new short[CheckBoardSize];
        }
    }

    public short GetBoardValue(int i, int j)
    {
        return BoardValue[i][j];
    }

    public void SetBoardValue(int i, int j, short Value)
    {
        BoardValue[i][j] = Value;
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
}