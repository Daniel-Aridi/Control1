using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Control1.curve25519;


namespace Control1.include
{
    internal enum sessionState
    {
        REQUEST1,
        RESPONSE1_REQUEST2,
        RESPONSE2,
        FINISHED,
    }
    public class Security
    {
        Curve25519 curve25519 = new Curve25519();
        private readonly bleTransport _transport;
        private CipherCTR _cipher;
        private string _pop;
        private sessionState _state;
        private bool _verified = false;
        private byte[] _publicKey = new byte[32];
        private byte[] _privateKey = new byte[32];
        private byte[] _devicePublicKey = new byte[32];
        private byte[] _deviceRandom = new byte[16];
        private byte[] _sharedKey = new byte[32];
        private byte[] clientVerify = new byte[32];

        public Security(bleTransport transport, string pop)
        {
            _transport = transport;
            _pop = pop;
            _state = sessionState.REQUEST1;
        }

        public async Task<bool> establish_session()
        {
            byte[] request = null;
            byte[] response = null;

            while (true)
            {
                request = security_session(response);

                if (request == null) break;
                try
                {
                    response = await _transport.send_bytes("prov-session", request);
                }
                catch (Exception)
                {
                    return false;
                }
                if (response == null) break;
            }
            return _verified;
        }

        private byte[] security_session(byte[] responseData)
        {

            if(_state == sessionState.REQUEST1)
            {
                _state = sessionState.RESPONSE1_REQUEST2;
                return setup0_request();
            }

            if (_state == sessionState.RESPONSE1_REQUEST2)
            {
                _state = sessionState.RESPONSE2;

                setup0_response(responseData);
                return setup1_request();
            }

            if (_state == sessionState.RESPONSE2)
            {
                _state = sessionState.FINISHED;

                setup1_response(responseData);
                return null;
            }
            return null;
        }

        public void generate_key_pairs()
        {
            try
            {
                curve25519.GenerateKeypair(_publicKey, _privateKey);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private byte[] setup0_request()
        {
            SessionData sessionData = new SessionData();

            sessionData.Sec1 = new Sec1Payload();
            sessionData.Sec1.Sc0 = new SessionCmd0();

            sessionData.SecVer = SecSchemeVersion.SecScheme1;

            generate_key_pairs();

            sessionData.Sec1.Sc0.ClientPubkey = ByteString.CopyFrom(_publicKey);

            // Serialize proto message to a byte array
            return sessionData.ToByteArray();

            //// Convert the byte array to a string using a specific encoding (ISO-8859-1)
            //string encodedString = Encoding.GetEncoding("ISO-8859-1").GetString(messageBytes);

            //// To decode the string back to a byte array (using ISO-8859-1)
            //return Encoding.GetEncoding("ISO-8859-1").GetBytes(encodedString);
        }

        private void setup0_response(byte[] responseData)
        {
            SessionData sessionData = new SessionData();

            sessionData = SessionData.Parser.ParseFrom(responseData);

            _devicePublicKey = sessionData.Sec1.Sr0.DevicePubkey.ToByteArray();

            _deviceRandom = sessionData.Sec1.Sr0.DeviceRandom.ToByteArray();

            curve25519.GenerateSharedSecret(_sharedKey, _devicePublicKey, _privateKey);

            // Convert the string to bytes using ISO-8859-1 encoding
            byte[] popBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(_pop);

            byte[] digest;
            using (SHA256 sha256 = SHA256.Create())
            {
                digest = sha256.ComputeHash(popBytes);
            }

            _sharedKey = A_XOR_B(_sharedKey, digest);

            _cipher = new CipherCTR(_sharedKey, _deviceRandom); //initialize an aes_ctr encryption instance using sharedKey and deviceRandom.

            clientVerify = EncryptData(_devicePublicKey);
        }

        private byte[] setup1_request()
        {
            SessionData sessionData = new SessionData();
            sessionData.Sec1 = new Sec1Payload();
            sessionData.Sec1.Sc0 = new SessionCmd0();
            sessionData.Sec1.Sc1 = new SessionCmd1();

            sessionData.SecVer = SecSchemeVersion.SecScheme1;

            sessionData.Sec1.Msg = Sec1MsgType.SessionCommand1;

            sessionData.Sec1.Sc1.ClientVerifyData = ByteString.CopyFrom(clientVerify);

            return sessionData.ToByteArray();
        }

        private bool setup1_response(byte[] responseData)
        {
            SessionData sessionData = new SessionData();

            sessionData = SessionData.Parser.ParseFrom(responseData);

            var deviceVerify = sessionData.Sec1.Sr1.DeviceVerifyData.ToByteArray();

            deviceVerify = DecryptData(deviceVerify);

            if (deviceVerify.ToString() == _publicKey.ToString())
            {
                _verified = true;
                return _verified;
            }
            else
            {
                _verified = false;
                return _verified;
            }
        }

        private static byte[] A_XOR_B(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Input arrays must have the same length");
            }

            byte[] result = new byte[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                result[i] = (byte)(a[i] ^ b[i]);
            }

            return result;
        }

        public byte[] EncryptData(byte[] data)
        {
            return _cipher.Encrypt(data);
        }

        public byte[] DecryptData(byte[] data)
        {
            return _cipher.Encrypt(data);
        }
    }
}
