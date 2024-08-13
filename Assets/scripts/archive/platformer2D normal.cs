using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class platformer2D : MonoBehaviour
{
    [SerializeField] private Vector3 playerHitbox;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask collisions;

    [SerializeField] private float xSpeed;
    [SerializeField] private float jump;
    [SerializeField] private float gravity;
    [SerializeField] private float flutter;
    [SerializeField] private int maxSlope;
    [SerializeField] private int coyoteTime;

    private PlayerControl playerInput;

    private Vector2 velocity = Vector2.zero;
    private Vector2 controls;
    private int coyote;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerControl();
        playerInput.Enable();
        readInput();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        readInput();
        physics();
    }

    private void physics()
    {
        velocity += controls.x * Vector2.right * xSpeed;
        velocity.x *= 0.9f;

        moveX();
        moveY();
    }

    private void moveX()
    {
        translate(velocity.x, 0f);
        if (isTouching())
        {
            for (int i = 0; i < maxSlope; i++)
            {
                if (isTouching())
                {
                    translate(0f, 1f);
                }
            }
            if (isTouching())
            {
                translate(velocity.x * -1f, -1f * maxSlope);
                velocity.x = 0f;
            }
        }
    }

    private void moveY()
    {
        coyote = (isTouchingFloor()) ? coyoteTime : coyote - 1;
        if((controls.y > 0f) && coyote > 0)
        {
            velocity.y = jump;
            coyote = 0;
        }

        velocity += Vector2.down * (gravity - controls.y * flutter);

        translate(0f, velocity.y);

        if(isTouching())
        {
            translate(0f, velocity.y * -1);
            velocity.y = 0f;
        }
    }

    private void translate(float x, float y)
    {
        player.Translate(x / 1000f * player.localScale.x, y / 1000f * player.localScale.y, 0f);
    }

    private bool isTouching()
    {
        return (isTouching(Vector3.zero));
    }

    private bool isTouching(Vector3 offset)
    {
        return (Physics2D.OverlapBox(player.position + offset, Vector3.Scale(player.localScale, playerHitbox), 0f, collisions) != null);
    }

    private bool isTouchingFloor()
    {
        return (isTouching(Vector3.down * 0.05f));
    }

    private void readInput()
    {
        controls = playerInput.player.move.ReadValue<Vector2>();
    }
}
