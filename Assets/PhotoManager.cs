using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhotoManager : MonoBehaviour {

    public RawImage photoView;

    Animator animator;
    AudioManager audioManager;
    GlitchEffect cameraGlitchEffect;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
        cameraGlitchEffect = Camera.main.GetComponent<GlitchEffect>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakePhoto()
    {
        animator.SetTrigger("photo");
    }

    public void OnTakePhoto()
    {
        cameraGlitchEffect.enabled = false;
        Texture2D tex = new Texture2D(1080, 1080, TextureFormat.RGB24, false);

        Camera activeCamera = Camera.main;

        RenderTexture rt = new RenderTexture(1080, 1080, 24);
        activeCamera.targetTexture = rt;
        activeCamera.Render();
        RenderTexture.active = rt;

        tex.ReadPixels(new Rect(0, 0, 1080, 1080), 0, 0);
        tex.Apply();
        activeCamera.targetTexture = null;

        photoView.texture = tex;

        audioManager.PlayPhotoSound();
        audioManager.AfterPhotoSoundtrack();
    }
}
