using System.Collections;
using System.Collections.Generic;
using Improbable.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class BinbagStoneFX : MonoBehaviour {

	public AudioClip stoneSound;
    public AudioClip rubbishSound;

	[Require]
	private ClientAuthorityCheck.Writer authorityCheck;

	private AudioSource stoneAudioSource;
    private AudioSource rubbishAudioSource;

	private void OnEnable()
	{
		LoadAudio();
	}

	private void OnDisable()
	{
		if (stoneAudioSource != null)
		{
			stoneAudioSource.Stop();
			Destroy(stoneAudioSource);
		}
		if (rubbishAudioSource != null)
		{
			rubbishAudioSource.Stop();
			Destroy(rubbishAudioSource);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
        if (authorityCheck != null && other.tag == "StoneWtf")
		{
            stoneAudioSource.Play();
		}
		else if (authorityCheck != null && other.tag == "Rubbish")
		{
			rubbishAudioSource.Play();
		}
	}

	private void LoadAudio()
	{
		stoneAudioSource = gameObject.AddComponent<AudioSource>();
		stoneAudioSource.clip = stoneSound;
		stoneAudioSource.loop = false;

		rubbishAudioSource = gameObject.AddComponent<AudioSource>();
		rubbishAudioSource.clip = rubbishSound;
		rubbishAudioSource.loop = false;
	}
}
