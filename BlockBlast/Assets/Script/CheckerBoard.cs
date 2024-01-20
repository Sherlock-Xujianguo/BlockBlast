using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheckerBoard : FishMonoSingleton<CheckerBoard>
{
    public static readonly int CheckBoardSize = CheckerBoardData.GetInstnace.CheckBoardSize;

    CheckerBoardData Data = CheckerBoardData.GetInstnace;

    Vector3[][] BoardPosition;

    bool CanPlaceCurrentBlock = false;

    Color BaseColor;
    Color PlacedColor = new Color(0f, 1.0f, 0f);

    new void Awake()
    {
        base.Awake();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        BoardPosition = new Vector3[CheckBoardSize][];
        for (int i = 0; i < CheckBoardSize; i++)
        {
            BoardPosition[i] = new Vector3[CheckBoardSize];
        }
        
        SetupBoard();

        RegisterMessage<OnReleaseBlockMessageData>(FishMessageDefine.OnReleaseBlock, OnReleaseBlock);
    }

    public void Clear()
    {
        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                TempBoardImage.color = BaseColor;
            }
        }

        Data.ClearBoardValue();
    }

    void __ClearReadyPlaceBlockValue() 
    {
        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (Data.GetBoardValue(i, j) == 1)
                {
                    GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                    Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                    TempBoardImage.color = BaseColor;
                    Data.ClearBoardValue(i, j);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PuzzleManager.GetInstance.CurrentDragBlock == null)
        {
            return;
        }

        UpdateCanPlaceCurrentBlock();
    }

    void UpdateCanPlaceCurrentBlock()
    {
        CanPlaceCurrentBlock = false;
        __ClearReadyPlaceBlockValue();

        BaseBlockComp CurrentDragBlock = PuzzleManager.GetInstance.CurrentDragBlock;
        GameObject RealBlockParent = CurrentDragBlock.RealBlockParentObject;

        int childBlockCount = RealBlockParent.transform.childCount;
        bool CanRelease = true;
        int CanPlaceCount = 0;

        for (int i = 0; i < childBlockCount; i++)
        {
            if (!CanRelease)
            {
                break;
            }

            Transform childBlock = RealBlockParent.transform.GetChild(i);
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (!CanRelease)
                {
                    break;
                }

                for (int k = 0; k < CheckBoardSize; k++)
                {
                    if (Vector3.Distance(childBlock.position, BoardPosition[j][k]) < CurrentDragBlock.RealSize/2)
                    {
                        if (Data.GetBoardValue(j, k) == 0)
                        {
                            GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", j, k)).gameObject;
                            Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                            TempBoardImage.color = new Color(1.0f, 0f, 0f);
                            Data.SetBoardValue(j, k, 1);

                            CanPlaceCount++;
                        }
                        else if (Data.GetBoardValue(j, k) == 2)
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

    void SetupBoard()
    {
        GameObject image = transform.Find("Image").gameObject;
        Image tempimage = image.GetComponent<Image>();
        BaseColor = tempimage.color;
        image.SetActive(false);
        RectTransform rect = image.GetComponent<RectTransform>();
        float size = rect.rect.width;

        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                GameObject TempImage = Instantiate(image);
                TempImage.SetActive(true);
                TempImage.transform.localPosition = new Vector3(-size * 3 - size / 2 + size * i + i, size * 3 + size / 2 - size * j - j, 0);
                TempImage.transform.SetParent(transform, false);
                TempImage.name = string.Format("BoardSingleImage_{0}_{1}", i, j);

                Data.ClearBoardValue(i, j);
                BoardPosition[i][j] = TempImage.transform.position;
            }
        }
    }

    public void OnReleaseBlock(OnReleaseBlockMessageData ReleaseData)
    {
        if (CanPlaceCurrentBlock == false)
        {
            return;
        }

        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (Data.GetBoardValue(i, j) == 1)
                {
                    GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                    Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                    TempBoardImage.color = PlacedColor;
                    Data.SetBoardValue(i,j, 2);
                }
            }
        }
        OnPlaceBlockMessageData PlaceData = new OnPlaceBlockMessageData();
        PlaceData.BlockComp = ReleaseData.BlockComp;
        SendMessage(FishMessageDefine.OnPlaceBlock, PlaceData);

        PuzzleManager.GetInstance.OnPlacedBlock();
    }

    public void CheckGoal()
    {
        List<int> i_GoalIndex = new List<int>();
        for (int i = 0; i < CheckBoardSize; i++)
        {
            bool canGoal = true;
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (Data.GetBoardValue(i, j) != 2)
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
        for (int j = 0; j < CheckBoardSize; j++)
        {
            bool canGoal = true;
            for (int i = 0; i < CheckBoardSize; i++)
            {
                if (Data.GetBoardValue(i, j) != 2)
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
            for (int j = 0; j < CheckBoardSize ; j++)
            {
                GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                TempBoardImage.color = BaseColor;
                Data.SetBoardValue(i ,j , 0);
            }
        }

        foreach (int j in j_GoalIndex)
        {
            for (int i = 0; i < CheckBoardSize ; i++)
            {
                GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                TempBoardImage.color = BaseColor;
                Data.SetBoardValue(i, j, 0);
            }
        }

        if (i_GoalIndex.Count + j_GoalIndex.Count > 0)
        {
            OnBlastBlockMessageData onBlastBlockMessageData = new OnBlastBlockMessageData();
            onBlastBlockMessageData.Row = i_GoalIndex.Count;
            onBlastBlockMessageData.Column = j_GoalIndex.Count;
            onBlastBlockMessageData.TotalBlastRowAndColumn = j_GoalIndex.Count;

            SendMessage(FishMessageDefine.OnBlastBlock, onBlastBlockMessageData);
        }
    }

    public bool HasRoomForBlock(BaseBlockComp blockComp)
    {
        BlockData blockData = blockComp.BlockData;
        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize ; j++)
            {
                if (Data.GetBoardValue(i, j) != 0)
                {
                    continue;
                }

                int offsetIndex = 0;
                for (offsetIndex = 0; offsetIndex < blockData.Size; offsetIndex++)
                {
                    int[] offset = blockData.Offset[offsetIndex];
                    if (i + offset[1] >= CheckBoardSize || i + offset[1] < 0 ||
                        j + offset[0] >= CheckBoardSize || j + offset[0] < 0 ||
                        Data.GetBoardValue(i+offset[1], j+offset[0]) != 0)
                    {
                        break;
                    }
                }
                if (offsetIndex == blockData.Size)
                {
                    return true;
                }
            }
        }


        return false;
    }
}
