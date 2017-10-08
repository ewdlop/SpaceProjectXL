using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destructibles : MonoBehaviour {

    public float health;
    public int index;
    public GameObject enemyTypeManger;
    public GameObject weaponManger;
    public GameObject healthSlider;
    public GameObject targetSprite;
    public bool isItTakesDamage;

    public float currentHealth;
    public float maxHealth;
    private Renderer renderer;
    private Color originalColor;
    enum MeteorSize
    {
        Large,
        Medium,
        Small
    }

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        enemyTypeManger = EnemyTypeManger.getEnemyTypeManger();
        
        foreach (GameObject enemy in enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeList)
        {
            index = enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeList.IndexOf(enemy);

            if (gameObject.name.Contains(enemyTypeManger.GetComponent<EnemyTypeManger>().enemyCloneName[index]))
            {
                health = enemyTypeManger.GetComponent<EnemyTypeManger>().health[index];
                isItTakesDamage = enemyTypeManger.GetComponent<EnemyTypeManger>().isItTakesDamage[index];
                break;
            }

        }
        currentHealth = health;
        maxHealth = health;
    }
    
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
            // Play rock breaking sound
            if (this.gameObject.tag == "RockLarge")
                SoundController.Play((int)SFX.RockBreakLarge);
                
            else if (this.gameObject.tag == "RockMedium")
                SoundController.Play((int)SFX.RockBreakMedium);
            else
                SoundController.Play((int)SFX.RockBreakSmall);
            //SoundController.PlayWithOutInterrpution((int)SFX.MONEY);

            GameController.playerScore += enemyTypeManger.GetComponent<EnemyTypeManger>().scoreFromDestroying[index];

            if (enemyTypeManger.GetComponent<EnemyTypeManger>().spawnEnemyAfterDeath[index] != null)
            {
                string parentSpawnerNames;
                if (gameObject.GetComponent<ProjectileController>() != null)
                {
                    parentSpawnerNames = gameObject.GetComponent<ProjectileController>().names;
                }
                else
                {
                    parentSpawnerNames = "";
                }
                switch (parentSpawnerNames)
                {
                    case "Boss1BlueBall(Clone)"://In projectilesController, the ball loses health and dies after (duration-1) seconds.
                        int numberOfSpawnBlueBall = enemyTypeManger.GetComponent<EnemyTypeManger>().numberOfSpawn[index];
                        if (gameObject.transform.localScale.x > 0.124f)/**making sure the blueballs decays 4(Log_0.5(0.125)+1 times), else gg FPS*/
                        {
                            for (int i = 0; i < numberOfSpawnBlueBall; i++)
                            {
                                GameObject clone = Instantiate(enemyTypeManger.GetComponent<EnemyTypeManger>().spawnEnemyAfterDeath[index], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                                clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos((360f / numberOfSpawnBlueBall * i) * Mathf.PI / 180), Mathf.Sin((360f / numberOfSpawnBlueBall * i) * Mathf.PI / 180));
                                clone.transform.localScale = new Vector3(gameObject.transform.localScale.x/2,gameObject.transform.localScale.y/2,1f);
                            }
                        }
                        break;
                    default:
                        float randomAngle = Random.Range(0f, 120f);
                        float randomSpeed = Random.Range(1f, 5f);
                        int numberOfSpawn = enemyTypeManger.GetComponent<EnemyTypeManger>().numberOfSpawn[index];
                        for (int i = 0; i < numberOfSpawn; i++)
                        {
                            GameObject clone = Instantiate(enemyTypeManger.GetComponent<EnemyTypeManger>().spawnEnemyAfterDeath[index], new Vector3(transform.position.x + 1f * Mathf.Cos((360f / numberOfSpawn * i + randomAngle)), transform.position.y + 1f * Mathf.Sin((360f / numberOfSpawn * i + randomAngle)), transform.position.z), Quaternion.identity);
                            clone.GetComponent<Rigidbody2D>().velocity = new Vector2(randomSpeed * Mathf.Cos((360f / numberOfSpawn * i + randomAngle) * Mathf.PI / 180), randomSpeed * Mathf.Sin((360f / numberOfSpawn * i + randomAngle) * Mathf.PI / 180));
                        }
                        break;
                }
            }
            string gameObjectNames = gameObject.name;
            switch (gameObjectNames)
            {
                default:
                    break;
            }
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Ship projectile 
        if (other.tag == "Projectile" &&isItTakesDamage)
        {
            string projectileName=other.gameObject.name;
            switch (projectileName)
            {
                case "LaserBeam":
                    Instantiate(other.gameObject.GetComponent<ProjectileController>().hiteffect, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -0.04f), Quaternion.identity);
                    break;
                case "ChasingMissiles(Clone)":
                    Destroy(other.gameObject.GetComponent<ProjectileController>().targetSprite);
                    Destroy(other.gameObject);

                    break;
                default:

                    Destroy(other.gameObject);
                    break;
            }
            
            int weaponIndex = other.gameObject.GetComponent<ProjectileController>().index;
            float damage = WeaponManager.playerWeaponList[weaponIndex].damage;
            
            string names = gameObject.name;



            switch (names)
            {
                case "Boss1itSelf":
                    float healthPercentage = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);
                    healthSlider.GetComponent<Slider>().value = healthPercentage;
                    currentHealth -= damage;
                    health = currentHealth;
                    renderer.material.SetFloat("_OcclusionStrength",1f-healthPercentage);

                    break;
                case "Boss2itSelf":
                    float healthPercentageBoss2 = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);
                    healthSlider.GetComponent<Slider>().value = healthPercentageBoss2;
                    currentHealth -= damage;
                    health = currentHealth;
                    renderer.material.SetFloat("_OcclusionStrength", 1f - healthPercentageBoss2);
                    break;

                case "greyMeteorLarge1(Clone)":
                    float healthPercentageRockLarge = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);
                    currentHealth -= damage;
                    health = currentHealth;
                    renderer.material.SetFloat("_OcclusionStrength", 1f - healthPercentageRockLarge);
                    break;
                case "greyMeteorMed1(Clone)":
                    float healthPercentageRockMedium = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);
                    currentHealth -= damage;
                    health = currentHealth;
                    renderer.material.SetFloat("_OcclusionStrength", 1f - healthPercentageRockMedium);
                    break;
                default:
                    float healthPercentageDefault= Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);
                    currentHealth -= damage;
                    health = currentHealth;
                    break;
            }
            GameController.playerScore += enemyTypeManger.GetComponent<EnemyTypeManger>().scoreFromHitting[index];
            if(projectileName!= "LaserBeam")
            Instantiate(other.gameObject.GetComponent<ProjectileController>().hiteffect, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -0.01f),Quaternion.identity);
        }

    }
     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            StartCoroutine("HitFlash");
        }

        if (other.tag == "Shield"  /*&& gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero &&*/)
        {

            if ((gameObject.tag != "RockSmall" && gameObject.tag != "RockMedium" && gameObject.tag != "RockLarge" && gameObject.tag != "Boss"&&!gameObject.name.Contains("RedBall") && !gameObject.name.Contains("Boss1BlueBall")))
            {

                float playerShipToCollidePointX = gameObject.transform.position.x - other.gameObject.transform.position.x;
                float playerShipToCollidePointY = gameObject.transform.position.y - other.gameObject.transform.position.y;
                float normalAngleinRad = Mathf.Atan2(playerShipToCollidePointY, playerShipToCollidePointX);

                //float playerShipFacingAngleingRad = (other.gameObject.transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
                float shieldFacingAngleingRad = (other.gameObject.transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
                //Vector2 playerShipFacingUnitVector = new Vector2(Mathf.Cos(playerShipFacingAngleingRad), Mathf.Sin(playerShipFacingAngleingRad));
                Vector2 shieldFacingUnitVector = new Vector2(Mathf.Cos(shieldFacingAngleingRad), Mathf.Sin(shieldFacingAngleingRad));
                Vector2 normalUnitVector = new Vector2(Mathf.Cos(normalAngleinRad), Mathf.Sin(normalAngleinRad));
                float cosineofCollideAngleRelativetoPlayShipFacing = Vector2.Dot(shieldFacingUnitVector, normalUnitVector);
                if (cosineofCollideAngleRelativetoPlayShipFacing > (-90f - 56f) / 180f)
                {
                    gameObject.gameObject.tag = "Projectile";
                    gameObject.gameObject.layer = LayerMask.NameToLayer("Reflected");
                    float cosineOfincidentAngleRelativetoNormal = Vector2.Dot(normalUnitVector, gameObject.GetComponent<Rigidbody2D>().velocity);
                    Vector2 gameObjectVelocityVector = gameObject.GetComponent<Rigidbody2D>().velocity - 2 * (cosineOfincidentAngleRelativetoNormal) * normalUnitVector;
                    float gameObjectVelocityVectorX = gameObjectVelocityVector.x;
                    float gameObjectVelocityVectorY = gameObjectVelocityVector.y;
                    Vector2 finalgameObjectVelocity = new Vector2(gameObjectVelocityVectorX, gameObjectVelocityVectorY);
                    gameObject.GetComponent<Rigidbody2D>().velocity = finalgameObjectVelocity;
                }
                
            }
        }
        
    }

    IEnumerator HitFlash()
    {
        this.renderer.material.color = GameController.instance.hitColor;
        yield return new WaitForSeconds(0.05f);
        this.renderer.material.color = originalColor;
    }
}
