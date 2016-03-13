using UnityEngine;
using System.Collections;

public class VelocityMonitor : MonoBehaviour {


	void Update () {
        Debug.Log(gameObject.GetComponent<Rigidbody>().velocity.magnitude);
	}
}
