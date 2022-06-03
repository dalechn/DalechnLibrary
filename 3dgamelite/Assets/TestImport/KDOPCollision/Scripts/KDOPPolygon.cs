using UnityEngine;
using System.Collections.Generic;

// A polygon used to generate K-DOPs. Also provides static helper functions
// for K-DOP generation.
public class KDOPPolygon
{
    public List<Vector3> Vertices = new List<Vector3>();
    public Vector3 Normal;

    private const float rcpSqrt2 = 0.70710678118f;
    private const float rcpSqrt3 = 0.57735026919f;

    public static Vector3[] Dir10X = new Vector3[]
    {
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.0f, rcpSqrt2, rcpSqrt2),
        new Vector3(0.0f, rcpSqrt2, -rcpSqrt2),
    };

    public static Vector3[] Dir10Y = new Vector3[]
    {
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(rcpSqrt2, 0.0f, rcpSqrt2),
        new Vector3(rcpSqrt2, 0.0f, -rcpSqrt2),
    };

    public static Vector3[] KDopDir10Z = new Vector3[]
    {
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(rcpSqrt2, rcpSqrt2, 0.0f),
        new Vector3(rcpSqrt2, -rcpSqrt2, 0.0f),
    };

    public static Vector3[] KDopDir18 = new Vector3[]
    {
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.0f, rcpSqrt2, rcpSqrt2),
        new Vector3(0.0f, rcpSqrt2, -rcpSqrt2),
        new Vector3(rcpSqrt2, 0.0f, rcpSqrt2),
        new Vector3(rcpSqrt2, 0.0f, -rcpSqrt2),
        new Vector3(rcpSqrt2, rcpSqrt2, 0.0f),
        new Vector3(rcpSqrt2, -rcpSqrt2, 0.0f),
    };

    public static Vector3[] KDopDir26 = new Vector3[]
    {
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.0f, rcpSqrt2, rcpSqrt2),
        new Vector3(0.0f, rcpSqrt2, -rcpSqrt2),
        new Vector3(rcpSqrt2, 0.0f, rcpSqrt2),
        new Vector3(rcpSqrt2, 0.0f, -rcpSqrt2),
        new Vector3(rcpSqrt2, rcpSqrt2, 0.0f),
        new Vector3(rcpSqrt2, -rcpSqrt2, 0.0f),
        new Vector3(rcpSqrt3, rcpSqrt3, rcpSqrt3),
        new Vector3(rcpSqrt3, -rcpSqrt3, rcpSqrt3),
        new Vector3(rcpSqrt3, rcpSqrt3, rcpSqrt3),
        new Vector3(-rcpSqrt3, -rcpSqrt3, rcpSqrt3),
    };

    // Split this polygon at a plane. 
    private void Split(Vector3 planeOrigin, Vector3 planeNormal)
    {
        // Loop over all edges i..j and check whether they intersect the plane
        for (int i = 0; i < Vertices.Count; ++i)
        {
            int j = (i + 1) % Vertices.Count;

            Vector3 t = Vertices[j] - Vertices[i];
            float t_dot_n = Vector3.Dot(t, planeNormal);

            // Dot product != 0? Possible intersection 
            if (Mathf.Abs(t_dot_n) > 1e-4f)
            {
                float d = Vector3.Dot(planeOrigin - Vertices[i], planeNormal) / t_dot_n;
                if (d > 0.0f && d < 1.0f)
                {
                    Vertices.Insert(i + 1, d * t + Vertices[i]);
                    ++i;
                }
            }
        }

        // Shift plane origin for clipping
        planeOrigin -= planeNormal * 0.001f;

        // Remove all vertices that are on the wrong side of the cutting plane
        for (int i = 0; i < Vertices.Count; ++i)
        {
            if (Vector3.Dot(Vertices[i] - planeOrigin, planeNormal) < 0.0f)
            {
                Vertices.RemoveAt(i);
                --i;
            }
        }
    }

    // Triangulates this polygon and adds it to lists of vertices / indices, 
    // removing doubles and degenerate triangles along the way.
    private void ToTriangles(List<Vector3> meshVertexList, List<int> meshIndexList)
    {
        List<int> vertexIds = new List<int>();
        
        // Add vertices to the list
        foreach (Vector3 v in Vertices)
        {
            // Loop over all previous vertices and to remove doubles
            bool foundDuplicate = false;
            for (int j = 0; j < meshVertexList.Count; ++j)
            {
                if ((meshVertexList[j] - v).sqrMagnitude < 1e-10f)
                {
                    foundDuplicate = true;
                    vertexIds.Add(j);
                    break;
                }
            }

            if (!foundDuplicate)
            {
                vertexIds.Add(meshVertexList.Count);
                meshVertexList.Add(v);
            }
        }

        // Add triangles (i.e. indices)
        for (int i = 0; i < vertexIds.Count - 2; ++i)
        {
            int i0 = vertexIds[0], i1 = vertexIds[i + 1], i2 = vertexIds[i + 2];

            // Prevent degenerate triangles
            if (i0 != i1 && i0 != i2 && i1 != i2)
            {
                meshIndexList.Add(vertexIds[0]);
                meshIndexList.Add(vertexIds[i + 1]);
                meshIndexList.Add(vertexIds[i + 2]);
            }
        }
    }

    // Helper function to generate arbitrary tangent / bitangent for a given normal
    private static void GenerateTangentSpace(Vector3 normal, out Vector3 tangent, out Vector3 bitangent)
    {
        Vector3 N = new Vector3(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));

        // Find best basis vectors.
        if (N.z > N.x && N.z > N.y)
        {
            tangent = new Vector3(1, 0, 0);
        }
        else
        {
            tangent = new Vector3(0, 0, 1);
        }

        tangent = Vector3.Normalize(tangent - normal * Vector3.Dot(tangent, normal));
        bitangent = Vector3.Cross(tangent, normal);
    }

    // Generate the polygons for a K-DOP by iteratively intersecting planes with each other.
    public static List<KDOPPolygon> GenerateKDopPolygons(Transform rootTransform, MeshFilter[] meshFilters, Vector3[] dirs)
    {
        int planePairCount = dirs.Length;
        float[] minDist = new float[planePairCount];
        float[] maxDist = new float[planePairCount];
        for (int i = 0; i < planePairCount; i++)
        {
            minDist[i] = float.MaxValue;
            maxDist[i] = -float.MaxValue;
        }

        // Extents of the initial planes
        float PlaneExtents = 0.0f;

        // Loop over all meshes
        foreach (MeshFilter meshFilter in meshFilters)
        {
            // Choose a value well over the extents of the mesh for initial planes
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            PlaneExtents = Mathf.Max(PlaneExtents, 100.0f * Mathf.Max(mesh.bounds.extents.x, Mathf.Max(mesh.bounds.extents.y, mesh.bounds.extents.z)));

            // For each vertex, project along each K-DOP direction, to find max and min in that direction
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                // We may be dealing with complex GameObjects here, so the root object may have a transform (which
                // will also be applied to the collision mesh and thus is to be ignored), and the children may have
                // a transform too (which is important to end up with a correct collision mesh for the compound
                // object). Let's deal with that.
                Vector3 vertex = vertices[i];
                vertex = meshFilter.transform.TransformPoint(vertex);
                vertex = rootTransform.InverseTransformPoint(vertex);

                for (int j = 0; j < planePairCount; j++)
                {
                    float dist = Vector3.Dot(vertex, dirs[j]);
                    minDist[j] = Mathf.Min(dist, minDist[j]);
                    maxDist[j] = Mathf.Max(dist, maxDist[j]);
                }
            }
        }

        // Generate and split polygons
        List<KDOPPolygon> polys = new List<KDOPPolygon>();
        for (int i = 0; i < planePairCount; i++)
        {
            // Get tangent / bitangent
            Vector3 AxisX, AxisY;
            GenerateTangentSpace(dirs[i], out AxisX, out AxisY);

            // which 0: Positive (maximum) plane
            // which 1: Negative (minimum) plane
            for (int which = 0; which < 2; ++which)
            {
                // Create first polygon
                KDOPPolygon poly = new KDOPPolygon();
                poly.Normal = (which == 0 ? dirs[i] : -dirs[i]);

                if (Vector3.Dot(Vector3.Cross(AxisX, AxisY), poly.Normal) > 0.0f)
                    AxisX *= -1;

                Vector3 baseP = dirs[i] * (which == 0 ? maxDist[i] : minDist[i]);
                poly.Vertices.Add(baseP + AxisX * PlaneExtents + AxisY * PlaneExtents);
                poly.Vertices.Add(baseP + AxisX * PlaneExtents - AxisY * PlaneExtents);
                poly.Vertices.Add(baseP - AxisX * PlaneExtents - AxisY * PlaneExtents);
                poly.Vertices.Add(baseP - AxisX * PlaneExtents + AxisY * PlaneExtents);

                for (int j = 0; j < planePairCount; j++)
                {
                    if (i != j || which == 1) poly.Split(dirs[j] * maxDist[j], -dirs[j]);
                    if (i != j || which == 0) poly.Split(dirs[j] * minDist[j], dirs[j]);
                }

                if (poly.Vertices.Count >= 3)
                    polys.Add(poly);
            }
        }

        return polys;
    }

    // Generate a mesh from a list 
    public static Mesh MeshFromPolygons(List<KDOPPolygon> polys)
    {
        // Generate mesh
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        foreach (KDOPPolygon p in polys)
            p.ToTriangles(vertices, indices);

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);

        return mesh;
    }
}
