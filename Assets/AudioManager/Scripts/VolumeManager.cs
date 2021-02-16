using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace Rellac.Audio
{
	[CreateAssetMenu(fileName = "New Volume Manager", menuName = "Audio/Volume Manager", order = 1)]
	public class VolumeManager : ScriptableObject
	{
		public AudioMixer mixer;
		[SerializeField] private VolumeGroup[] volumeGroups = new VolumeGroup[0];
		private Dictionary<string, VolumeGroup> groupDictionary = new Dictionary<string, VolumeGroup>();

		public void Initialise()
		{
			for (int i = 0; i < volumeGroups.Length; i++)
			{
				if (PlayerPrefs.HasKey("VolumeGroupLevel_" + volumeGroups[i].id))
				{
					volumeGroups[i].volume = PlayerPrefs.GetFloat("VolumeGroupLevel_" + volumeGroups[i].id);
				}
				else
				{
					volumeGroups[i].volume = 1;
				}
				groupDictionary.Add(volumeGroups[i].id, volumeGroups[i]);
				for (int j = 0; j < volumeGroups[i].mixerParams.Length; j++)
				{
					UpdateMixer(volumeGroups[i].volume, volumeGroups[i].mixerParams[j]);
				}
			}
		}

		public void SetVolumeLevel(string id, float input)
		{
			if (groupDictionary.TryGetValue(id, out VolumeGroup group))
			{
				group.volume = input;
				PlayerPrefs.SetFloat("VolumeGroupLevel_" + id, input);
				for (int i = 0; i < group.mixerParams.Length; i++)
				{
					UpdateMixer(input, group.mixerParams[i]);
				}
				return;
			}
			Debug.LogError("Invalid Volume Group: " + id);
		}

		private void UpdateMixer(float volume, string mixerParam)
		{
			var dbVolume = Mathf.Log10(volume) * 20;
			if (volume <= 0.0f)
			{
				dbVolume = -80.0f;
			}

			mixer.SetFloat(mixerParam, dbVolume);
		}

		[System.Serializable]
		private struct VolumeGroup
		{
			public string id;
			public string[] mixerParams;
			[HideInInspector]
			public float volume;
		}
	}
}