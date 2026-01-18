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
        if (
            GameManager.Instance != null
            && time <= GameManager.Instance.Config.warningThreshold
            && time > 0f
        )
        {
            timerText.color = warningColor;
        }
        else
        {
            timerText.color = normalColor;
        }
    }
}
