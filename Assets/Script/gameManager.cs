using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public enum gameState
{
    load,
    running,
    gameover,
    newlevel
};

public class gameManager : MonoBehaviour
{
    //needed vars
    public GameObject target;
    public GameObject enemy;
    public GameObject pilot01;
    public GameObject pilot02;
    public GameObject pilot03;
    public GameObject EnemyBullet;
    public float playerScore;
    public int earthHealth;
    public int GMLevel;
    public string pilot;
    public string difficulty;
    private float moveDirection;
    public int playerLives;
    public gameState gms;
    public float playerRotationRadius = 1.5f; //Adjusts the radius of player from center of Earth in editor.
    public float enemySpawnDistanceX = 9f; //Adjusts the X distance in which enemies can spawn in the scene.
    public float enemySpawnDistanceY = 4f; //Adjusts the X distance in which enemies can spawn in the scene.

    //pilot vars

    //diff vars
    public int enemyBaseSpeed = 20;
    public int StartEnemies = 10;
    public float moveDelta = 0.2F;
    private float myTime = 0.0F;
    private float nextMove = 0.5F;
    public float bulletSpeedEnemy = 10.0f;
    public float chanceEnemyFire;
    float startTime = 40.0f;
    float timeLeft = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        GameObject UIScore = GameObject.Find("/Canvas/uiScore");
        GameObject UIEarthHealth = GameObject.Find("/Canvas/EarthHealth");
        GameObject UILives = GameObject.Find("/Canvas/uiLives");
        LoadPilot();
        playerScore = 0.0f;
        earthHealth = 100;
        GMLevel = 1;
        playerLives = 2;
        Text uLives = UILives.GetComponent<Text>();
        uLives.text = "Lives: " + playerLives;
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
                    LoadPilot();
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

    void LoadPilot()
    {
        if (PlayerPrefs.GetString("pilot") == "Char01")
        {
            Instantiate(pilot01, new Vector3(playerRotationRadius, 0, -1), Quaternion.identity);
        }
        if (PlayerPrefs.GetString("pilot") == "Char02")
        {
            Instantiate(pilot02, new Vector3(playerRotationRadius, 0, -1), Quaternion.identity);
        }
        if (PlayerPrefs.GetString("pilot") == "Char03")
        {
            Instantiate(pilot03, new Vector3(playerRotationRadius, 0, -1), Quaternion.identity);
        }
    }

    void LoadDIffSettings()
    {
        Debug.Log(PlayerPrefs.GetString("diff"));
    }

    void LoadEnemies()
    {
        while (GameObject.FindGameObjectsWithTag("Enemy").Length <= StartEnemies)
        {
            //Vector3 position = new Vector3(Mathf.Round(Random.Range(-5.0f, 5.0f)), Mathf.Round(Random.Range(-5.0f, 5.0f)), -1); //Commenting this out in favor of a modified line.
            Vector3 position = new Vector3(Mathf.Round(Random.Range(-enemySpawnDistanceX, enemySpawnDistanceX)), Mathf.Round(Random.Range(-enemySpawnDistanceY, enemySpawnDistanceY)), -1);
            if (Vector3.Distance(new Vector3(0.0f, 0.0f, -1.0f), position) > 2)
            {
                Instantiate(enemy, position, Quaternion.identity);

                var objects = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var obj in objects)
                {
                    if (obj.tag == "Enemy")
                    {
                        obj.transform.right = target.transform.position - obj.transform.position; //Correct rotation so enemies face towards earth
                    }
                }
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
                obj.transform.right = target.transform.position - obj.transform.position; //Correct rotation so enemies face towards earth
                

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
