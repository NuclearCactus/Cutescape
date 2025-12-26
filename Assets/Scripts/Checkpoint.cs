using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint current;

    private bool activated = false;

    private SpriteRenderer sr;
    private BobbingMotion bobbing;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        bobbing = GetComponent<BobbingMotion>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Player"))
        {
            activated = true;
            current = this;

            // UI feedback
            CheckpointUIFeedback.Instance.ShowCheckpointText();

            // Make fully visible
            if (sr != null)
            {
                StartCoroutine(FadeToOpaque());
            }

            // Stop bobbing motion
            if (bobbing != null)
                bobbing.StopBobbing();

            // Checkpoint sound
            SFXManager.Instance.PlaySFX(SFXManager.Instance.checkpointSound);

            // Optional: visual change (disable pickup look)
            // GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    IEnumerator FadeToOpaque(float duration = 0.3f)
    {
        float startAlpha = sr.color.a;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, 1f, t / duration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
            yield return null;
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }
}
