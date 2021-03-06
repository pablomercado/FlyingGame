﻿using UnityEngine;
using System.Collections;
using System;

public class CircleSpawner : MonoBehaviour
{

    public RoundedCube RoundedCube;
    public int elements = 10;
    public float radius = 50f;
    public bool RandomSize = false;
    public GameObject Sphere;

    private float rotationController = 0f;
    private Vector3[] cubesPosition;
    private RoundedCube[] cubes;
    private float t = 0f;
    private float durationProgress = 0f;
    // Use this for initialization
    void Start()
    {
        ConfigureField();
        SpawnCubes();
    }

    private void ConfigureField()
    {
        float Pi2 = Mathf.PI * 2;
        float interval = Pi2 / elements;
        cubesPosition = new Vector3[elements];
        for (int i = 0; i < cubesPosition.Length; i++)
        {
            cubesPosition[i] = new Vector3(Mathf.Sin(interval * i) * radius, Mathf.Cos(interval * i) * radius, 0f);
        }
    }

    private void SpawnCubes()
    {
        cubes = new RoundedCube[elements];
        Debug.Assert(cubesPosition != null);
        if (cubesPosition != null)
        {
            for (int i = 0; i < cubesPosition.Length; i++)
            {
                RoundedCube cubeClone = Instantiate(RoundedCube, cubesPosition[i], Quaternion.identity) as RoundedCube;
                cubeClone.GetComponent<Rigidbody>().isKinematic = true;
                cubes[i] = cubeClone;
                if (RandomSize)
                {
                    cubeClone.xSize = Mathf.FloorToInt(UnityEngine.Random.Range(6f, 20f));
                    cubeClone.zSize = Mathf.FloorToInt(UnityEngine.Random.Range(6f, 20f));
                    cubeClone.ySize = Mathf.FloorToInt(UnityEngine.Random.Range(6f, 100f));
                }
                else
                {
                    cubeClone.xSize = 12;
                    cubeClone.zSize = 4;
                    cubeClone.ySize = 12;
                }
                cubeClone.Generate();
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
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                //StartCoroutine(ScaleMe(hit.transform));
                Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    if (rigidbody.isKinematic)
                        rigidbody.isKinematic = false;
                    else
                    {
                        rigidbody.isKinematic = true;
                        for (int i = 0; i < cubes.Length; i++)
                        {
                            if (rigidbody.transform == cubes[i].transform)
                            {
                                StartCoroutine(moveTo(3f, cubes[i].transform, cubesPosition[i], Vector3.zero));
                                //cubes[i].transform.position = cubesPosition[i];
                                //cubes[i].transform.rotation = Quaternion.Euler(Vector3.zero);
                            }
                                
                        }
                    }
                }
            }
        }
    }
}
