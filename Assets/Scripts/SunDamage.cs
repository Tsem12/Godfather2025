using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class SunDamage : MonoBehaviour
{
    [SerializeField] private float _lifeLooseMultiplier;
    [SerializeField] private AnimationCurve _curveMultiplier;
    private bool runningCoroutine = false;

    private bool _hasGameStarted;

    private void Start()
    {
        PlayerManager.Instance.OnGameStart += RetreiveGameStart;
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.OnGameStart -= RetreiveGameStart;
    }

    private void RetreiveGameStart() => _hasGameStarted = true;

    void Update()
    {
        if(!_hasGameStarted)
            return;
        
        foreach (Player i in PlayerManager.Instance.PlayersList)
        {
            i.Health.CurrentSunscreen -=
                Time.deltaTime * _curveMultiplier.Evaluate(i.Health.CurrentSunscreen / i.Health.MaxSunscreen);

            i.Health.CurrentSunscreen = Mathf.Max(0, i.Health.CurrentSunscreen);
            
            if (i.Health.CurrentSunscreen <= 0)
            {
                i.Health.CurrentHealth -= Time.deltaTime * _lifeLooseMultiplier;
                if (i.Health.CurrentHealth <= 0)
                {
                    Debug.Log($"{i.gameObject} loose");
                }
            }
        }
    }
    
}