using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonShipBackground : EnemyWeapon{

    public GameObject enemyShip;

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        Instantiate(enemyShip.gameObject,
               new Vector3(Random.Range(-10f,10f), 7.9f, 2f),
               Quaternion.identity);
    }

    public override void Kinematics() { }
}
