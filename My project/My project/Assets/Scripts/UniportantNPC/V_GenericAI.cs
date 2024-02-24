using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; //tf how did this spawn in? 

//using System.Drawing; //obsolete
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;


public class V_GenericAI : MonoBehaviour
{

    [Header("AI Settings")]
    [Space(5)]
    public float rangeOfRandomPatrol; //radius of sphere
    public float ChaseSpeed;
    public float PatrolSpeed;
    public float FollowPointSpeed;
    public float QuoteDelay;
    public float RotateSmoothTime = 0.1f;

    [Header("Refrences")]
    [Space(5)]
    public NavMeshAgent agent;
    public Animator animator;
    public Transform centrePoint; //centre of the area the agent wants to move around in
    [Header("Music And Sounds")]
    [Space(5)]
    public AudioSource source;
    public AudioSource Musicsource;
    public List<AudioClip> LowClips;
    public List<AudioClip> ChaseClips;
    [Header("AI Behaviour")]
    [Space(5)]
    //public V_AIStates currentState; //SCRAPED
    public bool RandomPatrol;
    public bool ChasePlayer;
    public bool FollowPoint;

    #region Private
    [HideInInspector] public bool StopCompletly = false; //called if you want to disamble all AI functions
    [HideInInspector] public GameObject Player;
    [HideInInspector] public bool CanMove = true; //called if you want the ai to stop
    private AudioClip LowHealthSound; //CaseOhs screechs
    private AudioClip ChaseMusicSound;
    private bool isChaseCalled = true;
    private float AngularVelocity = 0.0f;

    #endregion
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        InvokeRepeating("Quotes", 0f, QuoteDelay);

    }


    void Update()
    {

        //Refrences
        Player = GameObject.FindWithTag("Player");

        float distance = Vector3.Distance(transform.position, Player.transform.position);


        //AI Randomly patrols around the are you set
        if (RandomPatrol && StopCompletly == false)
        {
            isChaseCalled = true;
            CancelInvoke("ChaseMusic");
            animator.SetBool("FoundPlayer", false);
            if (agent.remainingDistance <= agent.stoppingDistance) //done with path
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, rangeOfRandomPatrol, out point)) //pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //Debug shit
                    agent.SetDestination(point);
                }
            }
        }

        //AI Chases down the player untill he is close enough to him
        if (ChasePlayer && StopCompletly == false)
        {
            if (isChaseCalled)
            {
                Invoke("ChaseMusic", 0f);
                isChaseCalled = false;
            }
            Debug.DrawRay(Player.transform.position, Vector3.up, Color.red, 1.0f); //Debug shit
            if (CanMove)
            {
                animator.SetBool("FoundPath", false);
                animator.SetBool("FoundPlayer", true);
                var target_rot = Quaternion.LookRotation(Player.transform.position - transform.position);
                var delta = Quaternion.Angle(transform.rotation, target_rot);
                if (delta > 0.0f)
                {
                    var t = Mathf.SmoothDampAngle(delta, 0.0f, ref AngularVelocity, RotateSmoothTime);
                    t = 1.0f - t / delta;
                    transform.rotation = Quaternion.Slerp(transform.rotation, target_rot, t);
                    agent.SetDestination(Player.transform.position);
                }
                animator.Play("Run");
                agent.speed = ChaseSpeed;
            }
            if (distance < agent.stoppingDistance)
            {
                //when he in close range stop (you can call any function you want here) maybe punch a player?
                animator.SetBool("CloseToPlayer", true);
                CanMove = false;
                animator.Play("ThrowBack");
            }
            else
            {
                animator.SetBool("CloseToPlayer", false);
                CanMove = true;
            }
        }

        //AI Follows a point on the map (UNFINISHED)
        if (FollowPoint && StopCompletly == false) //unfinished
        {
            animator.SetBool("FollowPath", true);
            isChaseCalled = true;
            CancelInvoke("ChaseMusic");
            agent.speed = FollowPointSpeed;
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            agent.speed = PatrolSpeed;
            animator.SetBool("FoundPath", true);
            animator.Play("Walk");
            //logic for smooth rotation
            var target_rot = Quaternion.LookRotation(randomPoint - transform.position);
            var delta = Quaternion.Angle(transform.rotation, target_rot);
            if (delta > 0.0f)
            {
                var t = Mathf.SmoothDampAngle(delta, 0.0f, ref AngularVelocity, RotateSmoothTime);
                t = 1.0f - t / delta;
                transform.rotation = Quaternion.Slerp(transform.rotation, target_rot, t);
            }
            result = hit.position;
            return true;
        }
        animator.SetBool("FoundPath", false);
        animator.Play("Idle");
        result = Vector3.zero;
        return false;
    }
    void Quotes()
    {
        LowHealthSound = LowClips[Random.Range(0, LowClips.Count)];
        source.clip = LowHealthSound;
        source.Play();
    }
    void ChaseMusic()
    {
        ChaseMusicSound = ChaseClips[Random.Range(0, ChaseClips.Count)];
        Musicsource.clip = ChaseMusicSound;
        Musicsource.Play();
    }
}
