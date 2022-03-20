using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrCamera : MonoBehaviour
{
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(Target.transform.position.x,Target.transform.position.y,-10f), 0.5f);
    }
}
