using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private PlayerInputActions PIA;
    private InputAction IA_Movement;
    private InputAction IA_Jump;
    private InputAction IA_Interaction;

    public Rigidbody2D rbPlayer;

    private float movementInput;
    public float movementSpeed;
    public float jumpForce;
    
    public float CastDistance;
    public Vector2 BoxSize;
    public SpriteRenderer spriteRenderer;
    public float rayOffset;
    public float interactionRayDistance;
    private interaction interaction;

    void Awake()
    {
        PIA = new PlayerInputActions();
        IA_Movement = PIA.Walking.Movement;
        IA_Movement.Enable();
        IA_Jump = PIA.Walking.Jump;
        IA_Jump.Enable();
        IA_Jump.started += JumpStarted;
        IA_Interaction = PIA.Walking.Interact;
        IA_Interaction.Enable();
        IA_Interaction.started += InteractionStarted;

        
    }

    private void JumpStarted(InputAction.CallbackContext context)
    {
        JumpExecuted();

    }
    private void InteractionStarted(InputAction.CallbackContext context)
    {
        InteractionExectuted();
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = IA_Movement.ReadValue<float>();
        SpriteFlipping();


    }
    private void FixedUpdate()
    {
        rbPlayer.AddForce(new Vector2(movementInput * movementSpeed, 0));
    }

    private void JumpExecuted()
    {
        if (IsTouchingGround())
        {
            rbPlayer.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
    public bool IsTouchingGround()
    {
        int mask = LayerMask.GetMask("Ground");

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, BoxSize, 0, transform.up, CastDistance, mask);

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void InteractionExectuted()
    {
        int mask = LayerMask.GetMask("Interaction");

        Vector2 origin = transform.position + new Vector3(rayOffset * movementInput, 0);
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            direction,
            interactionRayDistance,
            mask
        );

        Debug.DrawRay(origin, direction * interactionRayDistance, Color.green, 1f);

        if (hit.collider != null && hit.collider.gameObject != this.gameObject)
        {
            interaction item = hit.collider.transform.GetComponent<interaction>();
            if (item != null)
            {
                item.InteractionExectuted();
            }
        }
    }
    public void SpriteFlipping()
    {
        if (movementInput<0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput>0)
        {
            spriteRenderer.flipX = false;
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y + CastDistance), BoxSize);
    }

}
