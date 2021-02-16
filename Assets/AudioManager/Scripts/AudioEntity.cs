using UnityEngine;
/// <summary>
/// We need a Monobehaviour to run a coroutine when
/// an AudioSource is instantiated, so this empty
/// script is just here to reference the MonoBehaviour
/// MonoBehaviour can't be added directly to a GameObject
/// See:
/// SoundManager.PlayOneShot(AudioClip clip)
/// SoundManager.PlayLooping(AudioClip clip)
/// </summary>
namespace Rellac.Audio
{
	public class AudioEntity : MonoBehaviour
	{
	}
}
