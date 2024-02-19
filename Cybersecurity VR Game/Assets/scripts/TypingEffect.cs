using System.Collections;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    public float typeSpeed = 0.05f;  // Time delay between characters. You can adjust this for faster/slower typing.
    private TMP_Text textComponent;
    private string fullText;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private Coroutine typingCoroutine = null;

public void StartTyping(string message)
{

    if (typingCoroutine != null)
    {
        StopCoroutine(typingCoroutine);
    }
    fullText = message;
    typingCoroutine = StartCoroutine(TypeOutText());
}


    private IEnumerator TypeOutText()
    {
        textComponent.text = "";  // Clear existing text
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;  // Add next character
            yield return new WaitForSeconds(typeSpeed);  // Wait before typing the next character
        }
    }
}
