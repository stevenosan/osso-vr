// See https://aka.ms/new-console-template for more information
using osso_vr_data_processor;
using osso_vr_models;
using Interaction = osso_vr_models.Interaction;

Console.WriteLine("Hello, World!");

var fileReader = new FileReader();
var interactions = fileReader.ReadFile<List<Interaction>>("c:\\data\\goalTimes.json");

var users = new List<User>();

var userFiles = Directory.GetFiles("c:\\data\\users");

foreach(var userFile in userFiles)
{
    var user = fileReader.ReadFile<User>(userFile);
    users.Add(user);
}

var runs = new List<Run>();

var runFiles = Directory.GetFiles("c:\\data\\runs");

foreach (var runFile in runFiles)
{
    var run = fileReader.ReadFile<Run>(runFile);
    runs.Add(run);
}

foreach(var run in runs)
{
    foreach(var interaction in run.Interactions)
    {
        interaction.InteractionDefinition = interactions.Single(i => i.Id == interaction.InteractionId);
    }
}

var interactionIds = interactions.Select(i => i.Id);
var runResults = runs.Select(r => new RunResult(r, interactionIds)).ToList();

var result = new Result(runResults);

Console.WriteLine(result);