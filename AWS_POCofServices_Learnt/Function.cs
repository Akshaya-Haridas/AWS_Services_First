using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWS_POCofServices_Learnt;

public class Function
{
    
    /// <summary>
    /// A lambda function which is triggered by api gateway sends a message to sns topic which then sends to sqs queue and then it stores in s3 bucket
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionHandler(ILambdaContext context)
    {
        //To send message to SNS topic when Lambda is triggered by apigateway
        var service = new SNSServices();
        await service.SendToSNSTopic(context);
    }
}
