using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum SoundID
{
    BG_1 = 0,
}

public enum FxID
{
    Button = 0,
    SummonItem = 1,
}


public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] UserData userData;

    private AudioSource soundSource;
    private AudioSource[] fxSource = new AudioSource[Utilities.GetEnumCount<FxID>()];

    [SerializeField] private AudioClip[] soundAus;
    [SerializeField] private AudioClip[] fxAus;

    private bool isLoaded = false;
    private int indexSound;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

        soundSource = gameObject.AddComponent<AudioSource>();
        soundSource.loop = true;
    }

    private void Start()
    {
        Invoke(nameof(OnLoad), 1);
    }

    private void OnLoad()
    {
        if (soundAus.Length > 0)
        {
            isLoaded = true;

            indexSound = Random.Range(0, soundAus.Length);
            PlaySound((SoundID)indexSound);
        }
    }


    public void PlaySound(SoundID ID)
    {
        soundSource.clip = soundAus[(int)ID];
        soundSource.Play();
    }

    public void PlayFx(FxID ID)
    {
        if (DataManager.GameJsonData.information.IsAmbientsSounds && isLoaded)
        {
            if (fxSource[(int)ID] == null)
            {
                fxSource[(int)ID] = new GameObject().AddComponent<AudioSource>();
                fxSource[(int)ID].clip = fxAus[(int)ID];
                fxSource[(int)ID].loop = false;
                fxSource[(int)ID].transform.SetParent(transform);
            }
            fxSource[(int)ID].PlayOneShot(fxAus[(int)ID]);

            //Debug.Log(ID);
        }
    }

    public void ChangeSound(SoundID ID, float time)
    {
        
    }

}
