using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject[] germPrefabs;
    [SerializeField] private GameObject pillPrefab;

    private readonly int horizontal = 20;
    private readonly int vertical = 24;

    private int[][] board;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DrawBoard();

        GeneratePill();

        //RandomGenerate();
    }

    public int GetBoard(int y, int x)
    {
        return board[y][x];
    }

    public void UpdateBoard(int y, int x, int value)
    {
        board[y][x] = value;
    }

    public void PillSet()
    {
        DestroyGerms();
        GeneratePill();
    }

    private void DestroyGerms()
    {
        var currPill = Player1Controller.Instance.GetCurrentPill();
        //


    }

    private void GeneratePill()
    {
        var newpill = Instantiate(pillPrefab, transform);
        newpill.GetComponent<Pill>().Init(new Vector2Int(10, 20));
        Player1Controller.Instance.SetCurrentPill(newpill);
    }

    private void DrawWalls()
    {
        //left and right vertical walls
        for (int i = 0; i < vertical; i++)
        {
            var leftWall = Instantiate(wallPrefab, transform);
            leftWall.transform.localPosition = new Vector3(0, i, 0);
            UpdateBoard(i, 0, 1);

            var rightWall = Instantiate(wallPrefab, transform);
            rightWall.transform.localPosition = new Vector3(horizontal, i, 0);
            UpdateBoard(i, horizontal, 1);
        }

        //bottom walls
        for (int i = 1; i < horizontal; i++)
        {
            var bottomWall = Instantiate(wallPrefab, transform);
            bottomWall.transform.localPosition = new Vector3(i, 0, 0);
            UpdateBoard(0, i, 1);
        }

    }

    private void RandomGenerate()
    {
        for (int i = 0; i < horizontal; i++)
        {
            for (int j = 0; j < vertical; j++)
            {
                var randomGerm = Random.Range(0, germPrefabs.Length + 1);

                if (randomGerm < germPrefabs.Length)
                {
                    var germ = Instantiate(germPrefabs[randomGerm], transform);
                    germ.transform.localPosition = new Vector3(i, j, 0);
                }
            }
        }
    }

    private void DrawBoard()
    {
        board = new int[vertical + 1][];
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new int[horizontal + 1];
        }

        List<string> newBoard = new List<string>() {
            "W...................W",
            "W...................W",
            "W...................W",
            "W...................W",
            "W...................W",
            "W...................W",
            "W...................W",
            "W...................W",
            "W...................W",
            "W...................W",
            "W...................W",
            "W...R..R............W",
            "W...R..R............W",
            "W...................W",
            "W...YYYY............W",
            "W...................W",
            "W...BBBB............W",
            "W...................W",
            "W...................W",
            "W...................W",
            "W...................W",
            "W...................W",
            "WWWWWWWWWWWWWWWWWWWWW",
        };

        newBoard.Reverse();

        for (int y = 0; y < newBoard.Count; y++)
        {
            for (int x = 0; x < newBoard[y].Length; x++)
            {
                if (newBoard[y][x] == 'W')
                {
                    var wall = Instantiate(wallPrefab, transform);
                    wall.transform.localPosition = new Vector3(x, y, 0);
                    UpdateBoard(y, x, 'W');
                }
                else if (newBoard[y][x] == 'R')
                {
                    var germ = Instantiate(germPrefabs[0], transform);
                    germ.transform.localPosition = new Vector3(x, y, 0);
                    UpdateBoard(y, x, 'R');
                }
                else if (newBoard[y][x] == 'Y')
                {
                    var germ = Instantiate(germPrefabs[1], transform);
                    germ.transform.localPosition = new Vector3(x, y, 0);
                    UpdateBoard(y, x, 'Y');
                }
                else if (newBoard[y][x] == 'B')
                {
                    var germ = Instantiate(germPrefabs[2], transform);
                    germ.transform.localPosition = new Vector3(x, y, 0);
                    UpdateBoard(y, x, 'B');
                }
                else if (newBoard[y][x] == '.')
                {
                    UpdateBoard(y, x, '.');
                }
            }
        }
    }
}
