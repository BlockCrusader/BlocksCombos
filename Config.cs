using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace BlocksCombos
{
	[Label("Main Config")]
	public class Config : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("HeaderMain1")]

		[BackgroundColor(195, 90, 70)]
		[DrawTicks]
		[OptionStrings(new string[] { "Letter Rank", "Hit Count", "Disabled", "Both" })]
		[DefaultValue("Letter Rank")]
		public string MeterType;

		[Header("HeaderMain2")]

		public LetterRankSettings letterRankSettings = new LetterRankSettings();
		[SeparatePage]
		[BackgroundColor(190, 130, 10)]
		public class LetterRankSettings
		{
			[Header("HeaderLetterP")]

			[BackgroundColor(10, 65, 190)]
			[Range(-1500f, 1500f)]
			[DefaultValue(typeof(Vector2), "0, 0")]
			public Vector2 rankDisplacement = new Vector2(0, 0);

			[BackgroundColor(10, 65, 190)]
			[DefaultValue(false)]
			public bool rankAltDisplacement = false;

			[BackgroundColor(10, 65, 190)]
			[Range(-1500, 1500)]
			[DefaultValue(0)]
			public int rankNudgeX = 0;

			[BackgroundColor(10, 65, 190)]
			[Range(-1500, 1500)]
			[DefaultValue(0)]
			public int rankNudgeY = 0;

			[Header("HeaderLetterM")]
			//[Header("Mechanics")]

			[BackgroundColor(190, 35, 10)]
			[Range(0.25f, 3f)]
			[Increment(0.25f)]
			[DefaultValue(1f)]
			public float rankRate = 1f;

			[BackgroundColor(190, 35, 10)]
			[Range(0.025f, 0.3f)]
			[Increment(0.025f)]
			[DefaultValue(0.15f)]
			public float rankDrain = 0.15f;

			[BackgroundColor(190, 35, 10)]
			[DrawTicks]
			[OptionStrings(new string[] { "S (Default)", "SS", "SSS", "U", "A (Reduces max rank)" })]
			[DefaultValue("S (Default)")]
			public string maxRank = "S (Default)";

			[BackgroundColor(190, 35, 10)]
			[DefaultValue(true)]
			public bool rankHurtPenalty = true;

			[BackgroundColor(190, 35, 10)]
			[DefaultValue(false)]
			public bool rankResetPenalty = false;

			[BackgroundColor(190, 35, 10)]
			[Range(0.1f, 2f)]
			[Increment(0.1f)]  
			[DefaultValue(0.5f)]
			public float rankPenalty = 0.5f;

			[BackgroundColor(190, 35, 10)]
			[DefaultValue(false)]
			public bool useCustomRanking = false;

			[BackgroundColor(190, 35, 10)]
			[Range(5, 25000000)]
			[DefaultValue(20000)]
			public int customRankBase = 20000;

			[Header("HeaderLetterD")]

			[BackgroundColor(10, 190, 35)]
			[DrawTicks]
			[OptionStrings(new string[] { "Fancy (Default)", "Valkyrie", "Retro", "Leaf", "TwigLeaf", "StoneGold", "Sticks", "Remix", "Golden", "Platinum Fancy", "Crimson Fancy", "Emerald Fancy", "Tribute", "Thin", "Fill-Only"})]
			[DefaultValue("Fancy (Default)")]
			public string meterStyle = "Fancy (Default)";

			[BackgroundColor(10, 190, 35)]
			[DefaultValue(false)]
			public bool hideMeterBG = false;

			[BackgroundColor(10, 190, 35)]
			[DefaultValue(false)]
			public bool hideLetter = false;

			[BackgroundColor(10, 190, 35)]
			[DefaultValue(false)]
			public bool hideMeter = false;

			[Header("HeaderLetterF")]
			
			[BackgroundColor(190, 190, 35)]
			[DefaultValue(false)]
			public bool rankingFeedback = false;

			[BackgroundColor(190, 190, 35)]
			[DefaultValue(true)]
			public bool positiveRankFeedback = true;

			[BackgroundColor(190, 190, 35)]
			[DefaultValue(false)]
			public bool forcedRankFeedback = false;

			[BackgroundColor(190, 190, 35)]
			[DrawTicks]
			[OptionStrings(new string[] { "Disabled (Default)", "Twinkle", "Mana Crystal", "Golf Goal", "Recording", "Research", "Achievement", "Bell", "Meowmere", "Cymbal", "Duck", "Whistle" })]
			[DefaultValue("Disabled (Default)")]
			public string rankSFX = "Disabled (Default)";

			[BackgroundColor(190, 190, 35)]
			[DrawTicks]
			[OptionStrings(new string[] { "Disabled (Default)", "Shatter", "Bomb", "Whoopie Cushion", "Ice Shatter", "Flask Shatter", "Deadly Sphere Staff", "Ritual", "Metal", "Cymbal", "Whistle", "Duck" })]
			[DefaultValue("Disabled (Default)")] 
			public string demoteSFX = "Disabled (Default)";
		}

		public HitCountSettings hitCountSettings = new HitCountSettings();
		[SeparatePage]
		[BackgroundColor(120, 65, 160)]
		public class HitCountSettings
		{
			[Header("HeaderClockP")]

			[BackgroundColor(65, 65, 185)]
			[Range(-1500f, 1500f)]
			[DefaultValue(typeof(Vector2), "0, 0")]
			public Vector2 counterDisplacement = new Vector2(0, 0);

			[BackgroundColor(65, 65, 185)]
			[DefaultValue(false)]
			public bool counterAltDisplacement = false;

			[BackgroundColor(65, 65, 185)]
			[Range(-1500, 1500)]
			[DefaultValue(0)]
			public int counterNudgeX = 0;

			[BackgroundColor(65, 65, 185)]
			[Range(-1500, 1500)]
			[DefaultValue(0)]
			public int counterNudgeY = 0;

			[Header("HeaderClockM")]

			[BackgroundColor(185, 65, 65)]
			[Range(0.5f, 3f)]
			[Increment(0.25f)]
			[DefaultValue(1f)]
			public float clockDuration = 1f;

			[BackgroundColor(185, 65, 65)]
			[DefaultValue(true)]
			public bool counterHurtPenalty = true;

			[Header("HeaderClockD")]

			[BackgroundColor(75, 185, 65)]
			[DrawTicks]
			[OptionStrings(new string[] { "Modders' Cog (Default)", "Clock", "Sun Dance", "Thorn Chakram", "Light Disc", "Sunflower", "Lightning Ritual", "Crosshair", "Shortswords", "Sun", "Boulder", "Skeletron", "Banana", "Interdimensional Traveller", "Tribute" })]
			[DefaultValue("Modders' Cog (Default)")]
			public string clockStyle = "Modders' Cog (Default)";

			[BackgroundColor(75, 185, 65)]
			[Range(0f, 1f)]
			[Increment(0.05f)]
			[DefaultValue(0.2f)]
			public float clockSpinMult = 0.2f;

			[BackgroundColor(75, 185, 65)]
			[DefaultValue(false)]
			public bool reverseSpin = false;

			[BackgroundColor(75, 185, 65)]
			[DefaultValue(false)]
			public bool hideClock = false;

			[BackgroundColor(75, 185, 65)]
			[DefaultValue(false)]
			public bool hideCounter = false;

			[BackgroundColor(75, 185, 65)]
			[Range(2499, 2000000000)]
			[DefaultValue(9999)]
			public int maxCounterValue = 9999;

			[Header("HeaderClockF")]

			[BackgroundColor(185, 165, 65)]
			[DefaultValue(true)]
			public bool comboHitTotals = true;

			[BackgroundColor(185, 165, 65)]
			[DefaultValue(true)]
			public bool forcedTotals = true;

			[BackgroundColor(185, 165, 65)]
			[Range(99, 2000000000)]
			[DefaultValue(999)]
			public int largeComboThreshold = 999;

			[BackgroundColor(185, 165, 65)]
			[DrawTicks]
			[OptionStrings(new string[] { "Disabled (Default)", "Twinkle", "Mana Crystal", "Golf Goal", "Recording", "Research", "Achievement", "Bell", "Meowmere", "Cymbal", "Duck", "Whistle" })]
			[DefaultValue("Disabled (Default)")]
			public string largeComboSFX = "Disabled (Default)";

			[Header("HeaderClockDebug")]

			[BackgroundColor(115, 115, 115)]
			[DefaultValue(false)]
			public bool uiResetDebug = false;
		}
	}
}
