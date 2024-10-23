using System.Collections.Generic;
using DotPostHog.Model;
using UnityEngine;

namespace Plugins.PostHog.Scripts
{
    public class SendEvents : MonoBehaviour
    {
        private void Start()
        {
            PostHogManager.Instance.CaptureEvent("GameStarted", new PostHogEventProperties(new Dictionary<string, object> { { "level", 1 } }));
        }
    }
}