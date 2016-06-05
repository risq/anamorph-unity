using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class SilhouetteController : MonoBehaviour {

    public Color PrivateColor;
    public Color PublicColor;
    public Color ProColor;

    HSBColor PrivateColorHSB;
    HSBColor PublicColorHSB;
    HSBColor ProColorHSB;

    // Global
    GameObject PrivatePrimaryLight;
    GameObject PrivateSecondaryLight;
    GameObject PublicPrimaryLight;
    GameObject PublicSecondaryLight;
    GameObject ProPrimaryLight;
    GameObject ProSecondaryLight;

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

    // Influence
    PlaygroundParticlesC FollowersParticles;

    // Mood
    ModelSwitcher LeftLungModelSwitcher;
    Material LeftLungMaterial;

    ModelSwitcher RightLungModelSwitcher;
    Material RightLungMaterial;

    SplineDecorator Spline;

    const int maxParticles = 300;

    void Start () {
        PrivateColorHSB = new HSBColor(PrivateColor);
        PublicColorHSB = new HSBColor(PublicColor);
        ProColorHSB = new HSBColor(ProColor);

        // Global
        PrivatePrimaryLight = GameObject.Find("Light/Private/Primary");
        PrivateSecondaryLight = GameObject.Find("Light/Private/Secondary");
        PublicPrimaryLight = GameObject.Find("Light/Public/Primary");
        PublicSecondaryLight = GameObject.Find("Light/Public/Secondary");
        ProPrimaryLight = GameObject.Find("Light/Pro/Primary");
        ProSecondaryLight = GameObject.Find("Light/Pro/Secondary");

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

        // Influence
        FollowersParticles = GameObject.Find("Head/Followers/Followers Particles System").GetComponent<PlaygroundParticlesC>();

        // Mood
        LeftLungModelSwitcher = GameObject.Find("Left Lung Model/Inner").GetComponent<ModelSwitcher>();
        LeftLungMaterial = GameObject.Find("Left Lung Model/Inner").GetComponent<Renderer>().material;

        RightLungModelSwitcher = GameObject.Find("Right Lung Model/Inner").GetComponent<ModelSwitcher>();
        RightLungMaterial = GameObject.Find("Right Lung Model/Inner").GetComponent<Renderer>().material;

        Spline = GameObject.Find("Spline Model").GetComponent<SplineDecorator>();

        UpdateData(null);
    }
	
    public void UpdateData(JSONObject data)
    {
        IdentityCircle primaryCircle = (IdentityCircle)Random.Range(1, 4);
        IdentityCircle secondaryCircle = (IdentityCircle)Random.Range(1, 4);

        if (data)
        {
            string primaryCircleString = "";
            string secondaryCircleString = "";

            data.GetField("global").GetField(ref primaryCircleString, "primaryCircle");
            data.GetField("global").GetField(ref secondaryCircleString, "secondaryCircle");
            primaryCircle = GetIdentityCircle(primaryCircleString);
            secondaryCircle = GetIdentityCircle(secondaryCircleString);
        }

        Debug.Log("primaryCircle " + primaryCircle);
        Debug.Log("secondaryCircle " + secondaryCircle);
        SetLights(primaryCircle, secondaryCircle);

        // ========== Activity ==========

        // Freq
        float activityGlobalFreq = Random.value;
        float activityPrivateFreq = Random.value;
        float activityPublicFreq = Random.value;
        float activityProFreq = Random.value;

        if (data)
        {
            data.GetField("activity").GetField("globalData").GetField(ref activityGlobalFreq, "postFrequency");
            data.GetField("activity").GetField("privateData").GetField(ref activityPrivateFreq, "postFrequency");
            data.GetField("activity").GetField("publicData").GetField(ref activityPublicFreq, "postFrequency");
            data.GetField("activity").GetField("professionalData").GetField(ref activityProFreq, "postFrequency");
        }

        Color activityFreqMix = GenerateColor(activityPrivateFreq, activityPublicFreq, activityProFreq);

        LeftLowerArmModelSwitcher.currentValue = activityGlobalFreq * 100f;
        LeftLowerArmPublicModelSwitcher.currentValue = activityPublicFreq * 100f;
        LeftLowerArmPrivateModelSwitcher.currentValue = activityPrivateFreq * 100f;
        LeftLowerArmProModelSwitcher.currentValue = activityProFreq * 100f;
        SetColor(LeftLowerArmMaterial, activityFreqMix);

        RightLowerArmModelSwitcher.currentValue = activityGlobalFreq * 100f;
        RightLowerArmPublicModelSwitcher.currentValue = activityPublicFreq * 100f;
        RightLowerArmPrivateModelSwitcher.currentValue = activityPrivateFreq * 100f;
        LeftUpperArmProModelSwitcher.currentValue = activityProFreq * 100f;
        SetColor(RightLowerArmMaterial, activityFreqMix);

        LeftUpperArmModelSwitcher.currentValue = activityGlobalFreq * 100f;
        LeftUpperArmPublicModelSwitcher.currentValue = activityPublicFreq * 100f;
        LeftUpperArmPrivateModelSwitcher.currentValue = activityPrivateFreq * 100f;
        LeftUpperArmProModelSwitcher.currentValue = activityProFreq * 100f;
        SetColor(LeftUpperArmMaterial, activityFreqMix);

        // RightUpperArmModelSwitcher.currentValue = activityGlobalFreq * 100f;
        RightUpperArmPublicModelSwitcher.currentValue = activityPublicFreq * 100f;
        RightUpperArmPrivateModelSwitcher.currentValue = activityPrivateFreq * 100f;
        RightUpperArmProModelSwitcher.currentValue = activityProFreq * 100f;
        SetColor(RightUpperArmMaterial, activityFreqMix);

        // Volume
        float activityGlobalVol = Random.value;
        float activityPrivateVol = Random.value;
        float activityPublicVol = Random.value;
        float activityProVol = Random.value;

        if (data)
        {
            data.GetField("activity").GetField("globalData").GetField(ref activityGlobalVol, "volumePosts");
            data.GetField("activity").GetField("privateData").GetField(ref activityPrivateVol, "volumePosts");
            data.GetField("activity").GetField("publicData").GetField(ref activityPublicVol, "volumePosts");
            data.GetField("activity").GetField("professionalData").GetField(ref activityProVol, "volumePosts");
        }


        SetBones(activityGlobalFreq, activityPublicVol, activityPrivateVol, activityProVol);

        // Photos posts volume
        float activityPrivatePhotoVol = Random.value;
        float activityPublicPhotoVol = Random.value;
        float activityProPhotoVol = Random.value;

        if (data)
        {
            data.GetField("activity").GetField("privateData").GetField(ref activityPrivatePhotoVol, "volumePhotos");
            data.GetField("activity").GetField("publicData").GetField(ref activityPublicPhotoVol, "volumePhotos");
            data.GetField("activity").GetField("professionalData").GetField(ref activityProPhotoVol, "volumePhotos");
        }

        LeftElbowModelSwitcher.currentValue = activityPrivatePhotoVol * 100f;
        RightElbowModelSwitcher.currentValue = activityPrivatePhotoVol * 100f;

        LeftBottomShoulderModelSwitcher.currentValue = activityPublicPhotoVol * 100f;
        RightBottomShoulderModelSwitcher.currentValue = activityPublicPhotoVol * 100f;

        // ========== Influence ==========

        float influenceGlobalScore = Random.value;
        float influencePrivateScore = Random.value;
        float influencePublicScore = Random.value;
        float influenceProScore = Random.value;

        if (data)
        {
            data.GetField("influence").GetField("globalData").GetField(ref influenceGlobalScore, "influence");
            data.GetField("influence").GetField("privateData").GetField(ref influencePrivateScore, "influence");
            data.GetField("influence").GetField("publicData").GetField(ref influencePublicScore, "influence");
            data.GetField("influence").GetField("professionalData").GetField(ref influenceProScore, "influence");
        }

        Color influenceScoreMix = GenerateColor(influencePrivateScore, influencePublicScore, influenceProScore, 0.6f);

        FollowersParticles.particleCount = (int)(influenceGlobalScore * maxParticles);
        SetColor(FollowersParticles.particleSystemRenderer.material, influenceScoreMix);

        // ========== Mood ==========

        // Expressivity
        float moodGlobalExpressivity = Random.value;
        float moodPrivateExpressivity = Random.value;
        float moodPublicExpressivity = Random.value;
        float moodProExpressivity = Random.value;

        if (data)
        {
            data.GetField("mood").GetField("globalData").GetField(ref moodGlobalExpressivity, "expressivity");
            data.GetField("mood").GetField("privateData").GetField(ref moodPrivateExpressivity, "expressivity");
            data.GetField("mood").GetField("publicData").GetField(ref moodPublicExpressivity, "expressivity");
            data.GetField("mood").GetField("professionalData").GetField(ref moodProExpressivity, "expressivity");
        }

        Color moodExpressivityMix = GenerateColor(moodPrivateExpressivity, moodPublicExpressivity, moodProExpressivity, 0.6f);

        LeftLungModelSwitcher.currentValue = moodGlobalExpressivity * 100f;
        SetColor(LeftLungMaterial, moodExpressivityMix);

        RightLungModelSwitcher.currentValue = moodGlobalExpressivity * 100f;
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
        Transform[] splineBones = Spline.CreateSpline((int)(10f + activityFreqValue * 20f));

        for (int i = 0; i < splineBones.Length; i++)
        {
            splineBones[i].transform.FindChild("Public").localScale = new Vector3(1f + publicVol * 5f, 1, 1);
            splineBones[i].transform.FindChild("Private").localScale = new Vector3(1f + privateVol * 5f, 1, 1);
            splineBones[i].transform.FindChild("Pro").localScale = new Vector3(1f + proVol * 5f, 1, 1);
        }
    }

    void SetLights(IdentityCircle primaryCircle, IdentityCircle secondaryCircle)
    {
        if (primaryCircle == IdentityCircle.Private)
        {
            PrivatePrimaryLight.SetActive(true);
            PublicPrimaryLight.SetActive(false);
            ProPrimaryLight.SetActive(false);
        }
        else if (primaryCircle == IdentityCircle.Public)
        {
            PrivatePrimaryLight.SetActive(false);
            PublicPrimaryLight.SetActive(true);
            ProPrimaryLight.SetActive(false);
        }
        else if (primaryCircle == IdentityCircle.Pro)
        {
            PrivatePrimaryLight.SetActive(false);
            PublicPrimaryLight.SetActive(false);
            ProPrimaryLight.SetActive(true);
        }

        if (secondaryCircle == IdentityCircle.Private)
        {
            PrivateSecondaryLight.SetActive(true);
            PublicSecondaryLight.SetActive(false);
            ProSecondaryLight.SetActive(false);
        }
        else if (secondaryCircle == IdentityCircle.Public)
        {
            PrivateSecondaryLight.SetActive(false);
            PublicSecondaryLight.SetActive(true);
            ProSecondaryLight.SetActive(false);
        }
        else if (secondaryCircle == IdentityCircle.Pro)
        {
            PrivateSecondaryLight.SetActive(false);
            PublicSecondaryLight.SetActive(false);
            ProSecondaryLight.SetActive(true);
        }
    }

    Color GenerateColor(float privatePart, float publicPart, float proPart, float brightness = -1f)
    {
        publicPart = publicPart / (publicPart + privatePart + proPart);
        privatePart = privatePart / (publicPart + privatePart + proPart);
        proPart = proPart / (publicPart + privatePart + proPart);

        HSBColor hsb = new HSBColor(PrivateColorHSB.h * privatePart + PublicColorHSB.h * publicPart + ProColorHSB.h * proPart,
            PrivateColorHSB.s * privatePart + PublicColorHSB.s * publicPart + ProColorHSB.s * proPart,
            PrivateColorHSB.b * privatePart + PublicColorHSB.b * publicPart + ProColorHSB.b * proPart);

        if (brightness >= 0)
        {
            hsb.b = 1;
            hsb.s = 1 - brightness;
        }

        return hsb.ToColor();
    }

    IdentityCircle GetIdentityCircle(string circleString)
    {
        return  circleString == "private" ? IdentityCircle.Private :
                circleString == "public" ? IdentityCircle.Public :
                circleString == "pro" ? IdentityCircle.Pro :
                IdentityCircle.None;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "UpdateData"))
            UpdateData(null);

        if (GUI.Button(new Rect(10, 60, 150, 50), "Clean mem"))
            Resources.UnloadUnusedAssets();
    }
}
