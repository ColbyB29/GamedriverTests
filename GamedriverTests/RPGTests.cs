using System;
using System.Collections.Generic;
using NUnit.Framework;
using gdio.unity_api.v2;
using gdio.common.objects;
using System.Collections;

using gdio.unity_api;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamedriverTests.Scenes;

namespace GamedriverTests
{
    [TestFixture]
    public class RPGTests
    {
        //These parameters can be used to override settings used to test when running from the NUnit command line
        public string testMode = TestContext.Parameters.Get("Mode", "IDE");
        public string pathToExe = TestContext.Parameters.Get("pathToExe", null); // replace null with the path to your executable as needed

        ApiClient api;

        Scene_Start sc_Start;
        Scene_Home sc_Home;
        Scene_Outside sc_Outside;


        [OneTimeSetUp]
        public void Connect()
        {
            try
            {
                api = new ApiClient();
                // If an executable path was supplied, we will launch the standalone game
                if (pathToExe != null)
                {
                    ApiClient.Launch(pathToExe);
                    api.Connect("localhost", 19734, false, 30);
                }

                // If no executable path was given, we will attempt to connect to the Unity editor and initiate Play mode
                else if (testMode == "IDE")
                {
                    api.Connect("localhost", 19734, true, 30);
                }
                // Otherwise, attempt to connect to an already playing game
                else api.Connect("localhost", 19734, false, 30);

            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("Connect-FAIL.jpg", true, true);
                throw new Exception("Couldn't connect: " + ex.ToString());
            }

            
            // Enable input hooking
            api.EnableHooks(HookingObject.KEYBOARD);
            api.EnableHooks(HookingObject.MOUSE);
       
        }

        [Test, Order(1)]
        public void VerifySceneNameStart()
        {
            try
            {
                sc_Start = new Scene_Start();
                Assert.AreEqual(sc_Start.sn_Name, api.GetSceneName(),"Current scene name is incorrect");
                api.CaptureScreenshot("VerifySceneNameStart.jpg", true, true);
            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("VerifySceneNameStart-FAIL.jpg", true, true);
                throw new Exception("VerifySceneNameStart test fail: " + ex.ToString());

            }

        }

        [Test, Order(2)]
        public void StartGame()
        {
            try
            {
                sc_Start = new Scene_Start();
                api.WaitForObject(sc_Start.btn_NewGame);

                //Triggered if ClickObject fails at clicking btn_NewGame
                Assert.IsTrue(sc_Start.Start_ClickNewGameButton(api), "Couldn't click on button");

                //Write to console if Assert passes
                Console.WriteLine("Clicked on New Game Button!");
                api.CaptureScreenshot("StartGame.jpg", true, true);
            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("StartGame-FAIL.jpg", true, true);
                throw new Exception("StartGame test fail: " + ex.ToString());

            }

        }

        [Test, Order(3)]
        public void VerifySceneNameHome()
        {
            try
            {
                sc_Home = new Scene_Home();
                Assert.AreEqual(sc_Home.sn_Name, api.GetSceneName(), "Current scene name is incorrect");
                api.CaptureScreenshot("VerifySceneNameHome.jpg", true, true);
            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("VerifySceneNameHome-FAIL.jpg", true, true);
                throw new Exception("VerifySceneNameHome test fail: " + ex.ToString());

            }

        }

        [Test, Order(4)]
        public void NPCInteractionPositive()
        {
            try
            {
                sc_Home = new Scene_Home();

                Assert.True(sc_Home.Home_LoadScene(api), "Couldn't load Home");

                api.WaitForObject(sc_Home.pl_MainCharacter);
                Assert.IsTrue(sc_Home.Home_NPCInteractionPossitive(api),"Talk is not present, TEST FAILED");
                Console.WriteLine("Found the talk option");
                api.CaptureScreenshot("NPCInteraction.jpg", true, true);
            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("NPCInteractionPossitive-FAIL.jpg", true, true);
                throw new Exception("NPCInteractionPossitive test fail: " + ex.ToString());
            }
        }

