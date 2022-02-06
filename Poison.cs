// Creator: JackO'ManyNames
// Date: 5/1/2022
// Lasted edited: 6/2/2022

using UnityEngine;
using ThunderRoad;

namespace Mordeth
{
    public class Poison : MonoBehaviour
    {
        /// <summary>
        /// Starts up the Module.
        /// Turns off the particle system and creates event handlers
        /// </summary>
        public void Start()
        {
            this.item = base.GetComponent<Item>();
            this.ps = GameObject.Find("Shadow").GetComponent<ParticleSystem>();
            this.item.OnGrabEvent += GrabEventHandler;
            ps.Stop();
            foreach (CollisionHandler collisionHandler in item.collisionHandlers)
            {
                collisionHandler.OnCollisionStartEvent += CollisionEventHandler;
            }
            this.item.OnUngrabEvent += ReleaseEventHandler;
        }

        private void ReleaseEventHandler(Handle handle, RagdollHand ragdollHand, bool throwing)
        {
            bool enabled = base.enabled;
            if (enabled)
            {
                bool flag = item.handlers.Count <= 0;
                if (flag)
                {
                    this.ps.Stop();
                }
            }
        }

        /// <summary>
        /// Turns on the particle system when grabbed
        /// </summary>
        private void GrabEventHandler(Handle handle, RagdollHand ragdollHand)
        {
            bool enabled = base.enabled;
            if (enabled)
            {
                bool flag = item.handlers.Count > 0;
                if (flag)
                {
                    this.ps.Play();
                }
            }
        }


        /// <summary>
        /// Detects if the dagger does Non-Blunt damage to a creature.
        /// If so, start a coroutine on damaged creature to poison it.
        /// </summary>
        /// <param name="collisionInstance"></param>
        private void CollisionEventHandler(CollisionInstance collisionInstance)
        {
            if (base.enabled)
            {
                bool flag = collisionInstance.damageStruct.hitRagdollPart;
                bool flag2 = collisionInstance.damageStruct.damageType == DamageType.Blunt;
                if ((flag) && !(flag2))
                {
                    currC = collisionInstance.targetCollider.GetComponentInParent<Creature>();
                }
            }
        }

        /// <summary>
        /// If there is a creature that's been damaged, will start a timer doing damage to them
        /// over time. If their health drops below zero, kill the creature.
        /// </summary>
        private void Update()
        {
            if (currC != null)
            {
                bool flag = Time.time - startTime > 1f;
                if (flag)
                {
                    bool flag2 = currC.isKilled;
                    if (!flag2)
                    {
                        float num = currC.currentHealth;
                        float poison = num - 20f;
                        if (poison < 0f)
                        {
                            currC.Kill();
                            currC.ragdoll.SetState(Ragdoll.State.Destabilized, false);
                            currC = null;
                        }
                        else
                        {
                            currC.currentHealth = poison;
                        }
                    }
                    startTime = Time.time;
                }
            }
        }
        
        public Item item;
        public ParticleSystem ps;
        public Creature currC;
        private float startTime;
    }
}
