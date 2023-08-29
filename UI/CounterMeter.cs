using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using BlocksCombos.Players;
using Terraria.ID;
using System.Collections.Generic;
using System;

namespace BlocksCombos.UI
{
	internal class CounterMeter : UIState
	{
		private UIText text;
		private UIElement area;
		private UIImage counterClockCorner1;
		private UIImage counterClockCorner2;
		private UIImage counterClockCorner3;
		private UIImage counterClockCorner4;

        public override void OnInitialize()
		{
			area = new UIElement();
			area.Left.Set(-area.Width.Pixels - 380, 1f); 
			area.Top.Set(60, 0f); 
			area.Width.Set(120, 0f); 
			area.Height.Set(120, 0f);

			text = new UIText("0", 2f, true);
			text.Width.Set(120, 0f);
			text.Height.Set(67.5f, 0f);
			text.Top.Set(0, 0.5f);
			text.Left.Set(0, 0.5f);

			// This visual rotates to represent how much time is left before the counter resets
			// To properly center rotation, each corner has to be its own image

			float leftDisplace = 60f;
			float topDisplace = 33.75f;

			// Bottom right
			counterClockCorner1 = new UIImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
			counterClockCorner1.Left.Set(leftDisplace, 0.5f);
			counterClockCorner1.Top.Set(topDisplace, 0.5f);
			counterClockCorner1.Width.Set(90, 0f);
			counterClockCorner1.Height.Set(90, 0f);
			counterClockCorner1.Rotation = (float)(Math.PI * 2f);

			// Bottom Left
			counterClockCorner2 = new UIImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
			counterClockCorner2.Left.Set(leftDisplace, 0.5f);
			counterClockCorner2.Top.Set(topDisplace, 0.5f);
			counterClockCorner2.Width.Set(90, 0f);
			counterClockCorner2.Height.Set(90, 0f);
			counterClockCorner2.Rotation = counterClockCorner1.Rotation + (float)(Math.PI / 2f);

			// Top left
			counterClockCorner3 = new UIImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
			counterClockCorner3.Left.Set(leftDisplace, 0.5f);
			counterClockCorner3.Top.Set(topDisplace, 0.5f);
			counterClockCorner3.Width.Set(90, 0f);
			counterClockCorner3.Height.Set(90, 0f);
			counterClockCorner3.Rotation = counterClockCorner1.Rotation + (float)(Math.PI);

			// Top right
			counterClockCorner4 = new UIImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
			counterClockCorner4.Left.Set(leftDisplace, 0.5f);
			counterClockCorner4.Top.Set(topDisplace, 0.5f);
			counterClockCorner4.Width.Set(90, 0f);
			counterClockCorner4.Height.Set(90, 0f);
			counterClockCorner4.Rotation = counterClockCorner1.Rotation + (float)((3f* Math.PI) / 2f);

            area.Append(counterClockCorner1);
			area.Append(counterClockCorner2);
			area.Append(counterClockCorner3);
			area.Append(counterClockCorner4);
            area.Append(text);
			Append(area);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (ModContent.GetInstance<Config>().MeterType == "Letter Rank" || ModContent.GetInstance<Config>().MeterType == "Disabled")
			{
				return;
			}

			base.Draw(spriteBatch);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			SetComboClockVisual();

			// Check config and update UI position
			float xDisplacement = ModContent.GetInstance<Config>().hitCountSettings.counterDisplacement.X;
			float yDisplacement = ModContent.GetInstance<Config>().hitCountSettings.counterDisplacement.Y;
            if (ModContent.GetInstance<Config>().hitCountSettings.counterAltDisplacement)
            {
				xDisplacement = ModContent.GetInstance<Config>().hitCountSettings.counterNudgeX;
				yDisplacement = ModContent.GetInstance<Config>().hitCountSettings.counterNudgeY;
			}
			area.Left.Set((-area.Width.Pixels - 380) + xDisplacement, 1f);
			area.Top.Set(60 + yDisplacement, 0f);

			var modPlayer = Main.LocalPlayer.GetModPlayer<CounterPlayer>();
            counterClockCorner1.Rotation += modPlayer.rotSpeed;
			counterClockCorner2.Rotation = counterClockCorner1.Rotation + (float)(Math.PI / 2f);
			counterClockCorner3.Rotation = counterClockCorner1.Rotation + (float)(Math.PI);
			counterClockCorner4.Rotation = counterClockCorner1.Rotation + (float)((3f * Math.PI) / 2f);
			bool noRotat = modPlayer.rotSpeed == 0;
			bool debugReset = ModContent.GetInstance<Config>().hitCountSettings.uiResetDebug;
			bool nearMaxFloat = counterClockCorner4.Rotation > float.MaxValue - 2f;
			if (nearMaxFloat || (noRotat && debugReset)) 
			{
				counterClockCorner1.Rotation = (float)(Math.PI * 2f);
				counterClockCorner2.Rotation = counterClockCorner1.Rotation + (float)(Math.PI / 2f);
				counterClockCorner3.Rotation = counterClockCorner1.Rotation + (float)(Math.PI);
				counterClockCorner4.Rotation = counterClockCorner1.Rotation + (float)((3f * Math.PI) / 2f);
			}
			
			counterClockCorner1.Color = GetTieredColor(modPlayer.hitCounter);
			counterClockCorner1.Color.A = (byte)(counterClockCorner1.Color.A * 0.75f);
			counterClockCorner2.Color = GetTieredColor(modPlayer.hitCounter);
			counterClockCorner2.Color.A = (byte)(counterClockCorner2.Color.A * 0.75f);
			counterClockCorner3.Color = GetTieredColor(modPlayer.hitCounter);
			counterClockCorner3.Color.A = (byte)(counterClockCorner3.Color.A * 0.75f);
			counterClockCorner4.Color = GetTieredColor(modPlayer.hitCounter);
			counterClockCorner4.Color.A = (byte)(counterClockCorner4.Color.A * 0.75f);
			base.DrawSelf(spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			if (ModContent.GetInstance<Config>().MeterType == "Letter Rank" || ModContent.GetInstance<Config>().MeterType == "Disabled")
			{
				return;
			}

			var modPlayer = Main.LocalPlayer.GetModPlayer<CounterPlayer>();
			float scale = 2f;
			text.Top.Set(0, 0.5f);
			if (modPlayer.hitCounter > 99)
			{
				scale = 1.6f;
				text.Top.Set(0, 0.55f);
			}
			if (modPlayer.hitCounter > 999)
			{
				scale = 1.2f;
				text.Top.Set(0, 0.625f);
			}
			if (modPlayer.hitCounter > 9999)
			{
				scale = 0.9f;
				text.Top.Set(0, 0.65f);
			}
			if (modPlayer.hitCounter > 99999)
			{
				scale = 0.75f;
				text.Top.Set(0, 0.675f);
			}
			if (modPlayer.hitCounter > 999999)
			{
				scale = 0.6f;
				text.Top.Set(0, 0.695f);
			}
			if (modPlayer.hitCounter > 9999999)
			{
				scale = 0.5f;
				text.Top.Set(0, 0.7f);
			}
			if (modPlayer.hitCounter > 99999999)
			{
				scale = 0.45f;
				text.Top.Set(0, 0.7125f);
			}
			if (modPlayer.hitCounter > 999999999)
			{
				scale = 0.4f;
				text.Top.Set(0, 0.725f);
			}

			text.SetText($"{modPlayer.hitCounter}", scale, true);
			if(modPlayer.hitCounter > ModContent.GetInstance<Config>().hitCountSettings.maxCounterValue)
            {
				text.SetText("MAX", 1f, true); 
				text.Top.Set(0, 0.65f);
			}
			if (ModContent.GetInstance<Config>().hitCountSettings.hideCounter) 
			{
				text.SetText("", 2f, true); // Makes text blank to hide it
				text.Top.Set(0, 0.5f);
			}
			text.TextColor = GetTieredColor(modPlayer.hitCounter); 

			base.Update(gameTime);
		}

        /// <summary>
        /// Returns a color based on the inputted hit count.
        /// </summary>
        private static Color GetTieredColor(int hits)
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
				textColor = Color.Lerp(Color.LimeGreen, Color.SpringGreen, timing);
			}
			return textColor;
		}

