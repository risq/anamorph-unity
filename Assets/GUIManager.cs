using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

    enum HUD { None, Activity, Influence, PassiveIdentity, Mood, Interests };

    public GameObject activityHUD;
    public GameObject influenceHUD;
    public GameObject passiveIdentityHUD;
    public GameObject moodHUD;
    public GameObject interestsHUD;

    // Use this for initialization
    void Start () {
        SetActiveHUD(HUD.None);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetActiveHUD(HUD activeHUD)
    {
        if (activeHUD == HUD.Activity)
        {
            activityHUD.SetActive(true);
            influenceHUD.SetActive(false);
            passiveIdentityHUD.SetActive(false);
            moodHUD.SetActive(false);
            interestsHUD.SetActive(false);
        }
        else if (activeHUD == HUD.Influence)
        {
            activityHUD.SetActive(false);
            influenceHUD.SetActive(true);
            passiveIdentityHUD.SetActive(false);
            moodHUD.SetActive(false);
            interestsHUD.SetActive(false);
        }
        else if (activeHUD == HUD.PassiveIdentity)
        {
            activityHUD.SetActive(false);
            influenceHUD.SetActive(false);
            passiveIdentityHUD.SetActive(true);
            moodHUD.SetActive(false);
            interestsHUD.SetActive(false);
        }
        else if (activeHUD == HUD.Mood)
        {
            activityHUD.SetActive(false);
            influenceHUD.SetActive(false);
            passiveIdentityHUD.SetActive(false);
            moodHUD.SetActive(true);
            interestsHUD.SetActive(false);
        }
        else if (activeHUD == HUD.Interests)
        {
            activityHUD.SetActive(false);
            influenceHUD.SetActive(false);
            passiveIdentityHUD.SetActive(false);
            moodHUD.SetActive(false);
            interestsHUD.SetActive(true);
        }
        else
        {
            activityHUD.SetActive(false);
            influenceHUD.SetActive(false);
            passiveIdentityHUD.SetActive(false);
            moodHUD.SetActive(false);
            interestsHUD.SetActive(false);
        }
    }
}
