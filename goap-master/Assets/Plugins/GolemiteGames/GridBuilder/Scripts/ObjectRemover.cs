using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ObjectRemover : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    GridSquare currentObjGrid;
    GridSelector gridSelector;
    GameObject gameobjectChanged;
    bool matChanged = false;
    public Material removePreviewMat;
    Material hoveredObjectsMaterial;
    // Start is called before the first frame update
    void Start()
    {
        gridSelector = FindObjectOfType<GridSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.GetComponent<GridObject>()) {

                if(!matChanged)
                {
                    //Sets the previously selected object to change it back when not hovering over it
                    gameobjectChanged = hit.collider.gameObject;

                    //Gets the hovered objects material for replacement later
                    hoveredObjectsMaterial = gameobjectChanged.GetComponent<Renderer>().sharedMaterial;

                    //Changes the material for visual confirmation of deletion
                    gridSelector.ChangeObjMat(hit.collider.gameObject, removePreviewMat);

                    //So the code runs once for each object
                    matChanged = true;
                }
                
                //This replaces the objects material if it still finds a GridObject without touching something else, 
                //hence not reaching the else statement.
                if (gameobjectChanged != hit.collider.gameObject)
                {                    
                    //Replaces material if not deleted
                    gridSelector.ChangeObjMat(gameobjectChanged, hoveredObjectsMaterial);
                    matChanged = false;
                }


                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        //Remove Object
                        GridObject gridObject = hit.collider.gameObject.GetComponent<GridObject>();
                        currentObjGrid = gridObject.GetObjectGridSquare();

                        //Removes the object from the GridSquare object freeing up the cell for placement
                        currentObjGrid.ChangeCellStatus(gridObject.transform.position, null);

                        matChanged = false;
                        Destroy(gridObject.gameObject);
                    }
                }
                
            }
            //Only changing back to normal if not hovering over GridObject, so if overlapping objects, this code doesnt run.
            else
            {
                if(gameobjectChanged != null)
                {
                    if(matChanged)
                    {
                        //Changes the object material back to original
                        gridSelector.ChangeObjMat(gameobjectChanged, hoveredObjectsMaterial);
                        matChanged = false;
                    }
                    
                }

            }
        }
        else
        {
            if (gameobjectChanged != null)
            {
                gridSelector.ChangeObjMat(gameobjectChanged, hoveredObjectsMaterial);
            }
        }
    }
}


