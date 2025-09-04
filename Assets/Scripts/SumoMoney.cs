using System.Collections;
using UnityEngine;

public class SumoMoney : MonoBehaviour
{
    [Header("Money Values")]
    [SerializeField] private int moneyToAdd;
    [SerializeField] private int playerCurrentMoney;
    public int PlayerCurrentMoney
    {
        get { return playerCurrentMoney; }
        set
        {
            playerCurrentMoney = value;
        }
    }


    private float timeToWait = 5;

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
