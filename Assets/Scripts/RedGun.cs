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

    void OnCollisionEnter2D(Collision2D collision)
    {
        print("BLUE GUN HIT");
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController enemy = collision.gameObject.GetComponent<PlayerController>();
            enemy.TakeDamage(damage);
            print("HIT!");
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            print("DAFAK");
            Destroy(gameObject);
            SCORE.Instance.AddToScore(1);
        }
    }
}
