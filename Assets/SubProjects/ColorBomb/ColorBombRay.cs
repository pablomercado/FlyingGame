using UnityEngine;
using System.Collections;

public class ColorBombRay : MonoBehaviour {

    public ProceduralPlane[] planes;
    public Color[] Colors;
    private int xSize, ySize;

    void Start()
    {
        Generate(6, 1);
    }

    public void Generate(int _xSize, int _ySize)
    {
        int i = 0;
        foreach (var plane in planes) {
            plane.GetComponent<MeshRenderer>().material.SetInt("_MC", i);
            plane.GetComponent<MeshRenderer>().material.SetColor("_Color", Colors[i]);
            plane.Generate(_xSize, _ySize);
            i++;
        }
            
    }


}
