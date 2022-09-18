using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class SimpleTimer 
{
    private MonoBehaviour _monobeh;

    private bool _isTimerOn = false;
    private float _curTime;

    public event Action<float> OnTimeChange;
    public event Action OnTimerEnd;

    public SimpleTimer(MonoBehaviour monobeh)
    {
        _monobeh = monobeh;
    }

    public void StopTimer()
    {
        _monobeh?.StopAllCoroutines();
        _isTimerOn = false;
    }

    public void EnableTimer(float time)
    {
        if (!_isTimerOn)
        {
            _curTime = time;
            _monobeh.StartCoroutine(Timer(time));
        }
    }

    private IEnumerator Timer(float time)
    {
        _isTimerOn = true;
        for (float i = time; 0 <= _curTime; i -= Time.deltaTime)
        {
            _curTime = i;
            OnTimeChange?.Invoke(i);
            yield return new WaitForEndOfFrame();
        }
        _isTimerOn = false;
        OnTimerEnd?.Invoke();
    }
}
