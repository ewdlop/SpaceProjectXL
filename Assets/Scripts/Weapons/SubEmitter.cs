using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEmitter : Weapon {

    [Header("SubEmitter")]
    public float angularSpeed;
    public float emittingCoolDown;
    public float lifeTime;
    public int finalEmission;
    public GameObject emission;
    public GameObject ship;
    public float launchAngletoRad;
    void Start()
    {
        Kinematics();
        InvokeRepeating("Emit",0f, emittingCoolDown);
        StartCoroutine(Explode());
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject Emitter = Instantiate(gameObject, ship.position,Quaternion.identity);
        Emitter.GetComponent<SubEmitter>().launchAngletoRad = ship.gameObject.GetComponent<PlayerController>().cannonAngle * Mathf.Deg2Rad;
    }

    public override void Kinematics()
    {
        Vector2 relativeVelocity =
            speed * new Vector2(Mathf.Cos(launchAngletoRad),
            Mathf.Sin(launchAngletoRad));

        GetComponent<Rigidbody2D>().velocity = relativeVelocity;
        GetComponent<Rigidbody2D>().angularVelocity = angularSpeed;

    }

    public void Emit()
    {
        GameObject clone=Instantiate(emission, gameObject.transform.position,Quaternion.identity);
        clone.GetComponent<Weapon>().launchAngle = Random.Range(0f, 360f);
        Destroy(clone, 3);
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(lifeTime);
        for (int i = 0; i < finalEmission; i++)
        {
            GameObject clone = Instantiate(emission, 
                this.gameObject.transform.position+2*new Vector3(Mathf.Cos(i*2*Mathf.PI/finalEmission), Mathf.Sin(i * 2 * Mathf.PI / finalEmission), 0f),
                Quaternion.identity);
            clone.GetComponent<Weapon>().launchAngle = i * 360f / finalEmission+135f;
            Destroy(clone, 3);
        }
        DestroyObject(this.gameObject);
    }
}
