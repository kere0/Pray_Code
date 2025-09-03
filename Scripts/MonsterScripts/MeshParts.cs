using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParts : MonoBehaviour
{
    public Vector3 dir;
    public Vector3 rendDir;
    public Transform spiltBone;
    
    public SkinnedMeshRenderer[] meshRenderers;
    void Start()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    public Vector3 GetClosestMeshSurface(Vector3 hitPoint)
    {
        Vector3 closestPoint = hitPoint;
        Vector3 closestNormal = Vector3.zero;
        float minDist = Mathf.Infinity;

        foreach (var meshrenderer in meshRenderers)
        {
            if(meshrenderer == null) continue;
            Mesh bakedMesh = new Mesh();
            meshrenderer.BakeMesh(bakedMesh);
            
            Vector3[] vertices = bakedMesh.vertices;
            Vector3[] normals = bakedMesh.normals;

            for (int i = 0; i < vertices.Length; i++)
            {
                // 각 정점의 월드 좌표 계산
                Vector3 worldPos = meshrenderer.transform.TransformPoint(vertices[i]);
                float dist = Vector3.Distance(worldPos, hitPoint);

                if (dist < minDist)
                {
                    minDist = dist;
                    closestPoint = worldPos;
                    closestNormal = meshrenderer.transform.TransformDirection(normals[i]);
                    spiltBone = meshrenderer.transform;
                }
            }
        }
        rendDir =  hitPoint - closestPoint.normalized;
        dir = closestNormal;
        return closestPoint;
    }
}
