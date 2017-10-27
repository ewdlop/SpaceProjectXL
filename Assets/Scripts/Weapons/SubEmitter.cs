using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEmitter : Weapon {

    [Header("SubEmitter")]
    public float emittingCoolDown;
    public float lifeTime;
    public int finalEmission;
    public GameObject emission;

    void Start()
    {
        Kinematics();
        InvokeRepeating("Emit",0f, emittingCoolDown);
        StartCoroutine(Explode());
    }

    void Update()
    {

    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject emitter = Instantiate(this.gameObject, ship.position,Quaternion.identity);
    }

    public override void Kinematics()
    {
        float launchAngletoRad = this.GetComponent<Weapon>().launchAngle * Mathf.Deg2Rad;
        Vector2 relativeVelocity =
            speed * new Vector2(Mathf.Cos(launchAngletoRad),
            Mathf.Sin(launchAngletoRad));

        this.GetComponent<Rigidbody2D>().velocity = relativeVelocity;
    }

    public void Emit()
    {
        GameObject clone=Instantiate(emission, this.gameObject.transform.position,Quaternion.identity);
        clone.GetComponent<Weapon>().launchAngle = Random.Range(0f, 360f);
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
        }
        DestroyObject(this.gameObject);
    }
}
