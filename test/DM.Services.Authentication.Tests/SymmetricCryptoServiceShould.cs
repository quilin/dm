using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.Security;
using DM.Tests.Core;
using FluentAssertions;
using Xunit;

namespace DM.Services.Authentication.Tests;

public class SymmetricCryptoServiceShould : UnitTestBase
{
    private readonly TripleDesSymmetricCryptoService service = new();

    [Fact]
    public async Task CryptSymmetrically()
    {
        var input = "some value to encrypt";
        var encrypted = await service.Encrypt(input, CancellationToken.None);
        var decrypted = await service.Decrypt(encrypted, CancellationToken.None);

        decrypted.Should().Be(input);
        encrypted.Should().NotBe(input);
    }
}