using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichochetShot : Weapon {

    Vector2 velocity;
    public override void Kinematics()
    {
    }
    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject emitter = Instantiate(gameObject, ship.position + new Vector3(0f, 0.5f, 0f),Quaternion.identity);
        float randomAngle = Random.Range(0f,180f);
        emitter.GetComponent<Rigidbody2D>().linearVelocity = speed * 
            new Vector2(Mathf.Cos(randomAngle * Mathf.PI/180f), 
                        Mathf.Sin(randomAngle * Mathf.PI/180f));
        emitter.GetComponent<RichochetShot>().velocity = emitter.GetComponent<Rigidbody2D>().linearVelocity;
        Destroy(emitter, 4f);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "BoxBound")
        {
            SoundController.Play((int)SFX.Reflect, 0.15f);

            //i am so lazy
            if (collision.gameObject.name == "Left" ||
                collision.gameObject.name == "Right")
            {
                GetComponent<Rigidbody2D>().linearVelocity = new Vector2(velocity.x * -1f, velocity.y);
                velocity = GetComponent<Rigidbody2D>().linearVelocity;
            }

            if (collision.gameObject.name == "Top" ||
                collision.gameObject.name == "Bottom")
            {
                GetComponent<Rigidbody2D>().linearVelocity = new Vector2(velocity.x, velocity.y * -1f);
                velocity = GetComponent<Rigidbody2D>().linearVelocity;
            }
        }
    }
}
