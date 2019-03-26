using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WebApiSample.Workflows
{
    public class TestWorkflow : IWorkflow<MyDataClass>
    {
        public string Id => "TestWorkflow";

        public int Version => 1;

        public void Build(IWorkflowBuilder<MyDataClass> builder)
        {
            builder
                .StartWith(context => ExecutionResult.Next())
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
                .Output((data, step) =>
                {
                    var jsonData = data.EventData as JObject;
                    if (jsonData != null)
                    {
                        var value = jsonData.ToObject<MyDataClass>();
                        step.Value1 = value?.Value1;
                    }
                })
                .Then(context => Console.WriteLine("workflow complete"));
        }
    }

    public class MyDataClass
    {
        public string Value1 { get; set; }
    }
}
