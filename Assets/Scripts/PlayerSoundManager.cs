using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[System.Serializable]
public class Sound {
    public enum SoundType
    {
        SingleShot,
        LongLoop
    }

    public SoundType type;
    public PlayerSoundManager.SoundType soundType;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    [HideInInspector]
    public AudioSource source; // This will be assigned dynamically
}

public class PlayerSoundManager : MonoBehaviour
{
    public enum SoundType
    {
        NO_SOUND,
        Move_Cloud,
        Move_Water,
        Move_Wave,
        PCI_WaterPlants,
        PCI_Collectible,
        PCI_Explosion_Small,
        PCI_Explosion_Mid,
        PCI_Explosion_Large,
        Skill_Super_Jump,
        Skill_Spray,
        Character_Water_Jump,
        Character_Water_Land,
        Character_Water_Move,
        Animal_Sound_01,
        Animal_Sound_02,
        Animal_Sound_03,
        Animal_Sound_04
    }

    public static PlayerSoundManager Instance { get; private set; }

    [SerializeField]
    private Sound[] sounds;

    private Dictionary<SoundType, List<Sound>> soundMap;

    private Unity.Mathematics.Random random;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            random = new Unity.Mathematics.Random();
            
            soundMap = new Dictionary<SoundType, List<Sound>>();
            foreach (Sound sound in sounds)
            {
                GameObject soundGameObject = new GameObject("Sound_" + sound.soundType);
                soundGameObject.transform.SetParent(transform);
                AudioSource source = soundGameObject.AddComponent<AudioSource>();
                source.clip = sound.clip;
                source.volume = sound.volume;
                source.loop = sound.type == Sound.SoundType.LongLoop;
                sound.source = source;

                if (soundMap.ContainsKey(sound.soundType))
                {
                    soundMap[sound.soundType].Add(sound);
                }
                else
                {
                    soundMap.Add(sound.soundType,new List<Sound>(){sound});
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(SoundType soundType)
    {
        print(soundType);
        if (soundMap.TryGetValue(soundType, out List<Sound> soundList))
        {
            soundList[(int)(UnityEngine.Random.value * soundList.Count - 0.001f)].source.Play();
        }
    }

    
    public void PlaySound(String soundName)
    {
        PlaySound((SoundType)Enum.Parse(typeof(SoundType), soundName, true));
    }

    public void StopSound(SoundType soundType)
    {
        if (soundMap.TryGetValue(soundType, out List<Sound> soundList))
        {
            soundList[(int)(UnityEngine.Random.value * soundList.Count - 0.001f)].source.Play();
        }
    }
}
