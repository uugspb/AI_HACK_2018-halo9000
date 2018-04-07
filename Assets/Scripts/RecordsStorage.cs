using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[CreateAssetMenu]
public class RecordsStorage : ScriptableObject
{
//    public List<LemmingRunRecord> Records = new List<LemmingRunRecord>();

    public LemmingRunRecord GetNewRecord()
    {
        return new LemmingRunRecord();
    }
}