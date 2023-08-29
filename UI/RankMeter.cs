using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using BlocksCombos.Players;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.GameContent;

namespace BlocksCombos.UI
{
	internal class RankMeter : UIState
	{
		private UIText text;
		private UIElement area;
		private UIImage barFrame;

		public override void OnInitialize()
		{
			area = new UIElement();
			area.Left.Set(-area.Width.Pixels - 385, 1f); 
			area.Top.Set(45, 0f);
			area.Width.Set(120, 0f); 
			area.Height.Set(120, 0f);

			barFrame = new UIImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Fancy"));
			barFrame.Left.Set(0, 0.45f);
			barFrame.Top.Set(12, 1f);
			barFrame.Width.Set(138, 0f);
			barFrame.Height.Set(34, 0f);

			text = new UIText("F", 2f, true); 
			text.Width.Set(120, 0f);
			text.Height.Set(80, 0f);
			text.Top.Set(0, 0.5f);
			text.Left.Set(0, 0.5f);

			area.Append(barFrame);
			area.Append(text);
			Append(area);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (ModContent.GetInstance<Config>().MeterType == "Hit Count" || ModContent.GetInstance<Config>().MeterType == "Disabled")
            {
				return;
			}

			base.Draw(spriteBatch);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (ModContent.GetInstance<Config>().letterRankSettings.hideMeter)
			{
				return;
			}

			base.DrawSelf(spriteBatch);

			SetMeterVisual();

			float xDisplacement = ModContent.GetInstance<Config>().letterRankSettings.rankDisplacement.X;
			float yDisplacement = ModContent.GetInstance<Config>().letterRankSettings.rankDisplacement.Y;
			if (ModContent.GetInstance<Config>().letterRankSettings.rankAltDisplacement)
			{
				xDisplacement = ModContent.GetInstance<Config>().letterRankSettings.rankNudgeX;
				yDisplacement = ModContent.GetInstance<Config>().letterRankSettings.rankNudgeY;
			}
			area.Left.Set((-area.Width.Pixels - 385) + xDisplacement, 1f);
			area.Top.Set(45 + yDisplacement, 0f);

			var modPlayer = Main.LocalPlayer.GetModPlayer<RankPlayer>();
			float quotient = (1f - modPlayer.percentToRank);
			Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle(); // Determines where to draw the bar's fill
			hitbox.X += 8;
			hitbox.Width += 2;
			hitbox.Y += 9;
			hitbox.Height -= 19;
			int left = hitbox.Left;
			int right = hitbox.Right;
			int steps = (int)((right - left) * quotient);
			Color gradientA = GetTieredColor(modPlayer.rank);
			Color gradientB = GetTieredColor(modPlayer.rank + 1);
			// Overrides for higher tiers
			if(modPlayer.rank > 6)
            {
				gradientB = Colors.RarityDarkPurple;
			}
			if (modPlayer.rank > 7)
			{
				gradientA = Colors.RarityDarkPurple;
				gradientB = Colors.RarityAmber;
			}
			if (modPlayer.rank >= modPlayer.maxRank)
			{
				gradientA = new Color((byte)Main.DiscoR, (byte)Main.DiscoG, (byte)Main.DiscoB, Main.mouseTextColor);
				gradientB = new Color((byte)Main.DiscoR, (byte)Main.DiscoG, (byte)Main.DiscoB, Main.mouseTextColor);
			}
			// First fills in a simple background color
			if (!ModContent.GetInstance<Config>().letterRankSettings.hideMeterBG) 
            {
				int stepsBG = (int)((right - left));
				for (int i = 0; i < stepsBG; i += 1)
				{
					float percent = (float)i / (right - left);
					spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), new Color(55, 55, 55));
				}
			}
			// Fills in color to represent progress to next rank
			for (int i = 0; i < steps; i += 1)
			{
				float percent = (float)i / (right - left);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (ModContent.GetInstance<Config>().MeterType == "Hit Count" || ModContent.GetInstance<Config>().MeterType == "Disabled")
            {
				return;
			}

			var modPlayer = Main.LocalPlayer.GetModPlayer<RankPlayer>();
			
			text.SetText("F", 2f, true);
			text.Top.Set(0, 0.5f);
			if (modPlayer.rank > 0)
            {
				text.SetText("E", 2f, true);
				text.Top.Set(0, 0.5f);
			}
			if (modPlayer.rank > 1)
			{
				text.SetText("D", 2f, true);
				text.Top.Set(0, 0.5f);
			}
			if (modPlayer.rank > 2)
			{
				text.SetText("C", 2f, true);
				text.Top.Set(0, 0.5f);
			}
			if (modPlayer.rank > 3)
			{
				text.SetText("B", 2f, true);
				text.Top.Set(0, 0.5f);
			}
			if (modPlayer.rank > 4)
			{
				text.SetText("A", 2f, true);
				text.Top.Set(0, 0.5f);
			}
			if (modPlayer.rank > 5)
			{
				text.SetText("S", 2f, true);
				text.Top.Set(0, 0.5f);
			}
			if (modPlayer.rank > 6)
			{
				text.SetText("SS", 1.6f, true);
				text.Top.Set(0, 0.55f);
			}
			if (modPlayer.rank > 7)
			{
				text.SetText("SSS", 1.2f, true);
				text.Top.Set(0, 0.65f);
			}
			if (modPlayer.rank > 8)
			{
				text.SetText("U", 2f, true);
				text.Top.Set(0, 0.5f);
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.hideLetter) // Setting to hide the text display
			{
				text.SetText("", 2f, true); // Makes text blank to hide it
				text.Top.Set(0, 0.5f);
			}
			text.TextColor = GetTieredColor(modPlayer.rank); // Coloring based on combo

			base.Update(gameTime);
		}

