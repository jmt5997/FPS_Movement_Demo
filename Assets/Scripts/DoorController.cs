using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    public Transform hinge;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useDoor()
    {
        if(isOpen){
            hinge.transform.Rotate(0, 90f, 0, Space.Self);
        }
        else
        {
            hinge.transform.Rotate(0, -90f, 0, Space.Self);
        }
        isOpen = !isOpen;
    }

}
