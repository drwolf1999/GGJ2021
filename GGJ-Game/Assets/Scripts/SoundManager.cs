using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Clip
    {
        public string name;
        public AudioClip audioClip;
    }

    public List<Clip> list;
    public Dictionary<string, AudioClip> clipDictionary;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        clipDictionary = new Dictionary<string, AudioClip>();
        foreach(Clip clips in list)
        {
            clipDictionary.Add(clips.name, clips.audioClip);
        }
    }

    public void playSoundEffect(string name)
    {
        audioSource.PlayOneShot(clipDictionary[name], 0.7f);
    }
}
