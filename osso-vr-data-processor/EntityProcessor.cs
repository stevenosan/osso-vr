﻿using System.Text.Json;

namespace osso_vr_data_processor;
public static class EntityProcessor
{
    public static T ProcessFile<T>(string file)
    {
        var contents = File.ReadAllText(file);

        return JsonSerializer.Deserialize<T>(contents);
    }
}

public class FileReader
{
    public T ReadFile<T>(string file)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var fileContent = File.ReadAllText(file);
        return JsonSerializer.Deserialize<T>(fileContent, options);
    }
}
