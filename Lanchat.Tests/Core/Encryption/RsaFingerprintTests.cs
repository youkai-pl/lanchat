using System.Text;
using Lanchat.Core.Encryption;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Encryption
{
    public class RsaFingerprintTests
    {
        [Test]
        public void Fingerprint()
        {
            const string expectedFingerprint = "09:8f:6b:cd:46:21:d3:73:ca:de:4e:83:26:27:b4:f6";
            var data = Encoding.UTF8.GetBytes("test");
            var fingerprint = RsaFingerprint.GetMd5(data);
            Assert.AreEqual(expectedFingerprint, fingerprint);
        }
    }
}