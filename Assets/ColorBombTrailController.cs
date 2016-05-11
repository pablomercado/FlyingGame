using UnityEngine;
using System.Collections;

public class ColorBombTrailController : MonoBehaviour {

    public ParticleSystem ParticlesTip;
    public ParticleSystem ParticlesOrigin;
    public ParticleSystem ParticlesTarget;
    public GameObject Trail;
    private BoxCollider boxCollider;
    private float speedOfTrail = 2f;
    private float fraction = 0;
    private Vector3 targetPosition, initialPosition;
    private float angleInDegrees = 0f;
    private bool playing = false;

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

        Trail.transform.position = initialPosition;
        ParticlesTarget.transform.position = targetPosition;
        ParticlesOrigin.transform.position = initialPosition;
        Trail.transform.eulerAngles = new Vector3(0, 0, angleInDegrees);

        fraction = 0f;
        playing = true;
    }

    void Update () {

        if (playing) {
            if (ParticlesTip.isStopped) ParticlesTip.Play();
            if (ParticlesOrigin.isStopped) ParticlesOrigin.Play();
            Debug.Log(ParticlesTip.isPlaying);
            if (fraction < 1) {
                fraction += Time.deltaTime * speedOfTrail;
                Trail.transform.position = Vector3.Lerp(initialPosition, targetPosition, fraction);
            }
            else {
                fraction = 0f;
                playing = false;
                ParticlesTip.Stop();
                ParticlesOrigin.Stop();
                ParticlesTarget.Play();
            }
        }
    }
}
