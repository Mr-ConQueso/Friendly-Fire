using System;

// This file is auto-generated. Do not modify or move this file.

namespace SuperUnityBuild.Generated
{
    public enum ReleaseType
    {
        None,
        Release,
        Development,
    }

    public enum Platform
    {
        None,
        Linux,
        Android,
        PC,
        WebGL,
    }

    public enum ScriptingBackend
    {
        None,
        IL2CPP,
    }

    public enum Architecture
    {
        None,
        Linux_x64,
        Android,
        Windows_x86,
        Windows_x64,
        WebGL,
    }

    public enum Distribution
    {
        None,
    }

    public static class BuildConstants
    {
        public static readonly DateTime buildDate = new DateTime(638657558974260080);
        public const string version = "1.1.28SSINCE()";
        public const ReleaseType releaseType = ReleaseType.Release;
        public const Platform platform = Platform.WebGL;
        public const ScriptingBackend scriptingBackend = ScriptingBackend.IL2CPP;
        public const Architecture architecture = Architecture.WebGL;
        public const Distribution distribution = Distribution.None;
    }
}

