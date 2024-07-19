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
                     if (grabPlacedObject is Generator) {
                       
                        Generator generator = grabPlacedObject as Generator;
                            if (generator.GetItemCount() > 0) {
                                state = State.MovingToDropItem;
                                grabTimer = 0.5f;
                                generator.SetItemCount(generator.GetItemCount() - 1);
                                //Debug.Log(generator.GetItemCount());
                                UtilsClass.CreateWorldTextPopup(generator.GetItemCount().ToString(), transform.position);
                            }
                        }
                }
                
                break;
             case State.MovingToDropItem:
                timer -= Time.deltaTime;
                if (timer <= 0f) {
                    state = State.DroppingItem;
                }
                break;
            case State.DroppingItem:

                state = State.Cooldown;
                float COOLDOWN_TIME = .2f;
                timer = COOLDOWN_TIME;
                
                break;
        }
    }
}
