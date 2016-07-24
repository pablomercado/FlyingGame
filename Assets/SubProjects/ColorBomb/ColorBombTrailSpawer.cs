using UnityEngine;
using System.Collections;

public class ColorBombTrailSpawer : MonoBehaviour {


    public GameObject Target;
    public GameObject ColorBombTrail;

    void Update () {
        if (Input.GetKeyUp(KeyCode.Space)) {
            GameObject colorBombTrail = (GameObject)Instantiate(ColorBombTrail, gameObject.transform.position, Quaternion.identity);
            colorBombTrail.GetComponent<ColorBombTrailController>().ShootTrail(gameObject.transform.position, Target.transform.position);
        }
    }
}

