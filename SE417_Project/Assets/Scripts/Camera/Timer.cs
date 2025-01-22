using Signals;
using TMPro;
using UnityEngine;

namespace Camera
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private float duration;

        private float _timeRemaining;
        private bool _isCountingDown = false;

        private void OnEnable()
        {
            CoreGameSignals.Instance.OnGameStart += StartTimer;
            CoreGameSignals.Instance.OnPlayerDie += () => _isCountingDown = false;
            CoreGameSignals.Instance.OnCompleteLevel += () => _isCountingDown = false;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.OnGameStart -= StartTimer;
            CoreGameSignals.Instance.OnPlayerDie -= () => _isCountingDown = false;
            CoreGameSignals.Instance.OnCompleteLevel -= () => _isCountingDown = false;
        }
        
        private void Update()
        {
            if (_isCountingDown)
            {
                if (_timeRemaining > 0)
                {
                    _timeRemaining -= Time.deltaTime;
                    UpdateTimerText();
                }
                else
                {
                    _timeRemaining = 0;
                    CoreGameSignals.Instance.OnPlayerDie?.Invoke();
                    UpdateTimerText();
                    //TimerEnded();
                }
            }
        }

        private void StartTimer()
        {
            _timeRemaining = duration;
            _isCountingDown = true;
            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(_timeRemaining / 60);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60);
            timerText.text = $"Time\n{minutes:00}:{seconds:00}";
        }
    }
}
