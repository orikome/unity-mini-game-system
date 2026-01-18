using System.Collections;
using UnityEngine;

public static class Helpers
{
    // Makes animations start fast and slow down at the end for a more natural feel.
    public static float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }

    public static IEnumerator AnimateTransition(
        CanvasGroup from,
        CanvasGroup to,
        float duration = 0.5f
    )
    {
        // Prepare 'from' for fade out
        from.interactable = false;
        from.blocksRaycasts = false;
        RectTransform rtFrom = from.GetComponent<RectTransform>();
        Vector3 originalScaleFrom = rtFrom.localScale;

        // Prepare 'to' for fade in
        to.gameObject.SetActive(true);
        to.alpha = 0;
        to.interactable = false;
        to.blocksRaycasts = false;
        RectTransform rtTo = to.GetComponent<RectTransform>();
        Vector3 originalScaleTo = rtTo.localScale;
        rtTo.localScale = originalScaleTo * 0.9f;

        float halfDuration = duration / 2;
        float time = 0;

        // Fade out 'from'
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            float t = time / halfDuration;
            float eased = EaseOutCubic(t);
            from.alpha = 1 - eased;
            rtFrom.localScale = originalScaleFrom * (1 - 0.1f * eased);
            yield return null;
        }
        from.alpha = 0;
        rtFrom.localScale = originalScaleFrom;
        from.gameObject.SetActive(false);

        // Fade in 'to'
        time = 0;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            float t = time / halfDuration;
            float eased = EaseOutCubic(t);
            to.alpha = eased;
            rtTo.localScale = originalScaleTo * (0.9f + 0.1f * eased);
            yield return null;
        }
        to.alpha = 1;
        rtTo.localScale = originalScaleTo;
        to.interactable = true;
        to.blocksRaycasts = true;
    }
}
