using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FootSteps : MonoBehaviour
{
    [SerializeField] private List<AudioClip> footStepsClip;
    [SerializeField]private AudioSource audioSource;


    public void PlayFootStep()
    {
        
        audioSource.clip = footStepsClip[Random.Range(0, footStepsClip.Count)];
        audioSource.Play();
        //audioSource.PlayOneShot(footStepsClip[Random.Range(0, footStepsClip.Count)]);
        Debug.Log("Footstep");
    }
}
