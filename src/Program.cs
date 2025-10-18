using System;
using CentralDronesRepartidores;

namespace FlotaDeDronesRepartidores 
{
    class Program 
    {

        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Entrega por Drones ===\n");

            var flotaDrones = new FlotaDrones();
            flotaDrones.GestionarDrones();

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}