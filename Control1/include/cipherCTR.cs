using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Control1.include
{
    public class CipherCTR
    {
        private bool firstUse = true;
        private byte[] _key = new byte[32];
        private byte[] _counterBlock = new byte[16];
        private byte[] _cipher = new byte[16];
        private ICryptoTransform counterEncryptor;
        private int blockSize = 0;

        public CipherCTR(byte[] key, byte[] nonce)
        {
            if (_counterBlock.Length != nonce.Length || key.Length != _key.Length)
            {
                throw new ArgumentException(
                    "Salt size must be same as block size ");
            }
            this._key = key;
            this._counterBlock = nonce;
        }

        public byte[] Encrypt(byte[] data)
        {

            int i = 0;

            if (firstUse)
            {
                firstUse = false;
                counterEncryptor = Aes();
                counterEncryptor.TransformBlock(
                        _counterBlock, 0, _counterBlock.Length, _cipher, 0);
            }


            byte[] outPut = new byte[data.Length];


            while (i < data.Length)
            {
                if (blockSize == 16)
                {
                    IncrementCounterByOne();
                    blockSize = 0;
                    counterEncryptor.TransformBlock(
                    _counterBlock, 0, _counterBlock.Length, _cipher, 0);
                }

                while (blockSize < 16 && i < data.Length)
                {

                    outPut[i] = ((byte)(data[i] ^ _cipher[blockSize]));
                    blockSize++;
                    i++;
                }

            }

            return outPut;
        }

        private ICryptoTransform Aes()
        {

            using (SymmetricAlgorithm aes = new AesManaged())
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                var zeroIv = new byte[16];
                ICryptoTransform counterEncryptor = aes.CreateEncryptor(_key, zeroIv);

                return counterEncryptor;
            }
        }

        private void IncrementCounterByOne()
        {
            for (var i = _counterBlock.Length - 1; i >= 0; i--)
            {
                if (++_counterBlock[i] != 0)
                {
                    break;
                }
            }
        }
    }
}
