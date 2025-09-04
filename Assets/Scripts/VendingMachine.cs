using UnityEngine;

public class VendingMachine : MonoBehaviour
{

    private SumoMoney moneyRef;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //animation call for sun cream

        moneyRef = collision.gameObject.GetComponent<SumoMoney>();

        moneyRef.PlayerCurrentMoney -= 30;
    }
}
