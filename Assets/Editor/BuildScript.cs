using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    public static void BuildWindows()
    {
        // Read the -development custom parameter (presence is enough)
        var args = Environment.GetCommandLineArgs();
        bool development = Array.Exists(args, a => a == "-development");

        // Use only enabled scenes
        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        // Ensure output directory exists
        var buildPath = Path.Combine("build", "Windows");
        Directory.CreateDirectory(buildPath);

        // IMPORTANT: include .exe in the filename
        var exeName = PlayerSettings.productName + ".exe";
        var locationPathName = Path.Combine(buildPath, exeName);

        // Build options
        var options = BuildOptions.None;
        if (development)
        {
            options |= BuildOptions.Development | BuildOptions.AllowDebugging;
        }

        // Build and validate
        var report = BuildPipeline.BuildPlayer(scenes, locationPathName, BuildTarget.StandaloneWindows64, options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new Exception($"Windows build failed: {report.summary.result}");
        }

        Debug.Log($"Built Windows player: {locationPathName}");
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

        // Set the build target to Android
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        // For development builds, use APK
        if (development)
        {
            EditorUserBuildSettings.buildAppBundle = false;
            Directory.CreateDirectory(buildPath);
            BuildPipeline.BuildPlayer(scenes, Path.Combine(buildPath, PlayerSettings.productName + ".apk"),
                BuildTarget.Android, options);
        }
        // For release builds, use AAB
        else
        {
            EditorUserBuildSettings.buildAppBundle = true;
            Directory.CreateDirectory(buildPath);
            BuildPipeline.BuildPlayer(scenes, Path.Combine(buildPath, PlayerSettings.productName + ".aab"),
                BuildTarget.Android, options);
        }
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