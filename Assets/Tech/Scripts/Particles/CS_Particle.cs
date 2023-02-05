using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Particle : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animCurvePosY, _animCurvePosX;
    private Animation _anim;
    [SerializeField] private Transform _transformToMove, _transformToRotateOnce, _transformToAlwaysRotate;

    public void OnCreated(int dir = 1)
    {
        StartCoroutine(MoveCoroutine(dir));
    }

    private IEnumerator MoveCoroutine(int dir)
    {
        _anim = GetComponent<Animation>();

        float timer = 0;
        float randomRot = Random.Range(0f, 360f);

        float randomScale = Random.Range(0.8f, 1.05f);
        _transformToMove.localScale = Vector3.one * randomScale;

        float randomYFactor = Random.Range(0.8f, 1.05f);
        float randomXFactor = Random.Range(0.8f, 1.05f);

        _transformToRotateOnce.localEulerAngles = new Vector3(0, 0, randomRot);
        _anim.Play();

        while (timer < 1)
        {
            float newY = _animCurvePosY.Evaluate(timer) * randomYFactor;
            float newX = _animCurvePosX.Evaluate(timer) * dir * randomXFactor;
            Vector3 newPos = new Vector3(newX, newY, 0);
            _transformToMove.localPosition = newPos;
            _transformToRotateOnce.localEulerAngles += new Vector3(0, 0, Time.deltaTime * dir);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}