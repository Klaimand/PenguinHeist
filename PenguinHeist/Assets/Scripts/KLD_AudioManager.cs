using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips;
    public AudioMixerGroup group;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [SerializeField] bool canBeOverriden = false;

    [Range(0f, 0.5f)]
    public float randomVolume = 0f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0f;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        if (clips.Length == 1)
        {
            source.clip = clips[0];
        }
        else
        {
            source.clip = clips[Random.Range(0, clips.Length)];
        }
        source.outputAudioMixerGroup = group;
    }

    public AudioSource GetSource()
    {
        return source;
    }

    public void Play()
    {
        if (!source.isPlaying || (source.isPlaying && canBeOverriden))
        {
            if (clips.Length > 1)
            {
                source.clip = clips[Random.Range(0, clips.Length)];
            }
            //source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
            //source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
            source.Play();
        }
    }

    public void Stop()
    {
        source.Stop();
    }

}

public class KLD_AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    [SerializeField] Sound[] sounds;

    Dictionary<string, Sound> soundsKey = new Dictionary<string, Sound>();

    [SerializeField]
    string[] soundsToPlayOnStart;

    Sound[] soundsToAdd;

    public string curMusic;
    
    private void Start()
    {

        for (int i = 0; i < sounds.Length; i++)
        {
            AddSoundToDictionnary(sounds[i], "");
        }

        GetSound("Music").GetSource().loop = true;
        GetSound("Music_GameOver").GetSource().loop = true;
        GetSound("Music_Paused").GetSource().loop = true;
        GetSound("Music_Victory").GetSource().loop = true;
        
        foreach (string sound in soundsToPlayOnStart)
        {
            PlaySound(sound);
        }
    }

    public void AddSoundToDictionnary(Sound _sound, string _prefix)
    {
        GameObject _go = new GameObject("Sound_" + _prefix + _sound.name);
        _go.transform.parent = transform;
        _sound.SetSource(_go.AddComponent<AudioSource>());
        soundsKey.Add(_prefix + _sound.name, _sound);
    }

    public void PlaySound(string _key)
    {
        soundsKey[_key].Play();
        curMusic = _key;
    }

    public Sound GetSound(string _key)
    {
        return soundsKey[_key];
    }

    public void FadeOutInst(AudioSource _source, float time)
    {
        StartCoroutine(FadeOut(_source, time));
    }

    IEnumerator FadeOut(AudioSource _source, float time)
    {
        float curTime = 0f;
        float startVolume = _source.volume;

        while (curTime < time)
        {
            _source.volume = Mathf.Lerp(startVolume, 0f, curTime / time);
            curTime += Time.deltaTime;
            yield return null;
        }
        _source.volume = 0f;

        _source.Stop();
    }

    public void FadeInInst(AudioSource _source, float time)
    {
        StartCoroutine(FadeIn(_source, time));
    }

    IEnumerator FadeIn(AudioSource _source, float time)
    {
        _source.Play();
        float curTime = 0f;
        float endVolume = 0.7f;

        while (curTime < time)
        {
            _source.volume = Mathf.Lerp(0f, endVolume, curTime / time);
            curTime += Time.deltaTime;
            yield return null;
        }
        _source.volume = endVolume;
    }

    public void OutOfMenuMusic()
    {
        StartCoroutine(OutOfMenuMusicCoroutine());
    }

    IEnumerator OutOfMenuMusicCoroutine()
    {
        FadeOutInst(GetSound("MenuMusic").GetSource(), 1.5f);
        yield return new WaitForSeconds(0.5f);
        FadeInInst(GetSound("GameMusic").GetSource(), 1.5f);
    }

    public void OutOfGameMusic()
    {
        StartCoroutine(OutOfGameMusicCoroutine());
    }

    IEnumerator OutOfGameMusicCoroutine()
    {
        FadeOutInst(GetSound("GameMusic").GetSource(), 1.5f);
        yield return new WaitForSeconds(0.5f);
        FadeInInst(GetSound("MenuMusic").GetSource(), 1.5f);
    }

    public void StopAllLoopingSounds(float fadeTime = 1f)
    {
        print("iscalled");
        for (int i = 0; i < sounds.Length; i++)
        {
            if (soundsKey[sounds[i].name].GetSource().loop && soundsKey[sounds[i].name].GetSource().isPlaying)
            {
                soundsKey[sounds[i].name].GetSource().Stop();
                //FadeOutInst(soundsKey[sounds[i].name].GetSource(), fadeTime);
                print("stopped " + sounds[i].name);
            }
        }
    }

    public AudioMixer GetAudioMixer()
    {
        return mixer;
    }

    public void MenuToGame()
    {
        FadeOutInst(GetSound("Theme").GetSource(), 2f);

        FadeInInst(GetSound("Ambiance").GetSource(), 2f);
    }

    public void GameToMenu()
    {
        StopAllLoopingSounds(1.9f);

        FadeInInst(GetSound("Theme").GetSource(), 2f);
    }
}
