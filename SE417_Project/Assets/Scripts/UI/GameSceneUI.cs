using Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameSceneUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject winMenu;
        [SerializeField] private GameObject gameOverMenu;

        private bool _isGamePaused;
        private bool _isGameCompleted;
        
        private void OnEnable()
        {
            pauseMenu.SetActive(false);
            winMenu.SetActive(false);
            gameOverMenu.SetActive(false);
            CoreGameSignals.Instance.OnPlayerDie += OnPlayerDie;
            CoreGameSignals.Instance.OnCompleteLevel += OnCompleteLevel;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_isGamePaused && !_isGameCompleted)
                {
                    _isGamePaused = true;
                    pauseMenu.SetActive(true);
                    CoreGameSignals.Instance.OnPauseGame?.Invoke(true);
                }
                else
                {
                    _isGamePaused = false;
                    pauseMenu.SetActive(false);
                    CoreGameSignals.Instance.OnPauseGame?.Invoke(false);
                }
            }
        }
        
        private void OnDisable()
        {
            CoreGameSignals.Instance.OnPlayerDie -= OnPlayerDie;
            CoreGameSignals.Instance.OnCompleteLevel -= OnCompleteLevel;
        }

        private void OnCompleteLevel()
        {
            winMenu.SetActive(true);
            _isGameCompleted= true;
        }

        private void OnPlayerDie()
        {
            gameOverMenu.SetActive(true);
            _isGameCompleted = true;
        }

        public void ReturnToMainMenu()
        {
            _isGameCompleted = false;
            SceneManager.LoadScene("MainMenu");
        }
        
        public void RestartGame()
        {
            _isGameCompleted = false;
            SceneManager.LoadScene("SampleScene");
        }
    }
}
