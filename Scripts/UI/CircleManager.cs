using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleManager : MonoBehaviour
{
    public static CircleManager Instance;
    public List<Collider> circleMonsterList = new List<Collider>();
    public List<Collider> executionCircleMonsterList = new List<Collider>();

    public Action<Collider, Vector3> ExecutionAction;

    public void Awake()
    {
        Instance = this;
        ExecutionAction -= OnExecutionCircle;
        ExecutionAction += OnExecutionCircle;
    }
    public void OnExecutionCircle(Collider collider, Vector3 position)
    {
        if (executionCircleMonsterList.Count > 0)
        {
            Managers.PoolManager.ObjPop("ExecuteCircle", position);
            StartCoroutine(ExecuteListRemove(collider));
        }
    }

    public void ExecuteCircleAdd(Collider collider)
    {
        executionCircleMonsterList.Add(collider);
    }
    public void CircleMonsterAdd(Collider collider)
    {
        circleMonsterList.Add(collider);
    }
    public void CircleMonsterRemove(Collider collider)
    {
        circleMonsterList.Remove(collider);
    }
    public Collider GetClosestMonster()
    {
        if (circleMonsterList.Count == 0) return null;

        Collider closest = null;
        float minDistance = float.MaxValue;

        foreach (Collider monster in circleMonsterList)
        {
            if (monster == null) continue;

            float distance = Vector3.Distance(Player.Instance.transform.position, monster.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = monster;
            }
        }
        return closest;
    }
    public Collider GetClosestExecuteMonster()
    {
        if (executionCircleMonsterList.Count == 0) return null;

        Collider closest = null;
        float minDistance = float.MaxValue;

        foreach (Collider monster in executionCircleMonsterList)
        {
            if (monster == null) continue;

            float distance = Vector3.Distance(Player.Instance.transform.position, monster.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = monster;
            }
        }
        return closest;
    }
    public IEnumerator ExecuteListRemove(Collider collider)
    {
        yield return new WaitForSeconds(1f);
        if (executionCircleMonsterList.Contains(collider))
        {
            executionCircleMonsterList.Remove(collider);
        }
    }
}
