using Microsoft.VisualStudio.TestTools.UnitTesting;
using ncc2019.Common.BLL;
using ncc2019.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Controllers.Tests
{
    [TestClass()]
    public class APISControllerTests
    {
        [TestMethod()]
        public void GetPhoneNoTest()
        {
            string session_key = "XUIW/nQxeP6LUmWDMbhNoA==";
            string encryptedData = "5ISss+VSEn7Zmch1sDMaiujKC1hv9Y4NGrQIzaGFa5Nh+z9YOe9LzxdMHxFCkqj3SphS5J0E636BQEQmPhwIjvIfLoTM8ZhZZlsKLYxqyteAQ9u9Z7VCl802Yij+Pn0r+J8Tn/LiQpicMpgMkU3wmzf0QqvFb//6rtANPhUi1a+CQv9+hV82pH7ux9lpBPr4EyafFa1K7QKRDhRUDzU11A==";
            string iv = "lviz2xaoUzuohOSZv55kjw==";
            string block = UserInfoHelper.DecodeUserInfo(session_key, "", encryptedData, iv);
            Assert.Fail();
        }
    }
}