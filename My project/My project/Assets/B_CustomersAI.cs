using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_CustomersAI : MonoBehaviour, B_IInteractable
{
    public V_GenericPlayer Player;
    bool HasGotFood = false;
    
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
       if(Player.IsHoldingWaffle == true && HasGotFood == false)
        {
            Destroy(Player.InteractionPoint.GetChild(0));
            HasGotFood = true;                                  
        }
    }

}
