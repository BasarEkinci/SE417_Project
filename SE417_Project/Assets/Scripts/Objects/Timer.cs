using Signals;
using TMPro;
using UnityEngine;

namespace Objects
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        private float _duration;
        private float _timeRemaining;
        private bool _isCountingDown = false;
        
        private void OnEnable()
        {
            _duration = PlayerPrefs.GetFloat("GameTime");
            CoreGameSignals.Instance.OnGameStart += StartTimer;
            CoreGameSignals.Instance.OnPlayerDie += () => _isCountingDown = false;
            CoreGameSignals.Instance.OnCompleteLevel += () => _isCountingDown = false;
            CoreGameSignals.Instance.OnCompleteObjective += () => _isCountingDown = false;
            CoreGameSignals.Instance.OnPauseGame += (condition) => _isCountingDown = !condition;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.OnGameStart -= StartTimer;
            CoreGameSignals.Instance.OnPlayerDie -= () => _isCountingDown = false;
            CoreGameSignals.Instance.OnCompleteLevel -= () => _isCountingDown = false;
            CoreGameSignals.Instance.OnCompleteObjective -= () => _isCountingDown = false;
            CoreGameSignals.Instance.OnPauseGame -= (condition) => _isCountingDown = !condition;
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
                }
            }
        }

        private void StartTimer()
        {
            _timeRemaining = _duration;
            _isCountingDown = true;
            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(_timeRemaining / 60);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