        /// <summary>
        /// Sets the sprite used by the Combo Clock UI based on config.
        /// </summary>
        private void SetComboClockVisual()
        {
			counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
			counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
			counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
			counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
			// Based off the TModLoader logo
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Modders' Cog (Default)")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/TModCog"));
			}
			// Based off of EoL's Sun Dance
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Sun Dance")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SunDance"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SunDance"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SunDance"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SunDance"));
			}
			// Enlarged and adapted Light Disc sprite
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Light Disc")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Disc"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Disc"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Disc"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Disc"));
			}
			// Enlarged and adapted Loading Sunflower sprite
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Sunflower")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Flower"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Flower"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Flower"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Flower"));
			}
			// Adapted from part of the Lunatic Cultist's ritural FX
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Lightning Ritual")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Ritual"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Ritual"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Ritual"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Ritual"));
			}
			// Enlarged and adapted Thorn Chakram sprite
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Thorn Chakram")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Chakram"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Chakram"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Chakram"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Chakram"));
			}
			// Enlarged and adapted copper shortsword sprites, crossed
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Shortswords")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/ShortswordsA"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/ShortswordsB"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/ShortswordsC"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/ShortswordsD"));
			}
			// Enlarged and adapted 'secret' sunglasses sun sprite
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Sun")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SunA"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SunB"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SunC"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SunD"));
			}
			// Enlarged and adapted Boulder sprite
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Boulder")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/BoulderA"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/BoulderB"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/BoulderC"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/BoulderD"));
			}
			// Simple clock-like design
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Clock")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Clock"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Clock"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Clock"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Clock"));
			}
			// Simple crosshairs design
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Crosshair")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Crosshair"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Crosshair"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Crosshair"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Crosshair"));
			}
			// Modified Vampire Knives sprite, in dedication of Lienfors
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Tribute")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Tribute"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Tribute"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Tribute"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Tribute"));
			}
			// R O T A T E   B A N A N A
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Banana")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/BananaA"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/BananaB"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/BananaC"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/BananaD"));
			}
			// IM FREAKING SKELETRON
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Skeletron")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SkeletronA"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SkeletronB"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SkeletronC"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/SkeletronD"));
			}
			// Inspired by the combo UI from the Magalor Epilogue of Kirby's Return to Dreamland Deluxe
			if (ModContent.GetInstance<Config>().hitCountSettings.clockStyle == "Interdimensional Traveller")
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/MagalorA"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/MagalorB"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/MagalorA"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/MagalorB"));
			}

			// Seperate config that overrides the display with a blank one to hide the clock
			if (ModContent.GetInstance<Config>().hitCountSettings.hideClock)
			{
				counterClockCorner1.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Blank"));
				counterClockCorner2.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Blank"));
				counterClockCorner3.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Blank"));
				counterClockCorner4.SetImage(ModContent.Request<Texture2D>("BlocksCombos/UI/CounterClocks/Blank"));
			}
		}
	}

	class CounterUISystem : ModSystem
	{
		private UserInterface CounterMeterUserInterface;

		internal CounterMeter CounterMeter;

		public override void Load()
		{
			// All code below runs only if we're not loading on a server
			if (!Main.dedServ)
			{
				CounterMeter = new();
				CounterMeterUserInterface = new();
				CounterMeterUserInterface.SetState(CounterMeter);
			}
		}

		public override void UpdateUI(GameTime gameTime)
		{
			CounterMeterUserInterface?.Update(gameTime);
		}

		// Adds the UI onto the screen
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1)
			{
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"BlocksCombos: Counter Meter",
					delegate {
						CounterMeterUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
		
	}
}