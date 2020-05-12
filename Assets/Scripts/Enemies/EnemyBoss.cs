using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss : Enemy
{
    public enum BossState
    {
        StartMovement,
        Patrol,
        PrepareCharge,
        Charge,
        Return
    };

    public BossState currentState;

    // Bosses have multiple phases
    public int currentPhase = 1;

    [Header("Weapons")]
    public List<GameObject> enemyWeaponList;
    public EnemyAction[] actionsList;    
    public float actionsDelay = 0.5f; //not using yet

    [Header("Shooting settings")]
    public Transform leftFirePosition;
    public Transform rightFirePosition;    
    private PlayerController player;
    [SerializeField]
    private List<float> weaponCooldownTimerList = new List<float>();

    [Header("Movement settings")]
    public float speed = 1.0f;
    public float chargeSpeed = 10.0f;
    public float patrolTimerMax = 5.0f;
    private float patrolTimer;
    private Vector3 startPosition;
    private Vector3 endPosition;      // the position the boss will move to
    private Vector3 patrolPosition;
    private bool hasReachedEndPos;    // deactivates the weapons until it reaches the endPosition
    private bool hasReachedPatrolPos; // controls whether boss moves to next patrol location
    private bool hasReachedChargePos; // controls whether boss has finished charging
    private bool hasReachedReturnPos; // for after a boss has finished charging
    private bool hasStartedLerp;
    private bool hasNewLerpPositions;
    
    private float journeyLength;
    private float startTime;

    private float timer;
    private BGM bgm;
    public Text bossHealthText;

    public List<Transform> patrolTransformList = new List<Transform>();
    [Header("Boss3")]
    public bool isBoss3;
    // Use this for initialization
    bool hasCharged;

    void EnterPhase1()
    {

    }

    void EnterPhase2()
    {

    }

    void EnterPhase3()
    {

    }

    protected new void Start()
    {        
        bgm = FindObjectOfType<BGM>();
        hasReachedEndPos = false;
        startTime = Time.time;
        currentState = BossState.StartMovement;
        //journeyLength = Vector3.Distance(startPosition, endPosition);
        patrolTimer = patrolTimerMax;
        base.Start(); // Setup the hit flash
        player = FindObjectOfType<PlayerController>();
        player.SetCanShoot(false);
        for (int i = 0; i < enemyWeaponList.Count; ++i)
        {
            weaponCooldownTimerList.Add(0);
        }
        
    }

    void BossActionSwitches()
    {
        switch (currentState)
        {
            case BossState.StartMovement:
                StartMovement();
                break;
            case BossState.Patrol:
                Patrol();
                break;

            case BossState.Charge:
                ChargeAtPlayer();
                break;

            case BossState.Return:
                ReturnAfterCharge();
                break;

            default:
                Patrol();
                break;
        }
    }

    // Boss slowly moves down to the start position before entering patrol
    void StartMovement()
    {
        if (!hasReachedEndPos)
        {
            LerpMovement(ref hasReachedEndPos, speed);
        }
        else
        {
            bgm.SwapBGM(GameAudio.bossBattle);
            player.SetCanShoot(true);
            currentState = BossState.Patrol;
            hasNewLerpPositions = false;
            hasReachedEndPos = true;
            hasReachedPatrolPos = true;
            bossHealthText = GameObject.Find("BossHealthNumber").GetComponent<Text>();
            StringBuilder sb = new StringBuilder(health.ToString());
            sb.Append("/");
            sb.Append(maxHealth.ToString());
            bossHealthText.text = sb.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        BossActionSwitches();

        // Unlock boss weapons when boss is outside of the state movement position
        if (currentState != BossState.StartMovement)
        {
            // Increment each weaponCooldownTimer to determine which weapon should be unlocked
            for (int i = 0; i < enemyWeaponList.Count; ++i)
            {
                weaponCooldownTimerList[i] -= Time.deltaTime;
            }

            // Testing charge   
            /*
            if (!hasCharged)
            {
                currentState = BossState.Charge;
                hasCharged = true;
            }
            */
        }

        ChangePhase();

        if (health <= 0)
        {
            Death();
        }
    }

    private void ChangePhase()
    {
        float percentHealth = ((float)health / maxHealth);
        if (percentHealth < 0.75f && !hasCharged)
        {
            currentState = BossState.Charge;
            currentPhase = 2;
            hasCharged = true;
        }
        
        else if (percentHealth < 0.33f)
        {
            currentPhase = 3;
        }
    }

    public void Patrol()
    {
        if (hasReachedPatrolPos)
        {
            //Debug.Log(patrolTimer);
            patrolTimer -= Time.deltaTime;
            // Make enemy patrol again after waiting
            if (patrolTimer <= 0)
            {
                hasReachedPatrolPos = false;
                //startTime = Time.time;
                // Find a new random point 
                //Debug.Log(patrolTransformList.Count);
                endPosition =
                    patrolTransformList[(int)(UnityEngine.Random.Range(0, patrolTransformList.Count))].position;

                startPosition = transform.position;
              
                patrolTimer = patrolTimerMax;
            }
        }
        else
        {
            LerpMovement(ref hasReachedPatrolPos, speed);
        }
    }

    void PrepareCharge()
    {
        // Get charge position, rotate then wait
        startPosition = transform.position;
        endPosition = player.transform.position;
        StartCoroutine(DelayAction(3.0f));
    }

    IEnumerator DelayAction(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        currentState = BossState.Charge;
    }

    IEnumerator DelayBySeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void ChargeAtPlayer()
    {
        //Debug.Log("Charging at player");
        if (!hasNewLerpPositions)
        {
            //Debug.Log("Charge new positions");
            startPosition = transform.position;
            endPosition = player.transform.position;
            hasNewLerpPositions = true;
            hasReachedChargePos = false;
        }

        if (!hasReachedChargePos)
        {
            //Debug.Log("Lerp to player");
            LerpMovement(ref hasReachedChargePos, chargeSpeed);
        }
        else
        {
            //Debug.Log("Done Lerping");            
            currentState = BossState.Return;
            hasNewLerpPositions = false;
            hasReachedChargePos = true;
        }        
    }

    void LerpMovement(ref bool toggleBool, float shipSpeed)
    {
        if (!hasStartedLerp)
        {
            journeyLength = Vector2.Distance(startPosition, endPosition);
            hasStartedLerp = true;
            startTime = Time.time;
        }

        if (toggleBool == false)
        {            
            float distCovered = (Time.time - startTime) * shipSpeed;
            float fracJourney = distCovered / journeyLength;

            if (fracJourney < 1.0f)
            {
                transform.position = Vector2.Lerp(startPosition, endPosition, fracJourney);
            }
            else
            {
                toggleBool = true;
                hasStartedLerp = false;
                //hasNewLerpPositions = false;
            }
        }
    }

    public void ReturnAfterCharge()
    {
        // Just go back to our starting position prior to the charge    
        if (!hasNewLerpPositions)
        {
            //
            //return to the start pos
            //
            endPosition = startPosition;
            startPosition = transform.position;

            hasNewLerpPositions = true;
        }

        if (!hasReachedReturnPos)
        {
            LerpMovement(ref hasReachedReturnPos, speed);
        }
        else
        {
            //
            //treat charge as patrol
            //
            hasReachedPatrolPos = true;
            currentState = BossState.Patrol;           
        }
    }

    public void AddPatrolTransforms(Transform[] list)
    {
        Debug.Log(list.Length);

    }

    protected override void Kinematics()
    {

    }

    public void Fire()
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
        if (other.GetComponent<Weapon>() != null && isBoss3 && !other.GetComponent<Weapon>().isReflected)
        {
            return;
        }
        if (other.tag == "Projectile" && !hasCollided)
        {
            hasCollided = true;

            Instantiate(other.gameObject.GetComponent<Weapon>().hiteffect,
               new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -0.01f),
               Quaternion.identity);
            //need to tune damage
            if (hasReachedEndPos)
            {
                health -= other.gameObject.GetComponent<Weapon>().damage;
                StringBuilder sb = new StringBuilder(health.ToString());
                sb.Append("/");
                sb.Append(maxHealth.ToString());
                bossHealthText.text = sb.ToString();
                StartCoroutine(HitFlash());
            }
            if (other.gameObject.GetComponent<Weapon>().isPlaysMissileImpactSound)
                SoundController.Play((int)SFX.MissileExplosion);
            Destroy(other.gameObject);
            float healthPercentage = Mathf.Clamp((float)health / (float)maxHealth, 0.0f, 1.0f);
            //0.5f so it is not so "cracked"
            renderer.material.SetFloat("_OcclusionStrength", 0.5f * (1.0f - healthPercentage));
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Slicer" && hasReachedEndPos)
        {
            health -= other.gameObject.GetComponent<Weapon>().damage;
            StringBuilder sb = new StringBuilder(health.ToString());
            sb.Append("/");
            sb.Append(maxHealth.ToString());
            if (bossHealthText != null)
                bossHealthText.text = sb.ToString();
            StartCoroutine(HitFlash());
            Instantiate(other.gameObject.GetComponent<Weapon>().hiteffect,
                   new Vector3(transform.position.x, transform.position.y, -0.01f),
                   Quaternion.identity);
            float healthPercentage = Mathf.Clamp((float)health / (float)maxHealth, 0.0f, 1.0f);
            renderer.material.SetFloat("_OcclusionStrength", 0.5f * (1.0f - healthPercentage));
        }
    }
    public void setStartEndPosition(Vector3 start, Vector3 end)
    {
        startPosition = start;
        endPosition = end;
    }

    public bool BossIsActive()
    {
        return hasReachedEndPos;
    }

    void Death()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyProjectile");
        foreach(GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        SoundController.Play((int)SFX.BossDeath);
        bgm.SwapBGM(GameAudio.normal);
        GameController.playerScore += score;
        Destroy(this.gameObject);
        GameController.instance.Win();
    }



    public void decrementHealth(int damage)
    {
        health -= damage;
    }

}

