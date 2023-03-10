using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour {

        Health health;

        void Start() =>
            health = GetComponent<Health>();


        void Update() {
            if (health.IsDead())
                return;

            if (InteractWithCombat())
                return;

            if (InteractWithMovement())
                return;            
        }


        bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target == null)
                    continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                    continue;

                if (Input.GetMouseButton(0))
                    GetComponent<Fighter>().Hit(target.gameObject);

                return true;
            }

            return false;
        }


        bool InteractWithMovement() {
            RaycastHit hit;

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);

                return true;
            }

            return false;
        }



        static Ray GetMouseRay() =>
            Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}