using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlockComp : DragUI
{
    GameObject SingleBlockImageObject;
    List<GameObject> BlockObjectList;

    // Start is called before the first frame update
    public void Start()
    {
        base.Start();

        SingleBlockImageObject = transform.Find("SingleBlockImage").gameObject;
        SingleBlockImageObject.SetActive(false);

        // Test Edit
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupBlock(short[,] blocks)
    {
        for (int i = 0; i < blocks.GetLength(0); i++)
        {
            for (int j = 0; j < blocks.GetLength(1); j++)
            {
                short blockValue = blocks[i, j];
                if (blockValue > 0 ) 
                {
                    GameObject TempBlock = Instantiate<GameObject>(SingleBlockImageObject);
                    TempBlock.SetActive(true);
                    TempBlock.transform.localPosition = new Vector3(i*20+i, j*20+j, 0);
                    BlockObjectList.Add(TempBlock);
                }
            }
        }
    }
}
