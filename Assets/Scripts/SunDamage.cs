using UnityEngine;
using System.Collections;

public class SunDamage : MonoBehaviour
{
    
    private bool runningCoroutine = false;

    void Update()
    {
        ReduceSunscreen();
    }

    private void ReduceSunscreen()
    {
        if (!runningCoroutine)
        {
            foreach (Player i in PlayerManager.Instance.PlayersList)
            {
                if (i.Health.CurrentSunscreen < 70)
                {
                    runningCoroutine = true;
                    i.Health.CurrentSunscreen -= 1;

                    StartCoroutine(WaitDamageInstance(4));
                    runningCoroutine = false;

                }
                else if (i.Health.CurrentSunscreen < 40)
                {
                    runningCoroutine = true;
                    i.Health.CurrentSunscreen -= 1;

                    StartCoroutine(WaitDamageInstance(3));
                    runningCoroutine = false;

                }
                else
                {
                    runningCoroutine = true;
                    i.Health.CurrentSunscreen -= 1;

                    StartCoroutine(WaitDamageInstance(2));
                    runningCoroutine = false;
                }
            }
        }
    }

    IEnumerator WaitDamageInstance(float timeToWait)
    {
        Debug.Log("Waiting for " + timeToWait + " seconds.");

        yield return new WaitForSeconds(timeToWait);
    }
}