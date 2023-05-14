using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> enemieTypes = new List<GameObject>();

    public float timer;
    public float timeTillSpawn;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;


        if (timer >= timeTillSpawn)
        {
            timer = 0;
           // Instantiate(enemieTypes[0], spawnPoints[0], Quaternion.identity);
            GameObject bullet = Instantiate(enemieTypes[Random.Range(0, enemieTypes.Count)], spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform.position, Quaternion.identity);

            bullet.transform.position = new Vector3(bullet.transform.position.x, bullet.transform.position.y, 0);
        }
    }
}
