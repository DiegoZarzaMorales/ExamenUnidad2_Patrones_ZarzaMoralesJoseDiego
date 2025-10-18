using System;
using AlmacenamientoDronesRepartidores;

namespace CentralDronesRepartidores
{
    // Implementación del patrón Singleton para la central de drones
    // Garantiza una única instancia que coordina todas las entregas
    public class CentralDronesRepartidores
    {
        // Instancia única de la clase (Singleton)
        private static CentralDronesRepartidores? instancia;
        // Objeto de bloqueo para garantizar thread-safety
        private static readonly object padlock = new object();
        // Referencia al pool de drones (Object Pool)
        private readonly AlmacenDrones almacenDrones;
        
        public string DireccionEntrega { get; private set; }
        public string TelefonoEntrega { get; private set; }
        public string NombreDelCliente { get; private set; }

        // Constructor privado donde se realiza la conexión con el Object Pool
        // Al ser un Singleton, se asegura que solo exista una instancia del AlmacenDrones (Object Pool)
        // Esta es la conexión principal entre los dos patrones de diseño
        private CentralDronesRepartidores()
        {
            // Aquí se crea la única instancia del Object Pool que existirá en el sistema
            almacenDrones = new AlmacenDrones();
            DireccionEntrega = string.Empty;
            TelefonoEntrega = string.Empty;
            NombreDelCliente = string.Empty;
        }

        // Método Singleton para obtener la única instancia de la central
        public static CentralDronesRepartidores ObtenerInstancia()
        {
            if (instancia == null)
            {
                lock (padlock)
                {
                    if (instancia == null)
                    {
                        instancia = new CentralDronesRepartidores();
                    }
                }
            }
            return instancia;
        }

        // Método público para iniciar una nueva entrega
        // Recibe los detalles del pedido y coordina la entrega
        public void PeticionDeEntrega(string direccion, string telefono, string cliente)
        {
            DireccionEntrega = direccion;
            TelefonoEntrega = telefono;
            NombreDelCliente = cliente;

            RealizarEntrega();
        }

        // Método privado que maneja la lógica de entrega
        // Gestiona la obtención del dron, simulación de entrega y devolución al pool
        private void RealizarEntrega()
        {
            Console.WriteLine("\n=== Nueva Entrega ===");
            MostrarInformacionEntrega();

            // Obtiene un dron del Object Pool
            var dron = almacenDrones.SacarDeEspera();
            if (dron != null)
            {
                Console.WriteLine($"\nAsignando {dron.IdDelDron} para la entrega...");
                Console.WriteLine("El dron está en camino al destino");
                System.Threading.Thread.Sleep(3000); // Simulamos el tiempo de entrega
                
                Console.WriteLine("\nEntrega completada!");
                almacenDrones.PonerEnEspera(dron); // Devuelve el dron al almacen/PoolDeObjetos
            }
            else
            {
                Console.WriteLine("No se pudo realizar la entrega: no hay drones disponibles.");
            }

            almacenDrones.MostrarEstadoPool();
        }

        public void MostrarInformacionEntrega()
        {
            Console.WriteLine("\nInformacion de Entrega:");
            Console.WriteLine($"Direccion: {DireccionEntrega}");
            Console.WriteLine($"Telefono: {TelefonoEntrega}");
            Console.WriteLine($"Nombre del Cliente: {NombreDelCliente}");
        }
    }

    public class FlotaDrones
    {
        public void GestionarDrones()
        {
            var central = CentralDronesRepartidores.ObtenerInstancia();

            // Simular varias entregas
            central.PeticionDeEntrega("Xicotencatl Leyva", "664 245 2535", "Diego Zarza");
            central.PeticionDeEntrega("Urbis 2", "664 345 123", "Kevin Alvarez");
            central.PeticionDeEntrega("Cacho", "664 123 456 ", "Jordhy Rojas");
            central.PeticionDeEntrega("Salvatierra", "664 903 923", "Marvin Morales");
            central.PeticionDeEntrega("Natura", "664 882 023", "Jose Morales");

            //Aqui ponemos otra entrega para ver que no generamos otro dron y usamos un dron listo de el almacen(la piscina de objetos)
            central.PeticionDeEntrega("Riberas del bosque", "664 185 0613", "Juan Zarza");
        }
    }
}