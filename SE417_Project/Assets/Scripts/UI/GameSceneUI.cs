using System;
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

        private void OnEnable()
        {
            pauseMenu.SetActive(false);
            winMenu.SetActive(false);
            gameOverMenu.SetActive(false);
            CoreGameSignals.Instance.OnPlayerDie += OnPlayerDie;
            CoreGameSignals.Instance.OnCompleteLevel += OnCompleteLevel;
        }
        
        private void OnDisable()
        {
            CoreGameSignals.Instance.OnPlayerDie -= OnPlayerDie;
            CoreGameSignals.Instance.OnCompleteLevel -= OnCompleteLevel;
        }

        private void OnCompleteLevel()
        {
            winMenu.SetActive(true);
        }

        private void OnPlayerDie()
        {
            gameOverMenu.SetActive(true);
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        public void RestartGame()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
