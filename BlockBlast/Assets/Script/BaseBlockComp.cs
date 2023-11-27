using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseBlockComp : DragUI
{
    GameObject SingleBlockImageObject;
    BlockGenerator BlockGeneratorInstance; 

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

    public void SetupBlock(short[][] blocks)
    {
        if (SingleBlockImageObject == null)
        {
            Init();
        }
        for (int i = 0; i < blocks.Length; i++)
        {
            for (int j = 0; j < blocks.Length; j++)
            {
                short blockValue = blocks[i][j];
                if (blockValue > 0 ) 
                {
                    GameObject TempBlock = Instantiate<GameObject>(SingleBlockImageObject);
                    TempBlock.SetActive(true);
                    TempBlock.transform.SetParent(transform, false);
                    TempBlock.transform.localPosition = new Vector3(j*size + j, -i*size - i, 0);
                }
            }
        }
    }
}
