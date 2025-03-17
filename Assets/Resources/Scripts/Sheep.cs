using UnityEngine;
using DG.Tweening;
using System.Collections;
using FMODUnity;

public class Sheep : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private ParticleSystem _sparks;

    [SerializeField] private float _walkTime;
    [SerializeField] private float _jumpTime;
    [SerializeField] private float _waitTime;

    [SerializeField] private EventReference _bleetSound;
    [SerializeField] private EventReference _countSound;

    private Vector3 _forward;
    private Rigidbody _rigidBody;

    public bool _isJumping = false;
    public int level = 1;

    private void Start()
    {
        StartCoroutine(RandomWait(false));
        _rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// move to a random point
    /// </summary>
    private void RandomWalk()
    {
        Vector2 endPoint = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        _forward = new Vector3(endPoint.x, 1.5f, endPoint.y) - transform.position;

        transform.DOMoveX(endPoint.x, _walkTime);
        transform.DOMoveZ(endPoint.y, _walkTime);
        StartCoroutine(RandomWait(false));
    }

    private IEnumerator RandomWait(bool jumping)
    {
        if(jumping == true)
        {
            _isJumping=true;
            yield return new WaitForSeconds(_jumpTime + .25f);
            _isJumping = false;
        }
        yield return new WaitForSeconds(Random.Range(1f, _waitTime) + _walkTime);
        RandomWalk();
    }

    /// <summary>
    /// Jump over the fence when they get to it.
    /// </summary>

    private void OnTriggerEnter(Collider other)
    {
        Jump();
    }

    private void Jump()
    {
        // cancel all movement
        StopAllCoroutines();

        // tween the jump
        transform.DOKill();

        if(transform.position.z > 0)
        {
            transform.DOLocalJump( new Vector3(transform.position.x, 1.5f, Random.Range(-2, -5)), 3f, 1, _jumpTime).SetDelay(.25f).SetEase(Ease.Flash);
        }
        else
        {
            transform.DOLocalJump(new Vector3(transform.position.x, 1.5f, Random.Range(2, 5)), 3f, 1, _jumpTime).SetDelay(.25f).SetEase(Ease.Flash);
        }
        StartCoroutine(RandomWait(true));

        // play the jump sound
        RuntimeManager.PlayOneShot(_bleetSound);
    }

    /// <summary>
    /// hovering over the sheep with the mouse
    /// </summary>
    private void OnMouseOver()
    {
        if (_isJumping == true)
        {
            switch (level)
            {
                case 1:
                    GameManager.jumpsCounted++; break;
                case 2:
                    GameManager.jumpsCounted += 3; break;
                case 3:
                    GameManager.jumpsCounted += 10; break;
                case 4:
                    GameManager.jumpsCounted += 100; break;
            }
            GameManager.OnJumpCounted.Invoke();

            _sparks.Play();
            RuntimeManager.PlayOneShot(_countSound);
            _isJumping = false;
        }
    }
}
