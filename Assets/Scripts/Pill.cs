using UnityEngine;

public enum Rotation
{
    Zero, Ninety, OneEighty, TwoSeventy
}

public class Pill : MonoBehaviour
{
    [SerializeField] private GameObject[] rotations;
    [SerializeField] private Sprite red;
    [SerializeField] private Sprite blue;
    [SerializeField] private Sprite yellow;
    [SerializeField] private Sprite blueYellow;
    [SerializeField] private Sprite redBlue;
    [SerializeField] private Sprite redYellow;

    private int currIdx = 0;
    private Vector2Int pos;

    private float dropTimer;
    private const char EMPTY = '-';
    private char colorA;
    private char colorB;

    public void Init(Vector2Int startPos, string type)
    {
        pos = startPos;
        transform.localPosition = new Vector3(pos.x + Board.Instance.horizontalOffset, pos.y + Board.Instance.verticalOffset, 0) * Board.Instance.scale;

        Sprite pillColor = null;

        switch(type)
        {
            case "red": pillColor = red; colorA = 'r'; colorB = 'r'; break;
            case "blue": pillColor = blue; colorA = 'b'; colorB = 'b'; break;
            case "yellow": pillColor = yellow; colorA = 'y'; colorB = 'y'; break;
            case "blueYellow": pillColor = blueYellow; colorA = 'b'; colorB = 'y'; break;
            case "redBlue": pillColor = redBlue; colorA = 'r'; colorB = 'b'; break;
            case "redYellow": pillColor = redYellow; colorA = 'r'; colorB = 'y'; break;
            default:
                break;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = pillColor;
        }
    }

    private void Update()
    {
        dropTimer += Time.deltaTime;
        if (dropTimer >= 1)
        {
            dropTimer = 0;
            MoveDown(false);
        }
    }

    public void MoveLeft()
    {
        switch ((Rotation)currIdx)
        {
            case Rotation.Zero:
            case Rotation.OneEighty:
                if (Board.Instance.GetBoard(pos.x - 1, pos.y) != EMPTY)
                {
                    return;
                }
                break;
            case Rotation.Ninety:
            case Rotation.TwoSeventy:
                if (Board.Instance.GetBoard(pos.x - 1, pos.y) != EMPTY || Board.Instance.GetBoard(pos.x - 1, pos.y + 1) != EMPTY)
                {
                    return;
                }
                break;
            default:
                break;
        }

        pos.x -= 1;
        transform.Translate(Vector3.left * Board.Instance.scale);
    }

    public void MoveRight()
    {
        switch ((Rotation)currIdx)
        {
            case Rotation.Zero:
            case Rotation.OneEighty:
                if (Board.Instance.GetBoard(pos.x + 2, pos.y) != EMPTY)
                {
                    return;
                }
                break;
            case Rotation.Ninety:
            case Rotation.TwoSeventy:
                if (Board.Instance.GetBoard(pos.x + 1, pos.y) != EMPTY || Board.Instance.GetBoard(pos.x + 1, pos.y + 1) != EMPTY)
                {
                    return;
                }
                break;
            default:
                break;
        }

        pos.x += 1;
        transform.Translate(Vector3.right * Board.Instance.scale);
    }

    public void Rotate(int dir)
    {
        switch ((Rotation)currIdx)
        {
            case Rotation.Zero:
            case Rotation.OneEighty:
                // currently horizontal
                // check if area above is clear
                if (Board.Instance.GetBoard(pos.x, pos.y + 1) != EMPTY)
                {
                    // space above is blocked so check if
                    // moving pill down one slot first will make it work
                    if (Board.Instance.GetBoard(pos.x, pos.y - 1) != EMPTY)
                    {
                        return;
                    }

                    pos.y -= 1;
                    transform.Translate(Vector3.down * Board.Instance.scale);
                }

                break;
            case Rotation.Ninety:
            case Rotation.TwoSeventy:
                // currently vertical
                // check if area to right is clear
                if (Board.Instance.GetBoard(pos.x + 1, pos.y) != EMPTY)
                {
                    // space to right is blocked so check if
                    // moving pill left one slot first will make it work
                    if (Board.Instance.GetBoard(pos.x - 1, pos.y) != EMPTY)
                    {
                        return;
                    }

                    pos.x -= 1;
                    transform.Translate(Vector3.left * Board.Instance.scale);
                }
                break;
            default:
                break;
        }

        rotations[currIdx].SetActive(false);
        currIdx += dir;
        if (currIdx > 3) currIdx = 0;
        if (currIdx < 0) currIdx = 3;
        rotations[currIdx].SetActive(true);
    }

    public void MoveDown(bool fromPlayerInput)
    {
        if (fromPlayerInput)
            dropTimer = 0f;

        switch ((Rotation)currIdx)
        {
            case Rotation.Zero:
                if (Board.Instance.GetBoard(pos.x, pos.y - 1) != EMPTY || Board.Instance.GetBoard(pos.x + 1, pos.y - 1) != EMPTY)
                {
                    Board.Instance.UpdateBoard(pos.x, pos.y, colorA);
                    Board.Instance.UpdateBoard(pos.x + 1, pos.y, colorB);
                    Board.Instance.PillLanded(transform.gameObject, pos, new Vector2Int(pos.x + 1, pos.y), colorA, colorB);
                    enabled = false;
                    return;
                }
                break;
            case Rotation.OneEighty:
                if (Board.Instance.GetBoard(pos.x, pos.y - 1) != EMPTY || Board.Instance.GetBoard(pos.x + 1, pos.y - 1) != EMPTY)
                {
                    Board.Instance.UpdateBoard(pos.x, pos.y, colorB);
                    Board.Instance.UpdateBoard(pos.x + 1, pos.y, colorA);
                    Board.Instance.PillLanded(transform.gameObject, pos, new Vector2Int(pos.x + 1, pos.y), colorB, colorA);
                    enabled = false;
                    return;
                }
                break;
            case Rotation.Ninety:
                if (Board.Instance.GetBoard(pos.x, pos.y - 1) != EMPTY)
                {
                    Board.Instance.UpdateBoard(pos.x, pos.y, colorA);
                    Board.Instance.UpdateBoard(pos.x, pos.y + 1, colorB);
                    Board.Instance.PillLanded(transform.gameObject, pos, new Vector2Int(pos.x, pos.y + 1), colorA, colorB);
                    enabled = false;
                    return;
                }
                break;
            case Rotation.TwoSeventy:
                if (Board.Instance.GetBoard(pos.x, pos.y - 1) != EMPTY)
                {
                    Board.Instance.UpdateBoard(pos.x, pos.y, colorB);
                    Board.Instance.UpdateBoard(pos.x, pos.y + 1, colorA);
                    Board.Instance.PillLanded(transform.gameObject, pos, new Vector2Int(pos.x, pos.y + 1), colorB, colorA);
                    enabled = false;
                    return;
                }
                break;
            default:
                break;
        }

        pos.y -= 1;
        transform.Translate(Vector3.down * Board.Instance.scale);
    }
}