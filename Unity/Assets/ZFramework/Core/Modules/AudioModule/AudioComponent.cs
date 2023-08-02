using System;
using UnityEngine;
using System.Collections;
namespace ZFramework.Core {    
    public class AudioComponent : ZeroFrameWorkComponent
    {
        public override Type GetComponentType => typeof(AudioComponent);
        public enum E_AudioType { 
            BGM,
            SF,
            All
        }

        AudioSource SFAudioSource;
        AudioSource BGMAudioSource;
        AudioListener AudioListener;

        protected override void OnInit() {
            SFAudioSource = new GameObject("SFAudioSource").AddComponent<AudioSource>();
            BGMAudioSource = new GameObject("BGMAudioSource").AddComponent<AudioSource>();
            BGMAudioSource.loop = true;

            AudioListener = GameObject.FindObjectOfType<AudioListener>();
        }


        #region API

        public void PlaySF(E_AudioType type, AudioClip clip) 
        {
            if (type == E_AudioType.SF){
                PlayAudioClip(SFAudioSource, clip);
            }
            else if(type == E_AudioType.BGM) {
                PlayAudioClip(BGMAudioSource, clip);
            }            
        }
     
        public void SetVolume(E_AudioType type, float volume) 
        {
            if (type == E_AudioType.SF){
                SFAudioSource.volume = volume;
            }
            else if (type == E_AudioType.BGM) {
                BGMAudioSource.volume = volume;
            }
            else{
                SFAudioSource.volume = volume;
                BGMAudioSource.volume = volume;
            }
        }       

        public void ToggleMusic(bool toggle) {            
            PauseAudioSource(BGMAudioSource,toggle);
        }
        public void ToggleSF(bool toggle) {
            var audioSources = GameObject.FindObjectsOfType<AudioSource>();
            foreach (var item in audioSources)
            {
                if (item == BGMAudioSource) {
                    continue;
                }
                PauseAudioSource(item, toggle);
            }
        }

        
        #endregion

        #region Util
        void PlayAudioClip(AudioSource audioSource, AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

        void PauseAudioSource(AudioSource item,bool toggle) {
            if (!toggle) {
                item.Pause();
                item.mute = true;
            }
            else {
                item.UnPause();
                item.mute = false;
            }
        }
        #endregion
    }

}
