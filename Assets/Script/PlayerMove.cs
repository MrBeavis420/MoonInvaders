using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float speed = 10;
    public Rigidbody2D target;
    public GameObject bullet;
    public float fireDelta = 0.2F;
    public float bulletSpeed = 10.0f;
    private float myTime = 0.0F;
    private float nextFire = 0.5F;

    // Start is called before the first frame update
    void Start()
    {
        //load pilot settings

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject newBullet;
        GameObject GM = GameObject.Find("GameManager");
        gameManager GMScript = GM.GetComponent<gameManager>();

        if (GMScript.gms == gameState.running)
        {


            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.RotateAround(target.transform.position, Vector3.forward, speed * Time.deltaTime);

            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.RotateAround(target.transform.position, Vector3.forward, (-1*speed) * Time.deltaTime);
            }

            myTime = myTime + Time.deltaTime;

            if (Input.GetButton("Fire1") && myTime > nextFire)
            {
                nextFire = myTime + fireDelta;
                Vector3 pos = new Vector3(transform.position.x + (transform.right.x * transform.localScale.x / 6), transform.position.y + (transform.right.y * transform.localScale.y / 6), -1);
                newBullet = Instantiate(bullet, pos, Quaternion.identity);
                newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.position.x * bulletSpeed, transform.position.y * bulletSpeed);
                nextFire = nextFire - myTime;
                myTime = 0.0F;
            }
        }
        
    }
}