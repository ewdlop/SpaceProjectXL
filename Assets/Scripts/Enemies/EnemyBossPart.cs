using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossPart : EnemyShip
{

    [Header("Phase1 Weapons/Actions")]
    public List<GameObject> phase1Weapons;
    public List<EnemyAction> phase1Actions;
    [Header("Phase2 Weapons/Actions")]
    public List<GameObject> phase2Weapons;
    public List<EnemyAction> phase2Actions;
    [Header("Phase3 Weapons/Actions")]
    public List<GameObject> phase3Weapons;
    public List<EnemyAction> phase3Actions;


    // This is set by the enemyboss that controls the attachments
    // normal enemy ships do not have to deal with this
    private bool hasChangedWeapons;
    private int currentPhase;

    // Use this for initialization
    protected new void Start()
    {
        enemyWeaponList = new List<GameObject>();

        base.Start();
        timer = 0;
        parentShip = GetComponentInParent<EnemyBoss>();
        currentPhase = 0;

        if (isAmbushShip)
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

    private void CopyAndResetTimers<T>(List<T> source, ref List<T> dest)
    {
        dest = new List<T>(source);
    }

    private void ResetWeaponTimers()
    {
        weaponCooldownTimerList.Clear();
        for (int i = 0; i < enemyWeaponList.Count; ++i)
        {
            weaponCooldownTimerList.Add(0);
        }
    }

    // This function activates/deactivates the weapons associated with the current boss phase
    private void RefitWeapons()
    {
        //Debug.Log("Current Phase " + currentPhase);
        //Debug.Log("Parent Current Phase " + parentShip.currentPhase);

        // Phase has changed, so refit all weapons
        if (currentPhase != parentShip.currentPhase)
        {
            //Debug.Log("Refitting weapons for phase " + currentPhase);
            currentPhase = parentShip.currentPhase;
            if (currentPhase == 1)
            {
                actionsList = new List<EnemyAction>(phase1Actions);
                //Debug.Log("Phase1 actions: " + actionsList.Count);
                enemyWeaponList.Clear();
                enemyWeaponList = new List<GameObject>(phase1Weapons);
                ResetWeaponTimers();
            }
            else if (currentPhase == 2)
            {
                actionsList = new List<EnemyAction>(phase2Actions);
                //Debug.Log("Phase2 actions: " + actionsList.Count);
                enemyWeaponList.Clear();
                enemyWeaponList = new List<GameObject>(phase2Weapons);
                ResetWeaponTimers();
            }
            else if (currentPhase == 3)
            {
                actionsList = new List<EnemyAction>(phase3Actions);
                //Debug.Log("Phase3 actions: " + actionsList.Count);
                enemyWeaponList.Clear();
                enemyWeaponList = new List<GameObject>(phase3Weapons);
                ResetWeaponTimers();
            }
            else
            {
                Debug.Log("This boss has more than 3 phases?!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        RefitWeapons();

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

    public new void Fire()
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

    IEnumerator Shots(Weapon weapon, int count)
    {
        if (count > 0)
        {
            count--;
            weapon.Shoot(gameObject.transform, leftFirePosition, rightFirePosition);
            yield return new WaitForSeconds(weapon.shotDelay);
            StartCoroutine(Shots(weapon, count));
        }
        yield return null;
    }

    // Run the actions associated with that ship
    private void RunAction()
    {
        EnemyActions.runAction(this.gameObject);
    }

    IEnumerator FireCooldown(Weapon weapon)
    {
        weapon.isUnlocked = false;
        yield return new WaitForSeconds(weapon.cooldown);
        weapon.isUnlocked = true;
    }

    new void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            if (hasCollided == false)
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
                else
                {
                    if (parentShip.BossIsActive())
                    {
                        if (!parentShip.isBoss3)
                            parentShip.decrementHealth(other.gameObject.GetComponent<Weapon>().damage);
                    }
                }
                Destroy(other.gameObject);
                //0.5f so it is not so "cracked"
                //renderer.material.SetFloat("_OcclusionStrength", 0.5f*(1.0f - healthPercentage));
                StartCoroutine(HitFlash());
            }
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
