namespace TransactionProcessor.Domain.Utilities;

public static class Common
{
    public static string GetNextReportFileName(string reportPath)
    {
        int counter = 1;
        string directory = Path.GetDirectoryName(reportPath) ?? "";
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(reportPath);
        string extension = Path.GetExtension(reportPath);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string newFileName = reportPath;
        while (File.Exists(newFileName))
        {
            newFileName = Path.Combine(directory, $"{fileNameWithoutExtension}{counter}{extension}");
            counter++;
        }

        return newFileName;
    }

}
