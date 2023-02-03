using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d = null;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;
    private bool isJumping = false;
    private float lastGroundedTime = Mathf.Infinity;
    private float lastJumpTime = Mathf.Infinity;

    
    Vector2 movementDirection;

    PlayerInputActions playerInputActions; 

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Movement.performed += HorizontalMovement;
    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        //rb2d.AddForce(new Vector2(movementDirection.x,0) * movementSpeed, ForceMode2D.Force);
        rb2d.velocity = new Vector2(movementDirection.x * movementSpeed, rb2d.velocity.y ) ;
    }

    
    private void HorizontalMovement(InputAction.CallbackContext context)
    {
        movementDirection =  context.ReadValue<Vector2>();

    }

    private void Jump(InputAction.CallbackContext context)
    {
        
    }

}
