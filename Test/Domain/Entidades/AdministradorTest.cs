using minimal_api.Dominio.Entidades;

namespace Test.Domain.Entidades;

[TestClass]
public class AdministradorTest
{
    [TestMethod]
    public void TestarGetSetPropriedades()
    {
        // Arrange
        var adm = new Administrador();

        // Act
        adm.Id = 1;
        adm.Email = "teste@teste.com";
        adm.Senha = "teste";
        adm.Perfil = "Adm";

        // Assert
        Assert.AreEqual(1, adm.Id);
        Assert.AreEqual("teste@teste.com", adm.Email);
        Assert.AreEqual("teste", adm.Senha);
        Assert.AreEqual("Adm", adm.Perfil);
    }

    [TestMethod]
    public void TestarCriacaoAdministrador()
    {
        // Arrange & Act
        var adm = new Administrador
        {
            Email = "novo@teste.com",
            Senha = "senha123",
            Perfil = "Editor"
        };

        // Assert
        Assert.IsNotNull(adm.Email);
        Assert.IsNotNull(adm.Senha);
        Assert.IsNotNull(adm.Perfil);
        Assert.AreEqual("Editor", adm.Perfil);
    }
}
