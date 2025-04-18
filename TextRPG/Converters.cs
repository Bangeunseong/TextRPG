using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TextRPG
{
    class ArmorConverter : JsonConverter<Armor>
    {
        public override Armor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var json = doc.RootElement;

            string? typeName = json.GetProperty("Type").GetString();
            var data = json.GetProperty("Data").GetRawText();

            return typeName switch
            {
                "Helmet" => JsonSerializer.Deserialize<Helmet>(data, options)!,
                "ChestArmor" => JsonSerializer.Deserialize<ChestArmor>(data, options)!,
                "LegArmor" => JsonSerializer.Deserialize<LegArmor>(data, options)!,
                "Gauntlet" => JsonSerializer.Deserialize<Gauntlet>(data, options)!,
                "FootArmor" => JsonSerializer.Deserialize<FootArmor>(data, options)!,
                _ => throw new NotSupportedException($"Unknown armor type: {typeName}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Armor value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Type", value.GetType().Name);
            writer.WritePropertyName("Data");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }

    class WeaponConverter : JsonConverter<Weapon>
    {
        public override Weapon? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var json = doc.RootElement;

            string? typeName = json.GetProperty("Type").GetString();
            var data = json.GetProperty("Data").GetRawText();

            return typeName switch
            {
                "Sword" => JsonSerializer.Deserialize<Sword>(data, options)!,
                "Bow" => JsonSerializer.Deserialize<Bow>(data, options)!,
                "Staff" => JsonSerializer.Deserialize<Staff>(data, options)!,
                _ => throw new NotSupportedException($"Unknown weapon type: {typeName}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Weapon value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Type", value.GetType().Name);
            writer.WritePropertyName("Data");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }

    class ConsumableConverter : JsonConverter<Consumables>
    {
        public override Consumables? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var json = doc.RootElement;

            string? typeName = json.GetProperty("Type").GetString();
            var data = json.GetProperty("Data").GetRawText();

            return typeName switch
            {
                "HealthPotion" => JsonSerializer.Deserialize<HealthPotion>(data, options)!,
                "AttackBuffPotion" => JsonSerializer.Deserialize<AttackBuffPotion>(data, options)!,
                "DefendBuffPotion" => JsonSerializer.Deserialize<DefendBuffPotion>(data, options)!,
                "AllBuffPotion" => JsonSerializer.Deserialize<AllBuffPotion>(data, options)!,
                _ => throw new NotSupportedException($"Unknown potion type: {typeName}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Consumables value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Type", value.GetType().Name);
            writer.WritePropertyName("Data");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }

    class CharacterConverter : JsonConverter<Character>
    {
        public override Character? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var json = doc.RootElement;

            string? typeName = json.GetProperty("Type").GetString();
            var data = json.GetProperty("Data").GetRawText();

            return typeName switch
            {
                "Warrior" => JsonSerializer.Deserialize<Warrior>(data, options)!,
                "Wizard" => JsonSerializer.Deserialize<Wizard>(data, options)!,
                "Archer" => JsonSerializer.Deserialize<Archer>(data, options)!,
                _ => throw new NotSupportedException($"Unknown character type: {typeName}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Character value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Type", value.GetType().Name);
            writer.WritePropertyName("Data");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }

    class EquippedArmorConverter : JsonConverter<Armor[]>
    {
        public override Armor[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var list = JsonSerializer.Deserialize<List<Armor>>(ref reader, options);
            return list?.ToArray() ?? Array.Empty<Armor>();
        }

        public override void Write(Utf8JsonWriter writer, Armor[] value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.ToList(), options);
        }
    }
}