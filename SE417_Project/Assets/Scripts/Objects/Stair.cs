using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Objects
{
    public class Stair : MonoBehaviour
    {
        [SerializeField] private TMP_Text collectedPieceCountText;
        [SerializeField] private Image collectedPieceImage; 
        [SerializeField] private List<GameObject> pieces;
        [SerializeField] private AudioClip buildSound;
        private int _currentPieceCount;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            CoreGameSignals.Instance.OnCollectObject += OnCollectObject;
        }
        private void Start()
        {
            collectedPieceCountText.text = $"0/{pieces.Count}";
            foreach (var piece in pieces.Where(piece => piece.activeSelf))
            {
                piece.SetActive(false);
            }
        }
        private void OnDisable()
        {
            CoreGameSignals.Instance.OnCollectObject -= OnCollectObject;            
        }

        private void OnCollectObject()
        {
            _currentPieceCount++;
            collectedPieceImage.transform.DOScale(collectedPieceImage.transform.localScale * 1.1f,0.2f).SetLoops(2, LoopType.Yoyo);
            collectedPieceCountText.transform.DOScale(transform.localScale * 1.1f,0.2f).SetLoops(2, LoopType.Yoyo);
            collectedPieceCountText.text = $"{_currentPieceCount}/{pieces.Count}";
            if (_currentPieceCount == pieces.Count)
            {
                //1 mean level 1 is completed
                CoreGameSignals.Instance.OnCompleteObjective?.Invoke(1);
                CoreGameSignals.Instance.OnCompleteLevel?.Invoke(1);
            }   
        }
        
        public async UniTaskVoid Build()
        {
            foreach (var piece in pieces.Where(piece => !piece.activeSelf))
            {
                piece.SetActive(true);
                piece.transform.DOScale(Vector3.zero,0.1f).From().SetEase(Ease.OutBack);
                _audioSource.PlayOneShot(buildSound);
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            }
        }
    }
}
