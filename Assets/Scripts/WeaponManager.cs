using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public GameObject prefab;
    public string name;
    public int damage;
    public float launchAngle;
    public float speed;
    public bool isUnlocked;

    public Weapon(GameObject prefab, string weaponName, int weaponDamage, float launchAngle, float speed, bool isUnlocked)
    {
        this.prefab = prefab;
        this.name = weaponName;
        this.damage = weaponDamage;
        this.launchAngle = launchAngle;
        this.speed = speed;
        this.isUnlocked = isUnlocked;
    }
}

public class WeaponManager : MonoBehaviour {

    public List<GameObject> weaponPrefab;
    public static WeaponManager instance;
    public static List<Weapon> playerWeaponList = new List<Weapon>();
    public static List<Weapon> leftCorvetteWeaponList = new List<Weapon>();
    public static List<Weapon> rightCorvetteWeaponList = new List<Weapon>();
    void Awake ()
    {
        // Create each weapon the player can use
        Weapon redLaser = new Weapon(weaponPrefab[0], "RedLaserBullet", 2, 0.0f, 25.0f, true);
        Weapon leftCorvetteredLaser = new Weapon(weaponPrefab[0], "RedLaserBullet", 2, 0.0f, 25.0f, true);
        Weapon rightCorvetteredLaser = new Weapon(weaponPrefab[0], "RedLaserBullet", 2, 0.0f, 25.0f, true);
        Weapon blueLaser = new Weapon(weaponPrefab[1], "BlueLaserBullet", 2, 10.0f, 25.0f, false);
        Weapon sineMissles = new Weapon(weaponPrefab[2], "SineMissles", 2, 0.0f, 50.0f, false);
        Weapon circleMissles = new Weapon(weaponPrefab[3], "CircleMissles", 2, 0.0f, 50.0f, false);
        Weapon spreadLaser = new Weapon(weaponPrefab[4], "SpreadLaser", 2, 0.0f, 50.0f, false);
        Weapon redRocketMissiles = new Weapon(weaponPrefab[5], "RedRocketMissiles", 2, 0.0f, 0f, false);
        Weapon chasingMissiles = new Weapon(weaponPrefab[6], "ChasingMissiles", 2, 0.0f, 40f, false);
        Weapon laserBeam = new Weapon(weaponPrefab[7], "LaserBeam", 2, 0.0f, 40f, false);
        // Store the weapons into the weapon list
        playerWeaponList.Add(redLaser);
        playerWeaponList.Add(blueLaser);
        playerWeaponList.Add(sineMissles);
        playerWeaponList.Add(circleMissles);
        playerWeaponList.Add(spreadLaser);
        playerWeaponList.Add(redRocketMissiles);
        playerWeaponList.Add(chasingMissiles);
        playerWeaponList.Add(laserBeam);
        leftCorvetteWeaponList.Add(leftCorvetteredLaser);
        rightCorvetteWeaponList.Add(rightCorvetteredLaser);
    }

}
