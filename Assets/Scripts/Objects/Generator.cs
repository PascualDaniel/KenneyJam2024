using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Generator : PlacedObject
{
    

    [SerializeField] private Transform cajaPrefab;

    public Caja caja;

    protected override void Setup()
    {
        caja = Caja.Create(origin, cajaPrefab);
        Debug.Log("Caja creada en " + GetGridPosition() + caja);
    }

    public Caja GetCaja()
    {
        return caja;
    }

    public void RemoveCaja()
    {
        caja = null;
    }

    public bool HasCaja()
    {
       return caja != null;
    }



   
}
