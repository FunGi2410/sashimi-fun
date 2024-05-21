using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class WalkState : StateMachineBehaviour
{
    public static event Action<Customer> AddLineUpCustomerEvent;
    NavMeshAgent agent;
    bool isAddLineUpCustomer = false;
    bool isGoHome = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.agent = animator.GetComponent<NavMeshAgent>();
        /*if (animator.gameObject.GetComponent<Customer>().state == CustomerState.walking)
        {
            
            *//*this.agent = animator.GetComponent<NavMeshAgent>();
            GameObject go = GameObject.FindGameObjectWithTag("WayPoints");
            foreach (Transform point in go.transform)
                this.wayPoints.Add(point);

            this.agent.SetDestination(this.wayPoints[indexPoint].position);
            indexPoint++;*//*
            
            this.agent.SetDestination(this.entrancePos.position);
        }*/
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.agent.remainingDistance <= this.agent.stoppingDistance)
        {
            //if (animator.gameObject.GetComponent<Customer>().state != CustomerState.walking) return;
            /*if (indexPoint < wayPoints.Count)
                this.agent.SetDestination(this.wayPoints[indexPoint++].position);
            else if(indexPoint >= wayPoints.Count && !isAddLineUpCustomer)
            {
                isAddLineUpCustomer = true;
                AddLineUpCustomerEvent.Invoke(animator.gameObject.GetComponent<Customer>());
            }*/
            //animator.SetBool("isWalking", false);
            if (!isAddLineUpCustomer)
            {
                isAddLineUpCustomer = true;
                AddLineUpCustomerEvent.Invoke(animator.gameObject.GetComponent<Customer>());
            }
            else animator.SetBool("isWalking", false);
        }

        /*this.timer += Time.deltaTime;
        if (this.timer > 10f)
            animator.SetBool("isWalking", false);*/
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.agent.SetDestination(agent.transform.position);

        if (animator.gameObject.GetComponent<Customer>().state == CustomerState.finish)
            Destroy(animator.gameObject, 1f);

        //AddLineUpCustomerEvent.Invoke(animator.gameObject.GetComponent<Customer>());

        /*this.wayPoints[wayPoints.Count - 1].position = new Vector3(this.wayPoints[wayPoints.Count - 1].position.x,
            this.wayPoints[wayPoints.Count - 1].position.y,
            this.wayPoints[wayPoints.Count - 1].position.z - 7f);*/
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
