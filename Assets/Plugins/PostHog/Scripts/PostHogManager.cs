using System;
using DotPostHog;
using DotPostHog.Model;
using Plugins.PostHog.Scripts;
using UnityEngine;

public class PostHogManager : MonoBehaviour
{
    public static PostHogManager Instance { get; private set; }

    private IPostHogAnalytics _postHogAnalytics;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure the manager persists across scenes
            InitializePostHog();
        }
        else
        {
            Destroy(gameObject); // Ensure singleton pattern
        }
    }

    private void InitializePostHog()
    {
        try
        {
            var batchConfig = new PostHogEventBatchingConfiguration
            {
                BatchSizeLimit = 1000,
                Period = TimeSpan.FromSeconds(10)
            };
            _postHogAnalytics = PostHogAnalytics.Create(APIKey.PostHogApiKey, batchConfig: batchConfig);
            
            Debug.Log("PostHog Initialized successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error initializing PostHog: " + ex.Message);
        }
    }

    public void CaptureEvent(string eventName, PostHogEventProperties properties = null)
    {
        if (_postHogAnalytics == null) return;

        try
        {
            var eventProperties = properties ?? new PostHogEventProperties();
        
            _postHogAnalytics.Capture(eventName, eventProperties);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error capturing event: " + ex.Message);
        }
    }

    private void OnDestroy()
    {
        if (_postHogAnalytics != null)
        {
            _postHogAnalytics.Flush();
        }
    }
}
