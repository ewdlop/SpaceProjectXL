using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyShip : Enemy {

    [Header("Weapons")]
    public List<GameObject> enemyWeaponList;
    //public bool[] unlockWeapons = new bool[5];
    //public int[] actionsList;
    public List<EnemyAction> actionsList;
    //not using
    public float actionsDelay = 0.5f;
    //private List<float> weaponCooldownList;
    [SerializeField]
    public List<float> weaponCooldownTimerList = new List<float>();
    [Header("Shooting settings")]
    public Transform leftFirePosition;
    public Transform rightFirePosition;
    //private List<GameObject> enemyWeaponList = new List<GameObject>();

    // This is set by the enemyboss that controls the attachments
    // normal enemy ships do not have to deal with this
    protected EnemyBoss parentShip;
    protected bool canShoot;
    
    [Header("Movement settings")]
    public float speed = 5.0f;
    public float rotateSpeedFactor = 2f;
    public float timer;

    [Header("AmbushShipOnly")]
    public bool isAmbushShip;
    public float yRange;
    public bool isRightToLeft;

    [Header("RotateBetweenAnglesOnly(in degree)")]
    public float angleA;
    public float angleB;
    public float rotateSpeed;
    public float phase;
    public GameObject playerShip;

    // Use this for initialization
    protected new void Start () {

        parentShip = GetComponentInParent<EnemyBoss>();
           
        timer = 0;

        base.Start(); // Setup the hit flash

        if(isAmbushShip)
        {
            yRange = UnityEngine.Random.Range(-7.5f, 7.5f);
            float random = UnityEngine.Random.Range(-7.5f, 7.5f);
            if (random <= 0.5f) isRightToLeft = true;
        }

        for (int i = 0; i < enemyWeaponList.Count; ++i)
        {
            weaponCooldownTimerList.Add(0);
        }
       
	}
	
	// Update is called once per frame
	void Update () {

        if (parentShip != null)
        {
            if (parentShip.BossIsActive())
                canShoot = true;
            else
                canShoot = false;
        }
        else
        {
            canShoot = true;
        }     

        timer += Time.deltaTime;
        // Increment each weaponCooldownTimer to determine which weapon should be unlocked
        for (int i = 0; i < enemyWeaponList.Count; ++i)
        {            
            weaponCooldownTimerList[i] -= Time.deltaTime;
        }

        // Control movement 
        // Will have bug if there's two players 
        // Since we could potentially target a different player
        // in the shoot function while having the ship direct to player1
        //Kinematics();
        RunAction();        
        
        if (health <= 0)
        {
            Death();
        }
    }

    public void Fire()
    {
        if (canShoot)
        {
            // Shoot weapon
            for (int i = 0; i < enemyWeaponList.Count; ++i)
            {
                Weapon weapon = enemyWeaponList[i].GetComponent<Weapon>();
                // Fire only if the weapon is off cooldown                                    
                if (weaponCooldownTimerList[i] <= 0)
                {
                    //Parallel Recursive Coroutines? GG CPU?
                    StartCoroutine(Shots(weapon, weapon.totalShots));
                    weaponCooldownTimerList[i] = weapon.cooldown;
                    //StartCoroutine(FireCooldown(weapon));                
                }
            }
        }
    }

    IEnumerator Shots(Weapon weapon,int count)
    {
        if(count > 0)
        {
            count--;
            weapon.Shoot(gameObject.transform, leftFirePosition, rightFirePosition);
            yield return new WaitForSeconds(weapon.shotDelay);
            StartCoroutine(Shots(weapon,count));
        }
        yield return null;
    }

    // Run the actions associated with that ship
    private void RunAction()
    {
        EnemyActions.runAction(this.gameObject);
    }

    protected override void Kinematics()
    {

    }

    IEnumerator FireCooldown(Weapon weapon)
    {
        weapon.isUnlocked = false;
        yield return new WaitForSeconds(weapon.cooldown);
        weapon.isUnlocked = true;
    }

    new void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile" && hasCollided == false)
        {
            hasCollided = true;
            Instantiate(other.gameObject.GetComponent<Weapon>().hiteffect,
               new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -0.01f),
               Quaternion.identity);
            if (other.gameObject.GetComponent<Weapon>().isPlaysMissileImpactSound)
                SoundController.Play((int)SFX.MissileExplosion);
            //need to tune damage
            if (parentShip == null)
            {
                health -= other.gameObject.GetComponent<Weapon>().damage;
            }
            else if (parentShip.BossIsActive() && !parentShip.isBoss3)
            {
                parentShip.decrementHealth(other.gameObject.GetComponent<Weapon>().damage);
            }
            Destroy(other.gameObject);
            //0.5f so it is not so "cracked"
            //renderer.material.SetFloat("_OcclusionStrength", 0.5f*(1.0f - healthPercentage));
            StartCoroutine(HitFlash());
        }
    }
    //for boomerange, draven ult, laser beam type of weapon
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Slicer")
        {
            if (parentShip == null)
            {
                health -= other.gameObject.GetComponent<Weapon>().damage;
                Instantiate(other.gameObject.GetComponent<Weapon>().hiteffect,
                      new Vector3(transform.position.x, transform.position.y, -0.01f),
                      Quaternion.identity);
                StartCoroutine(HitFlash());
            }
            else
            {
                if (parentShip.BossIsActive())
                {
                    Instantiate(other.gameObject.GetComponent<Weapon>().hiteffect,
                      new Vector3(transform.position.x, transform.position.y, -0.01f),
                      Quaternion.identity);
                    parentShip.decrementHealth(other.gameObject.GetComponent<Weapon>().damage);
                    StartCoroutine(HitFlash());
                }
            }
        }
    }
    void Death()
    {
        // Only destroy the enemy ship if it's not attached to a boss ship
        if (parentShip == null)
        {           
            SoundController.Play((int)SFX.ShipDeath, 0.25f);
            GameController.playerScore += score;
            Destroy(this.gameObject);
        }
    }
}
