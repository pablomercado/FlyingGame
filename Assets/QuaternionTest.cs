using UnityEngine;
using System.Collections;

public class QuaternionTest : MonoBehaviour {

    public Transform Target;
    public float Speed = 1f;

	
	void Update () {
        Vector3 direction = transform.position - Target.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Speed * Time.deltaTime);
	}
}
