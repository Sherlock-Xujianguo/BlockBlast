using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    CheckerBoard CheckerBoardInstance;
    BlockGenerator BlockGeneratorInstance;
    FailPanel FailPanelInstance;
    public BaseBlockComp CurrentDragBlock;

    // Start is called before the first frame update
    void Start()
    {
        CheckerBoardInstance = transform.Find("CheckerBoard").GetComponent<CheckerBoard>();
        BlockGeneratorInstance = transform.Find("BlockGenerator").GetComponent<BlockGenerator>();
        FailPanelInstance = transform.Find("FailPanel").GetComponent<FailPanel>();
        FailPanelInstance.gameObject.SetActive(false);
        
        CheckerBoardInstance.RegisteryPuzzleManager(this);
        BlockGeneratorInstance.RegisteryPuzzleManager(this);
        FailPanelInstance.RegisteryPuzzleManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDragBlock(BaseBlockComp BlockCompInstance)
    {
        CurrentDragBlock = BlockCompInstance;
    }

    public void OnReleaseBlock(BaseBlockComp BlockCompInstance)
    {
        CheckerBoardInstance.OnReleaseBlock();
    }

    public void OnPlacedBlock()
    {
        BlockGeneratorInstance.DestroyBlock(CurrentDragBlock);

        if (BlockGeneratorInstance.IsAreaEmpty())
        {
            BlockGeneratorInstance.ResetArea();
        }

        CheckerBoardInstance.CheckGoal();

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
        List<BaseBlockComp> existBlocks = BlockGeneratorInstance.GetExistBlockComp();
        foreach (BaseBlockComp block in existBlocks)
        {
            if (CheckerBoardInstance.HasRoomForBlock(block))
            {
                return false;
            }
        }

        return true;
    }

    public void Restart()
    {
        FailPanelInstance.gameObject.SetActive(false);
        CheckerBoardInstance.Clear();
        BlockGeneratorInstance.ResetArea();
    }
}
