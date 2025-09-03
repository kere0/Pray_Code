using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonsterHpBar : MonoBehaviour
{
    private GameObject monster;
    public Slider slider;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 worldPos = Camera.main.WorldToScreenPoint(monster.transform.position + new Vector3(0f, 2f, 0f));
        slider.transform.position = worldPos;
    }
}
