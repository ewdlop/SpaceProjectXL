using UnityEngine;

public class EmittedLaser : Weapon {

    // DefaultLaser: 
    // Shoots forward at the specified launch angle
    void Start()
    {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {

    }

    public override void Kinematics()
    {
        float launchAngletoRad = this.GetComponent<Weapon>().launchAngle * Mathf.Deg2Rad;
        Vector2 relativeVelocity =
            speed * new Vector2(Mathf.Cos(launchAngletoRad),
            Mathf.Sin(launchAngletoRad));

        this.GetComponent<Rigidbody2D>().linearVelocity = relativeVelocity;
        this.transform.eulerAngles = new Vector3(0f, 0f, this.GetComponent<Weapon>().launchAngle - 90f);
    }
}
