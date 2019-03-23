using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeapon : Weapon
{
    /**
     * The inherited fields from base Weapon class
     * 
        public GameObject hiteffect;
        public int damage;
        public float launchAngle;
        public float speed = 50.0f;
        public bool isUnlocked;

        // Handles where/how the object is instantiated
        public abstract void Shoot(Transform ship, Transform leftFire, Transform rightFire);
        // Handles the projectile's movement
        public abstract void Kinematics();
     *
     **/
    public float fireDelay = 0.3f;  // the delay between each shot
    public float waveDelay = 1.0f;  // the delay between each wave
    protected bool hasCollided = false;


    //public abstract void Shoot(Transform ship, Transform target);

    protected void LateUpdate()
    {
        hasCollided = false;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Shield")
        {
            if (!hasCollided)
            {
                hasCollided = true;
                float playerShipToCollidePointX = transform.position.x - other.gameObject.transform.position.x;
                float playerShipToCollidePointY = transform.position.y - other.gameObject.transform.position.y;
                float normalAngleinRad = Mathf.Atan2(playerShipToCollidePointY, playerShipToCollidePointX);
                float shieldFacingAngleingRad = (other.gameObject.transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
                Vector2 shieldFacingUnitVector = new Vector2(Mathf.Cos(shieldFacingAngleingRad), Mathf.Sin(shieldFacingAngleingRad));
                Vector2 normalUnitVector = new Vector2(Mathf.Cos(normalAngleinRad), Mathf.Sin(normalAngleinRad));
                float cosineofCollideAngleRelativetoPlayShipFacing = Vector2.Dot(shieldFacingUnitVector, normalUnitVector);
                if (cosineofCollideAngleRelativetoPlayShipFacing > (-90f - 56f) / 180f)
                {
                    //Debug.Log("In shield arc");
                    gameObject.tag = "Projectile";
                    gameObject.layer = LayerMask.NameToLayer("Player");
                    isReflected = true;
                    float cosineOfincidentAngleRelativetoNormal = Vector2.Dot(normalUnitVector, gameObject.GetComponent<Rigidbody2D>().velocity);
                    Vector2 gameObjectVelocityVector = gameObject.GetComponent<Rigidbody2D>().velocity - 2 * (cosineOfincidentAngleRelativetoNormal) * normalUnitVector;
                    float gameObjectVelocityVectorX = gameObjectVelocityVector.x;
                    float gameObjectVelocityVectorY = gameObjectVelocityVector.y;
                    Vector2 finalgameObjectVelocity = new Vector2(gameObjectVelocityVectorX, gameObjectVelocityVectorY);
                    gameObject.GetComponent<Rigidbody2D>().velocity = 2f * finalgameObjectVelocity;

                    // Change rotation of the laser to match reflected direction
                    gameObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(gameObjectVelocityVectorY, gameObjectVelocityVectorX) * 180f / Mathf.PI - 90f);
                    // Play the reflect sound
                    SoundController.Play((int)SFX.Reflect, 0.2f);
                }
            }
        }
    }

}
