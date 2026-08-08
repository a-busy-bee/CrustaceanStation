using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Food : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private Animator hoverAnimator;
    [SerializeField] private GameObject particlesObject;
    [SerializeField] private Animator particlesObjectAnimator;
    [SerializeField] private bool isMedication;
    private Animator animator;
    private bool active = false;
    private bool used = false;

    private void Start()
    {
        hoverAnimator.enabled = false;
        animator = GetComponent<Animator>();

        particlesObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (active) { return; }
        hoverAnimator.enabled = true;
        hoverAnimator.Play("OnHover");
        
        if (isMedication) AudioManager.instance.PlaySound(AudioManager.SoundNames.Pills, true);
        else AudioManager.instance.PlaySound(AudioManager.SoundNames.IsoFood, true);
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (active || used) { return; }
        hoverAnimator.enabled = true;
        used = true;
        //hoverAnimator.Play("StopHover");
        StartCoroutine(LiftBottleAndParticles());
    }

    private IEnumerator LiftBottleAndParticles()
    {
        active = true;
        animator.Play("Lift");
        AudioManager.instance.PlaySound(AudioManager.SoundNames.ObjectLift, true);
        yield return new WaitForSeconds(0.75f);

        if (isMedication) AudioManager.instance.PlaySound(AudioManager.SoundNames.PillsInteraction);
        else AudioManager.instance.PlaySound(AudioManager.SoundNames.FoodInteraction);
            particlesObject.SetActive(true);
        particlesObjectAnimator.Play("Particles");
        yield return new WaitForSeconds(1f);

        animator.Play("PutDown");
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.PlaySound(AudioManager.SoundNames.LightPlace);
        active = false;

        TankSceneManager.instance.FoodConsumed();
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (active) { return; }
        hoverAnimator.enabled = true;
        hoverAnimator.Play("StopHover");

        if (isMedication) AudioManager.instance.PlaySound(AudioManager.SoundNames.Pills, true);
        else AudioManager.instance.PlaySound(AudioManager.SoundNames.IsoFood, true);
    }

    public GameObject GetParticles()
    {
        return particlesObject;
    }
}
