using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : FishMonoSingleton<PuzzleManager>
{
    new void Awake()
    {
        base.Awake();

        ScoreData _instance = ScoreData.GetInstnace;
    }

    FailPanel FailPanelInstance;
    public BaseBlockComp CurrentDragBlock;

    // Start is called before the first frame update
    void Start()
    {
        FailPanelInstance = transform.Find("FailPanel").GetComponent<FailPanel>();
        FailPanelInstance.gameObject.SetActive(false);

        RegisterMessage<OnDragBlockMessageData>(FishMessageDefine.OnDragBlock, OnDragBlock);
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

        if (BlockGenerator.GetInstance.IsAreaEmpty())
        {
            BlockGenerator.GetInstance.ResetArea();
        }

        CheckerBoard.GetInstance.CheckGoal();

        FinishPlaceBlockMessageData finishPlaceBlockMessageData = new FinishPlaceBlockMessageData();
        SendMessage(FishMessageDefine.FinishPlaceBlock, finishPlaceBlockMessageData);

        if (IsGameFail())
        {
            IEnumerator WaitAndDoSomething(float waitTime)
            {
                yield return new WaitForSeconds(waitTime);  
                FailPanelInstance.gameObject.SetActive(true);
            }

            // 在代码中调用协程
            StartCoroutine(WaitAndDoSomething(2.0f));
        }
    }

    public bool IsGameFail()
    {
        List<BaseBlockComp> existBlocks = BlockGenerator.GetInstance.GetExistBlockComp();
        foreach (BaseBlockComp block in existBlocks)
        {
            if (CheckerBoard.GetInstance.HasRoomForBlock(block))
            {
                return false;
            }
        }

        return true;
    }

    public void Restart()
    {
        FailPanelInstance.gameObject.SetActive(false);
        CheckerBoard.GetInstance.Clear();
        BlockGenerator.GetInstance.ResetArea();
    }
}
