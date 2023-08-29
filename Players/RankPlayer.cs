using System;
using Terraria;
using Terraria.ModLoader;

namespace BlocksCombos.Players
{
    public class RankPlayer : ModPlayer
    {
        public float dmgCounter = 0;
        public int rank;
        public float percentToRank;
        private readonly float eC = 1.175f; // Exponential Coefficient used in rank formula
        public int maxRank;

        public override void PreUpdate()
        {
            if (dmgCounter > 0 && !Player.dead && Player.active)
            {
                float maxValue = GetMaxValue();
                if (dmgCounter > maxValue)
                {
                    dmgCounter = maxValue;
                }
                dmgCounter -= GetDrainage();
            }
            else
            {
                dmgCounter = 0;
                rank = 0;
            }
            UpdateRank();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ValidTargetCheck(target) && hit.Damage > 0)
            {
                int dmg = hit.Damage;
                IncrementCounter(dmg);
            }
        }

        private static bool ValidTargetCheck(NPC target)
        {
            bool hostile = !target.CountsAsACritter && !target.friendly;
            return hostile && !target.SpawnedFromStatue && !target.immortal && !target.dontTakeDamage;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (ModContent.GetInstance<Config>().letterRankSettings.rankHurtPenalty)
            {
                // Causes a penalty on the meter, based on rank
                // The exponential coefficient is lower; higher ranks are proportionally penalized less, but still have an overall larger penalty
                float penaltyFactor = ModContent.GetInstance<Config>().letterRankSettings.rankPenalty;
                dmgCounter -= penaltyFactor * (float)(GetBaseDmgReq() * Math.Pow(1.17, rank));
                if (ModContent.GetInstance<Config>().letterRankSettings.rankResetPenalty)
                {
                    dmgCounter = 0;
                    rank = 0;
                }
            }
            UpdateRank();
        }

        /// <summary>
        /// Contributes damage to the Ranking combo system.
        /// </summary>
        /// <param name="dmg">Damage dealt.</param>
        private void IncrementCounter(int dmg)
        {
            if (Player.dead || !Player.active)
            {
                return;
            }
            dmgCounter += dmg;
            float maxValue = GetMaxValue();
            if (dmgCounter > maxValue)
            {
                dmgCounter = maxValue;
            }
            UpdateRank();
        }

        /// <summary>
        /// Calculates and updates combo Rank. Produces feedback FX as needed upon ranking up/down
        /// </summary>
        private void UpdateRank()
        {
            float usedBase = GetBaseDmgReq();
            float oldRank = rank;
            rank = 0; 
            int maxRank = GetMaxRank();
            for (int i = 0; i <= maxRank; i++) // For loop going up to 6 (or 7-9, based on config), representing rank
            {
                rank = i; // Set level to i

                // We must manully subtract damage from previous ranks before checking damage reqs to rank up
                float nonCumulativeDmg = dmgCounter;
                if (i > 0) 
                {
                    float previousDmg = 0;
                    for (int j = i; j > 0; j--) 
                    {
                        previousDmg += (int)(usedBase * Math.Pow(eC, j - 1));
                    }
                    nonCumulativeDmg -= previousDmg; 

                    if (nonCumulativeDmg < 0)
                    {
                        nonCumulativeDmg = 0;
                    }
                }

                if (nonCumulativeDmg >= (int)(usedBase * Math.Pow(eC, i)) && rank < maxRank) 
                {
                    continue;
                }
                else // If there isn't enough damage to rank up, break the loop, cutting rank off at the current i value
                {
                    UpdateToRank();
                    break;
                }
            }

            if(rank != oldRank)
            { 
                FeedbackPlayer feedbackPlayer = Player.GetModPlayer<FeedbackPlayer>();
                feedbackPlayer.RankingFX(rank > oldRank);
            }
        }

        /// <summary>
        /// Calculates and updates damage needed to rank up. Used for drawing purposes by the meter
        /// </summary>
        public void UpdateToRank()
        {
            float usedBase = GetBaseDmgReq();
            float nonCumulativeDmg = dmgCounter;
            if (rank > 0) 
            {
                float previousDmg = 0;
                for (int j = rank; j > 0; j--) 
                {
                    previousDmg += (int)(usedBase * Math.Pow(eC, j - 1));
                }
                nonCumulativeDmg -= previousDmg; 

                if (nonCumulativeDmg < 0)
                {
                    nonCumulativeDmg = 0;
                }
            }

            float toNextRank = ((int)(usedBase * (Math.Pow(eC, rank)))) - nonCumulativeDmg;
            float nextRankDmg = (int)(usedBase * Math.Pow(eC, rank));
            percentToRank = toNextRank / nextRankDmg;
            percentToRank = Utils.Clamp(percentToRank, 0f, 1f);
        }

