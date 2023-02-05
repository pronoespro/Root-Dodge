using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAndOut : MonoBehaviour
{

    [SerializeField] private SpriteRenderer rend;
    
    [SerializeField]private float FadeInTime=1,FadeTimeOffset,FadeOutTime=1;
    [SerializeField] private float AlphaOut = 0, AlphaIn = 1;

    private Coroutine cor;

    private void Start()
    {
        rend =rend!=null? rend: GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (cor != null){
            StopCoroutine(cor);
            cor = null;
        }
        cor = StartCoroutine(FadeIn());
    }

    private void OnDisable()
    {
        if (cor != null)
        {
            StopCoroutine(cor);
        }
    }

    private IEnumerator FadeIn()
    {
        float _timer = 0;
        while (_timer < FadeInTime)
        {

            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, Mathf.Lerp(AlphaOut, AlphaIn,_timer/FadeInTime));
            _timer += Time.deltaTime;

            yield return null;

        }
        _timer = 0;
        while (_timer < FadeTimeOffset)
        {
            _timer += Time.deltaTime;
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, AlphaIn);
            yield return null;
        }

        _timer = 0;
        while (_timer < FadeOutTime)
        {

            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, Mathf.Lerp(AlphaOut, AlphaIn, 1-_timer / FadeOutTime));
            _timer += Time.deltaTime;

            yield return null;

        }
    }

}
