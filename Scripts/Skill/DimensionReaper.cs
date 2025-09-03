using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DimensionReaper : MonoBehaviour
{
    public GameObject portal;
    public GameObject portalCenter;
    //private Collider[] colliders;
    private List<Collider> colliderList = new List<Collider>();
    private List<Transform> monsterTransformList = new List<Transform>();
    
    public bool portalOn = false;
    private bool isStartCoroutine = false;
    float collectTime;

    Dictionary<Transform, List<Transform>> monsterToParts = new Dictionary<Transform, List<Transform>>();

    void Awake()
    {
        portal = transform.GetChild(0).GetChild(1).gameObject;
        portalCenter = portal.transform.GetChild(4).GetChild(0).gameObject;
    }
    private void Update()
    {
        if (portalOn)
        {
            portal.SetActive(true);
            if (isStartCoroutine == false)
            {
                isStartCoroutine = true;
                StartCoroutine(EffectTimeCheckCoroutine());
            }
            colliderList.Clear();
            monsterTransformList.Clear();
            if (collectTime <= 2f)
            {
                collectTime+= Time.deltaTime;
                // 몬스터 등록
                Collider[] colliders = Physics.OverlapSphere(portal.transform.position, 3f);
                foreach (var col in colliders)
                {
                    if (col.CompareTag("Monster"))
                    {
                        IDamageAble monster = col.GetComponent<IDamageAble>();
                        if (monster != null && monster.NormalMonster)
                        {
                            //Transform root = col.transform.GetChild(1).root; // 본체 루트

                            if (!monsterToParts.ContainsKey(col.transform))
                            {
                                List<Transform> parts = new List<Transform>();
                                parts.AddRange(col.transform.GetChild(1).GetComponentsInChildren<Transform>());
                                parts.RemoveAt(0);
                                col.GetComponent<NormalMonsterBase>().agent.isStopped = true;
                                monsterToParts.Add(col.transform, parts);
                            }
                        }
                    }
                }
            }
            // 몬스터 파츠들을 포탈 중심으로 이동
            foreach (var kvp in monsterToParts)
            {
                foreach (Transform part in kvp.Value)
                {
                    part.position = Vector3.Slerp(part.position, portalCenter.transform.position, Time.deltaTime * 2f);
                }
            }
           // 거리에 가까워 졋는지 체크
           List<Transform> monstersToRemove = new List<Transform>();

            foreach (var kvp in monsterToParts)
            {
                bool allClose = true;
                foreach (Transform part in kvp.Value)
                {
                    if (Vector3.Distance(part.position, portalCenter.transform.position) > 1f)
                    {
                        Debug.Log(Vector3.Distance(part.position, portalCenter.transform.position));
                        allClose = false;
                        break;
                    }
                }

                if (allClose)
                {
                    Debug.Log("도달~~~~~~~~~~~~~~~~");
                    monstersToRemove.Add(kvp.Key);
                    kvp.Key.GetChild(1).transform.position = portalCenter.transform.position;
                }
            }
           // 삭제 
            foreach (Transform monsterRoot in monstersToRemove)
            {
                IDamageAble monster = monsterRoot.GetComponent<IDamageAble>();
                if (monster != null)
                {
                    Memory_Mission_Controller.Instance.Notify(monster.TargetID);
                }
                if (CircleManager.Instance.circleMonsterList.Contains(monster.Collider))
                {
                    CircleManager.Instance.CircleMonsterRemove(monster.Collider);
                }
                Destroy(monsterRoot.gameObject);
                monsterToParts.Remove(monsterRoot.transform);
            }
        }
    }
    IEnumerator EffectTimeCheckCoroutine()
    {
        while (true)
        {
            int effectEnd = 0;
            ParticleSystem[] patricle = gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var p in patricle)
            {
                if (p.IsAlive(true))
                {
                    effectEnd++;
                }
            }
            if (effectEnd == 0) break;
            yield return null;
        }
        portal.SetActive(false);
        portalOn = false;
        Managers.PoolManager.ObjPush(gameObject, gameObject.name); ;
        isStartCoroutine = false;
        monsterToParts.Clear();
        collectTime = 0f;
    }
}
