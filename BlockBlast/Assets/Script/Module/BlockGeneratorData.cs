using System.Collections.Generic;
using UnityEngine;


public class BlockGeneratorData : FishSingleton<BlockGeneratorData>, IInitable
{
    public int GenerateStep = 0;

    List<BlockData> BlockDataStore = null;
    Dictionary<string, BlockData> BlockDataName = null;

    public void Init()
    {
        BlockDataStore = new List<BlockData>();
        BlockDataName = new Dictionary<string, BlockData>();

        int maxKind = BlockShapesConfig.ShapeList.GetLength(0);
        for (int i = 0; i < maxKind; i++)
        {
            BlockData blockData = new BlockData();
            blockData.Initialize(BlockShapesConfig.ShapeList[i]);
            BlockDataStore.Add(blockData);

            if (BlockShapesConfig.BlockName.ContainsKey(i))
            {
                BlockDataName.Add(BlockShapesConfig.BlockName[i], blockData);
            }
        }

        Restart();
    }

    public void Restart()
    {
        GenerateStep = 0;
    }

    public BlockData[] GetBlockCompRandom()
    {
        int maxKind = BlockDataStore.Count;
        bool canPlace = false;

        int[] indexArray = new int[3];
        while (!canPlace)
        {
            for (int i = 0; i < 3; i++)
            {
                indexArray[i] = Random.Range(0, maxKind);
                BlockData blockData = BlockDataStore[indexArray[i]];

                // 三个随机必有一个能放置
                if (CheckerBoardData.GetInstnace.HasRoomForBlock(blockData))
                {
                    canPlace = true;
                }
            }
        }

        BlockData[] rst = new BlockData[3];
        for (int i = 0; i < 3; i++)
        {
            rst[i] = BlockDataStore[indexArray[i]];
        }

        GenerateStep++;
        return rst;
    }

    public BlockData[] GetBlockEasyRound()
    {
        List<KeyValuePair<int, int>> BurstValueToIndex = new List<KeyValuePair<int, int>>();
        List<KeyValuePair<int, int>> EdgeValueToIndex = new List<KeyValuePair<int, int>>();

        for (int i = 0; i < BlockDataStore.Count; i++)
        {
            BlockData blockData = BlockDataStore[i];

            bool CanPlace;
            int Burst;
            int Edge;
            CheckerBoardData.GetInstnace.BlockPlaceValue(blockData, out CanPlace, out Burst, out Edge);

            if (CanPlace)
            {
                BurstValueToIndex.Add(new KeyValuePair<int, int>(i, Burst));
                EdgeValueToIndex.Add(new KeyValuePair<int, int>(i, Edge));
            }
        }

        BurstValueToIndex.Sort((x, y) =>
        {
            return x.Value < y.Value ? 1 : -1;
        });

        EdgeValueToIndex.Sort((x, y) =>
        {
            return x.Value > y.Value ? 1 : -1;
        });

        BlockData[] ret = new BlockData[3];
        int retIndex = 0;
        if (BurstValueToIndex[0].Value > 0)
        {
            ret[retIndex] = BlockDataStore[BurstValueToIndex[0].Key];
        }

        for (int i = 0; i < EdgeValueToIndex.Count; i++)
        {
            if (retIndex >= 3)
            {
                break;
            }

            ret[retIndex] = BlockDataStore[EdgeValueToIndex[i].Key];
            retIndex++;
        }

        return ret;
    }

    public BlockData[] GetBlockEasyRoundPlus()
    {
        List<KeyValuePair<int, int>> EdgeValueToIndex = new List<KeyValuePair<int, int>>();

        for (int i = 0; i < BlockDataStore.Count; i++)
        {
            BlockData blockData = BlockDataStore[i];

            bool CanPlace;
            int Burst;
            int Edge;
            CheckerBoardData.GetInstnace.BlockPlaceValue(blockData, out CanPlace, out Burst, out Edge);

            if (CanPlace)
            {
                EdgeValueToIndex.Add(new KeyValuePair<int, int>(i, Edge));
            }
        }

        EdgeValueToIndex.Sort((x, y) =>
        {
            return x.Value > y.Value ? 1 : -1;
        });

        BlockData[] ret = new BlockData[3];
        int retIndex = 0;

        if (BlockDataName.ContainsKey("3x3") && CheckerBoardData.GetInstnace.HasRoomForBlock(BlockDataName["3x3"]))
        {
            ret[retIndex] = BlockDataName["3x3"];
            retIndex++;
        }

        for (int i = 0; i < EdgeValueToIndex.Count; i++)
        {
            if (retIndex >= 3)
            {
                break;
            }

            ret[retIndex] = BlockDataStore[EdgeValueToIndex[i].Key];
            retIndex++;
        }

        return ret;
    }
}
