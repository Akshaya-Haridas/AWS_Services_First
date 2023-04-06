using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_POCofServices_Learnt
{
    internal class S3Bucket
    {
        private readonly AmazonS3Client client;
        public S3Bucket()
        {
            client= new AmazonS3Client("AKIAUVNLRVBJFD4U5WOU", "VH944YZ+F57AbRlIrN+DicgwMnFqXV7917N7ux5+", Amazon.RegionEndpoint.USEast1);
        }

        public async Task creates3file(ReceiveMessageResponse msg,ILambdaContext context)
        {
            foreach (var item in msg.Messages)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(item.Body);
                MemoryStream ms = new MemoryStream(bytes);
                var request = new PutObjectRequest
                {
                    BucketName = "akshayastoredata",
                    InputStream = ms,
                    Key = $"{new Random().GetHashCode()} Store_Data",
                    ContentType = "application/json"
                };
                var response= await client.PutObjectAsync(request);
                context.Logger.LogInformation(JsonConvert.SerializeObject(response));   
            }
        }
    }

}
