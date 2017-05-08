using UnityEngine;
using System.Collections;

public class SmoothFollowCamController : MonoBehaviour
{
    public float dampTime = 0.15f;
    public Vector3 cameraSpeedModifier = Vector2.one;
    public Transform Target;

    Vector3 cameraVelocity = Vector3.zero;
    Vector3 cameraPos = Vector3.zero;
    Vector3 delta;

    float _targetOrtho = 5f;
    public float orthoZoomSpeed = 5f;
    public float MinZoom = 1.0f;
    public float MaxZoom = 20.0f;

    private void Start()
    {
        if (Target)
        {
            cameraPos = Camera.main.WorldToViewportPoint(Target.position);
            cameraPos.x = cameraPos.y = 0.5f;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePresent)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                _targetOrtho += -Input.mouseScrollDelta.y;
                _targetOrtho = Mathf.Clamp(_targetOrtho, MinZoom, MaxZoom);
            }
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, _targetOrtho, orthoZoomSpeed * Time.deltaTime);
        }

        if (Target)
        {
            //cameraPos = Camera.main.WorldToViewportPoint(Target.position);

            delta = Target.position - Camera.main.ViewportToWorldPoint(cameraPos); //Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cameraPos.z)); //(new Vector3(0.5, 0.5, point.z));
            delta.x *= cameraSpeedModifier.x;
            delta.y *= cameraSpeedModifier.y;
            delta.z *= cameraSpeedModifier.z;
            transform.position = Vector3.SmoothDamp(transform.position, (transform.position + delta), ref cameraVelocity, dampTime);
        }
    }
}

