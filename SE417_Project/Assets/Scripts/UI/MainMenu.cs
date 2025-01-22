using System;
using Signals;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsMenu;
        
        [Header("Mixers")]
        [SerializeField] private AudioMixer masterMixer; 
        
        [Header("Sliders")]
        [SerializeField] private Slider musicSlider;  
        [SerializeField] private Slider effectsSlider;  
        
        [Header("Toggles")]
        [SerializeField] private Toggle easyToggle;
        [SerializeField] private Toggle medToggle;
        [SerializeField] private Toggle hardToggle;

        private float _gameTime;
        private bool _isGamePaused;
        
        private void OnEnable()
        {
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
            SetEffectsVolume(effectsSlider.value);
            SetMusicVolume(musicSlider.value);
        }

        private void Start()
        {
            _gameTime = 180f;
            masterMixer.SetFloat("MusicVolume", 0);
            masterMixer.SetFloat("Effects", 0);
        }

        public void SetDifficulty()
        {
            if (easyToggle.isOn)
            {
                _gameTime = 180f;
                PlayerPrefs.SetFloat("GameTime", _gameTime);
            }
            else if (medToggle.isOn)
            {
                _gameTime = 150f;
                PlayerPrefs.SetFloat("GameTime", _gameTime);
            }
            else if (hardToggle.isOn)
            {
                _gameTime = 120f;
                PlayerPrefs.SetFloat("GameTime", _gameTime);
            }
        }
        
        public void SetMusicVolume(float sliderValue)
        {
            masterMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
            if (sliderValue <= 0)
            {
                masterMixer.SetFloat("MusicVolume", -80);
            }
        }
        
        public void SetEffectsVolume(float sliderValue)
        {
            masterMixer.SetFloat("Effects", Mathf.Log10(sliderValue) * 20);
            if (sliderValue <= 0)
            {
                masterMixer.SetFloat("Effects", -80);
            }
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
