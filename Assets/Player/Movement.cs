using UnityEngine;

public class Movement : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 5f; // Default speed
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float crouchSpeed = 2f;

    Vector2 horizontalInput;

    [SerializeField] float jumpHeight = 3.5f;
    bool jump;
    [SerializeField] float gravity = -30f;
    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;

    private bool sprinting = false;
    private bool crouching = false;
    private bool lerpCrouch = false;
    private float crouchTimer = 0f;

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);

        if (isGrounded)
        {
            verticalVelocity.y = 0;
        }

        // Calculate horizontal velocity based on input
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;

        // Apply movement using the CharacterController
        controller.Move(horizontalVelocity * Time.deltaTime);

        // Jumping
        if (jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jump = false;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        // Crouch Logic
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1f, p);
            else
                controller.height = Mathf.Lerp(controller.height, 3f, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    // Method to receive input from InputManager
    public void RecieveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;

        if (crouching)
        {
            controller.height = 1f; // Reduce height when crouching
            speed = crouchSpeed;    // Reduce speed
          
        }
        else
        {
            // Check if there's space above before standing up
            if (!Physics.Raycast(transform.position, Vector3.up, 1.1f, groundMask))
            {
                controller.height = 2f; // Reset height when standing
                speed = 5f;             // Reset speed
                
            }
        }
    }


    public void StartSprint()
    {
        sprinting = true;
        speed = sprintSpeed; // Increase speed when sprinting
    }

    public void StopSprint()
    {
        sprinting = false;
        speed = 5f; // Reset to default speed when sprinting stops
    }

    public void OnJumpPressed()
    {
        jump = true;
    }
}

