using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerLaser : Weapon {

    public Enemy[] enemies;
    public GameObject damageEffect;
    public GameObject line;
    public float laserBoxSpeed;
    public bool isPrefab;

    void Update()
    {
        if(!isPrefab)
            Kinematics();
    }

    public override void Kinematics()
    {
        foreach(Transform child in transform)
        {
            if (child.GetComponent<SeekerLaserProjectile>().target == null)
            {
                enemies = FindObjectsOfType<Enemy>();
                if (enemies.Length > 0)
                {
                    int random = Random.Range(0, enemies.Length);
                    child.GetComponent<SeekerLaserProjectile>().target = enemies[random].gameObject;
                }
                else
                {
                    LineRenderer lineRenderer = child.GetComponent<LineRenderer>();
                    Vector3[] points = { child.transform.position, child.transform.position };
                    lineRenderer.SetPositions(points);
                }
            }
            else
            {
                LineRenderer lineRenderer = child.GetComponent<LineRenderer>();
                Vector3[] points = { child.transform.position, child.GetComponent<SeekerLaserProjectile>().target.transform.position };
                lineRenderer.SetPositions(points);
                GameObject effect = Instantiate(damageEffect, child.GetComponent<SeekerLaserProjectile>().target.transform.position, Quaternion.identity);
                Destroy(effect, 0.1f);
            }
        }
        
    }
    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject laser = Instantiate(gameObject, ship.position, Quaternion.identity);
        Instantiate(line, laser.transform);
        Instantiate(line, laser.transform);
        Instantiate(line, laser.transform);
        Instantiate(line, laser.transform);
        laser.GetComponent<SeekerLaser>().isPrefab = false;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, laserBoxSpeed);
        Destroy(laser, 2f);
    }

}
