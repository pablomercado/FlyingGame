using UnityEngine;
using System.Collections;

public class DotProductTest : MonoBehaviour {


    public GameObject GO1;
    public GameObject GO2;
    public GameObject GO3;



    // Use this for initialization
    void Start () {
	
        


	}
	
	// Update is called once per frame
	void Update () {
        Vector3 vector1 = GO2.transform.position - GO1.transform.position;
        Vector3 vector2 = GO3.transform.position - GO1.transform.position;
        Debug.Log(Vector3.Dot(vector1, vector2)/vector1.magnitude);

    }
}
