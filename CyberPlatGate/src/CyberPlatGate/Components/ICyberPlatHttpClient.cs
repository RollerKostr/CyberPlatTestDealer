﻿using System.Threading.Tasks;
using CyberPlatGate.Contracts.Http;

namespace CyberPlatGate.Components
{
    public interface ICyberPlatHttpClient
    {
        Task<StatusResponse> Send(StatusRequest request);
        Task<PayResponse> Send(PayRequest request);
        Task<CheckResponse> Send(CheckRequest request);
    }
}