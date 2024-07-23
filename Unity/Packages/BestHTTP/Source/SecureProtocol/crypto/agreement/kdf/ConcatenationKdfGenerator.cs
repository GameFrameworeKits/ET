#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Kdf
{
    /// <summary>Generator for Concatenation Key Derivation Function defined in NIST SP 800-56A, Sect 5.8.1</summary>
    public sealed class ConcatenationKdfGenerator
        :   IDerivationFunction
    {
        private readonly IDigest m_digest;
        private readonly int m_hLen;

        private byte[] m_buffer;

        /// <param name="digest">the digest to be used as the source of generated bytes</param>
        public ConcatenationKdfGenerator(IDigest digest)
        {
            m_digest = digest;
            m_hLen = digest.GetDigestSize();
        }

        public void Init(IDerivationParameters param)
        {
            if (!(param is KdfParameters kdfParameters))
                throw new ArgumentException("KDF parameters required for ConcatenationKdfGenerator");

            byte[] sharedSecret = kdfParameters.GetSharedSecret();
            byte[] otherInfo = kdfParameters.GetIV();

            m_buffer = new byte[4 + sharedSecret.Length + otherInfo.Length + m_hLen];
            sharedSecret.CopyTo(m_buffer, 4);
            otherInfo.CopyTo(m_buffer, 4 + sharedSecret.Length);
        }

        /// <summary>the underlying digest.</summary>
        public IDigest Digest => m_digest;

        /// <summary>Fill <c>len</c> bytes of the output buffer with bytes generated from the derivation function.
        /// </summary>
        public int GenerateBytes(byte[]	output, int outOff, int length)
        {
            Check.OutputLength(output, outOff, length, "output buffer too small");

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || _UNITY_2021_2_OR_NEWER_
            return GenerateBytes(output.AsSpan(outOff, length));
#else
            int hashPos = m_buffer.Length - m_hLen;
            uint counter = 1;

            m_digest.Reset();

            int end = outOff + length;
            int limit = end - m_hLen;

            while (outOff <= limit)
            {
                Pack.UInt32_To_BE(counter++, m_buffer, 0);

                m_digest.BlockUpdate(m_buffer, 0, hashPos);
                m_digest.DoFinal(output, outOff);

                outOff += m_hLen;
            }

            if (outOff < end)
            {
                Pack.UInt32_To_BE(counter, m_buffer, 0);

                m_digest.BlockUpdate(m_buffer, 0, hashPos);
                m_digest.DoFinal(m_buffer, hashPos);

                Array.Copy(m_buffer, hashPos, output, outOff, end - outOff);
            }

            return length;
#endif
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || _UNITY_2021_2_OR_NEWER_
        public int GenerateBytes(Span<byte> output)
        {
            int hashPos = m_buffer.Length - m_hLen;
            uint counter = 1;

            m_digest.Reset();

            int pos = 0, length = output.Length, limit = length - m_hLen;

            while (pos <= limit)
            {
                Pack.UInt32_To_BE(counter++, m_buffer.AsSpan());

                m_digest.BlockUpdate(m_buffer.AsSpan(0, hashPos));
                m_digest.DoFinal(output[pos..]);

                pos += m_hLen;
            }

            if (pos < length)
            {
                Pack.UInt32_To_BE(counter, m_buffer.AsSpan());

                m_digest.BlockUpdate(m_buffer.AsSpan(0, hashPos));
                m_digest.DoFinal(m_buffer.AsSpan(hashPos));
                m_buffer.AsSpan(hashPos, length - pos).CopyTo(output[pos..]);
            }

            return length;
        }
#endif
    }
}
#pragma warning restore
#endif
