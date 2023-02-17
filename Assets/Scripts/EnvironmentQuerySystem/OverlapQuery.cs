
using UnityEngine;

[CreateAssetMenu(fileName = "OverlapQuery", menuName = "Query/OverlapQuery",order = 3)]

public  class OverlapQuery:Query
{
    public Vector3 center=Vector3.zero;
    public Vector3 size=Vector3.one;

}