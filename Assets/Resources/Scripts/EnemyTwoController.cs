using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoController : MonoBehaviour
{
    Transform target;

    float speed = 0.01f;

    bool followActive = true;

    public int enemyDirection;
    Vector3 velocity;

    Vector3 enemyPos;
    Vector3 prevPos;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyDirection = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (followActive == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (followActive == false)
        {
            prevPos = enemyPos;
            enemyPos += velocity;
            transform.position = enemyPos;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (followActive == true)
        {
            // Stop moving if tile encountered
            if (collision.gameObject.tag == "Tile")
            {
                followActive = false;
                EnemyDirectionChange();
                followActive = true;

                Debug.Log("Collision");

            }
        }

    }

    void EnemyDirectionChange()
    {
        if (followActive == false)
        {
            enemyDirection = Random.Range(1, 4);
            if (enemyDirection == 1)
            {
                velocity.y = 0.0f;
                velocity.x = speed;
            }

            else if (enemyDirection == 2)
            {
                velocity.y = 0.0f;
                velocity.x = -speed;
            }

            else if (enemyDirection == 3)
            {
                velocity.x = 0.0f;
                velocity.y = speed;
            }

            else if (enemyDirection == 4)
            {
                velocity.x = 0.0f;
                velocity.y = -speed;
            }
        }
    }
}


