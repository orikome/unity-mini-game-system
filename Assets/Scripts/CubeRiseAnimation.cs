using System.Collections;
using UnityEngine;

public class CubeRiseAnimation : MonoBehaviour
{
    public float animationDuration = 1f;
    private float delay = 0f;
    private Vector3 targetPosition;
    private bool hasAnimated = false;

    private void OnEnable()
    {
        if (!hasAnimated)
        {
            delay = Random.Range(0f, 0.5f);
            animationDuration = Random.Range(0.5f, 1f);
            StartCoroutine(AnimateRise());
        }
    }

    public void SetDelay(float newDelay)
    {
        delay = newDelay;
    }

    private IEnumerator AnimateRise()
    {
        hasAnimated = true;

        // Wait for the delay
        yield return new WaitForSeconds(delay);

        // Set target position (current position but at Y=0, assuming board is at Y=0)
        targetPosition = new Vector3(transform.position.x, 0f, transform.position.z);

        // Start position is current (which should be set to Y + riseHeight)
        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            t = Helpers.EaseOutCubic(t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // Ensure final position
        transform.position = targetPosition;
    }
}
