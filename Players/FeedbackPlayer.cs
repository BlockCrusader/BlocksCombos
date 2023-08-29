using Humanizer;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlocksCombos.Players
{
    public class FeedbackPlayer : ModPlayer
    {
        int feedbackVFXDelayC;
		int feedbackVFXDelayR;
		int feedbackSFXDelay;

        public override void PreUpdate()
        {
            if (feedbackSFXDelay > 0)
            {
                feedbackSFXDelay--;
            }
            if (feedbackVFXDelayC > 0)
            {
                feedbackVFXDelayC--;
            }
			if (feedbackVFXDelayR > 0)
			{
				feedbackVFXDelayR--;
			}
		}

		public void CounterFX()
        {
			CounterPlayer counterPlayer = Player.GetModPlayer<CounterPlayer>();

			// Counter: Hit-Count Feedback
			bool usingMeter = ModContent.GetInstance<Config>().MeterType == "Hit Count" || ModContent.GetInstance<Config>().MeterType == "Both";
			if (counterPlayer.hitCounter > 1 && feedbackVFXDelayC <= 0 && ModContent.GetInstance<Config>().hitCountSettings.comboHitTotals && (usingMeter || ModContent.GetInstance<Config>().hitCountSettings.forcedTotals))
			{
				// Visual feedback
				bool boldCount = counterPlayer.hitCounter > ModContent.GetInstance<Config>().hitCountSettings.largeComboThreshold;
				Color textColor = GetCounterColor(counterPlayer.hitCounter);
				CombatText.NewText(Player.Hitbox, textColor,
					Language.GetTextValue("Mods.BlocksCombos.CommonItemtooltip.HitFeedback").FormatWith(counterPlayer.hitCounter),
					boldCount);
				feedbackVFXDelayC = 30;

				// Sound feedback
				if (boldCount && ModContent.GetInstance<Config>().hitCountSettings.largeComboSFX != "Disabled (Default)" && feedbackSFXDelay <= 0)
				{
					feedbackSFXDelay = 30;
					PositiveSFX(ModContent.GetInstance<Config>().hitCountSettings.largeComboSFX);
				}
			}
		}

		public void RankingFX(bool rankUp)
        {
			RankPlayer rankPlayer = Player.GetModPlayer<RankPlayer>();

			// Sound feedback
			if (rankUp && ModContent.GetInstance<Config>().letterRankSettings.rankSFX != "Disabled (Default)" && feedbackSFXDelay <= 0)
			{
				feedbackSFXDelay = 30;
				PositiveSFX(ModContent.GetInstance<Config>().letterRankSettings.rankSFX);
			}
            else if (ModContent.GetInstance<Config>().letterRankSettings.rankSFX != "Disabled (Default)" && feedbackSFXDelay <= 0)
			{
				feedbackSFXDelay = 30;
				NegativeSFX(ModContent.GetInstance<Config>().letterRankSettings.demoteSFX);
			}

			// Visual feedback
			bool usingMeter = ModContent.GetInstance<Config>().MeterType == "Hit Count" || ModContent.GetInstance<Config>().MeterType == "Both";
			if (feedbackVFXDelayR <= 0 && ModContent.GetInstance<Config>().letterRankSettings.rankingFeedback && (usingMeter || ModContent.GetInstance<Config>().letterRankSettings.forcedRankFeedback))
            {
				if(rankUp || ModContent.GetInstance<Config>().letterRankSettings.positiveRankFeedback == false)
                {
					int rank = rankPlayer.rank;
                    if (!rankUp)
                    {
						rank--;
                    }
					bool boldRank = rank > 4; // A or higher
					Color textColor = GetRankColor(rankPlayer.rank);
					string rankText = GetRankLetter(rankPlayer.rank);
					CombatText.NewText(Player.Hitbox, textColor,
                        Language.GetTextValue("Mods.BlocksCombos.CommonItemtooltip.RankFeedback").FormatWith(rankText), 
						boldRank);
					feedbackVFXDelayR = 30;
				}
			}
		}

        /// <summary>
        /// Attempts to play a noise based on the given string.
        /// </summary>
        private void PositiveSFX(string soundName)
        {
			if (Main.myPlayer == Player.whoAmI)
			{
				if(soundName == "Twinkle")
                {
					SoundEngine.PlaySound(SoundID.Item4, Player.Center);
					return;
				}
				if (soundName == "Mana Crystal")
				{
					SoundEngine.PlaySound(SoundID.Item29, Player.Center);
					return;
				}
				if (soundName == "Golf Goal")
				{
					SoundEngine.PlaySound(SoundID.Item129, Player.Center);
					return;
				}
				if (soundName == "Recording")
				{
					SoundEngine.PlaySound(SoundID.Item166, Player.Center);
					return;
				}
				if (soundName == "Research")
				{
					SoundEngine.PlaySound(SoundID.ResearchComplete, Player.Center);
					return;
				}
				if (soundName == "Achievement")
				{
					SoundEngine.PlaySound(SoundID.AchievementComplete, Player.Center);
					return;
				}
				if (soundName == "Bell")
				{
					SoundEngine.PlaySound(SoundID.Item35, Player.Center);
					return;
				}
				if (soundName == "Meowmere")
				{
					SoundEngine.PlaySound(SoundID.Meowmere, Player.Center);
					return;
				}
				if (soundName == "Cymbal")
				{
					SoundEngine.PlaySound(SoundID.DrumCymbal1, Player.Center);
					return;
				}
				if (soundName == "Whistle")
				{
					SoundEngine.PlaySound(SoundID.Item128, Player.Center);
					return;
				}
				if (soundName == "Duck")
				{
					SoundEngine.PlaySound(SoundID.Duck, Player.Center);
					return;
				}
				return;
			}
		}

        /// <summary>
        /// Attempts to play a noise based on the given string.
        /// </summary>
        private void NegativeSFX(string soundName)
        {
			if(Main.myPlayer == Player.whoAmI)
            {
				if (Main.myPlayer == Player.whoAmI)
				{
					if (soundName == "Shatter" || soundName == "Shatter (Default)")
					{
						SoundEngine.PlaySound(SoundID.Shatter, Player.Center);
						return;
					}
					if (soundName == "Bomb")
					{
						SoundEngine.PlaySound(SoundID.Item14, Player.Center);
						return;
					}
					if (soundName == "Whoopie Chushion")
					{
						SoundEngine.PlaySound(SoundID.Item16, Player.Center);
						return;
					}
					if (soundName == "Ice Shatter")
					{
						SoundEngine.PlaySound(SoundID.Item27, Player.Center);
						return;
					}
					if (soundName == "Flask Shatter")
					{
						SoundEngine.PlaySound(SoundID.Item107, Player.Center);
						return;
					}
					if (soundName == "Deadly Sphere Staff")
					{
						SoundEngine.PlaySound(SoundID.Item113, Player.Center);
						return;
					}
					if (soundName == "Ritual")
					{
						SoundEngine.PlaySound(SoundID.Item123, Player.Center);
						return;
					}
					if (soundName == "Metal")
					{
						SoundEngine.PlaySound(SoundID.NPCHit4, Player.Center);
						return;
					}
					if (soundName == "Cymbal")
					{
						SoundEngine.PlaySound(SoundID.DrumCymbal1, Player.Center);
						return;
					}
					if (soundName == "Whistle")
					{
						SoundEngine.PlaySound(SoundID.Item128, Player.Center);
						return;
					}
					if (soundName == "Duck")
					{
						SoundEngine.PlaySound(SoundID.Duck, Player.Center);
						return;
					}
					return;
				}
			}
		}

        /// <summary>
        /// Returns a color based on the inputted hit count.
        /// </summary>
        private static Color GetCounterColor(int hits)
		{
			Color textColor = Colors.RarityTrash;
			if (hits > 1)
			{
				textColor = Colors.RarityNormal;
			}
			if (hits > 24)
			{
				textColor = Colors.RarityBlue;
			}
			if (hits > 49)
			{
				textColor = Colors.RarityGreen;
			}
			if (hits > 99)
			{
				textColor = Colors.RarityOrange;
			}
			if (hits > 149)
			{
				textColor = Colors.RarityRed;
			}
			if (hits > 199)
			{
				textColor = Colors.RarityPink;
			}
			if (hits > 249)
			{
				textColor = Colors.RarityPurple;
			}
			if (hits > 499)
			{
				textColor = Colors.RarityLime;
			}
			if (hits > 749)
			{
				textColor = Colors.RarityYellow;
			}
			if (hits > 999)
			{
				textColor = Colors.RarityCyan;
			}
			if (hits > 1449)
			{
				textColor = Colors.RarityDarkRed;
			}
			if (hits > 1999)
			{
				textColor = Colors.RarityDarkPurple;
			}
			if (hits > 2999)
			{
				textColor = new Color((byte)Main.DiscoR, (byte)Main.DiscoG, (byte)Main.DiscoB, Main.mouseTextColor);
			}
			if (hits > 4999)
			{
				textColor = new Color(255, (byte)(Main.masterColor * 200f), 0, Main.mouseTextColor);
			}
			if (hits > 9999)
			{
				float timing = (float)((Math.Sin((double)((float)Math.PI * 2f / 1f) * (double)Main.GlobalTimeWrappedHourly) + 1.0) * 0.5);
				textColor = Color.Lerp(new Color(50, 205, 50), new Color(160, 255, 210), timing);
			}
			return textColor;
		}

        /// <summary>
        /// Returns a color based on the inputted rank.
        /// </summary>
        private static Color GetRankColor(int rank)
		{
			Color textColor = Colors.RarityTrash;
			if (rank > 0)
			{
				textColor = Colors.RarityBlue;
			}
			if (rank > 1)
			{
				textColor = Colors.RarityGreen;
			}
			if (rank > 2)
			{
				textColor = Colors.RarityOrange;
			}
			if (rank > 3)
			{
				textColor = Colors.RarityRed;
			}
			if (rank > 4)
			{
				textColor = Colors.RarityPink;
			}
			if (rank > 5)
			{
				textColor = Colors.RarityYellow;
			}
			if (rank > 6)
			{
				textColor = Colors.RarityDarkRed;
			}
			if (rank > 7)
			{
				textColor = new Color((byte)Main.DiscoR, (byte)Main.DiscoG, (byte)Main.DiscoB, Main.mouseTextColor);
			}
			if (rank > 8)
			{
				textColor = new Color(255, (byte)(Main.masterColor * 200f), 0, Main.mouseTextColor);
			}
			return textColor;
					}

        /// <summary>
        /// Returns the respective letter represented by a given (int) rank.
        /// </summary>
        private static string GetRankLetter(int rank)
		{
			string letter = "F";
			if (rank > 0)
			{
				letter = "E";
			}
			if (rank > 1)
			{
				letter = "D";
			}
			if (rank > 2)
			{
				letter = "C";
			}
			if (rank > 3)
			{
				letter = "B";
			}
			if (rank > 4)
			{
				letter = "A";
			}
			if (rank > 5)
			{
				letter = "S";
			}
			if (rank > 6)
			{
				letter = "SS";
			}
			if (rank > 7)
			{
				letter = "SSS";
			}
			if (rank > 8)
			{
				letter = "U";
			}
			return letter;
		}
	}
}