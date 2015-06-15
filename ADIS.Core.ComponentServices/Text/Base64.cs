using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.ComponentServices.Text
{
    // Copyright (c) 2003-2009, Jodd Team (jodd.org). All Rights Reserved.

    public static class Base64
    {
        private static char[] S_BASE64CHAR = {
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
    'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
    'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
    'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
    'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
    'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
    '8', '9', '+', '/'
  };
        public static int BUF_SIZE = 256;
        private static char S_BASE64PAD = '=';
        private static byte[] S_DECODETABLE = new byte[128];
        static Base64()
        {
            for (int i = 0; i < S_DECODETABLE.Length; i++)
            {
                S_DECODETABLE[i] = Byte.MaxValue;          // 127
            }
            for (int i = 0; i < S_BASE64CHAR.Length; i++)    // 0 to 63
            {
                S_DECODETABLE[S_BASE64CHAR[i]] = (byte)i;
            }
        }

        private static int decode0(char[] ibuf, byte[] obuf, int wp)
        {
            int outlen = 3;
            if (ibuf[3] == S_BASE64PAD)
            {
                outlen = 2;
            }
            if (ibuf[2] == S_BASE64PAD)
            {
                outlen = 1;
            }
            int b0 = S_DECODETABLE[ibuf[0]];
            int b1 = S_DECODETABLE[ibuf[1]];
            int b2 = S_DECODETABLE[ibuf[2]];
            int b3 = S_DECODETABLE[ibuf[3]];
            switch (outlen)
            {
                case 1:
                    obuf[wp] = (byte)(b0 << 2 & 0xfc | b1 >> 4 & 0x3);
                    return 1;
                case 2:
                    obuf[wp++] = (byte)(b0 << 2 & 0xfc | b1 >> 4 & 0x3);
                    obuf[wp] = (byte)(b1 << 4 & 0xf0 | b2 >> 2 & 0xf);
                    return 2;
                case 3:
                    obuf[wp++] = (byte)(b0 << 2 & 0xfc | b1 >> 4 & 0x3);
                    obuf[wp++] = (byte)(b1 << 4 & 0xf0 | b2 >> 2 & 0xf);
                    obuf[wp] = (byte)(b2 << 6 & 0xc0 | b3 & 0x3f);
                    return 3;
                default:
                    throw new Exception("Internal Error");
            }
        }

        private static int decode0(byte[] ibuf, byte[] obuf, int wp)
        {
            int outlen = 3;
            if (ibuf[3] == S_BASE64PAD)
            {
                outlen = 2;
            }
            if (ibuf[2] == S_BASE64PAD)
            {
                outlen = 1;
            }
            int b0 = S_DECODETABLE[ibuf[0]];
            int b1 = S_DECODETABLE[ibuf[1]];
            int b2 = S_DECODETABLE[ibuf[2]];
            int b3 = S_DECODETABLE[ibuf[3]];
            switch (outlen)
            {
                case 1:
                    obuf[wp] = (byte)(b0 << 2 & 0xfc | b1 >> 4 & 0x3);
                    return 1;
                case 2:
                    obuf[wp++] = (byte)(b0 << 2 & 0xfc | b1 >> 4 & 0x3);
                    obuf[wp] = (byte)(b1 << 4 & 0xf0 | b2 >> 2 & 0xf);
                    return 2;
                case 3:
                    obuf[wp++] = (byte)(b0 << 2 & 0xfc | b1 >> 4 & 0x3);
                    obuf[wp++] = (byte)(b1 << 4 & 0xf0 | b2 >> 2 & 0xf);
                    obuf[wp] = (byte)(b2 << 6 & 0xc0 | b3 & 0x3f);
                    return 3;
                default:
                    throw new Exception("Internal Error");
            }
        }

        /**
         * Decode the base64 data.
         * @param data The base64 encoded data to be decoded
         * @param off The offset within the encoded data at which to start decoding
         * @param len The length of data to decode
         * @return The decoded data
         */
        public static byte[] decode(char[] data, int off, int len)
        {
            char[] ibuf = new char[4];
            int ibufcount = 0;
            byte[] obuf = new byte[(len >> 2) * 3 + 3];
            int obufcount = 0;
            for (int i = off; i < off + len; i++)
            {
                char ch = data[i];
                if (ch == S_BASE64PAD || ch < S_DECODETABLE.Length && S_DECODETABLE[ch] != Byte.MaxValue)
                {
                    ibuf[ibufcount++] = ch;
                    if (ibufcount == ibuf.Length)
                    {
                        ibufcount = 0;
                        obufcount += decode0(ibuf, obuf, obufcount);
                    }
                }
            }
            if (obufcount == obuf.Length)
            {
                return obuf;
            }
            byte[] ret = new byte[obufcount];
            Array.Copy(obuf, 0, ret, 0, obufcount);
            return ret;
        }

        public static byte[] decode(byte[] data, int off, int len)
        {
            char[] ibuf = new char[4];
            int ibufcount = 0;
            byte[] obuf = new byte[(len >> 2) * 3 + 3];
            int obufcount = 0;
            for (int i = off; i < off + len; i++)
            {
                char ch = (char)data[i];
                if (ch == S_BASE64PAD || ch < S_DECODETABLE.Length && S_DECODETABLE[ch] != Byte.MaxValue)
                {
                    ibuf[ibufcount++] = ch;
                    if (ibufcount == ibuf.Length)
                    {
                        ibufcount = 0;
                        obufcount += decode0(ibuf, obuf, obufcount);
                    }
                }
            }
            if (obufcount == obuf.Length)
            {
                return obuf;
            }
            byte[] ret = new byte[obufcount];
            Array.Copy(obuf, 0, ret, 0, obufcount);
            return ret;
        }



        /**
         * Decode the base64 data.
         * @param data The base64 encoded data to be decoded
         * @return The decoded data
         */
        public static byte[] decode(String data)
        {
            int ibufcount = 0;
            int slen = data.Length;
            byte[] ibuf = new byte[slen < BUF_SIZE + 3 ? slen : BUF_SIZE + 3];
            byte[] obuf = new byte[(slen >> 2) * 3 + 3];
            int obufcount = 0;
            int blen;

            for (int i = 0; i < slen; i += BUF_SIZE)
            {
                // buffer may contain unprocessed characters from previous step
                if (i + BUF_SIZE <= slen)
                {
                    UTF8Encoding.UTF8.GetBytes(data, i, i + BUF_SIZE, ibuf, ibufcount);

                    blen = BUF_SIZE + ibufcount;
                }
                else
                {
                    UTF8Encoding.UTF8.GetBytes(data, i, slen, ibuf, ibufcount);
                    blen = slen - i + ibufcount;
                }

                for (int j = ibufcount; j < blen; j++)
                {
                    char ch = (char)ibuf[j];
                    if (ch == S_BASE64PAD || ch < S_DECODETABLE.Length && S_DECODETABLE[ch] != Byte.MaxValue)
                    {
                        ibuf[ibufcount++] = (byte)ch;
                        // as soon as we have 4 chars process them
                        if (ibufcount == 4)
                        {
                            ibufcount = 0;
                            obufcount += decode0(ibuf, obuf, obufcount);
                        }
                    }
                }
            }
            if (obufcount == obuf.Length)
            {
                return obuf;
            }
            byte[] ret = new byte[obufcount];
            Array.Copy(obuf, 0, ret, 0, obufcount);
            return ret;
        }

        /**
         * Decode the base64 data.
         * @param data The base64 encoded data to be decoded
         * @param off The offset within the encoded data at which to start decoding
         * @param len The length of data to decode
         * @param ostream The OutputStream to which the decoded data should be
         *                written
         */
        public static void decode(char[] data, int off, int len, Stream ostream)
        {
            char[] ibuf = new char[4];
            int ibufcount = 0;
            byte[] obuf = new byte[3];
            for (int i = off; i < off + len; i++)
            {
                char ch = data[i];
                if (ch == S_BASE64PAD || ch < S_DECODETABLE.Length && S_DECODETABLE[ch] != Byte.MaxValue)
                {
                    ibuf[ibufcount++] = ch;
                    if (ibufcount == ibuf.Length)
                    {
                        ibufcount = 0;
                        int obufcount = decode0(ibuf, obuf, 0);
                        ostream.Write(obuf, 0, obufcount);
                    }
                }
            }
        }

        /**
         * Decode the base64 data.
         * @param data The base64 encoded data to be decoded
         * @param ostream The OutputStream to which the decoded data should be
         *                written
         */
        public static void decode(String data, Stream ostream)
        {
            byte[] ibuf = new byte[BUF_SIZE + 4];
            byte[] obuf = new byte[3];
            int slen = data.Length;
            int blen;
            int ibufcount = 0;

            for (int i = 0; i < slen; i += BUF_SIZE)
            {
                // buffer may contain unprocessed characters from previous step
                if (i + BUF_SIZE <= slen)
                {
                    UTF8Encoding.UTF8.GetBytes(data, i, i + BUF_SIZE, ibuf, ibufcount);
                    blen = BUF_SIZE + ibufcount;
                }
                else
                {
                    UTF8Encoding.UTF8.GetBytes(data, i, slen, ibuf, ibufcount);
                    blen = slen - i + ibufcount;
                }

                for (int j = ibufcount; j < blen; j++)
                {
                    char ch = (char)ibuf[j];
                    if (ch == S_BASE64PAD || ch < S_DECODETABLE.Length && S_DECODETABLE[ch] != Byte.MaxValue)
                    {
                        ibuf[ibufcount++] = (byte)ch;

                        // as sson as we have 4 chars process them
                        if (ibufcount == 4)
                        {
                            ibufcount = 0;
                            int obufcount = decode0(ibuf, obuf, 0);
                            ostream.Write(obuf, 0, obufcount);
                        }
                    }
                }
            }
        }

        /**
         * Returns base64 representation of specified byte array.
         * @param data The data to be encoded
         * @return The base64 encoded data
         */
        public static String encode(byte[] data)
        {
            return encode(data, 0, data.Length);
        }

        public static String encode(String s)
        {
            return encode(UTF8Encoding.UTF8.GetBytes(s), 0, s.Length);
        }

        /**
         * Returns base64 representation of specified byte array.
         * @param data The data to be encoded
         * @param off The offset within the data at which to start encoding
         * @param len The length of the data to encode
         * @return The base64 encoded data
         */
        public static String encode(byte[] data, int off, int len)
        {
            if (len <= 0)
            {
                return "";
            }
            char[] outchars = new char[(len / 3 << 2) + 4];
            int rindex = off;
            int windex = 0;
            int rest = len;
            while (rest >= 3)
            {
                int i = ((data[rindex] & 0xff) << 16)
                  + ((data[rindex + 1] & 0xff) << 8)
                  + (data[rindex + 2] & 0xff);
                outchars[windex++] = S_BASE64CHAR[i >> 18];
                outchars[windex++] = S_BASE64CHAR[(i >> 12) & 0x3f];
                outchars[windex++] = S_BASE64CHAR[(i >> 6) & 0x3f];
                outchars[windex++] = S_BASE64CHAR[i & 0x3f];
                rindex += 3;
                rest -= 3;
            }
            if (rest == 1)
            {
                int i = data[rindex] & 0xff;
                outchars[windex++] = S_BASE64CHAR[i >> 2];
                outchars[windex++] = S_BASE64CHAR[(i << 4) & 0x3f];
                outchars[windex++] = S_BASE64PAD;
                outchars[windex++] = S_BASE64PAD;
            }
            else if (rest == 2)
            {
                int i = ((data[rindex] & 0xff) << 8) + (data[rindex + 1] & 0xff);
                outchars[windex++] = S_BASE64CHAR[i >> 10];
                outchars[windex++] = S_BASE64CHAR[(i >> 4) & 0x3f];
                outchars[windex++] = S_BASE64CHAR[(i << 2) & 0x3f];
                outchars[windex++] = S_BASE64PAD;
            }
            return new String(outchars, 0, windex);
        }

        /**
         * Outputs base64 representation of the specified byte array to a byte stream.
         * @param data The data to be encoded
         * @param off The offset within the data at which to start encoding
         * @param len The length of the data to encode
         * @param ostream The OutputStream to which the encoded data should be
         *                written
         */
        public static void encode(byte[] data, int off, int len, Stream ostream)
        {
            if (len <= 0)
            {
                return;
            }
            byte[] outchars = new byte[4];
            int rindex = off;
            int rest = len;
            while (rest >= 3)
            {
                int i = ((data[rindex] & 0xff) << 16)
                  + ((data[rindex + 1] & 0xff) << 8)
                  + (data[rindex + 2] & 0xff);
                outchars[0] = (byte)S_BASE64CHAR[i >> 18];
                outchars[1] = (byte)S_BASE64CHAR[(i >> 12) & 0x3f];
                outchars[2] = (byte)S_BASE64CHAR[(i >> 6) & 0x3f];
                outchars[3] = (byte)S_BASE64CHAR[i & 0x3f];
                ostream.Write(outchars, 0, 4);
                rindex += 3;
                rest -= 3;
            }
            if (rest == 1)
            {
                int i = data[rindex] & 0xff;
                outchars[0] = (byte)S_BASE64CHAR[i >> 2];
                outchars[1] = (byte)S_BASE64CHAR[(i << 4) & 0x3f];
                outchars[2] = (byte)S_BASE64PAD;
                outchars[3] = (byte)S_BASE64PAD;
                ostream.Write(outchars, 0, 4);
            }
            else if (rest == 2)
            {
                int i = ((data[rindex] & 0xff) << 8) + (data[rindex + 1] & 0xff);
                outchars[0] = (byte)S_BASE64CHAR[i >> 10];
                outchars[1] = (byte)S_BASE64CHAR[(i >> 4) & 0x3f];
                outchars[2] = (byte)S_BASE64CHAR[(i << 2) & 0x3f];
                outchars[3] = (byte)S_BASE64PAD;
                ostream.Write(outchars, 0, 4);
            }
        }


    }

}
