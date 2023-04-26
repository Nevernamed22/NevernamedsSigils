using System;
using System.Collections.Generic;
using System.Text;
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using GBC;

namespace NevernamedsSigils
{
    public static class CustomAppearances
    {
        public static CardAppearanceBehaviour.Appearance GreyBackground;
        public static CardAppearanceBehaviour.Appearance GrueAppearance;
        public static CardAppearanceBehaviour.Appearance RareTerrainBackground;
        public static CardAppearanceBehaviour.Appearance TechPaperCardBackground;
        public static CardAppearanceBehaviour.Appearance TestRedBackground;

        public static CardAppearanceBehaviour.Appearance PixelBloodDecal;

        public static void Init()
        {
            GreyBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "GreyCardBackground", typeof(GreyCardBackground)).Id;
            GrueAppearance = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "GrueAppearance", typeof(GrueBackground)).Id;
            RareTerrainBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "RareTerrainBackground", typeof(RareTerrainBackground)).Id;
            TechPaperCardBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "TechPaperCardBackground", typeof(TechPaperCardBackground)).Id;

            TestRedBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "TestRedBackground", typeof(TestRedCardPixel)).Id;
            PixelBloodDecal = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "PixelBloodDecal", typeof(PixelBloodDecal)).Id;

        }

    }
    public class PixelBloodDecal : PixelAppearanceBehaviour
    {
        public override Sprite PixelAppearance()
        {
            return Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/pixelblooddecal.png"));
        }
    }
    public class TestRedCardPixel : PixelAppearanceBehaviour
    {
        public override Sprite OverrideBackground()
        {
            return Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/Appearances/redbackground.png"));
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
