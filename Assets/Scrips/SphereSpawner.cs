
using UnityEngine;
using System.Collections;
using System;

public class SphereSpawner : MonoBehaviour
{

    public GameObject RoundedCube;
    public int ElementsPerRing = 10;
    public int NumberOfRings = 10;
    public float radious = 50f;
    public bool RandomSize = false;

    private float rotationController = 0f;
    private Vector3[] cubesPosition;
    private GameObject[] cubes;
    private float t = 0f;
    private float durationProgress = 0f;

    void Start()
    {
        ConfigureField();
        StartCoroutine(SpawnCubes());
    }

    private void ConfigureField()
    {
        float Pi2 = Mathf.PI * 2;
        float xRadiantInterval = Pi2 / ElementsPerRing;
        float yRadiantInterval = Mathf.PI / NumberOfRings;
        cubesPosition = new Vector3[ElementsPerRing * NumberOfRings];
        float radiousRatio;
        for (int y = 1, r = 0; y <= NumberOfRings; y++)
        {
            radiousRatio = Mathf.Sin(yRadiantInterval * y);

            int dynamicElementsPerRing = Mathf.FloorToInt(radiousRatio * ElementsPerRing);

            float yInterval = (radious * 2) / NumberOfRings;

            xRadiantInterval = Pi2 / dynamicElementsPerRing;

            for (int i = 1; i <= dynamicElementsPerRing; i++, r++)
            {
                cubesPosition[r] = new Vector3(Mathf.Cos(xRadiantInterval * i) * radiousRatio * radious, Mathf.Cos(yRadiantInterval * y) * radious, Mathf.Sin(xRadiantInterval * i) * radiousRatio * radious);
                Debug.Log(r + " + component x, " + radiousRatio);
            }
        }
    }

    private IEnumerator SpawnCubes()
    {
        cubes = new GameObject[ElementsPerRing * NumberOfRings];
        Debug.Assert(cubesPosition != null);
        if (cubesPosition != null)
        {
            for (int i = 0; i < cubesPosition.Length; i++)
            {
                GameObject cubeClone = Instantiate(RoundedCube, cubesPosition[i], Quaternion.identity) as GameObject;
                cubeClone.GetComponent<Rigidbody>().isKinematic = true;
                cubes[i] = cubeClone;
                yield return null;
            }
        }
    }

    private IEnumerator moveTo(float duration, Transform cube, Vector3 aim, Vector3 aimRotation)
    {
        t = 0;
        durationProgress = 0f;
        Vector3 initialPosition = cube.transform.position;
        Quaternion initialRotation = cube.transform.rotation;
        Quaternion _aimRotation = Quaternion.Euler(aimRotation);
        while (t <= 1)
        {
            cube.transform.position = Vector3.Lerp(initialPosition, aim, t);
            cube.transform.rotation = Quaternion.Lerp(initialRotation, _aimRotation, t);
            t = durationProgress / duration;
            durationProgress += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    private IEnumerator vectorLerpOverTime(float time, Vector3 origin, Vector3 aim)
    {
        yield return null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    if (rigidbody.isKinematic == false)
                    {
                        rigidbody.isKinematic = true;
                        for (int i = 0; i < cubes.Length; i++)
                        {
                            if (rigidbody.transform == cubes[i].transform)
                                StartCoroutine(moveTo(3f, cubes[i].transform, cubesPosition[i], Vector3.zero));
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    if (rigidbody.isKinematic)
                        rigidbody.isKinematic = false;
                }
            }
        }
    }
}
