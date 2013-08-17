namespace MTO.Practices.Templating.Lexer
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class TokenListSerializer
    {
        /// <summary>
        /// Serializa um tokenlist
        /// </summary>
        /// <param name="tokenList">tokenlist a ser serializado</param>
        /// <returns>tokenlist serializado</returns>
        public static byte[] Serialize(this TokenList tokenList)
        {
            using (var memory = new MemoryStream(short.MaxValue))
            {
                // primeiro byte é a quantidade de tokens
                memory.Write(BitConverter.GetBytes(tokenList.Count), 0, 4);
                foreach (var token in tokenList)
                {
                    var contentBytes = token.Content.ToBytes();

                    // primeiros 2 bits indicam em quantos bytes está codificado o tamanho da string
                    // outros 6 bytes codificam o tipo de token
                    var sizeBytes = BitConverter.GetBytes(contentBytes.Length);
                    byte ident;
                    if (sizeBytes[1] == 0x00)
                    {
                        ident = 0x00;
                    }
                    else if (sizeBytes[2] == 0x00)
                    {
                        ident = 0x01;
                    }
                    else if (sizeBytes[3] == 0x00)
                    {
                        ident = 0x02;
                    }
                    else
                    {
                        ident = 0x03;
                    }

                    var firstByte = (byte)(ident << 5);
                    firstByte |= (byte)token.State;

                    memory.WriteByte(firstByte);
                    memory.Write(BitConverter.GetBytes(contentBytes.Length), 0, ident + 1);
                    memory.Write(contentBytes, 0, contentBytes.Length);
                }

                return memory.ToArray();
            }
        }

        /// <summary>
        /// Deserializa lista de tokens
        /// </summary>
        /// <param name="serializedData">dados serializados</param>
        /// <returns>objeto com tokens</returns>
        public static TokenList Deserialize(this byte[] serializedData)
        {
            var listSize = BitConverter.ToInt32(serializedData, 0);

            var tokens = new List<PToken>(listSize);
            var i = 4;
            while (i < serializedData.Length)
            {
                var firstByte = serializedData[i++];
                var state = firstByte & 0x3F;
                var mustRead = (firstByte >> 6) + 1;
                var sizeBytes = new byte[4];
                for (var j = 0; j < mustRead; j++)
                {
                    sizeBytes[j] = serializedData[i++];
                }

                var size = BitConverter.ToInt32(sizeBytes, 0);

                var content = System.Text.Encoding.UTF8.GetString(serializedData, i, size);
                i += size;

                tokens.Add(new PToken
                    {
                        Content = content,
                        State = state
                    });
            }

            return new TokenList(tokens);
        }
    }
}
