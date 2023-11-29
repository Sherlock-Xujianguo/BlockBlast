using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckerBoard : MonoBehaviour
{
    PuzzleManager PuzzleManagerInstance;

    bool CanPlaceCurrentBlock = false;

    // 0: 空 1：准备放置： 2：已放置
    short[][] BoardValue = new short[8][] { new short[8], new short[8], new short[8], new short[8], new short[8], new short[8], new short[8], new short[8]};
    Vector3[][] BoardPosition = new Vector3[8][] { new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8], new Vector3[8] };
    Color BaseColor;
    Color PlacedColor = new Color(0f, 1.0f, 0f);

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
        int activeChildBlockCount = 0;
        bool CanRelease = true;
        int CanPlaceCount = 0;

        for (int i = 0; i < childBlockCount; i++)
        {
            if (!CanRelease)
            {
                break;
            }

            Transform childBlock = PuzzleManagerInstance.CurrentDragBlock.transform.GetChild(i);
            if (!childBlock.gameObject.activeSelf)
            {
                continue;
            }

            activeChildBlockCount++;
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

        if (CanPlaceCount == activeChildBlockCount)
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

    public void OnReleaseBlock()
    {
        if (CanPlaceCurrentBlock == false)
        {
            return;
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (BoardValue[i][j] == 1)
                {
                    GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                    Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                    TempBoardImage.color = PlacedColor;
                    BoardValue[i][j] = 2;
                }
            }
        }

        PuzzleManagerInstance.OnPlacedBlock();
    }

    public void CheckGoal()
    {
        List<int> i_GoalIndex = new List<int>();
        for (int i = 0; i < 8; i++)
        {
            bool canGoal = true;
            for (int j = 0; j < 8; j++)
            {
                if (BoardValue[i][j] != 2)
                {
                    canGoal = false; 
                    break;
                }
            }
            if (canGoal)
            {
                i_GoalIndex.Add(i);
            }
        }

        List<int> j_GoalIndex = new List<int>();
        for (int j = 0; j < 8; j++)
        {
            bool canGoal = true;
            for (int i = 0; i < 8; i++)
            {
                if (BoardValue[i][j] != 2)
                {
                    canGoal = false;
                    break;
                }
            }
            if (canGoal)
            {
                j_GoalIndex.Add(j);
            }
        }

        foreach (int i in i_GoalIndex)
        {
            for (int j = 0; j < 8 ; j++)
            {
                GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                TempBoardImage.color = BaseColor;
                BoardValue[i][j] = 0;
            }
        }

        foreach (int j in j_GoalIndex)
        {
            for (int i = 0; i < 8 ; i++)
            {
                GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                TempBoardImage.color = BaseColor;
                BoardValue[i][j] = 0;
            }
        }
    }
}
