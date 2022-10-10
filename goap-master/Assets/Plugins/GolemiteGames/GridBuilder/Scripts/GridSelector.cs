using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridSelector : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    Vector3 gridSelectorPos;
    Vector3 currentPos;
    Vector3 placementCheckPosition;
    float selectedCellSize;
    GridSquare gridsquare;
    GridSquare currentGrid;
    MeshRenderer meshRenderer;

    GameObject selectedGameObjectToPlace;
    int startingLayer;
    GameObject previewObj;
    bool selected;
    Material selectorMat;

    [SerializeField] bool smoothMove;
    [SerializeField] float moveSpeed = 0.2f;
    [SerializeField] float hoverDistance = 0.01f;
    [SerializeField] bool dragBuild = false;

    [SerializeField] bool invalidPlacementFeedback = false;
    [SerializeField] bool showInvalidPreviewObj = false;
    [SerializeField] Material invalidPlacementMat;
    [SerializeField] ObjectPlacer objectPlacer;
    [SerializeField] Material objPreviewMat;
    Material originPreviewMat;
    private void Awake()
    {
        gridsquare = FindObjectOfType<GridSquare>();
        selectedCellSize = gridsquare.GetCellSize();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        meshRenderer.receiveShadows = false;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        selectorMat = GetComponent<Renderer>().sharedMaterial;
        originPreviewMat = objPreviewMat;
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 scale = transform.localScale;
        scale.x *= selectedCellSize;
        scale.z *= selectedCellSize;
        transform.localScale = scale;
        currentPos = gridSelectorPos;
    }
    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //Get the center of each hovered cell
            GetCellPosition();

            if (hit.collider.GetComponent<GridSquare>())
            {
                if (objectPlacer)
                {
                    if (selected)
                    {
                        CreatePreviewObject();
                        selected = false;
                    }

                    //Click build
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (selectedGameObjectToPlace != null)
                        {
                            currentGrid = hit.collider.GetComponent<GridSquare>();
                            PlaceObjectIfEmpty(currentGrid);
                        }
                    }

                    //Drag build
                    if (dragBuild)
                    {
                        if (Input.GetMouseButton(0))
                        {
                            if (selectedGameObjectToPlace != null)
                            {
                                currentGrid = hit.collider.GetComponent<GridSquare>();
                                PlaceObjectIfEmpty(currentGrid);
                            }
                        }
                    }

                    //Moves the preview object to the centre of the grid cell
                    if (previewObj != null)
                    {
                        if(smoothMove && currentPos != Vector3.zero)
                        {

                            previewObj.transform.position = Vector3.MoveTowards(previewObj.transform.position, gridSelectorPos, moveSpeed);
                        }
                        else
                        {
                            previewObj.transform.position = gridSelectorPos;
                        }
                    }

                }
                //Assign the nearest position on the grid to the selector object
                if(smoothMove && currentPos != Vector3.zero)
                {
                    transform.position = Vector3.MoveTowards(transform.position, gridSelectorPos, moveSpeed);
                }
                else
                {
                    transform.position = gridSelectorPos;
                }

                

                //Shows/Hides the selector tile depending if it is a placeable tile each time selector changes position
                if(gridSelectorPos != currentPos)
                {
                    ShowHideSelectorAndPreview();
                }
            }
            else
            {
                //If the user hovers over the edge of the grid, hide the selector and any preview object
                if (previewObj)
                {
                    previewObj.SetActive(false);
                }
                meshRenderer.enabled = false;
                
                //Sets the current position to something else other than newPos forcing the Selector to recheck if valid tile
                currentPos = Vector3.zero;
            }
        }
        else
        {
            //If the user isnt hovered on anything, hide the selector and any preview object
            if (previewObj)
            {
                previewObj.SetActive(false);
            }
            meshRenderer.enabled = false;
            
            //Sets the current position to something else other than newPos forcing the Selector to recheck if valid tile
            currentPos = Vector3.zero;
        }

        //Manually unselect and delete the preview
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectPreview();
        }
    }

    private void ShowHideSelectorAndPreview()
    {
        currentGrid = hit.collider.GetComponent<GridSquare>();
        //Can place in these cells
        if (currentGrid.CheckCellStatus(placementCheckPosition))
        {
            meshRenderer.enabled = true;
            ChangeObjMat(meshRenderer.gameObject, selectorMat);
             
            if (previewObj)
            {
                if(objPreviewMat)
                {
                    ChangeObjMat(previewObj, objPreviewMat);
                }
                else
                {
                    Debug.Log("Assign preview material");
                }
                previewObj.SetActive(true);
            }
        }
        else
        {
            //Cant place in these cells
            if (invalidPlacementFeedback)
            {
                if (invalidPlacementMat)
                {
                    meshRenderer.enabled = true;
                    ChangeObjMat(meshRenderer.gameObject, invalidPlacementMat);
                    if(previewObj && showInvalidPreviewObj)
                    {
                        ChangeObjMat(previewObj, invalidPlacementMat);
                    }
                }
                else
                {
                    Debug.Log("Assign invalidPlacementMat");
                }
            }
            else
            {
                meshRenderer.enabled = false;
            }


            if (previewObj)
            {
                if(invalidPlacementFeedback && showInvalidPreviewObj)
                {
                    previewObj.SetActive(true);
                }
                else
                {
                    previewObj.SetActive(false);
                }
            }
        }

        currentPos = gridSelectorPos;
    }

    private void PlaceObjectIfEmpty(GridSquare currentGrid)
    {
        if (currentGrid.CheckCellStatus(placementCheckPosition))
        {
            if (objectPlacer)
            {
                objectPlacer.PlaceObject(selectedGameObjectToPlace, placementCheckPosition, currentGrid, startingLayer);
            }
            else
            {
                Debug.Log("Assign ObjectPlacer");
            }
        }
    }

    private void CreatePreviewObject()
    {
        previewObj = Instantiate(selectedGameObjectToPlace, gridSelectorPos, Quaternion.identity);
        previewObj.name = "PreviewObject";
        if(objPreviewMat && objPreviewMat == originPreviewMat)
        {
            ChangeObjMat(previewObj, objPreviewMat);
        }
        else
        {
            objPreviewMat = previewObj.GetComponent<Renderer>().sharedMaterial;
        }
        ChangePreviewObjLayer(2);
    }

    public void ChangeObjMat(GameObject obj, Material newMat)
    {
        Renderer[] children;
        children = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children)
        {
            var mats = new Material[rend.materials.Length];
            for (var i = 0; i < rend.materials.Length; i++)
            {
                mats[i] = newMat;
            }
            rend.materials = mats;
        }
    }
    private void ChangePreviewObjLayer(int newLayer)
    {
        previewObj.layer = newLayer;
        Transform[] previewObjChildren = previewObj.GetComponentsInChildren<Transform>();
        foreach (Transform child in previewObjChildren)
        {
            child.gameObject.layer = 2;
        }
    }

    private void GetCellPosition()
    {
        float newXPos = (RoundTo((hit.point.x - hit.transform.position.x), selectedCellSize) - selectedCellSize / 2);
        float newYPos = (RoundTo((hit.point.y - hit.transform.position.y), selectedCellSize) - selectedCellSize / 2);
        float newZPos = (RoundTo((hit.point.z - hit.transform.position.z), selectedCellSize) - selectedCellSize / 2);

        gridSelectorPos = new Vector3(newXPos, hoverDistance, newZPos) + hit.transform.position;
        placementCheckPosition = new Vector3(newXPos, 0, newZPos) + hit.transform.position;

        float trimmedX = float.Parse(placementCheckPosition.x.ToString("F3"));
        float trimmedY = float.Parse(placementCheckPosition.y.ToString("F3"));
        float trimmedZ = float.Parse(placementCheckPosition.z.ToString("F3"));

        placementCheckPosition = new Vector3(trimmedX, trimmedY, trimmedZ);
    }

    float RoundTo(float value, float multipleOf)
    {
        float roundedNumber;
        roundedNumber = Mathf.Ceil(value / multipleOf) * multipleOf;
        //Prevents rounding up above grid width or height
        if (roundedNumber > gridsquare.GetGridHeight() || roundedNumber > gridsquare.GetGridWidth())
        {
            value -= 0.01f;
        }
        //Prevents the 0 rounding becoming negative number
        if(value == 0)
        {
            value += 0.01f;
        }
        roundedNumber = Mathf.Ceil(value / multipleOf) * multipleOf;
        return roundedNumber;
    }
    
    public void SetGameObjectToPlace(GameObject obj)
    {
        selectedGameObjectToPlace = obj;
        startingLayer = selectedGameObjectToPlace.layer;
        selected = true;
    }
    public void DeselectPreview()
    {
        selectedGameObjectToPlace = null;
        Destroy(previewObj);
        objPreviewMat = originPreviewMat;
    }
    public Vector3 GetSelectorPosition()
    {
        return placementCheckPosition;
    }
}
