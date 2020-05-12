using UnityEngine;

public class TrailLaser : Weapon {

    private bool isFiredFromRight;
    private GameObject target;
    private Enemy[] enemies;
    void Start()
    {
        float launchAngletoRad;
        if (isFiredFromRight)
            launchAngletoRad = Random.Range(-90, 0) * Mathf.Deg2Rad;
        else
            launchAngletoRad = Random.Range(-180, -90) * Mathf.Deg2Rad;
        Vector2 relativeVelocity =
            speed * new Vector2(Mathf.Cos(launchAngletoRad),
            Mathf.Sin(launchAngletoRad));
        gameObject.GetComponent<Rigidbody2D>().velocity = relativeVelocity;
    }

    void Update()
    {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        Instantiate(gameObject, leftFire.position,
               leftFire.rotation);
        GameObject rightProjectile;
        rightProjectile = Instantiate(gameObject, rightFire.position,
                 rightFire.rotation);
        rightProjectile.GetComponent<TrailLaser>().isFiredFromRight = true;
    }

    public override void Kinematics()
    {
        if (target == null)
        {
            enemies = FindObjectsOfType<Enemy>();
            if (enemies.Length > 0)
            {
                int random = Random.Range(0, enemies.Length);
                target = enemies[random].gameObject;
            }
            else
            {

            }
        }
        else
        {
            float deltaX = target.transform.position.x - gameObject.transform.position.x;
            float deltaY = target.transform.position.y - gameObject.transform.position.y;
            float angleRad = Mathf.Atan2(deltaY, deltaX);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.5f* Mathf.Cos(angleRad), 0.5f * Mathf.Sin(angleRad)),ForceMode2D.Impulse);
          
        }
    }
}
