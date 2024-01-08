using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class DamagePopup : MonoBehaviour
{
    public float riseSpeed = 0.5f;
    public float fadeOutTime = 1f;

    private TMP_Text damageText;

    private void Start()
    {
        damageText = GetComponent<TMP_Text>();
        StartCoroutine(FadeAndRise());
    }

    private IEnumerator FadeAndRise()
    {
        float elapsed = 0f;

        Color originalColor = damageText.color;
        Vector3 originalPosition = transform.position;

        while (elapsed < fadeOutTime)
        {
            float alpha = Mathf.Lerp(1, 0, elapsed / fadeOutTime);
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            transform.position = originalPosition + new Vector3(0, riseSpeed * elapsed, 0);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
