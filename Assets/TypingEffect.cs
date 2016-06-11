using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypingEffect : MonoBehaviour {

    public float letterPause = 0.3f;
    Text textComponent;

    string message;
    string newMessage;
    char[] messageChars;
    char[] newMessageChars;

    bool done = true;

    string st = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_#@$";

    // Use this for initialization
    void Start()
    {
        textComponent = GetComponent<Text>();
        message = textComponent.text;
        messageChars = message.ToCharArray();
    }

    public void StartTypingEffect()
    {
        if (messageChars.Length > 0)
        {
            textComponent.text = "";
            newMessage = "";
            for (int i = 0; i < messageChars.Length; i++)
            {
                if (messageChars[i] == '\n' || messageChars[i] == ' ')
                    newMessage += messageChars[i];
                else
                    newMessage += st[Random.Range(0, st.Length)];
            }
            newMessageChars = newMessage.ToCharArray();
            textComponent.text = newMessage;
            done = false;
        }
        
    }

    void Update()
    {
        if (!done)
        {
            newMessage = "";
            for (int i = 0; i < messageChars.Length; i++)
            {
                if (messageChars[i] != '\n' && messageChars[i] != ' ' && newMessageChars[i] != messageChars[i] && Random.value > 0.1f)
                {
                    newMessage += st[Random.Range(0, st.Length)];
                }
                else
                {
                    newMessage += messageChars[i];
                }
            }
            newMessageChars = newMessage.ToCharArray();
            textComponent.text = newMessage;

            if (newMessage == message)
            {
                done = true;
            }
        }
    }



    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            textComponent.text += letter;

            yield return 0;
            yield return new WaitForSeconds(Random.Range(0, letterPause));
        }
    }
}
