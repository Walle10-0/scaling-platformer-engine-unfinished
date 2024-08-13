using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class scalablePhysics : MonoBehaviour
{
    [SerializeField] private Vector3 hitbox;
    [SerializeField] protected Transform target;
    [SerializeField] private ContactFilter2D collisions;
    [SerializeField] private int ignoreCollisions;
    [SerializeField] private float gravity;
    [SerializeField] private float resistance;
    [SerializeField] private int maxSlope;

    private List<Collider2D> contacts = new List<Collider2D>();

    private Vector2 velocity = Vector2.zero;

    public virtual void physicsTick()
    {
        physics();
    }

    public void scale(float scale)
    {
        float absScale = Mathf.Abs(scale);
        float size = (1 + scale);
        getTarget().localScale = getScale() * size;
        if (isTouching())
        {
            translate(0f, absScale * 500f);
            if (isTouching())
            {
                if (isTouching(Vector3.right * absScale * 0.5f))
                {
                    if (isTouching(Vector3.left * absScale * 0.5f))
                    {
                        translate(0f, absScale * -500f);
                        getTarget().localScale = getScale() / size;
                    }
                    else
                    {
                        translate(absScale * -500f, 0f);
                    }
                }
                else
                {
                    translate(absScale * 500f, 0f);
                }
            }
        }
    }

    protected void physics()
    {
        moveX();
        moveY();
    }

    private void broadcastCollision(Vector2 velForce)
    {
        scalablePhysics x = null;
        foreach(Collider2D collider in contacts)
        {
            x = collider.gameObject.GetComponent<scalablePhysics>();
            if (x != null && x != this)
            {
                    x.changeVelocity(velForce * (getScale().magnitude / x.getScale().magnitude));
            }
        }
        contacts.Clear();
    }

    private void moveX()
    {
        velocity.x *= resistance;
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
                broadcastCollision(velocity * Vector2.right);
                velocity.x = 0f;
            }
        }
    }

    private void moveY()
    {
        velocity += Vector2.down * gravity;

        translate(0f, velocity.y);

        if(isTouching())
        {
            translate(0f, velocity.y * -1);
            broadcastCollision(velocity * Vector2.up);
            velocity.y = 0f;
        }
    }

    protected void translate(float x, float y)
    {
        target.Translate(x / 1000f * getScale().x, y / 1000f * getScale().y, 0f);
    }

    protected bool isTouching()
    {
        return (isTouching(Vector3.zero));
    }

    protected bool isTouching(Vector3 offset)
    {
        return (Physics2D.OverlapBox(target.position + Vector3.Scale(getScale(), offset), Vector3.Scale(getScale(), hitbox), 0f, collisions, contacts)) > ignoreCollisions;
    }

    protected bool isTouchingFloor()
    {
        return (isTouching(Vector3.down * 0.05f));
    }

    // setters and getters

    public Vector2 getVelocity()
    {
        return velocity;
    }

    public void changeVelocity(Vector2 newVel)
    {
        velocity += newVel;
    }

    public void setVelocity(Vector2 newVel)
    {
        velocity = newVel;
    }

    public Vector3 getScale()
    {
        return target.localScale;
    }

    protected Transform getTarget()
    {
        return target;
    }
}
