namespace Common.Files
{
    public static class FileExtensionHelper
    {
        public static string GetFileType(string sFileExtinsion)
        {
            
            switch ( sFileExtinsion)
            {
                case "doc":
                    return "Word Document";
                    break;
                case "xls":
                    return "Excel Document";
                    break;
                case "xlsx":
                    return "Excel Document";
                    break;
                case "docx":
                    return "Word Document";
                    break;
                case "pdf":
                    return "PDF Document";
                    break;
                default:
                    return "Unknown Document";
            }
        }
    }
}