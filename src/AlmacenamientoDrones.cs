using System;
using System.Collections.Generic;

namespace AlmacenamientoDronesRepartidores
{
    public class EstacionDeCargaDeDronesRepartidores
    {
        public interface IEstacionDeCarga
        {
            //La carga del dron es como el reinicio para poder mandarlo al Object Pool
            void CargarDron();
        }
    }

    //Aqui hago lo mismo que con las habitaciones, les ponemos estados a los drones para poder mandarlos al object pool 
    public enum EstadoDeCarga
    {
        Cargando,
        Cargado,
        Descargado
    }
    
    // Clase que representa cada dron individual y su capacidad de carga
    public class DronRepartidor : EstacionDeCargaDeDronesRepartidores.IEstacionDeCarga
    {
        public string IdDelDron { get; set; }
        public EstadoDeCarga EstadoDelDron { get; set; }

        public DronRepartidor(string id)
        {
            IdDelDron = id;
            EstadoDelDron = EstadoDeCarga.Descargado;
        }

        public void CargarDron()
        {
            Console.WriteLine($"{IdDelDron} esta siendo cargado.");
            EstadoDelDron = EstadoDeCarga.Cargando;

 
            System.Threading.Thread.Sleep(2000);
            EstadoDelDron = EstadoDeCarga.Cargado;
            Console.WriteLine($"{IdDelDron} esta completamente cargado.");
        }
    }

    // Object Pool: Gestiona un grupo de drones reutilizables
    // Implementación del patrón Object Pool para gestionar drones reutilizables
    public class AlmacenDrones
    {
        private Queue<DronRepartidor> listaDeEspera;
        private List<DronRepartidor> dronesEnUso;
        private static readonly int capacidadMaxima = 5;

        public AlmacenDrones()
        {
            listaDeEspera = new Queue<DronRepartidor>();
            dronesEnUso = new List<DronRepartidor>();

            // Inicializar el pool con drones
            for (int i = 1; i <= capacidadMaxima; i++)
            {
                var nuevoDron = new DronRepartidor($"Dron-{i}");
                listaDeEspera.Enqueue(nuevoDron);
            }
        }

        public DronRepartidor? SacarDeEspera()
        {
            if (listaDeEspera.Count == 0)
            {
                Console.WriteLine("No hay drones disponibles en este momento.");
                return null;
            }

            var dron = listaDeEspera.Dequeue();
            dronesEnUso.Add(dron);
            return dron;
        }

        public void PonerEnEspera(DronRepartidor dron)
        {
            if (dron != null && dronesEnUso.Contains(dron))
            {
                dronesEnUso.Remove(dron);
                dron.CargarDron(); // Recargar el dron antes de ponerlo disponible
                listaDeEspera.Enqueue(dron);
                Console.WriteLine($"{dron.IdDelDron} ha sido devuelto a la estacion.");
            }
        }

        public void MostrarEstadoPool()
        {
            Console.WriteLine($"\nEstado de la estacion de Drones:");
            Console.WriteLine($"Drones en espera: {listaDeEspera.Count}");
            Console.WriteLine($"Drones en uso: {dronesEnUso.Count}");
        }
    }
}