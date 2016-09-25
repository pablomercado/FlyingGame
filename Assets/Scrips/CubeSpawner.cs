using UnityEngine;
using System.Collections;
using System;

public class CubeSpawner : MonoBehaviour {

    public RoundedCube RoundedCube;
    public int xSize = 10;
    public int zSize = 10;
    public int distanceBetweenCubes = 20;
    public bool RandomSize = false;
    public GameObject Sphere;

    private float rotationController = 0f;
    private Vector3[] cubesPosition;
    private RoundedCube[] cubes;

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
        cubes = new RoundedCube[xSize * zSize];
        if (cubesPosition != null) {
            Quaternion rotation = Quaternion.Euler(new Vector3(10f, 0f, 0f));
            for (int i = 0; i < cubesPosition.Length; i++) {
                RoundedCube cubeClone = Instantiate(RoundedCube, cubesPosition[i], rotation) as RoundedCube;
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

    void Update () {

        rotationController += Time.deltaTime;
        Vector3 position = new Vector3(Mathf.Sin(rotationController), Mathf.Cos(rotationController), 0f);
        Debug.Log(rotationController + " ," + Mathf.Cos(rotationController));
        Sphere.transform.position = position;
        if (rotationController > Mathf.PI * 1)
            rotationController = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                //StartCoroutine(ScaleMe(hit.transform));
                Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
                if (rigidbody != null)
                    rigidbody.isKinematic = false;
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
        }
    }
}
