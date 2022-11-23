namespace Aoc2022Net.Days
{
    internal sealed class Day16 : Day
    {
        private sealed class PacketHeader
        {
            public int Version { get; set; }

            public int TypeId { get; set; }
        }

        private abstract class Packet
        {
            public PacketHeader Header { get; set; }
        }

        private sealed class LiteralValuePacket : Packet
        {
            public long Value { get; set; }
        }

        private sealed class OperatorPacket : Packet
        {
            public ICollection<Packet> SubPackets { get; set; }
        }

        public override object SolvePart1()
        {
            var packet = ParseInputPacket();
            return SumVersions(packet);
        }

        public override object SolvePart2()
        {
            var packet = ParseInputPacket();
            return Evaluate(packet);
        }

        private Packet ParseInputPacket()
        {
            var bits = string.Join(string.Empty, InputData
                .GetInputText()
                .Select(hexSymbol => Convert.ToString(Convert.ToInt32(hexSymbol.ToString(), 16), 2).PadLeft(4, '0')));
            
            var index = 0;
            return ParsePacket(bits, ref index);
        }

        private static int SumVersions(Packet packet)
        {
            var result = packet.Header.Version;
            if (packet is LiteralValuePacket)
                return result;

            var operatorPacket = (OperatorPacket)packet;
            return result + operatorPacket.SubPackets.Sum(SumVersions);
        }

        private static long Evaluate(Packet packet)
        {
            if (packet is LiteralValuePacket literalValuePacket)
                return literalValuePacket.Value;

            var operatorPacket = (OperatorPacket)packet;
            var operands = operatorPacket.SubPackets.Select(Evaluate);

            return operatorPacket.Header.TypeId switch
            {
                0 => operands.Sum(),
                1 => operands.Aggregate(1L, (result, operand) => result * operand),
                2 => operands.Min(),
                3 => operands.Max(),
                5 => operands.First() > operands.Last() ? 1 : 0,
                6 => operands.First() < operands.Last() ? 1 : 0,
                7 => operands.First() == operands.Last() ? 1 : 0
            };
        }

        private static Packet ParsePacket(string bits, ref int index)
        {
            var header = ParsePacketHeader(bits, ref index);

            Packet packet = header.TypeId switch
            {
                4 => ParseLiteralValuePacket(bits, ref index),
                _ => ParseOperatorPacket(bits, ref index)
            };

            packet.Header = header;
            return packet;
        }

        private static PacketHeader ParsePacketHeader(string bits, ref int index)
        {
            var version = Convert.ToInt32(bits.Substring(index, 3), 2);
            var typeId = Convert.ToInt32(bits.Substring(index + 3, 3), 2);
            index += 6;

            return new PacketHeader
            {
                Version = version,
                TypeId = typeId
            };
        }

        private static LiteralValuePacket ParseLiteralValuePacket(string bits, ref int index)
        {
            var valueString = string.Empty;

            while (true)
            {
                var group = bits.Substring(index, 5);
                index += group.Length;
                valueString += group[1..];

                if (group[0] == '0')
                    break;
            }

            return new LiteralValuePacket
            {
                Value = Convert.ToInt64(valueString, 2)
            };
        }

        private static OperatorPacket ParseOperatorPacket(string bits, ref int index)
        {
            var lengthTypeId = bits[index++];
            return new OperatorPacket
            {
                SubPackets = lengthTypeId switch
                {
                    '0' => ParseOperatorPacketByBitsCount(bits, ref index),
                    '1' => ParseOperatorPacketByPacketsCount(bits, ref index)
                }
            };
        }

        private static ICollection<Packet> ParseOperatorPacketByBitsCount(string bits, ref int index)
        {
            var result = new List<Packet>();

            var length = Convert.ToInt32(bits.Substring(index, 15), 2);
            index += 15;

            var startIndex = index;
            do
            {
                result.Add(ParsePacket(bits, ref index));
            }
            while (index - startIndex < length);

            return result;
        }

        private static ICollection<Packet> ParseOperatorPacketByPacketsCount(string bits, ref int index)
        {
            var result = new List<Packet>();

            var count = Convert.ToInt32(bits.Substring(index, 11), 2);
            index += 11;

            for (var i = 0; i < count; i++)
            {
                result.Add(ParsePacket(bits, ref index));
            }

            return result;
        }
    }
}
