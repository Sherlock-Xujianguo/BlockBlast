using System.Collections.Generic;
using UnityEngine;


public static class BlockShapesConfig
{
    public static short[][][] ShapeList = new short[][][]
    {
        //0
        new short [][]
        {   new short [] {1},
        },
        //1
        new short [][]
        {   new short [] {1, 1},
            new short [] {0, 1},
        },
        //2
        new short [][]
        {   new short [] {1, 1},
            new short [] {1, 1},
        },
        //3
        new short [][]
        {   new short [] {1, 0},
            new short [] {1, 0},
        },
        //4
        new short [][]
        {   new short [] {1, 1, 1 },
            new short [] {0, 0, 0 },
            new short [] {0, 0, 0 }
        },
        //5
        new short [][]
        {   new short[] { 1, 0, 0 },
            new short[] { 1, 0, 0 },
            new short[] { 1, 0, 0 },
        },
        //6
        new short [][]
        {   new short[] { 1, 1, 1 },
            new short[] { 0, 0, 1 },
            new short[] { 0, 0, 1 }
        },
        //7
        new short [][]
        {   new short[] { 1, 1, 0 },
            new short[] { 0, 1, 1 },
            new short[] { 0, 0, 0 }
        },
        //8
        new short [][]
        {   new short[] { 0, 1, 1 },
            new short[] { 1, 1, 0 },
            new short[] { 0, 0, 0 }
        },
        //9
        new short [][]
        {   new short[] { 1, 1, 1 },
            new short[] { 1, 1, 1 },
            new short[] { 1, 1, 1 }
        },
        //10
        new short [][]
        {   new short[] { 1, 1, 0 },
            new short[] { 1, 0, 0 },
            new short[] { 1, 0, 0 }
        },
        //11
        new short [][]
        {   new short[] { 1, 1, 1, 1, 1 },
            new short[] { 0, 0, 0, 0, 0 },
            new short[] { 0, 0, 0, 0, 0 },
            new short[] { 0, 0, 0, 0, 0 },
            new short[] { 0, 0, 0, 0, 0 },
        },
    };

    public static readonly Dictionary<int, string> BlockName = new Dictionary<int, string>
    {
        {9, "3x3" },
    };
}