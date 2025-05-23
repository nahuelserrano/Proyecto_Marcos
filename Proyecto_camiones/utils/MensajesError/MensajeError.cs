using System;

namespace Proyecto_camiones.Presentacion.Utils
{
    public static class MensajeError
    {
        // 1. No se encontró X por id
        public static string EntidadNoEncontrada(string nombreEntidad, int id) =>
            $"No se encontró {nombreEntidad} con el id: {id}";

        // 2. El id no es válido
        public static string IdInvalido(int id) =>
            $"El id: {id} no es válido. Debe ser mayor que cero";

        // 3. No se pudo crear X
        public static string ErrorCreacion(string nombreEntidad) =>
            $"No se pudo crear {nombreEntidad}";

        // 4. No se pudo eliminar X
        public static string ErrorEliminacion(string nombreEntidad) =>
            $"No se pudo eliminar {nombreEntidad}";

        // 5. No se pudo actualizar X
        public static string ErrorActualizacion(string nombreEntidad) =>
            $"No se pudo actualizar {nombreEntidad}";

        // 6. Error en la base de datos
        public static string ErrorBaseDatos(string detalleError) =>
            $"Error en la base de datos: {detalleError}";

        // 7. X eliminado con éxito
        public static string EliminacionExitosa(string nombreEntidad) =>
            $"{nombreEntidad} eliminado con éxito";

        // 8. X actualizado con éxito  
        public static string ActualizacionExitosa(string nombreEntidad) =>
            $"{nombreEntidad} actualizado con éxito";

        // 9. X creado con el id: [id]
        public static string CreacionExitosa(string nombreEntidad, int id) =>
            $"{nombreEntidad} creado exitosamente con el id: {id}";

        public static string atributoRequerido(string nombreAtributo) =>
            $"El campo: {nombreAtributo} es obligatorio";

        public static string longitudInvalida(string nombreAtributo, int min, int max) =>
            $"La longitud de el campo: {nombreAtributo} debe estar entre {min} y {max} caracteres";
        
        public static string valorInvalido(string nombreAtributo) =>
            $"El valor de:{nombreAtributo} no es válido";

        public static string fechaInvalida(string nombreAtributo) =>
            $"La fecha de: {nombreAtributo} no es válida";
        public static string objetoNulo( string nombreEntidad) =>
            $"El objeto: {nombreEntidad} no puede ser nulo";

         public static string numeroNoValido(string atributo) =>
            $"El numero del campo: {atributo} no puede ser menor o igual que 0";
        
        public static string ausenciaDeDatos(String nombreDatoAusente) =>
            $"El ID colocado: {nombreDatoAusente} no corresponde a ningun objeto";
        public static string PesoIncorrecto(float peso) =>
            $"El peso colocado: {peso} no es válido en el contexto";

        // Errores de conexión
        public static string errorConexion() =>
            "No se pudo establecer la conexión con la base de datos";
    }
}
