using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer; 
        [SerializeField] private Slider volumeSlider;  

        private void Start()
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        public void SetVolume(float sliderValue)
        {
            audioMixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
        }
        
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
