using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Gorge : BloodActivatedAbility
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gorge", "If [creature] has less than 10 health, pay 1 blood to grant it +3 health.",
                      typeof(Gorge),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/gorge.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/gorge_pixel.png"),
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return !playerUpkeep && base.Card.Health < 10 &&  base.Card.OpponentCard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (base.Card && base.Card.slot && base.Card.slot.opposingSlot && base.Card.slot.opposingSlot.Card != null)
            {
                if (base.Card.slot.opposingSlot.Card.Attack >= base.Card.Health)
                {
                    List<CardSlot> cardslots = Singleton<BoardManager>.Instance.GetSlots(false).FindAll(x => x.Card && x.Card.CanBeSacrificed && x.Card.PowerLevel < base.Card.PowerLevel);
                    if (cardslots.Count > 0)
                    {
                        yield return Tools.SeededRandomElement(cardslots).Card.Die(true, null);
                        yield return new WaitForSeconds(0.15f);
                        base.Card.Anim.StrongNegationEffect();
                        base.Card.AddTemporaryMod(new CardModificationInfo(0, 3));
                    }
                }
            }
            yield break;
        }
        public override int BloodRequired()
        {
            return 1;
        }
        public override bool AdditionalActivationParameters()
        {
            return base.Card.Health < 10;
        }
        public override IEnumerator OnBloodAbilityPostAllSacrifices()
        {
            yield return new WaitForSeconds(0.15f);
            base.Card.Anim.StrongNegationEffect();
            base.Card.AddTemporaryMod(new CardModificationInfo(0, 3));
            yield break;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

    }
}