using UnityEngine;
using System.Collections;

public class ColorBombRay : MonoBehaviour {

    public ProceduralPlane plane1;
    private int xSize, ySize;

    void Start()
    {
        Generate(6, 1);
    }

    public void Generate(int _xSize, int _ySize)
    {
        plane1.Generate(_xSize, _ySize);
    }


}
