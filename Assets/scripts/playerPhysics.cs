using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerPhysics : scalablePhysics
{
    [SerializeField] private float xSpeed;
    [SerializeField] private float jump;
    [SerializeField] private float flutter;
    [SerializeField] private int coyoteTime;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;

    private PlayerControl playerInput;

    private Vector2 mControls = Vector2.zero;
    private float sControls = 0f;
    private int coyote;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerControl();
        playerInput.Enable();
    }

    private void playerScale()
    {
        float newSize = (getScale() * (1 + (sControls * scaleSpeed))).sqrMagnitude;
        if (sControls != 0f && maxScale > newSize && minScale < newSize)
        {
            scale(sControls * scaleSpeed);
        }
    }

    override
    public void physicsTick()
    {
        readInput();
        inputMovement();
        physics();
        playerScale();
    }

    private void readInput()
    {
        mControls = playerInput.player.move.ReadValue<Vector2>();
        sControls = playerInput.player.scale.ReadValue<float>();
    }

    private void inputMovement()
    {
        changeVelocity(mControls.x * Vector2.right * xSpeed + Vector2.up * (mControls.y * flutter));

        coyote = (isTouchingFloor()) ? coyoteTime : coyote - 1;
        if ((mControls.y > 0f) && coyote > 0)
        {
            setVelocity(getVelocity() * Vector2.right + Vector2.up * jump);
            coyote = 0;
        }
    }
}
