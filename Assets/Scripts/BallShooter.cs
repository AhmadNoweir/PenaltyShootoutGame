using UnityEngine;
using UnityEngine.UI;
public class BallShooter : MonoBehaviour
{
    public Rigidbody ballRb;
    public float minForce = 0.1f;
    public float maxForce = 20f;
    public float maxChargeTime = 2f;
    public Slider powerSlider;
    private bool hasShot = false;
    private bool isCharging = false;
    private float chargeTime = 0f;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = ballRb.transform.position;
        if (powerSlider != null)
            powerSlider.value = 0;
    }
    void Update()
    {
        if (hasShot) return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                StartCharging();
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                Charging();
            if (touch.phase == TouchPhase.Ended)
                ShootBall(touch.position);
        }
        if (Input.GetMouseButtonDown(0))
            StartCharging();
        if (Input.GetMouseButton(0))
            Charging();
        if (Input.GetMouseButtonUp(0))
            ShootBall(Input.mousePosition);
    }
    void StartCharging()
    {
        isCharging = true;
        chargeTime = 0f;
    }
    void Charging()
    {
        if (!isCharging) return;
        chargeTime += Time.deltaTime;
        chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
        float powerPercent = chargeTime / maxChargeTime;
        if (powerSlider != null)
            powerSlider.value = powerPercent;
    }
    void ShootBall(Vector2 screenPosition)
    {
        hasShot = true;
        isCharging = false;
        float powerPercent = chargeTime / maxChargeTime;
        float power = Mathf.Lerp(minForce, maxForce, powerPercent);
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        Vector3 target;
        if (Physics.Raycast(ray, out hit))
            target = hit.point;
        else
            target = ray.GetPoint(15f);
        Vector3 direction = (target - ballRb.position);
        direction.y = Mathf.Clamp(direction.y, 0.2f, 0.6f);
        direction.Normalize();
        ballRb.linearVelocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        ballRb.AddForce(direction * power, ForceMode.Impulse);
        if (powerSlider != null)
            powerSlider.value = 0;
        Goalkeeper gk = FindFirstObjectByType<Goalkeeper>();
        if (gk != null)
            gk.Dive();
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
            gm.RegisterShot();
    }
    public void ResetBall()
    {
        ballRb.linearVelocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        ballRb.transform.position = startPosition;
        hasShot = false;
        chargeTime = 0f;
        if (powerSlider != null)
            powerSlider.value = 0;
    }
}