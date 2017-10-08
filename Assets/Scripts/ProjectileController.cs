using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    public float totalDamage;
    public float speed;
    public float duration;

    public float time;
    /*public float playerShipFiredAngleRad;*/
    public int index;

    public bool isFireFromRight;
    public GameObject hiteffect;
    public GameObject weaponManger;
    public GameObject playerSpaceShip;
    public GameObject targetSprite;
    public float playerSpaceShipX;
    public float playerSpaceShipY;
    public string names;

    void Start()
    {
        time = 1/60f;
        playerSpaceShipX = transform.position.x;
        playerSpaceShipY = transform.position.y;
        names = gameObject.name;
        /*
        foreach (GameObject weapon in weaponManger.GetComponent<WeaponManager>().weaponPrefab)
        {
            int indexOfWeapon = weaponManger.GetComponent<WeaponManager>().weaponPrefab.IndexOf(weapon);

            if (gameObject.name.Contains(WeaponManager.playerWeaponList[indexOfWeapon].name))
            {
                index = indexOfWeapon;
                speed = WeaponManager.playerWeaponList[indexOfWeapon].speed;
                names = WeaponManager.playerWeaponList[indexOfWeapon].name;
                break;
            }
        } 
        */   
    }


    void Update()
    {
        float velocityAngle = 0f;
        switch (names)
        {

            case "SineMissiles(Clone)":
                time += Time.deltaTime;
                float speedX = 10f;
                float speedY = speedX * 3 * Mathf.Cos(speedX * time);
                if (isFireFromRight)
                {
                    speedY *= -1;
                }
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speedX * Mathf.Cos(Mathf.PI / 2) - speedY * Mathf.Sin(Mathf.PI / 2), speedX * Mathf.Sin(Mathf.PI / 2) + speedY * Mathf.Cos(Mathf.PI / 2));
                velocityAngle = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, gameObject.GetComponent<Rigidbody2D>().velocity.x);
                transform.eulerAngles = new Vector3(0f, 0f, velocityAngle * Mathf.Rad2Deg - 180f);
                break;

            case "CircleMissiles(Clone)":
                time += Time.deltaTime;
                float angularSpeed = 5f;
                float phase = -1 * Mathf.PI / 2f;
                if (isFireFromRight)
                {
                    phase *= -1;
                }
                float speedX2 = -1 * angularSpeed * 2 * Mathf.Sin(angularSpeed * time + phase + Mathf.PI / 2);
                float speedY2 = angularSpeed * 2 * Mathf.Cos(angularSpeed * time + phase + Mathf.PI / 2);
                float positionX = playerSpaceShipX + 10 * time * Mathf.Cos(angularSpeed * time + phase + Mathf.PI / 2);
                float positionY = playerSpaceShipY + 10 * time * Mathf.Sin(angularSpeed * time + phase + Mathf.PI / 2);
                gameObject.transform.position = new Vector2(positionX, positionY);

                velocityAngle = Mathf.Atan2(speedY2, speedX2);
                transform.eulerAngles = new Vector3(0f, 0f, velocityAngle * Mathf.Rad2Deg - 180f);
                break;
            case "ChasingMissiles(Clone)":
                if (playerSpaceShip == null)//playerSpaceShip is the target
                {
                    Destructibles[] enemies = FindObjectsOfType<Destructibles>();
                    if (enemies.Length>0) {
                        int random = Random.Range(0, enemies.Length);
                        int count=0;
                        while (enemies[random].isItTakesDamage == false&&count<enemies.Length)//need to fix this later
                        {
                            random = Random.Range(0, enemies.Length);
                            count++;
                        }
                        if (targetSprite != null)
                        {
                            Destroy(targetSprite);
                        }
                        playerSpaceShip = enemies[random].gameObject;
                        targetSprite = Instantiate(enemies[random].targetSprite, playerSpaceShip.transform.position,Quaternion.identity);
                        targetSprite.transform.parent= enemies[random].gameObject.transform;
                        targetSprite.transform.localScale = enemies[random].targetSprite.transform.localScale;
                        targetSprite.SetActive(true);
                    }
                    else
                    {
                        DestroyObject(gameObject);
                        DestroyObject(gameObject.GetComponent<ProjectileController>().targetSprite);
                    }

                    
                }
                else
                {

                    float deltaXChasingMissiles = playerSpaceShip.transform.position.x - gameObject.transform.position.x;
                    float deltaYChasingMissiles = playerSpaceShip.transform.position.y - gameObject.transform.position.y;
                    float angleChasingMissiles = Mathf.Atan2(deltaYChasingMissiles, deltaXChasingMissiles);
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(10 * Mathf.Cos(angleChasingMissiles), 10 * Mathf.Sin(angleChasingMissiles));
                    gameObject.transform.eulerAngles = new Vector3(0f, 0f, angleChasingMissiles * Mathf.Rad2Deg - 90f);
                    if (targetSprite != null)
                    { 
                        targetSprite.transform.eulerAngles += new Vector3(0f, 0f,360*Time.deltaTime);
                        
                    }
                }
                break;
            case "Boss1RedBall(Clone)":
                time += Time.deltaTime;
                float x = 0.5f*time * Mathf.Cos(time);
                float y = 0.5f*time * Mathf.Sin(time);
                float norm = Mathf.Sqrt(Mathf.Pow(x, 2) + (Mathf.Pow(y, 2)));
                //Vector2 unitVector = 1f / norm * new Vector2(x, y);
                float redBallPosX = (1f+Mathf.Cos(10*time) / norm) * x;
                float redBallPosY = (1f+Mathf.Cos(10*time) / norm) * y;
                gameObject.transform.position = new Vector3(playerSpaceShipX+redBallPosX, playerSpaceShipY+ redBallPosY,-0.01f);

                //float facingX =-10*Mathf.Cos(time)*Mathf.Sin(10*time)-Mathf.Cos(10*time)*Mathf.Sin(time)-time*Mathf.Sin(time)+Mathf.Cos(time);
                //float facingY =-10*Mathf.Sin(time)*Mathf.Sin(10*time)+Mathf.Cos(10*time)*Mathf.Cos(time)+time*Mathf.Cos(time)+Mathf.Sin(time);
                //float facingAngle = Mathf.Atan2(facingY, facingX);
                //transform.eulerAngles = new Vector3(0f, 0f, facingAngle * Mathf.Rad2Deg -180f);
                break;
            case "Boss1BlueBall(Clone)":
                gameObject.GetComponent<Destructibles>().currentHealth -= gameObject.GetComponent<Destructibles>().maxHealth / (gameObject.GetComponent<ProjectileController>().duration-1f) * Time.deltaTime;
                break;
            case "RedRocketMissiles(Clone)":
                time += Time.deltaTime;
                if (time <= 0.3f)
                    if(isFireFromRight)
                        gameObject.transform.position += new Vector3(Time.deltaTime*10, 0f, 0f);
                    else
                    gameObject.transform.position-=new Vector3(Time.deltaTime*10,0f,0f);
                else
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 0.5f), ForceMode2D.Impulse);
                break;
            case "Boss2BrownBall(Clone)":
                float deltaXBrownBall = playerSpaceShip.transform.position.x - gameObject.transform.position.x;
                float deltaYBrownBall = playerSpaceShip.transform.position.y - gameObject.transform.position.y;
                float angleBrownBall = Mathf.Atan2(deltaYBrownBall, deltaXBrownBall);
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(3* Mathf.Cos(angleBrownBall), 3* Mathf.Sin(angleBrownBall));
                break;
            case "Boss2GreenMissile(Clone)":

                if (time >= 1f)
                {

                    time = 0f;
                    List<int> randomList= new List<int>();
                    randomList.Add(1);
                    randomList.Add(-1);
                    int random = randomList[Random.Range(0,2)];
                    float facingAngleRad = (gameObject.transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
                    gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity.magnitude*new Vector2(Mathf.Cos(facingAngleRad+random*Mathf.PI/2),Mathf.Sin(facingAngleRad+random* Mathf.PI /2));
                    gameObject.transform.eulerAngles=new Vector3(0f, 0f, gameObject.transform.eulerAngles.z+random *90f);
                }

                time += Time.deltaTime;
                
                break;
            case "LaserBeam":
                break;
            default:
                velocityAngle = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, gameObject.GetComponent<Rigidbody2D>().velocity.x);
                transform.eulerAngles = new Vector3(0f, 0f, velocityAngle * Mathf.Rad2Deg - 90f);
                break;
        }
    }

    IEnumerator WaitSForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }

}
