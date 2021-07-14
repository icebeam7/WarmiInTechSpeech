using System;
using System.Threading.Tasks;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;

namespace SpeechToTextApp
{
    class Program
    {
        static string key = "";
        static string region = "";
        static string idiomaOrigen = "es-MX";
        static string idiomaTraduccion = "en";

        static SpeechTranslationConfig ConfigurarServicioVoz()
        {
            var servicio = SpeechTranslationConfig.FromSubscription(key, region);
            servicio.SpeechRecognitionLanguage = idiomaOrigen;
            servicio.AddTargetLanguage(idiomaTraduccion);
            return servicio;
        }

        async static Task EscucharMicrofono()
        {
            var servicio = ConfigurarServicioVoz();
            using var microfono = AudioConfig.FromDefaultMicrophoneInput();

            using var reconocedor = new TranslationRecognizer(servicio, microfono);

            Console.WriteLine("Escuchemos lo que tienes que decir:");
            var resultado = await reconocedor.RecognizeOnceAsync();
            Console.WriteLine($"HAS DICHO: {resultado.Text}");

            switch (resultado.Reason)
            {
                case ResultReason.TranslatedSpeech:
                    Console.WriteLine($"TRADUCCIÓN: {resultado.Translations[idiomaTraduccion]}");
                    break;
                case ResultReason.RecognizedSpeech:
                    Console.WriteLine("ERROR: El mensaje no pudo ser traducico");
                    break;
                default:
                    Console.WriteLine($"ERROR: {resultado.Reason}");
                    break;
            }

            Console.WriteLine("\nPresiona una tecla para salir");
            Console.ReadKey();
        }

        async static Task Main(string[] args)
        {
            await EscucharMicrofono();
        }
    }
}