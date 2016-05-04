using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

    public GameObject ShipBody;
    public GameObject Camera;

    public float ForwardForce = 50f;
    public float terminalVelocity = 15f;
    public float maxAcceleration = 166.5f;
    public float RightCoefficient = 5f;
    public float UpCoefficient = 10f;
    public float PitchForce = 30f;

    public float LiftCoefficient = 1f;
    public float RollSpeed = 40f;

    private Rigidbody shipRigidBody;
    private float yaw = 0f;
    private float v = 0f;
    private float h = 0f;

    // Use this for initialization
    void Start () {
        shipRigidBody = ShipBody.GetComponent<Rigidbody>();
    }


    void Update()
    {
        //Camera orientation
        Vector3 cameraPosition = ShipBody.transform.position;
        Camera.transform.position = cameraPosition;

        Vector3 forward = ShipBody.transform.forward;
        // Zero out the y component of your forward vector to only get the direction in the X,Z plane
        forward.y = 0;
        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        Camera.transform.rotation = Quaternion.AngleAxis(headingAngle, Vector3.up);

        v = Input.GetAxis("Vertical") * 50f * Time.deltaTime;
        h = Input.GetAxis("Horizontal") * 50f * Time.deltaTime;

      
        
        //if (v != 0)
        //   rotationVector += ShipBody.transform.up * -v;


        //Vector3 rotationVectorUp = ShipBody.transform.forward + (ShipBody.transform.up * UpCoefficient * v);
        //rotationVectorUp = rotationVectorUp.normalized;
        //Vector3 rotationVector = new Vector3(v * 30f, ShipBody.transform.eulerAngles.y + yValue, h * 30f);

        Vector3 rollVector = shipRigidBody.transform.forward;
        Vector3 pitchVector = shipRigidBody.transform.right;

        //Quaternion rotation = new Quaternion();
        //rotation = Quaternion.AngleAxis(-60f * h, rollVector);
        //yaw += 30f * h * Time.deltaTime;
        //rotation *= Quaternion.AngleAxis(yaw, Vector3.up);
        //ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, rotation, RollSpeed * Time.deltaTime);
    }

    void FixedUpdate() {
        
        if (shipRigidBody.angularVelocity.magnitude < 1)
        {
            shipRigidBody.AddRelativeTorque(Vector3.right * v * PitchForce * Time.fixedDeltaTime);
        }
        //Debug.Log(shipRigidBody.angularVelocity.magnitude);
       
        shipRigidBody.AddTorque(Vector3.up * h * 15f * Time.fixedDeltaTime);
        shipRigidBody.AddRelativeTorque(Vector3.forward * h * -70f * Time.fixedDeltaTime);

        //Lift force
        Vector3 right = ShipBody.transform.right;
        right.x = 0f;
        float pitchAngle = ShipBody.transform.rotation.eulerAngles.x;
        bool upwardsRotation = false;
        if (pitchAngle > 180f) {
            pitchAngle -= 180f;
            upwardsRotation = true;
        }
        if (pitchAngle > 90f) pitchAngle -= 90f;

        if (upwardsRotation) {
            LiftCoefficient = 1 + (pitchAngle / 90);
        }
        else {
            LiftCoefficient = 1 - (pitchAngle / 90);
        }
        
        Debug.Log(LiftCoefficient);
        shipRigidBody.AddRelativeForce(Vector3.up * Physics.gravity.magnitude * shipRigidBody.mass * LiftCoefficient);

        //Forward force
        Vector3 addedForce = Vector3.forward * ForwardForce;
        if (shipRigidBody.velocity.magnitude < terminalVelocity) shipRigidBody.AddRelativeForce(addedForce);
    }
}
