using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string name;
    public float volume = 0.3f;
    public float pitch = 1f;

    public AudioSource s;
}
