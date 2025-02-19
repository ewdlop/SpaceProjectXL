using UnityEngine;

public class TrailLaser : Weapon
{

    private bool isFiredFromRight;
    private GameObject target;
    private Enemy[] enemies;
    void Start() {
        float launchAngletoRad;
        float random = Random.Range(-90, 0) * Mathf.Deg2Rad;
        if (isFiredFromRight)
            launchAngletoRad = random;
        else
            launchAngletoRad = Mathf.PI - random;
        gameObject.GetComponent<Rigidbody2D>().linearVelocity = speed * new Vector2(Mathf.Cos(launchAngletoRad),
            Mathf.Sin(launchAngletoRad));
    }

    void Update() {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire) {
        Instantiate(gameObject, leftFire.position,
               leftFire.rotation);
        GameObject rightProjectile = Instantiate(gameObject,
                                      rightFire.position,
                                      rightFire.rotation);
        rightProjectile.GetComponent<TrailLaser>().isFiredFromRight = true;
    }

    public override void Kinematics() {
        if (target == null) {
            enemies = FindObjectsOfType<Enemy>();
            if (enemies.Length > 0) {
                int random = Random.Range(0, enemies.Length);
                target = enemies[random].gameObject;
            }
        } else {
            float velocityRadian = Mathf.Atan2(gameObject.GetComponent<Rigidbody2D>().linearVelocity.y, gameObject.GetComponent<Rigidbody2D>().linearVelocity.x);
            Vector3 targetDisplacement = target.transform.position - gameObject.transform.position;
            float currentSpeed = gameObject.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
            float cosine = Vector3.Dot(targetDisplacement, gameObject.GetComponent<Rigidbody2D>().linearVelocity) / (targetDisplacement.magnitude * currentSpeed);
            float displacmentArcRad = Mathf.Acos(cosine);//[0,Pi]
            float maxTurnRad = Mathf.Min(displacmentArcRad, Mathf.PI * 0.95f);
            if (displacmentArcRad < 0.1f) {
                gameObject.GetComponent<Rigidbody2D>().AddForce(speed * 3f * new Vector2(Mathf.Cos(velocityRadian), Mathf.Sin(velocityRadian)), ForceMode2D.Impulse);
                return;
            }

            if (isFiredFromRight) {
                    Vector2 deltaVelocity = new Vector2(
    speed * (Mathf.Cos(velocityRadian + 5f * maxTurnRad * Time.deltaTime) - Mathf.Cos(velocityRadian)),
    speed * (Mathf.Sin(velocityRadian + 5f * maxTurnRad * Time.deltaTime) - Mathf.Sin(velocityRadian)));
                    gameObject.GetComponent<Rigidbody2D>().AddForce(deltaVelocity, ForceMode2D.Impulse);
            } else {
                Vector2 deltaVelocity = new Vector2(
                   speed * (Mathf.Cos(velocityRadian - 5f * maxTurnRad * Time.deltaTime) - Mathf.Cos(velocityRadian)),
                   speed * (Mathf.Sin(velocityRadian - 5f * maxTurnRad * Time.deltaTime) - Mathf.Sin(velocityRadian)));
                gameObject.GetComponent<Rigidbody2D>().AddForce(deltaVelocity, ForceMode2D.Impulse);
            }
        }
    }
}
