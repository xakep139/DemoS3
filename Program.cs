using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace DemoS3
{
    class Program
    {
        public const string BucketName = "vstore-objects-standalone";

        static async Task Main(string[] args)
        {
            var credentials = new BasicAWSCredentials("accessKey", "secretKey");
            var config = new AmazonS3Config
            {
                ServiceURL = "http://rgw.n3.hw"
            };

            using (var client = new AmazonS3Client(credentials, config))
            {
                using (var myBrokenStream = new BrokenStream())
                {
                    await WritingAnObject(client, myBrokenStream);
                }
            }
        }

        private static async Task WritingAnObject(IAmazonS3 client, Stream inputStream)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    InputStream = inputStream,
                    BucketName = BucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    Key = "1/temp.txt"
                };

                await client.PutObjectAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
