using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System;
using System.IO;

namespace AMSDataLoad.Helpers
{
    public class Helper
    {
        string _publicKeyXML = "<RSAKeyValue><Modulus>sbNMll1FKkYI4n5EwgDpwCv/niuSAIgMI6xDJxUPOFGjlarBOhyFBATMvcwXzauCTp6KJx0Lc4n7VHHd43CfmeK67q/udf32KVajK5q7go7jc+AiTnZFrAfZ+GbflSpGESyuQNGiyKaOyQoUTW87NLvXrH98ZJSU6k1bPdk5kbU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        string _privateKeyXML = "<RSAKeyValue><Modulus>sbNMll1FKkYI4n5EwgDpwCv/niuSAIgMI6xDJxUPOFGjlarBOhyFBATMvcwXzauCTp6KJx0Lc4n7VHHd43CfmeK67q/udf32KVajK5q7go7jc+AiTnZFrAfZ+GbflSpGESyuQNGiyKaOyQoUTW87NLvXrH98ZJSU6k1bPdk5kbU=</Modulus><Exponent>AQAB</Exponent><P>xUmegOM4FUQnKSigWL0LjHKhaqfoicgqvKd4bxobJDv5+xjik+s4NZiovm7C90mtZA2bc88/jNLEXR1VVXv9Zw==</P><Q>5pVtbkDeEhOLv6krj+DSOE1NllUSSEeEac4QylDiGQbEgg3NBy7eTz0nWoOVMXJL/9jYJovYsK2XsmjaLqwqgw==</Q><DP>BChB+8tN8jzGanqdrmEFbkc4GYxCPS0HoYQR6J1vNvtAkEb890r8mzyFScYBu75EthgHT5BtcWU7mA63Lp73Pw==</DP><DQ>rc7FBe5vdkC1fmsOIw3caAQdD8xgU4tVDEv/7AC77RFk3oN4oIl7mU8HcvrsYrE9CEVz6NpRJBw11I2kqLmt8Q==</DQ><InverseQ>Iabtruh5KGIfifqEmDmvz4AWR0b+2vh3/FSE9hPVodI+txQ/QkCzh1IFfognHcKZCyQbwtFQjJdWq5lw9P2yaw==</InverseQ><D>BxiK0WJOFb8QMcMrCbgxnVvkbDN/NAg6u93wHIppviz8ZEiaSLDE2wE6D8YrmSnlHTCVgbRWNbnn0FpvqtpfdRuTsj1Sl3shfscp2jFpn5/2xSKIAgwHn958PThKdJqhTFb4CeNsl/hiw4YVfg7fm8YKOuRjbQH5Lyjm7hiVhm0=</D></RSAKeyValue>";

        public string? FormatDate(string date)
        {
            if ((date == null) || (date == "") || (date == "Restricted")) return null;
            return date.Split(".")[0];
        }


        public string GetPublicKey()
        {
            string path = "./Assets/public_key.pem";
            string publicKeyPEM = File.ReadAllText(path);
            return publicKeyPEM;
        }

        private string  GetPrivateKey()
        {
            string path = "./Assets/private_key.pem";
            string privateKeyPEM = File.ReadAllText(path);
            return privateKeyPEM;
        }

        public string EncryptString(string clearText)
        {
            string publicKeyPEM =  GetPublicKey();
            RSAParameters rsaParams = ConvertPemToRSAParameters(publicKeyPEM);

            using (RSA publicRsa = RSA.Create())
            {
                publicRsa.ImportParameters(rsaParams);
                string publicKeyXML = publicRsa.ToXmlString(false);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(publicKeyXML);

                byte[] dataBytes = Encoding.Unicode.GetBytes(clearText);
                byte[] encrypted = rsa.Encrypt(dataBytes, false).ToArray();

                return JsonSerializer.Deserialize<string>(JsonSerializer.Serialize(encrypted)) + "";//.Replace("\"", "");
            }
        }

        public string DecryptString(string keyPassword, string encrypted)
        {
            string privateKeyPEM =  GetPrivateKey();
            RSAParameters rsaParams = ConvertPemToRSAParameters(privateKeyPEM, keyPassword);
            // var privateKey = File.ReadAllText("Assets/pwPayloadPrivate.pem");
            using (RSA privateRsa = RSA.Create())
            {
                // privateRsa.ImportFromPem(privateKeyPEM.ToCharArray());
                privateRsa.ImportParameters(rsaParams);
                string privateKeyXML = privateRsa.ToXmlString(true);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(privateKeyXML);

                byte[] encryptedBytes = Convert.FromBase64String(encrypted);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                string decryptedText = Encoding.Unicode.GetString(decryptedBytes);

                return decryptedText;
            }
        }


        private static RSAParameters ConvertPemToRSAParameters(string pem, string keyPassword = "")
        {
            // Console.WriteLine("" + pem);
            PemReader pemReader;
            if (keyPassword == "")
            {
                pemReader = new PemReader(new StringReader(pem));
                return ConvertPemReaderToPublicRSAParameters(pemReader);
            }
            else
            {
                // Console.WriteLine("" + keyPassword);
                // pemReader = new PemReader(new StringReader(pem));
                // pemReader = new PemReader(new StringReader(pem), new PasswordFinder(keyPassword));
                return ConvertPemToPrivateRSAParameters(pem, keyPassword);
            }
        }

        private static RSAParameters ConvertPemReaderToPublicRSAParameters(PemReader pemReader)
        {
            AsymmetricKeyParameter keyParameter = (AsymmetricKeyParameter)pemReader.ReadObject();
            RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)keyParameter;

            RSAParameters rsaParameters = new RSAParameters
            {
                Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
            };

            return rsaParameters;
        }

        public static RSAParameters ConvertPemToPrivateRSAParameters(string pemText, string password = "")
        {
            using var reader = new StringReader(pemText);

            object pemObject;
            if (password == "")
            {
                var pemReader = new PemReader(reader);
                pemObject = pemReader.ReadObject();
            }
            else
            {
                var passwordFinder = new PasswordFinder(password);
                var pemReader = new PemReader(reader, passwordFinder);
                pemObject = pemReader.ReadObject();
            }

            RsaPrivateCrtKeyParameters rsaParams = pemObject switch
            {
                AsymmetricCipherKeyPair keyPair => (RsaPrivateCrtKeyParameters)keyPair.Private,
                RsaPrivateCrtKeyParameters rsa => rsa,
                _ => throw new InvalidOperationException("Unsupported PEM format")
            };

            return ConvertPrivateCrtKeyToRSAParameters(rsaParams);
        }

        private static RSAParameters ConvertPrivateCrtKeyToRSAParameters(RsaPrivateCrtKeyParameters privKey)
        {
            return new RSAParameters
            {
                Modulus = privKey.Modulus.ToByteArrayUnsigned(),
                Exponent = privKey.PublicExponent.ToByteArrayUnsigned(),
                D = privKey.Exponent.ToByteArrayUnsigned(),
                P = privKey.P.ToByteArrayUnsigned(),
                Q = privKey.Q.ToByteArrayUnsigned(),
                DP = privKey.DP.ToByteArrayUnsigned(),
                DQ = privKey.DQ.ToByteArrayUnsigned(),
                InverseQ = privKey.QInv.ToByteArrayUnsigned()
            };
        }

    }
    class PasswordFinder : IPasswordFinder
    {
        private string password;

        public PasswordFinder(string password)
        {
            this.password = password;
        }


        public char[] GetPassword()
        {
            return password.ToCharArray();
        }
    }
}