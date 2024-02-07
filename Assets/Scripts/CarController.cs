using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] float speedMultiplier;
    [SerializeField] float motorForce;
    [SerializeField] float maxSteerAngle;
    [SerializeField] float maxBoost;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private float slowDownSpeed = 0.05f;

    Vector2 movement;
    private float steerAngle;
    private bool isAccelerating;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        HandleMotor();
        //ApplyAcceleration();
        HandleSteering();
    }

    void HandleMotor()
    {

        frontLeftWheelCollider.motorTorque = movement.y * speedMultiplier * motorForce * Time.fixedDeltaTime;
        frontRightWheelCollider.motorTorque = movement.y * speedMultiplier * motorForce * Time.fixedDeltaTime;
        if (movement.y <= 0)
        {
            rb.velocity= Vector3.Lerp(rb.velocity, new Vector3(0, 0, 0), slowDownSpeed );
        }
        // Debug.Log(speedMultiplier);
        
        // if(isAccelerating)
        // {
        //     ApplyAcceleration();
        // }
        Debug.Log(rb.velocity);
    }

    void ApplyAcceleration()
    {
        float accelerationBoost = (isAccelerating ? 500 : 0) * (movement.y > 0 ? 1 : -0.5f) * Time.fixedDeltaTime;
        if(rb.velocity.magnitude < maxBoost && movement.y > 0 && accelerationBoost > 0)
        {
            rb.AddForce(transform.forward * accelerationBoost, ForceMode.Acceleration);
        }
        if (rb.velocity.magnitude < maxBoost * 0.5 && movement.y < 0)
        {
            rb.AddForce(transform.forward * accelerationBoost, ForceMode.Acceleration);
        }
    }

    void HandleSteering()
    {
        steerAngle = maxSteerAngle * movement.x;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    void UpdateWheels()
    {
        UpdateWheel(frontLeftWheelCollider, frontRightWheelTransform);
        UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    void UpdateWheel(WheelCollider wheelCol, Transform wheelMesh)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCol.GetWorldPose(out pos, out rot);

        wheelMesh.position = pos;
        wheelMesh.rotation = rot;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        float f = context.ReadValue<float>();
        isAccelerating = f == 1;
        Debug.Log(isAccelerating);
    }
}
