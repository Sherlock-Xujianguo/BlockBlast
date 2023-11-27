using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckerBoard : MonoBehaviour
{
    PuzzleManager PuzzleManagerInstance;

    bool CanPlaceCurrentBlock = false;

    // 0: �� 1��׼�����ã� 2���ѷ���
    short[][] BoardValue = new short[8][] { new short[8], new short[8], new short[8], new short[8], new short[8], new short[8], new short[8], new short[8]};
    Vector3[][] BoardPosition = new Vector3[8][] { new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8] };
    Color BaseColor;

    // Start is called before the first frame update
    void Start()
    {
        SetupBoard();
    }

    void __ClearReadyPlaceBlockValue() 
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (BoardValue[i][j] == 1)
                {
                    GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                    Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                    TempBoardImage.color = BaseColor;
                    BoardValue[i][j] = 0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PuzzleManagerInstance == null || PuzzleManagerInstance.CurrentDragBlock == null)
        {
            return;
        }
        CanPlaceCurrentBlock = false;
        __ClearReadyPlaceBlockValue();

        int childBlockCount = PuzzleManagerInstance.CurrentDragBlock.transform.childCount;
        bool CanRelease = true;
        int CanPlaceCount = 0;

        for (int i = 0; i < childBlockCount; i++)
        {
            if (!CanRelease)
            {
                break;
            }

            Transform childBlock = PuzzleManagerInstance.CurrentDragBlock.transform.GetChild(i);

            for (int j = 0; j < 8; j++)
            {
                if (!CanRelease)
                {
                    break;
                }

                for (int k = 0; k < 8; k++)
                {
                    if (Vector3.Distance(childBlock.position, BoardPosition[j][k]) < 40)
                    {
                        if (BoardValue[j][k] == 0)
                        {
                            GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", j, k)).gameObject;
                            Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                            TempBoardImage.color = new Color(1.0f, 0f, 0f);
                            BoardValue[j][k] = 1;

                            CanPlaceCount++;
                        }
                        else if (BoardValue[j][k] == 2)
                        {
                            CanRelease = false;
                            __ClearReadyPlaceBlockValue();
                            break;
                        }
                    }
                }
            }
        }

        if (CanPlaceCount == childBlockCount)
        {
            CanPlaceCurrentBlock = true;
        }
    }

    public void RegisteryPuzzleManager(PuzzleManager Instance)
    {
        PuzzleManagerInstance = Instance;
    }

    void SetupBoard()
    {
        GameObject image = transform.Find("Image").gameObject;
        Image tempimage = image.GetComponent<Image>();
        BaseColor = tempimage.color;
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
                TempImage.name = string.Format("BoardSingleImage_{0}_{1}", i, j);

                BoardValue[i][j] = 0;
                BoardPosition[i][j] = TempImage.transform.position;
            }
        }
    }
}