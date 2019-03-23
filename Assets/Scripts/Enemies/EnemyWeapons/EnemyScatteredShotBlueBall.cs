using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScatteredShotBlueBall : EnemyWeapon
{

    public int subScatter;
    public int numberOfBlueBalls;
    public float smallBlueBallSpeed;
    public float subScatterTime;
    public float shrinkingFactor;
    public float speedUpFactor;
    private float timer;

    void Update()
    {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {

    }

    public override void Kinematics()
    {
        timer += Time.deltaTime;
        if (timer > subScatterTime)
        {
            subScatter--;
            smallBlueBallSpeed *= (1 + speedUpFactor);
            if (subScatter > 0)
            {
                for (int i = 0; i < numberOfBlueBalls; i++)
                {
                    GameObject blueBalls = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
                    blueBalls.GetComponent<Rigidbody2D>().velocity = smallBlueBallSpeed * new Vector2(
                        Mathf.Cos(2 * Mathf.PI / numberOfBlueBalls * i),
                        Mathf.Sin(2 * Mathf.PI / numberOfBlueBalls * i)
                        );
                    blueBalls.transform.localScale = new Vector3(transform.localScale.x * (1f - shrinkingFactor), transform.localScale.y * (1f - shrinkingFactor), transform.localScale.z);
                    Destroy(blueBalls.GetComponent<CircleCollider2D>());
                    CircleCollider2D collider2d = blueBalls.AddComponent<CircleCollider2D>();
                    collider2d.isTrigger = true;
                }
            }
            DestroyObject(gameObject);
        }
    }
}
