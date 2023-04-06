using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_POCofServices_Learnt
{
    internal class Services
    {
        private readonly AmazonSimpleNotificationServiceClient client; 
        public Services()
        {
            //Iam using my admin credentials not creating IAM users to assign any specific roles
            client= new AmazonSimpleNotificationServiceClient("AKIAUVNLRVBJFD4U5WOU", "VH944YZ+F57AbRlIrN+DicgwMnFqXV7917N7ux5+", Amazon.RegionEndpoint.USEast1);
        }
        
        private class Input 
        {
            public string Id { get; set; }
            public DateTime DateTime { get; set; }
        }

             Input data = new Input() 
             { 
             Id=new Random().GetHashCode().ToString(),
             DateTime=DateTime.Now
             };

        // send inputdata to SNS topic created in AWSconsole
         
        public async Task SendToSNSTopic(ILambdaContext context)
        {
            PublishRequest request = new PublishRequest(){ 
                Message =JsonConvert.SerializeObject(data),
                TopicArn= "arn:aws:sns:us-east-1:320871704658:InputSNS"
            };
            try
            {
                var response = await client.PublishAsync(request);
                context.Logger.LogInformation($"The response is {JsonConvert.SerializeObject(response)}");
                SQSService service = new SQSService();
                await service.ReceiveMessage(context);
            }
            catch(Exception ex)
            {
                context.Logger.LogInformation("Msg sending blocked"+ex.Message);
            }

        }
    }
}
