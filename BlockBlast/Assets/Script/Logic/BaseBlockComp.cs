using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseBlockComp : DragUI
{
    [SerializeField]
    public List<Sprite> Color;

    public int ColorIndex;

    public BlockData BlockData;

    public GameObject RealBlockParentObject;

    public GameObject PreviewBlockParentObject;

    public GameObject FirstRealBlock;

    GameObject SingleBlockImageObject;

    public void Init()
    {
        SingleBlockImageObject = transform.Find("SingleBlockImage").gameObject;
        SingleBlockImageObject.SetActive(false);
        RealBlockParentObject = transform.Find("RealBlockParent").gameObject;
        RealBlockParentObject.SetActive(false);
        PreviewBlockParentObject = transform.Find("PreviewBlockParent").gameObject;
        PreviewBlockParentObject.SetActive(true);
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

    public void SetupBlock(BlockData blockData, int colorIndex)
    {
        Init();

        ColorIndex = colorIndex % Color.Count;
        Sprite ColorSprite = Color[ColorIndex];
        SingleBlockImageObject.GetComponent<Image>().sprite = ColorSprite;
        

        BlockData = blockData;

        FirstRealBlock = null;

        short[][] blocks = BlockData.Matraix;
        for (int i = 0; i < blocks.Length; i++)
        {
            for (int j = 0; j < blocks[i].Length; j++)
            {
                short blockValue = blocks[i][j];

                if (blockValue > 0 ) 
                {
                    float MidIndex = BlockData.RowLength / 2.0f;

                    GameObject RealTempBlock = Instantiate(SingleBlockImageObject);
                    float RealSize = EffectConfig.RealBlockSize;
                    RealTempBlock.name = string.Format("RealBlockCompSingleImage_{0}_{1}", i, j);
                    RealTempBlock.SetActive(true);
                    RealTempBlock.transform.SetParent(RealBlockParentObject.transform, false);
                    RealTempBlock.transform.localPosition = new Vector3( (j-MidIndex)*RealSize + j, -i*RealSize - i, 0);
                    RealTempBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(RealSize, RealSize);
                    RealTempBlock.GetComponent<Image>().sprite = Color[ColorIndex];
                    if (FirstRealBlock == null)
                    {
                        FirstRealBlock = RealTempBlock;
                    }

                    GameObject PreviewTempBlock = Instantiate(SingleBlockImageObject);
                    float PreviewSize = EffectConfig.PreviewBlockSize;
                    PreviewTempBlock.name = string.Format("PreviewBlockCompSingleImage_{0}_{1}", i, j);
                    PreviewTempBlock.SetActive(true);
                    PreviewTempBlock.transform.SetParent(PreviewBlockParentObject.transform, false);
                    PreviewTempBlock.transform.localPosition = new Vector3( (j-MidIndex)*PreviewSize+j, -i*PreviewSize-i, 0);
                    PreviewTempBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(PreviewSize, PreviewSize);
                    PreviewTempBlock.GetComponent<Image>().sprite = Color[ColorIndex];
                }
            }
        }
    }

    new public void Start()
    {
        base.Start();

        
    }
}
