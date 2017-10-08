using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLaser : Weapon {

    void Update()
    {
        Kinematics();
    }

    public override void Kinematics()
    {
        /*
        float totalSpeed = 0;
        if (shipRB != null)
        {
            totalSpeed = shipRB.velocity.magnitude + weapon.GetComponent<Weapon>().speed;
        }
        else
        {
            totalSpeed = weapon.GetComponent<Weapon>().speed;

        }
        float leftWeaponUnitVectorX = Mathf.Cos(weaponAngletoRad);
        float leftWeaponUnitVecotrY = Mathf.Sin(weaponAngletoRad);

        Vector2 leftLaserRelativeVelocity = totalSpeed * new Vector2(leftWeaponUnitVectorX * Mathf.Cos(Mathf.PI / 2) - leftWeaponUnitVecotrY * Mathf.Sin(Mathf.PI / 2)
            , leftWeaponUnitVectorX * Mathf.Sin(Mathf.PI / 2) + leftWeaponUnitVecotrY * Mathf.Cos(Mathf.PI / 2));
        if (leftProjectile != null)
        {
            leftProjectile.GetComponent<Rigidbody2D>().velocity = leftLaserRelativeVelocity;
        }
        //------------------------------------------------------------------------------//
        float rightWeaponUnitVectorX = Mathf.Cos(-1 * weaponAngletoRad);
        float rightWeaponUnitVecotrY = Mathf.Sin(-1 * weaponAngletoRad);
        Vector2 rightLaserRelativeVelocity = totalSpeed * new Vector2(rightWeaponUnitVectorX * Mathf.Cos(Mathf.PI / 2) - rightWeaponUnitVecotrY * Mathf.Sin(Mathf.PI / 2)
            , rightWeaponUnitVectorX * Mathf.Sin(Mathf.PI / 2) + rightWeaponUnitVecotrY * Mathf.Cos(Mathf.PI / 2));
        if (rightProjectile != null)
        {
            rightProjectile.GetComponent<Rigidbody2D>().velocity = rightLaserRelativeVelocity;
        }
        */
    }
}
