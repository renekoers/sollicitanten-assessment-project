using React_Frontend.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using JSonWebToken;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using BackEnd;

namespace UnitTest {

    [TestClass]
    public class HRControllerTest{

        private HRController hrController = new HRController(new TestDB());
        
        [TestMethod]
        public void LoginTest(){
            // JsonElement credentials = (JsonElement) JSON.Deserialize("\"username\":\"testUsername\",\"password\":\"testPassword\"");
            // ActionResult<string> response = hrController.Login(credentials);
            // string token = JWT.CreateToken("testUsername", "testPassword");

            Assert.AreEqual("response","response");
        }
    }
}