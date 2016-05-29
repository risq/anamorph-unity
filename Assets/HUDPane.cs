using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HUDPane : MonoBehaviour {

    Vector3 initialPosition;
    Vector3 moveTo;

    Tweener tweener;
    
    RectTransform rtr;
	
	void Start () {
        rtr = GetComponent<RectTransform>();
        initialPosition = rtr.localPosition;
    }
	
	void Update () {

	}

    public void Fold ()
    {
        tweener.Kill();
        tweener = rtr.DOLocalMove(new Vector3(0, 0, 0), 0.5f);
    }

    public void Unfold()
    {
        tweener.Kill();
        tweener = rtr.DOLocalMove(initialPosition, 0.5f);
    }
}
