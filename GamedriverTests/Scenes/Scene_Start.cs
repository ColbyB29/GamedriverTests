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
    class Scene_Start
    {
        // --- SCENE NAME --- //
        public string sc_Main = "MainMenu";

        // --- BUTTONS --- //
        public string btn_NewGame = "//*[@name='New Game Button']";

        public bool Start_VerifyCurrentSceneName(string expectedScene, string currentScene)
        {
            return currentScene.ToString().CompareTo(expectedScene.ToString()) == 0;
        }
        public bool Start_ClickNewGameButton(ApiClient api)
        {
            bool flag = false;
            string currentScene = api.GetSceneName();
            bool isMainScene = Start_VerifyCurrentSceneName(sc_Main, currentScene);

            if (isMainScene)
            {
                Assert.AreEqual(true, api.ClickObject(MouseButtons.LEFT, btn_NewGame, 30), "Couldn't click start button");
                flag = true;
                api.Wait(1000);
            }
            return flag;
        }
    }
}
