using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadSceneAsync("SampleScene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
