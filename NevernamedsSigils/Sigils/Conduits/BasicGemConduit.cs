using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpponentBones;
using UnityEngine;
using GBC;

namespace NevernamedsSigils
{
    public class BasicGemConduit : Conduit
    {
        public override Ability Ability
        {
            get
            {
                return Ability.None;
            }
        }
        public virtual Ability GemAbility() { return Ability.GainGemBlue; }
        public virtual GemType GemColour() { return GemType.Blue; }
        public virtual string GainText() { return ""; }
        public virtual string LoseText() { return ""; }
        public virtual TextBox.Style TextStyle() { return TextBox.Style.Neutral; }
        public CardModificationInfo gemMod;
        bool completedConduitLastRecalc = false;
        public override void ManagedFixedUpdate()
        {
            if (!base.Card.Dead)
            {
                bool conduitThisRecalculate = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).Exists(x => Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(x).Contains(base.Card));
                if (conduitThisRecalculate != completedConduitLastRecalc)
                {
                    if (gemMod == null) { gemMod = new CardModificationInfo() { abilities = new List<Ability>() { GemAbility() }, singletonId = "GemConduitMod", nonCopyable = true }; }
                    //base.Card.Status.hiddenAbilities.Add(GemAbility());
                    if (conduitThisRecalculate) //Gain Sigil
                    {
                        base.Card.AddTemporaryMod(gemMod);
                    }
                    else //Lose Sigil
                    {
                        if (gemMod == null)
                        {
                            gemMod = base.Card.TemporaryMods.Find(x => x.singletonId == "GemConduitMod");
                        }
                        if (gemMod != null) { base.Card.RemoveTemporaryMod(gemMod); }
                    }
                    base.Card.RenderCard();
                    StartCoroutine(LateRecalculateGem());
                    completedConduitLastRecalc = conduitThisRecalculate;
                }
                base.ManagedFixedUpdate();
            }
        }
        public IEnumerator LateRecalculateGem()
        {
            yield return null;
            Singleton<ResourcesManager>.Instance.ForceGemsUpdate();
            yield break;
        }
    }
}
