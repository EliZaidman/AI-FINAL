using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGun : MonoBehaviour
{
    public int damage = 10;
    public float speed = 10f;
    public float lifetime = 5f;

    private Transform player;
    private Vector2 direction;

    private void Start()
    {
        player = PlayerController.Instance.transform;
        Destroy(gameObject, lifetime);
        CalculateDirection();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // Move the projectile towards the player
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void CalculateDirection()
    {
        // Calculate the direction towards the player
        if (player != null)
        {
            direction = (player.position - transform.position).normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile collides with the player
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
