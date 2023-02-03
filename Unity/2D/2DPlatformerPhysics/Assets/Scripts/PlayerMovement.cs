using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d = null;
    [SerializeField] private Transform leftSide = null;
    [SerializeField] private Transform rightSide = null;
    [SerializeField] private GameObject firstStepParticle = null;

    
    // Jump state variables
    [SerializeField] private float jumpForce = 10f;
    private bool isJumping = false;
    private float lastGroundedTime = Mathf.Infinity;
    private float lastJumpTime = Mathf.Infinity;

    // Run state variables
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float timeSinceMovementStarted = 0f;
    private float accelerationSpeed = 2f;
    private float deccelerationSpeed = 1f;
    private float timeBeforeFirstStep = 0.125f;
    private float firstStepAcceleration = 7.5f;
    private bool hasDoneFirstStep = false;
    
    Vector2 movementDirection;

    PlayerInputActions playerInputActions; 

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += Jump;

        playerInputActions.Player.Movement.started += OnMovementStart;
        playerInputActions.Player.Movement.performed += MovementHandler;
    }

    private void OnMovementStart(InputAction.CallbackContext context)
    {
        if (!Mathf.Approximately(rb2d.velocity.x, 0f)) { return; }

        timeSinceMovementStarted = 0f;
        hasDoneFirstStep = false;

    }

    private void Update() {
        
    }

    private void FixedUpdate() {

        ForceBased2DMovement();

    }


    /* Horizontal movement handled by basic forces */
    private void ForceBased2DMovement()
    {
        FirstStepAcceleration();
        if (!hasDoneFirstStep) { return; }

        float maxSpeed = movementSpeed * movementDirection.x;
        float speedDiff = maxSpeed - rb2d.velocity.x;
        float accelerationRate = (Mathf.Abs(maxSpeed) > 0.01f)? accelerationSpeed : deccelerationSpeed;
        float fixedMovement = Mathf.Pow(Mathf.Abs(speedDiff) * accelerationRate, 2f) * Mathf.Sign(speedDiff);

        rb2d.AddForce(fixedMovement * Vector2.right);

        // rb2d.AddForce(new Vector2(movementDirection.x,0) * movementSpeed, ForceMode2D.Force);
    }

    private void FirstStepAcceleration()
    {
        timeSinceMovementStarted += Time.deltaTime;

        // Add 'First Step Acceleration' after brief pause
        if (!hasDoneFirstStep && (timeSinceMovementStarted > timeBeforeFirstStep))
        {
            Transform instantiatePosition = (movementDirection.x > 0f)? rightSide : leftSide;
            Instantiate(firstStepParticle, instantiatePosition.position, Quaternion.identity);

            rb2d.AddForce(new Vector2(firstStepAcceleration * movementDirection.x, 0), ForceMode2D.Impulse);
            hasDoneFirstStep = true;
        }
        
    }

    /* Fixed movement without forces (acceleration, etc) */
    private void Fixed2DMovement()
    {
        rb2d.velocity = new Vector2(movementDirection.x * movementSpeed, rb2d.velocity.y );
    }

    private void MovementHandler(InputAction.CallbackContext context)
    {
        movementDirection =  context.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        // TODO: Make jump system -> (add cooldown between jumps and overlapping jump resistance)
    }

}
