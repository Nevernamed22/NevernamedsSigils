using System;
using System.Collections.Generic;
using System.Text;
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using GBC;
using InscryptionAPI.PixelCard;
//using InscryptionAPI.PixelCard;

namespace NevernamedsSigils
{
    public static class CustomAppearances
    {
        public static CardAppearanceBehaviour.Appearance HiddenStats;
        public static CardAppearanceBehaviour.Appearance GreyBackground;
        public static CardAppearanceBehaviour.Appearance LeshyCardBackground;
        public static CardAppearanceBehaviour.Appearance InstantEffectBackground;
        public static CardAppearanceBehaviour.Appearance GrueAppearance;
        public static CardAppearanceBehaviour.Appearance RareTerrainBackground;
        public static CardAppearanceBehaviour.Appearance TechPaperCardBackground;
        public static CardAppearanceBehaviour.Appearance TestRedBackground;
        public static CardAppearanceBehaviour.Appearance PixelGravestoneCard;

        public static CardAppearanceBehaviour.Appearance PixelBloodDecal;
        public static CardAppearanceBehaviour.Appearance PixelGooDecal;
        public static CardAppearanceBehaviour.Appearance PixelSmokeDecal;
        public static CardAppearanceBehaviour.Appearance PixelChaosCardDecal;

        //Act 2 Card Backs
        public static CardAppearanceBehaviour.Appearance PixelDeathcardBackground;
        public static CardAppearanceBehaviour.Appearance PixelRareBackground;
        public static CardAppearanceBehaviour.Appearance PixelGoldBackground;

        //Act 2 Boss Card Headers
        public static CardAppearanceBehaviour.Appearance PixelBoneHeader;
        public static CardAppearanceBehaviour.Appearance PixelScrappyHeader;
        public static CardAppearanceBehaviour.Appearance PixelTechHeader;
        public static CardAppearanceBehaviour.Appearance PixelSparkleHeader;

        //Act 2 Boss Card Decals
        public static CardAppearanceBehaviour.Appearance PixelDogBiteDecal;
        public static CardAppearanceBehaviour.Appearance PixelSnowflakeDecal;
        public static CardAppearanceBehaviour.Appearance PixelShipDecal;
        public static CardAppearanceBehaviour.Appearance PixelWaterDamageDecal;

        //Act 2 Boss Card Cardbacks
        public static CardAppearanceBehaviour.Appearance PixelGoobertBackground;
        public static CardAppearanceBehaviour.Appearance PixelPikeMageBackground;
        public static CardAppearanceBehaviour.Appearance PixelLonelyWizBackground;
        public static CardAppearanceBehaviour.Appearance PixelAnglerBackground;
        public static CardAppearanceBehaviour.Appearance PixelCharredBackground;

