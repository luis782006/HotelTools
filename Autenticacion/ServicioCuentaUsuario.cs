namespace HotelTools.Autenticacion
{
    public class ServicioCuentaUsuario 
    {
        private List<CuentaUsuario> usuarios;
        public ServicioCuentaUsuario()
        {
            usuarios = new List<CuentaUsuario>
            {
                new CuentaUsuario
                {
                    NombreUsuario = "admin",
                    Password = "admin",
                    Role = "Admin"
                },
                new CuentaUsuario
                {
                    NombreUsuario = "user",
                    Password = "user",
                    Role = "User"
                }
            };           
        }

        public CuentaUsuario? GetByUsername(string NombreUsuario)
        {
            return usuarios.FirstOrDefault(x => x.NombreUsuario == NombreUsuario);
        }
    }
}
