using NFe.Components.Exceptions;
using NFe.Settings;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace NFe.Components.Security
{
    internal static class SOAPSecurity


    {
        #region Public Methods

        public static BasicHttpBinding GetBinding()
        {
#if _fw46
            return new BasicHttpBinding
            {
                Name = "BasicHttpBinding_Username",
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.TransportWithMessageCredential,
                    Message = new BasicHttpMessageSecurity
                    {
                        ClientCredentialType = BasicHttpMessageCredentialType.UserName,
                        AlgorithmSuite = SecurityAlgorithmSuite.Default
                    }
                }
            };
#else
            throw new MunicipioSemSuporteAoNETFramework35Exception();
#endif
        }

        public static void SignUsingCredentials(int emp, object objectToSign)
        {
#if _fw46

            if (!(((dynamic)objectToSign).ClientCredentials is ClientCredentials clientCredentials))
                return;

            clientCredentials.UserName.UserName = Empresas.Configuracoes[emp].UsuarioWS;
            clientCredentials.UserName.Password = Empresas.Configuracoes[emp].SenhaWS;
#else
            throw new MunicipioSemSuporteAoNETFramework35Exception();
#endif
        }

#endregion Public Methods
    }
}
