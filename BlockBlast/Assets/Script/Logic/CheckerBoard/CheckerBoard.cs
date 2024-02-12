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

    Color OriginColor;

    Dictionary<int[], Sprite> OriginColorSprite = new Dictionary<int[], Sprite>();

    new void Awake()
    {
        base.Awake();
    }
    
    void Start()
    {
        BoardPosition = new Vector3[CheckBoardSize][];
        for (int i = 0; i < CheckBoardSize; i++)
        {
            BoardPosition[i] = new Vector3[CheckBoardSize];
        }
        
        __SetupBoard();

        RegisterMessage<OnReleaseBlockMessageData>(FishMessageDefine.OnReleaseBlock, __OnReleaseBlock);
    }

    void Update()
    {
        __ClearReadyPlaceBlockValue();

        if (PuzzleManager.GetInstance.CurrentDragBlock == null)
        {
            return;
        }

        __UpdateCanPlaceCurrentBlock();
    }

    public void Clear()
    {
        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                TempBoardImage.color = OriginColor;
            }
        }

        Data.ClearBoardValue();
    }

    public void CheckGoal()
    {
        List<int> i_GoalIndex;
        List<int> j_GoalIndex;

        Data.GetGoalRowAndColumn(out i_GoalIndex, out j_GoalIndex);

        foreach (int i in i_GoalIndex)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                TempBoardImage.color = OriginColor;
                Data.ClearBoardValue(i, j);
            }
        }

        foreach (int j in j_GoalIndex)
        {
            for (int i = 0; i < CheckBoardSize; i++)
            {
                GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                TempBoardImage.color = OriginColor;
                Data.ClearBoardValue(i, j);
            }
        }

        CheckerBoardData.GetInstnace.OnFinishPlaceBlock();

        if (i_GoalIndex.Count + j_GoalIndex.Count > 0)
        {
            OnBlastBlockMessageData onBlastBlockMessageData = new OnBlastBlockMessageData();
            onBlastBlockMessageData.Row = i_GoalIndex.Count;
            onBlastBlockMessageData.Column = j_GoalIndex.Count;
            onBlastBlockMessageData.TotalBlastRowAndColumn = j_GoalIndex.Count;

            SendMessage(FishMessageDefine.OnBlastBlock, onBlastBlockMessageData);
        }
    }


    void __OnReleaseBlock(OnReleaseBlockMessageData ReleaseData)
    {
        if (CanPlaceCurrentBlock == false)
        {
            return;
        }

        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (Data.IsBoardReady(i, j))
                {
                    GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                    Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                    TempBoardImage.sprite = ReleaseData.BlockComp.Color[ReleaseData.BlockComp.ColorIndex];
                    TempBoardImage.color = new Color(1, 1, 1);
                    Data.SetBoardPlaced(i, j);
                }
            }
        }
        OnPlaceBlockMessageData PlaceData = new OnPlaceBlockMessageData();
        PlaceData.BlockComp = ReleaseData.BlockComp;
        SendMessage(FishMessageDefine.OnPlaceBlock, PlaceData);

        PuzzleManager.GetInstance.OnPlacedBlock();
    }


    void __ClearReadyPlaceBlockValue() 
    {
        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (Data.IsBoardReady(i, j))
                {
                    GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                    Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                    TempBoardImage.color = OriginColor;
                    Data.ClearBoardValue(i, j);
                }
            }
        }

        foreach (KeyValuePair<int[], Sprite> pair in OriginColorSprite)
        {
            int[] ijIndex = pair.Key;
            Sprite color = pair.Value;
            GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", ijIndex[0], ijIndex[1])).gameObject;
            Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
            TempBoardImage.sprite = color;
        }

        OriginColorSprite.Clear();
    }

    

    void __UpdateCanPlaceCurrentBlock()
    {
        CanPlaceCurrentBlock = false;

        BaseBlockComp CurrentDragBlock = PuzzleManager.GetInstance.CurrentDragBlock;
        Sprite DragColorSprite = CurrentDragBlock.Color[CurrentDragBlock.ColorIndex];


        // 找到开头的方块对应的棋盘坐标
        Transform FirstRealBlockTrans = CurrentDragBlock.FirstRealBlock.transform;
        int[] FirstRealBlockTargetBoardXY = null;
        for (int i = 0; i < CheckBoardSize; i++)
        {
            if (FirstRealBlockTargetBoardXY != null)
            {
                break;
            }
            for (int j = 0; j < CheckBoardSize; j++)
            {
                if (Vector3.Distance(FirstRealBlockTrans.position, BoardPosition[i][j]) < EffectConfig.RealBlockSize/2+5
                    && Data.IsBoardClear(i, j))
                {
                    FirstRealBlockTargetBoardXY = new int[2] { i, j };
                }
            }
        }

        if (FirstRealBlockTargetBoardXY == null)
        {
            return;
        }

        // 检查后续offset是不是都有位置
        int[][] OffsetArray = CurrentDragBlock.BlockData.Offset;
        for (int offsetIndex = 0; offsetIndex < CurrentDragBlock.BlockData.Size; offsetIndex++)
        {
            int[] pair = OffsetArray[offsetIndex];
            int CheckX = FirstRealBlockTargetBoardXY[0] + pair[0];
            int CheckY = FirstRealBlockTargetBoardXY[1] + pair[1];
            if (CheckX >= CheckBoardSize || CheckX < 0
                || CheckY >= CheckBoardSize || CheckY < 0)
            {
                return;
            }
            
            if (!Data.IsBoardClear(CheckX, CheckY))
            {
                return;
            }
        }


        CanPlaceCurrentBlock = true;
        for (int offsetIndex = 0; offsetIndex < CurrentDragBlock.BlockData.Size; offsetIndex++)
        {
            int[] pair = OffsetArray[offsetIndex];
            int i = FirstRealBlockTargetBoardXY[0] + pair[0];
            int j = FirstRealBlockTargetBoardXY[1] + pair[1];
            Data.SetBoardReady(i, j);
            GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
            Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
            TempBoardImage.sprite = DragColorSprite;
            TempBoardImage.color = new Color(1, 1, 1, 0.4f);
        }

        List<int> i_ReadlyIndex;
        List<int> j_ReadlyIndex;
        
        Data.GetReadyRowAndColumn(out i_ReadlyIndex, out j_ReadlyIndex);
        foreach (int i in i_ReadlyIndex)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                Sprite color = TempBoardImage.sprite;
                int[] pair = new int[2] { i, j };
                OriginColorSprite.Add(pair, color);

                TempBoardImage.sprite = DragColorSprite;
            }
        }

        foreach (int j in j_ReadlyIndex)
        {
            for (int i = 0; i < CheckBoardSize; i++)
            {
                GameObject TempBoardImageGo = transform.Find(string.Format("BoardSingleImage_{0}_{1}", i, j)).gameObject;
                Image TempBoardImage = TempBoardImageGo.GetComponent<Image>();
                Sprite color = TempBoardImage.sprite;
                int[] pair = new int[2] { i, j };
                OriginColorSprite.Add(pair, color);

                TempBoardImage.sprite = DragColorSprite;
            }
        }
    }

    void __SetupBoard()
    {
        GameObject image = transform.Find("Image").gameObject;
        Image tempimage = image.GetComponent<Image>();
        
        OriginColor = tempimage.color;
        image.SetActive(false);

        for (int i = 0; i < CheckBoardSize; i++)
        {
            for (int j = 0; j < CheckBoardSize; j++)
            {
                GameObject TempImage = Instantiate(image);
                float size = EffectConfig.RealBlockSize;
                TempImage.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);

                TempImage.SetActive(true);
                TempImage.transform.localPosition = new Vector3(-size * 3 - size / 2 + size * i + i, size * 3 + size / 2 - size * j - j, 0);
                TempImage.transform.SetParent(transform, false);
                TempImage.name = string.Format("BoardSingleImage_{0}_{1}", i, j);

                Data.ClearBoardValue(i, j);
                BoardPosition[i][j] = TempImage.transform.position;
            }
        }
    }

    
}
