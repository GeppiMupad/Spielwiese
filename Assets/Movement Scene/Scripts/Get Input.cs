using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetInput : MonoBehaviour
{
    public Vector2 walk;

    public Vector2 rotation;

    public bool isSprinting = false;

    public bool isJumping = false;

    public bool isInteracting = false;


    public void OnMove(InputAction.CallbackContext _context)
    {
        walk = _context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext _context)
    {
        if(_context.performed)
        {
            isSprinting = true;
        }
        
        if(_context.canceled)
        {
            isSprinting = false;
        }
    }

    public void OnJump(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            isJumping = true;
        }

        if (_context.canceled)
        {
            isJumping = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            isInteracting = true;
        }

        if (_context.canceled)
        {
            isInteracting = false;
        }
    }

    public void OnRotation(InputAction.CallbackContext _context)
    {
        rotation += _context.ReadValue<Vector2>();
    }
}
