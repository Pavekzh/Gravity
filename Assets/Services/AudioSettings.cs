using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using BasicTools;
using UIExtended;

namespace Assets.Services
{
    public class AudioSettings : MonoBehaviour
    {
        [SerializeField] AudioMixerGroup mixer;
        [SerializeField] BindedToggle musicToggle;
        [SerializeField] BindedToggle sfxToggle;
        [SerializeField] float MusicBaseVolume;
        [SerializeField] float SFXBaseVolume;
       
        protected bool isMusicMuted;
        protected bool isSFXMuted;

        protected Binding<bool> musicToggleBinding = new Binding<bool>();
        protected Binding<bool> sfxToggleBinding = new Binding<bool>();


        public bool IsMusicMuted { get => isMusicMuted; }
        public bool IsSFXMuted { get => isSFXMuted; }

        protected void Start()
        {
            if(musicToggle != null)
                musicToggle.Binding = musicToggleBinding;
            if(sfxToggle != null)
                sfxToggle.Binding = sfxToggleBinding;

            musicToggleBinding.ValueChanged += MusicValueChanged;
            sfxToggleBinding.ValueChanged += SFXValueChanged;

            isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) != 0;
            isSFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) != 0;

            if (isMusicMuted)
                MuteMusic();
            else
                AmplifyMusic();

            if (isSFXMuted)
                MuteSFX();
            else
                AmplifySFX();
        }

        public void ToggleMusic()
        {
            if (isMusicMuted)
                AmplifyMusic();
            else
                MuteMusic();
        }

        public void ToggleSFX()
        {
            if (isSFXMuted)
                AmplifySFX();
            else
                MuteSFX();
        }

        public void MuteMusic()
        {
            muteMusic();
            musicToggleBinding.ChangeValue(false, this);
        } 

        public void MuteSFX()
        {
            muteSFX();
            sfxToggleBinding.ChangeValue(false,this);
        }

        public void AmplifyMusic()
        {
            amplifyMusic();
            musicToggleBinding.ChangeValue(true,this);
        }

        public void AmplifySFX()
        {
            amplifySFX();
            sfxToggleBinding.ChangeValue(true, this);
        }
        
        protected void MusicValueChanged(bool value,object sender)
        {                
            if(sender != (System.Object)this)
            {

                if (value)
                    amplifyMusic();
                else
                    muteMusic();
            }
        }

        protected void SFXValueChanged(bool value,object sender)
        {                
            if (sender != (System.Object)this)
            {

                if (value)
                    amplifySFX();
                else
                    muteSFX();
            }
        }

        protected void muteMusic()
        {
            mixer.audioMixer.SetFloat("MusicVolume", -80f);
            isMusicMuted = true;
            PlayerPrefs.SetInt("MusicMuted", 1);
        }

        protected void muteSFX()
        {
            mixer.audioMixer.SetFloat("UIVolume", -80f);
            isSFXMuted = true;
            PlayerPrefs.SetInt("SFXMuted", 1);
        }

        protected void amplifyMusic()
        {
            mixer.audioMixer.SetFloat("MusicVolume", MusicBaseVolume);
            isMusicMuted = false;
            PlayerPrefs.SetInt("MusicMuted", 0);
        }

        protected void amplifySFX()
        {
            mixer.audioMixer.SetFloat("UIVolume", SFXBaseVolume);
            isSFXMuted = false;
            PlayerPrefs.SetInt("SFXMuted", 0);
        }
    }
}