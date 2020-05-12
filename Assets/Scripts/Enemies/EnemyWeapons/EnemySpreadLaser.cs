using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpreadLaser : EnemyWeapon{

    public int shotsPerWave;
    public float angleSpan;
    public float angleBegin;
    [Header("Spining")]
    public float torque;

    private Vector2 shotDirection;

    void Update()
    {
        //Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        for (int i = 0; i < shotsPerWave; i++)
        {
            GameObject SpreadLaser = Instantiate(this.gameObject,
                new Vector3(ship.position.x, ship.position.y, -1f),
                Quaternion.identity);
            float gap = (angleSpan / (shotsPerWave-1));
            SpreadLaser.GetComponent<EnemySpreadLaser>().shotDirection =
                new Vector2(Mathf.Cos(angleBegin / 180f * Mathf.PI - gap / 180f * Mathf.PI * i),
                Mathf.Sin(angleBegin / 180f * Mathf.PI - gap / 180f * Mathf.PI * i));
            //rotation matrix
            Vector2 direction = SpreadLaser.GetComponent<EnemySpreadLaser>().shotDirection;
            Vector2 relativeVelocity = speed * new Vector2(
            Mathf.Cos((ship.transform.eulerAngles.z - 90f) * Mathf.Deg2Rad ) * direction.x - Mathf.Sin((ship.transform.eulerAngles.z - 90f) * Mathf.Deg2Rad) * direction.y
           ,Mathf.Sin((ship.transform.eulerAngles.z - 90f) * Mathf.Deg2Rad ) * direction.x + Mathf.Cos((ship.transform.eulerAngles.z - 90f) * Mathf.Deg2Rad) * direction.y);
            SpreadLaser.GetComponent<Rigidbody2D>().velocity = relativeVelocity;
            SpreadLaser.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(relativeVelocity.y, relativeVelocity.x) * Mathf.Rad2Deg - 90f);
            //for spining
            SpreadLaser.GetComponent<Rigidbody2D>().AddTorque(torque, ForceMode2D.Impulse);
        }
        SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
    }

    public override void Kinematics()
    {
        
    }
}
