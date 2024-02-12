using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockGenerator : FishMonoSingleton<BlockGenerator>
{
    BlockGeneratorData GeneratorData = new BlockGeneratorData();
    BaseBlockComp BaseBlockCompClass;

    BaseBlockComp BlockComp_1;
    BaseBlockComp BlockComp_2;
    BaseBlockComp BlockComp_3;

    Transform Pos_1;
    Transform Pos_2;
    Transform Pos_3;

    static int ColorIndex = 0;

    int ValidBlockCount = 0;

    new void Awake()
    {
        base.Awake();

        BaseBlockCompClass = transform.Find("BaseBlockComp").GetComponent<BaseBlockComp>();
        Pos_1 = transform.Find("Pos_1");
        Pos_2 = transform.Find("Pos_2");
        Pos_3 = transform.Find("Pos_3");

        if (PuzzleManager.GetInstance.Debug)
        {
            transform.Find("Debug").gameObject.SetActive(true);
        }
    }

    void Start()
    {

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

        RoundData.RoundState state = RoundData.GetInstnace.CurrentRoundState;
        if (PuzzleManager.GetInstance.Debug)
        {
            TextMeshProUGUI text = transform.Find("Debug/RoundState").GetComponent<TextMeshProUGUI>();
            if (text)
            {
                text.SetText(state.ToString());
            }
        }

        short[][][] blocks = GeneratorData.GetBlockCompRandom();
        BlockData blockData = new BlockData();
        blockData.Initialize(blocks[0]);

        BlockComp_1 = Instantiate(BaseBlockCompClass).GetComponent<BaseBlockComp>();
        BlockComp_1.gameObject.SetActive(true);
        BlockComp_1.transform.SetParent(transform, false);
        BlockComp_1.transform.position = Pos_1.position;
        BlockComp_1.SetupBlock(blockData, ColorIndex++);

        blocks = GeneratorData.GetBlockCompRandom();
        blockData = new BlockData();
        blockData.Initialize(blocks[1]);

        BlockComp_2 = Instantiate(BaseBlockCompClass).GetComponent<BaseBlockComp>();
        BlockComp_2.gameObject.SetActive(true);
        BlockComp_2.transform.SetParent(transform, false);
        BlockComp_2.transform.position = Pos_2.position;
        BlockComp_2.SetupBlock(blockData, ColorIndex++);

        blocks = GeneratorData.GetBlockCompRandom();
        blockData = new BlockData();
        blockData.Initialize(blocks[2]);

        BlockComp_3 = Instantiate(BaseBlockCompClass).GetComponent<BaseBlockComp>();
        BlockComp_3.gameObject.SetActive(true);
        BlockComp_3.transform.SetParent(transform, false);
        BlockComp_3.transform.position = Pos_3.position;
        BlockComp_3.SetupBlock(blockData, ColorIndex++);

        ValidBlockCount = 3;
    }

    public void OnDragBlock(BaseBlockComp BlockCompInstance)
    {
        OnDragBlockMessageData data = new OnDragBlockMessageData();
        data.BlockComp = BlockCompInstance;
        SendMessage(FishMessageDefine.OnDragBlock, data);
    }

    public void OnReleaseBlock(BaseBlockComp BlockCompInstance)
    {
        OnReleaseBlockMessageData data = new OnReleaseBlockMessageData();
        data.BlockComp = BlockCompInstance;
        SendMessage(FishMessageDefine.OnReleaseBlock, data);
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
