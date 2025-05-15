namespace HotelTools.Seguridad
{
    public class Result
    {
        public Object? obj { get; set; } = default!;
        public bool Success { get; } = default!;
        public string Error { get; } = string.Empty;


        private Result(Object obj, bool success, string error)
        {
            this.obj = obj;
            Success = success;
            Error = error;
        }

        /// <summary>
        /// Método estático de la clase Result que devuelve un resultado de operación exitosa.
        /// </summary> 
        /// <param name="obj">El objeto asociado al resultado exitoso.</param>
        /// <returns>Una instancia de Result que representa el resultado exitoso.</returns>
        public static Result Ok(Object? obj) => new Result(obj,true, null);

        /// <summary>
        /// Método estático de la clase Result que devuelve un resultado de operación exitosa.
        /// </summary> 
        /// <returns>Una instancia de Result que representa el resultado exitoso.</returns>
        public static Result Ok() => new Result(null, true, null);

        /// <summary>
        /// Método estático de la clase Result que devuelve un resultado de operación exitosa con un mensaje.
        /// </summary>   
        /// <param name="string">Mensaje de error.</param>
        public static Result Fail(string error) => new Result(null,false, error);

        /// <summary>
        /// Metod clase Result que devuelve un resultado de operación fallida.
        /// </summary>
        /// <returns></returns>
        public static Result Fail() => new Result(null, false, null);
    }
}
