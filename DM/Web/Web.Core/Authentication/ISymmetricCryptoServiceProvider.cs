using System;
using System.Threading;

namespace Web.Core.Authentication
{
    public interface ISymmetricCryptoServiceProvider
    {
        ISymmetricCryptoService GetTripleDes();
    }

    public class SymmetricCryptoServiceProvider : ISymmetricCryptoServiceProvider
    {
        private readonly Lazy<TripleDesSymmetricCryptoService> tripleDesSymmetricCryptoService;

        public SymmetricCryptoServiceProvider()
        {
            tripleDesSymmetricCryptoService = new Lazy<TripleDesSymmetricCryptoService>(
                () => new TripleDesSymmetricCryptoService(), LazyThreadSafetyMode.PublicationOnly);
        }

        public ISymmetricCryptoService GetTripleDes()
        {
            return tripleDesSymmetricCryptoService.Value;
        }
    }
}