        public static void Init()
        {
            //Backgrounds
            GreyBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "GreyCardBackground", typeof(GreyCardBackground)).Id;
            GrueAppearance = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "GrueAppearance", typeof(GrueBackground)).Id;
            RareTerrainBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "RareTerrainBackground", typeof(RareTerrainBackground)).Id;
            TechPaperCardBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "TechPaperCardBackground", typeof(TechPaperCardBackground)).Id;
            InstantEffectBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "InstantEffectBackground", typeof(InstantEffectCardBackground)).Id;
            LeshyCardBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "CabinFinaleOpponentCardBackground", typeof(LeshyCardBackground)).Id;

            HiddenStats = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "HiddenStats", typeof(HiddenStats)).Id;
            //Decals

            // TestRedBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "TestRedBackground", typeof(TestRedCardPixel)).Id;
            //PixelGravestoneCard = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelGravestoneCard", typeof(PixelGravestoneCard)).Id;

            PixelBloodDecal = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelBloodDecal", typeof(PixelBloodDecal)).Id;
            PixelGooDecal = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelGooDecal", typeof(PixelGooDecal)).Id;
            PixelSmokeDecal = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelSmokeDecal", typeof(PixelSmokeDecal)).Id;

            PixelChaosCardDecal = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelChaosCard", typeof(PixelChaosCardDecal)).Id;

            //Act 2 Card Backgrounds
            PixelDeathcardBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelDeathcardBackground", typeof(PixelDeathcardBackground)).Id;
            PixelRareBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelRareBackground", typeof(PixelRareCardBackground)).Id;
            PixelGoldBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelGoldBackground", typeof(PixelGoldCardBackground)).Id;

            //Act 2 Boss Card Headers
            PixelBoneHeader = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelBoneHeader", typeof(PixelBoneHeader)).Id;
            PixelScrappyHeader = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelScrappyHeader", typeof(PixelScrappyHeader)).Id;
            PixelTechHeader = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelTechHeader", typeof(PixelTechHeader)).Id;
            PixelSparkleHeader = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelSparkleHeader", typeof(PixelSparkleHeader)).Id;

            //Act 2 Boss Card Decals
            PixelDogBiteDecal = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelDogBiteDecal", typeof(PixelDogBiteDecal)).Id;
            PixelSnowflakeDecal = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelSnowflakeDecal", typeof(PixelSnowflakeDecal)).Id;
            PixelShipDecal = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelShipDecal", typeof(PixelShipDecal)).Id;
            PixelWaterDamageDecal = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelWaterDamageDecal", typeof(PixelWaterDamageDecal)).Id;

            //Act 2 Boss Card Backgrounds
            PixelGoobertBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelGoobertBackground", typeof(GoobertBackgroundPixel)).Id;
            PixelPikeMageBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelPikeMageBackground", typeof(PikeMageBackgroundPixel)).Id;
            PixelLonelyWizBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelLonelyWizBackground", typeof(LonelyWizBackgroundPixel)).Id;
            PixelAnglerBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelAnglerBackground", typeof(AnglerBackgroundPixel)).Id;
            PixelCharredBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelCharredBackground", typeof(CharredBackgroundPixel)).Id;
        }
    }
    //Act 2 Card Backgrounds
    #region PixelCardBackgrounds  
    public class PixelSmokeDecal : PixelAppearanceBehaviour
    {
        public override Sprite PixelAppearance() { return Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/pixelsmokedecal.png")); }
    }

    public class PixelGooDecal : PixelAppearanceBehaviour
    {
        public override Sprite PixelAppearance() { return Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/pixelgoodecal.png")); }
    }

    public class PixelGoldCardBackground : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/goldcardback_pixel.png"));
        public static Sprite texrare = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/goldcardbackrare_pixel.png"));
        public override Sprite OverrideBackground()
        {
            if (base.Card != null && base.Card.Info != null && base.Card.Info.metaCategories.Contains(CardMetaCategory.Rare)) { return texrare; }
            else { return tex; }
        }
    }
    public class PixelDeathcardBackground : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/pixel_card_empty_deathcard.png"));
        public override Sprite OverrideBackground() { return tex; }
    }
    public class PixelRareCardBackground : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/pixel_card_empty_rare.png"));
        public override Sprite OverrideBackground() { return tex; }
    }
    #endregion
    public class HiddenStats : CardAppearanceBehaviour
    {
        public override void ApplyAppearance() { this.UpdateAttackHidden(); }
        public override void OnPreRenderCard() { this.UpdateAttackHidden(); }
        private void UpdateAttackHidden()
        {
            base.Card.RenderInfo.hiddenAttack = true;
            base.Card.RenderInfo.hiddenHealth = true;
            if (base.Card is PlayableCard && (base.Card as PlayableCard).Attack > 0) { base.Card.RenderInfo.hiddenAttack = false; }
            if (base.Card is PlayableCard && (base.Card as PlayableCard).Health > 0) { base.Card.RenderInfo.hiddenHealth = false; }
        }
    }
    public class PixelSparkleHeader : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/sparkleheader_pixel.png"));
        public override Sprite PixelAppearance() { return tex; }
    }
    public class PixelChaosCardDecal : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/pixelchaoscard.png"));
        public override Sprite PixelAppearance() { return tex; }
    }
    public class PixelWaterDamageDecal : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/waterdamagedecal_pixel.png"));
        public override Sprite PixelAppearance() { return tex; }
    }
    public class CharredBackgroundPixel : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/charredbackground_pixel.png"));
        public override Sprite OverrideBackground() { return tex; }
    }
    public class AnglerBackgroundPixel : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/anglerbackground_pixel.png"));
        public override Sprite OverrideBackground() { return tex; }
    }
    public class LonelyWizBackgroundPixel : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/lonelywizardbackground_pixel.png"));
        public override Sprite OverrideBackground() { return tex; }
    }
    public class PikeMageBackgroundPixel : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/pikemagebackground_pixel.png"));
        public override Sprite OverrideBackground() { return tex; }
    }
    public class GoobertBackgroundPixel : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/goobertbackground_pixel.png"));
        public override Sprite OverrideBackground() { return tex; }
    }
    public class PixelShipDecal : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/shipdecal_pixel.png"));
        public override Sprite PixelAppearance() { return tex; }
    }
    public class PixelSnowflakeDecal : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/snowflakedecal_pixel.png"));
        public override Sprite PixelAppearance() { return tex; }
    }
    public class LeshyCardBackground : CardAppearanceBehaviour
    {
        public static Texture2D tex = Tools.LoadTex("NevernamedsSigils/Resources/Appearances/leshycard.png");
        public override void ApplyAppearance() { base.Card.renderInfo.baseTextureOverride = tex; }
    }
    public class PixelDogBiteDecal : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/dogbitedecal_pixel.png"));
        public override Sprite PixelAppearance() { return tex; }
    }
    public class PixelTechHeader : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/techheader_pixel.png"));
        public override Sprite PixelAppearance() { return tex; }
    }
    public class PixelScrappyHeader : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/scrapedheader_pixel.png"));
        public override Sprite PixelAppearance() { return tex; }
    }
    public class PixelBoneHeader : PixelAppearanceBehaviour
    {
        public static Sprite tex = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/boneheader_pixel.png"));
        public override Sprite PixelAppearance() { return tex; }
    }
    public class PixelBloodDecal : PixelAppearanceBehaviour
    {
        public override Sprite PixelAppearance()
        {
            return Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/pixelblooddecal.png"));
        }
    }
    /*
     public class TestRedCardPixel : PixelAppearanceBehaviour
     {
         public override Sprite OverrideBackground()
         {
             return Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/redbackground.png"));
         }
     }
     public class PixelGravestoneCard : PixelAppearanceBehaviour
     {
         public override Sprite OverrideBackground()
         {
             return Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/pixeltombstone.png"));
         }
     }*/

    public class InstantEffectCardBackground : CardAppearanceBehaviour
    {
        public static Texture2D instantEffectTex = Tools.LoadTex("NevernamedsSigils/Resources/Appearances/instanteffect_cardback.png");
        public static Texture2D instantEffectRareTex = Tools.LoadTex("NevernamedsSigils/Resources/Appearances/instanteffect_cardback_rare.png");
        public override void ApplyAppearance()
        {
            if (base.Card.Info != null && base.Card.Info.metaCategories.Contains(CardMetaCategory.Rare))
            {
                base.Card.renderInfo.baseTextureOverride = instantEffectRareTex;
            }
            else { base.Card.renderInfo.baseTextureOverride = instantEffectTex; }
        }
    }
    public class GreyCardBackground : CardAppearanceBehaviour
    {
        public override void ApplyAppearance()
        {
            base.Card.renderInfo.baseTextureOverride = Tools.LoadTex("NevernamedsSigils/Resources/Appearances/grey_cardback.png");
        }
    }
    public class TechPaperCardBackground : CardAppearanceBehaviour
    {
        public override void ApplyAppearance()
        {
            base.Card.renderInfo.baseTextureOverride = Tools.LoadTex("NevernamedsSigils/Resources/Appearances/tech_bg.png");
        }
    }
    public class GrueBackground : CardAppearanceBehaviour
    {
        public override void ApplyAppearance()
        {
            base.Card.renderInfo.baseTextureOverride = Tools.LoadTex("NevernamedsSigils/Resources/Appearances/grue_cardback.png");
            base.Card.renderInfo.attackTextColor = Color.white;
            base.Card.renderInfo.healthTextColor = Color.white;
            base.Card.renderInfo.healthTextOffset = new Vector2(0, 0.07f);
            base.Card.renderInfo.defaultAbilityColor = Color.white;
        }
    }
    public class RareTerrainBackground : CardAppearanceBehaviour
    {
        public override void ApplyAppearance()
        {
            base.Card.renderInfo.baseTextureOverride = Tools.LoadTex("NevernamedsSigils/Resources/Appearances/rareterrain_cardback.png");
        }
    }
}
