using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : Weapon {


    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        ship.GetComponent<PlayerController>().isShootingLaserBeam = true;
        ship.GetComponent<PlayerController>().EnableLaserBeam(true);
    }

    public override void Kinematics()
    {
    }
}
