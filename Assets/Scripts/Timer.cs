using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    public event Action OnTimeUp;
    public event Action<float> OnTimerTick;

    public float RemainingTime { get; private set; }
    public bool IsRunning { get; private set; }

    private float duration;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (!IsRunning)
            return;

        RemainingTime -= Time.deltaTime;
        OnTimerTick?.Invoke(RemainingTime);

        if (RemainingTime <= 0f)
        {
            RemainingTime = 0f;
            IsRunning = false;
            OnTimeUp?.Invoke();
        }
    }

    public void StartTimer(float duration)
    {
        this.duration = duration;
        RemainingTime = duration;
        IsRunning = true;
    }

    public void StopTimer()
    {
        IsRunning = false;
        RemainingTime = 0f;
    }

    public void PauseTimer()
    {
        IsRunning = false;
    }

    public void ResumeTimer()
    {
        if (RemainingTime > 0f)
        {
            IsRunning = true;
        }
    }

    public void ResetTimer()
    {
        RemainingTime = duration;
        IsRunning = false;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
