﻿
namespace StockForThePeople.WebApiExecuter
{
    public interface IWebApiExecuter
    {
        Task InvokeDeleteAsync(string uri);
        Task<T> InvokeGetAsync<T>(string uri);
        Task<T> InvokePostAsync<T>(string uri, T obj);
        Task InvokePutAsync<T>(string uri, T obj);
    }
}