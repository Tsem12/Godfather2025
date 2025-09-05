using System;
using DG.Tweening;
using UnityEngine;

public class RotatingPlateforme : MonoBehaviour
{
    [SerializeField] private float _rotatingSpeed;
    [SerializeField] private Ease _easeMode;

    private void Start()
    {
        transform.DORotate(new Vector3(0, 0, 360f), _rotatingSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetEase(_easeMode);
    }
}
