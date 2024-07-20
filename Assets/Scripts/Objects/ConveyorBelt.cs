using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : PlacedObject
{

    private Vector2Int previousPosition;
    private Vector2Int gridPosition;
    private Vector2Int nextPosition;

    private Caja caja;
    float COOLDOWN_TIME = 1f;
    private State state;
    private enum State
    {
        Cooldown,
        MovingToDropItem
    }
    private float timer;

    protected override void Setup()
    {
        gridPosition = origin;

        previousPosition = origin + PlacedObjectTypeSO.GetDirForwardVector(dir) * -1;
        nextPosition = origin + PlacedObjectTypeSO.GetDirForwardVector(dir);

        state = State.Cooldown;
    }


    public void ItemResetHasAlreadyMoved()
    {
        if (!IsEmpty())
        {
            GetCaja().ResetHasAlreadyMoved();
        }
    }

    public void Update()
    {

        switch (state)
        {
            default:
            case State.Cooldown:

                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    state = State.MovingToDropItem;
                }
                break;
            case State.MovingToDropItem:

                if (!IsEmpty() && GetCaja().CanMove())
                {
                    PlacedObject nextPlacedObject = GridBuildingSystem3D.Instance.GetGridObject(nextPosition).GetPlacedObject();
                    if (nextPlacedObject != null)
                    {
                        // Has object next
                        if (nextPlacedObject is ConveyorBelt)
                        {
                            ConveyorBelt conveyorBelt = nextPlacedObject as ConveyorBelt;
                            if (conveyorBelt.TrySetWorldItem(caja))
                            {
                                ItemResetHasAlreadyMoved();
                                // Successfully moved item onto next slot
                                // Move World Item
                                caja.SetGridPosition(conveyorBelt.GetGridPosition());

                                
                                // Remove current world item
                                RemoveWorldItem();
                            }
                        }

                        if (nextPlacedObject is Salida)
                        {
                            Salida salida = nextPlacedObject as Salida;

                            caja.SetGridPosition(salida.GetGridPosition());
                            salida.cargarCaja(caja);

                            CodeMonkey.Utils.UtilsClass.CreateWorldTextPopup("Caja entregada", transform.position);

                        }

                 
                        timer = COOLDOWN_TIME;
                        state = State.Cooldown;

                    }
                }
                break;
        }
    }

    public Vector2Int GetPreviousGridPosition()
    {
        return previousPosition;
    }

    public Vector2Int GetNextGridPosition()
    {
        return nextPosition;
    }

    public void SetCaja(Caja caja)
    {
        this.caja = caja;
    }

    public Caja GetCaja()
    {
        return caja;
    }

    public bool IsEmpty()
    {
        return caja == null;
    }

    public bool TrySetWorldItem(Caja caja)
    {
        if (IsEmpty())
        {
            this.caja = caja;
             timer = COOLDOWN_TIME;
                        state = State.Cooldown;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveWorldItem()
    {
        caja = null;
    }

    public bool TryGetCaja(out Caja caja)
    {
        if (!IsEmpty())
        {
            caja = this.caja;
            return true;
        }
        else
        {
            caja = null;
            return false;
        }
    }




}
