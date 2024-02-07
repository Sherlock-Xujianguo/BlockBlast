

public class BlockData : FishSimpleClass
{
    public short[][] Matraix;
    public int[] FirstBlockPosition = new int[] { -1, -1 };
    public int[][] Offset;
    public int Size;
    public int RowLength;

    public BlockData() { }

    public void Initialize(short[][] Matraix)
    {
        this.Matraix = Matraix;
        Size = 0;

        for (int i = 0; i < Matraix.Length; i++)
        {
            for (int j = 0; j < Matraix[i].Length; j++)
            {
                // 找第一个块的位置
                short blockValue = Matraix[i][j];
                if (FirstBlockPosition[0] == -1 && blockValue > 0)
                {
                    FirstBlockPosition[0] = i;
                    FirstBlockPosition[1] = j;
                }

                // 统计块的个数
                if (blockValue > 0)
                {
                    Size++;
                }
            }
        }

        Offset = new int[Size][];

        RowLength = 0;
        int offsetIndex = 0;
        for (int i = 0; i < Matraix.Length; i++)
        {
            for (int j = 0; j < Matraix[i].Length; j++)
            {
                short blockValue = Matraix[i][j];
                if (blockValue > 0)
                {
                    // 这里对应的是棋盘逻辑的offset
                    Offset[offsetIndex] = new int[] {j - FirstBlockPosition[1], i - FirstBlockPosition[0]};
                    offsetIndex++;

                    if (j - FirstBlockPosition[1] > RowLength)
                    {
                        RowLength = j - FirstBlockPosition[1];
                    }
                }
            }
        }
    }
}
