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

    TransformModifier LeftElbowModifier;
    TransformModifier RightElbowModifier;

    ModelSwitcher LeftBottomShoulderModelSwitcher;
    ModelSwitcher RightBottomShoulderModelSwitcher;
    ModelSwitcher LeftOuterBottomShoulderModelSwitcher;
    ModelSwitcher RightOuterBottomShoulderModelSwitcher;

    // Passive Identity
    ModelSwitcher LeftPrivateTopShoulderModelSwitcher;
    ModelSwitcher LeftPublicTopShoulderModelSwitcher;
    ModelSwitcher LeftProTopShoulderModelSwitcher;

    ModelSwitcher RightTopShoulderModelSwitcher;
    TransformModifier RightTopShoulderModifier;

    TransformModifier LeftPrivateTopShoulderInnerModifier;
    TransformModifier LeftPublicTopShoulderInnerModifier;
    TransformModifier LeftProTopShoulderInnerModifier;

    TransformModifier LeftPrivateTopShoulderOuterModifier;
    TransformModifier LeftPublicTopShoulderOuterModifier;
    TransformModifier LeftProTopShoulderOuterModifier;

    TransformModifier RightTopShoulderInnerModifier;

    // Influence
    PlaygroundParticlesC FollowersParticles;
    ManipulatorObjectC FollowersGravitationalManipulator;
    ManipulatorObjectC FollowersRepellentManipulator;

    ModelSwitcher HeadInnerLeftModelSwitcher;
    ModelSwitcher HeadInnerRightModelSwitcher;
    ModelSwitcher HeadCylinderModelSwitcher;

    TransformModifier HeadInnerLeftModifier;
    TransformModifier HeadInnerRightModifier;
    TransformModifier HeadCylinderModifier;

    ModelSwitcher LeftNeckModelSwitcher;
    ModelSwitcher RightNeckModelSwitcher;

    TransformModifier LeftNeckModifier;
    TransformModifier RightNeckModifier;

    // Mood
    ModelSwitcher LeftLungModelSwitcher;
    ModelSwitcher RightLungModelSwitcher;

    // Interests
    ModelSwitcher StomachModelSwitcher;
    TransformModifier StomachSphereModifier;
    RotateAround StomachSphereRotate;

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

        LeftElbowModifier = GameObject.Find("LeftElbow/Elbow Model/Inner").GetComponent<TransformModifier>();
        RightElbowModifier = GameObject.Find("RightElbow/Elbow Model/Inner").GetComponent<TransformModifier>();

        LeftBottomShoulderModelSwitcher = GameObject.Find("LeftShoulder/Full Shoulder/Bottom Shoulder Model/Inner").GetComponent<ModelSwitcher>();
        RightBottomShoulderModelSwitcher = GameObject.Find("RightShoulder/Full Shoulder/Bottom Shoulder Model/Inner").GetComponent<ModelSwitcher>();
        LeftOuterBottomShoulderModelSwitcher = GameObject.Find("LeftShoulder/Full Shoulder/Bottom Shoulder Model/Outer").GetComponent<ModelSwitcher>();
        RightOuterBottomShoulderModelSwitcher = GameObject.Find("RightShoulder/Full Shoulder/Bottom Shoulder Model/Outer").GetComponent<ModelSwitcher>();

        // Passive Identity
        LeftPrivateTopShoulderModelSwitcher = GameObject.Find("LeftShoulder/Full Shoulder/Left Top Shoulder Model/Private/Inner").GetComponent<ModelSwitcher>();
        LeftPublicTopShoulderModelSwitcher = GameObject.Find("LeftShoulder/Full Shoulder/Left Top Shoulder Model/Public/Inner").GetComponent<ModelSwitcher>();
        LeftProTopShoulderModelSwitcher = GameObject.Find("LeftShoulder/Full Shoulder/Left Top Shoulder Model/Pro/Inner").GetComponent<ModelSwitcher>();

        RightTopShoulderModelSwitcher = GameObject.Find("RightShoulder/Full Shoulder/Right Top Shoulder Model/Inner").GetComponent<ModelSwitcher>();
        RightTopShoulderModifier = GameObject.Find("RightShoulder/Full Shoulder/Right Top Shoulder Model/Inner").GetComponent<TransformModifier>();

        LeftPrivateTopShoulderInnerModifier = GameObject.Find("LeftShoulder/Full Shoulder/Left Top Shoulder Model/Private/Inner2").GetComponent<TransformModifier>();
        LeftPublicTopShoulderInnerModifier = GameObject.Find("LeftShoulder/Full Shoulder/Left Top Shoulder Model/Public/Inner2").GetComponent<TransformModifier>();
        LeftProTopShoulderInnerModifier = GameObject.Find("LeftShoulder/Full Shoulder/Left Top Shoulder Model/Pro/Inner2").GetComponent<TransformModifier>();

        LeftPrivateTopShoulderOuterModifier = GameObject.Find("LeftShoulder/Full Shoulder/Left Top Shoulder Model/Private/Outer").GetComponent<TransformModifier>();
        LeftPublicTopShoulderOuterModifier = GameObject.Find("LeftShoulder/Full Shoulder/Left Top Shoulder Model/Public/Outer").GetComponent<TransformModifier>();
        LeftProTopShoulderOuterModifier = GameObject.Find("LeftShoulder/Full Shoulder/Left Top Shoulder Model/Pro/Outer").GetComponent<TransformModifier>();

        RightTopShoulderInnerModifier = GameObject.Find("/Right Top Shoulder Inner 2/Inner").GetComponent<TransformModifier>();

        // Influence
        FollowersParticles = GameObject.Find("Head/Followers/Followers Particles System").GetComponent<PlaygroundParticlesC>();
        FollowersGravitationalManipulator = FollowersParticles.manipulators[0];
        FollowersRepellentManipulator = FollowersParticles.manipulators[1];

        HeadInnerLeftModelSwitcher = GameObject.Find("Head/Head Model/InnerLeft").GetComponent<ModelSwitcher>();
        HeadInnerRightModelSwitcher = GameObject.Find("Head/Head Model/InnerRight").GetComponent<ModelSwitcher>();
        HeadCylinderModelSwitcher = GameObject.Find("Head/Head Model/Cylinder").GetComponent<ModelSwitcher>();

        HeadInnerLeftModifier = GameObject.Find("Head/Head Model/InnerLeft").GetComponent<TransformModifier>();
        HeadInnerRightModifier = GameObject.Find("Head/Head Model/InnerRight").GetComponent<TransformModifier>();
        HeadCylinderModifier = GameObject.Find("Head/Head Model/Cylinder").GetComponent<TransformModifier>();

        LeftNeckModelSwitcher = GameObject.Find("/Left Neck Model/Inner").GetComponent<ModelSwitcher>();
        RightNeckModelSwitcher = GameObject.Find("/Right Neck Model/Inner").GetComponent<ModelSwitcher>();

        LeftNeckModifier = GameObject.Find("/Left Neck Model/Outer").GetComponent<TransformModifier>();
        RightNeckModifier = GameObject.Find("/Right Neck Model/Outer").GetComponent<TransformModifier>();

        // Mood
        LeftLungModelSwitcher = GameObject.Find("Left Lung Model/Inner").GetComponent<ModelSwitcher>();
        RightLungModelSwitcher = GameObject.Find("Right Lung Model/Inner").GetComponent<ModelSwitcher>();

        // Interests 
        StomachModelSwitcher = GameObject.Find("/Stomach Model/Inner").GetComponent<ModelSwitcher>();
        StomachSphereModifier = GameObject.Find("/Stomach Model/Sphere").GetComponent<TransformModifier>();
        StomachSphereRotate = GameObject.Find("/Stomach Model/Sphere").GetComponent<RotateAround>();

        Spline = GameObject.Find("Spline Model").GetComponent<SplineDecorator>();

        UpdateData(null);
    }
	
    public void UpdateData(JSONObject data)
    {
        // ========== General ==========

        IdentityCircle primaryCircle = (IdentityCircle)Random.Range(1, 4);
        IdentityCircle secondaryCircle = (IdentityCircle)Random.Range(1, 4);

        if (data)
        {
            string primaryCircleString = "";
            string secondaryCircleString = "";

            data.GetField("general").GetField("globalData").GetField(ref primaryCircleString, "primaryCircle");
            data.GetField("general").GetField("globalData").GetField(ref secondaryCircleString, "secondaryCircle");
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
            data.GetField("activity").GetField("globalData").GetField(ref activityGlobalFreq, "postFrequencyScore");
            data.GetField("activity").GetField("privateData").GetField(ref activityPrivateFreq, "postFrequencyScore");
            data.GetField("activity").GetField("publicData").GetField(ref activityPublicFreq, "postFrequencyScore");
            data.GetField("activity").GetField("professionalData").GetField(ref activityProFreq, "postFrequencyScore");
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
        RightLowerArmProModelSwitcher.currentValue = activityProFreq * 100f;
        SetColor(RightLowerArmMaterial, activityFreqMix);

        LeftUpperArmModelSwitcher.currentValue = activityGlobalFreq * 100f;
        LeftUpperArmPublicModelSwitcher.currentValue = activityPublicFreq * 100f;
        LeftUpperArmPrivateModelSwitcher.currentValue = activityPrivateFreq * 100f;
        LeftUpperArmProModelSwitcher.currentValue = activityProFreq * 100f;
        SetColor(LeftUpperArmMaterial, activityFreqMix);

        RightUpperArmModelSwitcher.currentValue = activityGlobalFreq * 100f;
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

        LeftElbowModifier.CurrentValue = activityPrivatePhotoVol * 100f;
        RightElbowModifier.CurrentValue = activityPrivatePhotoVol * 100f;

        LeftBottomShoulderModelSwitcher.currentValue = activityPublicPhotoVol * 100f;
        RightBottomShoulderModelSwitcher.currentValue = activityPublicPhotoVol * 100f;

        LeftOuterBottomShoulderModelSwitcher.currentValue = activityPublicPhotoVol * 100f;
        RightOuterBottomShoulderModelSwitcher.currentValue = activityPublicPhotoVol * 100f;

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
        FollowersGravitationalManipulator.size = Mathf.Lerp(1f, 6f, influenceGlobalScore);
        FollowersRepellentManipulator.strength = Mathf.Lerp(.5f, 4f, influenceGlobalScore);

        LeftNeckModelSwitcher.currentValue = influenceGlobalScore * 100f;
        RightNeckModelSwitcher.currentValue = influenceGlobalScore * 100f;

        LeftNeckModifier.CurrentValue = influenceGlobalScore * 100f;
        RightNeckModifier.CurrentValue = influenceGlobalScore * 100f;

        SetColor(FollowersParticles.particleSystemRenderer.material, influenceScoreMix);

        // Nb of likes
        float passiveIdPrivateNbOfLikes = Random.value;
        float passiveIdPublicNbOfLikes = Random.value;
        float passiveIdProNbOfLikes = Random.value;

        if (data)
        {
            data.GetField("influence").GetField("privateData").GetField(ref passiveIdPrivateNbOfLikes, "likesScore");
            data.GetField("influence").GetField("publicData").GetField(ref passiveIdPublicNbOfLikes, "likesScore");
            data.GetField("influence").GetField("professionalData").GetField(ref passiveIdProNbOfLikes, "likesScore");
        }

        HeadInnerLeftModelSwitcher.currentValue = passiveIdPrivateNbOfLikes * 100f;
        HeadInnerRightModelSwitcher.currentValue = passiveIdPublicNbOfLikes * 100f;
        HeadCylinderModelSwitcher.currentValue = passiveIdProNbOfLikes * 100f;

        HeadInnerLeftModifier.CurrentValue = passiveIdPrivateNbOfLikes * 100f;
        HeadInnerRightModifier.CurrentValue = passiveIdPublicNbOfLikes * 100f;
        HeadCylinderModifier.CurrentValue = passiveIdProNbOfLikes * 100f;


        //  ========== Passive Identity ==========

        float passiveIdGlobalScore = Random.value;
        float passiveIdPrivateScore = Random.value;
        float passiveIdPublicScore = Random.value;
        float passiveIdProScore = Random.value;

        if (data)
        {
            data.GetField("passiveIdentity").GetField("globalData").GetField(ref passiveIdGlobalScore, "score");
            data.GetField("passiveIdentity").GetField("privateData").GetField(ref passiveIdPrivateScore, "score");
            data.GetField("passiveIdentity").GetField("publicData").GetField(ref passiveIdPublicScore, "score");
            data.GetField("passiveIdentity").GetField("professionalData").GetField(ref passiveIdProScore, "score");
        }

        LeftPrivateTopShoulderModelSwitcher.currentValue = passiveIdPrivateScore * 100f;
        LeftPublicTopShoulderModelSwitcher.currentValue = passiveIdPublicScore * 100f;
        LeftProTopShoulderModelSwitcher.currentValue = passiveIdProScore * 100f;

        RightTopShoulderModelSwitcher.currentValue = passiveIdGlobalScore * 100f;
        RightTopShoulderModifier.CurrentValue = passiveIdGlobalScore * 100f;

        LeftPrivateTopShoulderInnerModifier.CurrentValue = passiveIdPrivateScore * 100f;
        LeftPublicTopShoulderInnerModifier.CurrentValue = passiveIdPublicScore * 100f;
        LeftProTopShoulderInnerModifier.CurrentValue = passiveIdProScore * 100f;

        LeftPrivateTopShoulderOuterModifier.CurrentValue = passiveIdPrivateScore * 100f;
        LeftPublicTopShoulderOuterModifier.CurrentValue = passiveIdPublicScore * 100f;
        LeftProTopShoulderOuterModifier.CurrentValue = passiveIdProScore * 100f;

        RightTopShoulderInnerModifier.CurrentValue = passiveIdGlobalScore * 100f;

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
        RightLungModelSwitcher.currentValue = moodGlobalExpressivity * 100f;

        // ========== Interests ==========

        float hobbiesVolume = Random.value;

        if (data)
        {
            data.GetField("hobbies").GetField("globalData").GetField(ref hobbiesVolume, "hobbiesVolume");
        }

        StomachModelSwitcher.currentValue = hobbiesVolume * 100f;
        StomachSphereModifier.CurrentValue = hobbiesVolume * 100f;
        StomachSphereRotate.speed = Mathf.Lerp(80f, 20f, hobbiesVolume);

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
            splineBones[i].transform.FindChild("Public").localScale = new Vector3(Mathf.Lerp(1f, 3f, publicVol), 1f, 1f);
            splineBones[i].transform.FindChild("Private").localScale = new Vector3(Mathf.Lerp(0.2f, 5f, privateVol), 2f, 1f);
            splineBones[i].transform.FindChild("Pro").localScale = new Vector3(0.2f, Mathf.Lerp(3.5f, 6f, proVol), 0.2f);
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
