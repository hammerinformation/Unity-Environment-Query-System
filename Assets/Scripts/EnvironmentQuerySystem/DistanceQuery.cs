using UnityEngine;

[CreateAssetMenu(fileName = "DistanceQuery", menuName = "Query/DistanceQuery",order = 1)]

public  class DistanceQuery:Query
{

    public float minDistance=0;
    public float maxDistance = 1;
    
}