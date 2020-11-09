using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum gameState
{
    load,
    running,
    gameover,
    newlevel
};

public class gameManager : MonoBehaviour
{
    public GameObject target;
    public GameObject enemy;
    public GameObject player;
    public int enemyBaseSpeed = 20;
    public int StartEnemies = 10;
    public float moveDelta = 0.2F;
    public GameObject EnemyBullet;
    public float bulletSpeedEnemy = 10.0f;
    public float chanceEnemyFire;
    public float playerScore;
    public int earthHealth;
    public int GMLevel;
    private float myTime = 0.0F;
    private float nextMove = 0.5F;
    float startTime = 40.0f;
    float timeLeft = 0.0f;
    private float moveDirection;
    public int playerLives;
    public gameState gms;

    // Start is called before the first frame update
    void Start()
    {
        GameObject UIScore = GameObject.Find("/Canvas/uiScore");
        GameObject UIEarthHealth = GameObject.Find("/Canvas/EarthHealth");
        GameObject UILives = GameObject.Find("/Canvas/uiLives");
        playerScore = 0.0f;
        earthHealth = 100;
        GMLevel = 1;
        playerLives = 2;
        Text uLives = UILives.GetComponent<Text>();
        uLives.text = "Lives: " + playerLives;
        Instantiate(player, new Vector3(1, 0, -1), Quaternion.identity);
        Text pScore = UIScore.GetComponent<Text>();
        pScore.text = "Score: " + playerScore;
        UIEarthHealth.GetComponent<Slider>().value = earthHealth;
        this.gms = gameState.load;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject UITimer = GameObject.Find("/Canvas/uiTimer");
        GameObject UILives = GameObject.Find("/Canvas/uiLives");

        if (this.gms == gameState.load)
        {
            timeLeft = startTime;
            LoadEnemies();
            this.gms = gameState.running;
        }


        if (this.gms == gameState.running)
        {
            myTime = myTime + Time.deltaTime;

            if (myTime > nextMove)
            {
                nextMove = myTime + moveDelta;
                MoveEnemies();
                nextMove = nextMove - myTime;
                myTime = 0.0F;
            }

            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                GMLevel += 1;
                this.gms = gameState.load;
            }

            if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
            {
                if(playerLives > 0)
                {
                    playerLives -= 1;
                    Text uLives = UILives.GetComponent<Text>();
                    uLives.text = "Lives: " + playerLives;
                    Instantiate(player, new Vector3(1, 0, -1), Quaternion.identity);
                }
                else
                {
                    this.gms = gameState.gameover;
                }
            }

            if(earthHealth == 0)
            {
                this.gms = gameState.gameover;
            }

            timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0, startTime);
            Text uTimer = UITimer.GetComponent<Text>();
            uTimer.text = "Timer: " + Mathf.Round(timeLeft);
            if (timeLeft == 0)
            {
                this.gms = gameState.gameover;
            }

        }

        if (this.gms == gameState.gameover)
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (this.gms == gameState.newlevel)
        {
            playerScore += ((Mathf.Round(startTime-timeLeft)) * GMLevel);
        }
    }

    void LoadEnemies()
    {
        while(GameObject.FindGameObjectsWithTag("Enemy").Length <= StartEnemies)
        {
            Vector3 position = new Vector3(Mathf.Round(Random.Range(-5.0f, 5.0f)), Mathf.Round(Random.Range(-5.0f, 5.0f)), -1);
            if (Vector3.Distance(new Vector3(0.0f, 0.0f, -1.0f), position) > 2)
            {
                Instantiate(enemy, position, Quaternion.identity);
            }
        }
    }

    void MoveEnemies()
    {
        GameObject newBullet;
        var objects = GameObject.FindGameObjectsWithTag("Enemy");
        moveDirection = 0.0f;
        while (moveDirection == 0.0f) {
            moveDirection = Mathf.Round(Random.Range(-1.0f, 1.0f));
        }
        foreach (var obj in objects)
        {
            if (obj.tag == "Enemy")
            {

                obj.transform.RotateAround(target.transform.position, Vector3.forward, (enemyBaseSpeed * moveDirection) * Time.deltaTime);

                if (Mathf.Round(Random.Range(0.0f, 100.0f)) <= chanceEnemyFire)
                {
                    Vector3 pos = new Vector3(obj.transform.position.x + (obj.transform.right.x * -obj.transform.localScale.x / 2), obj.transform.position.y + (obj.transform.right.y * -obj.transform.localScale.y / 2), -1);
                    newBullet = Instantiate(EnemyBullet, pos, Quaternion.identity);
                    Vector3 difference = new Vector3(0,0,0) - newBullet.transform.position;
                    float distance = difference.magnitude;
                    Vector2 direction = difference / distance;
                    direction.Normalize();
                    newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeedEnemy;
                }

            }
        }
    }

}
