using Terraria;
using Terraria.ModLoader;

namespace BlocksCombos.Players
{
    public class CounterPlayer : ModPlayer
    {
        public int hitCounter = 0;
        int expirationCounter = -1;
		public float rotSpeed = 0f;
        public override void PreUpdate()
        {
			if (hitCounter > 0 && expirationCounter > 0 && !Player.dead && Player.active)
            {
                expirationCounter--;
            }
            else
            {
				FeedbackPlayer feedbackPlayer = Player.GetModPlayer<FeedbackPlayer>();
				feedbackPlayer.CounterFX();

				hitCounter = 0;
                expirationCounter = -1;
            }
			float rotMult = ModContent.GetInstance<Config>().hitCountSettings.clockSpinMult;
			float timeMult = ModContent.GetInstance<Config>().hitCountSettings.clockDuration;
			rotSpeed = rotMult * (expirationCounter / (200 * timeMult));
			rotSpeed = Utils.Clamp(rotSpeed, 0f, 1f);
            if (ModContent.GetInstance<Config>().hitCountSettings.reverseSpin) 
            {
				rotSpeed *= -1f;
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ValidTargetCheck(target) && hit.Damage > 0)
            {
                IncrementCounter();
            }
        }

		private static bool ValidTargetCheck(NPC target)
		{
			bool hostile = !target.CountsAsACritter && !target.friendly;
			return hostile && !target.SpawnedFromStatue && !target.immortal && !target.dontTakeDamage;
		}

		public override void PostHurt(Player.HurtInfo info)
        {
			if(info.Damage > 0 && ModContent.GetInstance<Config>().hitCountSettings.counterHurtPenalty)
            {
				expirationCounter = -1;
			}
		}

        /// <summary>
        /// Updates and adds +1 to the hit-count combo
        /// </summary>
        private void IncrementCounter()
        {
			if (Player.dead || !Player.active)
			{
				return;
			}
			hitCounter++;
			if(hitCounter > 2000000000) // Hard cap at 2 billion to avoid integer limit
            {
				hitCounter = 2000000000;
			}
			float timeMult = ModContent.GetInstance<Config>().hitCountSettings.clockDuration;
			expirationCounter = (int)(200 * timeMult);
        }
	}
}