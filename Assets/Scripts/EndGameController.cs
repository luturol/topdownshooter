using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            bool hasCoin = other.gameObject.GetComponent<PlayerController>().GetHasCoin();
            Debug.Log("Player has coin: " + hasCoin);
            if (hasCoin)
            {
                var gameController = GameController.Instance;
                gameController.AddWin();

                gameController.SendToMenu();
            }
        }
    }
}
