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
        linkedFocusCamera.SetActive(true);

        canvasGroup.DOKill();
        canvasGroup.DOFade(1, 1);
    }

    public void Hide()
    {
        linkedFocusCamera.SetActive(false);
    
        canvasGroup.DOKill();
        canvasGroup.DOFade(0, 1);
    }

}
