using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPrinter : MonoBehaviour
{
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject grade2FX;
    [SerializeField] private GameObject grade3Fx;
    [SerializeField] private GameObject traitFX;
    [SerializeField] private GameObject explosionFX;
    [SerializeField] private GameObject victoryFX;
    [SerializeField] private GameObject textFX;
    [SerializeField] private Transform canvas;

    private Vector3 hitOffset;
    private Vector3 textOffset;

   
    public void PrintHitFX(Vector3 target)
    {
        GameObject temp = Instantiate(hitFX);
        temp.transform.forward = Camera.main.transform.forward;
        hitOffset = Random.insideUnitCircle * 0.5f;
        hitOffset.y += 1f;
        //hitOffset.z -= 1f;
        temp.transform.position = target + hitOffset;
        temp.SetActive(true);
    }
    public void PrintGradeFX(Transform target, int grade)
    {
        GameObject temp;
        if(grade == 2)
        {
            temp = Instantiate(grade2FX);
        }
        else
        {
            temp = Instantiate(grade3Fx);
        }
        temp.transform.parent = target;
        Vector3 pos = target.position;
        pos.y += 0.1f;
        temp.transform.position = pos;
    }
    public void PrintTraitFX(Transform target)
    {
        GameObject temp;
        temp = Instantiate(traitFX);
        temp.transform.parent = target;
        Vector3 pos = target.position;
        pos.y += 0.1f;
        temp.transform.position = pos;
    }
    public void PrintExplosionFX(Transform target)
    {
        GameObject temp;
        temp = Instantiate(explosionFX);
        Vector3 pos = target.position;
        pos.y += 0.5f;
        temp.transform.position = pos;
    }
    public void PrintVictoryFX(Transform target)
    {
        victoryFX.transform.position = target.position;
        victoryFX.SetActive(true);
    }
    public void PrintTextFX(string value, Vector3 target, TextType type)
    {
        GameObject temp = Instantiate(textFX, canvas);

        textOffset = Random.insideUnitCircle * 15f;
        Vector2 pos = Camera.main.WorldToScreenPoint(target);
        pos.x += textOffset.x;
        pos.y += 50f + textOffset.y;

        temp.GetComponent<TextFX>().InitText(value, pos, type);
    }
}
