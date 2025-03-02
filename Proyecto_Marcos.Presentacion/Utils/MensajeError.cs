using System;

namespace Proyecto_Marcos.Presentacion.Utils
{
    public static class MensajeError
    {
        // Mensajes genéricos para validaciones comunes
        public static string idInvalido(string nombreEntidad) =>
            $"El ID de: {nombreEntidad} no puede ser menor a 0";

        public static string atributoRequerido(string nombreAtributo) =>
            $"El campo: {nombreAtributo} es obligatorio";

        public static string longitudInvalida(string nombreAtributo, int min, int max) =>
            $"La longitud de el campo: {nombreAtributo} debe estar entre {min} y {max} caracteres";
        
        public static string valorInvalido(string nombreAtributo) =>
            $"El valor de:{nombreAtributo} no es válido";

        public static string fechaInvalida(string nombreAtributo) =>
            $"La fecha de: {nombreAtributo} no es válida";
        public static string objetoNulo(string nombreEntidad) =>
            $"El objeto: {nombreEntidad} no puede ser nulo";
    }

}
