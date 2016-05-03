using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "building")
        {
            Destroy(col.gameObject);
            Destroy(this.gameObject);
        }
    }
}
