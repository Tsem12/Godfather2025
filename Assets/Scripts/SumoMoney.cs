using System.Collections;
using UnityEngine;

public class SumoMoney : MonoBehaviour
{
    [Header("Money Values")]
    [SerializeField] private int playerCurrentMoney;
    [SerializeField] private int moneyToAdd;

    private float timeToWait = 5,;

    private void Start()
    {
        while (true)
        {
            AddMoney(moneyToAdd);

            StartCoroutine(WaitPlease(timeToWait));
        }
    }

    private void AddMoney(int val)
    {
        playerCurrentMoney += val;
    }

    IEnumerator WaitPlease(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }
}
