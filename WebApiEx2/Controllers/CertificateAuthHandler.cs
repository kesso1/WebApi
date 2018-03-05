using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApiEx2.Controllers
{
    public interface IValidateCertificates
    {
        bool IsValid(X509Certificate2 certificate);
        IPrincipal GetPrincipal(X509Certificate2 certificate2);
    }

    public class BasicCertificateValidator : IValidateCertificates
    {
        public bool IsValid(X509Certificate2 certificate)
        {
            return certificate.Issuer.Equals("CN=IIS Lab CARoot") && certificate.Subject.Equals("CN=AnyClientInIISLab");
        }

        public IPrincipal GetPrincipal(X509Certificate2 certificate2)
        {
            return new GenericPrincipal(
                new GenericIdentity(certificate2.Subject), new[] { "User" });
        }
    }

    public class CertificateAuthHandler : DelegatingHandler
    {
        public IValidateCertificates CertificateValidator { get; set; }

        public CertificateAuthHandler()
        {
            CertificateValidator = new BasicCertificateValidator();
        }

        protected override Task<HttpResponseMessage>
            SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            X509Certificate2 certificate = request.GetClientCertificate();
            if (certificate == null || !CertificateValidator.IsValid(certificate))
            {
                return Task<HttpResponseMessage>.Factory.StartNew(
                    () => request.CreateResponse(HttpStatusCode.Unauthorized));

            }
            Thread.CurrentPrincipal = CertificateValidator.GetPrincipal(certificate);
            return base.SendAsync(request, cancellationToken);
        }
    }
}