using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Vector3 velocity;
    public int enemyDirection;
    public float speed;
    Vector3 enemyPos;
    Vector3 prevPos;

    public Animator animator;
    string CURRENT_STATE;

    // Start is called before the first frame update
    void Start()
    {
        enemyDirection = Random.Range(1, 4);
        velocity = new Vector2(0.0f, 0.0f);
        enemyPos = transform.position;
    }

    void FixedUpdate()
    {
        prevPos = enemyPos;
        enemyPos += velocity;
        transform.position = enemyPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.s_gameState != GameState.Paused)
        {
            getDirection();
            EnemyMovement();

            switch (CURRENT_STATE)
            {
                case "walkLeft":
                    animator.Play("enemyLeft");
                    break;
                case "walkRight":
                    animator.Play("enemyRight");
                    break;
                case "walkUp":
                    animator.Play("enemyUp");
                    break;
                case "walkDown":
                    animator.Play("enemyDown");
                    break;
            }
        }
    }

    void EnemyMovement()
    {
        if (enemyDirection == 1)
        {
            velocity.y = 0.0f;
            velocity.x = speed;
            CURRENT_STATE = "walkRight";
        }

        else if (enemyDirection == 2)
        {
            velocity.y = 0.0f;
            velocity.x = -speed;
            CURRENT_STATE = "walkLeft";
        }

        else if (enemyDirection == 3)
        {
            velocity.x = 0.0f;
            velocity.y = speed;
            CURRENT_STATE = "walkUp";
        }

        else if (enemyDirection == 4)
        {
            velocity.x = 0.0f;
            velocity.y = -speed;
            CURRENT_STATE = "walkDown";
        }
    }

    // This function is public to allow the map to access it
    public void ChangeDirection()
    {
        int newDir = Random.Range(1, 5);

        while(newDir == enemyDirection)
        {
            newDir = Random.Range(1, 5);
        }

        enemyDirection = newDir;
    }

    public int getDirection()
    {
        return enemyDirection;
    }

    public void StopMovement()
    {
        //Debug.Log("Enemy Stop Called");
        enemyPos = prevPos;
        transform.position = enemyPos;
        ChangeDirection();
    }

    public void KillEnemy()
    {
        if (FindObjectOfType<SoundManager>() != null)
        {
            FindObjectOfType<SoundManager>().Play("BatDeath");
        }

        FindObjectOfType<Map>().RemoveEntityFromAllTiles(gameObject);
        Destroy(gameObject);
    }
}
