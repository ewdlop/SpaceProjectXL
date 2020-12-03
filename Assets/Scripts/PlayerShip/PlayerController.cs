using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed = 10.0f;
    public Transform WinPosition; // Ship moves upwards to this point when clicking the continue screen
    public GameObject minBound;
    public GameObject maxBound;

    [Header("Total lives")]
    public static int maxLives = 3;
    private int lives;

    [Header("Spawn Settings")]
    public Transform playerSpawn; // Where the player is respawned
    public GameObject colliderCircle;
    public float invincibleDuration = 1.2f;
    [SerializeField]
    private bool isInvincible;
    public float deathDuration = 1.5f;
    [SerializeField]
    private bool isDead;

    [Header("Shooting Controls")]
    public Transform leftFirePosition; 
    public Transform rightFirePosition;
    public float shotCoolDown = 0.2f;
    public GameObject mainWeaponObject;
    public GameObject supportWeaponObject;
    private Weapon mainWeapon;
    private Weapon supportWeapon;

    [Header("Ultimate Controls")]
    public GameObject ultimateWeaponObject;
    private float ultimateProgress; // current progress of the ultimate bar
    private bool isDrainingProgess; // sets the ultimate bar to be drained
    public float chargingDuration;  // the amount the bar is charged per tick
    public float dischargingDuration;  // the amount the bar is charged per tick
    public float ultimateDuration;  // the maximum amount allowed of the ultimate bar
    private Weapon ultimateWeapon;
    [Header("LaserBeam")]
    public GameObject laserBeam;
    public bool isShootingLaserBeam;
    [Header("Powerup")]
    [Header("Cannon Angle")]
    public GameObject cannon;
    public float cannonAngle;
    public int isFireRateBoosted;
    // For use in making ship flash upon receiving damage
    private new Renderer renderer;
    private Renderer circleRenderer;
    //private Renderer cannonRenderer;
    private bool isMovingToWin;
    private bool canShoot = true;

    private Vector3 startPosition, endPosition;
    private float startTime;
    private float journeyLength;

    [Header("Shield")]
    public GameObject shield;

    [Header("PowerUpImage")]
    [SerializeField]
    private Image shieldImage;
    [SerializeField]
    private Image multiDirectionalImage;
    [SerializeField]
    private Image fireRateBoostImage;
    public bool IsDead => isDead;
    public bool IsInvincible => isInvincible;
    public Image ShieldImage { get => shieldImage; set => shieldImage = value; }
    public Image MultiDirectionalImage { get => multiDirectionalImage; set => multiDirectionalImage = value; }
    public Image FireRateBoostImage { get => fireRateBoostImage; set => fireRateBoostImage = value; }

    void Start ()
    {
        isMovingToWin = false;
        canShoot = true;
        lives = maxLives;
        isInvincible = false;
        cannonAngle = 0f;
        renderer = GetComponent<Renderer>();
        circleRenderer = colliderCircle.GetComponent<Renderer>();
        mainWeapon = mainWeaponObject.GetComponent<Weapon>();
        mainWeapon.cooldownStamp = Time.time;
   
        supportWeapon = supportWeaponObject.GetComponent<Weapon>();
        supportWeapon.cooldownStamp = Time.time;

        ultimateWeapon = ultimateWeaponObject.GetComponent<Weapon>();
        //ultimateProgress = 0.0f;
        ultimateProgress = 0.7f;
        ultimateWeapon.cooldownStamp = 0.0f;

        EnableShield(false);
        EnableCannon(false);
    }

	void Update ()
    {
        if (!IsDead)
        {
            if (!isMovingToWin)
            {
                Kinematics();
                Shoot();
            }
            else
            {
                LerpMovementToWin();
            }
            if(cannon.activeInHierarchy)
                LineUpShipWithCursor();
        }
    }

    public void MoveShipToWin()
    {
        isMovingToWin = true;
        SoundController.Play((int)SFX.ShipThrust, 1.0f);
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, WinPosition.position.y, transform.position.z);
        journeyLength = Vector2.Distance(startPosition, endPosition);
        startTime = Time.time;
    }

    void LerpMovementToWin()
    {
        float distCovered = (Time.time - startTime) * (speed*1.5f);
        float fracJourney = distCovered / journeyLength;

        if (fracJourney < 1.0f)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, fracJourney);
        }
        else
        {
            // Move to next stage once ship reaches the win position thats off screen
            GameController.instance.MoveToNextStage();
        }
    }

    void LineUpShipWithCursor()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cannonAngle = Mathf.Atan2(
            mouseWorldPosition.y - cannon.transform.position.y,
            mouseWorldPosition.x - cannon.transform.position.x) * Mathf.Rad2Deg;
        cannon.transform.eulerAngles = new Vector3(0f, 0f, cannonAngle - 90f);
        transform.eulerAngles = new Vector3(0f, 0f, cannonAngle - 90f);
    }

    // Controls player movement
    void Kinematics()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.position += deltaX * new Vector3(1f, 0f, 0f);
        float deltaY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.position += deltaY * new Vector3(0f, 1f, 0f);

        float newX = gameObject.transform.position.x;
        float newY = gameObject.transform.position.y;

        // Check if the ship has gone out of bounds
        if (gameObject.transform.position.x >= maxBound.transform.position.x)
        {
            newX = maxBound.transform.position.x;
        }
        else if (gameObject.transform.position.x <= minBound.transform.position.x)
        {
            newX = minBound.transform.position.x;
        }

        if (gameObject.transform.position.y >= maxBound.transform.position.y)
        {
            newY = maxBound.transform.position.y;
        }
        else if (gameObject.transform.position.y <= minBound.transform.position.y)
        {
            newY = minBound.transform.position.y;
        }
        
        // Reset the new position, this is for if the ship goes out of bounds
        gameObject.transform.position = new Vector3(newX, newY, 0f);
       
    }

    // Controls shooting 
    void Shoot()
    {
        if (canShoot)
        {
            // Fire the main weapon if off cooldown
            if ((Input.GetAxis("MainFire") > 0) &&
                (mainWeapon.cooldownStamp < Time.time))
            {
                mainWeapon.Shoot(this.gameObject.transform, leftFirePosition, rightFirePosition);
                mainWeapon.cooldownStamp = Time.time + mainWeapon.cooldown / (1 + isFireRateBoosted);
            }

            // Fire the support weapon if off cooldown
            if ((Input.GetAxis("SupportFire") > 0) &&
                (supportWeapon.cooldownStamp < Time.time))
            {
                supportWeapon.Shoot(this.gameObject.transform, leftFirePosition, rightFirePosition);
                supportWeapon.cooldownStamp = Time.time + supportWeapon.cooldown / (1 + isFireRateBoosted);
            }

            // Handle the ultimate 
            controlUltimate();
        }
    }

    void controlUltimate()
    {
        // Fire the ultimate weapon if bar is full and we click the ultimate button
        if (Input.GetKeyDown("r") && ultimateProgress >= ultimateDuration)
        {
            isDrainingProgess = true;
            FillUltimateProgress(-1 * Time.deltaTime / ultimateDuration);
        }      
        else
        {
            // if ultimate was activated
            if (isDrainingProgess)
            {
                FillUltimateProgress(-1 * Time.deltaTime / dischargingDuration);
                // Instantiate the ultimate weapon while the bar is not yet 0
                if (ultimateProgress > 0.0f) 
                {
                    if (ultimateWeapon.cooldownStamp < Time.time)
                    {
                        ultimateWeapon.Shoot(this.gameObject.transform, leftFirePosition, rightFirePosition);
                        ultimateWeapon.cooldownStamp = Time.time + ultimateWeapon.cooldown;
                    }
                }
                // Ultimate ends when bar reaches 0
                else
                {
                    isDrainingProgess = false;
                    if (isShootingLaserBeam)
                    {
                        isShootingLaserBeam = false;
                        EnableLaserBeam(false);
                    }

                }
            }
            else
            {
                // charging
                FillUltimateProgress(Time.deltaTime / chargingDuration);
            }
        }
    }

    // This function is used by the EnemyBoss to disable the player from firing when
    // the boss transition is being played
    public void SetCanShoot(bool _canShoot)
    {
        canShoot = _canShoot;
    }

    public void FillUltimateProgress(float amount)
    {
        ultimateProgress = Mathf.Clamp(ultimateProgress + amount, 0f, ultimateDuration);
        //this.ultimateSlider.GetComponent<Slider>().value = progress;
    }

    public void FillUltimateProgressToFull()
    {
        ultimateProgress = ultimateDuration;
    }

    public float GetUltimateProgress()
    {
        return ultimateProgress;
    }

    public void Hit()
    {
        StartCoroutine(Death());
    }
    IEnumerator HitFlash()
    {
        // Flicker effect
        while (isInvincible)
        {
            this.renderer.enabled = false;
            circleRenderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            this.renderer.enabled = true;
            circleRenderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        // Make the ship flash while ship is invincible
        StartCoroutine(HitFlash());
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    IEnumerator Death()
    {
        SoundController.Play((int)SFX.ShipDeath);
        renderer.enabled = false;
        circleRenderer.enabled = false;
        isDead = true;
        isInvincible = true;
        EnableShield(false);
        EnableCannon(false);
        EnableLaserBeam(false);
        EnableFireRateBoost(0);
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        yield return new WaitForSeconds(deathDuration);
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        lives--;
        if (lives >= 0)
        {
            this.transform.position = playerSpawn.position;
            isDead = false;
            ShootLaserBeam();
            StartCoroutine(InvincibilityFrames());
        }
        else
        {
            isShootingLaserBeam = false;
            GameController.instance.isGameOver = true;
            Destroy(this.gameObject);
        }
    }

    public void AddLife()
    {
        lives++;
    }

    public int GetLives()
    {
        return lives;
    }

    public void EnableShield(bool enable)
    {
        shield.SetActive(enable);
        ShieldImage.gameObject.SetActive(enable);
    }

    public void EnableCannon(bool enable)
    {
        cannon.SetActive(enable);
        MultiDirectionalImage.gameObject.SetActive(enable);
        if (!enable)
            cannonAngle = 90f;
    }

    public void EnableLaserBeam(bool enable)
    {
        laserBeam.SetActive(enable);
    }

    public void EnableFireRateBoost(int enable)
    {
        FireRateBoostImage.gameObject.SetActive(enable!=0);
        isFireRateBoosted = enable;
    }

    public void ShootLaserBeam()
    {
        if (isShootingLaserBeam)
            EnableLaserBeam(true);
    }
}
