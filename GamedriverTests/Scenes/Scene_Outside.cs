using gdio.common.objects;
using gdio.unity_api.v2;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamedriverTests.Scenes
{

    //SCENE NAME: OutsideScene.unity
    class Scene_Outside
    {

        // ------- SCENE NAME ------- //
        public string sn_Name = "OutsideScene";

        // --------- PLAYER --------- //
        public string pl_MainCharacter = "//*[@name='Player']";

        // --------- PLANT OPTION ---------- //
        public string pl_plantOption = "//*[@name='Player']//*[@name='Plant']";

        // ---------- GARDEN 0 ----------- //
        public string gdn_0 = "//Garden[@name='Garden']";

        // ---------- ENEMY ------------- //
        public string enemy_0 = "//Enemy[@name='Enemy']";

        // ---------- DEATH TEST SPOT ----------- //
        public string deathTestSpot = "//Untagged[@name='Death Test Spot']";

        // --- UNITY FN COMPONENTS -- //
        public string unityFn_Transform = "/fn:component('UnityEngine.Transform')";
        public string unityFn_Text = "/fn:component('UnityEngine.UI.Text')/@text";

        // Used before a test to verify the test should run or if the currentScene is unexpected
        public bool Outside_VerifyCurrentSceneName(string expectedScene, string currentScene)
        {
            return currentScene.ToString().CompareTo(expectedScene.ToString()) == 0;
        }

        public bool Outside_PlantAcorn(ApiClient api)
        {
            bool flag = false;
            Vector3 gardenPosition = api.GetObjectPosition(gdn_0);
            Vector2 clickPosition = new Vector2(gardenPosition.x, gardenPosition.y);

            api.SetObjectFieldValue((pl_MainCharacter + unityFn_Transform).ToString(), "position", gardenPosition);
            api.Wait(1000);

            bool plantActive = api.GetObjectFieldValue<bool>("//*[@name='Plant']/@active");

            if(plantActive && (api.GetObjectFieldValue<int>("//*[@name='Player']/fn:component('PlayerInventory')/@Acorns") > 0))
            {
                Console.WriteLine("should be planting");
                //api.ClickObject(MouseButtons.LEFT, gdn_0, 30);
                api.ButtonPress("Fire1",5,30);
                api.Wait(6000);
            }


            
            flag = api.GetObjectFieldValue<bool>("//*[@name='Garden']/fn:component('Garden')/@isPlanted");
            return flag;
        }


        public bool Outside_TestDying(ApiClient api)
        {
            bool flag = false;

            Vector3 deathTestSpotPosition = api.GetObjectPosition(deathTestSpot);

            api.SetObjectFieldValue((pl_MainCharacter + unityFn_Transform).ToString(), "position", deathTestSpotPosition);
            api.Wait(10000);

            Outside_VerifyCurrentSceneName("GameOver", api.GetSceneName());
            flag = true;
            return flag;
        }

    }
}
