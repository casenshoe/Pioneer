using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicScript : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource music;
    public AudioMixer mixer;
    public AudioClip[] songs;
    public AudioSource actionMusic;
    public Slider musicSlider;
    public Slider sfxSlider;

    public float fadeDuration = 2.0f; // Time to fade between tracks

    public bool isFadingToAction = false;
    public bool isFadingToBackground = false;

    void Start()
    {
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            //Set first time opening to false
            //PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);

            //Do your stuff here
            PlayerPrefs.SetFloat("Music Volume", 0.5f);
            PlayerPrefs.SetFloat("SFX Volume", 0.5f);
        }

        int num = Random.Range(0, 3);
        music.clip = songs[num];
        mixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("Music Volume")) * 20);
        musicSlider.value = PlayerPrefs.GetFloat("Music Volume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume");
        actionMusic.volume = 0.0f;
        actionMusic.Play();
    }

    public IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void FadeToActionMusic()
    {
        if (!isFadingToAction)
        {
            StopAllCoroutines();
            StartCoroutine(FadeMusic(music, actionMusic));
            isFadingToAction = true;
            isFadingToBackground = false;
        }
    }

    public void FadeToBackgroundMusic()
    {
        if (!isFadingToBackground)
        {
            StopAllCoroutines();
            StartCoroutine(FadeMusic(actionMusic, music));
            isFadingToBackground = true;
            isFadingToAction = false;
        }
    }

    private IEnumerator FadeMusic(AudioSource from, AudioSource to)
    {
        float timer = 0.0f;
        float initialVolumeFrom = from.volume;
        float initialVolumeTo = to.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            from.volume = Mathf.Lerp(initialVolumeFrom, 0.0f, progress);
            to.volume = Mathf.Lerp(initialVolumeTo, 1.0f, progress);

            yield return null;
        }

        from.volume = 0.0f;
        to.volume = 1.0f;

        // Ensure only one track is playing fully
        from.Stop();
        to.Play();
    }

    public void onMusicValueChanged()
    {
        PlayerPrefs.SetFloat("Music Volume", musicSlider.value);
        PlayerPrefs.Save();
        mixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("Music Volume")) * 20);
    }

    public void onSFXValueChanged()
    {
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("SFX Volume", sfxSlider.value);
    }
}
