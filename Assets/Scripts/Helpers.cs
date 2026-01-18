using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    // Makes animations start fast and slow down at the end for a more natural feel.
    public static float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }

    // Calculate all valid knight moves from a position on the board
    public static HashSet<Vector2Int> GetKnightMoves(Vector2Int pos, int boardSize)
    {
        HashSet<Vector2Int> moves = new HashSet<Vector2Int>();
        int[] dx = { 2, 2, -2, -2, 1, 1, -1, -1 };
        int[] dz = { 1, -1, 1, -1, 2, -2, 2, -2 };

        for (int i = 0; i < 8; i++)
        {
            int nx = pos.x + dx[i];
            int nz = pos.y + dz[i];
            if (nx >= 0 && nx < boardSize && nz >= 0 && nz < boardSize)
            {
                moves.Add(new Vector2Int(nx, nz));
            }
        }
        return moves;
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
