using Giacom.CallDetails.CsvGenerator;

var rows = int.Parse(args[0]);

await CallDetailsGenerator.GenerateFile(new GeneratorRequest(rows));