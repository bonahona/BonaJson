using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BonaJsonException : Exception 
{
    public object Object { get; set; }
    public BonaJsonException() : base()
    {

    }

    public BonaJsonException(String message): base(message)
    {
    }

    public BonaJsonException(String message, object o) : base(message)
    {
        Object = null;
    }

    public BonaJsonException(String message, Exception innerException) : base(message, innerException)
    {
    }
}

