using UnityEngine;
using System.Collections;

public class ColorBombTrailController : MonoBehaviour {

    private BoxCollider boxCollider;
    private float speedOfTrail = 2f;
    private float fraction = 0;
    private Vector3 targetPosition, initialPosition;
    private float angleInDegrees = 0f;

    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    public void ShootTrail(Vector3 _initialPosition, Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
        initialPosition = _initialPosition;

        double angle2 = Mathf.Atan2(targetPosition.y - initialPosition.y, targetPosition.x - initialPosition.x);
        angleInDegrees = (float)(angle2);
        angleInDegrees = Mathf.Rad2Deg * angleInDegrees;

        gameObject.transform.position = initialPosition;
        //gameObject.transform.eulerAngles = new Vector3(0, 0, angleInDegrees);

        //Vector3 forceVector = gameObject.transform.right;
        fraction = 0f;
        //gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //gameObject.GetComponent<Rigidbody>().AddForce(forceVector * speedOfTrail, ForceMode.Impulse);
        //gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //gameObject.GetComponent<Rigidbody>().AddForce(forceVector * speedOfTrail, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update () {
        if (fraction < 1) {
            fraction += Time.deltaTime * speedOfTrail;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, fraction);
        }
    }

    //void OnCollisionEnter()
    //{

    //    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //    gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    //    gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //}
}
