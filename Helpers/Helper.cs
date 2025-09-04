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
            // string path = "./Assets/public_key.pem";
            // string publicKeyPEM = File.ReadAllText(path);
            string publicKeyPEM = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApruoXN58N3jhkp3NWF8W
Qi6GKJy3tAFpNgMjqFMnAOjqEJ3gfLJMBc9gq8RuLrb3sWb0u10mZet/U25Nx59+
CTMXlW92c8AcK/fDIqMe0za1lOPFYbgrU8/xn2+XE3SVnYXuufU16MBuDHAORX/Z
VlbcOWY+ttlYOVLBsT7nF8SqiFjI6l6cnambd1PoeFJiNlOG+88EFYKqUneH9Qmy
RHRuxAOeEHIcD3Jiio0T9ZnHnqnO8SqeoG5vsmf0AdFV9yFzopkHa6EOzqUaSqdt
ZOBvrP8JrWNr0ZT1ZogGhKm1OY3nyRYxhIEmV9ZkdegEOAGB+yAWJoPx2IlmsuTZ
hQIDAQAB
-----END PUBLIC KEY-----
";
            return publicKeyPEM;
        }

        private string  GetPrivateKey()
        {
            // string path = "./Assets/private_key.pem";
            // string privateKeyPEM = File.ReadAllText(path);
            string privateKeyPEM = @"-----BEGIN ENCRYPTED PRIVATE KEY-----
MIIFLTBXBgkqhkiG9w0BBQ0wSjApBgkqhkiG9w0BBQwwHAQI7wc6mXq541ICAggA
MAwGCCqGSIb3DQIJBQAwHQYJYIZIAWUDBAEqBBCIrSGv57uMLK6BP0yEKYt0BIIE
0AOKmH55AMcLx+ScJIbGv0zCmGHgm8KDbqmRF7UiPVRZjHbS626nqXGrp01EgMPt
peRAHq/J4KPatI5GrUOt1/UtjzegkwaXxuDgBLq5rGoyKaVTeBxsnUcClLtq7cx8
rWR7HdHDsw0/ywE1endmxR4g51SFc2zZdkZF6TBoMyhrQO0xvODbdydrnOGhIKW+
2PiCzrcT9tdCt1f0F1cirpF93gwRy6O+IhVhaAe14iVtrnhE6txTRYBcmOLI0DMf
tq6+NjxOktQyGsynNe2/hPvUQrtBsRsytWqQE+eBK/X46ObOSJOCmDngZy6k9T84
b1iduljhU4cF7WGGY1xWxPjJIPTAKXF4QTVF7xCFLIBavWTGFmO19XmZ1KTAvpLN
LLHdCw/QRlYKRHtcM1pPrf7C6gM0PzSXicWFkMMP+SxT96z/oENtbkdh2e9WAnj1
BktqxFWaCNJ5H+nDasHV1Ekp6mOXzRw1CIJYlsIsiBuedXQDWbY/e/x1Yyi9kYMc
It0ox3ebUX7h9AO6SXKL6Le2+pIdG8ScQlRZ4/ll0zrCuG8DmTi7SUBF3sk8DFkw
eF9cGIfzt1PoUQJu8qTXp8vFoTh2HMWk3QTLYRt/f5LT3HEnfTBzJMsiTUrPXOx5
MO7e2JWP7E/1o8bIgrFwgUoQSjOxArQXW7AiHtm78k0SirhKxuZ9/SN1uOLxtGOr
OGF4PmtRdVODoXIsFU+Pzv1Ggs0C/t9jQvSEKHVCbFqiMUgj0d6lwHGCkDxpox+r
1Q2Ot/zB9Doco964E6v/h7J9ziD8xP3D9URoQZ4IbRDMfapWqir+4pgjsXuXcOpx
VWEpAws3RkUH0DLEdzsBkPyAQSeKZZWWhWghkVdOx11okQMz3mTgBtkMj3IJWHlB
d0c1FZB7aZvTbTynZhhOesn8xJJCGRarE5KgkLT5AcBEHqKKKrz47NHeS0P5M4Ot
i3Pu0Lqhz9mO0SXc2IvpGedia/tncmPLrR2LuIpksGxmkJn3l5VXYOONwRn+4Zg1
6lBbXXld2dyOfr/ShEAvV5dmyKNT9ypS8YHvySOpnFZWuzvgGLoovq98g9iL6Rbi
p6hdPUkDvxnLb5byJyB8PziQN6A3jwIsRC1OeFIxhFolOxzL1mmUcUtje37o0t2M
5XWhmExWyiw+BUJcSk5XLJlma5aM8fNs15Gvhtp53vfpJepHC1+gSZ/p/soQGpg9
GLSog1tCSq5+OHXK1jEArCreN1BoJYXujMTycaNy6aYWNEGxwEFmywHOXCvVPUat
7nCZcyhc1Jkn/7eQP4MVtvKpZUmUucNG5A1xQxNYjZY77H8Inc+FpufXTrUpB15r
TGcxHIWijQtVhT/0jHREF0qt/pRtuSJ4N3sRimbPUKRoFLa6bP1/N+EtPGu32rzT
L0yQMp4a5GyqekdVNB6tKzV7p1u0QPbQ+XGqWTOOp19chDK50A/kqIRzg36zjqdU
86+j8AsDj2zJcNuXrwxls0OuhcchY1+BncoE/S2Pi8H0rO8CP+hPJvtaZeE4yKKJ
hSsafBwO+S3gS7Ar5NvLiRNTZW6LslSDz3g4t/g3/y0l0bjCv/P5E+TJS7YYjFOM
kJpjEDzCTftfw9RKRNHlzpk2okUSRFCq9bno6rr1bla4
-----END ENCRYPTED PRIVATE KEY-----
";
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