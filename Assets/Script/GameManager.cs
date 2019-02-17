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
        new Block(1, new Pos[3]{ new Pos( 0, 0), new Pos( 0, 0), new Pos( 0, 0) }, Color.clear),// null
        new Block(2, new Pos[3]{ new Pos( 0, 1), new Pos( 0,-1), new Pos( 0,-2) }, Color.cyan),// I
        new Block(1, new Pos[3]{ new Pos( 1, 0), new Pos( 0, 1), new Pos( 1, 1) }, Color.yellow),// O
        new Block(2, new Pos[3]{ new Pos(-1, 0), new Pos( 0, 1), new Pos( 1, 1) }, Color.green),// S
        new Block(2, new Pos[3]{ new Pos( 1, 0), new Pos( 0, 1), new Pos(-1, 1) }, Color.red),// Z
        new Block(4, new Pos[3]{ new Pos( 0, 1), new Pos( 1, 0), new Pos( 2, 0) }, Color.blue),// J
        new Block(4, new Pos[3]{ new Pos( 0, 1), new Pos(-1, 0), new Pos(-2, 0) }, new Color(   1f, 0.5f, 0f)),// L
        new Block(4, new Pos[3]{ new Pos( 0,-1), new Pos(-1, 0), new Pos( 1, 0) }, new Color( 0.5f,   0f, 1f)) // T
    };

    private Image[,] FieldCellImgs = new Image[20, 10];
    private int[,]   FieldCells    = new int[20, 10];

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
        for(int i = 0; i < FIELD_HIGHT; i++)
        {
            for(int j = 0; j < FIELD_WHIDTH; j++)
            {
                FieldCellImgs[i, j] = Instantiate(Resources.Load<GameObject>("Prefabs/Cell"), Field).GetComponent<Image>();
                FieldCellImgs[i, j].rectTransform.anchoredPosition = new Vector2(50 * j, 50 * i);
            }
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}


public class Block
{
    public int Rotate;
    public Pos[] CellPos;
    public Color color;
    
    public Block(int rotate, Pos[] cellPos, Color color)
    {
        this.Rotate = rotate;
        this.CellPos = cellPos;
        this.color = color;
    }
}


public class Pos
{
    int x;
    int y;

    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
