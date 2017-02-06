using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soundButton : MonoBehaviour {

    Animator animator;
    

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        GetComponent<Button>().onClick.AddListener(onClick);
	}
	
	// Update is called once per frame
	void onClick () {
        if (!animator.GetBool("isDisabled"))
            animator.SetBool("isDisabled", true);
        else
            animator.SetBool("isDisabled", false);
    }
}
