using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : FishMonoSingleton<PuzzleManager>
{
    [HideInInspector]
    public BaseBlockComp CurrentDragBlock;

    public bool Debug = false;

    FailPanel FailPanelInstance;

    new void Awake()
    {
        base.Awake();

        ScoreData _instance = ScoreData.GetInstnace;
    }

    

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        FailPanelInstance = transform.Find("FailPanel").GetComponent<FailPanel>();
        FailPanelInstance.gameObject.SetActive(false);

        RegisterMessage<OnDragBlockMessageData>(FishMessageDefine.OnDragBlock, OnDragBlock);

        Restart();
    }

    private void OnDestroy()
    {
        UnregisterMessage<OnDragBlockMessageData>(FishMessageDefine.OnDragBlock, OnDragBlock);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDragBlock(OnDragBlockMessageData OnDragBlockMessageData)
    {
        CurrentDragBlock = OnDragBlockMessageData.BlockComp;
    }

    public void OnPlacedBlock()
    {
        BlockGenerator.GetInstance.DestroyBlock(CurrentDragBlock);

        CheckerBoard.GetInstance.CheckGoal();

        FinishPlaceBlockMessageData finishPlaceBlockMessageData = new FinishPlaceBlockMessageData();
        SendMessage(FishMessageDefine.FinishPlaceBlock, finishPlaceBlockMessageData);


        if (BlockGenerator.GetInstance.IsAreaEmpty())
        {
            RoundData.GetInstnace.UpdateRoundState();
            BlockGenerator.GetInstance.ResetArea();
        }
        else if (IsGameFail())
        {
            IEnumerator WaitAndDoSomething(float waitTime)
            {
                yield return new WaitForSeconds(waitTime);
                FailPanelInstance.gameObject.SetActive(true);
            }

            StartCoroutine(WaitAndDoSomething(2.0f));
        }
    }

    public bool IsGameFail()
    {
        List<BaseBlockComp> existBlocks = BlockGenerator.GetInstance.GetExistBlockComp();
        foreach (BaseBlockComp block in existBlocks)
        {
            if (CheckerBoardData.GetInstnace.HasRoomForBlock(block.BlockData))
            {
                return false;
            }
        }

        return true;
    }

    public void Restart()
    {
        ScoreData.GetInstnace.Restart();
        CheckerBoard.GetInstance.Clear();
        BlockGeneratorData.GetInstnace.Restart();
        RoundData.GetInstnace.UpdateRoundState();
        BlockGenerator.GetInstance.ResetArea();
    }
}