        /// <summary>
        /// Returns a color based on the inputted rank.
        /// </summary>
        private static Color GetTieredColor(int rank)
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
        /// Sets the sprite used by the Rank Meter UI based on config.
        /// </summary>
        private void SetMeterVisual()
		{
			barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Fancy"));
			// The first batch are based off the minimap borders
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Fancy (Default)")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Fancy"));
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Valkyrie")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Valkyrie"));
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Retro")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Retro"));
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Leaf")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Leaf"));
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "TwigLeaf")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/TwigLeaf"));
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "StoneGold")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/StoneGold"));
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Sticks")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Sticks"));
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Remix")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Remix"));
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Golden")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Golden"));
			}
			// Alternate colors for the Fancy style
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Platinum Fancy")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/FancyPlat"));
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Crimson Fancy")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/FancyFTW"));
			}
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Emerald Fancy")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/FancyLegendary"));
			}
			// Lienfors themed style, as homage
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Tribute")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Tribute"));
			}
			// 1-Pixel thick line around the color fill
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Thin")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Thin"));
			}
			// Blank image for the bar and its background, leaving the color fill unbordered
			if (ModContent.GetInstance<Config>().letterRankSettings.meterStyle == "Fill-Only")
			{
				barFrame.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/RankMeterFrames/Blank"));
			}
		}
	}

	class RankUISystem : ModSystem
	{
		private UserInterface RankMeterUserInterface;

		internal RankMeter RankMeter;

		public override void Load()
		{
			// All code below runs only if we're not loading on a server
			if (!Main.dedServ)
			{
				RankMeter = new();
				RankMeterUserInterface = new();
				RankMeterUserInterface.SetState(RankMeter);
			}
		}

		public override void UpdateUI(GameTime gameTime)
		{
			RankMeterUserInterface?.Update(gameTime);
		}

        // Adds the UI onto the screen
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1)
			{
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"BlocksCombos: Rank Meter",
					delegate {
						RankMeterUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

	}
}