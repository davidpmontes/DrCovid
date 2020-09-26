using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Controller : MonoBehaviour
{
    public static Player1Controller Instance { get; private set; }

    private GameObject currentPill;

    private PlayerInput playerInput;
    private Vector2 moveInput;

    private float downHoldTime;
    private float leftHoldTime;
    private float rightHoldTime;

    private const float holdTime = 0.25f;
    private const float moveDelay = 0.1f;

    private void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        GetInput();
        VerticalMovement();
        HorizontalMovement();
    }

    public void SetCurrentPill(GameObject newPill)
    {
        currentPill = newPill;
    }

    private void GetInput()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    private void VerticalMovement()
    {
        if (currentPill == null)
            return;

        if (moveInput.y == 0)
        {
            downHoldTime = 0;
        }
        else if (moveInput.y == -1)
        {
            // first press
            if (downHoldTime == 0f)
            {
                currentPill.GetComponent<Pill>().MoveDown(true);
                downHoldTime -= Time.deltaTime;
            }
            // hold for 0.5 seconds 
            else if (downHoldTime < 0f)
            {
                downHoldTime -= Time.deltaTime;

                if (downHoldTime <= -holdTime)
                {
                    currentPill.GetComponent<Pill>().MoveDown(true);
                    downHoldTime = Time.deltaTime;
                }
            }
            else // holding, moves every 0.1f seconds
            {
                downHoldTime += Time.deltaTime;

                if (downHoldTime > moveDelay + Time.deltaTime)
                {
                    currentPill.GetComponent<Pill>().MoveDown(true);
                    downHoldTime = Time.deltaTime;
                }
            }
        }
    }

    private void HorizontalMovement()
    {
        if (currentPill == null)
            return;

        if (moveInput.x == 0)
        {
            leftHoldTime = 0f;
            rightHoldTime = 0f;
        }
        else if (moveInput.x == -1)
        {
            // first press
            if (leftHoldTime == 0f)
            {
                currentPill.GetComponent<Pill>().MoveLeft();
                leftHoldTime -= Time.deltaTime;
            }
            // hold for holdtime seconds 
            else if (leftHoldTime < 0f)
            {
                leftHoldTime -= Time.deltaTime;

                if (leftHoldTime <= -holdTime)
                {
                    currentPill.GetComponent<Pill>().MoveLeft();
                    leftHoldTime = Time.deltaTime;
                }
            }
            else // holding, moves every 0.1f seconds
            {
                leftHoldTime += Time.deltaTime;

                if (leftHoldTime > moveDelay + Time.deltaTime)
                {
                    currentPill.GetComponent<Pill>().MoveLeft();
                    leftHoldTime = Time.deltaTime;
                }
            }
        }
        else if (moveInput.x == 1)
        {
            // first press
            if (rightHoldTime == 0f)
            {
                currentPill.GetComponent<Pill>().MoveRight();
                rightHoldTime -= Time.deltaTime;
            }
            // hold for holdtime seconds 
            else if (rightHoldTime < 0f)
            {
                rightHoldTime -= Time.deltaTime;

                if (rightHoldTime <= -holdTime)
                {
                    currentPill.GetComponent<Pill>().MoveRight();
                    rightHoldTime = Time.deltaTime;
                }
            }
            else // holding, moves every 0.1f seconds
            {
                rightHoldTime += Time.deltaTime;

                if (rightHoldTime > moveDelay + Time.deltaTime)
                {
                    currentPill.GetComponent<Pill>().MoveRight();
                    rightHoldTime = Time.deltaTime;
                }
            }

        }
    }

    public void OnRotateCW()
    {
        if (currentPill == null)
            return;

        currentPill.GetComponent<Pill>().Rotate(-1);
    }

    public void OnRotateCC()
    {
        if (currentPill == null)
            return;

        currentPill.GetComponent<Pill>().Rotate(1);
    }
}
