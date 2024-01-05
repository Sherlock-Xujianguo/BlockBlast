using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseBlockComp : DragUI
{
    GameObject SingleBlockImageObject;
    BlockGenerator BlockGeneratorInstance;

    BlockData BlockData;

    float size = 20;

    public void Init()
    {
        SingleBlockImageObject = transform.Find("SingleBlockImage").gameObject;
        SingleBlockImageObject.SetActive(false);
        RectTransform rectTransform = SingleBlockImageObject.GetComponent<RectTransform>();
        size = rectTransform.rect.width;
    }

    public void RegisteryBlockGenerator(BlockGenerator blockGeneratorInstance)
    {
        BlockGeneratorInstance = blockGeneratorInstance;
    }

    // Start is called before the first frame update
    public void Start()
    {
        base.Start();

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnBasePointerDown(PointerEventData EventData)
    {
        BlockGeneratorInstance.OnDragBlock(this);
    }

    public override void OnBasePointerUp(PointerEventData EventData)
    {
        BlockGeneratorInstance.OnReleaseBlock(this);
    }

    public void SetupBlock(BlockData blockData)
    {
        this.BlockData = blockData;

        if (SingleBlockImageObject == null)
        {
            Init();
        }
        short[][] blocks = BlockData.Matraix;
        for (int i = 0; i < blocks.Length; i++)
        {
            for (int j = 0; j < blocks[i].Length; j++)
            {
                short blockValue = blocks[i][j];

                if (blockValue > 0 ) 
                {
                    GameObject TempBlock = Instantiate<GameObject>(SingleBlockImageObject);
                    TempBlock.name = string.Format("BlockCompSingleImage_{0}_{1}", i, j);
                    TempBlock.SetActive(true);
                    TempBlock.transform.SetParent(transform, false);
                    TempBlock.transform.localPosition = new Vector3(j*size + j, -i*size - i, 0);
                }
            }
        }
    }
}
