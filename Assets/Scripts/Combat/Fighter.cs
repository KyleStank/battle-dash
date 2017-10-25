using System.Collections;
using UnityEngine;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash {
    /// <summary>
    /// Class used for creating fighters.
    /// Should be extended for custom functionality.
    /// </summary>
    [AddComponentMenu("Battle Dash/Combat/Fighter (Temp)")]
	public class Fighter : MonoBehaviour {
        [SerializeField]
        protected Animator m_Animator = null;
        [SerializeField]
        protected float maxHealth = 100.0f;
        [SerializeField]
        protected float attackDamage = 15.0f;
        [SerializeField]
        protected Transform m_WeaponTransform = null;
        [SerializeField]
        protected Transform m_WeaponOriginParent = null;
        [SerializeField]
        protected Transform m_EquipPoint = null;
        [SerializeField]
        protected Transform[] hitParticlesToSpawn = new Transform[0];
        [SerializeField]
        protected Transform hitParticleSpawnLocation = null;
        
        protected Fighter currentOpponent;
        protected Vector3 m_InitialWeaponPos = Vector3.zero;
        protected Quaternion m_InitialWeaponRot = Quaternion.identity;
        protected float health = 100;
        protected float damageMultiplyer = 1.0f;
        protected bool isDefending = false;
        protected bool isVunerable = false;
        
        [Tooltip("Percentage of health that should be left when this fighter is ready to be defeated.")]
        public float healthDefeatMultiplyer = 0.0f;

        #region Properties
        /// <summary>
        /// The current amount of health that the fighter has.
        /// </summary>
        public float Health {
            get { return health; }
            set { health = value; }
        }

        /// <summary>
        /// The max health that the fighter can have.
        /// </summary>
        public float MaxHealth { get { return maxHealth; } }

        /// <summary>
        /// Returns a multiplyer that affects the damage.
        /// </summary>
        public float DamageMultiplyer {
            get { return damageMultiplyer; }
            set { damageMultiplyer = value; }
        }
        #endregion

        #region Methods

        #region Unity methods
        protected virtual void Start()
        {
            if(m_WeaponTransform == null)
            {
                Debug.LogWarning("No weapon game object was assigned in the Inspector! This must be assigned for the weapon animation to work!");
            }

            if(m_EquipPoint == null)
            {
                Debug.LogWarning("No character hand was assigned! This must be assigned for the weapon animation to work!");
            }

            health = maxHealth;
        }

        protected virtual void OnEnable()
        {
            //Subscribe to events
            if(m_Animator != null)
            {
                EventManager.StartListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, () => m_Animator.ResetTrigger("UnsheathL"));
                EventManager.StartListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, () => m_Animator.SetTrigger("UnsheathL"));
                EventManager.StartListening(Constants.EVENT_BOSSBATTLEENDCOMBAT, () => m_Animator.ResetTrigger("SheathL"));
                EventManager.StartListening(Constants.EVENT_BOSSBATTLEENDCOMBAT, () => m_Animator.SetTrigger("SheathL"));
            }
        }

        protected virtual void OnDisable()
        {
            //Unsubscribe to events
            if(m_Animator != null)
            {
                EventManager.StopListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, () => m_Animator.ResetTrigger("UnsheathL"));
                EventManager.StopListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, () => m_Animator.SetTrigger("UnsheathL"));
                EventManager.StopListening(Constants.EVENT_BOSSBATTLEENDCOMBAT, () => m_Animator.ResetTrigger("SheathL"));
                EventManager.StopListening(Constants.EVENT_BOSSBATTLEENDCOMBAT, () => m_Animator.SetTrigger("SheathL"));
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Sets the current opponent of this fighter.
        /// </summary>
        /// <param name="fighter">Opponent fighter.</param>
        public void SetOpponent(Fighter fighter) {
            if(fighter != null)
                currentOpponent = fighter;
        }

        /// <summary>
        /// Heals the health by a certain amount.
        /// </summary>
        /// <param name="health">Amount to heal.</param>
		public virtual void Heal(int health) {
            this.health += health;
            this.health = Mathf.Clamp(this.health, 0, maxHealth);
        }

        /// <summary>
        /// Restores the health back to it's fullest.
        /// </summary>
        public virtual void RestoreHealth() {
            health = maxHealth;
        }

        /// <summary>
        /// Damage this figher.
        /// </summary>
        /// <param name="damage">Amount of damage to take.</param>
        public void TakeDamage(float damage) {
            float prevHealth = health;

            //Remove health
            health -= (damage * damageMultiplyer);
            health = Mathf.Clamp(health, 0, maxHealth);

            //Display hit numbers
            GUIManager.Instance.DisplayFloatingText(currentOpponent.transform, damage.ToString());

            //Check if figher has died
            if(health <= (maxHealth * healthDefeatMultiplyer)) {
                health = maxHealth * healthDefeatMultiplyer;
                Vunerable();
                return;
            }

            //Play the take animation if damage was taken and the figher has not died
            if(m_Animator != null && prevHealth != health && !isDefending)
            {
                m_Animator.SetTrigger("GetHit");
            }
        }

        /// <summary>
        /// Makes the fighter enter a vunerable state where he can fully die.
        /// </summary>
        public void Vunerable() {
            isVunerable = true;

            /* **Temp** */
            Die();
            /* **Temp** */

            //Finsh code here later...
        }

        /// <summary>
        /// Makes the figher die. Mainly used for animation purposes only.
        /// </summary>
        public void Die()
        {
            if(m_Animator != null)
            {
                m_Animator.ResetTrigger("Die");
                m_Animator.SetTrigger("Die");
            }
        }

        /// <summary>
        /// Starts an attack of an opponent by playing the attack animation.
        /// </summary>
        public void StartAttack() {
            //Play the attack animation
            if(m_Animator != null)
            {
                m_Animator.ResetTrigger("AttackL1");
                m_Animator.SetTrigger("AttackL1");
            }
        }
        
        /// <summary>
        /// Attack this fighter's current opponent.
        /// </summary>
        public void AttackOpponent() {
            //Attack
            if(currentOpponent != null)
            {
                currentOpponent.TakeDamage(attackDamage * damageMultiplyer);
            }
        }

        /// <summary>
        /// Makes the fighter start defending.
        /// </summary>
        public void Defend() {
            StartCoroutine(StartDefending());

            //Play animation
            if(m_Animator != null)
            {
                m_Animator.ResetTrigger("Defend");
                m_Animator.SetTrigger("Defend");
            }
        }

        public void EnableWeapon()
        {
            if(m_WeaponTransform == null || m_EquipPoint == null)
            {
                Debug.LogWarning("Can't enable weapon. Weapon game object or character hand was not assigned in the Inspector.");
                return;
            }

            m_InitialWeaponPos = m_WeaponTransform.localPosition;
            m_InitialWeaponRot = m_WeaponTransform.localRotation;

            m_WeaponTransform.SetParent(m_EquipPoint);
            m_WeaponTransform.SetPositionAndRotation(m_EquipPoint.position, m_EquipPoint.rotation);
        }

        public void DisableWeapon()
        {
            if(m_WeaponTransform == null || m_EquipPoint == null)
            {
                Debug.LogWarning("Can't disable weapon. Weapon game object or character hand was not assigned in the Inspector.");
                return;
            }

            m_WeaponTransform.SetParent(m_WeaponOriginParent);
            m_WeaponTransform.localPosition = m_InitialWeaponPos;
            m_WeaponTransform.localRotation = m_InitialWeaponRot;
        }

        /// <summary>
        /// Spawns attack particles at the opponent.
        /// </summary>
        public void SpawnAttackParticles() {
            if(currentOpponent == null)
                return;

            //Randomly choose a hit particle to spawn
            if(currentOpponent.hitParticleSpawnLocation != null && hitParticlesToSpawn != null)
            {
                Instantiate(hitParticlesToSpawn[Random.Range(0, hitParticlesToSpawn.Length)].gameObject, currentOpponent.hitParticleSpawnLocation.position, Quaternion.identity);
            }
        }
        #endregion

        #region IEnumerator methods
        /// <summary>
        /// Allows the fighter to defend for one frame.
        /// </summary>
        /// <returns></returns>
        IEnumerator StartDefending() {
            isDefending = true;

            yield return new WaitForEndOfFrame();

            isDefending = false;
        }
        #endregion

        #endregion
    }
}
