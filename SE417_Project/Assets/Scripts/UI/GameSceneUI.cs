using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameSceneUI : MonoBehaviour
    {
        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(1);
        }
    }
}
