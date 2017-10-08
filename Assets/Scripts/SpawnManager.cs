using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnGameObjbect{
    public GameObject type;
    public List <GameObject> parts;
    public string name;
    public float time;//When do they starts to spawn
    public float spawnDelay;
    public float waveTimer;
    public float timeStamp;
    public string iEnumeratorName;
}


public class SpawnManager : MonoBehaviour {

    public List<SpawnGameObjbect> enemyShipSpawnType;
    public List<SpawnGameObjbect> powerUpsList;
    public List<SpawnGameObjbect> bossList;

    public Transform min;
    public Transform max;
    public GameObject mediumMeteor;
    public GameObject largeMeteor;
    public int totalMeteors;
    public float waveTimer;
    public float spawnDelay;
    private float meteorsTimeStamp;

    public Transform powerUpmin;
    public Transform powerUpmax;
    public float powerUpWaveTimer;
    private float allPowerUpTimeStamp;

    public float bossWaveTimer;
    private float bossTimeStamp;
    public int bossIndex;
    public GameObject bossHealthBar;

   
    void Start()
    {
        bossIndex = 0;
        meteorsTimeStamp = Time.time;
        allPowerUpTimeStamp = Time.time;
        bossTimeStamp = Time.time;
    }

    void Update()
    {
        if (meteorsTimeStamp <= Time.time)
        {
            StartCoroutine("SpawnMeteors");

            meteorsTimeStamp = Time.time + waveTimer;
        }
        if (allPowerUpTimeStamp <= Time.time)
        {
            StartCoroutine("SpawnPowerUps");

            allPowerUpTimeStamp = Time.time + powerUpWaveTimer;
        }
        if (bossTimeStamp <= Time.time)
        {
            StartCoroutine("SpawnBosses");
 
        }
        foreach (SpawnGameObjbect spawn in enemyShipSpawnType)
        {
            if (spawn.timeStamp >= spawn.time && spawn.timeStamp < Time.time)
            {
                object[] parms = { spawn.spawnDelay, spawn.type };
                StartCoroutine(spawn.iEnumeratorName, parms);
                spawn.timeStamp = Time.time + spawn.waveTimer;
            }

        }
        
    }



    /* Handles spawning the debris into map at startup */
    IEnumerator SpawnMeteors()
    {

        float meteorSpawnProb;  // Spawn probability of each meteor
        Vector3 position;       // Spawn position of each meteor   

        for (int i = 0; i < totalMeteors; ++i)
        {

            meteorSpawnProb = Random.Range(0.0f, 1.0f);
            position = new Vector3(Random.Range(min.position.x, max.position.x),
                                    Random.Range(min.position.y, max.position.y),
                                    0.0f);

            if (meteorSpawnProb < 0.25f)  // Spawn large meteor
            {
                GameObject meteor = Instantiate(largeMeteor, position, Quaternion.identity);
                meteor.GetComponent<Rigidbody2D>().velocity = new Vector2(3.0f * Random.Range(-1.0f, 1.0f),
                    GameController.instance.scrollSpeed);
    
            }
            else if (meteorSpawnProb < 0.60f) // Spawn medium meteor
            {
                GameObject meteor = Instantiate(mediumMeteor, position, Quaternion.identity);
                meteor.GetComponent<Rigidbody2D>().velocity = new Vector2(3.0f * Random.Range(-1.0f, 1.0f),
                    GameController.instance.scrollSpeed);

            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnEnemyShip(object[] parms)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject ship = Instantiate((GameObject)parms[1], new Vector3(10f, 1.5f, -0.01f), Quaternion.identity);
            Destroy(ship, 5f);
            yield return new WaitForSeconds((float)parms[0]);
        }
    }

    IEnumerator SpawnPowerUps()
    {
        int sizeOfList = powerUpsList.Count;
        int randomInt = Random.Range(0, sizeOfList);//[,)
        Vector3 position = new Vector3(Random.Range(powerUpmin.position.x, powerUpmax.position.x),
        Random.Range(powerUpmin.position.y, powerUpmax.position.y), 0.0f);
        GameObject powerUp = Instantiate(powerUpsList[randomInt].type, position, Quaternion.Euler(powerUpsList[randomInt].type.transform.eulerAngles));
        powerUp.GetComponent<Rigidbody2D>().velocity = 10f * new Vector2(Mathf.Cos(Random.Range(0f, 2 * Mathf.PI)), Mathf.Sin(Random.Range(0f, 2 * Mathf.PI)));
        yield return new WaitForSeconds(powerUpWaveTimer);
    }


    IEnumerator SpawnBosses()
    {
        float health = 0f;

        if (bossList[bossIndex].type != null)
        {
            bossHealthBar.SetActive(true);
            GameObject boss = bossList[bossIndex].type;
            boss.SetActive(true);//using scene object;

            if (health <= 0.1)
            {
                health = 0.1f;//GetCurrentHealth() returns 0 at first frame. We have to make a fake health for the while check
            }
            else
            {
                health = bossList[bossIndex].type.GetComponent<Destructibles>().currentHealth;
            }
        }
        else
        {
            health = 0;
        }

        while (health > 0f)
        {
            yield return null;//wait till the boss is dead
        }
        bossHealthBar.SetActive(false);
        bossIndex = TheBoss.IncreaseBossIndex(bossIndex,bossList.Count);
        
        StartCoroutine("WaitSeconds", bossList[bossIndex].waveTimer);
        bossTimeStamp = Time.time + bossList[bossIndex].waveTimer;
    }

    
    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }



}
