﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public GameObject target;
    public GameObject enemy;
    public int enemyBaseSpeed = 20;
    public int StartEnemies = 10;
    public float moveDelta = 0.2F;
    public GameObject EnemyBullet;
    public float bulletSpeedEnemy = 10.0f;
    public float chanceEnemyFire;
    private float myTime = 0.0F;
    private float nextMove = 0.5F;
    private float moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        LoadEnemies();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myTime = myTime + Time.deltaTime;

        if (myTime > nextMove)
        {
            nextMove = myTime + moveDelta;
            MoveEnemies();
            nextMove = nextMove - myTime;
            myTime = 0.0F;
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
