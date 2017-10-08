using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeManger : MonoBehaviour {

    public List<GameObject> enemyTypeList;
    public List<GameObject> spawnEnemyAfterDeath;
    public List<int> numberOfSpawn;
    public List<float> health;
    public List<float> enemyTypeDamageOnPlayerSpaceShip;
    public List<float> scoreFromHitting;
    public List<float> scoreFromDestroying;
    public List<string> enemyCloneName;
    public List<bool> isItTakesDamage;

    // so like this 
    class Enemy
    {
        int numberOfSpawn;
        int health;
        int score;
        string name;
        bool doesTakeDamage;
    }


    public static GameObject enemyTypeManger;

    //public static List<GameObject> enemyTypeListStatic;
    //public static List<float> enemyTypeDamageOnPlayerSpaceShip;

   void Awake()
    {
        enemyTypeManger = gameObject;
    }
	
    public static GameObject getEnemyTypeManger()
    {
        return enemyTypeManger;
    }

}
