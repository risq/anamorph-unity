using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HUDPane : MonoBehaviour {

    public IdentityCircle circle = IdentityCircle.None;

    Vector3 initialPosition;
    Vector3 moveTo;

    Tweener tweener;
    
    RectTransform rtr;
    CanvasGroup canvasGroup;
	
	void Start () {
        rtr = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rtr.localPosition;
    }
	
	void Update () {

	}

    public void Fold ()
    {
        rtr.DOKill();
        rtr.DOLocalMove(new Vector3(0, 0, 100), 0.2f);
        canvasGroup.DOFade(0, 0.2f);
    }

    public void Unfold()
    {
        rtr.DOKill();
        rtr.DOLocalMove(initialPosition, .5f);
        canvasGroup.DOFade(1, 1f);
    }
}
