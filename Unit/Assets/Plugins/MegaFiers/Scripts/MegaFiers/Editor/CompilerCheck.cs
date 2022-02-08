using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using MegaFiers;
using Unity.Collections;

[InitializeOnLoad]
public class CompilerCheck
{
	static CompilerCheck()
	{
#if UNITY_2021_OR_NEWER
#else
		CompilationPipeline.assemblyCompilationStarted += Dispose;
#endif
		CompilationPipeline.assemblyCompilationFinished += Dispose2;
		CompilationPipeline.compilationStarted += Dispose1;
		EditorApplication.playModeStateChanged += LogPlayModeState;
	}

	static void MemoryCleanup()
	{
		MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();

		for ( int i = 0; i < mods.Length; i++ )
		{
			mods[i].ResetCorners();
			mods[i].DisposeArrays();
		}

		MegaWrap[] wraps = GameObject.FindObjectsOfType<MegaWrap>();

		for ( int i = 0; i < wraps.Length; i++ )
			wraps[i].DisposeArrays();
	}

	static void Dispose(string assembly)
	{
		MemoryCleanup();
	}

	static void Dispose2(string assembly, CompilerMessage[] msgs)
	{
		MemoryCleanup();
	}

	static void Dispose1(object obj)
	{
		MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();

		for ( int i = 0; i < mods.Length; i++ )
			mods[i].ResetCorners();
	}

	static void LogPlayModeState(PlayModeStateChange state)
	{
		NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;

		if ( state == PlayModeStateChange.ExitingEditMode )
		{
			//MegaModifyObject.noUpdate = true;
			MegaWrap.IsPlaying = true;
			MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();

			for ( int i = 0; i < mods.Length; i++ )
			{
				mods[i].noUpdate = true;
				mods[i].ResetCorners();
				mods[i].DisposeArrays();
			}

			MegaWrap[] wraps = GameObject.FindObjectsOfType<MegaWrap>();

			for ( int i = 0; i < wraps.Length; i++ )
			{
				wraps[i].canGetMesh = false;
				wraps[i].DisposeArrays();
			}
			return;
		}

		if ( state == PlayModeStateChange.ExitingPlayMode )
		{
			MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();

			for ( int i = 0; i < mods.Length; i++ )
			{
				mods[i].noUpdate = true;
			}
		}

		if ( state == PlayModeStateChange.EnteredEditMode )
		{
			MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();

			for ( int i = 0; i < mods.Length; i++ )
			{
				mods[i].noUpdate = false;
			}
		}

		if ( state == PlayModeStateChange.EnteredPlayMode )
		{
			MegaModifyObject[] mods = GameObject.FindObjectsOfType<MegaModifyObject>();

			for ( int i = 0; i < mods.Length; i++ )
				mods[i].noUpdate = false;

			//MegaModifyObject.noUpdate = false;
			MegaWrap[] wraps = GameObject.FindObjectsOfType<MegaWrap>();

			for ( int i = 0; i < wraps.Length; i++ )
				wraps[i].canGetMesh = true;
		}
		else
			MegaWrap.IsPlaying = false;
	}
}
