using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HUDGroup : MonoBehaviour {

    public GameObject linkedFocusCamera;

    HUDPane[] panes;
    int panesCount;
    CanvasGroup canvasGroup;
    Tweener tweener;

    void Start ()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        panes = GetComponentsInChildren<HUDPane>();
        panesCount = panes.Length;

        linkedFocusCamera.SetActive(false);
    }

    public void Show()
    {
        linkedFocusCamera.SetActive(true);
        tweener.Kill();
        tweener = canvasGroup.DOFade(1, 0.5f);
        for (int i = 0; i < panesCount; i++)
        {
            panes[i].Unfold();
        }
    }

    public void Hide()
    {
        linkedFocusCamera.SetActive(false);
        tweener = canvasGroup.DOFade(0, 0.5f);
        for (int i = 0; i < panesCount; i++)
        {
            panes[i].Fold();
        }
    }

}
