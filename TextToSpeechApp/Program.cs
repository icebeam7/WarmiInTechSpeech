using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace TextToSpeechApp
{
    class Program
    {
        static string key = "";
        static string region = "";
        static string idiomaOrigen = "es-MX";
        static string vozOrigen = "es-MX-DaliaNeural";
        static string nombreArchivo1 = "mensaje.wav";
        static string nombreArchivo2 = "conversacion.wav";

        static SpeechConfig ConfigurarServicioVoz()
        {
            var servicio = SpeechConfig.FromSubscription(key, region);
            servicio.SpeechSynthesisLanguage = idiomaOrigen;
            servicio.SpeechSynthesisVoiceName = vozOrigen;
            return servicio;
        }

        static async Task Main()
        {
            Console.WriteLine("Escribe un mensaje");
            var mensaje = Console.ReadLine();

            var servicio = ConfigurarServicioVoz();
            await GenerarAudio(servicio, mensaje);
            Console.WriteLine($"Revisa el archivo {nombreArchivo1}");
            Console.WriteLine("Presiona una tecla para continuar");
            Console.ReadKey();

            Console.WriteLine("Ahora escucharás una conversación.");
            await EscucharyGenerarAudio(servicio);
            Console.WriteLine("Presiona una tecla para finalizar");
            Console.ReadKey();
        }

        static async Task GenerarAudio(SpeechConfig servicio, string mensaje)
        {
            using var audio = AudioConfig.FromWavFileOutput("audio.wav");
            using var sintetizador = new SpeechSynthesizer(servicio, audio);
            await sintetizador.SpeakTextAsync(mensaje);
        }

        static async Task EscucharyGenerarAudio(SpeechConfig servicio)
        {
            using var sintetizador = new SpeechSynthesizer(servicio);

            var ssml = File.ReadAllText("./dialogo.xml");
            var resultado = await sintetizador.SpeakSsmlAsync(ssml);
            
            using var stream = AudioDataStream.FromResult(resultado);
            await stream.SaveToWaveFileAsync(nombreArchivo2);
        }
    }
}
