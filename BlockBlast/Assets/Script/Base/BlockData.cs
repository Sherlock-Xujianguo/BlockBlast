using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockData : FishSimpleClass
{
    public short[][] Matraix;
    public int[] FirstBlockPosition = new int[] { -1, -1 };
    public int[][] Offset;
    public int Size;

    public BlockData() { }

    public void Initialize(short[][] Matraix)
    {
        this.Matraix = Matraix;
        Size = 0;

        for (int i = 0; i < Matraix.Length; i++)
        {
            for (int j = 0; j < Matraix[i].Length; j++)
            {
                short blockValue = Matraix[i][j];
                if (FirstBlockPosition[0] == -1 && blockValue > 0)
                {
                    FirstBlockPosition[0] = i;
                    FirstBlockPosition[1] = j;
                }
                if (blockValue > 0)
                {
                    Size++;
                }
            }
        }

        Offset = new int[Size][];

        int offsetIndex = 0;
        for (int i = 0; i < Matraix.Length; i++)
        {
            for (int j = 0; j < Matraix[i].Length; j++)
            {
                short blockValue = Matraix[i][j];
                if (blockValue > 0)
                {
                    Offset[offsetIndex] = new int[] { i - FirstBlockPosition[0], j - FirstBlockPosition[1] };
                    offsetIndex++;
                }
            }
        }
    }
}
