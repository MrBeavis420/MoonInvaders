using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bulletCollision : MonoBehaviour
{

    private void Start()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject UIScore = GameObject.Find("/Canvas/uiScore");
        GameObject UIEarthHealth = GameObject.Find("/Canvas/EarthHealth");
        GameObject GM = GameObject.Find("GameManager");
        gameManager GMScript = GM.GetComponent<gameManager>();

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);

            if (gameObject.name.Substring(0,6) == "Bullet")
            {
                GMScript.playerScore += (1 * GMScript.GMLevel);

                Text pScore = UIScore.GetComponent<Text>();
                pScore.text = "Score: " + GMScript.playerScore;

            }
        }

        if (collision.gameObject.CompareTag("Bullet") && gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);

            GMScript.playerScore += (2 * GMScript.GMLevel);

            Text pScore = UIScore.GetComponent<Text>();
            pScore.text = "Score: " + GMScript.playerScore;
        }

            if (collision.gameObject.CompareTag("Player"))
        { 
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.name =="Earth")
        {
            Destroy(gameObject);
            GMScript.earthHealth -= 1;
            UIEarthHealth.GetComponent<Slider>().value = GMScript.earthHealth;
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
