using UnityEngine;
using System.Collections;

public class VendingMachine : MonoBehaviour
{

    private SumoMoney moneyRef;
    private bool canInteract;
    [SerializeField] private float timeToWait = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canInteract)
        {
            moneyRef = collision.gameObject.GetComponent<SumoMoney>();

            moneyRef.PlayerCurrentMoney -= 30;

            canInteract = false;

            StartCoroutine(WaitRechargeInstance(timeToWait));

            canInteract = true;
        }
    }

    IEnumerator WaitRechargeInstance(float timeToWait)
    {
        Debug.Log("Waiting for " + timeToWait + " seconds.");

        yield return new WaitForSeconds(timeToWait);
    }
}
