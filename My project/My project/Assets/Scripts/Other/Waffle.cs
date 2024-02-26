using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waffle : MonoBehaviour, B_IInteractable
{
    public GameObject Wafflew;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if(FindObjectOfType<V_GenericPlayer>().IsHoldingWaffle == true)
        {

        }
        else
        {
            this.gameObject.SetActive(false);
            FindObjectOfType<V_GenericPlayer>().SpawnWaffleInHand(Wafflew);
            FindObjectOfType<V_GenericPlayer>().IsHoldingWaffle = true;
        }
        
    }
}
