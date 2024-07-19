using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Testing : MonoBehaviour
{
    private Grid<bool> boolGrid;


    [SerializeField] private GameObject plane;
    private void Start()
    {
        boolGrid = new Grid<bool>(20, 10, 8f, Vector3.zero, (Grid<bool> g, int x, int y) => false);

        
    }

    private void Update()
    {
        Vector3 position = GetMouseWorldPosition();
       if(Input.GetMouseButtonDown(0))
        {
           
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0;
        return vec;
    }
    public Vector3 GetMouseWorldPositionWithZ(Vector3 mousePosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(mousePosition);
        return worldPosition;
    }
}

