using UnityEngine;

public class Shield : MonoBehaviour {

    [SerializeField]
    private GameObject playerSpaceShip;
    [SerializeField]
    private float eulerAngleY;

    public GameObject PlayerSpaceShip { get => playerSpaceShip; set => playerSpaceShip = value; }
    public float EulerAngleY { get => eulerAngleY; set => eulerAngleY = value; }

    void Update ()
    {
        eulerAngleY += Time.deltaTime * 360f;
        if (!MenuManager.isPaused && PlayerSpaceShip != null) {
            transform.position = PlayerSpaceShip.transform.position;
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float facingAngle = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0f, eulerAngleY, facingAngle - 90f);
        }
    }
}
