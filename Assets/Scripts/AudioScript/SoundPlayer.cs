using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundPlayer:MonoBehaviour
{
    public Sound[] sounds;
    public static SoundPlayer sp;
    private void Awake()
    {
        if(sp == null)
        {
            sp = this;
        }
        foreach( var sound in sounds)
        {
            sound.s =gameObject.AddComponent<AudioSource>();
            sound.s.clip = sound.clip;
            sound.s.volume = sound.volume;
            sound.s.pitch = sound.pitch;
        
        }
    }
    
    public void Play(string name)
    {
        
        var son = Array.Find(sounds, sound => sound.name == name);
        if(son == null)
        {
            Debug.Log("name");
        }
        else
        {
            
            son.volume = AudioListener.volume;
            son.s.Play();
        }

    }
}
