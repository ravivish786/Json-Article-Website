namespace Json_Article_Website.Extention
{
    public static class TimeEncodeExtention
    {
        public static string EncodeTime(this DateTime dateTime)
        {
            // convert to byte 
            var bytes = BitConverter.GetBytes(dateTime.Ticks);
            // convert to base64 string
            return Convert.ToBase64String(bytes);
        }
    }
}
