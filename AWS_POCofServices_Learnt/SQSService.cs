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
            client= new AmazonSQSClient("", "", Amazon.RegionEndpoint.USEast1);
            
        }
        /// <summary>
        /// To collect message from SQS queue and then send to S3 bucket
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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
              // To call method creating file inside s3 bucket and write message in it
                S3Bucket s3 = new S3Bucket();
                await s3.Creates3file(response, context);
            }
        }
    }
}