        /// <summary>
        /// Calculates and returns passive combo loss over time, based on config. and current rank
        /// </summary>
        private float GetDrainage()
        {
            float usedBase = GetBaseDmgReq();
            int nextRank = rank;
            float drainFactor = ModContent.GetInstance<Config>().letterRankSettings.rankDrain;
            return drainFactor * (1f/60f) * (float)(usedBase * Math.Pow(1.18, nextRank));
        }

        /// <summary>
        /// Returns the base amount of damage needed to rank up. This number is used to calculate damage reqs for all ranks. Dynamically changes based on bosses defeated
        /// </summary>
        private static float GetBaseDmgReq()
        {
            float baseMult = ModContent.GetInstance<Config>().letterRankSettings.rankRate;
            if (ModContent.GetInstance<Config>().letterRankSettings.useCustomRanking)
            {
                return ModContent.GetInstance<Config>().letterRankSettings.customRankBase * baseMult;
            }
            if (NPC.downedMoonlord)
            {
                return 7500f * baseMult;
            }
            if (NPC.downedAncientCultist)
            {
                return 4800f * baseMult;
            }
            if (NPC.downedGolemBoss || NPC.downedEmpressOfLight || (NPC.downedPlantBoss && NPC.downedFishron)) 
            {
                return 3600f * baseMult;
            }
            if (Condition.DownedMechBossAll.IsMet() && NPC.downedFishron) 
            {
                return 3225f * baseMult;
            }
            if (NPC.downedPlantBoss) 
            {
                return 2850f * baseMult;
            }
            if (NPC.downedMechBossAny && NPC.downedFishron) // 1 Mech AND Duke Fishron
            {
                return 2525f * baseMult;
            }
            if (Condition.DownedMechBossAll.IsMet() || NPC.downedFishron) // Post-Mechs OR Duke Fishron at any time
            {
                return 2200f * baseMult;
            }
            if (NPC.downedMechBossAny)
            {
                return 1600f * baseMult;
            }
            if (NPC.downedQueenSlime)
            {
                return 1100f * baseMult;
            }
            if (Main.hardMode)
            {
                return 750f * baseMult;
            }
            if (NPC.downedBoss3)
            {
                return 400f * baseMult;
            }
            if (NPC.downedBoss2 || NPC.downedQueenBee || NPC.downedDeerclops)
            {
                return 275f * baseMult;
            }
            if (NPC.downedBoss1 || NPC.downedSlimeKing)
            {
                return 175f * baseMult;
            }
            return 100f * baseMult;
        }

        /// <summary>
        /// Calculates and returns the maximum possible value of the combo
        /// </summary>
        private float GetMaxValue()
        {
            int maxRank = GetMaxRank();
            float total = 0f;
            for(int i = 0; i <= maxRank; i++)
            {
                total += (float)(GetBaseDmgReq() * Math.Pow(eC, i));
            }
            return total;
        }

        /// <summary>
        /// Calculates and returns the highest rank avaliable (As determined by config)
        /// </summary>
        private int GetMaxRank()
        {
            if (ModContent.GetInstance<Config>().letterRankSettings.maxRank == "S (Default)")
            {
                maxRank = 6;
                return 6; 
            }
            if (ModContent.GetInstance<Config>().letterRankSettings.maxRank == "SS")
            {
                maxRank = 7;
                return 7; 
            }
            if (ModContent.GetInstance<Config>().letterRankSettings.maxRank == "SSS")
            {
                maxRank = 8;
                return 8; 
            }
            if (ModContent.GetInstance<Config>().letterRankSettings.maxRank == "U")
            {
                maxRank = 9;
                return 9;
            }
            if (ModContent.GetInstance<Config>().letterRankSettings.maxRank == "A (Reduces max rank)")
            {
                maxRank = 5;
                return 5; 
            }
            maxRank = 6;
            return 6; 
        }
    }
}