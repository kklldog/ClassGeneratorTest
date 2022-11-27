using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassGeneratorTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGeneratorTest.Tests
{
    [TestClass()]
    public class EmitGeneratorTests
    {
        [TestMethod()]
        public void GenerateTest()
        {
            var generator = new EmitGenerator();
            var type = generator.GenerateType();
            var instance = Activator.CreateInstance(type);
            Assert.IsNotNull(type);
            Assert.IsNotNull(instance);

            dynamic dy = instance;

            dy.Id = "123";
            Assert.AreEqual("123", dy.Id);

            dy.Name = "name";
            Assert.AreEqual("name", dy.Name);
        }

        [TestMethod()]
        public void EfTestTest()
        {
            var generator = new EmitGenerator();
            var dbset = generator.EfTest();
        }
    }
}