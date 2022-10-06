using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calcatz.WorldSpaceCanvasUI;

public class ColliderSelectionEventExample : MonoBehaviour {

    [SerializeField] private WorldSpaceUIElement[] allWorldSpaceTexts;

    private Dictionary<GameObject, WorldSpaceUIElement> characterTextDictionary;

    private void Awake() {
        characterTextDictionary = new Dictionary<GameObject, WorldSpaceUIElement>();
        foreach(WorldSpaceUIElement worldSpaceText in allWorldSpaceTexts) {
            Transform target = worldSpaceText.worldSpaceTargetObject;
            GameObject character = target.parent.gameObject;
            characterTextDictionary.Add(character, worldSpaceText);
            worldSpaceText.gameObject.SetActive(false);
        }
    }

    public void SetCharacterTextsEnabled(GameObject[] _selectedCharacters) {
        foreach (WorldSpaceUIElement worldSpaceText in allWorldSpaceTexts) {
            worldSpaceText.gameObject.SetActive(false);
        }
        foreach (GameObject character in _selectedCharacters) {
            if (characterTextDictionary.ContainsKey(character)) {
                characterTextDictionary[character].gameObject.SetActive(true);
            }
        }
    }

}
