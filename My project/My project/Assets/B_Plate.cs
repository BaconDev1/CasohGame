using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Plate : MonoBehaviour, B_IInteractable
{
    public GameObject Waffle;
    public GameObject WaffleSpawnPoint;
    public bool IsOnPlate = false;
    public V_GenericPlayer Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<V_GenericPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void Interact()
    {
        if (FindObjectOfType<V_GenericPlayer>().IsHoldingWaffle == true && IsOnPlate == false)
        {
            Instantiate(Waffle, WaffleSpawnPoint.transform.position, Quaternion.identity, WaffleSpawnPoint.transform);
            Destroy(FindObjectOfType<V_GenericPlayer>().InteractionPoint.GetChild(0).gameObject);
            Player.IsHoldingWaffle = false;
            IsOnPlate=true;
            
        }else if(FindObjectOfType<V_GenericPlayer>().IsHoldingWaffle == false && IsOnPlate == true)
        {
            this.transform.parent = FindObjectOfType<V_GenericPlayer>().InteractionPoint;
            this.transform.localPosition = Vector3.zero;
            Player.IsHoldingWaffle = true;
        }
        else
        {

        }
    }
}
