using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float panSpeed = 100f;

    public float minPanZ;
    public float maxPanZ;
    public float minPanX;
    public float maxPanX;

    Camera cam;

    private void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        //Pan up and down with keys
        pos = PanWithKeyboard(pos);

        //Clamps the camera
        pos = ClampPositions(pos);

        transform.position = pos;
    }

    private Vector3 PanWithKeyboard(Vector3 pos)
    {
        if (Input.GetKey("w"))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        return pos;
    }

    private Vector3 ClampPositions(Vector3 pos)
    {
        pos.z = Mathf.Clamp(pos.z, minPanZ, maxPanZ);
        pos.x = Mathf.Clamp(pos.x, minPanX, maxPanX);
        return pos;
    }
}
