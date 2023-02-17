using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


public class EnvironmentQuerySystem : MonoBehaviour
{
    [field: Header("Grid"), Space] public int x = 5;
    public int y = 5;
    public float dist = 5f;
    
    [field: Header("Raycast"), Space] 
    [SerializeField] private bool enableRaycastQuery;
    public RaycastQuery raycastQuery;
    
    [field: Header("DistanceQuery"), Space] 
    [SerializeField]
    private bool enableDistanceQuery;
    public DistanceQuery distanceQuery;
    
    [field: Header("OverlapQuery"), Space] 
    [SerializeField] private bool enableOverlapQuery;
    public OverlapQuery overlapQuery;
    
    [field: Header("DotQuery"), Space] 
    [SerializeField] private bool enableDotQuery;
    public DotQuery dotQuery;
    
    [HideInInspector] public List<GameObject> items = new();


    private void Generate()
    {
        
        var eqs = new GameObject("EQS");

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                g.transform.position = new Vector3((i * dist), 0, (j * dist));
                g.transform.SetParent(eqs.transform);
                g.AddComponent<EqsItem>();
                EqsItem eqsItem = g.GetComponent<EqsItem>();
                eqsItem.EnableEqsItem(this, enableRaycastQuery, enableDistanceQuery, enableOverlapQuery, enableDotQuery);
                eqsItem.AddAllQueries(raycastQuery, distanceQuery, overlapQuery, dotQuery);
                g.GetComponent<Collider>().isTrigger = true;
                items.Add(g);
                g.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        eqs.transform.SetParent(this.transform);
        eqs.transform.localScale = Vector3.one;

        var xPos = (x * dist) - dist;
        var yPos = (y * dist) - dist;
        
        eqs.transform.localPosition = new Vector3(-xPos / 2f, 0, -yPos / 2f);
    }

    private void Start()
    {
        Generate();
    }


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR


        for (int j = 0; j < items.Count; j++)
        {
            UnityEditor.Handles.color = Color.white;
            GUIStyle style = new GUIStyle();
            style.fontSize = 15;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;
            EqsItem eqsItem = items[j].GetComponent<EqsItem>();
            float value = eqsItem.score;
            Gizmos.color = value == 1f ? Color.green :
                value == 0.75f ? Color.yellow :
                value == 0.50f ? Color.white :
                value == 0.25f ? Color.blue : Color.red;
            Gizmos.DrawSphere(items[j].transform.position,1f);
            UnityEditor.Handles.Label(items[j].transform.position + Vector3.up * 1.5f, value.ToString(CultureInfo.InvariantCulture), style);
        }
#endif
    }
}