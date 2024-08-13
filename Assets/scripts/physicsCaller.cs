using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsCaller : MonoBehaviour
{
    private scalablePhysics[] children;

    // Start is called before the first frame update
    void Start()
    {
        updateChildren();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        callPhysicsLoop();
    }

    public void callPhysicsLoop()
    {
        foreach (scalablePhysics physics in children)
        {
            physics.physicsTick();
        }
    }

    public void updateChildren()
    {
        children = GetComponentsInChildren<scalablePhysics>();
    }
}
