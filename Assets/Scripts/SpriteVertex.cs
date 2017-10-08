using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteVertex : MonoBehaviour
{

    public Material wrapMat;
    public Material normalMat;
    public GameObject hole;
    private float time0;
    private float time;
    private bool isHitBlackHole;
    private float r0;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - hole.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.y - hole.transform.position.y, 2));
        if (distance < 8.44f)
        {
            if (!isHitBlackHole)
            {
                gameObject.GetComponent<SpriteRenderer>().material = wrapMat;

                wrapMat.SetFloat("_shipX", gameObject.transform.position.x);
                wrapMat.SetFloat("_shipY", gameObject.transform.position.y);
                wrapMat.SetFloat("_holeX", hole.transform.position.x);
                wrapMat.SetFloat("_holeY", hole.transform.position.y);
                time0 = Mathf.Atan2(gameObject.transform.position.y - hole.transform.position.y, gameObject.transform.position.x - hole.transform.position.x);
                if (time0 < 0)
                {
                    time0 += 2 * Mathf.PI;
                }
                time = time0/5f;
                r0 = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.y - hole.transform.position.y, 2)+ Mathf.Pow(gameObject.transform.position.x - hole.transform.position.x, 2));
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                isHitBlackHole = true;
            }
            else
            {
                if (distance < 0.7)
                {
                    //PlayerShipActions.isKillShip = true;
                    gameObject.GetComponent<SpriteVertex>().enabled = false;
                }
                else
                {
                    wrapMat.SetFloat("_shipX", gameObject.transform.position.x);
                    wrapMat.SetFloat("_shipY", gameObject.transform.position.y);
                    wrapMat.SetFloat("_holeX", hole.transform.position.x);
                    wrapMat.SetFloat("_holeY", hole.transform.position.y);
                    time += Time.deltaTime;
                    float radius = r0 / Mathf.Exp(-0.5f*time0/10)*Mathf.Exp(-0.5f*time);
                    gameObject.transform.position = new Vector2(hole.transform.position.x+radius * Mathf.Cos(5*time), hole.transform.position.y+radius * Mathf.Sin(5*time));
                }
            }
        }
        else {
            gameObject.GetComponent<SpriteRenderer>().material = normalMat;

        }
        

    }
    }
