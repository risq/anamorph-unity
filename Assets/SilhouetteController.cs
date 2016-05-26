using UnityEngine;
using System.Collections;

public class SilhouetteController : MonoBehaviour {

    // Activity
    ModelSwitcher LeftLowerArmModelSwitcher;
    ModelSwitcher LeftLowerArmPublicModelSwitcher;
    ModelSwitcher LeftLowerArmPrivateModelSwitcher;
    ModelSwitcher LeftLowerArmProModelSwitcher;
    Material LeftLowerArmMaterial;

    ModelSwitcher RightLowerArmModelSwitcher;
    ModelSwitcher RightLowerArmPublicModelSwitcher;
    ModelSwitcher RightLowerArmPrivateModelSwitcher;
    ModelSwitcher RightLowerArmProModelSwitcher;
    Material RightLowerArmMaterial;

    ModelSwitcher LeftUpperArmModelSwitcher;
    ModelSwitcher LeftUpperArmPublicModelSwitcher;
    ModelSwitcher LeftUpperArmPrivateModelSwitcher;
    ModelSwitcher LeftUpperArmProModelSwitcher;
    Material LeftUpperArmMaterial;

    ModelSwitcher RightUpperArmModelSwitcher;
    ModelSwitcher RightUpperArmPublicModelSwitcher;
    ModelSwitcher RightUpperArmPrivateModelSwitcher;
    ModelSwitcher RightUpperArmProModelSwitcher;
    Material RightUpperArmMaterial;

    ModelSwitcher LeftLungModelSwitcher;
    Material LeftLungMaterial;

    ModelSwitcher RightLungModelSwitcher;
    Material RightLungMaterial;

    SplineDecorator Spline;

