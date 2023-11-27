using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    CheckerBoard CheckerBoardInstance;
    BlockGenerator BlockGeneratorInstance;
    public BaseBlockComp CurrentDragBlock;

    // Start is called before the first frame update
    void Start()
    {
        CheckerBoardInstance = transform.Find("CheckerBoardInstance").GetComponent<CheckerBoard>();
        BlockGeneratorInstance = transform.Find("BlockGenerator").GetComponent<BlockGenerator>();

        CheckerBoardInstance.RegisteryPuzzleManager(this);
        BlockGeneratorInstance.RegisteryPuzzleManager(this);
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

    }

}
