using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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

    [Header("Sound FX")]
    public bool EnableSoundFX = false;
    [ConditionalHide("EnableSoundFX", true)]
    public AudioClip SFX;
    [ConditionalHide("EnableSoundFX", true)]
    public float volume;
    [ConditionalHide("EnableSoundFX", true)]
    public bool fadeIn;
    [ConditionalHide("EnableSoundFX", true)]
    public bool fadeOut;

}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
    //The name of the bool field that will be in control
    public string ConditionalSourceField = "EnableSoundFX";
    //TRUE = Hide in inspector / FALSE = Disable in inspector 
    public bool HideInInspector = false;

    public ConditionalHideAttribute(string conditionalSourceField)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
    }

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
    }
}

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (!condHAtt.HideInInspector || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (!condHAtt.HideInInspector || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
    {
        bool enabled = true;
        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField); //changes the path to the conditionalsource property path
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            enabled = sourcePropertyValue.boolValue;
        }
        else
        {
            Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
        }

        return enabled;
    }
}

public class ElementalMovement : MonoBehaviour
{
    private int currentElementalIndex;
    public int secondElementalIndex;
    public bool secondEnding;
    public ElementalInfo[] elementalInfo;
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
                if (elementalInfo[currentElementalIndex].EnableSoundFX == true)
                {
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
                }

                if (secondEnding == true && elementalInfo[currentElementalIndex].timer > elementalInfo[currentElementalIndex].waitTime)
                {
                    elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, false);
                    currentElementalIndex = secondElementalIndex;
                }

                if (elementalInfo[currentElementalIndex].timer > elementalInfo[currentElementalIndex].waitTime)
                {
                    elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, false);
                    musicHandler.Stop();
                    elementalInfo[currentElementalIndex].timer = 0;
                    currentElementalIndex++;
                    if (secondEnding == true)
                    {
                        secondElementalIndex++;
                    }
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
                if (elementalInfo[currentElementalIndex].EnableSoundFX == true)
                {
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
                }

                if (secondEnding == true && elementalInfo[currentElementalIndex].timer > elementalInfo[currentElementalIndex].waitTime)
                {
                    elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, false);
                    currentElementalIndex = secondElementalIndex;
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
                        if (secondEnding == true)
                        {
                            secondElementalIndex++;
                        }
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