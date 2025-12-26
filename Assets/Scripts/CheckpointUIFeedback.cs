using UnityEngine;
using TMPro;
using System.Collections;

public class CheckpointUIFeedback : MonoBehaviour
{
    public static CheckpointUIFeedback Instance;

    public TMP_Text checkpointText;
    public float fadeInTime = 0.2f;
    public float holdTime = 1.2f;
    public float fadeOutTime = 0.4f;

    void Awake()
    {
        Instance = this;
        checkpointText.alpha = 0f;
    }

    public void ShowCheckpointText()
    {
        StopAllCoroutines();
        StartCoroutine(CheckpointRoutine());
    }

    IEnumerator CheckpointRoutine()
    {
        // Fade in
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            checkpointText.alpha = Mathf.Lerp(0, 1, t / fadeInTime);
            yield return null;
        }

        checkpointText.alpha = 1f;
        yield return new WaitForSeconds(holdTime);

        // Fade out
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            checkpointText.alpha = Mathf.Lerp(1, 0, t / fadeOutTime);
            yield return null;
        }

        checkpointText.alpha = 0f;
    }
}
