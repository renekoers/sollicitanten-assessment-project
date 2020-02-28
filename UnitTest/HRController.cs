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
        public async Task LoginSuccesTest(){
            
            
        }

        [TestMethod]
        public async Task AddCandidateSuccess() {
            ActionResult<string> response = await hrController.AddCandidate("testadd");
            Assert.AreEqual(((StatusCodeResult) response.Result).StatusCode, 200);
        }

         [TestMethod]
        public async Task AddCandidateEmptyStringAsName() {
            ActionResult<string> response = await hrController.AddCandidate("");
            Assert.AreEqual(((StatusCodeResult) response.Result).StatusCode, 422);
        }

         [TestMethod]
        public async Task AddCandidateNameIsNull() {
            ActionResult<string> response = await hrController.AddCandidate(null);
            Assert.AreEqual(((StatusCodeResult) response.Result).StatusCode, 422);
        }

        
    }
}