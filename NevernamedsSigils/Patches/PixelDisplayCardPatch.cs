using DiskCardGame;
using GBC;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(PixelCardAbilityIcons), nameof(PixelCardAbilityIcons.DisplayAbilities), typeof(List<Ability>), typeof(PlayableCard))]
    public class PixelDisplayCardsPatch
    {
        [HarmonyPrefix]
        public static void PixelDisplayCards(List<Ability> abilities, PlayableCard card)
        {
           if (card != null)
            {
                for (int i = abilities.Count -1; i >= 0; i--)
                {
                    if (card.Status.hiddenAbilities.Contains(abilities[i]))
                    {
                        abilities.RemoveAt(i);
                    }
                }
                foreach (CardModificationInfo info in card.temporaryMods.FindAll((x) => x.abilities.Count > 0))
                {
                    foreach (Ability ab in info.abilities)
                    {
                        if (!card.Status.hiddenAbilities.Contains(ab)) abilities.Add(ab);
                    }
                }

               
            }
        }
    }
}
