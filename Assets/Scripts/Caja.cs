using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Caja : MonoBehaviour
{

    private static Transform model;
    public static Vector2Int gridPos;
    private bool hasAlreadyMoved;

    float COOLDOWN_TIME = .1f;
    float timer;

   


    public static Caja Create(Vector2Int gridPosition, Transform modelo)
    {
        model = modelo;
        gridPos = gridPosition;
        Transform worldItemTransform = Instantiate(modelo, GridBuildingSystem3D.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

        Caja caja = worldItemTransform.GetComponent<Caja>();
        caja.SetGridPosition(gridPosition);

        return caja;
    }



    private void Start()
    {
        //put the model in the right position   
        // transform.position = GridBuildingSystem3D.Instance.GetWorldPosition(gridPosition);

    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, GridBuildingSystem3D.Instance.GetWorldPosition(gridPos), Time.deltaTime * 10f);
    }

    public void SetGridPosition(Vector2Int gridPosition)
    {
        gridPos = gridPosition;
    }

    public bool CanMove()
    {
        return !hasAlreadyMoved;
    }

    public void SetHasAlreadyMoved()
    {
        hasAlreadyMoved = true;
    }

    public void ResetHasAlreadyMoved()
    {
        hasAlreadyMoved = false;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
