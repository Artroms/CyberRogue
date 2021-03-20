using UnityEngine;
using System.Collections;
using Pathfinding;
using DVInterfaces;

public class Bot : MonoBehaviour
{
    private AIDestinationSetter destinationSetter;
    private Animator animator;
    private IEnumerator attackRoutine;
    private AIPath aiPath;
    private bool CanAttack => Vector3.Distance(transform.position, SingletonePlayer.Instance.transform.position) < 1;
    // Start is called before the first frame update
    void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = SingletonePlayer.Instance.transform;
        animator = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanAttack && attackRoutine == null)
        {
            attackRoutine = AttackRoutine();
            StartCoroutine(attackRoutine);
        }
        animator.SetFloat("Velocity Z", aiPath.velocity.magnitude);
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
            if (CanAttack)
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
                if (CanAttack)
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
                    if (CanAttack)
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
                        if (CanAttack)
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
                            if (CanAttack)
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


    private void TryDamage()
    {
        var hit = Physics.SphereCastAll(transform.position, 0.1f, transform.forward, 1);
        for (int i = 0; hit != null && i < hit.Length; i++)
        {
            if (hit[i].transform == transform)
                continue;
            var h = hit[i].collider.GetComponent<IHealth>();
            if (h != null && !h.Equals(null))
            {
                h.Points--;
            }
        }
    }
}
