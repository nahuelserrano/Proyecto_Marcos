
using Proyecto_camiones.DTOs;


namespace Proyecto_camiones.Presentacion.Models
{
    public class Chofer
    {
        public int Id;
        public string Nombre { get; set; }

        public Chofer(string nombre)
        {
            this.Nombre = nombre;
        }

        public ChoferDTO toDTO()
        {
            ChoferDTO choferDTO = new ChoferDTO(this.Nombre);
            return choferDTO;
        }
    }
}
