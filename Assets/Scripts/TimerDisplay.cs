using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Color normalColor = Color.white;

    [SerializeField]
    private Color warningColor = Color.red;

    [SerializeField]
    private float warningThreshold = 10f;

    private void Start()
    {
        if (Timer.Instance != null)
        {
            Timer.Instance.OnTimerTick += UpdateTimerDisplay;
        }
    }

    private void OnDestroy()
    {
        if (Timer.Instance != null)
        {
            Timer.Instance.OnTimerTick -= UpdateTimerDisplay;
        }
    }

    private void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"{minutes}:{seconds:00}";

        // Change color when time is running low
        if (time <= warningThreshold && time > 0f)
        {
            timerText.color = warningColor;
        }
        else
        {
            timerText.color = normalColor;
        }
    }
}
