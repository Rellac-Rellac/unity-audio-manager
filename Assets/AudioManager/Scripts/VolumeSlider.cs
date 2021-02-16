using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Rellac.Audio
{
	[RequireComponent(typeof(Slider))]
	public class VolumeSlider : MonoBehaviour
	{
		public string volumeGroupId;
		public VolumeSliderEvent onValuechanged;

		private Slider slider_;
		private Slider slider
		{
			get
			{
				if (slider_ == null) slider_ = GetComponent<Slider>();
				return slider_;
			}
		}

		// Start is called before the first frame update
		void Start()
		{
			if (PlayerPrefs.HasKey("VolumeGroupLevel_" + volumeGroupId))
			{
				slider.SetValueWithoutNotify(PlayerPrefs.GetFloat("VolumeGroupLevel_" + volumeGroupId));
			}
			else
			{
				slider.SetValueWithoutNotify(1);
			}
			slider.onValueChanged.AddListener((volume) => ValueChanged(volumeGroupId, volume));
		}

		private void ValueChanged(string id, float volume)
		{
			onValuechanged.Invoke(id, volume);
		}

		[System.Serializable]
		public class VolumeSliderEvent : UnityEvent<string, float>
		{
		}
	}
}