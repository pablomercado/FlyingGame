using UnityEngine;
using System.Collections;
using System;

public class CubeSpawner : MonoBehaviour {

    public RoundedCube RoundedCube;
    private Vector3[] cubesPosition;
    public int xSize = 10;
    public int zSize = 10;
    public int distanceBetweenCubes = 20;

    // Use this for initialization
    void Start () {
        ConfigureField();
        SpawnCubes();
	}

    private void ConfigureField()
    {
        cubesPosition = new Vector3[xSize * zSize];
        for (int i = 0, z = 0; z < zSize; z++) {
            for (int x = 0; x < xSize; x++, i++) {
                cubesPosition[i] = new Vector3(x * distanceBetweenCubes, 0, z * distanceBetweenCubes);
            }
        }
    }

    private void SpawnCubes()
    {
        if (cubesPosition != null) {
            for (int i = 0; i < cubesPosition.Length; i++) {
                RoundedCube cubeClone = Instantiate(RoundedCube, cubesPosition[i], gameObject.transform.rotation) as RoundedCube;
                cubeClone.xSize = Mathf.FloorToInt(UnityEngine.Random.Range(6f, 20f));
                cubeClone.zSize = Mathf.FloorToInt(UnityEngine.Random.Range(6f, 20f));
                cubeClone.ySize = Mathf.FloorToInt(UnityEngine.Random.Range(6f, 100f));
                cubeClone.Generate();
            }
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