    // Use this for initialization
    void Start () {
        LeftLowerArmModelSwitcher = GameObject.Find("LeftElbow/Lower Arm Model/Inner").GetComponent<ModelSwitcher>();
        LeftLowerArmPublicModelSwitcher = GameObject.Find("LeftElbow/Lower Arm Model/Public").GetComponent<ModelSwitcher>();
        LeftLowerArmPrivateModelSwitcher = GameObject.Find("LeftElbow/Lower Arm Model/Private").GetComponent<ModelSwitcher>();
        LeftLowerArmProModelSwitcher = GameObject.Find("LeftElbow/Lower Arm Model/Pro").GetComponent<ModelSwitcher>();
        LeftLowerArmMaterial = GameObject.Find("LeftElbow/Lower Arm Model/Inner").GetComponent<Renderer>().material;

        RightLowerArmModelSwitcher = GameObject.Find("RightElbow/Lower Arm Model/Inner").GetComponent<ModelSwitcher>();
        RightLowerArmPublicModelSwitcher = GameObject.Find("RightElbow/Lower Arm Model/Public").GetComponent<ModelSwitcher>();
        RightLowerArmPrivateModelSwitcher = GameObject.Find("RightElbow/Lower Arm Model/Private").GetComponent<ModelSwitcher>();
        RightLowerArmProModelSwitcher = GameObject.Find("RightElbow/Lower Arm Model/Pro").GetComponent<ModelSwitcher>();
        RightLowerArmMaterial = GameObject.Find("RightElbow/Lower Arm Model/Inner").GetComponent<Renderer>().material;

        LeftUpperArmModelSwitcher = GameObject.Find("LeftUpperArm/Upper Arm Model/Inner").GetComponent<ModelSwitcher>();
        LeftUpperArmPublicModelSwitcher = GameObject.Find("LeftUpperArm/Upper Arm Model/Public").GetComponent<ModelSwitcher>();
        LeftUpperArmPrivateModelSwitcher = GameObject.Find("LeftUpperArm/Upper Arm Model/Private").GetComponent<ModelSwitcher>();
        LeftUpperArmProModelSwitcher = GameObject.Find("LeftUpperArm/Upper Arm Model/Pro").GetComponent<ModelSwitcher>();
        LeftUpperArmMaterial = GameObject.Find("LeftUpperArm/Upper Arm Model/Inner").GetComponent<Renderer>().material;

        RightUpperArmModelSwitcher = GameObject.Find("RightUpperArm/Upper Arm Model/Inner").GetComponent<ModelSwitcher>();
        RightUpperArmPublicModelSwitcher = GameObject.Find("RightUpperArm/Upper Arm Model/Public").GetComponent<ModelSwitcher>();
        RightUpperArmPrivateModelSwitcher = GameObject.Find("RightUpperArm/Upper Arm Model/Private").GetComponent<ModelSwitcher>();
        RightUpperArmProModelSwitcher = GameObject.Find("RightUpperArm/Upper Arm Model/Pro").GetComponent<ModelSwitcher>();
        RightUpperArmMaterial = GameObject.Find("RightUpperArm/Upper Arm Model/Inner").GetComponent<Renderer>().material;

        LeftLungModelSwitcher = GameObject.Find("Left Lung Model/Inner").GetComponent<ModelSwitcher>();
        LeftLungMaterial = GameObject.Find("Left Lung Model/Inner").GetComponent<Renderer>().material;

        RightLungModelSwitcher = GameObject.Find("Right Lung Model/Inner").GetComponent<ModelSwitcher>();
        RightLungMaterial = GameObject.Find("Right Lung Model/Inner").GetComponent<Renderer>().material;

        Spline = GameObject.Find("Spline Model").GetComponent<SplineDecorator>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void UpdateData()
    {
        float activityFreqValue = Random.Range(0, 100);
        float activityPublicVol = Random.Range(0, 100);
        float activityPrivateVol = Random.Range(0, 100);
        float activityProVol = Random.Range(0, 100);
        Color activityFreqMix = new Color(Random.value, Random.value, Random.value);
        float moodGlobalExpressivity = Random.Range(0, 100);
        Color moodExpressivityMix = new Color(Random.value, Random.value, Random.value);

        LeftLowerArmModelSwitcher.currentValue = activityFreqValue;
        LeftLowerArmPublicModelSwitcher.currentValue = Random.Range(0, 100);
        LeftLowerArmPrivateModelSwitcher.currentValue = Random.Range(0, 100);
        LeftLowerArmProModelSwitcher.currentValue = Random.Range(0, 100);
        SetColor(LeftLowerArmMaterial, activityFreqMix);

        RightLowerArmModelSwitcher.currentValue = activityFreqValue;
        RightLowerArmPublicModelSwitcher.currentValue = Random.Range(0, 100);
        RightLowerArmPrivateModelSwitcher.currentValue = Random.Range(0, 100);
        LeftUpperArmProModelSwitcher.currentValue = Random.Range(0, 100);
        SetColor(RightLowerArmMaterial, activityFreqMix);

        LeftUpperArmModelSwitcher.currentValue = activityFreqValue;
        LeftUpperArmPublicModelSwitcher.currentValue = Random.Range(0, 100);
        LeftUpperArmPrivateModelSwitcher.currentValue = Random.Range(0, 100);
        LeftUpperArmProModelSwitcher.currentValue = Random.Range(0, 100);
        SetColor(LeftUpperArmMaterial, activityFreqMix);

        RightUpperArmModelSwitcher.currentValue = activityFreqValue;
        RightUpperArmPublicModelSwitcher.currentValue = Random.Range(0, 100);
        RightUpperArmPrivateModelSwitcher.currentValue = Random.Range(0, 100);
        RightUpperArmProModelSwitcher.currentValue = Random.Range(0, 100);
        SetColor(RightUpperArmMaterial, activityFreqMix);

        LeftLungModelSwitcher.currentValue = moodGlobalExpressivity;
        SetColor(LeftLungMaterial, moodExpressivityMix);

        RightLungModelSwitcher.currentValue = moodGlobalExpressivity;
        SetColor(RightLungMaterial, moodExpressivityMix);

        SetBones(activityFreqValue, activityPublicVol, activityPrivateVol, activityProVol);
    }

    void SetColor(Material objectMaterial, Color color)
    {
        objectMaterial.SetColor("_FresColor", color);
        objectMaterial.SetColor("_DiffColor", color);
    }

    void SetBones(float activityFreqValue, float publicVol, float privateVol, float proVol)
    {
        Transform[] splineBones = Spline.CreateSpline((int)(4 + activityFreqValue / 4));

        for (int i = 0; i < splineBones.Length; i++)
        {
            splineBones[i].transform.FindChild("Public").localScale = new Vector3(1f + publicVol / 100f * 5f, 1, 1);
            splineBones[i].transform.FindChild("Private").localScale = new Vector3(1f + privateVol / 100f * 5f, 1, 1);
            splineBones[i].transform.FindChild("Pro").localScale = new Vector3(1f + proVol / 100f * 5f, 1, 1);
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "UpdateData"))
            UpdateData();
    }
}
