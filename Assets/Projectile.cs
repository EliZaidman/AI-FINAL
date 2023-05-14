using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 0.1f;
    public int damage = 10;

    private Vector3 targetPosition;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        //transform.position += transform.forward * speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

    }

    public void Fire(Vector3 target)
    {
        targetPosition = target; // Set the target position to move towards
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController enemy = collision.gameObject.GetComponent<PlayerController>();
            enemy.TakeDamage(damage);
            print("HIT!");
            Destroy(gameObject);
        }
    }
}
