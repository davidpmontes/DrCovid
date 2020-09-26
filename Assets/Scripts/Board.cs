using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    private Dictionary<Vector2Int, GameObject> items = new Dictionary<Vector2Int, GameObject>();

    [SerializeField] private GameObject[] germPrefabs;
    [SerializeField] private GameObject[] debrisPrefabs;
    [SerializeField] private GameObject pillPrefab;

    private readonly int RED_GERM = 0;
    private readonly int YELLOW_GERM = 1;
    private readonly int BLUE_GERM = 2;

    private readonly int RED_DEBRIS = 0;
    private readonly int YELLOW_DEBRIS = 1;
    private readonly int BLUE_DEBRIS = 2;

    private readonly int width = 10;
    private readonly int height = 17;

    public readonly int horizontalOffset = -5;
    public readonly int verticalOffset = 4;
    public readonly float scale = 0.0625f;

    private char[][] board;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DrawBoard();
        GeneratePill();
    }

    public int GetBoard(int x, int y)
    {
        return board[y][x];
    }

    public void UpdateBoard(int x, int y, char value)
    {
        board[y][x] = value;
    }

    public void DebrisLanded(Vector2Int pos, string color)
    {

    }

    /// <summary>
    /// Pill has landed on ground
    /// </summary>
    public void PillLanded(GameObject pill, Vector2Int posA, Vector2Int posB, char colorA, char colorB)
    {
        Player1Controller.Instance.SetCurrentPill(null);

        int debrisColorA = 0;
        if (colorA == 'r') debrisColorA = 0;
        if (colorA == 'y') debrisColorA = 1;
        if (colorA == 'b') debrisColorA = 2;

        int debrisColorB = 0;
        if (colorB == 'r') debrisColorB = 0;
        if (colorB == 'y') debrisColorB = 1;
        if (colorB == 'b') debrisColorB = 2;

        GameObject debrisA = Instantiate(debrisPrefabs[debrisColorA], null);
        debrisA.GetComponent<Debris>().Init(posA);
        items[posA] = debrisA;

        GameObject debrisB = Instantiate(debrisPrefabs[debrisColorB], null);
        debrisB.GetComponent<Debris>().Init(posB);
        items[posB] = debrisB;

        Destroy(pill);

        HashSet<Vector2Int> destroyables = new HashSet<Vector2Int>();
        Scan(debrisA, posA, colorA, ref destroyables);
        Scan(debrisB, posB, colorB, ref destroyables);

        foreach(var destroyable in destroyables)
        {
            board[destroyable.y][destroyable.x] = '-';
            Destroy(items[destroyable]);
            items[destroyable] = null;
        }

        GeneratePill();
    }

    private void Scan(GameObject debris, Vector2Int pos, char color, ref HashSet<Vector2Int> destroyables)
    {
        for (int x = pos.x - 1; x > 0; x--)
        {
            if (items.ContainsKey(new Vector2Int(x, pos.y)) && board[pos.y][x] == char.ToUpper(color))
            {
                destroyables.Add(pos);
                destroyables.Add(new Vector2Int(x, pos.y));
            }
            else
            {
                break;
            }
        }

        for (int x = pos.x + 1; x < width; x++)
        {
            if (items.ContainsKey(new Vector2Int(x, pos.y)) && board[pos.y][x] == char.ToUpper(color))
            {
                destroyables.Add(pos);
                destroyables.Add(new Vector2Int(x, pos.y));
            }
            else
            {
                break;
            }
        }

        for (int y = pos.y + 1; y < height; y++)
        {
            if (items.ContainsKey(new Vector2Int(pos.x, y)) && board[y][pos.x] == char.ToUpper(color))
            {
                destroyables.Add(pos);
                destroyables.Add(new Vector2Int(pos.x, y));
            }
            else
            {
                break;
            }
        }

        for (int y = pos.y - 1; y > 0; y--)
        {
            if (items.ContainsKey(new Vector2Int(pos.x, y)) && board[y][pos.x] == char.ToUpper(color))
            {
                destroyables.Add(pos);
                destroyables.Add(new Vector2Int(pos.x, y));
            }
            else
            {
                break;
            }
        }
    }

    public void GeneratePill(string type = null)
    {
        if (type == null)
        {
            string[] types = new string[] { "red", "blue", "yellow", "blueYellow", "redBlue", "redYellow" };
            type = types[Random.Range(0, types.Length)];
        }

        var newpill = Instantiate(pillPrefab, null);
        newpill.GetComponent<Pill>().Init(new Vector2Int(4, 15), type);
        Player1Controller.Instance.SetCurrentPill(newpill);
    }

    private void RandomGenerate()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
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

    public void PrintBoard()
    {
        string boardString = "";

        for (int i = board.Length - 1; i >= 0; i--)
        {
            foreach (char letter in board[i])
            {
                boardString += letter.ToString();
            }
            boardString += "\n";
        }

        Debug.Log(boardString);
    }

    private void DrawBoard()
    {
        board = new char[height][];
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new char[width];
        }

        List<string> newBoard = new List<string>() {
            "----------",
            "WR------RW",
            "W--------W",
            "W--------W",
            "W--------W",
            "W--------W",
            "W--------W",
            "W--------W",
            "W--------W",
            "W--------W",
            "W--------W",
            "W--------W",
            "W--------W",
            "W--------W",
            "W--------W",
            "WRBYRBYRBW",
            "WWWWWWWWWW",
        };

        newBoard.Reverse();

        for (int y = 0; y < newBoard.Count; y++)
        {
            for (int x = 0; x < newBoard[y].Length; x++)
            {
                if (newBoard[y][x] == 'W')
                {
                    UpdateBoard(x, y, 'W');
                }
                else if (newBoard[y][x] == 'R')
                {
                    var germ = Instantiate(germPrefabs[RED_GERM], transform);
                    germ.transform.localPosition = new Vector3(x + horizontalOffset, y + verticalOffset, 0) * scale;
                    items[new Vector2Int(x, y)] = germ;
                    UpdateBoard(x, y, 'R');
                }
                else if (newBoard[y][x] == 'Y')
                {
                    var germ = Instantiate(germPrefabs[YELLOW_GERM], transform);
                    germ.transform.localPosition = new Vector3(x + horizontalOffset, y + verticalOffset, 0) * scale;
                    items[new Vector2Int(x, y)] = germ;
                    UpdateBoard(x, y, 'Y');
                }
                else if (newBoard[y][x] == 'B')
                {
                    var germ = Instantiate(germPrefabs[BLUE_GERM], transform);
                    germ.transform.localPosition = new Vector3(x + horizontalOffset, y + verticalOffset, 0) * scale;
                    items[new Vector2Int(x, y)] = germ;
                    UpdateBoard(x, y, 'B');
                }
                else if (newBoard[y][x] == '-')
                {
                    UpdateBoard(x, y, '-');
                }
            }
        }
    }
}
