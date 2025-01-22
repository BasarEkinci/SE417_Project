using System;
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
            restartButton.SetActive(true);
            restartButton.SetActive(false);
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(1);
        }
        
        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}
