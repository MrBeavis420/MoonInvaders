using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.collider.gameObject);
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
