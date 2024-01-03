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
    //SCENE NAME: HomeScene.unity
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
        public string door_Outside = "//*[@name='Door_Outside']";


        // ----- DIALOG MANAGER CONTINUE BUTTON------- //
        public string btn_ContinueDialouge = "//Untagged[@name='Canvas']//Untagged[@name='Dialogue Box']//Untagged[@name='Continue Button']";

        // --- UNITY FN COMPONENTS -- //
        public string unityFn_Transform = "/fn:component('UnityEngine.Transform')";
        public string unityFn_Text = "/fn:component('UnityEngine.UI.Text')/@text";


        public string unityFn_TextMeshPro = "/fn:component('TextMeshProUGUI')/@text";

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
 
            Vector3 npcPosition = api.GetObjectPosition(npc_mrNPC);
            //Vector3 npcPositionToFailTest = new Vector3(10,10,0);            
            api.SetObjectFieldValue((pl_MainCharacter + unityFn_Transform).ToString(), "position", npcPosition);
            api.Wait(1000);
            
            bool talkActive = api.GetObjectFieldValue<bool>("//*[@name='Talk']/@active");
            
            flag = talkActive;
            api.Wait(1000);
            return flag;
        }

        public bool Home_NPCInteractionNegative(ApiClient api)
        {
            bool flag = false;

            Vector3 npcPositionToFailTest = new Vector3(-6.5f,0,0);
            api.SetObjectFieldValue((pl_MainCharacter + unityFn_Transform).ToString(), "position", npcPositionToFailTest);
            api.Wait(1000);

            bool talkActive = api.GetObjectFieldValue<bool>("//*[@name='Talk']/@active");
            
            flag = !talkActive;
            return flag;
        }

        public bool Home_TestDialogue(ApiClient api)
        {
            bool flag = false;
            bool flag1 = false;
            bool flag2 = false;

            // I know there are 2 sentences in the dialog, Will need changed for dynamic amounts of dialogue which is possible
            api.ButtonPress("Fire1", 5,30); // start the dialogue
            flag = api.ClickObject(MouseButtons.LEFT, btn_ContinueDialouge, 5); // continue once
            api.Wait(3000);
            flag1 = api.ClickObject(MouseButtons.LEFT, btn_ContinueDialouge, 5); // continue twice
            api.Wait(3000);

            string dialogue = api.GetObjectFieldValue<string>("//Untagged[@name='Canvas']//Untagged[@name='Dialogue Box']//Untagged[@name='Dialogue (1)']" + unityFn_Text);

            if (dialogue.Equals("Do you know the muffin man? o.O"))
                flag2 = true;

            if (flag && flag1)
                return true;
            else
                return false;
        }

        public bool Home_MoveToDoor(ApiClient api)
        {
            bool flag = false;
            Vector3 doorPosition = api.GetObjectPosition(door_Outside);

            api.SetObjectFieldValue((pl_MainCharacter + unityFn_Transform).ToString(), "position", doorPosition);
            api.Wait(1000);

            Home_VerifyCurrentSceneName("OutsideScene", api.GetSceneName());
            flag = true;
            return flag;
        }
    }
}
