using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemy : MonoBehaviour
{
    // movement variables
    public float moveSpeed = 5f;
    private bool movingRight = false;

    // shooting variables
    public GameObject laserPrefab;
    public Transform[] firePoints;
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    // shield variables
    public float maxShield = 50;
    private float currentShield;

    // health variables
    public int maxHealth = 100;
    private float currentHealth;

    // state machine variables
    public enum EnemyState { Moving, Shooting, Shielded, Destroyed };
    public EnemyState currentState = EnemyState.Moving;

    private void Start()
    {
        currentShield = maxShield;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Moving:
                Move();
                break;
            case EnemyState.Shooting:
                Shoot();
                break;
            case EnemyState.Shielded:
                break;
            case EnemyState.Destroyed:
                Destroy(gameObject);
                break;
        }
    }

    private void Move()
    {
        // move horizontally from right to left
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            transform.localScale = new Vector2(2, 2);
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            transform.localScale = new Vector2(-2, 2);
        }

        // check if at edge of screen, then change direction
        if (transform.position.x >= 8f)
        {
            movingRight = false;
        }
        else if (transform.position.x <= -8f)
        {
            movingRight = true;
        }

        // switch to shooting state after a few seconds
        if (Time.time > nextFireTime)
        {
            currentState = EnemyState.Shooting;
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        // shoot a spread of three laser beams in a fan shape
        for (int i = 0; i < firePoints.Length; i++)
        {
            Instantiate(laserPrefab, firePoints[i].position, firePoints[i].rotation);
        }

        // switch back to moving state
        currentState = EnemyState.Moving;
    }

    public void TakeDamage(float damage)
    {
        // check if shield is still active
        if (currentState == EnemyState.Shielded)
        {
            currentShield -= damage;

            // switch to destroyed state if shield is destroyed
            if (currentShield <= 0)
            {
                currentState = EnemyState.Destroyed;
            }
        }
        else
        {
            // take damage and switch to shielded state if shield is still active
            currentHealth -= damage;
            if (currentHealth <= maxHealth / 2)
            {
                currentState = EnemyState.Shielded;
            }

            // switch to destroyed state if health is low
            if (currentHealth <= 0)
            {
                currentState = EnemyState.Destroyed;
            }
        }
    }

    private void LateUpdate()
    {
        // get the camera object
        var camera = Camera.main;

        // calculate the distance between the enemy and the camera
        var distance = (transform.position - camera.transform.position).sqrMagnitude;

        // if the enemy is too far away, move it closer
        if (distance > 600)
        {
            var direction = (camera.transform.position - transform.position).normalized;
            transform.position += direction * Time.deltaTime * 50;

            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
}
