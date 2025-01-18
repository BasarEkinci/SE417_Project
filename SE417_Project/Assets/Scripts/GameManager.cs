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
        CoreGameSignals.Instance.OnCompleteObjective += OnCompleteObjective;
        CoreGameSignals.Instance.OnCompleteLevel += OnCompleteLevel;
    }
    
    private void OnDisable()
    {
        CoreGameSignals.Instance.OnCompleteObjective -= OnCompleteObjective;
        CoreGameSignals.Instance.OnCompleteLevel -= OnCompleteLevel;
        
    }

    private void OnCompleteLevel()
    {
        
    }

    private void OnCompleteObjective()
    {
        ChangeCameraAsync().Forget();
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
