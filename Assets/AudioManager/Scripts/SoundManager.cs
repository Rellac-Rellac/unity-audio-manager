using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
namespace Rellac.Audio
{
	[CreateAssetMenu(fileName = "New Sound Manager", menuName = "Audio/Sound Manager", order = 1)]
	public class SoundManager : ScriptableObject
	{
		[SerializeField] private ClipGroup[] clipGroups = null;

		private Dictionary<string, AudioSource> loopingGroups = new Dictionary<string, AudioSource>();

		/// <summary>
		/// Determine if a group is already looping
		/// </summary>
		/// <param name="clipGroup">group id to check</param>
		/// <returns>true if group is already looping</returns>
		public bool IsLooping(string clipGroup)
		{
			return loopingGroups.ContainsKey(clipGroup);
		}

		/// <summary>
		/// Play a single audio clip one time from a specific clip group
		/// </summary>
		/// <param name="clipGroup">group to find random AudioClip in</param>
		public void PlayOneShot(string clipGroup)
		{
			PlayOneShotAudio(clipGroup);
		}

		/// <summary>
		/// Play a looping audio clip from a specific clip group
		/// </summary>
		/// <param name="clipGroup">group to find random AudioClip in</param>
		public void PlayLooping(string clipGroup)
		{
			PlayLoopingAudio(clipGroup);
		}

		/// <summary>
		/// Play a single audio clip one time from a specific clip group
		/// Targetted at a specific location
		/// </summary>
		/// <param name="clipGroup">group to find random AudioClip in</param>
		/// <param name="position">position to center audio</param>
		public AudioSource PlayOneShotAudio(string clipGroup, Vector3 position)
		{
			AudioSource source = PlayOneShotAudio(clipGroup);
			source.transform.position = position;
			return source;
		}

		/// <summary>
		/// Play a single audio clip one time from a specific clip group
		/// Targetted at a specific Transform
		/// </summary>
		/// <param name="clipGroup">group to find random AudioClip in</param>
		/// <param name="target">Target for audio to follow</param>
		public AudioSource PlayOneShotAudio(string clipGroup, Transform target)
		{
			AudioSource source = PlayOneShotAudio(clipGroup);
			source.transform.SetParent(target);
			source.transform.localPosition = Vector3.zero;
			return source;
		}

		/// <summary>
		/// Play a single audio clip one time from a specific clip group
		/// </summary>
		/// <param name="clipGroup">group to find random AudioClip in</param>
		public AudioSource PlayOneShotAudio(string clipGroup)
		{
			for (int i = 0; i < clipGroups.Length; i++)
			{
				if (clipGroups[i].id == clipGroup)
				{
					int idx = Random.Range(0, clipGroups[i].clips.Length);
					AudioSource source = PlayOneShot(clipGroups[i].clips[idx]);
					source.outputAudioMixerGroup = clipGroups[i].mixer;
					return source;
				}
			}
			Debug.LogError("Tried to get an AudioClip from unspecified group: " + clipGroup);
			return null;
		}

		/// <summary>
		/// Play a looping audio clip from a specific clip group
		/// Targetted at a specific location
		/// </summary>
		/// <param name="clipGroup">group to find random AudioClip in</param>
		/// <param name="position">position to center audio</param>
		public AudioSource PlayLoopingAudio(string clipGroup, Vector3 position)
		{
			AudioSource source = PlayLoopingAudio(clipGroup);
			source.transform.position = position;
			return source;
		}

		/// <summary>
		/// Play a looping audio clip from a specific clip group
		/// Targetted at a specific Transform
		/// </summary>
		/// <param name="clipGroup">group to find random AudioClip in</param>
		/// <param name="target">Target for audio to follow</param>
		public AudioSource PlayLoopingAudio(string clipGroup, Transform target)
		{
			AudioSource source = PlayLoopingAudio(clipGroup);
			source.transform.SetParent(target);
			source.transform.localPosition = Vector3.zero;
			return source;
		}

		/// <summary>
		/// Play a looping audio clip from a specific clip group
		/// </summary>
		/// <param name="clipGroup">group to find random AudioClip in</param>
		public AudioSource PlayLoopingAudio(string clipGroup)
		{
			for (int i = 0; i < clipGroups.Length; i++)
			{
				if (clipGroups[i].id == clipGroup)
				{
					int idx = Random.Range(0, clipGroups[i].clips.Length);
					AudioSource source = PlayLooping(clipGroup, clipGroups[i].clips[idx]);
					source.outputAudioMixerGroup = clipGroups[i].mixer;
					if (loopingGroups.TryGetValue(clipGroups[i].id, out AudioSource value))
					{
						Destroy(value.gameObject);
						loopingGroups[clipGroups[i].id] = source;
					}
					else
					{
						loopingGroups.Add(clipGroups[i].id, source);
					}
					return source;
				}
			}
			Debug.LogError("Tried to get an AudioClip from unspecified group: " + clipGroup);
			return null;
		}

		/// <summary>
		/// Stop a looping clip group
		/// </summary>
		/// <param name="clipGroup">group to stop</param>
		public void StopLooping(string clipGroup)
		{
			foreach (string key in loopingGroups.Keys)
			{
				if (key == clipGroup)
				{
					if (loopingGroups[key] != null) {
						Destroy(loopingGroups[key].gameObject);
					}
				}
			}
			loopingGroups.Remove(clipGroup);
		}

		/// <summary>
		/// Play a single audio clip one time
		/// </summary>
		/// <param name="clip">clip to play</param>
		private AudioSource PlayOneShot(AudioClip clip)
		{
			GameObject go = new GameObject("AudioOneShot: " + clip.name);
			AudioSource source = go.AddComponent<AudioSource>();
			source.PlayOneShot(clip);

			DontDestroyOnLoad(go);
			go.AddComponent<AudioEntity>().StartCoroutine(DestroyClipGO(source, clip.length));
			return source;
		}

		private IEnumerator DestroyClipGO(AudioSource input, float time)
		{
			yield return new WaitForSeconds(time);
			if (input != null)
			{
				Destroy(input.gameObject);
			}
		}
		/// <summary>
		/// Play a looping audio clip
		/// </summary>
		/// <param name="clip">clip to play</param>
		private AudioSource PlayLooping(string clipGroup, AudioClip clip)
		{
			GameObject go = new GameObject("AudioLooping: " + clip.name);
			AudioSource source = go.AddComponent<AudioSource>();
			source.PlayOneShot(clip);

			DontDestroyOnLoad(go);
			go.AddComponent<AudioEntity>().StartCoroutine(LoopClip(clipGroup, source, clip));
			return source;
		}

		private IEnumerator LoopClip(string clipGroup, AudioSource input, AudioClip clip)
		{
			yield return new WaitForSeconds(clip.length);
			if (loopingGroups.ContainsKey(clipGroup))
			{
				PlayLoopingAudio(clipGroup).outputAudioMixerGroup = input.outputAudioMixerGroup;
			}
			if (input != null)
			{
				Destroy(input.gameObject);
			}
		}

		[System.Serializable]
		private struct ClipGroup
		{
			public string id;
			public AudioMixerGroup mixer;
			public AudioClip[] clips;
		}
	}
}