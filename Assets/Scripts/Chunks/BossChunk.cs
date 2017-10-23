using UnityEngine;

namespace KyleStankovich.BattleDash {
    /// <summary>
    /// Class responsible for handling chunks throughout the game.
    /// </summary>
    [AddComponentMenu("Battle Dash/Chunks/Boss Chunk")]
    public class BossChunk : Chunk {
        [SerializeField]
        Fighter boss = null;
        [SerializeField]
        Transform bossSpawnPoint = null;
        [SerializeField]
        Quaternion bossRotation = Quaternion.identity;

        Animator anim = null;

        #region Methods

        #region Unity methods
        new void Awake() {
            //base.Awake();

            if(boss == null || bossSpawnPoint == null)
                return;

            //Get the boss's animator
            anim = boss.gameObject.GetComponent<Animator>();

            //Spawn the boss
            Instantiate(boss.gameObject, bossSpawnPoint.position, bossRotation, gameObject.transform);
        }

        private void OnEnable() {
            //Make sure boss is not in the dead animation state
            if(anim != null)
                anim.SetTrigger("alive");
        }
        #endregion

        #endregion
    }
}
