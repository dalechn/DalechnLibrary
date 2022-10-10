using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectObject : MonoBehaviour
{
    public GameObject prefab;
    GridSelector gridSelector;
    RemoveMode removeMode;
    Button btn;
    private void Awake()
    {
        removeMode = FindObjectOfType<RemoveMode>();
        gridSelector = FindObjectOfType<GridSelector>();
        btn = GetComponent<Button>();
    }
        
    private void Start()
    {
        btn.onClick.AddListener(() => {
            if(removeMode != null)
            {
                removeMode.DisableRemoveMode();
                removeMode.ChangeText();
            }
            if(gridSelector != null)
            {
                gridSelector.DeselectPreview();
                if(prefab)
                {
                    gridSelector.SetGameObjectToPlace(prefab);
                }
            }
        });
    }
}

