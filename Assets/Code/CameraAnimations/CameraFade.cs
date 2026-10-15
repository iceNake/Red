using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class CameraFade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration;

    public IEnumerator FadeOut()
    {
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            Color color = fadeImage.color;
            color.a = Mathf.Lerp(0, 1, time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            Color color = fadeImage.color;
            color.a = Mathf.Lerp(1, 0, time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }
}