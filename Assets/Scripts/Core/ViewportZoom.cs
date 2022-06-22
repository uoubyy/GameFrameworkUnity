using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ViewportZoom : MonoBehaviour
{
    private Vector3 m_touchStart;

    [SerializeField]
    float zoomOutMin = 1;

    [SerializeField]
    float zoomOutMax = 8;

    [SerializeField]
    float zoomStep = 0.01f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float curMagnitude = (touchOne.position - touchZero.position).magnitude;

            Zoom((curMagnitude - prevMagnitude) * zoomStep);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 offset = m_touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += offset;
        }

        if (EditorWindow.mouseOverWindow && EditorWindow.mouseOverWindow.ToString() == " (UnityEditor.GameView)")
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    public void Zoom(float increament)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increament, zoomOutMin, zoomOutMax);
    }
}
