using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface B_IInteractable
{
    public void Interact();
}

public class B_Intoractor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if(Physics.Raycast(r, out RaycastHit hitinfo, InteractRange))
            {
                if(hitinfo.collider.gameObject.TryGetComponent(out B_IInteractable interactObj))
                {
                    interactObj.Interact();
                    Debug.Log("Interact");
                }
            }
        }
    }
}
