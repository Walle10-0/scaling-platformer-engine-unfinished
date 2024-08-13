using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform cam;
    [SerializeField] private float distance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float XYpull;
    [SerializeField] private float Zpull;
    private Vector3 target;
    private Vector3 cameraTheory;

    // Update is called once per frame
    void FixedUpdate()
    {
        cameraTheory = Vector3.Scale(cam.position, (Vector3.right + Vector3.up));
        if (((player.position - cameraTheory).sqrMagnitude / (cam.position.z * cam.position.z)) < maxDistance)
        {
            target = Vector3.Lerp(cameraTheory, player.position, XYpull);
            target = Vector3.Lerp(cameraTheory, player.position, XYpull);
        }
        else
        {
            target = player.position;
            Debug.Log("e");
        }

        target.z = Mathf.Lerp(cam.position.z, (distance * player.localScale.magnitude * -1f), Zpull);


        cam.position = target;
    }
}
