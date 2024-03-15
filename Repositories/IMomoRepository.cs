using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IMomoRepository
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model, int reservationID);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
        Task<MomoCreateRefundResponseModel> CreateRefundAsync(OrderInfoModel model, int reservationID);
    }
}
