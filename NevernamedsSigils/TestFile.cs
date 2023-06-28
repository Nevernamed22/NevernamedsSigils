using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Card;
using InscryptionAPI;
using System.Collections;

namespace NevernamedsSigils
{
    public static class CustomCostManager
    {
        public static void RegisterCustomCost(string costProperty, BasicCustomCost handler)
        {
            if (!costStorage.ContainsKey(costProperty))
            {
                costStorage.Add(costProperty, handler);
            }
        }
        public static BasicCustomCost GetHandler(string costProperty)
        {
            if (costStorage.ContainsKey(costProperty))
            {
                return costStorage[costProperty];
            }
            else return null;
        }
        public static List<string> GetAllRegisteredCostProperties(CardInfo card = null)
        {
            List<string> toReturn = new List<string>() { };
            if (card != null)
            {
                foreach (string property in costStorage.Keys) { if (card.GetExtendedProperty(property) != null) { toReturn.Add(property); } }
            }
            else
            {
                toReturn.AddRange(costStorage.Keys);
            }
            return toReturn;
        }
        private static Dictionary<string, BasicCustomCost> costStorage = new Dictionary<string, BasicCustomCost>() { };
    }
    public class BasicCustomCost : ScriptableObject
    {
        public virtual Texture GetCostTexture(PlayableCard card)
        {
            return null;
        }
        public virtual Texture GetPixelCostTexture(PlayableCard card, bool APISmallCostEnabled)
        {
            return null;
        }
        public virtual bool CostSatisfied(PlayableCard card)
        {
            return true;
        }
        public virtual string CostUnsatisfiedText(PlayableCard card)
        {
            return null;
        }
        public virtual int CostPointValue(PlayableCard card)
        {
            return 0;
        }
        public virtual int CostPointValue(string costAmount)
        {
            return 0;
        }
        public virtual IEnumerator OnPlayed(PlayableCard card) { yield break; }
        public virtual bool RespondsToOnSelectSlotsSequence(PlayableCard card) { return false; }
        public virtual IEnumerator OnSelectSlotsSequence(PlayableCard card) { yield break; }
    }
    public class ClearGemExample : BasicCustomCost
    {
        public static void Init()
        {
            CustomCostManager.RegisterCustomCost("ClearGemsCost", ScriptableObject.CreateInstance<ClearGemExample>());
        }
        public int GetClearGemsCount(PlayableCard card)
        {
            int? cost = card.Info.GetExtendedPropertyAsInt("ClearGemsCost");
            if (cost != null) { return (int)cost; }
            return 0;
        }
        public override int CostPointValue(PlayableCard card)
        {
            return GetClearGemsCount(card) * 2;
        }
        public override int CostPointValue(string cost)
        {
            int toreturn = 0;
            bool succeed = int.TryParse(cost, out toreturn);
            toreturn = succeed ? toreturn : 0;
            return toreturn * 2;
        }
        public override bool CostSatisfied(PlayableCard card)
        {
            if (Singleton<BoardManager>.Instance.GetSlots(!card.OpponentCard).FindAll(x => x.Card != null && x.Card.Info.traits.Contains(Trait.Gem)).Count >= GetClearGemsCount(card))
            {
                return true;
            }
            return false;
        }
        public override string CostUnsatisfiedText(PlayableCard card)
        {
            return $"You don't have enough gems to play that {card.Info.DisplayedNameLocalized}!";
        }

        public override Texture GetCostTexture(PlayableCard card)
        {
            if (GetClearGemsCount(card) == 3)
            {
                return null;
            }
            else if (GetClearGemsCount(card) == 2)
            {
                return null;
            }
            else
            {
                return null;
            }
        }
        public override Texture GetPixelCostTexture(PlayableCard card, bool APISmallCostEnabled)
        {
            if (APISmallCostEnabled)
            {
                if (GetClearGemsCount(card) == 3)
                {
                    return null;
                }
                else if (GetClearGemsCount(card) == 2)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (GetClearGemsCount(card) == 3)
                {
                    return null;
                }
                else if (GetClearGemsCount(card) == 2)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
