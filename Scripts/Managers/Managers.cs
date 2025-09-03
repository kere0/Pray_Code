using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    private static bool _Init;
    
    private PoolManager poolManager = new PoolManager();
    private CSVLoader csvLoader = new CSVLoader();
    private DataManager dataManager = new DataManager();
    private SaveManager saveManager = new SaveManager();
    
    public static PoolManager PoolManager {get {return Instance.poolManager;} }
    public static CSVLoader CSVLoader { get { return Instance.csvLoader; } }
    public static DataManager DataManager { get { return Instance.dataManager; } }
    public static SaveManager SaveManager { get { return Instance.saveManager; } }
    public static Managers Instance
    {
        get
        {
            if (_Init == false)
            {
                _Init = true;
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject("@Managers");
                    go.AddComponent<Managers>();
                }
                DontDestroyOnLoad(go);
                _instance = go.GetComponent<Managers>();
            }
            return _instance;
        }
    }
    // 씬전환될때 클리어 해주기
    public void Clear()
    {
        SaveManager.Clear();
        SaveSnapshot.Instance.Clear();
        PoolManager.pools.Clear();
        DataManager.DataTableClear();
        PlayerSkill.Instance.Clear();
        CombatSystem.Instance.creatureDic.Clear();
        Inventory.Instance.Clear();
    }
}
