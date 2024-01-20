
using UnityEngine;


public class BlockGeneratorData : FishSingleton<BlockGeneratorData>
{
    public BlockGeneratorData() { }

    public short[][] GetBlockCompRandom()
    {
        int maxKind = BlockShapesConfig.ShapeList.GetLength(0);
        int index = Random.Range(0, maxKind);

        return BlockShapesConfig.ShapeList[index];
    }
}
