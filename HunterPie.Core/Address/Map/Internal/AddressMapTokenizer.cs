using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace HunterPie.Core.Address.Map.Internal
{
    internal class AddressMapTokenizer
    {

        public static string ConsumeUntilChar(StreamReader stream, char token) => 
            ConsumeUntilChars(stream, new HashSet<char>() { token });

        public static string ConsumeUntilChars(StreamReader stream, char[] tokens) =>
            ConsumeUntilChars(stream, tokens.ToHashSet());

        public static string ConsumeUntilChars(StreamReader stream, HashSet<char> tokens)
        {
            // Allocates 64 bytes in our stack for faster array manipulation
            int bufferMaxSize = 64;

            Span<char> buffer = stackalloc char[bufferMaxSize];
            StringBuilder builder = new StringBuilder();

            int charCount = 0;
            while (!stream.EndOfStream)
            {
                int character = stream.Read();
                int nextCharacter = stream.Peek();

                if (character != -1 && !tokens.Contains((char)character))
                    buffer[charCount % bufferMaxSize] = (char)character;

                // Transfer buffered characters to StringBuilder when the buffer is full or if we hit the desired last character
                if (charCount + 1 == bufferMaxSize || tokens.Contains((char)nextCharacter) || nextCharacter == -1)
                    builder.Append(buffer.Slice(0, charCount + 1));

                if (tokens.Contains((char)nextCharacter))
                {
                    break;
                }

                charCount = (charCount + 1) % bufferMaxSize;
            }

            return builder.ToString();
        }

        public static void ConsumeTokens(StreamReader stream, char[] tokens) => ConsumeTokens(stream, tokens.ToHashSet());

        public static void ConsumeTokens(StreamReader stream, HashSet<char> tokens)
        {
            while (!stream.EndOfStream)
            {
                int character = stream.Peek();

                if (tokens.Contains((char)character))
                    stream.Read();
                else
                    break;
            }
        }
    }
}
