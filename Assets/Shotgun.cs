using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 0.2f;
    public int damage = 10;

    public GameObject targetPosition;
    private Transform player;
    private Vector2 direction;
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.y * 3);
        Destroy(gameObject, lifetime);
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
    public void Fire(Vector3 target)
    {
        targetPosition.transform.position = target; // Set the target position to move towards
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController enemy = collision.gameObject.GetComponent<PlayerController>();
            enemy.TakeDamage(damage);
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
