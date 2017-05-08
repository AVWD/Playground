using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class KeybCamController : MonoBehaviour
{
    public float moveSpeed = 2f;
    Vector3 translation = Vector3.zero;
    float _targetOrtho = 5f;
    public float orthoZoomSpeed = 5f;
    public float MinZoom = 1.0f;
    public float MaxZoom = 20.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePresent)
        {
            // Zoom
            if (Input.mouseScrollDelta.y != 0)
            {
                _targetOrtho += -Input.mouseScrollDelta.y;
                _targetOrtho = Mathf.Clamp(_targetOrtho, MinZoom, MaxZoom);
            }
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, _targetOrtho, orthoZoomSpeed * Time.deltaTime);
        }

        translation.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        translation.y = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        Camera.main.transform.Translate(translation);
    }
}
