using UnityEngine;
using System;
using System.Linq;

public abstract class Tween<T> : MonoBehaviour, ITween
{
    public event Action<T> OnUpdate;
    public event Action OnCompleted;

    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private T from;
    [SerializeField]
    private T to;
    [SerializeField]
    private float duration = 1f;
    [SerializeField]
    private float delay;
    [SerializeField]
    private bool playAtStart;

    private T CurrentFrom { get; set; }
    private T CurrentTo { get; set; }
    private float ElapsedTime { get; set; }
    private float DelayTime { get; set; }

    public bool IsValid => curve != null && curve.keys.Length > 0;
    public bool IsPlaying { get; set; }
    public float EndTime => curve.keys.Last().time;
    public float CurrentTime => ElapsedTime * (EndTime / duration);

    private void Start()
    {
        if (playAtStart)
            Play();
    }

    private void Update()
    {
        if (!IsPlaying)
            return;

        var deltaTime = Time.deltaTime;
        if (DelayTime > 0f)
        {
            DelayTime -= deltaTime;
            return;
        }

        var t = Evaluate(deltaTime);
        var value = Lerp(CurrentFrom, CurrentTo, t);
        SetValue(value);
        OnUpdate?.Invoke(value);
        CheckCompleted();
    }

    public void Initialize(T from, T to)
    {
        this.from = from;
        this.to = to;
    }

    public abstract T Lerp(T from, T to, float t);
    public abstract void SetValue(T value);

    private float Evaluate(float deltaTime)
    {
        if (!IsValid)
            return 0f;
        ElapsedTime += deltaTime;
        return curve.Evaluate(CurrentTime);
    }

    private void CheckCompleted()
    {
        if (CurrentTime >= EndTime && curve.postWrapMode == WrapMode.ClampForever)
        {
            Stop();
            OnCompleted?.Invoke();
        }
    }

    public void Play(bool reverse = false)
    {
        Reset(reverse);
        IsPlaying = true;
    }

    public void Stop()
    {
        IsPlaying = false;
    }

    public void Reset(bool reverse = false)
    {
        DelayTime = delay;
        ElapsedTime = 0;
        if (reverse)
        {
            CurrentFrom = to;
            CurrentTo = from;
        }
        else
        {
            CurrentFrom = from;
            CurrentTo = to;
        }

        SetValue(CurrentFrom);
    }
}
