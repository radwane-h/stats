module JSONAccess = 
      

    #r "System.Data"
    #r "System.Data.Linq"
    #r @"C:\Users\Radwane.Hassen\Documents\Visual Studio 2012\Projects\MPGTest\packages\FSharp.Data.2.3.2\lib\net40\FSharp.Data.dll"
    #r @"C:\Users\Radwane.Hassen\Documents\Visual Studio 2012\Projects\MPGTest\packages\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"

    open FSharp.Data
    open FSharp.Data.JsonExtensions
    open FSharp.Charting
//    open Microsoft.FSharp.Data.TypeProviders


//    let value = JsonValue.Load(@"C:\Users\Radwane.Hassen\Documents\Visual Studio 2012\Projects\MPGTest\MPGTest\myteam.json")
    type teams = JsonProvider<"C:\Users\Radwane.Hassen\Documents\Visual Studio 2012\Projects\MPGTest\MPGTest\myteam.json">
    let data = WorldBank.GetSample()
    
    data 
        |> List.map Chart.Line
    