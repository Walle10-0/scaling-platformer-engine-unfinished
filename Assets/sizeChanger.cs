using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sizeChanger : MonoBehaviour
{
    [SerializeField] private Vector3 hitbox;
    [SerializeField] protected Transform target;
    [SerializeField] private ContactFilter2D collisions;
    [SerializeField] private float scaleSpeed;

    private List<Collider2D> contacts = new List<Collider2D>();

    // Update is called once per frame
    void FixedUpdate()
    {
        Physics2D.OverlapBox(target.position, Vector3.one, 0f, collisions, contacts);

        scalablePhysics x = null;
        foreach (Collider2D collider in contacts)
        {
            x = collider.gameObject.GetComponent<scalablePhysics>();
            if (x != null)
            {
                x.scale(scaleSpeed);
            }
        }
        contacts.Clear();
    }
}
