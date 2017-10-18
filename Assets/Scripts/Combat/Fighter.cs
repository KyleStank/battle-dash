using System.Collections;
using UnityEngine;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Class used for creating fighters.
    /// Should be extended for custom functionality.
    /// </summary>
    [AddComponentMenu("Battle Dash/Combat/Fighter (Temp)")]
	public class Fighter : MonoBehaviour {
        [SerializeField]
        protected float maxHealth = 100.0f;
        [SerializeField]
        protected float attackDamage = 15.0f;
        [SerializeField]
        protected Transform[] hitParticlesToSpawn = new Transform[0];
        [SerializeField]
        protected Transform hitParticleSpawnLocation = null;
        
        protected Fighter currentOpponent;
        protected float health = 100;
        protected float damageMultiplyer = 1.0f;
        protected bool isDefending = false;
        protected bool isVunerable = false;

        [HideInInspector]
        //public Animator anim;
        
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
        protected virtual void Awake() {
            //anim = GetComponent<Animator>();
            health = maxHealth;
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
            //if(anim != null && prevHealth != health && !isDefending)
                //anim.SetTrigger("getHit");
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
        public void Die() {
            //if(anim != null)
                //anim.SetTrigger("die");
        }

        /// <summary>
        /// Starts an attack of an opponent by playing the attack animation.
        /// </summary>
        public void StartAttack() {
            //Play the attack animation
            //if(anim != null)
                //anim.SetTrigger("attack");
        }
        
        /// <summary>
        /// Attack this fighter's current opponent.
        /// </summary>
        public void AttackOpponent() {
            //Attack
            if(currentOpponent != null)
                currentOpponent.TakeDamage(attackDamage * damageMultiplyer);
        }

        /// <summary>
        /// Makes the fighter start defending.
        /// </summary>
        public void Defend() {
            StartCoroutine(StartDefending());

            //Play animation
            //if(anim != null)
                //anim.SetTrigger("defend");
        }

        /// <summary>
        /// Spawns attack particles at the opponent.
        /// </summary>
        public void SpawnAttackParticles() {
            if(currentOpponent == null)
                return;

            //Randomly choose a hit particle to spawn
            if(currentOpponent.hitParticleSpawnLocation != null && hitParticlesToSpawn != null)
                Instantiate(hitParticlesToSpawn[Random.Range(0, hitParticlesToSpawn.Length)].gameObject, currentOpponent.hitParticleSpawnLocation.position, Quaternion.identity);
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
