using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CodeMonkey.Utils;
using UnityEngine;

public class Extractor : PlacedObject
{

    private enum State
    {
        Cooldown,
        WaitingForItemToGrab,
        MovingToDropItem,
        DroppingItem,
    }

    private Vector2Int grabPosition;
    private Vector2Int dropPosition;

    private float timer;
    private float grabTimer;

    private Caja caja;

    private State state;
    protected override void Setup()
    {
        grabPosition = origin + PlacedObjectTypeSO.GetDirForwardVector(dir) * -1;
        dropPosition = origin + PlacedObjectTypeSO.GetDirForwardVector(dir);



        state = State.Cooldown;
    }


    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            default:
            case State.Cooldown:

                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    state = State.WaitingForItemToGrab;
                }
                break;
            case State.WaitingForItemToGrab:

                PlacedObject grabPlacedObject = GridBuildingSystem3D.Instance.GetGridObject(grabPosition).GetPlacedObject();
                PlacedObject dropPlacedObject = GridBuildingSystem3D.Instance.GetGridObject(dropPosition).GetPlacedObject();

                if (grabPlacedObject != null && dropPlacedObject != null)
                {


                    if (grabPlacedObject is Generator)
                    {

                        Generator generator = grabPlacedObject as Generator;
                        if (generator.HasCaja())
                        {

                            state = State.MovingToDropItem;
                            grabTimer = 0.5f;
                            caja = generator.GetCaja();
                            generator.RemoveCaja();
                            UtilsClass.CreateWorldTextPopup("Caja", transform.position);

                        }
                    }
                }

                break;
            case State.MovingToDropItem:
                timer -= Time.deltaTime;
                caja.SetGridPosition(GetGridPosition());
                if (timer <= 0f)
                {

                    state = State.DroppingItem;
                }
                break;
            case State.DroppingItem:
                dropPlacedObject = GridBuildingSystem3D.Instance.GetGridObject(dropPosition).GetPlacedObject();
                if (dropPlacedObject != null)
                {
                    if (dropPlacedObject is ConveyorBelt)
                    {

                        ConveyorBelt belt = dropPlacedObject as ConveyorBelt;
                        belt.SetCaja(caja);
                        caja.SetGridPosition(belt.GetGridPosition());
                        caja = null;

                        state = State.Cooldown;
                        float COOLDOWN_TIME = .2f;
                        timer = COOLDOWN_TIME;

                    }
                }

                break;
        }
    }
}
