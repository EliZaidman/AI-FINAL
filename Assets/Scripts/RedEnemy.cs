using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemy : MonoBehaviour
{
    public float speed = 5f;
    public float fireRate = 2f;
    public float bulletSpeed = 10f;
    public float health = 10f;

    public Transform playerTransform;
    public GameObject projectilePrefab;

    private Vector3 targetPosition;
    private float nextFireTime;

    private void Start()
    {
        targetPosition = GetRandomPosition();
        playerTransform = GameObject.Find("Player").GetComponentInChildren<Transform>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomPosition();
        }

        Vector3 direction = playerTransform.position - transform.position;
        direction.z = 0f;
        transform.up = direction.normalized;

        if (Time.time > nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();

        if (projectile != null)
        {
            health -= projectile.damage;

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Fire()
    {
        // Instantiate the projectile prefab
        GameObject projectile = Instantiate(projectilePrefab, transform.position, new Quaternion(0,0,0,0));

        // Calculate direction to player
        Vector3 direction = playerTransform.position - transform.position;
        direction.x = 0f; // Set the z-component to 0 to ensure movement only in the x-y plane

        // Set the rotation of the projectile to look towards the player only on the z-axis
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

        rotation.x = 0f;
        rotation.y = 0f;
        // Set the rotation of the projectile
        projectile.transform.rotation = rotation;

        // Get the projectile script component and fire the projectile
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.Fire(playerTransform.position);
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-5f, 5f);
        float y = Random.Range(-3f, 3f);
        return new Vector3(x, y, transform.position.z);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        print("HIT!!!!!!00");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
