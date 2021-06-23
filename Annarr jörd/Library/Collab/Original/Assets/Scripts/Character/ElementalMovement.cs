using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ElementalBehaviorType { Static, MoveToPosition };

[Serializable]
public class ElementalInfo
{
    public string Description;
    public ElementalBehaviorType behavior;
    public Transform startPostion;
    public Transform endPostion;
    public Transform target;
    public float speed;
    public float timer;
    public float waitTime;
    public Animator anim;
    public string anima;
    public AudioClip SFX;
    public float volume;
    public bool fadeIn;
    public bool fadeOut;
    
}

public class ElementalMovement : MonoBehaviour
{
    public ElementalInfo[] elementalInfo;
    private int currentElementalIndex;
    private Rigidbody rb;
    private AudioSource musicHandler;
    public Vector3 offset;

    void Start()
    {
        currentElementalIndex = 0;
        transform.position = elementalInfo[currentElementalIndex].startPostion.position;
        rb = GetComponent<Rigidbody>();
        musicHandler = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (currentElementalIndex < elementalInfo.Length)
        {
            if (elementalInfo[currentElementalIndex].behavior == ElementalBehaviorType.Static)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                transform.position = elementalInfo[currentElementalIndex].startPostion.position;
                transform.LookAt(elementalInfo[currentElementalIndex].target);
                elementalInfo[currentElementalIndex].timer += Time.deltaTime;
                elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, true);
                // Sound Effects
                if (!musicHandler.isPlaying)
                {
                    musicHandler.PlayOneShot(elementalInfo[currentElementalIndex].SFX,
                        elementalInfo[currentElementalIndex].volume);
                    // Fade In
                    if (elementalInfo[currentElementalIndex].fadeIn == true)
                    {
                        StartCoroutine(FadeAudioSource.StartFade(musicHandler,
                        2f, 0, elementalInfo[currentElementalIndex].volume, 0));
                    }
                    // Fade Out
                    if (elementalInfo[currentElementalIndex].fadeOut == true)
                    {
                        StartCoroutine(FadeAudioSource.StartFade(musicHandler,
                        2f, elementalInfo[currentElementalIndex].volume, 0, elementalInfo[currentElementalIndex].waitTime - 2f));
                    }
                }

                if (elementalInfo[currentElementalIndex].timer > elementalInfo[currentElementalIndex].waitTime)
                {
                    elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, false);
                    musicHandler.Stop();
                    elementalInfo[currentElementalIndex].timer = 0;
                    currentElementalIndex++;
                    if (currentElementalIndex < elementalInfo.Length)
                    {
                        transform.position = elementalInfo[currentElementalIndex].startPostion.position;
                    }
                }
            }
            else if (elementalInfo[currentElementalIndex].behavior == ElementalBehaviorType.MoveToPosition)
            {
                rb.constraints = ~RigidbodyConstraints.FreezeAll;
                transform.LookAt(elementalInfo[currentElementalIndex].target);
                transform.position = Vector3.MoveTowards(transform.position, elementalInfo[currentElementalIndex].endPostion.position - offset,
                                                         elementalInfo[currentElementalIndex].speed * Time.deltaTime);
                elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, true);
                // Sound Effects
                if (!musicHandler.isPlaying)
                {
                    musicHandler.PlayOneShot(elementalInfo[currentElementalIndex].SFX,
                        elementalInfo[currentElementalIndex].volume);
                    // Fade In
                    if (elementalInfo[currentElementalIndex].fadeIn == true)
                    {
                        StartCoroutine(FadeAudioSource.StartFade(musicHandler,
                        2f, 0, elementalInfo[currentElementalIndex].volume, 0));
                    }
                    // Fade Out
                    if (elementalInfo[currentElementalIndex].fadeOut == true)
                    {
                        StartCoroutine(FadeAudioSource.StartFade(musicHandler,
                        2f, elementalInfo[currentElementalIndex].volume, 0, elementalInfo[currentElementalIndex].waitTime - 2f));
                    }
                }

                if (Vector3.Distance(transform.position, elementalInfo[currentElementalIndex].endPostion.position) < 1f)
                {
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, false);
                    musicHandler.Stop();
                    elementalInfo[currentElementalIndex].timer += Time.deltaTime;
                    if (elementalInfo[currentElementalIndex].timer > elementalInfo[currentElementalIndex].waitTime)
                    {
                        elementalInfo[currentElementalIndex].timer = 0;
                        currentElementalIndex++;
                        if (currentElementalIndex < elementalInfo.Length)
                        {
                            transform.position = elementalInfo[currentElementalIndex].startPostion.position;
                        }
                    }
                }
            }
        }

    }

    public static class FadeAudioSource
    {

        public static IEnumerator StartFade(AudioSource audioSource, float duration, float start, float targetVolume, float timeRemaining)
        {
            yield return new WaitForSeconds(timeRemaining);
            float currentTime = 0;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }
    }
}
