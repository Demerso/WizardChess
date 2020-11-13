using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource s;
    public AudioClip[] songs;
    int index = 0;
    public static MusicPlayer mp;
    private void Awake()
    {
        s = gameObject.AddComponent<AudioSource>();
        
        s.volume = 0.3f;
        index = Random.Range(0, songs.Length - 1);
        s.clip = songs[index];
        s.Play();

    }

    private void musicPlay()
    {
        if (s.isPlaying) //need to chek if the current clip is playing
        {
            s.clip = songs[index];
            s.Play();
        }
        else
        {
            s.clip = songs[index];
        }
        Debug.Log(index);


    }   
    public void ChangeState()
    {
        if (s.isPlaying)
        {
            s.Pause();
        }
        else
        {

            s.Play();
        }
    }
    
    public void Next()
    {
        index = (index + 1) % songs.Length; 
        musicPlay();
    }
    public void Prev()
    {
        index = (index + songs.Length -1) % songs.Length;
        musicPlay();
    }

    
}
