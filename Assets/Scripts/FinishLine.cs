using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tortoise playerScript = collision.GetComponent<Tortoise>();
        if (collision.CompareTag("Player") && playerScript!=null)
        {
            playerScript.rb.velocity = playerScript.rb.velocity * 0.4f;
            playerScript.SetSpeed(0f);
            GameManager.Instance.EndGame();
        }
    }
}
