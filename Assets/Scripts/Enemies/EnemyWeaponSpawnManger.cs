using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWeapon
{
    public GameObject enemyWeaponPrefab;
    public float time;
    public float coolDown;
    public float fireDelay;
    public float fireDelayBetweenSets;
    public float timeStamp;
    public int numberOfShotsPerSet;
    public int numberOfSets;
    public bool isCanShoot;

    public EnemyWeapon(GameObject enemyWeaponPrefab,float time, float coolDown, float fireDelay,float fireDelayBetweenSets, float timeStamp, int numberOfShotsPerSet,int numberOfSets, bool isCanShoot){
        this.enemyWeaponPrefab = enemyWeaponPrefab;
        this.time = time;
        this.coolDown = coolDown;
        this.fireDelay = fireDelay;
        this.fireDelayBetweenSets = fireDelayBetweenSets;
        this.timeStamp = timeStamp;//using coroutine, we might dont need this
        this.numberOfShotsPerSet = numberOfShotsPerSet;
        this.numberOfSets = numberOfSets;
        this.isCanShoot = isCanShoot;
    }
}
public class EnemyWeaponSpawnManger : MonoBehaviour
{

    public static List<EnemyWeapon> boss1WeaponList = new List<EnemyWeapon>();
    public static List<EnemyWeapon> boss1LeftTurretWeaponList = new List<EnemyWeapon>();
    public static List<EnemyWeapon> boss1RightTurretWeaponList = new List<EnemyWeapon>();
    public static List<EnemyWeapon> boss1BackTurretWeaponList = new List<EnemyWeapon>();
    public static List<EnemyWeapon> boss2WeaponList = new List<EnemyWeapon>();
    public List<GameObject> boss1WeaponPrefab;

    void Awake()
    {
        EnemyWeapon boss1GreenBall = new EnemyWeapon(boss1WeaponPrefab[1], 0, 5f, 0.05f, 1f, 0f, 3, 3, true);
        EnemyWeapon boss1YellowBallRight = new EnemyWeapon(boss1WeaponPrefab[2], 0, 0.1f, 0f, 0f, 0f, 1, 1, true);
        EnemyWeapon boss1YellowBallLeft = new EnemyWeapon(boss1WeaponPrefab[2], 0, 0.1f, 0f,0f, 0f, 1, 1, true);
        EnemyWeapon boss1BlueBall = new EnemyWeapon(boss1WeaponPrefab[3], 0, 15f, 0f, 0f, 0f, 1, 1, true);
        EnemyWeapon boss2RedBall = new EnemyWeapon(boss1WeaponPrefab[0], 0, 0.1f, 0f, 0f, 0f, 1, 1, true);
        EnemyWeapon boss2BrownBall = new EnemyWeapon(boss1WeaponPrefab[4], 0, 5f, 0f, 0f, 0f, 1, 1, true);
        EnemyWeapon boss2GreenMissile = new EnemyWeapon(boss1WeaponPrefab[5], 0, 5f, 0.2f, 1f, 1f, 4, 4, true);
        boss1WeaponList.Add(boss1GreenBall);
        boss1LeftTurretWeaponList.Add(boss1YellowBallLeft);
        boss1RightTurretWeaponList.Add(boss1YellowBallRight);
        boss1BackTurretWeaponList.Add(boss1BlueBall);
        boss2WeaponList.Add(boss2RedBall);
        //boss2WeaponList.Add(boss2BrownBall);
        boss2WeaponList.Add(boss2GreenMissile);
    }
}
