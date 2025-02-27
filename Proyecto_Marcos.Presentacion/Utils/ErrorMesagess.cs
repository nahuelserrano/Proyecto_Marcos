using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransporteApp.Utils
{
    public static class ErrorMessages
    {
        // Mensajes genéricos para validaciones comunes
        public static string InvalidId(string entityName) =>
            $"El ID de {entityName} no puede ser menor a 0";

        public static string RequiredField(string fieldName) =>
            $"El campo {fieldName} es obligatorio";

        public static string InvalidLength(string fieldName, int min, int max) =>
            $"La longitud de {fieldName} debe estar entre {min} y {max} caracteres";

        // Puedes agregar categorías específicas también
        public static class Authentication
        {
            public static string InvalidCredentials =>
                "Las credenciales proporcionadas no son válidas";
        }
    }
}
