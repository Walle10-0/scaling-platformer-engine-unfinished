using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class platformer2DScalable : MonoBehaviour
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
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;

    private PlayerControl playerInput;

    private Vector2 velocity = Vector2.zero;
    private Vector2 mControls;
    private float sControls;
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
        scale();
    }

    private void scale()
    {
        if(sControls != 0f && maxScale > (player.localScale * (1 + sControls * scaleSpeed)).sqrMagnitude && minScale < (player.localScale * (1 + sControls * scaleSpeed)).sqrMagnitude)
        {
            player.localScale = player.localScale * (1 + sControls * scaleSpeed);
            if (isTouching())
            {
                translate(0f, scaleSpeed * 500f);
                if (isTouching())
                {
                    if (isTouching(Vector3.right * scaleSpeed * 0.5f))
                    {
                        if (isTouching(Vector3.left * scaleSpeed * 0.5f))
                        {
                            translate(0f, scaleSpeed * -500f);
                            player.localScale = player.localScale / (1 + sControls * scaleSpeed);
                        }
                        else
                        {
                            translate(scaleSpeed * -500f, 0f);
                        }
                    }
                    else
                    {
                        translate(scaleSpeed * 500f, 0f);
                    }
                }
            }

        }
    }

    private void physics()
    {
        moveX();
        moveY();
    }

    private void moveX()
    {
        velocity += mControls.x * Vector2.right * xSpeed;  // movement
        velocity.x *= 0.9f;
        translate(velocity.x, 0f);

        wallDetect();
    }

    private void wallDetect()
    {
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
        if((mControls.y > 0f) && coyote > 0)
        {
            velocity.y = jump;
            coyote = 0;
        }

        velocity += Vector2.down * (gravity - mControls.y * flutter);

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
        return (Physics2D.OverlapBox(player.position + Vector3.Scale(player.localScale, offset), Vector3.Scale(player.localScale, playerHitbox), 0f, collisions) != null);
    }

    private bool isTouchingFloor()
    {
        return (isTouching(Vector3.down * 0.05f));
    }

    private void readInput()
    {
        mControls = playerInput.player.move.ReadValue<Vector2>();
        sControls = playerInput.player.scale.ReadValue<float>();
    }
}
