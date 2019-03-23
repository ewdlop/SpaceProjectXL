using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerLaser : Weapon {

    public Enemy[] enemies;
    public GameObject target;
    public GameObject projectile;
    public float laserBoxSpeed;
    public int numberOfLaserPerBall;
    public bool isPrefab;
    void Update()
    {
        if(!isPrefab)
            Kinematics();
    }

    public override void Kinematics()
    {
        if (target == null)
        {
            enemies = FindObjectsOfType<Enemy>();
            if (enemies.Length > 0)
            {
                int random = UnityEngine.Random.Range(0, enemies.Length);
                target = enemies[random].gameObject;
                LineRenderer lineRenderer = GetComponent<LineRenderer>();
                Vector3[] points = { gameObject.transform.position, target.transform.position };
                lineRenderer.SetPositions(points);
            }
            else
            {
                LineRenderer lineRenderer = GetComponent<LineRenderer>();
                Vector3[] points = { gameObject.transform.position, gameObject.transform.position };
                lineRenderer.SetPositions(points);
            }
        }
        else
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            Vector3[] points = { gameObject.transform.position, target.transform.position };
            lineRenderer.SetPositions(points);
            GameObject effect = Instantiate(projectile, target.transform.position, Quaternion.identity);
            DestroyObject(effect, 0.1f);
        }
    }
    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject laser = Instantiate(gameObject, ship.position, Quaternion.identity);
        laser.GetComponent<SeekerLaser>().isPrefab = false;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, laserBoxSpeed);
        DestroyObject(laser, 2f);
    }

}
