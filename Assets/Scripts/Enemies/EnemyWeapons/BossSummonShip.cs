using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonShip : EnemyWeapon{

    public GameObject enemyShip;
    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        Instantiate(enemyShip.gameObject,
               new Vector3(ship.position.x, ship.position.y, 2f),
               Quaternion.identity);
    }

    public override void Kinematics() { }
}
