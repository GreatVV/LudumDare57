﻿using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace DCFApixels
{
    public static class DebugXConsts
    {
        public const float IMMEDIATE_DURATION = -1;
        public const float DOT_SIZE = 0.05f;

        public readonly static bool IsSRP = GraphicsSettings.currentRenderPipeline != null;
        public readonly static bool IsSupportsComputeShaders = SystemInfo.supportsComputeShaders;
    }
    public static partial class DebugX
    {
        internal const MethodImplOptions LINE = MethodImplOptions.AggressiveInlining;

        private const float DEFAULT_DURATION = 0f;
        private static readonly Color DEFAULT_COLOR = Color.white;

        private const string GLOBAL_TIME_SCALE_PREF_NAME = "DCFApixels.DebugX.TimeScale";
        private const string GLOBAL_DOT_SIZE_PREF_NAME = "DCFApixels.DebugX.DotSize";
        private const string GLOBAL_COLOR_PREF_NAME = "DCFApixels.DebugX.Color";

        private readonly static int GlobalDotSizePropertyID = Shader.PropertyToID("_DebugX_GlobalDotSize");
        private readonly static int GlobalColorPropertyID = Shader.PropertyToID("_DebugX_GlobalColor");
        internal readonly static int ColorPropertyID = Shader.PropertyToID("_Color");



        private enum DebugXPauseState
        {
            Unpaused = 0,
            PreUnpaused = 1, //нужно чтобы отщелкунть паузу с задержкой в один тик
            Paused = 2,
        }
    }
    public enum DebugXLine
    {
        Default,
        Arrow,
        Fade,
    }
}
namespace DCFApixels.DebugXCore
{
    public static class DebugXDefines
    {
        public const bool DISABLE_DEBUGX_INBUILD =
#if DISABLE_DEBUGX_INBUILD
            true;
#else
            false;
#endif
    }
}