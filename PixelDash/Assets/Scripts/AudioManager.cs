using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    // Use this for initialization

    public AudioSource BGM;
    public AudioSource OK;
    public AudioSource ERROR;

    void Start()
    {

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public  void OpenBGM()
    {
        if (!BGM.isPlaying)
        {
            BGM.Play();
        }
       
    }

    public  void CloseBGM()
    {

        if (BGM.isPlaying)
        {
            BGM.Stop();
        }
    }

    public void PlayAudioOK()
    {

        if (!OK.isPlaying)
        {
            OK.Play();
         
        }
    }

    public void PlayAudioError()
    {
        if (!ERROR.isPlaying)
        {
            ERROR.Play();
        }

    }


}
