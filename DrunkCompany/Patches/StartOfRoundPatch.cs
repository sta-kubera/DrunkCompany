//using GameNetcodeStuff;
//using HarmonyLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

////namespace DrunkCompany.Patches
////{
////	[HarmonyPatch(typeof(StartOfRound))]
////	internal class StartOfRoundPatch
////	{
////		[HarmonyPatch("Start")]
////		[HarmonyPostfix]
////		public static void StartPostFix()
////		{
////			PlayerControllerB player = GameNetworkManager.Instance.localPlayerController; // local player
////			UnlockableSuit[] suits = StartOfRound.Instance.unlockablesList.unlockables.Where(u => u.unlockableType == 0); // 0 = suit
////			UnlockableSuit suit = ...; // idk, it can be random if you want
////			suit.SwitchSuitToThis(player);
////		}
////	}
////}
