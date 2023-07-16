using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemy : MonoBehaviour
{
    public Transform player;
    public Transform shootPos;
    public float movementSpeed = 5f;
    public float rotationSpeed = 180f;
    public GameObject projectilePrefab;
    public int baseDamage = 10;
    public float damageMultiplier = 2f;
    public float speedMultiplier = 2f;
    public float HP = 2f;

    private enum ShipState
    {
        Idle,
        Following,
        Shooting
    }

    [SerializeField] private ShipState currentState = ShipState.Idle;
    public float nextShootTime;
    public int currentDamage;
    public float currentSpeed;

    private void Start()
    {
        player = PlayerController.Instance.transform;
        currentDamage = baseDamage;
        currentSpeed = movementSpeed;
    }

    public float pewpewTime = 4;
    private void Update()
    {
        pewpewTime += Time.deltaTime;
        switch (currentState)
        {
            case ShipState.Idle:
                IdleState();
                break;
            case ShipState.Following:
                FollowingState();
                break;
            case ShipState.Shooting:
                ShootingState();
                break;
        }
    }

    private void IdleState()
    {
        // Transition to Following state when player is within a certain range
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > 10f)
        {
            currentState = ShipState.Following;
        }
    }

    private void FollowingState()
    {
        // Rotate towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // Move towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);

        // Transition to Shooting state when facing the player
        float angle = Quaternion.Angle(transform.rotation, lookRotation);
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > 16f)
        {
            currentState = ShipState.Shooting;
        }
        if (angle < 5f)
        {
            //currentState = ShipState.Shooting;
            //nextShootTime = Time.time;
        }
    }

    private void ShootingState()
    {
        // Shoot projectiles at the player
        if (nextShootTime <= pewpewTime)
        {
            pewpewTime = 0;
            StartCoroutine(PewPew());
        }

        // Transition back to Following state when the player is out of range
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > 15f)
        {
            currentState = ShipState.Following;
        }
    }

    IEnumerator PewPew()
    {
        Shoot();
        yield return new WaitForSeconds(0.2f);
        Shoot();
        yield return new WaitForSeconds(0.2f);
        Shoot();
        yield return new WaitForSeconds(0.2f);
        Shoot();
    }
    private void Shoot()
    {
        // Instantiate a projectile and set its damage
        GameObject projectile = Instantiate(projectilePrefab, shootPos.position, Quaternion.identity);
        RedGun projectileController = projectile.GetComponent<RedGun>();
        projectileController.damage = currentDamage;

        // Set the projectile's velocity based on its forward direction
        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        projectileRigidbody.velocity = transform.up * projectileController.speed;

        // Destroy the projectile after a certain time
        Destroy(projectile, 3f);
    }

    bool tookDMG = false;
    public void TakeDamage(float hp)
    {
        // Double the damage and speed
        if (!tookDMG)
        {
            tookDMG = true;
            currentDamage *= 2;
            currentSpeed *= 5;
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
        HP -= hp;

        if (HP <= 0)
        {
            Destroy(gameObject);
            SCORE.Instance.AddToScore(5);
            PlayerController.Instance.GivePlayerHP(10);
        }
    }
}
