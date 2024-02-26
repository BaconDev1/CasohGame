using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Griddle : MonoBehaviour, B_IInteractable
{
    public GameObject Waffle;
    public GameObject WaffleCooked;
    public GameObject WaffleParent;
    public GameObject WaffleCooked2;
    bool waffleIsOnPan;
    bool IsCooking;
    // Start is called before the first frame update
    void Start()
    {
        waffleIsOnPan = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if(FindObjectOfType<V_GenericPlayer>().IsHoldingWaffle == true && IsCooking == false)
        {

            Destroy(FindObjectOfType<V_GenericPlayer>().InteractionPoint.GetChild(0).gameObject);
            Instantiate(Waffle, WaffleParent.transform.position, WaffleParent.transform.rotation, WaffleParent.transform);
            IsCooking = true;
            waffleIsOnPan = true;
            StartCoroutine(CookTimer());
            

        }
        else
        {
            if(waffleIsOnPan == true)
            {
                FindObjectOfType<V_GenericPlayer>().SpawnWaffleInHand(WaffleCooked2);
                FindObjectOfType<V_GenericPlayer>().IsHoldingWaffle = true;
                Destroy(WaffleParent.transform.GetChild(0).gameObject);
                waffleIsOnPan=false;
                IsCooking=false;
            }
        }
    }

    IEnumerator CookTimer()
    {


        yield return new WaitForSeconds(3);

        Destroy(WaffleParent.transform.GetChild(0).gameObject);
        Instantiate(WaffleCooked, WaffleParent.transform.position, WaffleParent.transform.rotation, WaffleParent.transform);
        
    }
}
