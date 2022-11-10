using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothTime;

    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 desiredPos = playerTransform.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothTime * Time.deltaTime);
        transform.position = smoothPos;
    }
}
