using var client = new HttpClient();
using CancellationTokenSource cts = new CancellationTokenSource();
cts.CancelAfter(300);

var taskListJson = client.GetStringAsync(
    "https://cdn.jsdelivr.net/gh/Naramsim/ninjask@master/data/api/v2/pokemon/1/index.json", cts.Token);

Console.WriteLine(taskListJson.Result);
//After 300 ms sec, the Task should be cacelled