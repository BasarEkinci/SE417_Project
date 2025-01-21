using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Objects;
using Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject restartButton;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject stairsCamera;
    [SerializeField] private GameObject jessieCamera;
    [SerializeField] private Stair stair;

    private void OnEnable()
    {
        CoreGameSignals.Instance.OnCompleteObjective += OnCompleteObjective;
        CoreGameSignals.Instance.OnCompleteLevel += OnCompleteLevel;
        CoreGameSignals.Instance.OnPlayerDie += () => restartButton.SetActive(true);
    }

    private void Start()
    {
        jessieCamera.SetActive(false);
        stairsCamera.SetActive(false);
    }

    private void OnDisable()
    {
        CoreGameSignals.Instance.OnCompleteObjective -= OnCompleteObjective;
        CoreGameSignals.Instance.OnCompleteLevel -= OnCompleteLevel;
        CoreGameSignals.Instance.OnPlayerDie -= () => restartButton.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        CoreGameSignals.Instance.OnGameStart?.Invoke();
    }
    
    private void OnCompleteLevel()
    {
        LevelCompleteState().Forget();
        restartButton.SetActive(true);
    }

    private void OnCompleteObjective()
    {
        StairBuildState().Forget();
    }
    
    private async UniTask StairBuildState()
    { 
        await PerformFade(Color.black, 1f);
        ToggleCamera(playerCamera, stairsCamera);
        await PerformFade(Color.clear, 1f);
        stair.Build().Forget();
        await UniTask.Delay(TimeSpan.FromSeconds(2.5f));
        await PerformFade(Color.black, 1f);
        ToggleCamera(stairsCamera, playerCamera);
        await PerformFade(Color.clear, 1f);
    }
    
    private async UniTaskVoid LevelCompleteState()
    {
        await PerformFade(Color.black, 1f);
        ToggleCamera(playerCamera, jessieCamera);
        await PerformFade(Color.clear, 1f);
    }

    private async UniTask PerformFade(Color targetColor, float duration)
    {
        fadeImage.DOColor(targetColor, duration);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
    }

    private void ToggleCamera(GameObject deactivateCamera, GameObject activateCamera)
    {
        deactivateCamera.SetActive(false);
        activateCamera.SetActive(true);
    }
}
