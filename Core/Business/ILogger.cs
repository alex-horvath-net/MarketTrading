using Microsoft.Extensions.Logging;

namespace Core.Business;

public interface ILogger<T> where T : class
{
    void Trace(string? template);
    void Trace<P0>(string template, P0 p0);
    void Trace<P0, P1>(string template, P0 p0, P1 p1);
    void Trace<P0, P1, P2>(string template, P0 p0, P1 p1, P2 p2);
    void Trace<P0, P1, P2, P3>(string template, P0 p0, P1 p1, P2 p2, P3 p3);
    void Trace<P0, P1, P2, P3, P4>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4);
    void Trace<P0, P1, P2, P3, P4, P5>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    void Trace<P0, P1, P2, P3, P4, P5, P6>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6);
    void Trace<P0, P1, P2, P3, P4, P5, P6, P7>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7);
    void Trace<P0, P1, P2, P3, P4, P5, P6, P7, P8>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8);
    void Trace<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9);


    void Debug(string? template);
    void Debug<P0>(string template, P0 p0);
    void Debug<P0, P1>(string template, P0 p0, P1 p1);
    void Debug<P0, P1, P2>(string template, P0 p0, P1 p1, P2 p2);
    void Debug<P0, P1, P2, P3>(string template, P0 p0, P1 p1, P2 p2, P3 p3);
    void Debug<P0, P1, P2, P3, P4>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4);
    void Debug<P0, P1, P2, P3, P4, P5>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    void Debug<P0, P1, P2, P3, P4, P5, P6>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6);
    void Debug<P0, P1, P2, P3, P4, P5, P6, P7>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7);
    void Debug<P0, P1, P2, P3, P4, P5, P6, P7, P8>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8);
    void Debug<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9);


    void Inform( string? template);
    void Inform<P0>( string template, P0 p0);
    void Inform<P0, P1>( string template, P0 p0, P1 p1);
    void Inform<P0, P1, P2>( string template, P0 p0, P1 p1, P2 p2);
    void Inform<P0, P1, P2, P3>( string template, P0 p0, P1 p1, P2 p2, P3 p3);
    void Inform<P0, P1, P2, P3, P4>( string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4);
    void Inform<P0, P1, P2, P3, P4, P5>( string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    void Inform<P0, P1, P2, P3, P4, P5, P6>( string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6);
    void Inform<P0, P1, P2, P3, P4, P5, P6, P7>( string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7);
    void Inform<P0, P1, P2, P3, P4, P5, P6, P7, P8>( string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8);
    void Inform<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>( string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9);


    void Warning(string? template);
    void Warning<P0>(string template, P0 p0);
    void Warning<P0, P1>(string template, P0 p0, P1 p1);
    void Warning<P0, P1, P2>(string template, P0 p0, P1 p1, P2 p2);
    void Warning<P0, P1, P2, P3>(string template, P0 p0, P1 p1, P2 p2, P3 p3);
    void Warning<P0, P1, P2, P3, P4>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4);
    void Warning<P0, P1, P2, P3, P4, P5>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    void Warning<P0, P1, P2, P3, P4, P5, P6>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6);
    void Warning<P0, P1, P2, P3, P4, P5, P6, P7>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7);
    void Warning<P0, P1, P2, P3, P4, P5, P6, P7, P8>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8);
    void Warning<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9);


    void Error(Exception exception, string? template);
    void Error<P0>(Exception exception, string template, P0 p0);
    void Error<P0, P1>(Exception exception, string template, P0 p0, P1 p1);
    void Error<P0, P1, P2>(Exception exception, string template, P0 p0, P1 p1, P2 p2);
    void Error<P0, P1, P2, P3>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3);
    void Error<P0, P1, P2, P3, P4>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4);
    void Error<P0, P1, P2, P3, P4, P5>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    void Error<P0, P1, P2, P3, P4, P5, P6>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6);
    void Error<P0, P1, P2, P3, P4, P5, P6, P7>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7);
    void Error<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8);
    void Error<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9);
}