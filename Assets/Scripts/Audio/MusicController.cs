using UnityEngine;

public class MusicController : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private SoundData _musicData;
    
    private void Start()
    {
        AudioController.Instance.CreateSound()
            .WithSoundData(_musicData)
            .WithRandomPitch(false)
            .WithPosition(transform.position)
            .Play();
    }
}
