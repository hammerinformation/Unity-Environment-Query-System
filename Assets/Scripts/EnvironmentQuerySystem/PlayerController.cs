
using UnityEngine;

public class PlayerController : MonoBehaviour
{



    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");


        transform.Translate(h / 8f, 0, v / 8f);
        
    }
}