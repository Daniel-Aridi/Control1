using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Control1.curve25519
{
    public class Curve25519
    {
        private readonly long[] _121665 = new long[16];
        private static readonly byte[] _9 = { 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public void GenerateRandomBytes(byte[] output)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(output);
        }

        private static void Unpack25519(long[] output, byte[] input)
        {
            for (int i = 0; i < 16; ++i)
            {
                output[i] = input[2 * i] + ((long)input[2 * i + 1] << 8);
            }
            output[15] &= 0x7fff;
        }

        private static void Carry25519(long[] input)
        {
            long carry;

            for (int i = 0; i < 16; ++i)
            {
                carry = input[i] >> 16;
                input[i] -= carry << 16;
                if (i < 15)
                {
                    input[i + 1] += carry;
                }
                else
                {
                    input[0] += 38 * carry;
                }
            }
        }

        private static void Swap25519(long[] p, long[] q, long bit)
        {
            long t;
            long i;
            long c = ~(bit - 1);

            for (i = 0; i < 16; ++i)
            {
                t = c & (p[i] ^ q[i]);
                p[i] ^= t;
                q[i] ^= t;
            }
        }

        private static void Pack25519(byte[] output, long[] input)
        {
            long carry;
            long[] m = new long[16];
            long[] t = new long[16];

            for (int i = 0; i < 16; ++i)
            {
                t[i] = input[i];
            }

            Carry25519(t);
            Carry25519(t);
            Carry25519(t);

            for (int j = 0; j < 2; ++j)
            {
                m[0] = t[0] - 0xffed;

                for (int i = 1; i < 15; i++)
                {
                    m[i] = t[i] - 0xffff - ((m[i - 1] >> 16) & 1);
                    m[i - 1] &= 0xffff;
                }

                m[15] = t[15] - 0x7fff - ((m[14] >> 16) & 1);
                carry = (m[15] >> 16) & 1;
                m[14] &= 0xffff;
                Swap25519(t, m, 1 - carry);
            }

            for (int i = 0; i < 16; ++i)
            {
                output[2 * i] = (byte)(t[i] & 0xff);
                output[(2 * i) + 1] = (byte)(t[i] >> 8);
            }
        }

        private static void Fadd(long[] output, long[] a, long[] b)
        {
            for (int i = 0; i < 16; ++i)
            {
                output[i] = a[i] + b[i];
            }
        }

        private static void Fsub(long[] output, long[] a, long[] b)
        {
            for (int i = 0; i < 16; ++i)
            {
                output[i] = a[i] - b[i];
            }
        }

        private static void Fmul(long[] output, long[] a, long[] b)
        {
            long[] product = new long[31];

            for (int i = 0; i < 31; ++i)
            {
                product[i] = 0;
            }

            for (int i = 0; i < 16; ++i)
            {
                for (int j = 0; j < 16; ++j)
                {
                    product[i + j] += a[i] * b[j];
                }
            }

            for (int i = 0; i < 15; ++i)
            {
                product[i] += 38 * product[i + 16];
            }

            for (int i = 0; i < 16; ++i)
            {
                output[i] = product[i];
            }

            Carry25519(output);
            Carry25519(output);
        }

        private static void Finverse(long[] output, long[] input)
        {
            long[] c = new long[16];

            for (int i = 0; i < 16; ++i)
            {
                c[i] = input[i];
            }

            for (int i = 253; i >= 0; i--)
            {
                Fmul(c, c, c);

                if (i != 2 && i != 4)
                {
                    Fmul(c, c, input);
                }
            }
            for (int i = 0; i < 16; ++i)
            {
                output[i] = c[i];
            }
        }

        private void Scalarmult(byte[] output, byte[] scalar, byte[] point)
        {
            _121665[0] = 0xDB41;
            _121665[1] = 1;
            byte[] clamped = new byte[32];
            int bit;


            long[] a = new long[16];
            long[] b = new long[16];
            long[] c = new long[16];
            long[] d = new long[16];
            long[] e = new long[16];
            long[] f = new long[16];
            long[] x = new long[16];


            for (int i = 0; i < 32; ++i)
            {
                clamped[i] = scalar[i];
            }

            clamped[0] &= 0xf8;
            clamped[31] = (byte)((clamped[31] & 0x7f) | 0x40);
            Unpack25519(x, point);

            for (int i = 0; i < 16; ++i)
            {
                b[i] = x[i];
                d[i] = a[i] = c[i] = 0;
            }

            a[0] = d[0] = 1;


            for (int i = 254; i >= 0; --i)
            {

                bit = (clamped[i >> 3] >> (i & 7)) & 1;
                Swap25519(a, b, bit);
                Swap25519(c, d, bit);
                Fadd(e, a, c);
                Fsub(a, a, c);
                Fadd(c, b, d);
                Fsub(b, b, d);
                Fmul(d, e, e);
                Fmul(f, a, a);
                Fmul(a, c, a);
                Fmul(c, b, e);
                Fadd(e, a, c);
                Fsub(a, a, c);
                Fmul(b, a, a);
                Fsub(c, d, f);
                Fmul(a, c, _121665);
                Fadd(a, a, d);
                Fmul(c, c, a);
                Fmul(a, d, f);
                Fmul(d, b, x);
                Fmul(b, e, e);
                Swap25519(a, b, bit);
                Swap25519(c, d, bit);
            }
            Finverse(c, c);
            Fmul(a, a, c);
            Pack25519(output, a);
        }

        private void ScalarmultBase(byte[] output, byte[] scalar)
        {
            Scalarmult(output, scalar, _9);
        }

        public void GenerateKeypair(byte[] publicKey, byte[] privateKey)
        {
            GenerateRandomBytes(privateKey);
            ScalarmultBase(publicKey, privateKey);
        }

        public void GenerateSharedSecret(byte[] output, byte[] publicKey, byte[] privateKey)
        {
            Scalarmult(output, privateKey, publicKey);
        }
    }
}