        [Test, Order(5)]
        public void TestDialogue()
        {
            try
            {
                sc_Home = new Scene_Home();

                Assert.True(sc_Home.Home_LoadScene(api), "Couldn't load Home");

                api.WaitForObject(sc_Home.pl_MainCharacter);

                Assert.IsTrue(sc_Home.Home_TestDialogue(api), "Could not click continue twice during the dialogue test");

                api.CaptureScreenshot("TestDialogue.jpg", true, true);
            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("TestDialogue-FAIL.jpg", true, true);
                throw new Exception("TestDialogue test fail: " + ex.ToString());
            }
        }

        [Test, Order(6)]
        public void NPCInteractionNegative()
        {
            try
            {
                sc_Home = new Scene_Home();

                Assert.True(sc_Home.Home_LoadScene(api), "Couldn't load Home");

                api.WaitForObject(sc_Home.pl_MainCharacter);
                Assert.IsTrue(sc_Home.Home_NPCInteractionNegative(api), "Talk is present, TEST FAILED");
                Console.WriteLine("Did not find the talk option");
                api.CaptureScreenshot("NPCInteraction.jpg", true, true);
            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("NPCInteractionNegative-FAIL.jpg", true, true);
                throw new Exception("NPCInteractionNegative test fail: " + ex.ToString());
            }
        }

       

        [Test, Order(7)]
        public void MoveOutside()
        {
            try
            {
                sc_Home = new Scene_Home();

                Assert.True(sc_Home.Home_LoadScene(api), "Couldn't load Home");

                api.WaitForObject(sc_Home.pl_MainCharacter);
                Assert.IsTrue(sc_Home.Home_MoveToDoor(api),"Cannot move player to the door");
                Console.WriteLine("Moving player to door");
                api.CaptureScreenshot("MoveOutside.jpg", true, true);
            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("MoveOutside-FAIL.jpg", true, true);
                throw new Exception("MoveOutside test fail: " + ex.ToString());
            }
        }

        [Test, Order(8)]
        public void VerifySceneNameOutside()
        {
            try
            {
                sc_Outside = new Scene_Outside();
                Assert.IsTrue(sc_Outside.Outside_VerifyCurrentSceneName(sc_Outside.sn_Name, api.GetSceneName()), "Current scene name is incorrect");
                api.CaptureScreenshot("VerifySceneNameOutside.jpg", true, true);
            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("VerifySceneNameOutside-FAIL.jpg", true, true);
                throw new Exception("VerifySceneNameOutside test fail: " + ex.ToString());
            }
        }

        [Test, Order(9)]
        public void PlantAcorn()
        {
            try
            {
                sc_Outside = new Scene_Outside();
                Assert.IsTrue(sc_Outside.Outside_VerifyCurrentSceneName(sc_Outside.sn_Name, api.GetSceneName()), "Current scene name is incorrect");
                Assert.IsTrue(sc_Outside.Outside_PlantAcorn(api), "Cannot plant acorn");
                Console.WriteLine("planting acorn");
                api.CaptureScreenshot("PlantAcorn.jpg", true, true);
            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("PlantAcorn-FAIL.jpg", true, true);
                throw new Exception("PlantAcorn test fail: " + ex.ToString());
            }
        }

        [Test, Order(10)]
        public void TestDying()
        {
            try
            {
                sc_Outside = new Scene_Outside();
                Assert.IsTrue(sc_Outside.Outside_VerifyCurrentSceneName(sc_Outside.sn_Name, api.GetSceneName()), "Current scene name is incorrect");
                Assert.IsTrue(sc_Outside.Outside_TestDying(api), "Play did not die as expected");
                Console.WriteLine("player is dead");
                api.CaptureScreenshot("TestDying.jpg", true, true);
            }
            catch (Exception ex)
            {
                api.CaptureScreenshot("TestDying-FAIL.jpg", true, true);
                throw new Exception("TestDying test fail: " + ex.ToString());
            }
        }

        [OneTimeTearDown]
        public void Disconnect()
        {
            // Disconnect the GameDriver client from the agent
          //  api.DisableHooks(HookingObject.ALL);
            api.Wait(2000);
            api.Disconnect();
            api.Wait(2000);
        }
    }
}
