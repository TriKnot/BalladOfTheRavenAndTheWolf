using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;


public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _input;

    public Vector2 moveInput{ get; private set; }
    public bool moveIsPressed{ get; private set; }
    

    private void OnEnable()
    {
        _input = new PlayerInput();
        _input.Player.Enable();

            // Subscribe to events
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMove;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        moveInput = obj.ReadValue<Vector2>();
        moveIsPressed = moveInput != Vector2.zero;
    }

    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMove;
    }
}
