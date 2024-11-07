using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    public SoundData data { get; private set;  }
    private AudioSource _audioSource;
    private Coroutine _playingCoroutine;

    private void Awake()
    {
        _audioSource = gameObject.GetOrAddComponent<AudioSource>();
    }

    public void Play()
    {
        if (_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
        }
        
        _audioSource.Play();
        _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
    }

    public void Initialize(SoundData incomingData)
    {
        this.data = incomingData;
        _audioSource.clip = incomingData.Clip;
        _audioSource.outputAudioMixerGroup = incomingData.MixerGroup;
        _audioSource.loop = incomingData.Loop;
        _audioSource.playOnAwake = incomingData.PlayOnAwake;
    }

    public void Stop()
    {
        if (_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
            _playingCoroutine = null;
        }
        _audioSource.Stop();
        AudioController.Instance.ReturnToPool(this);
    }

    private IEnumerator WaitForSoundToEnd()
    {
        yield return new WaitWhile(() => _audioSource.isPlaying);
        AudioController.Instance.ReturnToPool(this);
    }

    public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
    {
        _audioSource.pitch += Random.Range(min, max);
    }
}