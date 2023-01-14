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

        Animator animator;

        Health target;

        float timeSinceLastAttack = 0;


        void Awake() =>
            animator = GetComponent<Animator>();


        void Update()
        {                        
            timeSinceLastAttack += Time.deltaTime;
            MoveToAttackTarget();
        }


        void MoveToAttackTarget()
        {
            if (target == null || target.IsDead())
                return;

            if (!GetIsInRange()) 
                GetComponent<Mover>().MoveTo(target.transform.position);                

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
        }

        #endregion


        // Played via Animation event.
        public void AttackTarget(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }


        void Hit()
        {
            if (target == null)
                return;
                
            target.TakeDamage(weaponDamage);
        }


        public bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }        

    }    
}