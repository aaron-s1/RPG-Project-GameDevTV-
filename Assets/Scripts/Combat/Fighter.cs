using System;
using UnityEngine;
using RPG.Movement;
using RPG.Core;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Transform handTransform = null;

        // [SerializeField] RuntimeAnimatorController controller;
        [SerializeField] AnimatorOverrideController weaponOverride = null;


        Animator animator;

        Health target;

        float timeSinceLastAttack = Mathf.Infinity;


        void Start() {
            animator = GetComponent<Animator>();

            // var animController = animator.GetComponent<RuntimeAnimatorController>();
            // animController = controller;

            if (gameObject.tag == "Player")
                SpawnWeapon();
        }

        void Update()
        {                        
            timeSinceLastAttack += Time.deltaTime;
            MoveToAttackTarget();
        }

        void SpawnWeapon() {
            animator.runtimeAnimatorController = weaponOverride;
            Instantiate(weaponPrefab, handTransform);
        }

        void MoveToAttackTarget()
        {
            if (target == null || target.IsDead())
                return;

            if (!GetIsInRange()) 
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);                

            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }


        void AttackBehavior()
        {
            transform.LookAt(target.transform.position);

            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This triggers Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
                return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return (targetToTest != null && !targetToTest.IsDead());
        }


        #region Attacking: Toggle Animator triggers.

        void TriggerAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }
        
        void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        public void Cancel()
        {            
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        #endregion

        

        // Played via Animation event.
        public void Attack(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }


        void Hit()
        {
            if (target == null)
                return;

            target.TakeDamage(weaponDamage);
        }


        public bool GetIsInRange() =>
            Vector3.Distance(transform.position, target.transform.position) < weaponRange;
    }
}