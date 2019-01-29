using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimationManager : MonoBehaviour
{

    [SerializeField]
    private Animator myAnimator;

    void Start () {
        SetFloat ("MyFloat",1f);
        StartCoroutine (WaitForSetTrigger ());
    }


    private IEnumerator WaitForSetTrigger () {
        yield return new WaitForSeconds (2f);
        SetTrigger ("MyTrigger");
    }

    private void SetFloat (string floatName, float value) {
        myAnimator.SetFloat (floatName , value);
    }

    private void SetTrigger (string triggerName) {
        myAnimator.SetTrigger (triggerName);
    }

}
