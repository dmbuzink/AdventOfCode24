namespace AdventOfCode24;

public class InputHelper
{
    const string FileLocation = @"F:\devfile\AdventOfCode24\AdventOfCode24\AdventOfCode24\Inputs";

    private static string GetPathForInputType(EInputType inputType)
    {
        return inputType switch
        {
            EInputType.Tests => "Tests",
            EInputType.Real => "Real"
        };
    }

    private static string ConstructPath(int projectNumber, EInputType inputType) =>
        Path.Combine(FileLocation, GetPathForInputType(inputType), $"Input_{projectNumber}.txt");

    public static IEnumerable<string> ReadLines(int projectNumber, EInputType inputType) =>
        File.ReadLines(ConstructPath(projectNumber, inputType));
}

public enum EInputType
{
    Tests = 1,
    Real = 2
}