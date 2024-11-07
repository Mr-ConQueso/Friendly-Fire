using System.Collections.Generic;
using System.Linq;
using Audio;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static AudioController Instance;
    
    // ---- / Public Variables / ---- //
    private IObjectPool<SoundEmitter> _soundEmitterPool;
    private readonly List<SoundEmitter> _activeSoundEmitter = new();
    public readonly Queue<SoundEmitter> FrequentSoundEmitters = new();
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private SoundEmitter _soundEmitterPrefab;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxPoolSize = 100;
    [SerializeField] private int _maxSoundInstances = 30;

    public SoundBuilder CreateSound() => new SoundBuilder(this);

    public bool CanPlaySound(SoundData data)
    {
        if (!data.FrequentSound) return true;

        if (FrequentSoundEmitters.Count >= _maxSoundInstances && FrequentSoundEmitters.TryDequeue(out var soundEmitter))
        {
            try
            {
                soundEmitter.Stop();
                return true;
            }
            catch
            {
                Debug.Log("SoundEmitter is already released");
            }

            return false;
        }

        return true;
    }

    public SoundEmitter Get()
    {
        return _soundEmitterPool.Get();
    }

    public void ReturnToPool(SoundEmitter soundEmitter)
    {
        _soundEmitterPool.Release(soundEmitter);
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        InitializePool();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        foreach (SoundEmitter emitter in _activeSoundEmitter.Where(emitter => emitter.gameObject != null))
        {
            Destroy(emitter.gameObject);
        }

        _activeSoundEmitter.Clear();
    }

    private static void OnDestroyPoolObject(SoundEmitter soundEmitter)
    {
        Destroy(soundEmitter);
    }

    private void OnReturnedToPool(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(false);
        _activeSoundEmitter.Remove(soundEmitter);
    }

    private void OnTakeFromPool(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(true);
        _activeSoundEmitter.Add(soundEmitter);
    }

    private SoundEmitter CreateSoundEmitter()
    {
        SoundEmitter soundEmitter = Instantiate(_soundEmitterPrefab);
        _soundEmitterPrefab.gameObject.SetActive(false);
        return soundEmitter;
    }

    private void InitializePool()
    {
        _soundEmitterPool = new ObjectPool<SoundEmitter>(
            CreateSoundEmitter,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            _collectionCheck,
            _defaultCapacity,
            _maxPoolSize);
    }
}
