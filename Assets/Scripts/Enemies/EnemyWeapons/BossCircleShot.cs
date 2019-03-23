using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCircleShot : Weapon{

    public GameObject smallBall;
    public GameObject[] smallBalls;
    public int number;
    public float radius;
    public float rotatingSpeed;
    public float phase;
    public float timer;
    private float radiusInit;

    void Start()
    {
        timer = 0f;
        smallBalls = new GameObject[number];
        for (int i = 0; i < number; i++)
        {
            smallBalls[i] = Instantiate(smallBall, transform);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        Instantiate(this.gameObject,
               ship);
    }

    public override void Kinematics()
    {
       int i = 0;
       foreach(GameObject ball in smallBalls)
       {
            if(ball!= null)
            {
                ball.transform.position = transform.position + radius * new Vector3(
                    Mathf.Cos((rotatingSpeed * timer + phase + i * 360f/ number) * Mathf.PI / 180f),
                    Mathf.Sin((rotatingSpeed * timer + phase + i * 360f / number) * Mathf.PI / 180f),
                    0f);
                i++;
            }

       }
    }
}
