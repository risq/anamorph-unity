using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class HUDScreen : MonoBehaviour {

    public CanvasGroup canvasGroup;

    TypingEffect[] typingEffects;

    // Use this for initialization
    void Awake ()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Text[] texts = GetComponentsInChildren<Text>();
        typingEffects = new TypingEffect[texts.Length];

        for (int i = 0; i < texts.Length; i++)
        {
            typingEffects[i] = texts[i].gameObject.AddComponent<TypingEffect>();
            typingEffects[i].speed = 0.05f;
        }
    }

    public Tweener FadeIn()
    {
        canvasGroup.DOKill();
        Glitch();
        return canvasGroup.DOFade(1, 1);
    }

    public Tweener FadeOut()
    {
        canvasGroup.DOKill();
        return canvasGroup.DOFade(0, 1);
    }

    void Glitch()
    {
        foreach (TypingEffect typingEffect in typingEffects)
        {
            typingEffect.StartTypingEffect();
        }
    }
}
