using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DVInterfaces;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public GameObject attackCollider;
    private CharacterController controller;
    private Animator animator;
    private Vector3 smoothVec;
    private IEnumerator rollRoutine;
    private IEnumerator attackRoutine;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        direction *= speed;
        smoothVec = Vector3.Lerp(smoothVec, direction, Time.deltaTime * 5);
        Vector3 rotVec = smoothVec == Vector3.zero ? transform.forward : smoothVec;
        if (rollRoutine != null && attackRoutine == null)
        {
            controller.SimpleMove(transform.forward * speed);
            transform.rotation = Quaternion.LookRotation(transform.forward * speed);
        }
        else if (attackRoutine != null && rollRoutine == null)
        {
            controller.SimpleMove(Vector3.zero);
            transform.rotation = Quaternion.LookRotation(rotVec);
        }
        else
        {
            controller.SimpleMove(direction);
            transform.rotation = Quaternion.LookRotation(rotVec);
        }
            
        if (Input.GetKeyDown(KeyCode.LeftShift) && rollRoutine == null && attackRoutine == null)
        {
                rollRoutine = RollRoutine();
                StartCoroutine(rollRoutine);
        }
        else
            animator.SetFloat("Velocity Z", smoothVec.magnitude);
        if (Input.GetMouseButtonDown(0) && attackRoutine == null && rollRoutine == null)
        {
            attackRoutine = AttackRoutine();
            StartCoroutine(attackRoutine);
        }
    }

    private void TryDamage()
    {
        var hit = Physics.SphereCastAll(transform.position, 0.1f, transform.forward, 1);
        for (int i = 0; hit != null && i < hit.Length; i++)
        {
            if (hit[i].transform == transform)
                continue;
            var h = hit[i].collider.GetComponent<IHealth>();
            if(h != null && !h.Equals(null))
            {
                h.Points--;
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        animator.SetTrigger("Trigger");
        yield return new WaitForSeconds(0.35f);
        TryDamage();
        float timer = 0;
        bool pressed = false;
        float wait = 0.35f;
        while (timer < wait)
        {
            yield return null;
            if (Input.GetMouseButtonDown(0))
            {
                pressed = true;
            }
            timer += Time.deltaTime;
        }
        if (pressed)
        {
            pressed = false;
            timer = 0;
            animator.SetInteger("Action", 2);
            animator.SetTrigger("Trigger");
            yield return new WaitForSeconds(0.35f);
            TryDamage();
            while (timer < wait)
            {
                yield return null;
                if (Input.GetMouseButtonDown(0))
                {
                    pressed = true;
                }
                timer += Time.deltaTime;
            }
            if (pressed)
            {
                pressed = false;
                timer = 0;
                animator.SetInteger("Action", 3);
                animator.SetTrigger("Trigger");
                yield return new WaitForSeconds(0.35f);
                TryDamage();
                while (timer < wait)
                {
                    yield return null;
                    if (Input.GetMouseButtonDown(0))
                    {
                        pressed = true;
                    }
                    timer += Time.deltaTime;
                }
                if (pressed)
                {
                    pressed = false;
                    timer = 0;
                    animator.SetInteger("Action", 4);
                    animator.SetTrigger("Trigger");
                    yield return new WaitForSeconds(0.35f);
                    TryDamage();
                    while (timer < wait)
                    {
                        yield return null;
                        if (Input.GetMouseButtonDown(0))
                        {
                            pressed = true;
                        }
                        timer += Time.deltaTime;
                    }
                    if (pressed)
                    {
                        pressed = false;
                        timer = 0;
                        animator.SetInteger("Action", 5);
                        animator.SetTrigger("Trigger");
                        yield return new WaitForSeconds(0.35f);
                        TryDamage();
                        while (timer < wait)
                        {
                            yield return null;
                            if (Input.GetMouseButtonDown(0))
                            {
                                pressed = true;
                            }
                            timer += Time.deltaTime;
                        }
                        if (pressed)
                        {
                            animator.SetInteger("Action", 6);
                            animator.SetTrigger("Trigger");
                            yield return new WaitForSeconds(0.35f);
                            TryDamage();
                            yield return new WaitForSeconds(wait);
                        }
                    }
                }
            }
        }
        attackRoutine = null;
        animator.SetInteger("Action", 1);
    }

    private IEnumerator RollRoutine()
    {
        attackCollider?.gameObject.SetActive(true);
        animator.Play("Unarmed-DiveRoll-Forward1");
        speed *= 1.5f;
        yield return new WaitForSeconds(0.95f);
        speed /= 1.5f;
        rollRoutine = null;
        attackCollider?.gameObject.SetActive(false);
    }
}
