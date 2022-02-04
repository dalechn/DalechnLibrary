using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_control : MonoBehaviour
{
    public int animation_num;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = this.GetComponent<Animator>();

        this.animator.SetBool("animation_" + this.animation_num, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
