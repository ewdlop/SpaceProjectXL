using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipActions : MonoBehaviour {

    Vector3 mouseWorldPosition;
    Rigidbody2D rb;
    public float torqueStrengthAngle;
    public float boostStrength;
    public float playerShipFacingAngle;
    public float dotProduct;
    public float speed;

    public Camera mainCam;
    public GameObject leftEngineFire;
    public GameObject rightEngineFire;
    public GameObject sideLeftEngineFire;
    public GameObject sideRightEngineFire;
    public GameObject enemyTypeManger;
    public GameObject powerUpManger;
    public GameObject shield;
    public List <PowerUp> powerUpList=new List<PowerUp>();

    public Text speedText;


    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        powerUpList = powerUpManger.GetComponent<PowerUpManger>().GetPowerUpList();
    }

	void Update ()
    {
        if (!GameController.instance.isGameOver && !MenuManager.isPaused)
        {
            mouseWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            playerShipFacingAngle = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * 180 / Mathf.PI;
            speedText.text = "Speed:"+rb.velocity.magnitude;
            //transform.eulerAngles = new Vector3(0, 0, PlayerShip_FacingAngle - 90);

            //For EngineFireBoost  Sprites;
            if (Input.GetButtonUp("Vertical"))
            {
                leftEngineFire.gameObject.SetActive(false);
                rightEngineFire.gameObject.SetActive(false);
            }
            if (Input.GetButtonDown("Vertical"))
            {
                leftEngineFire.gameObject.SetActive(true);
                rightEngineFire.gameObject.SetActive(true);
            }
            if(Input.GetButton("Horizontal") && Mathf.Sign(Input.GetAxis("Horizontal") )== 1){
                sideLeftEngineFire.gameObject.SetActive(true);
                sideRightEngineFire.gameObject.SetActive(false);
            }
            if (Input.GetButtonUp("Horizontal") && Mathf.Sign(Input.GetAxis("Horizontal")) == 1)
            {
                sideLeftEngineFire.gameObject.SetActive(false);
            }
            if (Input.GetButton("Horizontal") && Mathf.Sign(Input.GetAxis("Horizontal")) == -1)
            {
                sideRightEngineFire.gameObject.SetActive(true);
                sideLeftEngineFire.gameObject.SetActive(false);
            }
            if (Input.GetButtonUp("Horizontal") && Mathf.Sign(Input.GetAxis("Horizontal")) == -1)
            {
                sideRightEngineFire.gameObject.SetActive(false);
            }
            if (Input.anyKey)
                Kinematics();

        }


    }


    void Kinematics()
    {
        //Boost foward in the direction which the playSpaceShip faces.
        //float forceX = (Input.GetAxis("Vertical") * rb.mass * boostStrength * Mathf.Cos((transform.eulerAngles.z + 90) * Mathf.PI / 180));
        //float forceY = (Input.GetAxis("Vertical") * rb.mass * boostStrength * Mathf.Sin((transform.eulerAngles.z + 90) * Mathf.PI / 180));

        if (Input.GetAxis("Horizontal") != 0)
        {       
            float xDirection=Input.GetAxis("Horizontal");

            //Rotate the spaceship
            //transform.eulerAngles = transform.eulerAngles+new Vector3(0, 0, -torqueStrengthAngle * Mathf.Sign(Input.GetAxis("Horizontal")));

            //Keep the speed and turn the spaceship
            //float initialVelocityX= rb.velocity.magnitude*Mathf.Cos((transform.eulerAngles.z + 90) * Mathf.PI / 180);
            //float initialVelocityY= rb.velocity.magnitude*Mathf.Sin((transform.eulerAngles.z + 90) * Mathf.PI / 180); 

            //float finalVelocityX= rb.velocity.magnitude * Mathf.Cos((transform.eulerAngles.z + 90 - direction * torqueStrengthAngle) * Mathf.PI / 180); 
            //float finalVelocityY= rb.velocity.magnitude * Mathf.Sin((transform.eulerAngles.z + 90 - direction * torqueStrengthAngle) * Mathf.PI / 180);

            //float tangentialForceX = finalVelocityX - initialVelocityX;
            //float tangentialForceY = finalVelocityY - initialVelocityY;

            //Update velocity


            float deltaX = xDirection * 10 * Time.deltaTime;
            gameObject.transform.position = gameObject.transform.position + new Vector3(deltaX, 0f,0f);
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            float yDirection = Input.GetAxis("Vertical");
            float deltaY = yDirection * 10 * Time.deltaTime;
            gameObject.transform.position = gameObject.transform.position + new Vector3(0f, deltaY, 0f);
        }
    }

   
    void OnCollisionEnter2D(Collision2D collidedTarget)
    {
        if (collidedTarget.gameObject.tag == "PowerUp")
        {  
            foreach (PowerUp powerUp in powerUpList)
            {
                int index =powerUpList.IndexOf(powerUp);

                if (collidedTarget.gameObject.name.Contains(powerUp.powerUpName))
                {
                    float healthRepair = powerUp.healthRepair;

                    gameObject.GetComponent<PlayerShipDestructible>().IncreaseHealth(healthRepair);
                    switch (powerUp.playerWeaponIndex)
                    {
                        case 100://health powerup
                            DestroyObject(collidedTarget.gameObject);
                            break;
                        case 101://shield powerup
                            PowerUpManger.weaponPanelOffset += 1f;
                            //TODO unlock using the name
                            //WeaponManager.playerWeaponList[1].isUnlocked = true;
                            shield.SetActive(true);
                            PowerUpManger.shieldOn = true;
                            powerUp.IncreaseDuration();//increase the duration
                            powerUp.SetProgress();//update the bar
                            powerUp.weaponPanelBar.SetActive(true);
                            DestroyObject(collidedTarget.gameObject);
                            //*powerUp.weaponPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-80f, -38.4f + 102.4f * PowerUpManger.weaponPanelOffset);

                            break;
                        default:
                            int weaponIndex =powerUp.playerWeaponIndex;
                            //WeaponManager.playerWeaponList[weaponIndex].isUnlocked = true;
                            PowerUpManger.weaponPanelOffset += 1f;
                            powerUp.IncreaseDuration();
                            powerUp.SetProgress();
                            powerUp.weaponPanelBar.SetActive(true);
                            DestroyObject(collidedTarget.gameObject);
                            //*powerUp.weaponPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-80f, -38.4f + 102.4f * PowerUpManger.weaponPanelOffset);

                            break;
                    }

                    break;
                }
            }
        }
    }


    void OnTriggerStay2D(Collider2D collidedTarget)
    {
        if (collidedTarget.gameObject.tag=="PowerUp"|| PlayerShipDestructible.isPlayerShipInvincible || GameController.instance.isGameOver)
        {
            return; // Ship is invincible so do nothing
        }
        else
        {
            foreach (GameObject enemy in enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeList)
            {
                int index = enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeList.IndexOf(enemy);
                if (collidedTarget.gameObject.name.Contains(enemyTypeManger.GetComponent<EnemyTypeManger>().enemyCloneName[index]))
                {
                    float damage = enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeDamageOnPlayerSpaceShip[index];
                    gameObject.GetComponent<PlayerShipDestructible>().DecreaseHealth(damage);
                    if (collidedTarget.gameObject.tag == "EP")/*EP=EnemyProjectiles*/
                    {
                        Instantiate(collidedTarget.gameObject.GetComponent<ProjectileController>().hiteffect, new Vector3(collidedTarget.gameObject.transform.position.x, collidedTarget.gameObject.transform.position.y, -0.01f), Quaternion.identity);
                        Destroy(collidedTarget.gameObject);
                    }

                    break;
                }
            }
        }
    }


}
