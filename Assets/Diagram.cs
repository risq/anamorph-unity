using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Diagram : MonoBehaviour {
    public float privateValue;
    public float publicValue;
    public float proValue;

    public Image privateImage;
    public Image publicImage;
    public Image proImage;

    public Text privateText;
    public Text publicText;
    public Text proText;

    void Awake () {
        UpdateDiagram();
	}
	
	public void UpdateDiagram() {
        float privatePart = privateValue / (publicValue + privateValue + proValue);
        float publicPart = publicValue / (publicValue + privateValue + proValue);
        float proPart = proValue / (publicValue + privateValue + proValue);

        privateImage.fillAmount = privatePart;
        privateImage.transform.localRotation = Quaternion.Euler(Vector3.zero);
        privateText.text = Mathf.Round(privatePart * 100) + "%";

        publicImage.fillAmount = publicPart;
        publicImage.transform.localRotation = Quaternion.Euler(-Vector3.forward * 360 * privatePart);
        publicText.text = Mathf.Round(publicPart * 100) + "%";

        proImage.fillAmount = proPart;
        proImage.transform.localRotation = Quaternion.Euler(-Vector3.forward * 360 * (privatePart + publicPart));
        proText.text = Mathf.Round(proPart * 100) + "%";
    }
}
