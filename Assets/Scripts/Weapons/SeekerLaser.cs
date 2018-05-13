using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerLaser : Weapon {

    public Transform playerShip;
    public Enemy[] enemies;
    public GameObject target;
    public GameObject projectile;

    void Update()
    {
        Kinematics();
    }

    public override void Kinematics()
    {
        if (playerShip.gameObject.GetComponent<PlayerController>().IsInvincible || playerShip == null)
        {
            DestroyObject(gameObject);
        }
        else
        {
            enemies = FindObjectsOfType<Enemy>();
            if (target == null)
            {
                if (enemies.Length > 0)
                {
                    int random = UnityEngine.Random.Range(0, enemies.Length);
                    target = enemies[random].gameObject;
                    LineRenderer lineRenderer = GetComponent<LineRenderer>();
                    Vector3[] points = { playerShip.position, target.transform.position };
                    lineRenderer.SetPositions(points);
                }
                else
                {
                    LineRenderer lineRenderer = GetComponent<LineRenderer>();
                    Vector3[] points = { playerShip.position, playerShip.position };
                    lineRenderer.SetPositions(points);

                }
            }
            else
            {
                LineRenderer lineRenderer = GetComponent<LineRenderer>();
                if (playerShip != null)
                {
                    Vector3[] points = { playerShip.position, target.transform.position };
                    lineRenderer.SetPositions(points);
                    GameObject effect = Instantiate(projectile, target.transform.position, Quaternion.identity);
                    DestroyObject(effect, 0.1f);
                }
                else
                {
                    DestroyObject(gameObject);
                }
            }
        }
       
    }
    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject laser = Instantiate(gameObject, ship.transform.position, Quaternion.identity);
        laser.GetComponent<SeekerLaser>().playerShip = ship;
        LineRenderer lineRenderer = laser.GetComponent<LineRenderer>();
        Vector3[] points = { ship.position, ship.position };
        lineRenderer.SetPositions(points);
        DestroyObject(laser, cooldown);
    }

}
