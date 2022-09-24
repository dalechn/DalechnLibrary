using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dalechn
{
    [ExecuteAlways]
    public class MeshBoundingBoxGenerator : MonoBehaviour
    {
        public enum DrawType { CUBE, RECT }

        public DrawType type = DrawType.CUBE;
        public Color drawColor = Color.red;
        public int glCount = 5;

        public bool ShowMeshGizmos;
        public bool useCombine;

        public MeshBoundingBox boundingBox;

        private BoxCollider boxCollider;

        [ContextMenu("GenerateMeshBoundingBox")]
        private void GenerateMeshBoundingBox()
        {
            Mesh mesh = GetBoundingMesh();
            boundingBox = new MeshBoundingBox(transform, mesh.bounds.center, mesh.bounds.size / 2);
        }

        [ContextMenu("GenerateBoxCollider")]
        private void GenerateBoxCollider()
        {
            Mesh mesh = GetBoundingMesh();
            boxCollider = gameObject.AddAndGetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.center = mesh.bounds.center - transform.position;
            boxCollider.size = mesh.bounds.size;
        }

        private Mesh GetBoundingMesh()
        {
            Mesh mesh;
            if (useCombine)
            {
                mesh = GetWorldCombineMesh(gameObject);
            }
            else
            {
                mesh = GetWorldMesh(gameObject);
            }
            return mesh;
        }

        public Mesh GetWorldCombineMesh(GameObject go)
        {
            SkinnedMeshRenderer[] skinRenderers = go.GetComponentsInChildren<SkinnedMeshRenderer>();
            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combineInstances = new CombineInstance[skinRenderers.Length + meshFilters.Length];

            int index = 0;
            for (int i = 0; i < skinRenderers.Length; i++)
            {
                var render = skinRenderers[i];
                var mesh = new Mesh();
                render.BakeMesh(mesh);
                combineInstances[index] = new CombineInstance
                {
                    mesh = mesh,
                    transform = render.gameObject.transform.localToWorldMatrix,
                    subMeshIndex = 0
                };

                index++;
            }

            for (int i = 0; i < meshFilters.Length; i++)
            {
                var render = meshFilters[i];
                var temp = (render.sharedMesh != null) ? render.sharedMesh : render.mesh;
                var mesh = (Mesh)Instantiate(temp);
                combineInstances[index] = new CombineInstance
                {
                    mesh = mesh,
                    transform = render.gameObject.transform.localToWorldMatrix,
                    subMeshIndex = 0
                };

                index++;
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combineInstances, true, true);

            return combinedMesh;
        }

        public Mesh GetWorldMesh(GameObject meshGO)
        {
            Mesh mesh = null;
            if (meshGO != null)
            {
                SkinnedMeshRenderer skinnedMeshRenderer = meshGO.GetComponent<SkinnedMeshRenderer>();
                if (skinnedMeshRenderer != null)
                {
                    mesh = new Mesh();
                    skinnedMeshRenderer.BakeMesh(mesh);
                }
                else
                {
                    MeshFilter meshFilter = meshGO.GetComponent<MeshFilter>();
                    mesh = (Mesh)Instantiate((meshFilter.sharedMesh != null) ? meshFilter.sharedMesh : meshFilter.mesh);
                }
            }

            if (meshGO == null || mesh == null) { return null; }

            CombineInstance combineInstance;
            combineInstance = new CombineInstance { mesh = mesh, transform = meshGO.transform.localToWorldMatrix, subMeshIndex = 0 };
            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(new CombineInstance[] { combineInstance }, true, true);
            return combinedMesh;
        }

        private void OnDrawGizmos()
        {
            if (ShowMeshGizmos)
            {
                if (boundingBox != null)
                {
                    boundingBox.DrawBoundingBox();
                }
            }
        }

        static Material s_LineMaterial;
        static void CreateLineMaterial()
        {
            if (!s_LineMaterial)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                s_LineMaterial = new Material(shader);
                s_LineMaterial.hideFlags = HideFlags.HideAndDontSave;
                s_LineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                s_LineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                s_LineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                s_LineMaterial.SetInt("_ZWrite", 0);
            }
        }

        //OnPostRender OnPreRender需要挂在相机脚本下
        private void OnRenderObject()
        {
            CreateLineMaterial();
            s_LineMaterial.SetPass(0);

            if (type == DrawType.CUBE)
            {
                DrawCube();
            }
            else if (type == DrawType.RECT)
            {
                GetScreenRect();
                DrawLockRange();
            }
        }

        Vector3[] worldVertex;
        Rect rect = new Rect();

        private void DrawCube()
        {
            worldVertex = boundingBox.GetWorldBoundingPoints();
            //GL.PushMatrix();
            //GL.LoadPixelMatrix();

            GL.Begin(GL.LINES);
            GL.Color(drawColor);

            GL.Vertex3(worldVertex[0].x, worldVertex[0].y, worldVertex[0].z);
            GL.Vertex3(worldVertex[1].x, worldVertex[1].y, worldVertex[1].z);

            GL.Vertex3(worldVertex[0].x, worldVertex[0].y, worldVertex[0].z);
            GL.Vertex3(worldVertex[2].x, worldVertex[2].y, worldVertex[2].z);

            GL.Vertex3(worldVertex[0].x, worldVertex[0].y, worldVertex[0].z);
            GL.Vertex3(worldVertex[3].x, worldVertex[3].y, worldVertex[3].z);

            GL.Vertex3(worldVertex[1].x, worldVertex[1].y, worldVertex[1].z);
            GL.Vertex3(worldVertex[4].x, worldVertex[4].y, worldVertex[4].z);

            GL.Vertex3(worldVertex[1].x, worldVertex[1].y, worldVertex[1].z);
            GL.Vertex3(worldVertex[6].x, worldVertex[6].y, worldVertex[6].z);

            GL.Vertex3(worldVertex[2].x, worldVertex[2].y, worldVertex[2].z);
            GL.Vertex3(worldVertex[4].x, worldVertex[4].y, worldVertex[4].z);

            GL.Vertex3(worldVertex[2].x, worldVertex[2].y, worldVertex[2].z);
            GL.Vertex3(worldVertex[5].x, worldVertex[5].y, worldVertex[5].z);

            GL.Vertex3(worldVertex[3].x, worldVertex[3].y, worldVertex[3].z);
            GL.Vertex3(worldVertex[5].x, worldVertex[5].y, worldVertex[5].z);

            GL.Vertex3(worldVertex[3].x, worldVertex[3].y, worldVertex[3].z);
            GL.Vertex3(worldVertex[6].x, worldVertex[6].y, worldVertex[6].z);

            GL.Vertex3(worldVertex[4].x, worldVertex[4].y, worldVertex[4].z);
            GL.Vertex3(worldVertex[7].x, worldVertex[7].y, worldVertex[7].z);

            GL.Vertex3(worldVertex[5].x, worldVertex[5].y, worldVertex[5].z);
            GL.Vertex3(worldVertex[7].x, worldVertex[7].y, worldVertex[7].z);

            GL.Vertex3(worldVertex[6].x, worldVertex[6].y, worldVertex[6].z);
            GL.Vertex3(worldVertex[7].x, worldVertex[7].y, worldVertex[7].z);

            GL.End();

            //GL.PopMatrix();
        }

        private Rect GetScreenRect()
        {
            worldVertex = boundingBox.GetWorldBoundingPoints();

            float xMin = float.MaxValue, xMax = float.MinValue, yMin = float.MaxValue, yMax = float.MinValue;
            for (int i = 0; i < worldVertex.Length; i++)
            {
                Vector3 screenVertex = Camera.main.WorldToScreenPoint(worldVertex[i]);
                if (screenVertex.x < xMin)
                {
                    xMin = screenVertex.x;
                }
                if (screenVertex.x > xMax)
                {
                    xMax = screenVertex.x;
                }
                if (screenVertex.y < yMin)
                {
                    yMin = screenVertex.y;
                }
                if (screenVertex.y > yMax)
                {
                    yMax = screenVertex.y;
                }
            }

            rect = Rect.MinMaxRect(xMin, yMin, xMax, yMax);

            return rect;
        }

        private void DrawLockRange()
        {
            GL.PushMatrix();
            GL.LoadPixelMatrix(); // 加载正交矩阵?

            GL.Begin(GL.LINES);
            GL.Color(drawColor);

            GL.Vertex3(rect.xMin, rect.yMin, 0);
            GL.Vertex3(rect.xMin, rect.yMin + glCount, 0);
            GL.Vertex3(rect.xMin, rect.yMin, 0);
            GL.Vertex3(rect.xMin + glCount, rect.yMin, 0);

            GL.Vertex3(rect.xMax, rect.yMin, 0);
            GL.Vertex3(rect.xMax, rect.yMin + glCount, 0);
            GL.Vertex3(rect.xMax, rect.yMin, 0);
            GL.Vertex3(rect.xMax - glCount, rect.yMin, 0);

            GL.Vertex3(rect.xMin, rect.yMax, 0);
            GL.Vertex3(rect.xMin, rect.yMax - glCount, 0);
            GL.Vertex3(rect.xMin, rect.yMax, 0);
            GL.Vertex3(rect.xMin + glCount, rect.yMax, 0);

            GL.Vertex3(rect.xMax, rect.yMax, 0);
            GL.Vertex3(rect.xMax, rect.yMax - glCount, 0);
            GL.Vertex3(rect.xMax, rect.yMax, 0);
            GL.Vertex3(rect.xMax - glCount, rect.yMax, 0);

            GL.End();

            GL.PopMatrix();
        }
    }


    public static class MonoBehaviourExtensionMethods
    {
        public static T AddAndGetComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }

    [System.Serializable]
    public class MeshBoundingBox
    {
        public Transform body;
        public Vector3 centerOffset;
        public Vector3 halfSize;
        public Vector3[] localPoints;

        public MeshBoundingBox(Transform body, Vector3 center, Vector3 halfSize)
        {
            this.body = body;
            this.centerOffset = center - body.position;
            this.halfSize = halfSize;
            this.localPoints = new Vector3[]
                {
                halfSize,
                new Vector3(-halfSize.x, halfSize.y , halfSize.z),
                new Vector3(halfSize.x, -halfSize.y , halfSize.z),
                new Vector3(halfSize.x, halfSize.y , -halfSize.z),
                new Vector3(-halfSize.x, -halfSize.y , halfSize.z),
                new Vector3(halfSize.x, -halfSize.y , -halfSize.z),
                new Vector3(-halfSize.x, halfSize.y , -halfSize.z),
                -halfSize,
                };
        }

        public Vector3[] GetWorldBoundingPoints()
        {
            Vector3[] worldVertex = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                worldVertex[i] = body.position + centerOffset + localPoints[i];
            }
            return worldVertex;
        }

        public void DrawBoundingBox()
        {
            Vector3[] worldVertex = GetWorldBoundingPoints();
            for (int i = 0; i < worldVertex.Length; i++)
            {
                Gizmos.DrawSphere(worldVertex[i], 0.05f + i * 0.05f);
            }

            Gizmos.DrawWireCube(body.position + centerOffset, halfSize * 2);
        }
    }


}
