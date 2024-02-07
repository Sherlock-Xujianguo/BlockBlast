
using UnityEngine;

enum GeneratorState
{
    RandomRound = 0,
    EasyRound = 1,
    EasyRoundPlus = 2,
    HardRound = 3,
    HardRoundPlus = 4,
}

public class BlockGeneratorData : FishSingleton<BlockGeneratorData>
{
    public BlockGeneratorData() { }

    public short[][][] GetBlockCompRandom()
    {
        int maxKind = BlockShapesConfig.ShapeList.GetLength(0);
        bool canPlace = false;

        int[] indexArray = new int[3];
        while (!canPlace)
        {
            for (int i = 0; i < 3; i++)
            {
                indexArray[i] = Random.Range(0, maxKind);
                BlockData blockData = new BlockData();
                blockData.Initialize(BlockShapesConfig.ShapeList[indexArray[i]]);
                if (CheckerBoardData.GetInstnace.HasRoomForBlock(blockData))
                {
                    canPlace = true;
                }
            }
        }

        short[][][] rst = new short[3][][];
        for (int i = 0; i < 3; i++)
        {
            rst[i] = BlockShapesConfig.ShapeList[indexArray[i]];
        }
        return rst;
    }
}
