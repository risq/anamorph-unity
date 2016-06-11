using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HUDGroup : MonoBehaviour {

    public GameObject linkedFocusCamera;

    HUDPane[] panes;
    int panesCount;
    CanvasGroup canvasGroup;
    Tweener tweener;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        panes = GetComponentsInChildren<HUDPane>();
        panesCount = panes.Length;
    }

    public void Show()
    {
        Debug.Log("Show hud: " + name);
        linkedFocusCamera.SetActive(true);

        canvasGroup.DOKill();
        canvasGroup.DOFade(1, 1);
    }

    public void Hide()
    {
        Debug.Log("Hide hud: " + name);
        linkedFocusCamera.SetActive(false);
    
        canvasGroup.DOKill();
        canvasGroup.DOFade(0, 1);
    }

    public void Filter(IdentityCircle circle)
    {
        for(int i = 0; i < panesCount; i++)
        {
            if (panes[i].circle == IdentityCircle.Global || panes[i].circle == circle)
            {
                panes[i].Unfold();
            }
            else
            {
                panes[i].Fold();
            }
        }
    }

}
