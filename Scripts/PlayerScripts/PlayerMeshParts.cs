using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeshParts : MonoBehaviour
{
    private Vector3 dir;
    public SkinnedMeshRenderer[] meshRenderers;  // 여러 개를 배열로 넣기

    private void Start()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    public Vector3 GetClosestMeshSurface(Vector3 hitPoint)
    {
        Vector3 closestPoint = hitPoint;
        Vector3 closestNormal = Vector3.zero;  // 가장 가까운 법선 벡터
        float minDist = Mathf.Infinity;

        foreach (var renderer in meshRenderers)
        {
            if (renderer == null) continue;

            Mesh bakedMesh = new Mesh();
            renderer.BakeMesh(bakedMesh);
    
            Vector3[] vertices = bakedMesh.vertices;
            Vector3[] normals = bakedMesh.normals;  // 법선 벡터 배열
            
            for (int i = 0; i < vertices.Length; i++)
            {
                // 각 정점의 월드 좌표 계산
                Vector3 worldPos = renderer.transform.TransformPoint(vertices[i]);
                float dist = Vector3.Distance(worldPos, hitPoint);

                if (dist < minDist)
                {
                    minDist = dist;
                    closestPoint = worldPos;
                    closestNormal = renderer.transform.TransformDirection(normals[i]);  // 해당 정점의 법선 벡터
                }
            }
        }
        dir = closestNormal.normalized;

        return closestPoint + dir * 0.1f;
    }

    private void OnPartDestroyed()
    {
        Debug.Log($"{gameObject.name} 파괴됨!");
        // 파괴 연출 or FSM 이벤트 전송 가능
    }
}