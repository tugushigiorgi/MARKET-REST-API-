using System;

namespace Business.Validation;

public   class MarketException :Exception
{
    public MarketException(string message) : base(message)
    {
    }

    // Custom constructor to initialize the exception message and inner exception
    public  MarketException(string message, Exception innerException) : base(message, innerException)
    {
    }
}