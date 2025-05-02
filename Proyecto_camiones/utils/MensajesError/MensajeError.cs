﻿using System;

namespace Proyecto_camiones.Presentacion.Utils
{
    public static class MensajeError
    {
        // Mensajes genéricos para validaciones comunes
        public static string idInvalido(int id) =>
            $"El ID ingresado: {id} no puede ser menor a 0";

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
        
        public static string NoExisteId(string nombreEntidad, int id) =>
            $"No existe ningún {nombreEntidad} con el id: {id}";
        public static string ausenciaDeDatos(String nombreDatoAusente) =>
            $"El ID colocado: {nombreDatoAusente} no corresponde a ningun objeto";
        public static string PesoIncorrecto(float peso) =>
            $"El peso colocado: {peso} no es válido en el contexto";

        // Errores operacionales
        public static string errorOperacion(string operacion, string entidad) =>
            $"Ocurrió un error al {operacion} el/la {entidad}";

        // Errores de conexión
        public static string errorConexion() =>
            "No se pudo establecer la conexión con la base de datos";

        // Errores de validación comunes

        // Errores del CRUD
        public static string errorActualizacion(string entidad) =>
            $"No se pudo actualizar el/la {entidad}";

        public static string noExistenRegistros(string entidad, string filtro = null) =>
            filtro == null
                ? $"No existen registros de {entidad}"
                : $"No existen registros de {entidad} con: {filtro}";

        public static string ErrorCreacion(string entidad) =>
            $"No se pudo crear el objeto {entidad}";

    }

}
