using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Controller : MonoBehaviour
{
    public static Player1Controller Instance { get; private set; }

    [SerializeField] private GameObject currentPill;

    private Vector2 moveInput;
    private bool rotateCW;
    private bool rotateCC;

    private float downHoldTime;
    private float leftHoldTime;
    private float rightHoldTime;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        GetInput();
        VerticalMovement();
        HorizontalMovement();
        RotatePill();
    }

    public GameObject GetCurrentPill()
    {
        return currentPill;
    }

    public void SetCurrentPill(GameObject newPill)
    {
        currentPill = newPill;
    }

    private void GetInput()
    {
        moveInput = Gamepad.current.dpad.ReadValue();
        rotateCC = Gamepad.current.buttonSouth.wasPressedThisFrame;
        rotateCW = Gamepad.current.buttonEast.wasPressedThisFrame;
    }

    private void VerticalMovement()
    {
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

                if (downHoldTime <= -0.5f)
                {
                    currentPill.GetComponent<Pill>().MoveDown(true);
                    downHoldTime = Time.deltaTime;
                }
            }
            else // holding, moves every 0.1f seconds
            {
                downHoldTime += Time.deltaTime;

                if (downHoldTime > 0.1f + Time.deltaTime)
                {
                    currentPill.GetComponent<Pill>().MoveDown(true);
                    downHoldTime = Time.deltaTime;
                }
            }
        }
    }

    private void HorizontalMovement()
    {
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
            // hold for 0.5 seconds 
            else if (leftHoldTime < 0f)
            {
                leftHoldTime -= Time.deltaTime;

                if (leftHoldTime <= -0.5f)
                {
                    currentPill.GetComponent<Pill>().MoveLeft();
                    leftHoldTime = Time.deltaTime;
                }
            }
            else // holding, moves every 0.1f seconds
            {
                leftHoldTime += Time.deltaTime;

                if (leftHoldTime > 0.1f + Time.deltaTime)
                {
                    currentPill.GetComponent<Pill>().MoveLeft();
                    leftHoldTime = Time.deltaTime;
                }
            }
        }
        else if (moveInput.x == 1)
        {
            if (rightHoldTime == 0f)
            {
                currentPill.GetComponent<Pill>().MoveRight();
                rightHoldTime -= Time.deltaTime;
            }
            else if (rightHoldTime < 0f)
            {
                rightHoldTime -= Time.deltaTime;

                if (rightHoldTime <= -0.5f)
                {
                    currentPill.GetComponent<Pill>().MoveRight();
                    rightHoldTime = Time.deltaTime;
                }
            }
            else
            {
                rightHoldTime += Time.deltaTime;

                if (rightHoldTime > 0.1f + Time.deltaTime)
                {
                    currentPill.GetComponent<Pill>().MoveRight();
                    rightHoldTime = Time.deltaTime;
                }
            }

        }
    }

    private void RotatePill()
    {
        if (rotateCC)
        {
            currentPill.GetComponent<Pill>().Rotate(1);
        }
        else if (rotateCW)
        {
            currentPill.GetComponent<Pill>().Rotate(-1);
        }
    }
}
