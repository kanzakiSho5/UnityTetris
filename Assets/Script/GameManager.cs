using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Transform Field;

    const int FIELD_WHIDTH = 10;
    const int FIELD_HIGHT  = 20;

    readonly Block[] Blocks =
    {
        new Block(1, new Pos[3]{ new Pos( 0, 0), new Pos( 0, 0), new Pos( 0, 0) }, Color.white),    // null
        new Block(2, new Pos[3]{ new Pos( 0, 1), new Pos( 0,-1), new Pos( 0,-2) }, Color.cyan),     // I
        new Block(1, new Pos[3]{ new Pos( 1, 0), new Pos( 0, 1), new Pos( 1, 1) }, Color.yellow),   // O
        new Block(2, new Pos[3]{ new Pos(-1, 0), new Pos( 0, 1), new Pos( 1, 1) }, Color.green),    // S
        new Block(2, new Pos[3]{ new Pos( 1, 0), new Pos( 0, 1), new Pos(-1, 1) }, Color.red),      // Z
        new Block(4, new Pos[3]{ new Pos( 0, 1), new Pos( 1, 0), new Pos( 2, 0) }, Color.blue),     // J
        new Block(4, new Pos[3]{ new Pos( 0, 1), new Pos(-1, 0), new Pos(-2, 0) }, new Color(   1f, 0.5f, 0f)), // L
        new Block(4, new Pos[3]{ new Pos( 0,-1), new Pos(-1, 0), new Pos( 1, 0) }, new Color( 0.5f,   0f, 1f))  // T
    };

    private Image[,] FieldCellImgs = new Image[20, 10];
    private int[,]   FieldCells    = new int[25, 12];
    private Status CurrentStatus = new Status();
    private int TimeCount = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        init();
    }

    private void init()
    {
        for(int i = 0; i < FieldCells.GetLength(0);i++)
        {
            for(int j = 0; j < FieldCells.GetLength(1); j++)
            {
                FieldCells[i, j] = 0;
                if (i == 0 || j == 0 || j == 11)
                    FieldCells[i, j] = -1;
            }
        }



        for(int i = 0; i < FIELD_HIGHT; i++)
        {
            for(int j = 0; j < FIELD_WHIDTH; j++)
            {

                FieldCellImgs[i, j] = Instantiate(Resources.Load<GameObject>("Prefabs/Cell"), Field).GetComponent<Image>();
                FieldCellImgs[i, j].rectTransform.anchoredPosition = new Vector2(50 * j, 50 * i);
            }
        }
        CurrentStatus = new Status(5, 21, Random.Range(1, 8),0);
        PutBlock(CurrentStatus);
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (Input.anyKey)
        {
            InputKey();
        }

        //Debug
        {
            Debug.Log(
                FieldCells[19, 1] + "," + FieldCells[19, 2] + "," + FieldCells[19, 3] + "," + FieldCells[19, 4] + "," + FieldCells[19, 5] + "," + FieldCells[19, 6] + "," + FieldCells[19, 7] + "," + FieldCells[19, 8] + "," + FieldCells[19, 9] + "," + FieldCells[19, 10] + "," + FieldCells[19, 11] + ",\n" +
                FieldCells[18, 1] + "," + FieldCells[18, 2] + "," + FieldCells[18, 3] + "," + FieldCells[18, 4] + "," + FieldCells[18, 5] + "," + FieldCells[18, 6] + "," + FieldCells[18, 7] + "," + FieldCells[18, 8] + "," + FieldCells[18, 9] + "," + FieldCells[18, 10] + ",\n" +
                FieldCells[17, 1] + "," + FieldCells[17, 2] + "," + FieldCells[17, 3] + "," + FieldCells[17, 4] + "," + FieldCells[17, 5] + "," + FieldCells[17, 6] + "," + FieldCells[17, 7] + "," + FieldCells[17, 8] + "," + FieldCells[17, 9] + "," + FieldCells[17, 10] + ",\n" +
                FieldCells[16, 1] + "," + FieldCells[16, 2] + "," + FieldCells[16, 3] + "," + FieldCells[16, 4] + "," + FieldCells[16, 5] + "," + FieldCells[16, 6] + "," + FieldCells[16, 7] + "," + FieldCells[16, 8] + "," + FieldCells[16, 9] + "," + FieldCells[16, 10] + ",\n" +
                FieldCells[15, 1] + "," + FieldCells[15, 2] + "," + FieldCells[15, 3] + "," + FieldCells[15, 4] + "," + FieldCells[15, 5] + "," + FieldCells[15, 6] + "," + FieldCells[15, 7] + "," + FieldCells[15, 8] + "," + FieldCells[15, 9] + "," + FieldCells[15, 10] + ",\n" +
                FieldCells[14, 1] + "," + FieldCells[14, 2] + "," + FieldCells[14, 3] + "," + FieldCells[14, 4] + "," + FieldCells[14, 5] + "," + FieldCells[14, 6] + "," + FieldCells[14, 7] + "," + FieldCells[14, 8] + "," + FieldCells[14, 9] + "," + FieldCells[14, 10] + ",\n" +
                FieldCells[13, 1] + "," + FieldCells[13, 2] + "," + FieldCells[13, 3] + "," + FieldCells[13, 4] + "," + FieldCells[13, 5] + "," + FieldCells[13, 6] + "," + FieldCells[13, 7] + "," + FieldCells[13, 8] + "," + FieldCells[13, 9] + "," + FieldCells[13, 10] + ",\n" +
                FieldCells[12, 1] + "," + FieldCells[12, 2] + "," + FieldCells[12, 3] + "," + FieldCells[12, 4] + "," + FieldCells[12, 5] + "," + FieldCells[12, 6] + "," + FieldCells[12, 7] + "," + FieldCells[12, 8] + "," + FieldCells[12, 9] + "," + FieldCells[12, 10] + ",\n" +
                FieldCells[11, 1] + "," + FieldCells[11, 2] + "," + FieldCells[11, 3] + "," + FieldCells[11, 4] + "," + FieldCells[11, 5] + "," + FieldCells[11, 6] + "," + FieldCells[11, 7] + "," + FieldCells[11, 8] + "," + FieldCells[11, 9] + "," + FieldCells[11, 10] + ",\n" +
                FieldCells[10, 1] + "," + FieldCells[10, 2] + "," + FieldCells[10, 3] + "," + FieldCells[10, 4] + "," + FieldCells[10, 5] + "," + FieldCells[10, 6] + "," + FieldCells[10, 7] + "," + FieldCells[10, 8] + "," + FieldCells[10, 9] + "," + FieldCells[10, 10] + ",\n" +
                FieldCells[9, 1] + "," + FieldCells[9, 2] + "," + FieldCells[9, 3] + "," + FieldCells[9, 4] + "," + FieldCells[9, 5] + "," + FieldCells[9, 6] + "," + FieldCells[9, 7] + "," + FieldCells[9, 8] + "," + FieldCells[9, 9] + "," + FieldCells[9, 10] + ",\n" +
                FieldCells[8, 1] + "," + FieldCells[8, 2] + "," + FieldCells[8, 3] + "," + FieldCells[8, 4] + "," + FieldCells[8, 5] + "," + FieldCells[8, 6] + "," + FieldCells[8, 7] + "," + FieldCells[8, 8] + "," + FieldCells[8, 9] + "," + FieldCells[8, 10] + ",\n" +
                FieldCells[7, 1] + "," + FieldCells[7, 2] + "," + FieldCells[7, 3] + "," + FieldCells[7, 4] + "," + FieldCells[7, 5] + "," + FieldCells[7, 6] + "," + FieldCells[7, 7] + "," + FieldCells[7, 8] + "," + FieldCells[7, 9] + "," + FieldCells[7, 10] + ",\n" +
                FieldCells[6, 1] + "," + FieldCells[6, 2] + "," + FieldCells[6, 3] + "," + FieldCells[6, 4] + "," + FieldCells[6, 5] + "," + FieldCells[6, 6] + "," + FieldCells[6, 7] + "," + FieldCells[6, 8] + "," + FieldCells[6, 9] + "," + FieldCells[6, 10] + ",\n" +
                FieldCells[5, 1] + "," + FieldCells[5, 2] + "," + FieldCells[5, 3] + "," + FieldCells[5, 4] + "," + FieldCells[5, 5] + "," + FieldCells[5, 6] + "," + FieldCells[5, 7] + "," + FieldCells[5, 8] + "," + FieldCells[5, 9] + "," + FieldCells[5, 10] + ",\n" +
                FieldCells[4, 1] + "," + FieldCells[4, 2] + "," + FieldCells[4, 3] + "," + FieldCells[4, 4] + "," + FieldCells[4, 5] + "," + FieldCells[4, 6] + "," + FieldCells[4, 7] + "," + FieldCells[4, 8] + "," + FieldCells[4, 9] + "," + FieldCells[4, 10] + ",\n" +
                FieldCells[3, 1] + "," + FieldCells[3, 2] + "," + FieldCells[3, 3] + "," + FieldCells[3, 4] + "," + FieldCells[3, 5] + "," + FieldCells[3, 6] + "," + FieldCells[3, 7] + "," + FieldCells[3, 8] + "," + FieldCells[3, 9] + "," + FieldCells[3, 10] + ",\n" +
                FieldCells[2, 1] + "," + FieldCells[2, 2] + "," + FieldCells[2, 3] + "," + FieldCells[2, 4] + "," + FieldCells[2, 5] + "," + FieldCells[2, 6] + "," + FieldCells[2, 7] + "," + FieldCells[2, 8] + "," + FieldCells[2, 9] + "," + FieldCells[2, 10] + ",\n" +
                FieldCells[1, 1] + "," + FieldCells[1, 2] + "," + FieldCells[1, 3] + "," + FieldCells[1, 4] + "," + FieldCells[1, 5] + "," + FieldCells[1, 6] + "," + FieldCells[1, 7] + "," + FieldCells[1, 8] + "," + FieldCells[1, 9] + "," + FieldCells[1, 10] + ",\n" +
                FieldCells[0, 1] + "," + FieldCells[0, 2] + "," + FieldCells[0, 3] + "," + FieldCells[0, 4] + "," + FieldCells[0, 5] + "," + FieldCells[0, 6] + "," + FieldCells[0, 7] + "," + FieldCells[0, 8] + "," + FieldCells[0, 9] + "," + FieldCells[0, 10] + ",\n"
            );
        }

        for (int i = 0; i < FIELD_HIGHT; i++)
        {
            for (int j = 0; j < FIELD_WHIDTH; j++)
            {
                FieldCellImgs[i, j].color = Blocks[FieldCells[i + 1, j + 1]].color;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isGameOver)
            return;
        TimeCount++;
        if(TimeCount % 30 == 0)
        {
            DownBlock();
        }

    }

    private void InputKey()
    {
        Status n = new Status(CurrentStatus.x, CurrentStatus.y, CurrentStatus.type, CurrentStatus.rotate);
        if(Input.GetKey(KeyCode.S))
        {
            n.y--;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            n.x--;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            n.x++;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            n.rotate++;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {

        }

        if (n.x != CurrentStatus.x || n.y != CurrentStatus.y || n.rotate != CurrentStatus.rotate)
        {
            deleteBlock(CurrentStatus);
            if(PutBlock(n))
            {
                CurrentStatus = n;
            }
            else
            {
                PutBlock(CurrentStatus);
            }
        }
    }

    private bool CreateBlock()
    {
        CurrentStatus = new Status(5, 21, Random.Range(1, 8), 0);
        return PutBlock(CurrentStatus);
    }

    private void DownBlock()
    {
        deleteBlock(CurrentStatus);
        CurrentStatus.y--;
        if(!PutBlock(CurrentStatus))
        {
            CurrentStatus.y++;
            PutBlock(CurrentStatus);
            if(!CreateBlock())
            {
                StartCoroutine(GameOver());
            }
        }
    }

    private IEnumerator GameOver()
    {
        isGameOver = true;
        for (int i = 0; i < FIELD_HIGHT; i++)
        {
            yield return new WaitForSeconds(0.1f);
            for (int j = 0; j < FIELD_WHIDTH; j++)
            {
                FieldCellImgs[i, j].color = Color.red;
            }
        }

        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        for (int i = FIELD_HIGHT - 1; i >= 0 ; i--)
        {
            yield return new WaitForSeconds(0.1f);
            for (int j = 0; j < FIELD_WHIDTH; j++)
            {
                FieldCellImgs[i, j].color = Color.white;
            }
        }

        isGameOver = false;
        init();
    }

    private bool PutBlock(Status status, bool action = false)
    {
        if (FieldCells[status.y, status.x] != 0)
        {
            return false;
        }

        if (action)
        {
            FieldCells[status.y, status.x] = status.type;
        }

        for (int i = 0; i < 3; i++)
        {
            int dx = Blocks[status.type].cellPos[i].x;
            int dy = Blocks[status.type].cellPos[i].y;
            int r = status.rotate % Blocks[status.type].rotate;
            for(int j = 0; j < r; j++)
            {
                //　90度回転させる
                int nx = dx, ny = dy;
                dx = ny;
                dy = -nx;
            }
            if(FieldCells[status.y + dy,status.x + dx] != 0)
            {
                return false;
            }
            if(action)
            {
                FieldCells[status.y + dy, status.x + dx] = status.type;
            }
        }
        if(!action)
        {
            PutBlock(status, true);
        }
        return true;
    }

    private bool deleteBlock(Status status)
    {
        FieldCells[status.y, status.x] = 0;
        for (int i = 0; i < 3; i++)
        {
            int dx = Blocks[status.type].cellPos[i].x;
            int dy = Blocks[status.type].cellPos[i].y;
            int r = status.rotate % Blocks[status.type].rotate;
            for (int j = 0; j < r; j++)
            {
                //　90度回転させる
                int nx = dx, ny = dy;
                dx = ny;
                dy = -nx;
            }
            FieldCells[status.y + dy, status.x + dx] = 0;
            
        }
        return true;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}


public class Block
{
    public int rotate;
    public Pos[] cellPos;
    public Color color;
    
    public Block(int rotate, Pos[] cellPos, Color color)
    {
        this.rotate = rotate;
        this.cellPos = cellPos;
        this.color = color;
    }
}


public class Pos
{
    public int x;
    public int y;

    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Status
{
    public int x;
    public int y;
    public int type;
    public int rotate;

    public Status(int x, int y, int type, int rotate)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        this.rotate = rotate;
    }

    public Status()
    {
        this.x = 0;
        this.y = 0;
        this.type = 0;
        this.rotate = 0;
    }
}
