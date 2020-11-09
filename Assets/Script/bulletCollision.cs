using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCollision : MonoBehaviour
{

    private void Start()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
            if (gameObject.name.Substring(0,6) == "Bullet")
            {
                GameObject GM = GameObject.Find("GameManager");
                gameManager GMScript = GM.GetComponent<gameManager>();
                GMScript.playerScore += 1;
                Debug.Log(GMScript.playerScore);
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.name =="Earth")
        {
            Destroy(gameObject);
        }

    }

    void FixedUpdate()
    {

        if(Vector3.Distance(new Vector3(0.0f, 0.0f, -1.0f), transform.position) > 20)
        {
            Destroy(gameObject);
        }

    }
}
