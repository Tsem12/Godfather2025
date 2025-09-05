using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EffetLauncher : MonoBehaviour
{
    public static EffetLauncher Instance;
    [SerializeField] private MMFeedbacks _bumpFeedBack;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void TriggerBumpFeedback()
    {
        _bumpFeedBack.PlayFeedbacks();
    }
}
