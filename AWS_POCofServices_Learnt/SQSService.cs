using Amazon.Lambda.Core;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_POCofServices_Learnt
{
    internal class SQSService
    {
        private readonly AmazonSQSClient client;
        public SQSService() 
        {
            client= new AmazonSQSClient("AKIAUVNLRVBJFD4U5WOU", "VH944YZ+F57AbRlIrN+DicgwMnFqXV7917N7ux5+", Amazon.RegionEndpoint.USEast1);
            
        }

        public async Task ReceiveMessage(ILambdaContext context)
        {
            int maxNumberOfMessages = 5;
            var visibilityTimeout = 5;
            var request = new ReceiveMessageRequest()
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/320871704658/MyQueue",
                MaxNumberOfMessages = maxNumberOfMessages,
                VisibilityTimeout = visibilityTimeout,
                WaitTimeSeconds = 5
            };
            var response = await client.ReceiveMessageAsync(request);
            if (response != null)
            {
                context.Logger.LogInformation($" This is response from SQS queue{JsonConvert.SerializeObject(response)}");
                S3Bucket s3 = new S3Bucket();
                await s3.creates3file(response, context);
            }
        }
    }
}
