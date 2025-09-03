using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class MonsterSpawner : MonoBehaviour
// {
//     private GameObject kryll;
//     private float spawnTime;
//     void Awake()
//     {
//         kryll = Resources.Load<GameObject>("Kryll/Kryll");
//         Managers.PoolManager.ObjInit(kryll, 10);
//     }
//
//     private void Start()
//     {
//         Managers.PoolManager.ObjPop(kryll.name, transform.position);
//     }
//
//     void Update()
//     {
//         spawnTime += Time.deltaTime;
//         if (spawnTime >= 60f)
//         {
//             Managers.PoolManager.ObjPop(kryll.name, transform.position);
//             spawnTime = 0f;
//         }
//     }
// }
