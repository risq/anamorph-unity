using UnityEngine;
using System.Collections;

public class HudFollowObject : MonoBehaviour {

    public Transform target;
    public GameObject line;
    public Transform lineOrigin;
    public Vector3 offset;
    public bool lerp = true;
    public float speed = 1f;
    public bool rotateFishEye = false;

    RectTransform rectTransform;
    Transform lineTransform;
    LineRenderer lineRenderer;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (line)
        {
            lineRenderer = line.GetComponent<LineRenderer>();
            lineTransform = line.GetComponent<Transform>();
        }
    }

    void Update()
    {
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(target.position + offset);

        if (lerp)
        {
            Vector2 lerpedPosition = Vector3.Lerp(rectTransform.anchorMin, viewportPoint, Time.deltaTime * speed);
            rectTransform.anchorMin = lerpedPosition;
            rectTransform.anchorMax = lerpedPosition;
        }
        else
        {
            rectTransform.anchorMin = viewportPoint;
            rectTransform.anchorMax = viewportPoint;
        }

        if (line)
        {
            Vector3[] points = new Vector3[2];

            if (lineOrigin)
                points[0] = lineOrigin.position;
            else 
                points[0] = lineTransform.position;

            points[1] = target.position;
            lineRenderer.SetPositions(points);
        }

        if (rotateFishEye)
        {
            rectTransform.localRotation = Quaternion.Euler(40 - (60 * viewportPoint.y), -105 + (180 * viewportPoint.x), 0);
        }
    }
}
