using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCompType
{
    short[][][] BlockCompShape = new short[][][]
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

    public short[][] GetBlockCompRandom()
    {
        int maxKind = BlockCompShape.GetLength(0);
        int index = Random.Range(0, maxKind);

        return BlockCompShape[index];
    }
}

public class BlockGenerator : MonoBehaviour
{
    BlockCompType BlockCompShapeGenerator = new BlockCompType();
    BaseBlockComp BaseBlockCompClass;

    BaseBlockComp BlockComp_1;
    BaseBlockComp BlockComp_2;
    BaseBlockComp BlockComp_3;

    Transform Pos_1;
    Transform Pos_2;
    Transform Pos_3;

    PuzzleManager PuzzleManagerInstance;

    int ValidBlockCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        BaseBlockCompClass = transform.Find("BaseBlockComp").GetComponent<BaseBlockComp>();
        Pos_1 = transform.Find("Pos_1");
        Pos_2 = transform.Find("Pos_2");
        Pos_3 = transform.Find("Pos_3");

        ResetArea();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisteryPuzzleManager(PuzzleManager Instance)
    {
        PuzzleManagerInstance = Instance;
    }

    public List<BaseBlockComp> GetExistBlockComp()
    {
        List<BaseBlockComp> Result = new List<BaseBlockComp>();
        if ( BlockComp_1 != null)
        {
            Result.Add(BlockComp_1);
        }
        if ( BlockComp_2 != null)
        {
            Result.Add(BlockComp_2);
        }
        if ( BlockComp_3 != null)
        {
            Result.Add(BlockComp_3);
        }
        return Result;
    }

    public void ResetArea()
    {
        if (BlockComp_1 != null)
        DestroyImmediate(BlockComp_1.gameObject);
        if ( BlockComp_2 != null)
        DestroyImmediate(BlockComp_2.gameObject);
        if ( BlockComp_3 != null)
        DestroyImmediate(BlockComp_3.gameObject);

        short[][] blocks = BlockCompShapeGenerator.GetBlockCompRandom();
        BlockComp_1 = Instantiate(BaseBlockCompClass).GetComponent<BaseBlockComp>();
        BlockComp_1.gameObject.SetActive(true);
        BlockComp_1.transform.SetParent(transform, false);
        BlockComp_1.transform.position = Pos_1.position;
        BlockComp_1.SetupBlock(blocks);
        BlockComp_1.RegisteryBlockGenerator(this);

        blocks = BlockCompShapeGenerator.GetBlockCompRandom();
        BlockComp_2 = Instantiate(BaseBlockCompClass).GetComponent<BaseBlockComp>();
        BlockComp_2.gameObject.SetActive(true);
        BlockComp_2.transform.SetParent(transform, false);
        BlockComp_2.transform.position = Pos_2.position;
        BlockComp_2.SetupBlock(blocks);
        BlockComp_2.RegisteryBlockGenerator(this);

        blocks = BlockCompShapeGenerator.GetBlockCompRandom();
        BlockComp_3 = Instantiate(BaseBlockCompClass).GetComponent<BaseBlockComp>();
        BlockComp_3.gameObject.SetActive(true);
        BlockComp_3.transform.SetParent(transform, false);
        BlockComp_3.transform.position = Pos_3.position;
        BlockComp_3.SetupBlock(blocks);
        BlockComp_3.RegisteryBlockGenerator(this);

        ValidBlockCount = 3;
    }

    public void OnDragBlock(BaseBlockComp BlockCompInstance)
    {
        PuzzleManagerInstance.OnDragBlock(BlockCompInstance);
    }

    public void OnReleaseBlock(BaseBlockComp BlockCompInstance)
    {
        PuzzleManagerInstance.OnReleaseBlock(BlockCompInstance);
    }

    public void DestroyBlock(BaseBlockComp BlockComp)
    {
        DestroyImmediate(BlockComp.gameObject);
        DestroyImmediate(BlockComp);
        ValidBlockCount--;
    }

    public bool IsAreaEmpty()
    {
        if (ValidBlockCount == 0) return true;
        return false;
    }
}
