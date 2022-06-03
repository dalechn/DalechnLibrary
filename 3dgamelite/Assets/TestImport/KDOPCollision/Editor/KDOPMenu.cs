using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class KDOPMenu
{
    private static string KDOPPath = "KDOPCollisionMeshPath";

    private static List<int> processedMeshes = new List<int>();

    // Generates a new K-DOP for a given GameObject. If askBeforeOverwrite is true, a dialog will appear
    // when a K-DOP already exists.
    private static void GenerateKDOPCollision(GameObject gameObject, Vector3[] dirs, bool askBeforeOverwrite)
    {
        // Find all mesh filters in the current game object and its children
        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

        // Check whether there are any meshes at all
        bool anyMeshes = false;
        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.sharedMesh != null)
            {
                anyMeshes = true;
                break;
            }
        }

        // No meshes? Get out!
        if (anyMeshes == false)
        {
            string errMsg = "GameObject '" + gameObject.name + "' does not contain any valid meshes.";
            if (askBeforeOverwrite)
                EditorUtility.DisplayDialog("Error!", errMsg, "Okay");
            else
                Debug.LogError(errMsg, gameObject);
            return;
        }

        // Make sure the object has a collider
        MeshCollider collider = gameObject.GetComponent<MeshCollider>();
        if (collider == null)
            collider = Undo.AddComponent(gameObject, typeof(MeshCollider)) as MeshCollider;

        // Get an instance ID for the object, depending on whether it's a prefab,
        // a single mesh or just something random
        int instanceID;
        PrefabType prefType = PrefabUtility.GetPrefabType(gameObject);
        bool isPrefab = (prefType != PrefabType.None &&
                         prefType != PrefabType.DisconnectedPrefabInstance &&
                         prefType != PrefabType.DisconnectedModelPrefabInstance);
        Object prefab = PrefabUtility.GetPrefabParent(gameObject);
        if (isPrefab && prefab != null)
            instanceID = prefab.GetInstanceID();
        else if (meshFilters.Length == 1 && gameObject.GetComponent<MeshFilter>() != null)
            instanceID = meshFilters[0].sharedMesh.GetInstanceID();
        else
            instanceID = gameObject.GetInstanceID();

        // Assemble path
        string path = "Assets" + EditorPrefs.GetString(KDOPPath, "") + "/";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        path += instanceID + ".asset";

        // Try to find an existing mesh
        Mesh existingMesh = AssetDatabase.LoadAssetAtPath(path, typeof(Mesh)) as Mesh;
        if (existingMesh != null)
        {
            if (processedMeshes.Contains(instanceID))
            {
                // In case we already processed this mesh before in this run (i.e. through a similar GameObject
                // being selected), use the existing mesh
                collider.sharedMesh = existingMesh;
                collider.convex = true;
                return;
            }
            else if (askBeforeOverwrite && EditorUtility.DisplayDialog("Overwrite K-DOP?", "A K-DOP collision mesh for '" +
                gameObject.name + "' already exists. Overwriting the mesh will automatically update it on all other objects" +
                " that reference it. Sounds good?", "Overwrite", "Use existing") == false)
            {
                processedMeshes.Add(instanceID);

                // If asked for overwrite and the user declined, use the existing mesh
                collider.sharedMesh = existingMesh;
                collider.convex = true;
                return;
            }
        }

        // Their either was no existing mesh or the user wanted to overwrite it,
        // generate a new K-DOP
        List<KDOPPolygon> polys = KDOPPolygon.GenerateKDopPolygons(gameObject.transform, meshFilters, dirs);
        Mesh kdopMesh = KDOPPolygon.MeshFromPolygons(polys);

        // Try to save the mesh and set it
        try
        {
            if (existingMesh == null)
            {
                // Recalculate normals and save the mesh
                kdopMesh.RecalculateNormals();
                AssetDatabase.CreateAsset(kdopMesh, path);
                AssetDatabase.SaveAssets();

                Debug.Log(path + " saved.", gameObject);

                collider.sharedMesh = kdopMesh;
                collider.convex = true;
            }
            else
            {
                // Overwrite the existing mesh, keeping refs intact
                existingMesh.Clear();
                existingMesh.vertices = kdopMesh.vertices;
                existingMesh.SetIndices(kdopMesh.GetIndices(0), MeshTopology.Triangles, 0);
                existingMesh.RecalculateNormals();
                kdopMesh = existingMesh;
                AssetDatabase.SaveAssets();

                Debug.Log(path + " saved.", gameObject);

                // Loop over all mesh colliders and reassign the replaced mesh if necessary
                foreach (MeshCollider mc in GameObject.FindObjectsOfType<MeshCollider>())
                {
                    if (mc.sharedMesh == existingMesh || mc.sharedMesh == null)
                    {
                        mc.sharedMesh = existingMesh;
                        mc.convex = true;
                        Debug.Log("Updated collision mesh for '" + mc.gameObject.name + "'.");
                    }
                }
            }
        }
        catch (UnityException)
        {
            Debug.LogError("Could not create asset for '" + gameObject.name + "' at '" + path + "'");
            kdopMesh.name = "UNSAVED.asset";
        }
        processedMeshes.Add(instanceID);
    }

    // Generates K-DOPs for all selected GameObjects.
    private static void GenerateKDOPCollisionForSelection(Vector3[] dirs)
    {
		// Must be run in edit mode
		if (Application.isPlaying)
		{
			EditorUtility.DisplayDialog("Play mode", "You must exit play mode to generate K-DOPs!", "Okay");
			return;
		}
		
        processedMeshes.Clear();

        if (Selection.gameObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("No GameObject selected", "Please select at least one GameObject to generate K-DOPs for.", "Okay");
        }
        else
        {
            if (Selection.gameObjects.Length > 1)
            {
                bool dialog = EditorUtility.DisplayDialog("Generating K-DOPs", "You are generating " + dirs.Length * 2 +
                    "DOPs for " + Selection.gameObjects.Length + " game objects. This will overwrite current MeshCollider settings " +
                    "and might take some time. This action cannot be undone. Proceed?", "Okay", "Cancel");
                if (dialog == false)
                    return;
            }

            int progress = 0;
            int amount = Selection.gameObjects.Length;

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                EditorUtility.DisplayProgressBar("Generating K-DOPs: " + progress + "/" + amount,
                    "Processing GameObject " + gameObject.name + "...", (float) progress / (float) amount);

                Undo.RecordObject(gameObject, "Generate KDOP Collision");

                GenerateKDOPCollision(gameObject, dirs, Selection.gameObjects.Length == 1);
                progress++;
            }

            // Remove the progress bar to show that work has finished
            EditorUtility.ClearProgressBar();
        }
    }


    //-------------------------------------------------------------------------
    // MENU ITEMS

    // Helper function: When using the right-click context menu, the functions are called
    // once for each object; When using the top menu, they are only called once. We need
    // to account for this by skipping all but the first.
    private static bool SkipMenuItem(MenuCommand menuCommand)
    {
        // If no objects are selected or this isn't the first, skip
        return (menuCommand.context != null && menuCommand.context != Selection.objects[0]);
    }

    [MenuItem("GameObject/K-DOP Collision/10DOP-X Simplified Collision", false, 0)]
    private static void KDOP10X(MenuCommand menuCommand)
    {
        if (SkipMenuItem(menuCommand)) return;
        GenerateKDOPCollisionForSelection(KDOPPolygon.Dir10X);
    }

    [MenuItem("GameObject/K-DOP Collision/10DOP-Y Simplified Collision", false, 2)]
    private static void KDOP10Y(MenuCommand menuCommand)
    {
        if (SkipMenuItem(menuCommand)) return;
        GenerateKDOPCollisionForSelection(KDOPPolygon.Dir10Y);
    }

    [MenuItem("GameObject/K-DOP Collision/10DOP-Z Simplified Collision", false, 4)]
    private static void KDOP10Z(MenuCommand menuCommand)
    {
        if (SkipMenuItem(menuCommand)) return;
        GenerateKDOPCollisionForSelection(KDOPPolygon.KDopDir10Z);
    }

    [MenuItem("GameObject/K-DOP Collision/18DOP Simplified Collision", false, 6)]
    private static void KDOP18(MenuCommand menuCommand)
    {
        if (SkipMenuItem(menuCommand)) return;
        GenerateKDOPCollisionForSelection(KDOPPolygon.KDopDir18);
    }

    [MenuItem("GameObject/K-DOP Collision/26DOP Simplified Collision", false, 8)]
    private static void KDOP26(MenuCommand menuCommand)
    {
        if (SkipMenuItem(menuCommand)) return;
        GenerateKDOPCollisionForSelection(KDOPPolygon.KDopDir26);
    }

    [MenuItem("GameObject/K-DOP Collision/Set collision mesh folder...", false, 20)]
    private static void SetCollisionMeshFolder(MenuCommand menuCommand)
    {
        // Let the user select a new path
        string path = EditorUtility.OpenFolderPanel("Select K-DOP collision mesh folder", "Assets" + EditorPrefs.GetString(KDOPPath, ""), "");
        if (path == "")
            return;

        // Check whether the path is in the assets folder
        if (path.Length >= Application.dataPath.Length && path.Substring(0, Application.dataPath.Length).Equals(Application.dataPath))
        {
            path = path.Remove(0, Application.dataPath.Length);
            EditorPrefs.SetString(KDOPPath, path);
        }
        else
        {
            EditorUtility.DisplayDialog("Error!", "Please select a path inside your Assets folder.", "Okay");
        }
    }
}
