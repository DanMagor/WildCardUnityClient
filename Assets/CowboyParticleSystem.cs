using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CowboyParticleSystem : MonoBehaviour
{
    private Vector3 defaultScale;
    private Vector3 defaultPosition;

    private Sequence sequence;

    public float appearanceTime = 0.05f;
    public float flightTime = 1f;
    public float flightDistance = 1.5f;

    public TextMesh textMesh;
    void Awake()
    {
        defaultScale = transform.localScale;
        defaultPosition = transform.position;
        textMesh = GetComponent<TextMesh>();
    }
    // Start is called before the first frame update
    void Start()
    {
        

    }

    void OnEnable()
    {

        
        sequence = DOTween.Sequence();
        var newColor = textMesh.color;
        newColor.a = 1;
        sequence.Append(DOTween.To(() => textMesh.color, x => textMesh.color = x, newColor, appearanceTime));
        sequence.Append(transform.DOMoveY(transform.position.y + flightDistance, flightTime));
        newColor.a = 0;
        sequence.Join(DOTween.To(() => textMesh.color, x => textMesh.color = x, newColor, flightTime));
        sequence.AppendCallback(() =>
        {
            transform.position = defaultPosition;
            this.enabled = false;
        });
        sequence.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
