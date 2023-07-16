using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    // Public property to access the instance
    public static PlayerController Instance
    {
        get
        {
            // Check if the instance is null
            if (instance == null)
            {
                // Find the singleton instance in the scene
                instance = FindObjectOfType<PlayerController>();

                // If no instance found, create a new GameObject and attach the singleton component to it
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(PlayerController).Name);
                    instance = singletonObject.AddComponent<PlayerController>();
                }

                // Make sure the singleton instance persists between scene changes
                //DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }
    private void Awake()
    {
        // Enforce the singleton pattern by destroying duplicate instances
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }
    // movement variables
    public float moveSpeed = 5f;
    public GameObject looseScreen;

    // shooting variables
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public float bulletSpeed = 10f;

    // health variables
    public float maxHealth = 100f;
    private float currentHealth;
    public float GetCurrentHealth() { return currentHealth; }
    public float GetMaxHealth() { return maxHealth; }
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {  // movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);
        transform.position += movement * moveSpeed * Time.deltaTime;

        // rotation
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));



        // shooting
        if (Input.GetMouseButton(0) && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            // instantiate bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Vector3 bulletDirection = (mousePos - bullet.transform.position).normalized;
            float bulletAngle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, bulletAngle));
            bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * bulletSpeed, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // do something when the player dies, such as restart the level or show a game over screen
        Debug.Log("Player has died.");
        gameObject.SetActive(false);
        looseScreen.SetActive(true);
    }

    public void GivePlayerHP(int hp) { currentHealth += hp; }
}
