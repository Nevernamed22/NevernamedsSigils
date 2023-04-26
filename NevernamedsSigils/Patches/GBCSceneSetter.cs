using GBC;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NevernamedsSigils.Patches
{/*
    [HarmonyPatch] // <-- This annotation lets Harmony find the class.
    public class Example
    {
        [HarmonyPatch(typeof(CameraController), nameof(CameraController.Start))]
        [HarmonyPostfix]
        private static void ExamplePatch()
        {
            // If scene isn't "GBC_CardBattle", return. (As this patch is specific for that scene.)
            if (SceneManager.GetActiveScene().name != "GBC_CardBattle") return;

            Debug.Log("Patch ran");

            Sprite character = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/eyeforbattle_pixel.png"));

            Transform pixelCardBattle = GameObject.Find("PixelCardBattle").transform;

            GameObject dummy = new GameObject("Dummy");
            dummy.transform.SetParent(pixelCardBattle);
            dummy.transform.localPosition = new Vector3(2.0555f, 0.97f, 0f); // You'll have to tweak this position yourself.
            dummy.layer = LayerMask.NameToLayer("GBCPixel");
            SpriteRenderer sr = dummy.AddComponent<SpriteRenderer>();
            sr.sprite = character;
            
        }
    }*/
}
