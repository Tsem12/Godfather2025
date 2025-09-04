using UnityEngine;
using System.Collections;

public class SunDamage : MonoBehaviour

{
    private SumoHealth[] players;
    private bool runningCoroutine = false;

    void Start()
    {
        if (players == null)
        {
            players = (SumoHealth[])FindObjectsByType(typeof(SumoHealth), FindObjectsSortMode.InstanceID);
        }
    }

    void Update()
    {
        ReduceSunscreen();
    }

    private void ReduceSunscreen()
    {
        if (!runningCoroutine)
        {
            foreach (SumoHealth i in players)
            {
                if (i.CurrentSunscreen < 70)
                {
                    runningCoroutine = true;
                    i.CurrentSunscreen -= 1;

                    StartCoroutine(WaitDamageInstance(4));
                    runningCoroutine = false;

                }
                else if (i.CurrentSunscreen < 40)
                {
                    runningCoroutine = true;
                    i.CurrentSunscreen -= 1;

                    StartCoroutine(WaitDamageInstance(3));
                    runningCoroutine = false;

                }
                else
                {
                    runningCoroutine = true;
                    i.CurrentSunscreen -= 1;

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