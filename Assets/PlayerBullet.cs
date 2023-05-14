using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 1f;
    public float damage = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            RedEnemy enemy = collision.gameObject.GetComponent<RedEnemy>();
            enemy.TakeDamage(damage);
            print("HIT!");
            Destroy(gameObject);
        }
    }
}
