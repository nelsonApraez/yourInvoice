///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.Business.TransformModule;

namespace yourInvoice.Common.UnitTest
{
    public class TransformModuleUnitTest
    {
        [Fact]
        public void ReplaceTokens_ShouldReplaceTokensInString()
        {
            // Arrange
            string body = "Hola {{nombre}}, tu correo es {{correo}}.";
            var replacements = new Dictionary<string, string>
        {
            { "{{nombre}}", "Juan" },
            { "{{correo}}", "juan@example.com" }
        };

            string expected = "Hola Juan, tu correo es juan@example.com.";

            // Act
            string result = TransformModule.ReplaceTokens(body, replacements); // Reemplaza "YourClassName" con el nombre de la clase que contiene el m�todo.

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReplaceTokens_ShouldHandleMissingTokens()
        {
            // Arrange
            string body = "Hola {{nombre}}, tu correo es {{correo}}.";
            var replacements = new Dictionary<string, string>
        {
            { "{{nombre}}", "Juan" }
        };

            string expected = "Hola Juan, tu correo es {{correo}}.";

            // Act
            string result = TransformModule.ReplaceTokens(body, replacements); // Reemplaza "YourClassName" con el nombre de la clase que contiene el m�todo.

            // Assert
            Assert.Equal(expected, result);
        }
    }
}