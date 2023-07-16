using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGun : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 1.5f;
    public int damage = 10;
    Transform lastPos;
    public GameObject targetPosition;

    void Start()
    {
        if (targetPosition == null)
        {
            targetPosition = GameObject.Find("PlayerA");
            lastPos = targetPosition.transform;
        }
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.y * 3);
        //Destroy(gameObject, lifetime);
    }

    void Update()
    {
        //transform.position += transform.forward * speed * Time.deltaTime;
        if(targetPosition == null)
        transform.position = Vector3.MoveTowards(transform.position, lastPos.position, speed * Time.deltaTime);

    }

    public void Fire(Vector3 target)
    {
        targetPosition.transform.position = target; // Set the target position to move towards
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
        }
    }
}
