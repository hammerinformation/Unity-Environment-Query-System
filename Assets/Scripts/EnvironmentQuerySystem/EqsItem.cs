using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EqsItem : MonoBehaviour
{
    private bool environmentQueryEnabled;

    public float score;
    [SerializeField] private bool enableRaycastQuery;
    [SerializeField] private bool enableDistanceQuery;
    [SerializeField] private bool enableOverlapQuery;
    [SerializeField] private bool enableDotQuery;

    private RaycastQuery raycastQuery;
    private DistanceQuery distanceQuery;
    private OverlapQuery overlapQuery;
    private DotQuery dotQuery;

    private GameObject eqsGameObject;


    private List<string> tagList = new();
    public bool detected = false;
    public bool bDistance = false;
    public bool bOverlap = false;
    public bool bRaycast = false;
    public bool bDot = false;
    public float dot;
    [SerializeField]private List<bool> boolList = new(){};


    private void OnEnable()
    {
        
        int layerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = layerIgnoreRaycast;
    }

    public void EnableEqsItem(EnvironmentQuerySystem environmentQuerySystem, bool enableRaycastQuery,
        bool enableDistanceQuery, bool enableOverlapQuery,bool enableDotQuery)
    {
        
        this.enableRaycastQuery = enableRaycastQuery;
        
        this.enableDistanceQuery = enableDistanceQuery;
       

   

        this.enableOverlapQuery = enableOverlapQuery;

        this.enableDotQuery = enableDotQuery;

        eqsGameObject = environmentQuerySystem.gameObject;

        environmentQueryEnabled = this.enableDistanceQuery || this.enableRaycastQuery || this.enableOverlapQuery||this.enableDotQuery;
    }

    public void AddAllQueries(RaycastQuery raycastQuery, DistanceQuery distanceQuery, OverlapQuery overlapQuery,DotQuery dotQuery)
    {
        this.raycastQuery = raycastQuery;
        this.distanceQuery = distanceQuery;
        this.overlapQuery = overlapQuery;
        this.dotQuery = dotQuery;
        GetComponent<BoxCollider>().size = overlapQuery.size;
        GetComponent<BoxCollider>().center = overlapQuery.center;
    }

    private void AddTag(string tag)
    {
        if (tagList.Contains(tag) == false)
            tagList.Add(tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enableOverlapQuery)
        {
            return;
        }

        var tag = other.gameObject.tag;
        if (tagList.Contains(tag) || tagList.Count == 0)
        {
            bOverlap = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bOverlap = false;
    }
    public static explicit operator float(EqsItem eqsItem) => eqsItem.score;

    private void Update()
    {
        if (environmentQueryEnabled)
        {
            if (enableDistanceQuery)
            {
                var distance = Vector3.Distance(eqsGameObject.transform.position, transform.position);
                if (distance > distanceQuery.minDistance && distance < distanceQuery.maxDistance)
                {
                    bDistance = true;
                }
                else
                {
                    bDistance = false;

                }
            }


            if (enableRaycastQuery)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, eqsGameObject.transform.position - transform.position, out hit,
                        raycastQuery.maxDistance))
                {

                    if (hit.collider.gameObject == eqsGameObject)
                    {
                        bRaycast = true;
                    }
                    else
                    {
                        bRaycast = false;

                    }
                }
            }


            if (enableDotQuery)
            {
                Vector3 forward = eqsGameObject.transform.TransformDirection(Vector3.forward);
                Vector3 toOther = transform.position - eqsGameObject.transform.position;

                 dot = Vector3.Dot(forward, toOther.normalized);
                if (dot >= dotQuery.minValue && dot <= dotQuery.maxValue)
                {
                    bDot = true;
                }
                else
                {
                    bDot = false;
                }
               
            }
            
            
            boolList.Clear();
            if(enableDistanceQuery){boolList.Add(bDistance);}
            if(enableRaycastQuery){boolList.Add(bRaycast);}
            if(enableOverlapQuery){boolList.Add(bOverlap);}
            if(enableDotQuery){boolList.Add(bDot);}
            
            score = (boolList.Count(c => c))/(float)boolList.Count;
            score = (float)Math.Round(score, 2);


        }
    }
}