﻿namespace WebApiHeinsohn.Models
{
    public class TareaEstado
    {
        public int Id { get; set; } 
        public int IdTarea { get; set; }
        public int IdEstado { get; set; }
        public string? NombreTarea { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime Fecha { get; set; }
    }
}
