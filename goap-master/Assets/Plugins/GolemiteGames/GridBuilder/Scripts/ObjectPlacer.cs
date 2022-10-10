using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ObjectPlacer : MonoBehaviour
{
    public void PlaceObject(GameObject obj, Vector3 placePos, GridSquare grid, int layer)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {

            bool isEmpty = grid.CheckCellStatus(placePos);

            if(isEmpty)
            {
                grid.ChangeCellStatus(placePos, obj);
                GameObject clonedObj = Instantiate(obj, placePos, Quaternion.identity);
                GridObject clonedObjGrid = clonedObj.AddComponent<GridObject>();
                clonedObjGrid.SetObjectGridSquare(grid);
                clonedObj.layer = layer;
                Transform[] clonedObjChildren = clonedObj.GetComponentsInChildren<Transform>();
                foreach (Transform child in clonedObjChildren)
                {
                    child.gameObject.layer = layer;
                }

                clonedObj.transform.parent = grid.GetGridObjContainer().transform;
            }
            else
            {
                //Debug.Log("Cannot place here");
            }
            
        }

    }
}
