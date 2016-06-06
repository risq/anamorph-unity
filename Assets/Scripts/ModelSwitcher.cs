using UnityEngine;
using System.Collections;

public class ModelSwitcher : MonoBehaviour {

    public GameObject[] framesPrefabs;
    private Material material;

    public float averageActivationTime = 3;
    public float averageDeactivationTime = 0.05f;
    public float minIntensity = -40f;
    public float maxIntensity = 40f;
    public bool glitch = false;

    [Range(0.0f, 100.0f)]
    public float currentValue = 0;

    public float sineAmplitude = 50;
    public float sineSpeed = 2;
    private float sineOffset = 0;

    private float lastValue = -1;
    
    private int currentIndex = -1;
    private int framesCount;

    void Start () {
        framesCount = framesPrefabs.Length;

        Renderer renderer = GetComponent<Renderer>();

        if (renderer)
        {
            material = renderer.material;
            renderer.enabled = false;
        }

        for (int i = 0; i < framesCount; i++)
        {
            framesPrefabs[i] = Instantiate(framesPrefabs[i]);
            framesPrefabs[i].transform.SetParent(transform, false);
            framesPrefabs[i].SetActive(false);

            if (material)
                SetMaterial(framesPrefabs[i].transform, material);
        }

        if (glitch)
        {
            StartCoroutine(ActivationLoop());
        }

        sineOffset = Random.value;
    }
	
	void Update () {
        float tempValue = currentValue + Mathf.Sin(sineOffset + Time.time * sineSpeed) * sineAmplitude;
        if (tempValue != lastValue)
        {
            UpdateModel(tempValue);
            lastValue = tempValue;
        }
	}

    void UpdateModel(float pos)
    {
        int index = pos <= 0 ? 0 : pos >= 100 ? framesCount - 1 : (int) Mathf.Clamp(Mathf.Round(pos * (framesCount - 1) / 100), 0, framesCount - 1);

        if (currentIndex != index)
        {
            if (currentIndex > -1)
                framesPrefabs[currentIndex].SetActive(false);

            framesPrefabs[index].SetActive(true);
            currentIndex = index;
        }
            
    }

    void SetMaterial(Transform frameTransform, Material material)
    {
        foreach (Transform child in frameTransform)
        {
            SetMaterial(child, material);

            Renderer renderer = child.GetComponent<Renderer>();

            if(renderer)
                renderer.material = material;
        }
    }

    IEnumerator ActivationLoop()
    {
        while (true)
        {
            float glitchValue = Random.Range(minIntensity, maxIntensity);
            UpdateModel(currentValue + glitchValue);
            yield return new WaitForSeconds(Random.Range(0, averageDeactivationTime * 2));
            UpdateModel(currentValue);
            yield return new WaitForSeconds(Random.Range(0, averageActivationTime * 2));
        }
    }

    [ContextMenu("Sort Frames")]
    void DoSortFrames()
    {
        System.Array.Sort(framesPrefabs, (a, b) => a.name.CompareTo(b.name));
        Debug.Log(gameObject.name + ".frames have been sorted alphabetically.");
    }
    
    [ContextMenu("Inverse Sort Frames")]
    void DoInverseSortFrames()
    {
        System.Array.Sort(framesPrefabs, (a, b) => b.name.CompareTo(a.name));
        Debug.Log(gameObject.name + ".frames have been sorted alphabetically.");
    }
}
