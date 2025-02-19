using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScatteredShot : Weapon{

    public GameObject smallBlueBall;
    public int numberOfScatteredBlueBalls;
    public int subScatter;
    public float shrinkingFactor;
    public float speedUpFactor;
    public float smallBlueBallSpeed;
    public float firstScatteredTime;
    public float subScatterTime;
    private float timer;

    void Start()
    {
        timer = 0;
    }

    void Update()
    {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject Shot = Instantiate(this.gameObject,
               new Vector3(ship.position.x, ship.position.y, -2f),
               Quaternion.identity);
        Shot.GetComponent<Rigidbody2D>().linearVelocity = speed * new Vector2(Mathf.Cos((ship.transform.eulerAngles.z - 90f) * Mathf.Deg2Rad),
                Mathf.Sin((ship.transform.eulerAngles.z - 90f) * Mathf.Deg2Rad));
        //SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
    }

    public override void Kinematics()
    {
        timer += Time.deltaTime;
        if (timer > firstScatteredTime)
        {
            for (int i = 0; i < numberOfScatteredBlueBalls; i++)
            {
                GameObject blueBalls = Instantiate(smallBlueBall, gameObject.transform.position, Quaternion.identity);
                blueBalls.GetComponent<Rigidbody2D>().linearVelocity = smallBlueBallSpeed * new Vector2(
                    Mathf.Cos(2 * Mathf.PI / numberOfScatteredBlueBalls * i),
                    Mathf.Sin(2 * Mathf.PI / numberOfScatteredBlueBalls * i)
                    );
                blueBalls.GetComponent<EnemyScatteredShotBlueBall>().smallBlueBallSpeed = smallBlueBallSpeed;
                blueBalls.GetComponent<EnemyScatteredShotBlueBall>().subScatterTime = subScatterTime;
                blueBalls.GetComponent<EnemyScatteredShotBlueBall>().subScatter = subScatter;
                blueBalls.GetComponent<EnemyScatteredShotBlueBall>().shrinkingFactor = shrinkingFactor;
                blueBalls.GetComponent<EnemyScatteredShotBlueBall>().numberOfBlueBalls = numberOfScatteredBlueBalls;
                blueBalls.GetComponent<EnemyScatteredShotBlueBall>().speedUpFactor = speedUpFactor;
            }
            Destroy(gameObject);
        }
    }
}
