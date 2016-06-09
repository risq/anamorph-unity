using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QRCodeFetcher : MonoBehaviour {
    public string url = "http://anamorph.valentinledrapier.com/qr/1";

    IEnumerator Start()
    {
        // Start a download of the given URL
        WWW www = new WWW(url);

        // Wait for download to complete
        yield return www;

        // assign texture
        RawImage rawImage = GetComponent<RawImage>();
        rawImage.texture = www.texture;
    }
}
