using UnityEngine;

public class Pill : MonoBehaviour
{
    [SerializeField] private GameObject[] rotations;
    [SerializeField] private GameObject redDebris;
    [SerializeField] private GameObject blueDebris;
    [SerializeField] private GameObject yellowDebris;
 
    private int currIdx = 0;
    private Vector2Int pos;

    private const int Zero = 0;
    private const int Ninety = 1;
    private const int OneEighty = 2;
    private const int TwoSeventy = 3;

    private float dropTimer;
    private const char EMPTY = '.';

    public void Init(Vector2Int startPos)
    {
        pos = startPos;
        transform.localPosition = new Vector3(pos.x, pos.y, 0);
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
        switch (currIdx)
        {
            case Zero:
            case OneEighty:
                if (Board.Instance.GetBoard(pos.y, pos.x - 1) != EMPTY)
                {
                    return;
                }
                break;
            case Ninety:
            case TwoSeventy:
                if (Board.Instance.GetBoard(pos.y, pos.x - 1) != EMPTY || Board.Instance.GetBoard(pos.y + 1, pos.x - 1) != EMPTY)
                {
                    return;
                }
                break;
            default:
                break;
        }

        pos.x -= 1;
        transform.Translate(Vector3.left);
    }

    public void MoveRight()
    {
        switch (currIdx)
        {
            case Zero:
            case OneEighty:
                if (Board.Instance.GetBoard(pos.y, pos.x + 2) != EMPTY)
                {
                    return;
                }
                break;
            case Ninety:
            case TwoSeventy:
                if (Board.Instance.GetBoard(pos.y, pos.x + 1) != EMPTY || Board.Instance.GetBoard(pos.y + 1, pos.x + 1) != EMPTY)
                {
                    return;
                }
                break;
            default:
                break;
        }

        pos.x += 1;
        transform.Translate(Vector3.right);
    }

    public void Rotate(int dir)
    {
        switch (currIdx)
        {
            case Zero:
            case OneEighty:
                // currently horizontal
                // check if area above is clear
                if (Board.Instance.GetBoard(pos.y + 1, pos.x) != EMPTY)
                {
                    // space above is blocked so check if
                    // moving pill down one slot first will make it work
                    if (Board.Instance.GetBoard(pos.y - 1, pos.x) != EMPTY)
                    {
                        return;
                    }

                    pos.y -= 1;
                    transform.Translate(Vector3.down);
                }

                break;
            case Ninety:
            case TwoSeventy:
                // currently vertical
                // check if area to right is clear
                if (Board.Instance.GetBoard(pos.y, pos.x + 1) != EMPTY)
                {
                    // space to right is blocked so check if
                    // moving pill left one slot first will make it work
                    if (Board.Instance.GetBoard(pos.y, pos.x - 1) != EMPTY)
                    {
                        return;
                    }

                    pos.x -= 1;
                    transform.Translate(Vector3.left);
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

        switch (currIdx)
        {
            case Zero:
            case OneEighty:
                if (Board.Instance.GetBoard(pos.y - 1, pos.x) != EMPTY || Board.Instance.GetBoard(pos.y - 1, pos.x + 1) != EMPTY)
                {
                    Board.Instance.UpdateBoard(pos.y, pos.x, 1);
                    Board.Instance.UpdateBoard(pos.y, pos.x + 1, 1);
                    Board.Instance.PillSet();
                    enabled = false;
                }
                else
                {
                    pos.y -= 1;
                    transform.Translate(Vector3.down);
                }
                break;
            case Ninety:
            case TwoSeventy:
                if (Board.Instance.GetBoard(pos.y - 1, pos.x) != EMPTY)
                {
                    Board.Instance.UpdateBoard(pos.y, pos.x, 1);
                    Board.Instance.UpdateBoard(pos.y + 1, pos.x, 1);
                    Board.Instance.PillSet();
                    enabled = false;
                }
                else
                {
                    pos.y -= 1;
                    transform.Translate(Vector3.down);
                }
                break;
            default:
                break;
        }
    }


}
