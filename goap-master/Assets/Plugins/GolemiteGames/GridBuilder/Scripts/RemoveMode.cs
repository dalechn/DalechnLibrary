using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RemoveMode : MonoBehaviour
{
    ObjectRemover objectRemover;
    GridSelector gridSelector;
    Button btn;
    Text removeText;
    private void Awake()
    {
        objectRemover = FindObjectOfType<ObjectRemover>();
        gridSelector = FindObjectOfType<GridSelector>();
        btn = GetComponent<Button>();
        removeText = transform.GetChild(0).GetComponent<Text>();
    }

    private void Start()
    {
        //Entering object removal mode
        btn.onClick.AddListener(() => {
            //Need to disable placement aswell
            if (gridSelector)
            {
                gridSelector.DeselectPreview();
            }
            EnableRemoveMode();
            ChangeText();
        });
    }
    private void Update()
    {
        //For exiting object removal mode
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            DisableRemoveMode();
            ChangeText();
        }
        
    }
    public void DisableRemoveMode()
    {
        if(objectRemover)
        objectRemover.enabled = false;
    }
    public void EnableRemoveMode()
    {
        if(objectRemover)
        objectRemover.enabled = true;
    }
    public void ChangeText()
    {
        if(objectRemover)
        {
            if (objectRemover.enabled)
            {
                if(removeText)
                removeText.text = "Remove Mode Active";
            }
            else
            {
                if(removeText)
                removeText.text = "Remove Mode";
            }
        }
    }
}
