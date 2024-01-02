using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdio.common.objects;
using gdio.unity_api;
using gdio.unity_api.v2;
using NUnit.Framework;

namespace GamedriverTests.Scenes
{
    class Scene_Home
    {
        // ------- SCENE NAME ------- //
        public string sn_Name = "HomeScene";

        // --------- PLAYER --------- //
        public string pl_MainCharacter = "//*[@name='Player']";

        // --------- TALK OPTION ----------//
        public string pl_TalkOption = "//*[@name='Player']//*[@name='Talk']";
                                        

        // ------ NPCS ------ //
        public string npc_mrNPC = "//*[@name='Mr.NPC']";


        // -------- DOORS --------- //
        public string door_outside = "//*[@name='Door_Outside']";


        // ----- DIALOG MANAGER ------- //
        public string dm_HomeDialogueManager = "//*[@name='DialogueManager']";


        // --- UNITY FN COMPONENTS -- //
        public string unityFn_Transform = "/fn:component('UnityEngine.Transform')";
        public string unityFn_Text = "/fn:component('UnityEngine.UI.Text')/@text";

        public bool Home_LoadScene(ApiClient api)
        {
            return api.LoadScene(sn_Name.ToString(), 30);
        }
        public bool Home_VerifyCurrentSceneName(string expectedScene, string currentScene)
        {
            return currentScene.ToString().CompareTo(expectedScene.ToString()) == 0;
        }
        public bool Home_NPCInteractionPossitive(ApiClient api)
        {
            bool flag = false;
            string currentScene = api.GetSceneName();
            bool correctSceneName = Home_VerifyCurrentSceneName(sn_Name, currentScene);

            if (correctSceneName)
            {
                Vector3 npcPosition = api.GetObjectPosition(npc_mrNPC);
                //Vector3 npcPositionToFailTest = new Vector3(10,10,0);
                
                api.SetObjectFieldValue((pl_MainCharacter + unityFn_Transform).ToString(), "position", npcPosition);
                api.Wait(1000);

                bool talkActive = api.GetObjectFieldValue<bool>("//*[@name='Talk']/@active");

                Assert.IsTrue(talkActive == true, "talk is active, test passed");
                flag = talkActive;

                api.Wait(1000);
            }
            return flag;
        }

        public bool Home_NPCInteractionNegative(ApiClient api)
        {
            bool flag = false;
            string currentScene = api.GetSceneName();
            bool correctSceneName = Home_VerifyCurrentSceneName(sn_Name, currentScene);

            if (correctSceneName)
            {
                //Vector3 npcPosition = api.GetObjectPosition(npc_mrNPC);
                Vector3 npcPositionToFailTest = new Vector3(10,10,0);

                api.SetObjectFieldValue((pl_MainCharacter + unityFn_Transform).ToString(), "position", npcPositionToFailTest);
                api.Wait(1000);

                bool talkActive = api.GetObjectFieldValue<bool>("//*[@name='Talk']/@active");

                Assert.IsTrue(talkActive == false, "talk is not active, test passed");
                flag = !talkActive;
            }
            return flag;
        }
    }
}
