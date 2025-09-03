using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    [System.Serializable]
    // 딕셔너리의 Key, Value 리스트에 값 넣어줌
    public class SerializableDict<Tkey, TValue>
    {
        // FromJosn 할때 값 다 들어옴
        [SerializeField] List<Tkey> keys = new List<Tkey>();
        [SerializeField] List<TValue> values = new List<TValue>();

        public SerializableDict(Dictionary<Tkey, TValue> dict)
        {
            foreach (var dic in dict)
            {
                keys.Add(dic.Key);
                values.Add(dic.Value);
            }
        }

        // 나눴던 Key, Value 딕셔너리로 다시 합침
        public Dictionary<Tkey, TValue> ToDictionary() 
        {
            Dictionary<Tkey, TValue> dict = new Dictionary<Tkey, TValue>();
            if (keys == null || values == null) return new Dictionary<Tkey, TValue>();
            for (int i = 0; i < keys.Count; i++)
            {
                dict[keys[i]] = values[i];
                Debug.Log(dict[keys[i]]);
            }
            return dict;
        }

        public void Clear()
        {
            if (keys == null || values == null) return;
            keys.Clear();
            values.Clear();
        }
    }
}
    