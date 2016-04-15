using UnityEngine;
using System.Collections;

public class HudFollowObject : MonoBehaviour {

    public GameObject target;
    public GameObject line;

    RectTransform rectTransform;
    RectTransform canvasRect;
    Transform lineTransform;
    Transform targetTransform;

    LineRenderer lineRenderer;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<RectTransform>();
        lineRenderer = line.GetComponent<LineRenderer>();
        lineTransform = line.GetComponent<Transform>();
        targetTransform = target.GetComponent<Transform>();
    }

    void Update()
    {
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(targetTransform.position);
        Vector2 lerpedPosition = Vector3.Lerp(rectTransform.anchorMin, viewportPoint, 0.1f);
        rectTransform.anchorMin = lerpedPosition;
        rectTransform.anchorMax = lerpedPosition;

        Vector3[] points = new Vector3[2];
        points[0] = lineTransform.position;
        points[1] = targetTransform.position;
        lineRenderer.SetPositions(points);

        rectTransform.localRotation = Quaternion.Euler(40 - (60 * viewportPoint.y), -105 + (180 * viewportPoint.x), 0);
    }
}
