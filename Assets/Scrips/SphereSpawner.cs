
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
    public GameObject Magnet;

    private float rotationController = 0f;
    private Vector3[] cubesPosition;
    private GameObject[] cubes;

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
                //cubeClone.index = i;
                cubes[i] = cubeClone;
            }
        }
        yield return null;
    }

    private IEnumerator moveTo(float duration, Transform cube, Vector3 aim, Vector3 aimRotation)
    {
        float t = 0;
        float durationProgress = 0f;
        Vector3 initialPosition = cube.transform.position;
        Quaternion initialRotation = cube.transform.rotation;
        Quaternion _aimRotation = Quaternion.Euler(aimRotation);
        while (t <= 1)
        {
            cube.transform.position = Vector3.Lerp(initialPosition, aim, t);
            cube.transform.rotation = Quaternion.Lerp(initialRotation, _aimRotation, t);
            t = durationProgress / duration;
            durationProgress += Time.deltaTime;
            Debug.Log(t);
        }
        yield return null;
    }

    private IEnumerator vectorLerpOverTime(float time, Vector3 origin, Vector3 aim)
    {
        yield return null;
    }
    
    void Update()
    {
        //int i = 200;
        for (int i = 0; i < cubesPosition.Length; i++)
        {
            float positionMagnitude = cubesPosition[i].magnitude;
            float scalar = Vector3.Dot(Magnet.transform.position, cubesPosition[i]) / (positionMagnitude + 0.0000001f);
            if (scalar >= positionMagnitude)
            {
                float extraNumber = (scalar / (positionMagnitude + 0.000000001f)) - 1f;
                cubes[i].transform.position = cubesPosition[i] * (1f + Mathf.Sin(Mathf.Pow(extraNumber/2f, 2f)));
                //Vector3 newVector = cubesPosition[i].normalized * scalar;
                //if (newVector.magnitude >= positionMagnitude)
                //    cubes[i].transform.position = newVector;
                //else
                //    cubes[i].transform.position = cubesPosition[i];
            }
            else
                cubes[i].transform.position = cubesPosition[i];
        }

        //if (Input.GetMouseButton(1) || Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log("Pressed right click.");
        //    RaycastHit hit;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
        //        Debug.Log(hit.transform);
        //        if (rigidbody != null)
        //        {
        //            if (rigidbody.isKinematic == false)
        //            {
        //                rigidbody.isKinematic = true;
        //                for (int i = 0; i < cubes.Length; i++)
        //                {
        //                    if (hit.transform == cubes[i].transform)
        //                        StartCoroutine(moveTo(3f, cubes[i].transform, cubesPosition[i], Vector3.zero));
        //                }
        //            }
        //        }
        //    }
        //}
        //if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit hit;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
        //        if (rigidbody != null)
        //        {
        //            if (rigidbody.isKinematic)
        //                rigidbody.isKinematic = false;
        //        }
        //    }
        //}
    }
}
