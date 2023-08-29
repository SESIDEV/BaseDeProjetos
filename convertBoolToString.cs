using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtensãoTipoBoolParaString.Tests
{
    [TestClass]
    public class BoolExtensionsTests
    {
        [TestMethod]
        public void ToSimNaoString_NullValue_ReturnsSemValor()
        {
            // Arrange
            bool? value = null;

            // Act
            string result = value.ToSimNaoString();

            // Assert
            Assert.AreEqual("Sem valor", result);
        }

        [TestMethod]
        public void ToSimNaoString_TrueValue_ReturnsSim()
        {
            // Arrange
            bool? value = true;

            // Act
            string result = value.ToSimNaoString();

            // Assert
            Assert.AreEqual("Sim", result);
        }

        [TestMethod]
        public void ToSimNaoString_FalseValue_ReturnsNao()
        {
            // Arrange
            bool? value = false;

            // Act
            string result = value.ToSimNaoString();

            // Assert
            Assert.AreEqual("Não", result);
        }
    }
}
