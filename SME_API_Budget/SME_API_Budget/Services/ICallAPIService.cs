using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;

namespace SME_API_Budget.Services
{
    public interface ICallAPIService
    {
        Task<ApiResponseReturnProjectModels> GetDataApiAsync_ReturnProject(MapiInformationModels param,string year, string projectcode);
        Task<ApiResponseReturnAreaModels> GetDataApiAsync_ReturnArea(MapiInformationModels apiModels, string? year, string? projectcode);
        Task<ApiResponseReturnPOutputModels> GetDataApiAsync_ReturnPOutput(MapiInformationModels apiModels, string? year, string? projectcode);
        Task<ApiResponseReturnPOutComeModels> GetDataApiAsync_ReturnPOutCome(MapiInformationModels apiModels, string? year, string? projectcode);
        Task<ApiResponseReturnPRiskModels> GetDataApiAsync_ReturnPRisk(MapiInformationModels apiModels, string? year, string? projectcode);

        Task<ApiResponseReturnPPayModels> GetDataApiAsync_ReturnPPay(MapiInformationModels apiModels, string? year, string? projectcode);
        Task<ApiResponseReturnPActivityModels> GetDataApiAsync_ReturnPActivity(MapiInformationModels apiModels, string? year, string? projectcode);
        Task<ApiResponseReturnPPlanBdgModels> GetDataApiAsync_ReturnPPlanBdg(MapiInformationModels apiModels, string? year, string? projectcode);

        Task<ApiRecPRResponseModel> RecDataApiAsync_RecPRs(MapiInformationModels apiModels, string? year, string? projectcode, string? ref_code, RecPRSubDataModel SendData);
        Task<ApiResponseReturnPExpectedModels> GetDataApiAsync_ReturnExpected(MapiInformationModels apiModels, string? year, string? projectcode);
    }
}
