using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GetPrefabImage : MonoBehaviour
{
    public GameObject objectPrefab;
    Image image;
    Texture2D preview;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(SetThumbnail());
    }
    IEnumerator SetThumbnail()
    {
        Texture2D thumbnail = null;
        while (thumbnail == null)
        {
            thumbnail = AssetPreview.GetAssetPreview(objectPrefab);
            yield return new WaitForSeconds(.5f);
        }
        var sprite = Sprite.Create(thumbnail, new Rect(0.0f, 0.0f, thumbnail.width, thumbnail.height), new Vector2(0.5f, 0.5f), 100.0f);
        image.sprite = sprite;
    }
}
