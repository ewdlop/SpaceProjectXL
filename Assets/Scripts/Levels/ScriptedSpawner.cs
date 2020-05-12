using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

class ScriptedSpawn
{
    public ScriptedSpawn(string name, float wDelay, float sDelay, int wAmount, GameObject gObject)
    {
        spawnName = name;
        waveDelay = wDelay;
        spawnDelay = sDelay;
        waveAmount = wAmount;
        spawnPosition = Vector2.zero;
        gameObject = gObject;
    }

    public ScriptedSpawn(string name, float wDelay, float sDelay, int wAmount, Vector2 pos, GameObject gObject)
    {
        spawnName = name;
        waveDelay = wDelay;
        spawnDelay = sDelay;
        waveAmount = wAmount;
        spawnPosition = pos;
        gameObject = gObject;
    }

    public string spawnName;
    public float  waveDelay;
    public float  spawnDelay;
    public int    waveAmount;
    public Vector2 spawnPosition;
    public GameObject gameObject;
}

public class ScriptedSpawner : MonoBehaviour
{
    //[Header("Skip To Boss options")]
    //public bool skipToBoss = false;
    [Header("Testing Stage script")]
    public bool isTestingStage;
    [Header("StageScript")]
    public TextAsset[] spawnScripts;
    public TextAsset defaultScript;
    private TextAsset spawnScript;

    [Header("Game objects to be spawned (fill with prefabs)")]
    public List<GameObject> spawnObjectList;

    [Header("Boss settings")]
    //public GameObject bossSpawn;
    public GameObject[] bossSpawns;
    public float bossSpawnDelay;
    private bool hasBossSpawned;
    public List<Transform> bossPatrolList = new List<Transform>();

    [Header("Spawn boundaries")]
    public Transform min;
    public Transform max;
    public Transform bossStartPos;
    public Transform bossEndPos;

    private Queue<ScriptedSpawn> spawnQueue;
    private char[] delimiterChars = { ' ', ',', '\t', '<', '>' };
    //private string pattern = "^[a-zA-Z][a-zA-Z0-9]*$";
    private float waveTimer;
    private float bossTimer;

    //we should link playership using this, 
    //check if <EnemyShip> is !null for the spawn prefanb
    [Header("PlayerShip")]
    public GameObject playerShip;
    public GameController gameController;


    // Use this for initialization
    void Awake()
    {        
        spawnQueue = new Queue<ScriptedSpawn>();
        SetStageText();
        if (!GameController.skipToBoss)
        {
            ParseTextFileAndSetupSpawnQueue();
        }
    }

    void Start()
    {        
        hasBossSpawned = false;
        // Set the initial spawn time based on the first enemy
        if(!GameController.skipToBoss)
            waveTimer = spawnQueue.Peek().waveDelay;
        bossTimer = bossSpawnDelay;
        //Debug.Log("spawning a " + spawnQueue.Peek().spawnName + " in " + waveTimer + " seconds");
    }

    void Update()
    {
        waveTimer -= Time.deltaTime;

        if (spawnQueue.Count > 0 && waveTimer <= 0 && !gameController.GetComponent<GameController>().isGameOver)
        {        
            spawnEnemyFromQueue();
            /*
            Debug.Log("spawning a " + spawnQueue.Peek().spawnName + " in " + waveTimer + " seconds and at <" + 
                spawnQueue.Peek().spawnPosition.x + "," + spawnQueue.Peek().spawnPosition.y);
            */
        }
        
        if (spawnQueue.Count <= 0 && !hasBossSpawned)
        {
            // Spawn the boss as soon as there are no more enemy ships
            // and no more powerups on field
            if (FindObjectOfType<EnemyShip>() == null &&
                FindObjectOfType<Powerup>() == null)
            {
                // Only start the timer if there is no longer any enemyships
                bossTimer -= Time.deltaTime;
                if (bossTimer <= 0)
                {
                    spawnBossObject();
                    hasBossSpawned = true;
                }
            }
            else
            {
                bossTimer = bossSpawnDelay;
            }                
        }
    }
  
    private void spawnEnemyFromQueue()
    {
        ScriptedSpawn spawn;
        spawn = spawnQueue.Dequeue();

        StartCoroutine(SpawnObject(spawn));

        if (spawnQueue.Count > 0)
        {
            waveTimer = spawnQueue.Peek().waveDelay;
        }
    }

    private void spawnBossObject()
    {
        GameObject spawnedObject = 
            Instantiate(bossSpawns[GameController.stage], bossStartPos.position, Quaternion.identity);

        EnemyBoss boss = spawnedObject.GetComponent<EnemyBoss>();

        boss.setStartEndPosition(bossStartPos.position, bossEndPos.position);
        //boss.AddPatrolTransforms(bossPatrolList.ToArray());

        boss.patrolTransformList.AddRange(bossPatrolList);

        BGM bgm = FindObjectOfType<BGM>();
        bgm.SwapBGM(GameAudio.bossTransition);
    }

    IEnumerator SpawnObject(ScriptedSpawn spawnObject)
    {
        Vector3 position;

        for (int i = 0; i < spawnObject.waveAmount; ++i)
        {
            // Use random position for now 
            if (spawnObject.spawnPosition == Vector2.zero)
            {
                position = new Vector3(Random.Range(min.position.x, max.position.x),
                                       Random.Range(min.position.y, max.position.y),
                                       0.0f);
            }
            else
            {
                position = spawnObject.spawnPosition;
            }

            Instantiate(spawnObject.gameObject, position, Quaternion.identity);

            yield return new WaitForSeconds(spawnObject.spawnDelay);
        }        
    }



    // Parse and add the words from the word text file into the word list
    private void ParseTextFileAndSetupSpawnQueue()
    {
        // Parse each line in the textfile
        string[] lines = spawnScript.text.Split('\n');
        // Parse each word in that line
        foreach (string line in lines)
        {
            string[] words = line.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
            {
                // Skip empty lines
                continue;
            }
            
            string sName = words[0];                // Get name from file
            float wDelay = float.Parse(words[1]);  // Get and convert spawn wait from file
            float sDelay = float.Parse(words[2]);
            int wAmount = int.Parse(words[3]);
            Vector2 sPos = Vector2.zero;

            //for(int i = 0; i <words.Length; i++)
            //{
                //Debug.Log(i + ": " + words[i]);
            //}

            // Set the spawn position if it is provided
            if (words.Length >= 6)
            {
                sPos = new Vector2(float.Parse(words[4]), float.Parse(words[5]));
            }

            //Debug.Log("storing a " + sName + " in " + wDelay + " seconds and at <" +
            //  sPos.x + "," + sPos.y + ">");
            // Check if the string matches any of the attached prefabs
            foreach (GameObject go in spawnObjectList)
            {
                if (go != null)
                {
                    // Find the matching gameobject based on the name string (case-insensitive)
                    if (go.name.ToString().ToLower().Equals(sName.ToLower()))
                    {
                        spawnQueue.Enqueue(new ScriptedSpawn(sName, wDelay, sDelay, wAmount, sPos, go));
                    }
                }
            }
        }
    }
    void SetStageText()
    {
        if (GameController.stage < spawnScripts.Length && spawnScripts[GameController.stage] != null && !isTestingStage)
        {
            spawnScript = spawnScripts[GameController.stage];
            //spawnScript = defaultScript;
        }
        else
        {
            spawnScript = defaultScript;
        }
    }

}
