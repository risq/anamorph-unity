using UnityEngine;
using System.Collections;

public class TransformModifier : MonoBehaviour {
    [Header("Minimum position")]
    public float minX = 0f;
    public float minY = 0f;
    public float minZ = 0f;

    [Header("Minimum Scale")]
    public float minScaleX = 1f;
    public float minScaleY = 1f;
    public float minScaleZ = 1f;

    
    [Header("Maximum Position")]
    [Space(10)]
    public float maxX = 0f;
    public float maxY = 0f;
    public float maxZ = 0f;

    [Header("Maximum Scale")]
    public float maxScaleX = 1f;
    public float maxScaleY = 1f;
    public float maxScaleZ = 1f;

    public float CurrentValue
    {
        get { return currentValue; }
        set
        {
            currentValue = value;
            UpdateTransform();
        }
    }

    float currentValue = 0;

    void Awake () {

	}
	
	public void UpdateTransform () {
        transform.localPosition = new Vector3(minX + (maxX - minX) * currentValue / 100, minY + (maxY - minY) * currentValue / 100, minZ + (maxZ - minZ) * currentValue / 100);
        transform.localScale = new Vector3(minScaleX + (maxScaleX - minScaleX) * currentValue / 100, minScaleY + (maxScaleY - minScaleY) * currentValue / 100, minScaleZ + (maxScaleZ - minScaleZ) * currentValue / 100);
	}
}
