using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShapes
{
    public static short[][][] ShapeList = new short[][][]
    {
        new short [][]
        {   new short [] {1, 1, 1 },
            new short [] {0, 0, 0 },
            new short [] {0, 0, 0 }
        },
        new short [][]
        {   new short[] { 1, 0, 0 },
            new short[] { 1, 0, 0 },
            new short[] { 1, 0, 0 },
        },
        new short [][]
        {   new short[] { 1, 1, 1 },
            new short[] { 0, 0, 1 },
            new short[] { 0, 0, 1 }
        },
        new short [][]
        {   new short[] { 1, 1, 0 },
            new short[] { 0, 1, 1 },
            new short[] { 0, 0, 0 }
        },
        new short [][]
        {   new short[] { 0, 1, 1 },
            new short[] { 1, 1, 0 },
            new short[] { 0, 0, 0 }
        },
        new short [][]
        {   new short[] { 1, 1, 1, 1, 1 },
            new short[] { 0, 0, 0, 0, 0 },
            new short[] { 0, 0, 0, 0, 0 },
            new short[] { 0, 0, 0, 0, 0 },
            new short[] { 0, 0, 0, 0, 0 },
        },
    };

    public static short[][] GetBlockCompRandom()
    {
        int maxKind = ShapeList.GetLength(0);
        int index = Random.Range(0, maxKind);

        return ShapeList[index];
    }
}

public class BlockGeneratorData : FishSingleton<BlockGeneratorData>
{
    public BlockGeneratorData() { }

    public short[][] GetBlockCompRandom()
    {
        return BlockShapes.GetBlockCompRandom();
    }
}
