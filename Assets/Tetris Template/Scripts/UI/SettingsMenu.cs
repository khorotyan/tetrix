using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject soundCross;

    public void TurnUpDownSound(bool isLoading = false)
    {
        if (AudioListener.volume == 0)
        {
            soundCross.SetActive(false);
            AudioListener.volume = 1.0f;
            Managers.Audio.PlayUIClick();
            if (!isLoading)
                GameController.Instance.OnSoundPreferenceChange(true);
        }
        else if (AudioListener.volume == 1.0f)
        {
            soundCross.SetActive(true);
            AudioListener.volume = 0f;
            if (!isLoading)
                GameController.Instance.OnSoundPreferenceChange(false);
        }
    }

    public void OpenFacebookPage()
    {
        Application.OpenURL(Constants.FACEBOOK_URL);
    }

    public void OpenTwitterPage()
    {
        Application.OpenURL(Constants.TWITTER_URL);
    }

    public void OpenContact()
    {
        //Application.OpenURL(Constants.CONTACT_URL);
    }

    public void RateAsset()
    {
        Application.OpenURL(Constants.ASSETSTORE_URL);
    }
}
