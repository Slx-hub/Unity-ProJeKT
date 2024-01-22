using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> Sounds;
    private static SoundManager Instance;

    private void Start()
    {
        Instance = this;
    }

    public static AudioClip GetSoundByName(string name)
    {
        return Instance.Sounds.Find(s => s.name == name);
    }
}
