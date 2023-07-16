using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemy : MonoBehaviour
{
    // movement variables
    public float moveSpeed = 5f;

    // bombing variables
    public GameObject bombPrefab;
    public Transform bombDropPoint;
    public float bombRate = 2f;
    private float nextBombTime = 0f;

    // cloaking variables
    public float cloakDuration = 3f;
    public float cloakStartTime = 0f;
    bool hasBeenClocked = false;
    // health variables
    public int maxHealth = 100;
    private float currentHealth;

    // state machine variables
    public enum EnemyState { Moving, DroppingBombs, Cloaked, Destroyed };
    public EnemyState currentState = EnemyState.Moving;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Moving:
                Move();
                break;
            case EnemyState.DroppingBombs:
                DropBomb();
                break;
            case EnemyState.Cloaked:
                Cloak();
                break;
            case EnemyState.Destroyed:
                Destroy(gameObject);
                break;
        }
    }

    private void Move()
    {
        // move vertically from top to bottom
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

        // switch to dropping bombs state after a few seconds
        if (Time.time > nextBombTime)
        {
            currentState = EnemyState.DroppingBombs;
            nextBombTime = Time.time + bombRate;
        }
        if (currentHealth <=50 && !hasBeenClocked)
        {
            currentState = EnemyState.Cloaked;
        }
    }

    private void DropBomb()
    {
        // drop a bomb at the designated drop point
        Instantiate(bombPrefab, bombDropPoint.position, Quaternion.identity);

        // switch back to moving state
        currentState = EnemyState.Moving;
    }

    public void Cloak()
    {
        hasBeenClocked = true;
        // activate the cloaking device
        cloakStartTime += Time.deltaTime;
        currentState = EnemyState.Cloaked;

        // make the enemy temporarily invisible
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;

        // make the enemy temporarily invulnerable
        gameObject.GetComponent<Collider2D>().enabled = false;
        if (cloakStartTime >= cloakDuration)
        {
            cloakStartTime = 0;
            Uncloak();
        }
    }

    public void Uncloak()
    {
        // deactivate the cloaking device
        currentState = EnemyState.Moving;

        // make the enemy visible again
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;

        // make the enemy vulnerable again
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void TakeDamage(float damage)
    {
        // take damage only if not cloaked
        if (currentState != EnemyState.Cloaked)
        {
            currentHealth -= damage;

            // switch to destroyed state if health is low
            if (currentHealth <= 0)
            {
                currentState = EnemyState.Destroyed;
                PlayerController.Instance.GivePlayerHP(10);
            }
        }
    }
}
