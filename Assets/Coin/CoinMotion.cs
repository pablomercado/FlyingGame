using UnityEngine;
using System.Collections;

public class CoinMotion : MonoBehaviour {

    [SerializeField]
    public float multiplier;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(currentPos.x, Mathf.Sin(Time.time + multiplier), currentPos.z);

        transform.eulerAngles = new Vector3(Time.time * 200f, 0f, 0f);
	}
}
