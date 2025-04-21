using System.Text.Json;
using System.Text.Json.Serialization;

namespace TextRPG
{
    /// <summary>
    /// Converter for Armor Class
    /// </summary>
    class ArmorConverter : JsonConverter<Armor>
    {
        public override Armor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var json = doc.RootElement;

            string? typeName = json.GetProperty("Type").GetString();
            var data = json.GetProperty("ArmorData").GetRawText();

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
            writer.WritePropertyName("ArmorData");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Converter for Weapon Class
    /// </summary>
    class WeaponConverter : JsonConverter<Weapon>
    {
        public override Weapon? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var json = doc.RootElement;

            string? typeName = json.GetProperty("Type").GetString();
            var data = json.GetProperty("WeaponData").GetRawText();

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
            writer.WritePropertyName("WeaponData");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Converter for Consumables Class
    /// </summary>
    class ConsumableConverter : JsonConverter<Consumables>
    {
        public override Consumables? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var json = doc.RootElement;

            string? typeName = json.GetProperty("Type").GetString();
            var data = json.GetProperty("ConsumableData").GetRawText();

            return typeName switch
            {
                "HealthPotion" => JsonSerializer.Deserialize<HealthPotion>(data, options)!,
                "MagicPotion" => JsonSerializer.Deserialize<MagicPotion>(data, options)!,
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
            writer.WritePropertyName("ConsumableData");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Converter for Character Class
    /// </summary>
    class CharacterConverter : JsonConverter<Character>
    {
        public override Character? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var json = doc.RootElement;

            string? typeName = json.GetProperty("Type").GetString();
            var data = json.GetProperty("CharacterData").GetRawText();

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
            writer.WritePropertyName("CharacterData");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Converter for Quest Class
    /// </summary>
    class QuestConverter : JsonConverter<Quest>
    {
        public override Quest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var json = doc.RootElement;

            string? typeName = json.GetProperty("Type").GetString();
            var data = json.GetProperty("QuestData").GetRawText();

            return typeName switch
            {
                "NormalQuest" => JsonSerializer.Deserialize<NormalQuest>(data, options)!,
                "SpecialQuest" => JsonSerializer.Deserialize<SpecialQuest>(data, options)!,
                _ => throw new NotSupportedException($"Unknown character type: {typeName}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Quest value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Type", value.GetType().Name);
            writer.WritePropertyName("QuestData");
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Converter for Armor Class Array
    /// </summary>
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