using UnityEngine;

public class Debris : MonoBehaviour
{
    private Vector2Int pos;
    private float dropTimer;
    private string colorA = "b";

    private const char EMPTY = '-';

    public void Init(Vector2Int startPos)
    {
        pos = startPos;
        transform.localPosition = new Vector3(pos.x + Board.Instance.horizontalOffset, pos.y + Board.Instance.verticalOffset, 0) * Board.Instance.scale;
    }

    //private void Update()
    //{
    //    dropTimer += Time.deltaTime;
    //    if (dropTimer >= 1)
    //    {
    //        dropTimer = 0;
    //        MoveDown(false);
    //    }
    //}

    public void MoveDown(bool fromPlayerInput)
    {
        if (fromPlayerInput)
            dropTimer = 0f;

        if (Board.Instance.GetBoard(pos.x, pos.y - 1) != EMPTY || Board.Instance.GetBoard(pos.x + 1, pos.y - 1) != EMPTY)
        {
            //Board.Instance.UpdateBoard(pos.x, pos.y, 1);
            Board.Instance.DebrisLanded(pos, colorA);
            enabled = false;
            return;
        }

        pos.y -= 1;
        transform.Translate(Vector3.down * Board.Instance.scale);
    }
}
