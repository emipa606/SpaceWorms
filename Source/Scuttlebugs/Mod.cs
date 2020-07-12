using UnityEngine;
using Verse;

namespace Scuttlebugs
{
    [StaticConstructorOnStartup]
    internal class ScuttlebugsMod : Mod
    {
        /// <summary>
        /// Cunstructor
        /// </summary>
        /// <param name="content"></param>
        public ScuttlebugsMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ScuttlebugsSettings>();
        }

        /// <summary>
        /// The instance-settings for the mod
        /// </summary>
        internal ScuttlebugsSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = GetSettings<ScuttlebugsSettings>();
                }
                return settings;
            }
            set
            {
                settings = value;
            }
        }

        /// <summary>
        /// The title for the mod-settings
        /// </summary>
        /// <returns></returns>
        public override string SettingsCategory()
        {
            return "Space Worms";
        }

        /// <summary>
        /// The settings-window
        /// For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
        /// </summary>
        /// <param name="rect"></param>
        public override void DoSettingsWindowContents(Rect rect)
        {
            settings.ChangeDef();
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect);
            listing_Standard.Label("Space Worms incident base-chance: " + ScuttlebugsSettings.IncidentChance, -1, "Default value is 1.5");
            ScuttlebugsSettings.IncidentChance = Widgets.HorizontalSlider(listing_Standard.GetRect(20), ScuttlebugsSettings.IncidentChance, 0, 10f, false, "Chance", null, null, 0.01f);
            listing_Standard.End();
            settings.Write();
            settings.ChangeDef();
        }

        /// <summary>
        /// The private settings
        /// </summary>
        private ScuttlebugsSettings settings;

    }
}
