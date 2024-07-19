using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Extractor : PlacedObject
{
    private Vector2Int grabPosition;
    private Vector2Int dropPosition;
    private float grabTimer;


    protected override void Setup()
    {
        grabPosition = origin + PlacedObjectTypeSO.GetDirForwardVector(dir) * -1;
        dropPosition = origin + PlacedObjectTypeSO.GetDirForwardVector(dir);
    }


    // Update is called once per frame
    private void Update()
    {
        grabTimer -= Time.deltaTime;
        if (grabTimer <= 0f)
        {
            grabTimer += 1f;
            PlacedObject grabPlacedObject = GridBuildingSystem3D.Instance.GetGridObject(grabPosition).GetPlacedObject();
            PlacedObject dropPlacedObject = GridBuildingSystem3D.Instance.GetGridObject(dropPosition).GetPlacedObject();
            if (grabPlacedObject != null && dropPlacedObject == null)
            {
                GridBuildingSystem3D.Instance.GetGridObject(grabPosition).SetPlacedObject(null);
                GridBuildingSystem3D.Instance.GetGridObject(dropPosition).SetPlacedObject(grabPlacedObject);
            }
        }

    }
}
