using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 1f;
    public float damage = 0.5f;
    PlayerController _player;
    void Start()
    {
        Destroy(gameObject, lifetime);
        _player = PlayerController.Instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("HIT!@IJHU");

        if (collision.gameObject.CompareTag("Bullet"))
        {
            SCORE.Instance.AddToScore(1);
            collision.gameObject.SetActive(false);
            print("DAFAK");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            RedEnemy enemy = collision.gameObject.GetComponent<RedEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            GreenEnemy _enemy = collision.gameObject.GetComponent<GreenEnemy>();
            if (_enemy != null)
            {
                _enemy.TakeDamage(damage);
            }

            BlueEnemy __enemy = collision.gameObject.GetComponent<BlueEnemy>();
            if (__enemy != null)
            {
                __enemy.TakeDamage(damage);
            }
            print("HIT!");
            Destroy(gameObject);
        }
    }
}
