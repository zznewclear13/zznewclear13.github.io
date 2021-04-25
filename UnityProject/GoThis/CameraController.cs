using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float angleSpeed = 0.1f;
    //private bool navigate = false;

    public float upMax;
    public float upMin;

    private float upAngle;
    private float rightAngle;
    private Vector3 currentAngle;

    //private Camera cam;
    private Transform camTransform;

    void Awake()
    {
        camTransform = GetComponentInChildren<Camera>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        Navigate();
    }

    private Vector2 MapSquareToCircle(Vector2 vec)
    {
        return new Vector2(vec.x * Mathf.Sqrt(1 - vec.y * vec.y / 2), vec.y * Mathf.Sqrt(1 - vec.x * vec.x / 2));
    }

    private void Navigate()
    {
        if(Input.GetMouseButtonDown(1))
        {
            currentAngle = transform.rotation.eulerAngles;
        }

        if (Input.GetMouseButton(1))
        {
            float right = -Input.GetAxis("Horizontal");
            float forward = Input.GetAxis("Vertical");

            //movement
            Vector2 remapVec = MapSquareToCircle(new Vector2(forward, right));
            Vector3 offset =   remapVec.x * camTransform.forward - remapVec.y * camTransform.right;
            //Vector3 offset = new Vector3(direction.x, 0, direction.y);
            transform.position += offset * moveSpeed * Time.deltaTime;

            //rotation
            float rightAdd = Input.GetAxis("Mouse Y");
            float upAdd = Input.GetAxis("Mouse X");
            currentAngle.y += upAdd * angleSpeed;
            currentAngle.x -= rightAdd * angleSpeed;

            Vector3 targetAngle = currentAngle;
            transform.rotation = Quaternion.Euler(targetAngle);

        }

        if (Input.GetMouseButtonUp(1))
        {

        }

    }

}
