using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridSquare : MonoBehaviour
{
    public enum GridType
    {
        SingleCell,
        Chequered,
        Simple
    }
    public enum BlockType
    {
        BlockBoth,
        BlockAbove,
        BelowBelow
    }
    //Grid private variables
    Vector3[] gridPoints;
    Vector3[] cells;
    int numCells;
    Dictionary<Vector3, GameObject> gridCellsStatus;

    GameObject cellContainer;
    GameObject gridObjContainer;

    [Min(1)]
    [SerializeField] int gridWidth = 10;
    [Min(1)]
    [SerializeField] int gridHeight = 10;
    [Min(0.001f)]
    [SerializeField] float cellSize = 1;

    [SerializeField] GridType gridType;
    [SerializeField] bool drawSimple;
    [SerializeField] bool checkMatRuntime;
    [SerializeField] float tileX = 2;
    [SerializeField] float tileY = 2;

    //Simple mesh creation variables
    Mesh mesh;
    Vector3[] meshVertices;
    int[] meshTriangles;
    BoxCollider meshCollider;
    float colliderThickness = 0.01f;
    Vector2[] uvs;
    Renderer rend;

    //Auto cell blocking variables
    [SerializeField] bool autoCellBlocking = false;
    [SerializeField] BlockType blocktype;
    [SerializeField] bool showAboveBoxColliders = false;
    [SerializeField] bool showBelowRays = false;
    [SerializeField] bool checkGroundHits = false;
    [SerializeField] float aboveCheckBoxSize = 1f;
    [SerializeField] float aboveCheckBoxHeight = 1f;
    [SerializeField] int groundLayer;
    Vector3 checkBoxSize;

    public float groundDistance = 0.05f;
    
    //Prefab variables
    [SerializeField] GameObject gridCellPrefab;
    [SerializeField] GameObject secondGridCellPrefab;
    [SerializeField] GameObject blockedAboveCellPrefab;
    [SerializeField] GameObject blockedBelowCellPrefab;
    [SerializeField] bool drawGridPositions = false;
    [SerializeField] bool drawCellPositions = false;

    [ContextMenu("Generate")]  
    public  void Generate()
    {
        Start();
    }

    void Start()
    {
        CreateGrid();
        CreateGridCells();
        CreateGridStatus();

        //yield return new WaitForFixedUpdate();
        //yield return null;

        gridObjContainer = new GameObject("GridObjContainer");
        gridObjContainer.transform.parent = transform;

        //Keep this above collider creation as this causes the physics to detect itself        
        if (gridType == GridType.SingleCell || gridType == GridType.Chequered)
        {
            cellContainer = new GameObject("CellContainer");
            cellContainer.transform.parent = transform;
            CreateSingleCellGrid();
        }
        if (gridType == GridType.Simple)
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            rend = GetComponent<Renderer>();
            CreateSimpleGrid();
        }

        //Keep this last
        meshCollider = gameObject.AddComponent<BoxCollider>();
        SetColliderSize();
    }

    private void CreateAutoCellBlock(GameObject prefab, int i)
    {
        RaycastHit hit;
        Collider[] hitColliders;

        //Create four points from the centre of each cell
        float halfCell = cellSize * 0.5f;
        Vector3 point1 = new Vector3(-halfCell, 0, halfCell);
        Vector3 point2 = new Vector3(halfCell, 0, halfCell);
        Vector3 point3 = new Vector3(halfCell, 0, -halfCell);
        Vector3 point4 = new Vector3(-halfCell, 0, -halfCell);

        bool point1Clear = CheckPoint(point1);
        bool point2Clear = CheckPoint(point2);
        bool point3Clear = CheckPoint(point3);
        bool point4Clear = CheckPoint(point4);
        bool boxClear = false;


        hitColliders = Physics.OverlapBox(cells[i], GetCheckBoxSize());

        for (int k = 0; k <= hitColliders.Length; k++)
        {
            if(hitColliders.Length == 1)
            {
                if (hitColliders[0].gameObject.layer == groundLayer)
                {
                    boxClear = true;
                }
            }
            if (hitColliders.Length == 0)
            {
                boxClear = true;
            }
            
        }
        bool CheckPoint(Vector3 cellPoint)
        {
            if (Physics.Raycast(cells[i] + cellPoint, Vector3.down, out hit, groundDistance))
            {
                if (hit.distance < groundDistance)
                {
                    if(checkGroundHits)
                    {
                        Debug.Log(hit.distance);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        if(blocktype == BlockType.BlockBoth)
        {
            if (point1Clear && point2Clear && point3Clear && point4Clear && boxClear)
            {
                createPhysicalGridCellObject(prefab, i);
            }
            else
            {
                if (blockedAboveCellPrefab != null && point1Clear && point2Clear && point3Clear && point4Clear)
                {
                    createPhysicalGridCellObject(blockedAboveCellPrefab, i);
                }
                if (blockedBelowCellPrefab != null && boxClear)
                {
                    createPhysicalGridCellObject(blockedBelowCellPrefab, i);
                }
                //Marks the cells with something in them
                ChangeCellStatus(cells[i], gameObject);
            }
        }
        if(blocktype == BlockType.BlockAbove)
        {
            if (boxClear)
            {
                createPhysicalGridCellObject(prefab, i);
            }
            else
            {
                if (blockedAboveCellPrefab != null)
                {
                    createPhysicalGridCellObject(blockedAboveCellPrefab, i);
                }
                //Marks the cells with something in them
                ChangeCellStatus(cells[i], gameObject);
            }
        }
        if(blocktype == BlockType.BelowBelow)
        {
            if (point1Clear && point2Clear && point3Clear && point4Clear)
            {
                createPhysicalGridCellObject(prefab, i);
            }
            else
            {
                if (blockedBelowCellPrefab != null)
                {
                    createPhysicalGridCellObject(blockedBelowCellPrefab, i);
                }
                //Marks the cells with something in them
                ChangeCellStatus(cells[i], gameObject);
            }
        }
    }

    private void createPhysicalGridCellObject(GameObject prefab, int i)
    {
        //This function creates the physical cell instance
        if (gridCellPrefab == null)
            return;
        GameObject gridCellInstance = Instantiate(prefab, cells[i], Quaternion.identity);
        Vector3 scale = gridCellInstance.transform.localScale;
        scale.x *= cellSize;
        scale.z *= cellSize;
        gridCellInstance.transform.localScale = scale;

        gridCellInstance.transform.parent = cellContainer.transform;
        if(prefab == gridCellPrefab || prefab == secondGridCellPrefab)
        {
            gridCellInstance.name = "GridCell: " + cells[i];
        }
        else
        {
            gridCellInstance.name = "BlockedCell: " + cells[i];
        }
    }



    private Vector3 GetCheckBoxSize()
    {
        //Determines the size of the checkbox used in the physics calcs
        checkBoxSize = new Vector3((cellSize * aboveCheckBoxSize) * 0.5f, aboveCheckBoxHeight, (cellSize * aboveCheckBoxSize) * 0.5f);
        return checkBoxSize;
    }
    private void Update()
    {
        if (checkMatRuntime)
        {
            UpdateMaterial();
        }
    }
    private void CreateGridCells()
    {
        //Fills the cells array with the centre position
        numCells = gridHeight * gridWidth;
        cells = new Vector3[numCells];

        if (gridPoints == null)
            return;
        for (int i = 1, j = 0; i < gridPoints.Length - gridHeight; i++)
        {
            if (i % (gridHeight + 1) != 0)
            {
                cells[j] = gridPoints[i - 1] + new Vector3(cellSize, 0, cellSize) * 0.5f;

                float trimmedX = float.Parse(cells[j].x.ToString("F3"));
                float trimmedY = float.Parse(cells[j].y.ToString("F3"));
                float trimmedZ = float.Parse(cells[j].z.ToString("F3"));

                cells[j] = new Vector3(trimmedX, trimmedY, trimmedZ);

                j++;
            }
        }
    }
    private void CreateGridStatus()
    {
        //Initialises each cell with a value of null to begin with, or in other words, its empty.
        gridCellsStatus = new Dictionary<Vector3, GameObject>();
        
        for (int i = 0; i < cells.Length; i++)
        {
            gridCellsStatus.Add(cells[i], null);
        };
    }
    public bool CheckCellStatus(Vector3 cellPos)
    {
        bool isEmpty;
        GameObject value;
        gridCellsStatus.TryGetValue(cellPos, out value);
        if (value == null)
        {
            isEmpty = true;
        }
        else
        {
            isEmpty = false;
        }
        return isEmpty;
    }
    public void ChangeCellStatus(Vector3 cellPos, GameObject obj)
    {
        gridCellsStatus[cellPos] = obj;
    }

    private void CreateSingleCellGrid()
    {
        if(gridType == GridType.Chequered)
        {
            if(autoCellBlocking)
            {
                int c = 0;
                //Loops through all the cells
                for (int i = 0; i < gridWidth; i++)
                {
                    for (int j = 0; j < gridHeight; j++)
                    {
                        //This creates the checkered effect
                        if ((i + j) % 2 == 0)
                        {
                                CreateAutoCellBlock(gridCellPrefab, c);
                        }
                        else
                        {
                            if(secondGridCellPrefab)
                            {
                                CreateAutoCellBlock(secondGridCellPrefab, c);
                            }
                            else
                            {
                                ChangeCellStatus(cells[c], gameObject);
                                Debug.Log("Assign second prefab in inspector to use for checkered pattern");
                            }

                        }
                        c++;
                    }
                }
            }
            else
            {
                int c = 0;
                for (int i = 0; i < gridWidth; i++)
                {
                    for (int j = 0; j < gridHeight; j++)
                    {
                        if ((i + j) % 2 == 0)
                        {
                            createPhysicalGridCellObject(gridCellPrefab, c);
                        }
                        else
                        {
                            if(secondGridCellPrefab)
                            {
                                createPhysicalGridCellObject(secondGridCellPrefab, c);
                            }
                            else
                            {
                                Debug.Log("Assign second prefab in inspector to use for checkered pattern");
                            }

                        }
                        c++;
                    }
                }
            }

            
        }
        if(gridType == GridType.SingleCell)
        {
            if(autoCellBlocking)
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    CreateAutoCellBlock(gridCellPrefab, i);
                }
            }
            else
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    createPhysicalGridCellObject(gridCellPrefab, i);
                }
            }
        }
        
    }

    private void CreateSimpleGrid()
    {
        //Creates vertices from the gridPoints array
        meshVertices = new Vector3[]
        {
            gridPoints[0] - transform.position,
            gridPoints[gridHeight] - transform.position,
            gridPoints[gridWidth * (gridHeight + 1)] - transform.position,
            gridPoints[gridPoints.Length - 1] - transform.position
        };

        meshTriangles = new int[]
        {
            0, 1, 2,
            1, 3, 2
        };

        CreateMesh();
    }

    private void CreateUVs()
    {
        uvs = new Vector2[meshVertices.Length];

        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(1, 0);
        uvs[3] = new Vector2(1, 1);
        
    }

    private void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = meshVertices;
        mesh.triangles = meshTriangles;
        mesh.name = "SimpleGridMesh";
        
        CreateUVs();
        mesh.uv = uvs;
        UpdateMaterial();

        mesh.RecalculateNormals();
    }

    private void SetColliderSize()
    {
        //Changes the size and position of the collider based on cellSize and the gridwidth
        meshCollider.size = new Vector3(gridWidth * cellSize, colliderThickness, gridHeight * cellSize);
        meshCollider.center = new Vector3((float)gridWidth * cellSize / 2, (colliderThickness / 2), (float)gridHeight * cellSize / 2);
    }

    private void UpdateMaterial()
    {
        rend.sharedMaterial.mainTextureScale = new Vector2((float)gridWidth / tileX, (float)gridHeight / tileY);
        
    }

    private void CreateGrid()
    {
        //Creates the initial grid of points
        if (gridWidth > 0 && gridHeight > 0)
        {
            gridPoints = new Vector3[(gridWidth + 1) * (gridHeight + 1)];
            for (int i = 0, x = 0; x <= gridWidth; x++)
            {
                for (int z = 0; z <= gridHeight; z++)
                {
                    gridPoints[i] = 
                    new Vector3(
                        transform.position.x + x * cellSize,
                        transform.position.y, 
                        transform.position.z + z * cellSize);
                    i++;
                }
            }
        }
    }
    private void OnValidate()
    {
        CreateGrid();
        CreateGridCells();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (transform.hasChanged)
        {
            CreateGrid();
            CreateGridCells();
            transform.hasChanged = false;
        }

        if (drawGridPositions)
        {
            //Draw the points the grid is made up from
            DrawPointPositions();
        }
        if(drawCellPositions)
        {
            //Draw the cell coordinates in world space
            DrawCellPositions();
        }
        if (autoCellBlocking)
        {
            if (showAboveBoxColliders)
            {
                //Draw the autocellblock physics check boxes
                DrawAutoCellBlockingBoxes();
            }
            if(showBelowRays)
            {
                DrawAutoCellBlockingRays();
            }
            
        }   
        //Draws the grid gizmo
        if(!drawSimple)
        {
            DrawLines();
        }
        else
        {
            DrawSimpleLines();
        }
        
    }

    private void DrawSimpleLines()
    {
        Gizmos.DrawLine(gridPoints[0], gridPoints[gridHeight]);
        Gizmos.DrawLine(gridPoints[gridHeight], gridPoints[gridPoints.Length - 1]);
        Gizmos.DrawLine(gridPoints[gridPoints.Length - 1], gridPoints[gridPoints.Length - 1 - gridHeight]);
        Gizmos.DrawLine(gridPoints[0], gridPoints[gridPoints.Length - 1 - gridHeight]);
    }

    private void DrawAutoCellBlockingRays()
    {
        float halfCell = cellSize * 0.5f;
        Vector3 point1 = new Vector3(-halfCell, 0, halfCell);
        Vector3 point2 = new Vector3(halfCell, 0, halfCell);
        Vector3 point3 = new Vector3(halfCell, 0, -halfCell);
        Vector3 point4 = new Vector3(-halfCell, 0, -halfCell);
        if (cells != null)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                Gizmos.DrawLine(cells[i] + point1, cells[i] + point1 + new Vector3(0, -groundDistance, 0));
                Gizmos.DrawLine(cells[i] + point2, cells[i] + point2 + new Vector3(0, -groundDistance, 0));
                Gizmos.DrawLine(cells[i] + point3, cells[i] + point3 + new Vector3(0, -groundDistance, 0));
                Gizmos.DrawLine(cells[i] + point4, cells[i] + point4 + new Vector3(0, -groundDistance, 0));
            }
        }
    }

    private void DrawAutoCellBlockingBoxes()
    {
        if (cells != null)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                Gizmos.DrawWireCube(cells[i], new Vector3(cellSize * aboveCheckBoxSize, aboveCheckBoxHeight, cellSize * aboveCheckBoxSize));
            }
        }
    }

    private void DrawLines()
    {
        if (gridHeight > 0 && gridWidth > 0)
        {
            //Z lines
            for (int x = 0; x < gridWidth + 1; x++)
            {
                for (int z = 0; z < gridHeight; z++)
                {
                    Gizmos.DrawLine(gridPoints[x * (gridHeight + 1) + z], gridPoints[x * (gridHeight + 1) + z + 1]);
                }
            }
            //X lines
            for (int z = 0; z < gridHeight + 1; z++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    Gizmos.DrawLine(gridPoints[z * gridWidth + x], gridPoints[z * gridWidth + (gridHeight + 1) + x]);
                }
            }
        }
    }

    private void DrawCellPositions()
    {
#if UNITY_EDITOR
        for (int i = 0; i < numCells; i++)
        {
            UnityEditor.Handles.Label(cells[i], cells[i].ToString("F3"));
        }
#endif
    }

    private void DrawPointPositions()
    {
#if UNITY_EDITOR
        for (int i = 0; i < gridPoints.Length; i++)
        {
            UnityEditor.Handles.Label(gridPoints[i], new Vector2(gridPoints[i].x, gridPoints[i].z).ToString("F3"));
        }
#endif
    }
    public GameObject GetGridObjContainer()
    {
        return gridObjContainer;
    }
    public float GetCellSize()
    {
        return cellSize;
    }
    public float GetGridHeight()
    {
        return gridHeight;
    }
    public float GetGridWidth()
    {
        return gridWidth;
    }
    public Dictionary<Vector3, GameObject> GetCellsDictionary()
    {
        foreach (KeyValuePair<Vector3, GameObject> entry in gridCellsStatus)
        {
            Debug.Log(entry);
        }
        return gridCellsStatus;
    }
    
}
