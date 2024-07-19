using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class GridBuidingSystem : MonoBehaviour
{

    [SerializeField]
    private List<PlacedObjectTypeSO> placedObjectTypeSOList;
    private PlacedObjectTypeSO placedObjectTypeSO;

    private GridXZ<GridObject> grid;
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;
    private void Awake()
    {
        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));

        placedObjectTypeSO = placedObjectTypeSOList[0];
    }

    public class GridObject
    {
        private GridXZ<GridObject> grid;
        private int x;
        private int z;
        private Transform transform;

        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void SetTransform(Transform transform)
        {
            this.transform = transform;
            grid.TriggerGridObjectChanged(x, z);
        }
        public void ClearTransform()
        {
            transform = null;
        }

        public bool CanBuild()
        {
            return transform == null;
        }

        public override string ToString()
        {
            return x + ", " + z + "\n" + transform;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXZ(Mouse3D.GetMouseWorldPosition(), out int x, out int z);

            List<Vector2Int> gridPositionList =
                placedObjectTypeSO.GetGridPositionList(new Vector2Int(x, z), dir);

            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z)
                    + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                Transform t = Instantiate(
                    placedObjectTypeSO.prefab,
                     placedObjectWorldPosition,
                      Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetTransform(t);
                }


            }
            else
            {
                UtilsClass.CreateWorldTextPopup("Can't Build Here", Mouse3D.GetMouseWorldPosition());
            }



        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
            UtilsClass.CreateWorldTextPopup("Rotating " + dir, Mouse3D.GetMouseWorldPosition());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            placedObjectTypeSO = placedObjectTypeSOList[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            placedObjectTypeSO = placedObjectTypeSOList[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            placedObjectTypeSO = placedObjectTypeSOList[2];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            placedObjectTypeSO = placedObjectTypeSOList[3];
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            placedObjectTypeSO = placedObjectTypeSOList[4];
        }
    }



}