using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
	[System.Serializable]
	public struct SceneMusic
	{
		public AudioClip audio;
		public string sceneName;
	}
	public SceneMusic[] musics;

	private AudioSource audioPlayer;
	private string lastScene;
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);

		audioPlayer = GetComponent<AudioSource>();

		playMusic(SceneManager.GetActiveScene().name);
		lastScene = SceneManager.GetActiveScene().name;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// called second
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		playMusic(scene.name);
		lastScene = scene.name;
	}
	void playMusic(string name)
	{
		if (lastScene == name) return;
		foreach (SceneMusic music in musics)
		{
			if (music.sceneName == name)
			{
				audioPlayer.clip = music.audio;
				audioPlayer.Stop();
				audioPlayer.Play();
			}
		}
	}
}
