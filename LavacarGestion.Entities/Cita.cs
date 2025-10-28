namespace LavacarGestion.Entities
{
    public class Cita
    {
        public int CitaId { get; set; }
        public DateTime FechaCita { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public EstadoCita Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Relación con cliente
        public int ClienteId { get; set; }
        public virtual Cliente? Cliente { get; set; }

        // Relación con vehículo
        public int VehiculoId { get; set; }
        public virtual Vehiculo? Vehiculo { get; set; }
    }

    public enum EstadoCita
    {
        Ingresada = 1,
        Cancelada = 2,
        Concluida = 3
    }
}