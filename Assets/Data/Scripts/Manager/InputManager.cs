using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terresquall;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get => instance; }

    private Vector3 mousePos;
    public Vector3 MousePos { get => mousePos; }

    private Vector2 moveDir;
    public Vector2 MoveDir { get => moveDir; }

    private Vector2 mobileMoveDir;
    public Vector2 MobileMoveDir { get => mobileMoveDir; }

    private bool onLeftClick = false;
    public bool OnLeftClick { get => onLeftClick; }

    private bool onRightClick = false;
    public bool OnRightClick { get => onRightClick; }

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        // GetLeftClick();
        // GetRightClick();
    }
    private void FixedUpdate()
    {
        //GetMousePos();
        //MoveInput();
        MobileInput();
    }

    protected virtual void GetMousePos()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    protected virtual Vector2 MoveInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        return moveDir = new Vector2(moveX, moveY).normalized;
    }

    protected virtual Vector2 MobileInput()
    {
        float moveX, moveY;
        if (VirtualJoystick.CountActiveInstances() > 0)
        {
            moveX = VirtualJoystick.GetAxisRaw("horizontal");
            moveY = VirtualJoystick.GetAxisRaw("vertical");
        }
        else
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
        }

        return mobileMoveDir = new Vector2(moveX, moveY).normalized;
    }

    protected virtual void GetRightClick() // Right mouse click
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            onRightClick = true;
        }
        else
        {
            onRightClick = false;
        }
    }

    protected virtual void GetLeftClick() // Left mouse click
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            onLeftClick = true;
        }
        else
        {
            onLeftClick = false;
        }
    }

}
