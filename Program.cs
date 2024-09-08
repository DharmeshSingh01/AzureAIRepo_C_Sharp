using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ComputerVisionQuickstart
{
    class Program
    {
        static string key = "COMPUTER_VISION_KEY";
        static string endpoint = "COMPUTER_VISION_ENDPOINT";
        static string ANALYZE_URL_IMAGE = "https://storage101ds.blob.core.windows.net/image/IMG_20231210_175625688.jpg";
        static void Main(string[] args)
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision ");
            ComputerVisionClient client = Authenticate(endpoint, key);
            AnalyzeImageUrl(client, ANALYZE_URL_IMAGE).Wait();
        }
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            { Endpoint = endpoint };
            return client;
        }
        public static async Task AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces,VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags,VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color,VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects

            };
            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();

            ImageAnalysis result = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);
            Console.WriteLine("Summary");
            foreach (var caption in result.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
            }
            Console.WriteLine();
            Console.WriteLine("Categories:");
            foreach (var category in result.Categories)
            {
                Console.WriteLine($"{category.Name} with confidance {category.Score}");
            }
            Console.WriteLine();
            Console.WriteLine("Tags");
            foreach (var tag in result.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            Console.WriteLine();
            Console.WriteLine("Objects:");
            Console.WriteLine();
            foreach (var obj in result.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence} at location {obj.Rectangle.X}, " +
                  $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H}");
            }
            Console.WriteLine("Faces:");
            foreach (var face in result.Faces)
            {
                Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, " +
                  $"{face.FaceRectangle.Left}, {face.FaceRectangle.Top + face.FaceRectangle.Width}, " +
                  $"{face.FaceRectangle.Top + face.FaceRectangle.Height}");
            }
            Console.WriteLine();
            Console.WriteLine("Brands:");
            foreach (var brand in result.Brands)
            {
                Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence} at location {brand.Rectangle.X}, " +
                  $"{brand.Rectangle.X + brand.Rectangle.W}, {brand.Rectangle.Y}, {brand.Rectangle.Y + brand.Rectangle.H}");
            }
            Console.ReadKey();
        }

    }
}