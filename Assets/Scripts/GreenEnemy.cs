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
    public float maxShield = 5000;
    public float currentShield = 5000;

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
        // Rotate towards the player
        //Vector3 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 2 * Time.deltaTime);
        if (!movingRight)
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
        // Calculate the angle between bullets in the cone
        float angleStep = 360f / 8f;

        // Spawn bullets in a cone shape
        for (int i = 0; i < 8; i++)
        {
            // Calculate the rotation for each bullet in the cone
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angleStep * i);

            // Instantiate a projectile and set its damage and rotation
            GameObject projectile = Instantiate(laserPrefab, firePoints[5].position, bulletRotation);
            Shotgun projectileController = projectile.GetComponent<Shotgun>();
           // projectileController.damage = currentDamage;

            // Set the projectile's velocity based on its forward direction
            Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
            projectileRigidbody.velocity = projectile.transform.up * projectileController.speed;

            // Destroy the projectile after a certain time
            Destroy(projectile, 5f);
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
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 1, 1f);
            // switch to destroyed state if shield is destroyed
            if (currentShield <= 0)
            {
                currentState = EnemyState.Destroyed;
                gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }
        else
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            // take damage and switch to shielded state if shield is still active
            currentHealth -= damage;
            if (currentHealth <= maxHealth / 2)
            {
                currentState = EnemyState.Shielded;
            }

            // switch to destroyed state if health is low
            if (currentHealth <= 0)
            {
                PlayerController.Instance.GivePlayerHP(10);
                SCORE.Instance.AddToScore(6);
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
