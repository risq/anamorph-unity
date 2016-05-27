using UnityEngine;
using System.Collections;

public class SilhouetteController : MonoBehaviour {

    public Color PrivateColor;
    public Color PublicColor;
    public Color ProColor;

    // Global
    GameObject PrivateLights;
    GameObject PublicLights;
    GameObject ProLights;

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

    ModelSwitcher LeftElbowModelSwitcher;
    ModelSwitcher RightElbowModelSwitcher;

    ModelSwitcher LeftBottomShoulderModelSwitcher;
    ModelSwitcher RightBottomShoulderModelSwitcher;

    // Mood
    ModelSwitcher LeftLungModelSwitcher;
    Material LeftLungMaterial;

    ModelSwitcher RightLungModelSwitcher;
    Material RightLungMaterial;

    SplineDecorator Spline;

    void Start () {
        // Global
        PrivateLights = GameObject.Find("Light/Private");
        PublicLights = GameObject.Find("Light/Public");
        ProLights = GameObject.Find("Light/Pro");

        // Activity
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

        LeftElbowModelSwitcher = GameObject.Find("LeftElbow/Elbow Model/Inner").GetComponent<ModelSwitcher>();
        RightElbowModelSwitcher = GameObject.Find("RightElbow/Elbow Model/Inner").GetComponent<ModelSwitcher>();

        LeftBottomShoulderModelSwitcher = GameObject.Find("LeftShoulder/Full Shoulder/Bottom Shoulder Model/Inner").GetComponent<ModelSwitcher>();
        RightBottomShoulderModelSwitcher = GameObject.Find("RightShoulder/Full Shoulder/Bottom Shoulder Model/Inner").GetComponent<ModelSwitcher>();

        // Mood
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
        IdentityCircle mainCircle = (IdentityCircle)Random.Range(1, 4);
        SetMainLight(mainCircle);

        // ========== Activity ==========

        // Freq
        float activityGlobalFreq = Random.Range(0, 100);
        float activityPrivateFreq = Random.Range(0, 100);
        float activityPublicFreq = Random.Range(0, 100);
        float activityProFreq = Random.Range(0, 100);
        Color activityFreqMix = GenerateColor(activityGlobalFreq, activityPrivateFreq, activityPublicFreq);     

        LeftLowerArmModelSwitcher.currentValue = activityGlobalFreq;
        LeftLowerArmPublicModelSwitcher.currentValue = activityPublicFreq;
        LeftLowerArmPrivateModelSwitcher.currentValue = activityPrivateFreq;
        LeftLowerArmProModelSwitcher.currentValue = activityProFreq;
        SetColor(LeftLowerArmMaterial, activityFreqMix);

        RightLowerArmModelSwitcher.currentValue = activityGlobalFreq;
        RightLowerArmPublicModelSwitcher.currentValue = activityPublicFreq;
        RightLowerArmPrivateModelSwitcher.currentValue = activityPrivateFreq;
        LeftUpperArmProModelSwitcher.currentValue = activityProFreq;
        SetColor(RightLowerArmMaterial, activityFreqMix);

        LeftUpperArmModelSwitcher.currentValue = activityGlobalFreq;
        LeftUpperArmPublicModelSwitcher.currentValue = activityPublicFreq;
        LeftUpperArmPrivateModelSwitcher.currentValue = activityPrivateFreq;
        LeftUpperArmProModelSwitcher.currentValue = activityProFreq;
        SetColor(LeftUpperArmMaterial, activityFreqMix);

        RightUpperArmModelSwitcher.currentValue = activityGlobalFreq;
        RightUpperArmPublicModelSwitcher.currentValue = activityPublicFreq;
        RightUpperArmPrivateModelSwitcher.currentValue = activityPrivateFreq;
        RightUpperArmProModelSwitcher.currentValue = activityProFreq;
        SetColor(RightUpperArmMaterial, activityFreqMix);

        // Volume
        float activityGlobalVol = Random.Range(0, 100);
        float activityPrivateVol = Random.Range(0, 100);
        float activityPublicVol = Random.Range(0, 100);
        float activityProVol = Random.Range(0, 100);

        SetBones(activityGlobalFreq, activityPublicVol, activityPrivateVol, activityProVol);

        // Photos posts volume
        float activityPrivatePhotoVol = Random.Range(0, 100);
        float activityPublicPhotoVol = Random.Range(0, 100);
        float activityProPhotoVol = Random.Range(0, 100);

        LeftElbowModelSwitcher.currentValue = activityPrivatePhotoVol;
        RightElbowModelSwitcher.currentValue = activityPrivatePhotoVol;

        LeftBottomShoulderModelSwitcher.currentValue = activityPublicPhotoVol;
        RightBottomShoulderModelSwitcher.currentValue = activityPublicPhotoVol;



        // ========== Mood ==========

        // Expressivity
        float moodGlobalExpressivity = Random.Range(0, 100);
        Color moodExpressivityMix = new Color(Random.value, Random.value, Random.value);

        LeftLungModelSwitcher.currentValue = moodGlobalExpressivity;
        SetColor(LeftLungMaterial, moodExpressivityMix);

        RightLungModelSwitcher.currentValue = moodGlobalExpressivity;
        SetColor(RightLungMaterial, moodExpressivityMix);
    }

    void SetColor(Material objectMaterial, Color color)
    {
        objectMaterial.SetColor("_Color", color);
        // objectMaterial.SetColor("_FresColor", color);
        // objectMaterial.SetColor("_DiffColor", color);
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

    void SetMainLight(IdentityCircle mainCircle)
    {
        if (mainCircle == IdentityCircle.Private) {
            PrivateLights.SetActive(true);
            PublicLights.SetActive(false);
            ProLights.SetActive(false);
        }
        else if (mainCircle == IdentityCircle.Public)
        {
            PrivateLights.SetActive(false);
            PublicLights.SetActive(true);
            ProLights.SetActive(false);
        }
        else if (mainCircle == IdentityCircle.Pro)
        {
            PrivateLights.SetActive(false);
            PublicLights.SetActive(false);
            ProLights.SetActive(true);
        }
    }

    Color GenerateColor(float privatePart, float publicPart, float proPart)
    {
        publicPart = publicPart / (publicPart + privatePart + proPart);
        privatePart = publicPart / (publicPart + privatePart + proPart);
        proPart = publicPart / (publicPart + privatePart + proPart);

        return new Color(PrivateColor.r * publicPart + PublicColor.r * publicPart + ProColor.r * proPart,
            PrivateColor.g * publicPart + PublicColor.g * publicPart + ProColor.g * proPart,
            PrivateColor.b * publicPart + PublicColor.b * publicPart + ProColor.b * proPart);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "UpdateData"))
            UpdateData();

        if (GUI.Button(new Rect(10, 60, 150, 50), "Clean mem"))
            Resources.UnloadUnusedAssets();
    }
}
