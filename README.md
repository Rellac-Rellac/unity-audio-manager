# unity-audio-manager
ScriptableObject Architecture implemented in an Audio Manager 

This manager will allow your designer to make an audio call from anywhere in the project with minimal input. Using a grouping system, you can specify a number of audio clips to correspond to a specific key.

It will also allow an easy way to manage volume channels with PlayerPrefs integration

# Sound Manager
---------------
Create > Audio > Sound Manager

To start using the sound manager, you must first populate the "Clip Groups" listed in the ScriptableObject. The id listed on each of these entries is what you will call when you want to play the sound. A mixer output group can be specified for the outgoing clips. Each entry has an array of AudioClips referenced - the manager will choose from this list randomly, leave just a single entry for a specific clip every time.

The clips can be called via SoundManager.PlayOneShot(string) and SoundManager.PlayLooping(string) respectively. Call SoundManager.StopLooping(string) to stop a looping clip.

A Vector3/Transform may also be specified in the Play functions to allow a sound to be centered on a position/follow a transform

# Volume Manager
-----------------
Create > Audio > Volume Manager

To properly use the volume manager, you should call VolumeManager.Initialise() before any audio calls are made. This will read from PlayerPrefs and apply any volume adjustments needed.

The Volume Manager ScriptableObject will have an array of "Volume Groups" listed. Each entry has an id specified to call whenever a volume adjustment is made and an array of mixer params to adjust that are associated with volume parameters. A default is supplied in the example, but it's important to expose the volume parameter of the mixer group you wish to adjust and give it an appropriate name to reference in the mixer params of the volume group entries.

To change the volume level of your listed mixer params, call SetVolumeLevel(string, float). It's recommended to keep this float a value from 0-1

# Volume Slider
--------------

The VolumeSlider script is used to replace the script of any default Unity Canvas Slider. It will work in the same way, but will update its value on start and will update the volume level when adjusted.

# Audio Entity
--------------

Due to the way coroutines and ScriptableObjects don't play well nicely together, it's important that there is a Monobehaviour to reference from the ScriptableObject. This script exists solely to expose a Monobehaviour to the Sound Manager without the need to add an otherwise useless object to the scene (this is attached to the instantiated AudioSources)


