using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseBlockComp : DragUI
{
    public BlockData BlockData;

    public GameObject RealBlockParentObject;

    public GameObject PreviewBlockParentObject;

    public float RealSize = 80;
    public float PreviewSize = 40;

    GameObject SingleBlockImageObject;

    public void Init()
    {
        SingleBlockImageObject = transform.Find("SingleBlockImage").gameObject;
        SingleBlockImageObject.SetActive(false);
        RealBlockParentObject = transform.Find("RealBlockParent").gameObject;
        RealBlockParentObject.SetActive(false);
        PreviewBlockParentObject = transform.Find("PreviewBlockParent").gameObject;
        PreviewBlockParentObject.SetActive(true);

        RectTransform rectTransform = SingleBlockImageObject.GetComponent<RectTransform>();
        RealSize = rectTransform.rect.width;

    }

    
    public override void OnBasePointerDown(PointerEventData EventData)
    {
        BlockGenerator.GetInstance.OnDragBlock(this);

        // TODO:从preview到Real切换的动画
        PreviewBlockParentObject.SetActive(false);
        RealBlockParentObject.SetActive(true);
    }

    public override void OnBasePointerUp(PointerEventData EventData)
    {
        PreviewBlockParentObject.SetActive(true);
        RealBlockParentObject.SetActive(false);

        BlockGenerator.GetInstance.OnReleaseBlock(this);
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
                    GameObject RealTempBlock = Instantiate(SingleBlockImageObject);
                    RealTempBlock.name = string.Format("RealBlockCompSingleImage_{0}_{1}", i, j);
                    RealTempBlock.SetActive(true);
                    RealTempBlock.transform.SetParent(RealBlockParentObject.transform, false);
                    RealTempBlock.transform.localPosition = new Vector3(j*RealSize + j, -i*RealSize - i, 0);

                    GameObject PreviewTempBlock = Instantiate(SingleBlockImageObject);
                    PreviewTempBlock.name = string.Format("PreviewBlockCompSingleImage_{0}_{1}", i, j);
                    PreviewTempBlock.SetActive(true);
                    PreviewTempBlock.transform.SetParent(PreviewBlockParentObject.transform, false);
                    PreviewTempBlock.transform.localPosition = new Vector3(j*PreviewSize+j, -i*PreviewSize-i, 0);
                    PreviewTempBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(PreviewSize, PreviewSize);
                }
            }
        }
    }

    new public void Start()
    {
        base.Start();

        Init();
    }
}
