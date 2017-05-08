using UnityEngine;
using System.Collections;

public class TouchCamController : MonoBehaviour
{

    public Vector2 MoveSensitivity = Vector2.one;
    public bool SensitiveMode = true;

    public float orthoZoomSpeed = 0.05f;
    public float MinZoom = 1.0f;
    public float MaxZoom = 20.0f;

    public bool InvertX = false;
    public bool InvertY = false;

    private float _targetOrtho = 0f;

    Vector2 touchDelta;
    Vector2 touch1Prev, touch2Prev;
    float touchMagPrev, touchMagNew, deltaMagDiff;

    // Use this for initialization
    void Start()
    {
        _targetOrtho = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (SensitiveMode)
        {
            MoveSensitivity.x = Camera.main.orthographicSize / 5;
            MoveSensitivity.y = Camera.main.orthographicSize / 5;
        }
        else
        {
            MoveSensitivity = Vector2.one;
        }

        if (Input.touches.Length > 0)
        {
            // Single Touch
            if (Input.touches.Length == 1 && Input.touches[0].phase == TouchPhase.Moved)
            {
                // Move
                touchDelta = Input.GetTouch(0).deltaPosition;
                touchDelta.x = InvertX ? -touchDelta.x : touchDelta.x;
                touchDelta.y = InvertY ? -touchDelta.y : touchDelta.y;
                Camera.main.transform.Translate(-touchDelta.x * MoveSensitivity.x * Time.deltaTime, -touchDelta.y * MoveSensitivity.y * Time.deltaTime, 0);
            }
            else if (Input.touches.Length == 2)
            {
                // Zoom
                touch1Prev = Input.touches[0].position - Input.touches[0].deltaPosition;
                touch2Prev = Input.touches[1].position - Input.touches[1].deltaPosition;

                touchMagPrev = (touch1Prev - touch2Prev).magnitude;
                touchMagNew = (Input.touches[0].position - Input.touches[2].position).magnitude;

                deltaMagDiff = touchMagPrev - touchMagNew;
                Camera.main.orthographicSize += deltaMagDiff * orthoZoomSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, MinZoom, MaxZoom);
            }
        }

        if (Input.mousePresent)
        {
            // Zoom
            if (Input.mouseScrollDelta.y != 0)
            {
                _targetOrtho += -Input.mouseScrollDelta.y;
                _targetOrtho = Mathf.Clamp(_targetOrtho, MinZoom, MaxZoom);
            }
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, _targetOrtho, 5f * Time.deltaTime);

            // Drag
            // Grab click on first mouse down
            if (Input.GetMouseButtonDown(0))
            {
                touch1Prev = Input.mousePosition;
                return;
            }
            // Return if no mouse down
            if (!Input.GetMouseButton(0)) return;
            // Move cam
            touchDelta = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition - touch1Prev);
            touchDelta.x = InvertX ? -touchDelta.x : touchDelta.x;
            touchDelta.y = InvertY ? -touchDelta.y : touchDelta.y;
            Camera.main.transform.Translate(-touchDelta.x * MoveSensitivity.x * Time.deltaTime, -touchDelta.y * MoveSensitivity.y * Time.deltaTime, 0);
        }

    }

}
