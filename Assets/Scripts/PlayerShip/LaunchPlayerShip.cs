using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchPlayerShip : MonoBehaviour
{

    public float launchAngle;
    public float countdownTimeLeft;
    public Camera mainCam;
    public LineRenderer launchNormalArrow;
    public Text countdownText;
    public static bool isPlayerShipLaunched= false;
    public static bool isPlayerShipDebugMode;
    public Vector3 mouseWorldPosition;
    public GameObject leftEngineFire;
    public GameObject rightEngineFire;

    public bool debugMode;
    
    void Awake()
    {
        if(debugMode == true)
        {
            isPlayerShipDebugMode = true;
            
        }
    }


    // Update is called once per frame
    void Update()

    {
        if (debugMode == false)
        {
            //Countdown before launch
            if (countdownTimeLeft > 0)
            {
                countdownText.text = Mathf.Floor(countdownTimeLeft).ToString();
                countdownTimeLeft -= Time.deltaTime;
                //Get Cursor posistion;
                Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
                //Get Launch Direction;
                launchAngle = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * 180 / Mathf.PI;
                //offset by 90 degree, Spaceship is facing North initially
                transform.eulerAngles = new Vector3(0, 0, launchAngle - 90);
                launchNormalArrow = GetComponent<LineRenderer>();
                //http://answers.unity3d.com/questions/1100566/making-a-arrow-instead-of-linerenderer.html
                launchNormalArrow.widthCurve = new AnimationCurve(
                     new Keyframe(0, 0.4f)
                     , new Keyframe(1, 0f));
                launchNormalArrow.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0));
                launchNormalArrow.SetPosition(1, new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0));
                //it wont draw the normal vector properly rip;
                //Vector3 Offset = new Vector3(MouseWorldPosition.x - transform.position.x, MouseWorldPosition.y - transform.position.y, 0);
                //Vector3 MouseWorldPosition2 = new Vector3((MouseWorldPosition.x - transform.position.x) / Offset.magnitude, (MouseWorldPosition.y - transform.position.y) / Offset.magnitude, 0);
                //LaunchNormal.SetPosition(1, new Vector3(MouseWorldPosition2.x, MouseWorldPosition2.y, 0));
                //Debug.log(MouseWorldPosition2.x+","+MouseWorldPosition2.y);
            }
            else if (countdownTimeLeft < 0)
            {
                Launch();

            }

            if (isPlayerShipLaunched)
            {

                mouseWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
                launchNormalArrow.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0));
                launchNormalArrow.SetPosition(1, new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0));
                //Rigidbody2D rb = GetComponent<Rigidbody2D>();
                //float playerShipFacingAngle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * 180 / Mathf.PI; ;

                //face the ship in the direction of velocity
                //transform.eulerAngles = new Vector3(0, 0, PlayerShip_FacingAngle - 90);

            }

        }
        else
        {
            isPlayerShipLaunched = true;
            countdownText.gameObject.SetActive(false);
            //LaunchNormalArrow = GetComponent<LineRenderer>();
            //LaunchNormalArrow.widthCurve = new AnimationCurve(
                 //new Keyframe(0, 0.4f)
                 //, new Keyframe(1, 0f));
            //LaunchNormalArrow.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0));
            //LaunchNormalArrow.SetPosition(1, new Vector3(MouseWorldPosition.x, MouseWorldPosition.y, 0));
        }
    }
    void Launch()
    {

        isPlayerShipLaunched = true;//for enabling the gravity;
        countdownTimeLeft = 0;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        mouseWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        float boost = Mathf.Sqrt(Mathf.Pow((mouseWorldPosition.y - transform.position.y), 2) + Mathf.Pow((mouseWorldPosition.x - transform.position.x), 2));
        float launchAngleRadian = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x);
        //give it a kick
        rb.AddForce(new Vector2(boost * rb.mass * Mathf.Cos(launchAngleRadian), boost * rb.mass * Mathf.Sin(launchAngleRadian)), ForceMode2D.Impulse);
        leftEngineFire.gameObject.SetActive(false);
        rightEngineFire.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        //LaunchNormalArrow.enabled = false;

    }




    
}
