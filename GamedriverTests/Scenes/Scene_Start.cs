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
    //SCENE NAME: MainMenu.unity
    
    class Scene_Start
    {
        // --- SCENE NAME --- //
        public string sn_Name = "MainMenu";

        // --- BUTTONS --- //
        public string btn_NewGame = "//*[@name='New Game Button']";

        // Used before a test to verify the test should run or if the currentScene is unexpected
        public bool Start_VerifyCurrentSceneName(string expectedScene, string currentScene)
        {
            return currentScene.ToString().CompareTo(expectedScene.ToString()) == 0;
        }

        // TEST: If ClickObject returns true with btn_NewGame 
        public bool Start_ClickNewGameButton(ApiClient api)
        {
            bool flag = false;
            string currentScene = api.GetSceneName();
            bool isMainScene = Start_VerifyCurrentSceneName(sn_Name, currentScene);

            if (isMainScene)
            {
                flag = api.ClickObject(MouseButtons.LEFT, btn_NewGame, 30);
                api.Wait(1000);
            }
            return flag;
        }

    }
}
