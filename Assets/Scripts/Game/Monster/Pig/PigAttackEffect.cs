using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAttackEffect : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //anim.Play("Pig_Attack_Effect");
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAnimEvent()
    {
        //anim.Play("New State");
        gameObject.SetActive(false);
    }
}
