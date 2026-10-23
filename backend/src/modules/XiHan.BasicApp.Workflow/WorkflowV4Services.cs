// xihan-migrate(V4-03): 在特性的 Configure 里调用 AddWorkflowV4Services()。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.Workflow.Application.AppServices;
using XiHan.BasicApp.Workflow.Application.EventHandlers;
using XiHan.BasicApp.Workflow.Application.QueryServices;
using XiHan.Framework.Messaging.Abstractions;

namespace XiHan.BasicApp.Workflow;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public static class WorkflowV4Services
{
    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    public static IServiceCollection AddWorkflowV4Services(this IServiceCollection services)
    {
        services.AddScoped<IWorkflowApprovalAppService, WorkflowApprovalAppService>();
        services.AddScoped<IWorkflowDefinitionAppService, WorkflowDefinitionAppService>();
        services.AddScoped<IWorkflowInstanceAppService, WorkflowInstanceAppService>();
        services.AddScoped<IWorkflowTodoAppService, WorkflowTodoAppService>();
        services.AddMessageHandler<WorkflowCarbonCopyRecordHandler>(WorkflowCarbonCopyRecordHandler.WorkflowUserTaskCreatedEventDataTopic);
        services.AddMessageHandler<WorkflowInstanceFaultedNotificationHandler>(WorkflowInstanceFaultedNotificationHandler.WorkflowInstanceFaultedEventDataTopic);
        services.AddMessageHandler<WorkflowUserTaskCreatedNotificationHandler>(WorkflowUserTaskCreatedNotificationHandler.WorkflowUserTaskCreatedEventDataTopic);
        services.AddMessageHandler<WorkflowUserTaskTransferredNotificationHandler>(WorkflowUserTaskTransferredNotificationHandler.WorkflowUserTaskTransferredEventDataTopic);
        services.AddScoped<IWorkflowApprovalQueryService, WorkflowApprovalQueryService>();
        services.AddScoped<IWorkflowDefinitionQueryService, WorkflowDefinitionQueryService>();
        services.AddScoped<IWorkflowDirectoryQueryService, WorkflowDirectoryQueryService>();
        services.AddScoped<IWorkflowInstanceQueryService, WorkflowInstanceQueryService>();
        services.AddScoped<IWorkflowTodoQueryService, WorkflowTodoQueryService>();
        return services;
    }
}
