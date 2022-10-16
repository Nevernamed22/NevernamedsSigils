using System;
using System.Collections.Generic;
using System.Text;
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace NevernamedsSigils
{
    public static class CustomAppearances
    {
        public static CardAppearanceBehaviour.Appearance GreyBackground;
        public static CardAppearanceBehaviour.Appearance GrueAppearance;
        public static CardAppearanceBehaviour.Appearance RareTerrainBackground;
        public static void Init()
        {
            GreyBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "GreyCardBackground", typeof(GreyCardBackground)).Id;
            GrueAppearance = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "GrueAppearance", typeof(GrueBackground)).Id;
            RareTerrainBackground = CardAppearanceBehaviourManager.Add("nevernamed.inscryption.sigils", "RareTerrainBackground", typeof(RareTerrainBackground)).Id;
        }
        
    }
    public class GreyCardBackground : CardAppearanceBehaviour
    {
        public override void ApplyAppearance()
        {
            base.Card.renderInfo.baseTextureOverride = Tools.LoadTex("NevernamedsSigils/Resources/Appearances/grey_cardback.png");
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
