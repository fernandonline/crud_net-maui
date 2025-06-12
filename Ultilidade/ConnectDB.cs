namespace crud_maui.Ultilidade
{
    public static class ConnectDB
    {
        public static string DevolverRota(string nomeBaseDados)
        {
            string rotaBaseDados = string.Empty;
            if(DeviceInfo.Platform == DevicePlatform.Android)
            {
                rotaBaseDados = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                rotaBaseDados = Path.Combine(rotaBaseDados, nomeBaseDados);
            }
            return rotaBaseDados;
        }
    }
}
