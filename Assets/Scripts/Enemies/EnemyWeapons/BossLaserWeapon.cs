using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserWeapon : EnemyWeapon {

    public GameObject laser;
    public ParticleSystem charging;
    public float laserDuration;

    void Start()
    {

    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        charging.Stop();
        SoundController.Play((int)SFX.Boss4LaserFiring);
        laser.SetActive(true);
        Kinematics();
    }

    public override void Kinematics() 
    {
         StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(laserDuration);
        laser.SetActive(false);
        charging.Play();
        SoundController.Play((int)SFX.Boss4LaserCharging);
    }
}
