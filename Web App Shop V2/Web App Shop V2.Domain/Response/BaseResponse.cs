using System;
using Web_App_Shop_V2.Domain.Enum;

namespace Web_App_Shop_V2.Domain.Response;

public class BaseResponse<T> : IBaseResponse<T>
{
	public string description { get; set; }

	public StatusCode statusCode { get; set; }

	public T data { get; set; }
}

public interface IBaseResponse<T>
{
    T data { get; }

    StatusCode statusCode { get; }

	string description { get; }
}

