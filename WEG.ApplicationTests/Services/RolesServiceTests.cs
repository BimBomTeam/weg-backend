using Microsoft.VisualStudio.TestTools.UnitTesting;
using WEG.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEG.Application.Services.Tests
{
    [TestClass()]
    public class RolesServiceTests
    {
        [TestMethod()]
        public void GetRandomRolesFromPoolTest()
        {
            RolesService service = new RolesService();
            var result = service.GetRandomRolesFromPool();

            Assert.IsNotNull(result);
        }
    }
}