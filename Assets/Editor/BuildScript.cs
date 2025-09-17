using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BuildScript
    {
        public static void BuildWindows()
        {
            var args = Environment.GetCommandLineArgs();
            var development = Array.Exists(args, arg => arg == "-development");

            var scenes = EditorBuildSettings.scenes;
            var buildPath = "build/Windows";

            var options = BuildOptions.None;
            if (development)
            {
                options |= BuildOptions.Development;
                options |= BuildOptions.AllowDebugging;
            }

            Directory.CreateDirectory(buildPath);
            BuildPipeline.BuildPlayer(scenes, Path.Combine(buildPath, PlayerSettings.productName),
                BuildTarget.StandaloneWindows64, options);
        }

        public static void BuildAndroid()
        {
            var args = Environment.GetCommandLineArgs();
            var development = Array.Exists(args, arg => arg == "-development");

            var scenes = EditorBuildSettings.scenes;
            var buildPath = "build/Android";

            var options = BuildOptions.None;
            if (development)
            {
                options |= BuildOptions.Development;
                options |= BuildOptions.AllowDebugging;
            }

            Directory.CreateDirectory(buildPath);
            BuildPipeline.BuildPlayer(scenes, Path.Combine(buildPath, PlayerSettings.productName + ".apk"),
                BuildTarget.Android, options);
        }

        public static void BuildiOS()
        {
            var args = Environment.GetCommandLineArgs();
            var development = Array.Exists(args, arg => arg == "-development");

            var scenes = EditorBuildSettings.scenes;
            var buildPath = "build/iOS";

            var options = BuildOptions.None;
            if (development)
            {
                options |= BuildOptions.Development;
            }

            Directory.CreateDirectory(buildPath);
            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.iOS, options);
        }
    }