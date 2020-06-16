
using Newtonsoft.Json;

public class MessageConverter {

    public static string Serialize<T>(T sourceObject)
    {
        return JsonConvert.SerializeObject(sourceObject);
    }
}