using Calculator.Extractors.Satisfactory;
using NUnit.Framework;
using System.Collections.Generic;

namespace TestCalculator
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            var result1 = FGArrayParser.Parse("((ItemClass=BlueprintGeneratedClass'\"/Game/FactoryGame/Resource/Parts/Cement/Desc_Cement.Desc_Cement_C\"',Amount=2),(ItemClass=BlueprintGeneratedClass'\"/Game/FactoryGame/Resource/Parts/Cement/Desc_Cement.Desc_Cement_C\"',Amount=99))");
            var result2 = FGArrayParser.Parse("(B=1234,G=1234,R=1234,A=1234)");
            var result3 = FGArrayParser.Parse("(Rotation=(X=0.000000,Y=0.000000,Z=0.000000,W=1.000000),Translation=(X=0.000000,Y=0.000000,Z=0.000000),Scale3D=(X=1.000000,Y=1.000000,Z=1.000000))");
        }

        [Test]
        public void Test1()
        {
            var result = FGArrayParser.Parse("((ItemClass=BlueprintGeneratedClass'\"/Game/FactoryGame/Resource/Parts/Cement/Desc_Cement.Desc_Cement_C\"',Amount=2),(ItemClass=BlueprintGeneratedClass'\"/Game/FactoryGame/Resource/Parts/Cement/Desc_Cement.Desc_Cement_C\"',Amount=99))");
            Assert.IsFalse(result == null, "Result must be return something");
            Assert.IsTrue(result is List<object>, "Result must be a List");
            Assert.IsTrue((result as List<object>)[0] is Dictionary<string, object>, "Result[0] must be a Dictionary");
            Assert.IsTrue(((result as List<object>)[0] as Dictionary<string, object>).Count == 2, "Result[0].Count must be 2");
            Assert.IsTrue(((dynamic)result)[1] is Dictionary<string, object>, "Result[0] must be a Dictionary");
            Assert.IsTrue(((result as List<object>)[1] as Dictionary<string, object>).Count == 2, "Result[1].Count must be 2");
            Assert.Pass();
        }
        [Test]
        public void Test2()
        {
            var result = FGArrayParser.Parse("(B=1234,G=1234,R=1234,A=1234)");
            Assert.IsFalse(result == null, "Result must be return something");
            Assert.IsTrue(result is Dictionary<string,object>, "Result must be a Dictionary");
            Assert.IsTrue((result as Dictionary<string, object>).Count == 4, "Result.Count must be 4");
            Assert.Pass();
        }
    }
}