using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class springObject : scalablePhysics
{
    private Vector3 rest;
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private float yResist;
    private Vector2 multiplier = Vector2.zero;

    void Start()
    {
        rest = getTarget().position;
        multiplier += lockX ? Vector2.zero : Vector2.right;
        multiplier += (lockY ? Vector2.zero : Vector2.up);
    }

    override
    public void physicsTick()
    {
        spring();
        physics();
    }

    private void spring()
    {
        changeVelocity(Vector2.ClampMagnitude(Vector2.Scale((rest - getTarget().position), (rest - getTarget().position)), 500f));
        setVelocity(getVelocity().x * (lockX ? Vector2.zero : Vector2.right) + getVelocity().y * yResist * (lockY ? Vector2.zero : Vector2.up));
    }
}
