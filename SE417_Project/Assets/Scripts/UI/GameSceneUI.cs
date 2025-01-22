using Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameSceneUI : MonoBehaviour
    {
        [SerializeField] private GameObject restartButton;
        [SerializeField] private GameObject mainMenuButton;

        private void OnEnable()
        {
            restartButton.SetActive(false);
            CoreGameSignals.Instance.OnPlayerDie += OnPlayerDie;
        }
        
        private void OnDisable()
        {
            CoreGameSignals.Instance.OnPlayerDie -= OnPlayerDie;
        }

        private void OnPlayerDie()
        {
            restartButton.SetActive(true);
            mainMenuButton.SetActive(true);
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
