using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Objects;
using Signals;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject stairsCamera;
    [SerializeField] private Stair stair;

    private void OnEnable()
    {
        PlayerSignals.Instance.OnCompleteLevel += OnCompleteLevel;
    }
    
    private void OnDisable()
    {
        PlayerSignals.Instance.OnCompleteLevel -= OnCompleteLevel;
        
    }

    private void OnCompleteLevel(int levelIndex)
    {
        if (levelIndex == 1)
        {
            ChangeCameraAsync().Forget();
        }
    }

    private async UniTaskVoid ChangeCamera()
    {
        fadeImage.DOColor(Color.black, 1f);
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        fadeImage.DOColor(Color.clear, 1f);
        playerCamera.SetActive(false);
        stairsCamera.SetActive(true);
        stair.Build().Forget();
        await UniTask.Delay(TimeSpan.FromSeconds(2.5f));
        fadeImage.DOColor(Color.black, 1f);
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        fadeImage.DOColor(Color.clear, 1f);
        playerCamera.SetActive(true);
        stairsCamera.SetActive(false);
    }
    
    private async UniTask ChangeCameraAsync()
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
