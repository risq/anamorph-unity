using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QRCodeFetcher : MonoBehaviour {
    string url = "http://localhost:8080/qr";

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
