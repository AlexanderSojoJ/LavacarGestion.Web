namespace LavacarGestion.Entities
{
    public class Vehiculo
    {
        public int VehiculoId { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string Color { get; set; } = string.Empty;

        // Relación con cliente (un vehículo pertenece a UN solo cliente)
        public int ClienteId { get; set; }
        public virtual Cliente? Cliente { get; set; }

        // Relación con citas
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}