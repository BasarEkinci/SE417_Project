using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsMenu;
        
        [Header("Mixers")]
        [SerializeField] private AudioMixer musicMixer; 
        [SerializeField] private AudioMixer effectMixer;
        
        [Header("Sliders")]
        [SerializeField] private Slider musicSlider;  
        [SerializeField] private Slider effectsSlider;  
        
        [Header("Toggles")]
        [SerializeField] private Toggle easyToggle;
        [SerializeField] private Toggle medToggle;
        [SerializeField] private Toggle hardToggle;

        private float _gameTime;

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
            musicMixer.SetFloat("MusicVolume", 0);
            effectMixer.SetFloat("Volume", 0);
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
            musicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
            if (sliderValue <= 0)
            {
                musicMixer.SetFloat("MusicVolume", -80);
            }
        }
        
        public void SetEffectsVolume(float sliderValue)
        {
            effectMixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
            if (sliderValue <= 0)
            {
                musicMixer.SetFloat("Volume", -80);
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
