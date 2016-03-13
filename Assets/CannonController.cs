using UnityEngine;
using System.Collections;

public class CannonController : MonoBehaviour {

    public BulletController Bullet;
    public float bulletForce = 500f;


    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Joystick1Button2)) {

            BulletController bulletClone = Instantiate(Bullet, transform.position + Vector3.forward * 2f, gameObject.transform.rotation) as BulletController;
            bulletClone.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletForce);
        }

    }
}
