using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HingeJointListener : MonoBehaviour
{
    AudioSource audio;
    private bool soundPlay = false;

    // how many degrees from min/max the hinge has to be 
    // before it is detected as min or max.
    [SerializeField]
    private float angleThreshold = 1f;


    [SerializeField]
    private HingeJointState hingeJointState = HingeJointState.Between;


    [SerializeField]
    private UnityEvent OnMinLimitReached;


    [SerializeField]
    private UnityEvent OnMaxLimitReached;


    [SerializeField]
    private UnityEvent OnBetweenReached;

    private enum HingeJointState { Min, Max, Between }

    // assign your target hinge joint here
    [SerializeField]
    private HingeJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        // try to get hinge joint if it is not assigned
        if(joint == null)
        {
            joint = GetComponent<HingeJoint>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // check the hinge joint angle and create event based on the angle   

        // first get the difference between the current angle and min/max angles of the hinge
        float distanceToMin = Mathf.Abs(joint.angle - joint.limits.min);
        float distanceToMax = Mathf.Abs(joint.angle - joint.limits.max);

        // reached min?
        if(distanceToMin < angleThreshold)
        {
            // invoke UnityEvent, but do it only once
            if(hingeJointState != HingeJointState.Min)
            {
                if (soundPlay)
                {
                    soundPlay = false;
                }
                OnMinLimitReached.Invoke();
            }
            hingeJointState=HingeJointState.Min;
        }
        // reached max?
        else if (distanceToMax < angleThreshold)
        {
            // invoke UnityEvent, but do it only once
            if (hingeJointState != HingeJointState.Max)
            {
                if (soundPlay)
                {
                    soundPlay = false;
                }
                OnMaxLimitReached.Invoke();
            }
            hingeJointState = HingeJointState.Max;
        }
        // between min and max
        else
        {
            if (hingeJointState != HingeJointState.Between)
            {
                if (!soundPlay)
                {
                    audio.Play();
                    soundPlay = true;
                }
                OnBetweenReached.Invoke();
            }
            hingeJointState = HingeJointState.Between;

        }

    }
}
