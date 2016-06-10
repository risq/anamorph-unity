using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class HUDPane : MonoBehaviour {

    public IdentityCircle circle = IdentityCircle.None;

    Vector3 initialPosition;
    Vector3 moveTo;

    Tweener tweener;
    
    RectTransform rtr;
    CanvasGroup canvasGroup;

    TypingEffect[] typingEffects;

    bool folded = false;


    void Start () {
        rtr = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rtr.localPosition;

        Text[] texts = GetComponentsInChildren<Text>();
        typingEffects = new TypingEffect[texts.Length];

        for (int i = 0; i < texts.Length; i++)
        {
            typingEffects[i] = texts[i].gameObject.AddComponent<TypingEffect>();
        }     
    }
	
	void Update () {

	}

    public void Fold ()
    {
        //if (!folded)
        //{
            rtr.DOKill();
            rtr.DOLocalMove(new Vector3(0, 0, 100), 0.2f);
            canvasGroup.DOFade(0, 0.2f);
            folded = true;
        //}
    }

    public void Unfold()
    {
        //if (folded)
        //{
            rtr.DOKill();
            rtr.DOLocalMove(initialPosition, .5f);
            canvasGroup.DOFade(1, 1f);
            foreach (TypingEffect typingEffect in typingEffects)
            {
                typingEffect.StartTypingEffect();
            }
            folded = false;
        //}
    }
}
