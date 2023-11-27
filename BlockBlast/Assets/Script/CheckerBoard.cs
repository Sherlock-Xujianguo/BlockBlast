using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerBoard : MonoBehaviour
{
    PuzzleManager PuzzleManagerInstance;

    // 0: 空 1：准备放置： 2：已放置
    short[][] BoardValue = new short[8][] { new short[8], new short[8], new short[8], new short[8], new short[8], new short[8], new short[8], new short[8]};
    Vector3[][] BoardPosition = new Vector3[8][] { new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8] };

    // Start is called before the first frame update
    void Start()
    {
        SetupBoard();
    }

    // Update is called once per frame
    void Update()
    {
        if (PuzzleManagerInstance.CurrentDragBlock == null)
        {
            return;
        }


        int childBlockCount = PuzzleManagerInstance.CurrentDragBlock.transform.childCount;
        for (int i = 0; i < childBlockCount; i++)
        {
            bool CanRelease = false;

            Transform childBlock = PuzzleManagerInstance.CurrentDragBlock.transform.GetChild(i);

            for (int j = 0; j < 8; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (Vector3.Distance(childBlock.position, BoardPosition[j][k]) < 10)
                    {
                        
                    }
                }
            }
        }
    }

    public void RegisteryPuzzleManager(PuzzleManager Instance)
    {
        PuzzleManagerInstance = Instance;
    }

    void SetupBoard()
    {
        GameObject image = transform.Find("Image").gameObject;
        image.SetActive(false);
        RectTransform rect = image.GetComponent<RectTransform>();
        float size = rect.rect.width;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject TempImage = Instantiate(image);
                TempImage.SetActive(true);
                TempImage.transform.localPosition = new Vector3(-size * 3 - size / 2 + size * i + i, size * 3 + size / 2 - size * j - j, 0);
                TempImage.transform.SetParent(transform, false);

                BoardValue[i][j] = 0;
                BoardPosition[i][j] = TempImage.transform.position;
            }
        }
    }
}
