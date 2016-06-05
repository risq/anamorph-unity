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

    void Awake () {
        UpdateDiagram();
	}
	
	public void UpdateDiagram() {
        float privatePart = privateValue / (publicValue + privateValue + proValue);
        float publicPart = publicValue / (publicValue + privateValue + proValue);
        float proPart = proValue / (publicValue + privateValue + proValue);

        privateImage.fillAmount = privatePart;
        privateImage.transform.localRotation = Quaternion.Euler(Vector3.zero);

        publicImage.fillAmount = publicPart;
        publicImage.transform.localRotation = Quaternion.Euler(-Vector3.forward * 360 * privatePart);

        proImage.fillAmount = proPart;
        proImage.transform.localRotation = Quaternion.Euler(-Vector3.forward * 360 * (privatePart + publicPart));
    }
}
