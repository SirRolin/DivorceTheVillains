using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathing : MonoBehaviour
{
    public POICollections POIC;
    private NavMeshAgent agent;
    [SerializeField]
    private float idletimeRemaining = 0.0f;
    public float idletimeMin = 0;
    public float idletimeMax = 10;
    [SerializeField]
    private Animator Animator;

    private const string isWalking = "isWalking";


    // Chooses a new destination
    private void SelectNewDestination(){
        idletimeRemaining = -10000;
        if (POIC!=null){
            agent.SetDestination(POIC.GetRandomPOI().position);
            Animator.SetBool(isWalking, true);
        }
    }
    private void AssignIdletime(){
        idletimeRemaining = Random.Range(idletimeMin, idletimeMax);
        Animator.SetBool(isWalking, false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()    
    {
        if(TryGetComponent<NavMeshAgent>(out var NMA)){
            agent = NMA;
        }
        else{
            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.stoppingDistance = 0.5f;
            agent.acceleration = 50;
            agent.speed = 1.2f;
        }
        SelectNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance<=agent.stoppingDistance){
            if (idletimeRemaining<=-10000){
                AssignIdletime();
               

            }    
            else if(idletimeRemaining<=0){
                SelectNewDestination();
                
            }
            else{
                idletimeRemaining -= Time.deltaTime;
            }
            
        }
        
    }
}
