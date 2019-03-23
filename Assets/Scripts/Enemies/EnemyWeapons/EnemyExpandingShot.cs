using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExpandingShot : Weapon{

    public GameObject smallGreenBall;
    public int number;
    public float greenBallSpeed;
    public float expandingFactor;
    public float period;
    private float timer;
    private float boomTimer;
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
               new Vector3(ship.position.x, ship.position.y, -0.01f),
               Quaternion.identity);
        Vector2 relativeVelocity = speed * new Vector2(Mathf.Cos((ship.transform.eulerAngles.z + 90f) * Mathf.Deg2Rad),
                Mathf.Sin((ship.transform.eulerAngles.z + 90f) * Mathf.Deg2Rad));
        Shot.GetComponent<Rigidbody2D>().velocity = relativeVelocity;
        //SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
    }

    public override void Kinematics()
    {
        timer += Time.deltaTime;
        boomTimer += Time.deltaTime;
        if (boomTimer > 4)
        {
            for (int i = 0; i < number; i++)
            {
                GameObject greenBalls = Instantiate(smallGreenBall, gameObject.transform.position, Quaternion.identity);
                greenBalls.GetComponent<Rigidbody2D>().velocity = greenBallSpeed * new Vector2(
                    Mathf.Cos(2 * Mathf.PI / number * i),
                    Mathf.Sin(2 * Mathf.PI / number * i)
                    );
            }
            DestroyObject(gameObject);
        }
        else
        {
            if (timer >= period)
            {
                timer = 0;
                transform.localScale = new Vector3(transform.localScale.x * (1f + expandingFactor), transform.localScale.y * (1f + expandingFactor), transform.localScale.z);
                Destroy(GetComponent<CircleCollider2D>());
                if (GetComponentInChildren<Light>() != null)
                {
                    GetComponentInChildren<Light>().range = GetComponentInChildren<Light>().range * (1f + expandingFactor);
                    GetComponentInChildren<Light>().intensity = GetComponentInChildren<Light>().intensity * (1f + expandingFactor);
                }
                CircleCollider2D collider2d = gameObject.AddComponent<CircleCollider2D>();
                collider2d.isTrigger = true;
            }
        }
    }
}
