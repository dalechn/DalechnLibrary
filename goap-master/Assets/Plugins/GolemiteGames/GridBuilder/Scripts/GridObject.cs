using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    private GridSquare objectsGridSquare;

    public void SetObjectGridSquare(GridSquare grid)
    {
        objectsGridSquare = grid;
    }
    public GridSquare GetObjectGridSquare()
    {
        return objectsGridSquare;
    }
    
}
